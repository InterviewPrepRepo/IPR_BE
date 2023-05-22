using System.Net.Mail;
using System.Net;

namespace IPR_BE.Services;

public class SMTPService {
    private SmtpClient smtp;
    public SMTPService(IConfiguration iconfig) {
        smtp = new SmtpClient(iconfig["SMTP:Host"], int.Parse(iconfig["SMTP:Port"]!))
        {
            Credentials = new NetworkCredential(iconfig["SMTP:Username"], iconfig["SMTP:Password"]),
            EnableSsl = true,
        };
    }

    public void SendEmail(MailMessage msg) {
        smtp.Send(msg);
    }
}