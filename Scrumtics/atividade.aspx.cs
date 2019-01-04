using System;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Xml;

namespace Scrumtics
{
    public partial class atividade : System.Web.UI.Page
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
            else
            {
                long _pacote;
                try
                {
                    _pacote = Convert.ToInt64(pacote);
                }
                catch (Exception)
                {
                    Response.RedirectPermanent("pacote.aspx", true);
                }
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
                parametros.Add("usuario", usuario);
                parametros.Add("foco", !IsPostBack ? "S" : "N");
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/atividade_cadastro.ashx", "POST", parametros);
                html = Encoding.UTF8.GetString(resposta);
            }
            if (html.Contains("status= ;"))
            {
                Session["Mensagem"] = string.Format(Application["MensagemErro"].ToString(), "Pacote não encontrado.");
                Response.RedirectPermanent("pacote.aspx", true);
            }
            else if (!html.Contains("<tr>"))
            {
                Session["Mensagem"] = string.Format(Application["MensagemErro"].ToString(), "Usuário não vinculado ao pacote.");
                Response.RedirectPermanent("pacote.aspx", true);
            }
            else
            {
                if (html.Contains(string.Format("sm={0};", usuario)))
                {
                    Session["ScrumMaster"] = true;
                    Div3.Visible = html.Contains("previstos=S;");
                }
                else
                {
                    if (html.Contains("status=V;"))
                    {
                        Session["Mensagem"] = string.Format(Application["MensagemErro"].ToString(), "Pacote não liberado.");
                        Response.RedirectPermanent("pacote.aspx", true);
                    }
                    Session["ScrumMaster"] = false;
                    PlaceHolder2.Visible = false;
                    Div2.Visible = false;
                    Div3.Visible = false;
                }                
                PlaceHolder1.Controls.Add(new LiteralControl(html));
            }
        }

        protected void ButtonConfirm1_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "C");
                parametros.Add("pacote", pacote);
                parametros.Add("descricao", NewDescription.Text);
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/atividade_cadastro.ashx", "POST", parametros);
                xml = Encoding.UTF8.GetString(resposta);
            }
            if (!xml.Contains("<mensagem") && xml.Contains("<id>"))
            {
                mensagem = string.Format(Application["MensagemSucesso"].ToString(), "Atividade criada com sucesso.");
                NewDescription.Text = string.Empty;
                string html = string.Empty;
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "B");
                    parametros.Add("pacote", pacote);
                    parametros.Add("usuario", usuario);
                    parametros.Add("foco", !IsPostBack ? "S" : "N");
                    byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/atividade_cadastro.ashx", "POST", parametros);
                    html = Encoding.UTF8.GetString(resposta);
                }
                Div3.Visible = html.Contains("previstos=S;");
                PlaceHolder1.Controls.Clear();
                PlaceHolder1.Controls.Add(new LiteralControl(html));
            }
            else
            {
                mensagem = string.Format(Application["MensagemErro"].ToString(), "Atividade não cadastrada.");
            }
        }
    }
}