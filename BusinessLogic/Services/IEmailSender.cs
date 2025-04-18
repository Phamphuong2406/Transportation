using BusinessLogic.DTOs.SendEmail;
using MailKit.Net.Smtp;
using MimeKit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IEmailSender
    {
        void SendEmail(Message email);
        Task SendEmailAsync(Message message);
    }

    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(Message message)// phương thức này nhận vào một Message (thường chứa To, Subject, Content).
        {
            var emailMessage = CreateEmailMessage(message);//Gọi hàm CreateEmailMessage để chuyển Message thành MimeMessage (thư email chuẩn).
            Send(emailMessage);//Sau đó gọi Send hoặc SendAsync để gửi đi.
        }
        public async Task SendEmailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message)//Tạo nội dung email với CreateEmailMessage
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("",_emailConfig.From));
            emailMessage.To.AddRange(message.To);//Người nhận: To lấy từ message.To.
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h2 style= 'color:red'>{0}</h2>", message.Content )};

            return emailMessage;
        }
      //  Gửi email thực tế với Send
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);//Dùng MailKit để kết nối đến máy chủ SMTP,Connect: kết nối SMTP server (ví dụ Gmail, Outlook, Mailtrap...).
                    client.AuthenticationMechanisms.Remove("XOAUTH2");//Authenticate: xác thực tài khoản gửi.
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);//Send: gửi email đi.
                }
                catch (Exception)
                {

                    throw;
                }
                finally//finally: đảm bảo luôn Disconnect và Dispose sau khi gửi.
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

        }
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }

        }
    }
}
