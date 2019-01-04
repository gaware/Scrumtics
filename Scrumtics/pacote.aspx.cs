using System;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Scrumtics
{
    public partial class pacote : System.Web.UI.Page
    {
        private string usuario;
        private string senha;
        private bool logou;
        private string _pacote;
        private string mensagem;
        public string Mensagem { get { return mensagem; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            Response.AppendHeader("pragma", "no-cache");
            if (!(bool)Session["Iniciou"] || 
                string.IsNullOrWhiteSpace(Session["UsuarioLogin"].ToString()) ||
                Session["UsuarioLogin"].ToString() != Request.Form["usuario"] &&
                !string.IsNullOrWhiteSpace(Request.Form["usuario"]))
            {
                usuario = Request.Form["usuario"];
                senha = Request.Form["senha"];
                logou = ((bool)Session["Iniciou"] && !string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(senha));
                if (logou)
                {
                    string xml = string.Empty;
                    using (WebClient client = new WebClient())
                    {
                        var parametros = new System.Collections.Specialized.NameValueCollection();
                        parametros.Add("modo", "B");
                        parametros.Add("login", usuario);
                        parametros.Add("senha", senha);
                        byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/usuario_cadastro.ashx", "POST", parametros);
                        xml = Encoding.UTF8.GetString(resposta);
                    }
                    if (!xml.Contains("<mensagem") && xml.Contains("<id>"))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xml);
                        if (Convert.ToInt64(xmlDoc.SelectSingleNode("/retorno/id").InnerText) > 0)
                        {
                            Session["UsuarioId"] = xmlDoc.SelectSingleNode("/retorno/id").InnerText;
                            Session["UsuarioLogin"] = usuario;
                            Session["ScrumMaster"] = false;
                        }
                        else
                        {
                            logou = false;
                        }
                    }
                    else
                    {
                        logou = false;
                    }
                }
                if (!logou)
                {
                    Session["Mensagem"] = string.Format(Application["MensagemErro"].ToString(), "Usuário não autorizado.");
                    Response.RedirectPermanent("inicio.aspx", true);
                }
            }
            else
            {
                usuario = Session["UsuarioId"].ToString();
                mensagem = Session["Mensagem"].ToString();
                Session["ScrumMaster"] = false;
                Session["Mensagem"] = string.Empty;
            }
        }

        protected void ButtonConfirm1_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "C");
                parametros.Add("descricao", NewDescription.Text);
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/pacote_cadastro.ashx", "POST", parametros);
                xml = Encoding.UTF8.GetString(resposta);
            }
            if (!xml.Contains("<mensagem") && xml.Contains("<id>"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                _pacote = xmlDoc.SelectSingleNode("/retorno/id").InnerText;
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "C");
                    parametros.Add("pacote", _pacote);
                    parametros.Add("usuario", usuario);
                    parametros.Add("sm", usuario);
                    client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/equipe_cadastro.ashx", "POST", parametros);
                }
                Session["ScrumMaster"] = true;
                Response.Clear();
                StringBuilder formPost = new StringBuilder();
                formPost.Append("<html>");
                formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "equipe.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", _pacote);
                formPost.Append("</form>");
                formPost.Append("</body>");
                formPost.Append("</html>");
                Response.Write(formPost.ToString());                
                Response.End();
            }
            else
            {
                mensagem = string.Format(Application["MensagemErro"].ToString(), "Pacote não cadastrado.");
            }
        }
    }
}