using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SmartStockAI.Domain.Authentication.Interfaces;

namespace SmartStockAI.Infrastructure.Authentication.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public SmtpEmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var smtpHost = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
        var smtpUser = _configuration["Email:SmtpUser"];
        var smtpPass = _configuration["Email:SmtpPass"];
        var from = _configuration["Email:From"];

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        var mail = new MailMessage(from, to, subject, body)
        {
            IsBodyHtml = false
        };

        await client.SendMailAsync(mail);
    }
}