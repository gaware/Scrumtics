using System;
using System.Net;
using System.Web;

namespace Scrumtics
{
    /// <summary>
    /// Summary description for usuario_cadastro
    /// </summary>
    public class usuario_cadastro : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            RegistroUsuario cadUsuario = new RegistroUsuario();

            if (context.Request.Form["modo"] == "C")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                context.Response.Write("<id>" +
                                        cadUsuario.CriaRegistro(
                                            context.Request.Form["login"],
                                            context.Request.Form["senha"],
                                            context.Request.Form["nome"],
                                            context.Request.Form["email"],
                                            context.Request.Form["imagem"]
                                            ) + "</id>" + Environment.NewLine);
                if (cadUsuario.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadUsuario.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "M")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                context.Response.Write("<id>" +
                                       cadUsuario.ModificaRegistro(
                                           Convert.ToInt64(context.Request.Form["id"]),
                                           context.Request.Form["login"],
                                           context.Request.Form["senha"],
                                           context.Request.Form["nome"],
                                           context.Request.Form["email"],
                                           context.Request.Form["imagem"]
                                           ) + "</id>" + Environment.NewLine);
                if (cadUsuario.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadUsuario.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "B")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                if (string.IsNullOrWhiteSpace(context.Request.Form["id"]))
                {

                    context.Response.Write("<id>" +
                                       cadUsuario.BuscaUsuario(
                                           context.Request.Form["login"],
                                           context.Request.Form["senha"]
                                           ) + "</id>" + Environment.NewLine);
                }
                else
                {
                    string[] usuario = cadUsuario.BuscaUsuario(
                                           Convert.ToInt64(context.Request.Form["id"])
                                           );
                    for (int i = 3; i < usuario.Length; i = i + 4)
                    {
                        context.Response.Write("<nome>" + usuario[i - 3] +
                                               "</nome>" + Environment.NewLine);
                        context.Response.Write("<email>" + usuario[i - 2] +
                                               "</email>" + Environment.NewLine);
                        context.Response.Write("<imagem>" + usuario[i - 1] +
                                               "</imagem>" + Environment.NewLine);
                        context.Response.Write("<pacotes>" + usuario[i] +
                                               "</pacotes>" + Environment.NewLine);
                    }
                }
                if (cadUsuario.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadUsuario.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else
            {
                context.Response.RedirectPermanent("inicio.aspx");
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}