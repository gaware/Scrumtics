using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace Scrumtics
{
    public class Email
    {
        public static string EnviaNoticacao(string para, string assunto, string corpo)
        {
            try
            {
                if (ConfigurationManager.AppSettings.Get("NOTIFICAR") == "S")
                {
                    SmtpClient envio = new SmtpClient();
                    envio.Host = ConfigurationManager.AppSettings.Get("SERVIDOR");
                    envio.Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("PORTA"));
                    envio.EnableSsl = true;
                    envio.UseDefaultCredentials = false;
                    envio.Credentials = new NetworkCredential(ConfigurationManager.AppSettings.Get("REMETENTE"), ConfigurationManager.AppSettings.Get("SENHA"));
                    MailMessage mensagem = new MailMessage();
                    mensagem.From = new MailAddress(ConfigurationManager.AppSettings.Get("REMETENTE"), " ");
                    mensagem.To.Add(para);
                    mensagem.Subject = assunto;
                    mensagem.IsBodyHtml = false;
                    mensagem.BodyEncoding = Encoding.UTF8;
                    mensagem.Body = corpo;
                    envio.Send(mensagem);
                    mensagem.Dispose();
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                return "ERROR | " + e.Message;
            }
        }
    }
}