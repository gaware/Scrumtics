using System;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Scrumtics
{
    /// <summary>
    /// Summary description for imagem
    /// </summary>
    public class imagem : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            RegistroUsuario cadUsuario = new RegistroUsuario();

            if (!string.IsNullOrWhiteSpace(context.Request.QueryString["usuario"]))
            {
                context.Response.ContentType = "image/png";
                string xml = string.Empty;
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "B");
                    parametros.Add("id", context.Request.QueryString["usuario"]);
                    byte[] resposta = client.UploadValues(context.Request.Url.Scheme + "://localhost:" + context.Request.Url.Port + "/usuario_cadastro.ashx", "POST", parametros);
                    xml = Encoding.UTF8.GetString(resposta);
                }
                if (!xml.Contains("<mensagem") && xml.Contains("<imagem>"))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    if (!string.IsNullOrWhiteSpace(xmlDoc.SelectSingleNode("/retorno/imagem").InnerText))
                    {
                        if (xmlDoc.SelectSingleNode("/retorno/imagem").InnerText.StartsWith("JPG_"))
                        {
                            context.Response.ContentType = "image/jpg";
                        }
                        context.Response.BinaryWrite(Convert.FromBase64String(xmlDoc.SelectSingleNode("/retorno/imagem").InnerText.Substring(4)));
                    }
                    else
                    {
                        context.Response.WriteFile("img/User-Coat-Red-icon.png");
                    }
                }
                else
                {
                    context.Response.WriteFile("img/User-Coat-Red-icon.png");
                }
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