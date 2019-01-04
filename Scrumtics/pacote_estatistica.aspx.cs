using System;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Xml;

namespace Scrumtics
{
    public partial class pacote_estatistica : System.Web.UI.Page
    {
        private string usuario;
        private string pacote;
        public string Pacote { get { return pacote; } }
        private string descricao;
        public string Descricao { get { return descricao; } }

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
                parametros.Add("modo", "G");
                parametros.Add("id", pacote);
                parametros.Add("previsto", "S");
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/pacote_cadastro.ashx", "POST", parametros);
                html = Encoding.UTF8.GetString(resposta);
            }
            PlaceHolder1.Controls.Add(new LiteralControl(html.Substring(0, html.IndexOf("<!-- usuarios; -->") - 1)));
            PlaceHolder2.Controls.Add(new LiteralControl(html.Substring(html.IndexOf("<!-- usuarios; -->"))));
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "G");
                parametros.Add("id", pacote);
                parametros.Add("previsto", "N");
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/pacote_cadastro.ashx", "POST", parametros);
                html = Encoding.UTF8.GetString(resposta);
            }
            PlaceHolder3.Controls.Add(new LiteralControl(html.Substring(0, html.IndexOf("<!-- usuarios; -->") - 1)));
            PlaceHolder4.Controls.Add(new LiteralControl(html.Substring(html.IndexOf("<!-- usuarios; -->"))));
        }
    }
}