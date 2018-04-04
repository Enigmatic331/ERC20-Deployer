Public Class inputbox
    Private input As String

    Public ReadOnly Property GetInput As String
        Get
            Return input
        End Get
    End Property

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If Not String.IsNullOrEmpty(txtinput.Text) Then
            input = txtinput.Text
            Me.Close()
        Else
            MessageBox.Show("Please enter a password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
End Class