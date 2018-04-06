Imports Nethereum.Web3
Imports Nethereum.Hex.HexTypes
Imports System.Numerics
Imports Newtonsoft.Json
Imports System.Text.RegularExpressions

Public Class Main
    Dim failCount As Integer
    Dim contractBytecode As String
    Dim contractABI As String
    Dim gasPrice As BigInteger

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles btnDeploy.Click
        Dim proceed As Boolean = True
        Dim contractAddress As String
        Dim contract As Nethereum.Contracts.Contract
        Dim strTokenName As String, intDecimal As Integer, bIntTotalSupply As BigInteger, strSymbol As String, strNetwork As String
        Dim account As Accounts.Account
        Dim iweb3 As Web3

        ' prevalidate
        If String.IsNullOrEmpty(txtName.Text) Then
            MessageBox.Show("Token name cannot be empty.", "Token name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            proceed = False
        ElseIf (String.IsNullOrEmpty(txtDecimals.Text.ToString) OrElse txtDecimals.Text < 0) Then
            MessageBox.Show("Token decimal cannot be empty/must be 0 or more.", "Token decimals.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            proceed = False
        ElseIf (String.IsNullOrEmpty(txtTotalSupply.Text.ToString) OrElse txtTotalSupply.Text <= 0) Then
            MessageBox.Show("Total Supply cannot be empty/must be 1 or more.", "Total Supply.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            proceed = False
        ElseIf String.IsNullOrEmpty(txtSymbol.Text) Then
            MessageBox.Show("Token symbol cannot be empty.", "Token symbol.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
            proceed = False
        ElseIf cmbNetwork.SelectedItem.Key.ToString.Contains("https://mainnet.infura.io/") Then
            If MessageBox.Show("You have selected Mainnet for the tokens to be deployed." &
                               "Deploying tokens will cost real money - Would you like to proceed?",
                               "Deploying to Mainnet", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                proceed = True
            Else
                proceed = False
            End If
        End If

        If proceed = True Then
            'prompt message to guide users
            If MessageBox.Show(Me, "Deploying Ethereum ERC20 tokens requires some amount of ETH. Select an Ethereum JSON wallet with some balance in it." & vbCrLf & vbCrLf &
                                 "Would you like to proceed?", "Deploy ERC20?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                proceed = True
            Else
                proceed = False
            End If
        End If


        If proceed = True Then
            enableControls(False)

            ' get token details
            strTokenName = txtName.Text
            Integer.TryParse(txtDecimals.Text, intDecimal)
            BigInteger.TryParse(txtTotalSupply.Text, bIntTotalSupply)
            strSymbol = txtSymbol.Text
            strNetwork = cmbNetwork.SelectedItem.Key

            Try
                'get wallet, create account object
                Dim fileOpener As OpenFileDialog = New OpenFileDialog
                Dim strFileName As String

                fileOpener.Title = "Select Ethereum JSON wallet"
                fileOpener.RestoreDirectory = True
                fileOpener.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"

                If fileOpener.ShowDialog() = DialogResult.OK Then
                    'file selected - Allow to proceed
                    strFileName = fileOpener.FileName

                    'read file string
                    Dim wallet As String = My.Computer.FileSystem.ReadAllText(strFileName)
                    If Not String.IsNullOrEmpty(wallet) Then
                        'ask for password
                        Dim inputBox = New inputbox
                        Call inputBox.ShowDialog()
                        If String.IsNullOrEmpty(inputBox.GetInput) Then
                            MessageBox.Show("Please enter a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            proceed = False
                        Else
                            Try
                                AddMessage("Decrypting wallet, this may take a couple seconds...")
                                account = Accounts.Account.LoadFromKeyStore(wallet, inputBox.GetInput)
                                iweb3 = New Web3(account, strNetwork)
                            Catch ex As Exception
                                AddMessage("Unable to unlock wallet. Please check if password is correct. Error: " & ex.Message)
                                proceed = False
                            End Try
                        End If
                        inputBox = Nothing
                    Else
                        MessageBox.Show("Please select a valid Ethereum JSON wallet.", "Select a wallet", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    proceed = False
                    MessageBox.Show("Please select an Ethereum JSON wallet.", "Select a wallet", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                AddMessage("Error loading wallet: " & ex.Message)
                proceed = False
            End Try
            enableControls(True)
        End If

        If proceed Then
            enableControls(False)
            AddMessage("Wallet unlocked, publishing token contract.")
            AddMessage("This may take several minutes depending on the network you are connected to.")
            AddMessage("")
            Me.Cursor = Cursors.WaitCursor

            'insert compiled contracts and initialise web3
            Dim TokenAbi = contractABI
            Dim TokenbyteCode = contractBytecode

            Try
                Dim gas = New HexBigInteger(2500000)
                Dim hGasPrice = New HexBigInteger(gasPrice)
                Dim value = New HexBigInteger(0)
                AddMessage("Deploying token and waiting for transaction receipt...")
                Dim receiptHash = Await iweb3.Eth.DeployContract.SendRequestAndWaitForReceiptAsync(TokenAbi, TokenbyteCode, account.Address, gas, hGasPrice, value, , strTokenName, intDecimal, strSymbol, bIntTotalSupply)

                ' 1. Deploy token
                ' To check:
                '       > Token name
                '       > Decimals
                '       > Symbol
                '       > Total Supply
                '       > isTransferable (decided to ommit this for now)
                If receiptHash IsNot Nothing Then
                    contractAddress = receiptHash.ContractAddress
                    txtAddress.Text = contractAddress

                    AddMessage(txtName.Text & " deployed! Now running test cases to ensure all is fine...")
                    AddMessage("")
                    AddMessage("")
                    AddMessage("Starting token test cases - 4 test case.")

                    contract = iweb3.Eth.GetContract(TokenAbi, contractAddress)
                    AddMessage("Token deployed on address: " & contractAddress)
                    AddMessage("")

                    Dim tokenName As String = txtName.Text
                    Dim tokenDecimals As Integer = txtDecimals.Text
                    Dim tokenSymbol As String = txtSymbol.Text
                    Dim totalSupply As New HexBigInteger(BigInteger.Multiply(txtTotalSupply.Text, BigInteger.Pow(10, tokenDecimals)))
                    Dim isTransferable As Boolean = False

                    ' double checks done - In case connected network does not return a value for some reason
                    Dim rTokenName As String = Await contract.GetFunction("name").CallAsync(Of String)
                    If String.IsNullOrEmpty(rTokenName) Then rTokenName = Await contract.GetFunction("name").CallAsync(Of String)

                    Dim rTokenDecimals As String = Await contract.GetFunction("decimals").CallAsync(Of Integer)
                    If String.IsNullOrEmpty(rTokenDecimals) Then rTokenDecimals = Await contract.GetFunction("decimals").CallAsync(Of Integer)

                    Dim rTokenSymbol As String = Await contract.GetFunction("symbol").CallAsync(Of String)
                    If String.IsNullOrEmpty(rTokenSymbol) Then rTokenSymbol = Await contract.GetFunction("symbol").CallAsync(Of String)

                    Dim rTotalSupply As New HexBigInteger(Await contract.GetFunction("totalSupply").CallAsync(Of BigInteger))
                    If rTotalSupply Is Nothing Then rTotalSupply = New HexBigInteger(Await contract.GetFunction("totalSupply").CallAsync(Of BigInteger))
                    'Dim rIsTransferable As Boolean = Await contract.GetFunction("isTransferable").CallAsync(Of Boolean)



                    AddMessage("[" & Eval(tokenName, rTokenName) & "]" & vbTab & "Token name expected " & tokenName & ", returns " & rTokenName)
                    AddMessage("[" & Eval(tokenDecimals, Integer.Parse(rTokenDecimals)) & "]" & vbTab & "Token decimals expected " & tokenDecimals & ", returns " & rTokenDecimals)
                    AddMessage("[" & Eval(tokenSymbol, rTokenSymbol) & "]" & vbTab & "Token symbol expected " & tokenSymbol & ", returns " & rTokenSymbol)
                    AddMessage("[" & Eval(totalSupply.Value.ToString, rTotalSupply.Value.ToString) & "]" & vbTab & "Token supply expected " &
                           BigInteger.Divide(totalSupply.Value, BigInteger.Pow(10, tokenDecimals)).ToString &
                           ", returns : " &
                           BigInteger.Divide(rTotalSupply.Value, BigInteger.Pow(10, rTokenDecimals)).ToString)
                    'AddMessage("[" & Eval(isTransferable, rIsTransferable) & "]" & vbTab & "Token transferable expected " & isTransferable & ", returns " & rIsTransferable)

                    AddMessage("")
                    AddMessage("Token Test Case Passed: 4/" & (4 - failCount))
                End If
            Catch ex As Exception
                AddMessage("Error during token deployment. Error: " & ex.Message)
            End Try


            enableControls(True)
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Public Sub enableControls(ByVal bool As Boolean)
        txtName.Enabled = bool
        txtSymbol.Enabled = bool
        txtTotalSupply.Enabled = bool
        cmbNetwork.Enabled = bool
        txtDecimals.Enabled = bool
        btnDeploy.Enabled = bool
    End Sub

    Public Sub AddMessage(ByVal message As String)
        lsConsole.Items.Add(message)
        lsConsole.TopIndex = lsConsole.Items.Count - 1
        Application.DoEvents()
    End Sub

    Public Function RemovePrefix(ByVal input As String) As String
        If input.Substring(0, 2) = "0x" Then
            input = input.Substring(2)
        End If
        Return input
    End Function

    Public Function Eval(param1 As Object, param2 As Object, Optional Reset As Boolean = False) As String
        If Reset Then
            failCount = 0
        End If
        If param1.Equals(param2) Then
            Return "PASS"
        Else
            failCount += 1
            Return "FAIL"
        End If
    End Function

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim tooltip As New ToolTip()
        Dim setting As Settings
        tooltip.ShowAlways = True

        'token name
        tooltip.SetToolTip(lblName, "Specify your token name (E.g. ""My Token"")")
        tooltip.SetToolTip(txtName, "Specify your token name (E.g. ""My Token"")")

        'token symbol
        tooltip.SetToolTip(lblSymbol, "Specify your token symbol (E.g. ""MTKN""). 3 to 5 characters are ideal.")
        tooltip.SetToolTip(txtSymbol, "Specify your token symbol (E.g. ""MTKN""). 3 to 5 characters are ideal.")

        'token decimal
        tooltip.SetToolTip(lblDecimals, "Specify your token decimal (E.g. 18).")
        tooltip.SetToolTip(txtDecimals, "Specify your token decimal (E.g. 18).")

        'token totalSupply
        tooltip.SetToolTip(lblTotalSupply, "Specify your token total supply (E.g. 1,000,000).")
        tooltip.SetToolTip(txtTotalSupply, "Specify your token total supply (E.g. 1,000,000).")

        'select network to deploy your token to
        tooltip.SetToolTip(lblNetwork, "Select network to deploy token to.")
        tooltip.SetToolTip(cmbNetwork, "Select network to deploy token to.")

        If System.IO.File.Exists("settings.json") Then
            'read from JSON file
            Dim json As String = My.Computer.FileSystem.ReadAllText("settings.json")
            setting = New Settings
            setting = JsonConvert.DeserializeObject(Of Settings)(json)
        Else
            setting = New Settings
            setting.Network.Add("https://rinkeby.infura.io/", "Rinkeby (Testnet)")
            setting.Network.Add("https://ropsten.infura.io/", "Ropsten (Testnet)")
            setting.Network.Add("https://kovan.infura.io/", "Kovan (Testnet)")
            setting.Network.Add("http://localhost:8545/", "Localhost 8545")
            setting.Network.Add("https://mainnet.infura.io/", "Mainnet (Livenet)")
            setting.ContractByteCode = "0x6060604052341561000f57600080fd5b60405161086a38038061086a83398101604052808051820191906020018051919060200180518201919060200180519150600390508480516100559291602001906100f8565b506004805460ff191660ff851617905560058280516100789291602001906100f8565b5060045460ff16600a0a8102600681905560028054600160a060020a03191633600160a060020a0316908117909155600081815260208190526040808220849055919290917fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef91905190815260200160405180910390a350505050610193565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061013957805160ff1916838001178555610166565b82800160010185558215610166579182015b8281111561016657825182559160200191906001019061014b565b50610172929150610176565b5090565b61019091905b80821115610172576000815560010161017c565b90565b6106c8806101a26000396000f3006060604052600436106100c45763ffffffff7c010000000000000000000000000000000000000000000000000000000060003504166306fdde0381146100c9578063095ea7b31461015357806318160ddd1461018957806323b872dd146101ae578063313ce567146101d657806337fb7e21146101ff57806370a082311461022e578063879f30ad1461024d5780638da5cb5b1461026557806395d89b4114610278578063a9059cbb1461028b578063dd62ed3e146102ad578063f2fde38b146102d2575b600080fd5b34156100d457600080fd5b6100dc6102f1565b60405160208082528190810183818151815260200191508051906020019080838360005b83811015610118578082015183820152602001610100565b50505050905090810190601f1680156101455780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b341561015e57600080fd5b610175600160a060020a036004351660243561038f565b604051901515815260200160405180910390f35b341561019457600080fd5b61019c610435565b60405190815260200160405180910390f35b34156101b957600080fd5b610175600160a060020a0360043581169060243516604435610478565b34156101e157600080fd5b6101e961048d565b60405160ff909116815260200160405180910390f35b341561020a57600080fd5b610212610496565b604051600160a060020a03909116815260200160405180910390f35b341561023957600080fd5b61019c600160a060020a03600435166104a5565b341561025857600080fd5b6102636004356104c0565b005b341561027057600080fd5b610212610588565b341561028357600080fd5b6100dc610597565b341561029657600080fd5b610175600160a060020a0360043516602435610602565b34156102b857600080fd5b61019c600160a060020a0360043581169060243516610615565b34156102dd57600080fd5b610263600160a060020a0360043516610640565b60038054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156103875780601f1061035c57610100808354040283529160200191610387565b820191906000526020600020905b81548152906001019060200180831161036a57829003601f168201915b505050505081565b60008115806103c15750600160a060020a03338116600090815260016020908152604080832093871683529290522054155b15156103cc57600080fd5b600160a060020a03338116600081815260016020908152604080832094881680845294909152908190208590557f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b9259085905190815260200160405180910390a350600192915050565b600080805260208190527fad3228b676f7d3cd4284a5443f17f1962b36e491b30a40b2405849e597ba5fb5546006546104739163ffffffff61068a16565b905090565b6000610485848484610478565b949350505050565b60045460ff1681565b600754600160a060020a031681565b600160a060020a031660009081526020819052604090205490565b60008082116104ce57600080fd5b600160a060020a0333166000908152602081905260409020548211156104f357600080fd5b5033600160a060020a038116600090815260208190526040902054610518908361068a565b600160a060020a038216600090815260208190526040902055600654610544908363ffffffff61068a16565b600655600160a060020a0381167fcc16f5dbb4873280815c1ee09dbd06736cffcc184412cf7a71a0fdb75d397ca58360405190815260200160405180910390a25050565b600254600160a060020a031681565b60058054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156103875780601f1061035c57610100808354040283529160200191610387565b600061060e8383610602565b9392505050565b600160a060020a03918216600090815260016020908152604080832093909416825291909152205490565b60025433600160a060020a0390811691161461065b57600080fd5b6002805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0392909216919091179055565b60008282111561069657fe5b509003905600a165627a7a723058204b8096bab5d2053ebfcc7416b6f78979cf56277c1791c848cec7320f7cb694530029"
            setting.ContractABI = "[{""constant"":true,""inputs"":[],""name"":""name"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_spender"",""type"":""address""},{""name"":""_value"",""type"":""uint256""}],""name"":""approve"",""outputs"":[{""name"":""success"",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""totalSupply"",""outputs"":[{""name"":"""",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_from"",""type"":""address""},{""name"":""_to"",""type"":""address""},{""name"":""_value"",""type"":""uint256""}],""name"":""transferFrom"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""decimals"",""outputs"":[{""name"":"""",""type"":""uint8""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""distributionAddress"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""}],""name"":""balanceOf"",""outputs"":[{""name"":""balance"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_value"",""type"":""uint256""}],""name"":""burnSent"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""owner"",""outputs"":[{""name"":"""",""type"":""address""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":true,""inputs"":[],""name"":""symbol"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""_to"",""type"":""address""},{""name"":""_value"",""type"":""uint256""}],""name"":""transfer"",""outputs"":[{""name"":"""",""type"":""bool""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""constant"":true,""inputs"":[{""name"":""_owner"",""type"":""address""},{""name"":""_spender"",""type"":""address""}],""name"":""allowance"",""outputs"":[{""name"":""remaining"",""type"":""uint256""}],""payable"":false,""stateMutability"":""view"",""type"":""function""},{""constant"":false,""inputs"":[{""name"":""newOwner"",""type"":""address""}],""name"":""transferOwnership"",""outputs"":[],""payable"":false,""stateMutability"":""nonpayable"",""type"":""function""},{""inputs"":[{""name"":""_name"",""type"":""string""},{""name"":""_decimals"",""type"":""uint8""},{""name"":""_sym"",""type"":""string""},{""name"":""_totalSupply"",""type"":""uint256""}],""payable"":false,""stateMutability"":""nonpayable"",""type"":""constructor""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""burner"",""type"":""address""},{""indexed"":false,""name"":""value"",""type"":""uint256""}],""name"":""Burn"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""_from"",""type"":""address""},{""indexed"":true,""name"":""_to"",""type"":""address""},{""indexed"":false,""name"":""_value"",""type"":""uint256""}],""name"":""Transfer"",""type"":""event""},{""anonymous"":false,""inputs"":[{""indexed"":true,""name"":""_owner"",""type"":""address""},{""indexed"":true,""name"":""_spender"",""type"":""address""},{""indexed"":false,""name"":""_value"",""type"":""uint256""}],""name"":""Approval"",""type"":""event""}]"
            setting.GasPriceInGwei = 10000000000
            Dim json As String = JsonConvert.SerializeObject(setting, Formatting.Indented)
            Dim objWriter As New System.IO.StreamWriter("settings.json")
            objWriter.Write(json)
            objWriter.Close()
        End If


        cmbNetwork.DataSource = New BindingSource(setting.Network, Nothing)
        cmbNetwork.DisplayMember = "Value"
        cmbNetwork.ValueMember = "Key"
        cmbNetwork.SelectedIndex = 0

        contractBytecode = setting.ContractByteCode
        contractABI = setting.ContractABI

        gasPrice = setting.GasPriceInGwei
    End Sub

    Private Sub txtNum_KeyPress(sender As Object, e As System.Windows.Forms.KeyPressEventArgs) Handles txtDecimals.KeyPress, txtTotalSupply.KeyPress
        If Not Char.IsNumber(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtNum_TextChanged(sender As Object, e As EventArgs) Handles txtDecimals.TextChanged, txtTotalSupply.TextChanged
        Dim digitsOnly As Regex = New Regex("[^\d]")
        sender.Text = digitsOnly.Replace(sender.Text, "")
    End Sub
End Class

Public Class Settings
    Public Property Network() As New Dictionary(Of String, String)
    Public Property ContractByteCode As String
    Public Property ContractABI As String
    Public Property GasPriceInGwei As BigInteger
End Class