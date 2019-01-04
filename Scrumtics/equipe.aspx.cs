using System;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Xml;

namespace Scrumtics
{
    public partial class equipe : System.Web.UI.Page
    {
        private string usuario;
        private string pacote;
        public string Pacote { get { return pacote; } }
        private string descricao;
        public string Descricao { get { return descricao; } }
        private string mensagem;
        public string Mensagem { get { return mensagem; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Session["UsuarioId"].ToString();
            pacote = Request.Form["pacote"];
            if (!(bool)Session["Iniciou"] || string.IsNullOrWhiteSpace(usuario))
            {
                Session["Mensagem"] = string.Format(Application["MensagemErro"].ToString(), "Usuário não autorizado.");
                Response.RedirectPermanent("inicio.aspx", true);
            }
            else if (string.IsNullOrWhiteSpace(pacote))
            {
                Response.RedirectPermanent("pacote.aspx", true);
            }
            string xml = string.Empty;
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "B");
                parametros.Add("id", pacote);
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/pacote_cadastro.ashx", "POST", parametros);
                xml = Encoding.UTF8.GetString(resposta);
            }
            if (!xml.Contains("<mensagem") && xml.Contains("<descricao>"))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                descricao = xmlDoc.SelectSingleNode("/retorno/descricao").InnerText;
            }
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "B");
                parametros.Add("pacote", pacote);
                parametros.Add("sm", usuario);
                parametros.Add("foco", !IsPostBack ? "S" : "N");
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/equipe_cadastro.ashx", "POST", parametros);
                html = Encoding.UTF8.GetString(resposta);
            }
            PlaceHolder1.Controls.Add(new LiteralControl(html));
        }

        protected void ButtonConfirm1_Click(object sender, EventArgs e)
        {
            if (NewLogin.Text.Length < 5)
            {
                mensagem = string.Format(Application["MensagemErro"].ToString(), "Usuário deve possuir 5 caracteres ou mais.");
            }
            else
            {
                string xml = string.Empty;

                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "C");
                    parametros.Add("login", NewLogin.Text);
                    parametros.Add("senha", "123");
                    parametros.Add("nome", NewName.Text);
                    parametros.Add("email", NewEmail.Text);
                    parametros.Add("imagem", string.Empty);
                    byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/usuario_cadastro.ashx", "POST", parametros);
                    xml = Encoding.UTF8.GetString(resposta);
                }
                if (!xml.Contains("<mensagem") && xml.Contains("<id>"))
                {
                    mensagem = string.Format(Application["MensagemSucesso"].ToString(),
                        string.Format("Usuário {0} criado com sucesso com a senha padrão.", NewLogin.Text.TrimEnd()));
                    NewLogin.Text = string.Empty;
                    NewName.Text = string.Empty;
                    NewEmail.Text = string.Empty;
                    string html = string.Empty;
                    using (WebClient client = new WebClient())
                    {
                        var parametros = new System.Collections.Specialized.NameValueCollection();
                        parametros.Add("modo", "B");
                        parametros.Add("pacote", pacote);
                        parametros.Add("sm", usuario);
                        parametros.Add("foco", !IsPostBack ? "S" : "N");
                        byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/equipe_cadastro.ashx", "POST", parametros);
                        html = Encoding.UTF8.GetString(resposta);
                    }
                    PlaceHolder1.Controls.Clear();
                    PlaceHolder1.Controls.Add(new LiteralControl(html));
                }
                else
                {
                    mensagem = string.Format(Application["MensagemErro"].ToString(), "Usuário já cadastrado.");
                }
            }
        }
    }
}