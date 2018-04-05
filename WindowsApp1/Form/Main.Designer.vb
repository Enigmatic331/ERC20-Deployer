<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.btnDeploy = New System.Windows.Forms.Button()
        Me.lsConsole = New System.Windows.Forms.ListBox()
        Me.cmbNetwork = New System.Windows.Forms.ComboBox()
        Me.lblNetwork = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblDecimals = New System.Windows.Forms.Label()
        Me.lblTotalSupply = New System.Windows.Forms.Label()
        Me.lblSymbol = New System.Windows.Forms.Label()
        Me.txtTotalSupply = New System.Windows.Forms.TextBox()
        Me.txtDecimals = New System.Windows.Forms.TextBox()
        Me.txtSymbol = New System.Windows.Forms.TextBox()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAddress = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnDeploy
        '
        Me.btnDeploy.Location = New System.Drawing.Point(12, 307)
        Me.btnDeploy.Name = "btnDeploy"
        Me.btnDeploy.Size = New System.Drawing.Size(236, 50)
        Me.btnDeploy.TabIndex = 11
        Me.btnDeploy.Text = "Deploy"
        Me.btnDeploy.UseVisualStyleBackColor = True
        '
        'lsConsole
        '
        Me.lsConsole.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lsConsole.FormattingEnabled = True
        Me.lsConsole.HorizontalScrollbar = True
        Me.lsConsole.ItemHeight = 25
        Me.lsConsole.Location = New System.Drawing.Point(12, 376)
        Me.lsConsole.Name = "lsConsole"
        Me.lsConsole.Size = New System.Drawing.Size(1085, 504)
        Me.lsConsole.TabIndex = 12
        '
        'cmbNetwork
        '
        Me.cmbNetwork.FormattingEnabled = True
        Me.cmbNetwork.Location = New System.Drawing.Point(211, 211)
        Me.cmbNetwork.Name = "cmbNetwork"
        Me.cmbNetwork.Size = New System.Drawing.Size(247, 33)
        Me.cmbNetwork.TabIndex = 10
        '
        'lblNetwork
        '
        Me.lblNetwork.AutoSize = True
        Me.lblNetwork.Location = New System.Drawing.Point(89, 214)
        Me.lblNetwork.Name = "lblNetwork"
        Me.lblNetwork.Size = New System.Drawing.Size(116, 25)
        Me.lblNetwork.TabIndex = 9
        Me.lblNetwork.Text = "Deploy To:"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(65, 46)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(140, 25)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Token Name:"
        '
        'lblDecimals
        '
        Me.lblDecimals.AutoSize = True
        Me.lblDecimals.Location = New System.Drawing.Point(99, 130)
        Me.lblDecimals.Name = "lblDecimals"
        Me.lblDecimals.Size = New System.Drawing.Size(106, 25)
        Me.lblDecimals.TabIndex = 5
        Me.lblDecimals.Text = "Decimals:"
        '
        'lblTotalSupply
        '
        Me.lblTotalSupply.AutoSize = True
        Me.lblTotalSupply.Location = New System.Drawing.Point(67, 172)
        Me.lblTotalSupply.Name = "lblTotalSupply"
        Me.lblTotalSupply.Size = New System.Drawing.Size(138, 25)
        Me.lblTotalSupply.TabIndex = 7
        Me.lblTotalSupply.Text = "Total Supply:"
        '
        'lblSymbol
        '
        Me.lblSymbol.AutoSize = True
        Me.lblSymbol.Location = New System.Drawing.Point(50, 88)
        Me.lblSymbol.Name = "lblSymbol"
        Me.lblSymbol.Size = New System.Drawing.Size(155, 25)
        Me.lblSymbol.TabIndex = 3
        Me.lblSymbol.Text = "Token Symbol:"
        '
        'txtTotalSupply
        '
        Me.txtTotalSupply.Location = New System.Drawing.Point(211, 169)
        Me.txtTotalSupply.Name = "txtTotalSupply"
        Me.txtTotalSupply.Size = New System.Drawing.Size(247, 31)
        Me.txtTotalSupply.TabIndex = 8
        '
        'txtDecimals
        '
        Me.txtDecimals.Location = New System.Drawing.Point(211, 127)
        Me.txtDecimals.Name = "txtDecimals"
        Me.txtDecimals.Size = New System.Drawing.Size(247, 31)
        Me.txtDecimals.TabIndex = 6
        '
        'txtSymbol
        '
        Me.txtSymbol.Location = New System.Drawing.Point(211, 85)
        Me.txtSymbol.Name = "txtSymbol"
        Me.txtSymbol.Size = New System.Drawing.Size(247, 31)
        Me.txtSymbol.TabIndex = 4
        '
        'txtName
        '
        Me.txtName.Location = New System.Drawing.Point(211, 43)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(247, 31)
        Me.txtName.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(375, 320)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(260, 25)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Deployed Token Address:"
        '
        'txtAddress
        '
        Me.txtAddress.Location = New System.Drawing.Point(641, 317)
        Me.txtAddress.Name = "txtAddress"
        Me.txtAddress.ReadOnly = True
        Me.txtAddress.Size = New System.Drawing.Size(456, 31)
        Me.txtAddress.TabIndex = 14
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1119, 902)
        Me.Controls.Add(Me.txtAddress)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.txtSymbol)
        Me.Controls.Add(Me.txtDecimals)
        Me.Controls.Add(Me.txtTotalSupply)
        Me.Controls.Add(Me.lblSymbol)
        Me.Controls.Add(Me.lblTotalSupply)
        Me.Controls.Add(Me.lblDecimals)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblNetwork)
        Me.Controls.Add(Me.cmbNetwork)
        Me.Controls.Add(Me.lsConsole)
        Me.Controls.Add(Me.btnDeploy)
        Me.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Main"
        Me.Text = "Easy ERC20 Token Deployer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnDeploy As Button
    Friend WithEvents lsConsole As ListBox
    Friend WithEvents cmbNetwork As ComboBox
    Friend WithEvents lblNetwork As Label
    Friend WithEvents lblName As Label
    Friend WithEvents lblDecimals As Label
    Friend WithEvents lblTotalSupply As Label
    Friend WithEvents lblSymbol As Label
    Friend WithEvents txtTotalSupply As TextBox
    Friend WithEvents txtDecimals As TextBox
    Friend WithEvents txtSymbol As TextBox
    Friend WithEvents txtName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtAddress As TextBox
End Class
