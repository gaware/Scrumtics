using System;

namespace Scrumtics
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["MensagemSucesso"] = @"<div class=""alert alert-success alert-dismissible"" role=""alert"">" +
                                             @"<button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Fechar""><span aria-hidden=""true"">&times;</span></button>" +
                                              "<strong>Sucesso:</strong> {0}</div>";
            Application["MensagemInfo"] = @"<div class=""alert alert-info alert-dismissible"" role=""alert"">" +
                                          @"<button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Fechar""><span aria-hidden=""true"">&times;</span></button>" +
                                           "<strong>Informação:</strong> {0}</div>";
            Application["MensagemAviso"] = @"<div class=""alert alert-warning alert-dismissible"" role=""alert"">" +
                                           @"<button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Fechar""><span aria-hidden=""true"">&times;</span></button>" +
                                            "<strong>Aviso:</strong> {0}</div>";
            Application["MensagemErro"] = @"<div class=""alert alert-danger alert-dismissible"" role=""alert"">" +
                                          @"<button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Fechar""><span aria-hidden=""true"">&times;</span></button>" + 
                                           "<strong>Erro:</strong> {0}</div>";
            Application.UnLock();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Session.Timeout = 60;
            Session["Iniciou"] = false;
            Session["UsuarioId"] = string.Empty;
            Session["UsuarioLogin"] = string.Empty;
            Session["ScrumMaster"] = false;
            Session["Mensagem"] = string.Empty;
            Application.UnLock();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}