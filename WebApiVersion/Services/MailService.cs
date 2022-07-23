using System.Net.Mail;
using WebApiVersion.Models;

namespace WebApiVersion.Services;

public interface IMaiLService
{
    void SendEmail(string to, string @from, string subject, string text, DocumentDownloadModel document);
}


public class MailService : IMaiLService
{
    public void SendEmail(string from, string to, string subject, string body, DocumentDownloadModel document)
    {
        var mailMessage = new MailMessage(from, to, subject, body);
        mailMessage.Attachments.Add(new Attachment(document.Stream, "document", document.Mime));
        
        using var smtpClient = new SmtpClient();
        smtpClient.Send(mailMessage);
    }
}
