Imports System
Imports System.Web.Mail

Namespace DONEIN_NET.Dictionary

	Public Class Mail

		Public Function mail_send(recipient As String, sender As String, body As String, Optional subject As String = "", Optional cc As String = "", Optional bcc As String = "", Optional reply_to As String = "", Optional priority As String = "Normal", Optional smtp_server As String = "")
			
			Dim mail_object As New MailMessage

			'// === SET THE SENDER === 
			mail_object.From = sender.Trim

			'// === SET THE REPLY TO ADDRESS ===
			If reply_to.Trim.Length > 0 Then
				mail_object.Headers.Add("Reply-To", reply_to)
			Else
				mail_object.Headers.Add("Reply-To", sender.Trim)
			End If

			'// === SET THE RECIPIENTS ===
			mail_object.To = recipient.Trim
			If cc.Trim.Length > 0 Then
				mail_object.Cc = cc.Trim
			End If
			If bcc.Trim.Length > 0 Then
				mail_object.bcc = bcc.Trim
			End If

			'// === SET THE SUBJECT ===
			mail_object.Subject  = subject.Trim

			'// === SET THE MESSAGE BODY TEXT ===
			mail_object.Body = body.Trim

			'// === SET PRIORITY OF MESSAGE (DEFAULT IS NORMAL) ===
			If priority.Trim = "High" Then
				mail_object.Priority = MailPriority.High
			ElseIf priority.Trim = "Low" Then
				mail_object.Priority = MailPriority.Low
			Else
				mail_object.Priority = MailPriority.Normal
			End If

			'// === SET FORMAT OF THE MAIL ===
			mail_object.BodyFormat = MailFormat.Html
		 	
			'// === SET THE SMTP SERVER ===
			If smtp_server.Trim.Length > 0 Then
				SmtpMail.SmtpServer  = smtp_server.Trim
			Else
				SmtpMail.SmtpServer  = "127.0.0.1"
			End If
			
			'// === SEND THE MESSAGE, RETURN ANY ERRORS ===
			Try
				SmtpMail.Send(mail_object)
				Return ""
			Catch mail_exception as Exception
				Return mail_exception.ToString
			End Try

		End Function
		    
	End Class

End NameSpace


