using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text.Json;
using albionSCRAPER.Models;

namespace albionSCRAPER.Services;

public class MailNotificationServer
{
    private readonly EmailConfig _emailConfig;

    public MailNotificationServer()
    {
        _emailConfig = new EmailConfig();
    }

    private EmailConfig LoadEmailConfig()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "albionSCRAPER.Data.mailconfig.json";
        
        using Stream? stream  = assembly.GetManifestResourceStream(resourceName);

        if (stream == null)
        {
            throw new FileNotFoundException("Brak pliku konfiguracyjnego dla emaila. Sprawdz Data/");
        }
        
        using var reader = new StreamReader(stream);
        string json = reader.ReadToEnd();
        
        return JsonSerializer.Deserialize<EmailConfig>(json) ?? throw new InvalidOperationException("Nieprawidlowy format konfiguracji");
    }

    public async Task SendNotificationAsync(string toEmail, string subject, string body)
    {
        var message = new MailMessage()
        {
            From = new MailAddress(_emailConfig.SenderEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };
        message.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.Port)
        {
            Port = _emailConfig.Port,
            Credentials = new NetworkCredential(_emailConfig.SenderEmail, _emailConfig.SenderPassword),
            EnableSsl = _emailConfig.EnableSsl
        };
        
        await client.SendMailAsync(message);
    }

}