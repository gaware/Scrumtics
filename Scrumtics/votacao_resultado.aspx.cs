using System;
using System.Net;
using System.Text;
using System.Xml;

namespace Scrumtics
{
    public partial class votacao_resultado : System.Web.UI.Page
    {
        private string usuario;
        private string pacote;
        public string Pacote { get { return pacote; } }
        private string votacao;
        private string atividade;
        public string Atividade { get { return atividade; } }
        private string tarefa;
        public string Tarefa { get { return tarefa; } }
        public string Votacao { get { return votacao; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Session["UsuarioId"].ToString();
            pacote = Request.Form["pacote"];
            atividade = Request.Form["atividade"];
            tarefa = Request.Form["tarefa"];
            votacao = Request.Form["votacao"];
            if (!(bool)Session["Iniciou"] || string.IsNullOrWhiteSpace(usuario))
            {
                Session["Mensagem"] = string.Format(Application["MensagemErro"].ToString(), "Usuário não autorizado.");
                Response.RedirectPermanent("inicio.aspx", true);
            }
            else if (string.IsNullOrWhiteSpace(votacao))
            {
                Response.RedirectPermanent("pacote.aspx", true);
            }
            else if (!IsPostBack)
            {
                string xml = string.Empty;
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "M");
                    parametros.Add("id", string.IsNullOrWhiteSpace(tarefa) ? atividade : tarefa);
                    parametros.Add("status", "1");
                    parametros.Add("pacote", pacote);
                    if (!string.IsNullOrWhiteSpace(tarefa))
                    {
                        parametros.Add("atividade", atividade);
                    }
                    byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port +
                        (string.IsNullOrWhiteSpace(tarefa) ? "/atividade" : "/tarefa") + "_cadastro.ashx", "POST", parametros);
                    xml = Encoding.UTF8.GetString(resposta);
                }
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "G");
                    parametros.Add("votacao", votacao);
                    byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/votacao_cadastro.ashx", "POST", parametros);
                    xml = Encoding.UTF8.GetString(resposta);
                }
                if (!xml.Contains("<mensagem") && xml.Contains("<media>"))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    LabelMedia.Text = xmlDoc.SelectSingleNode("/retorno/media").InnerText;
                    LabelTotal.Text = xmlDoc.SelectSingleNode("/retorno/total").InnerText;
                    LabelMaioria.Text = xmlDoc.SelectSingleNode("/retorno/maioria").InnerText;
                    LabelQuantidade.Text = xmlDoc.SelectSingleNode("/retorno/quantidade").InnerText;
                    NewProvided.Text = xmlDoc.SelectSingleNode("/retorno/previsto").InnerText.Replace(",", ".");
                }
                else
                {
                    LabelMedia.Text = "0,00";
                    LabelTotal.Text = "0";
                    LabelMaioria.Text = "0";
                    LabelQuantidade.Text = "0";
                }
            }
        }

        protected void ButtonConfirm1_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "M");
                parametros.Add("id", string.IsNullOrWhiteSpace(tarefa) ? atividade : tarefa);
                parametros.Add("previsto", NewProvided.Text);
                parametros.Add("pacote", pacote);
                if (!string.IsNullOrWhiteSpace(tarefa))
                {
                    parametros.Add("atividade", atividade);
                }
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port +
                    (string.IsNullOrWhiteSpace(tarefa) ? "/atividade" : "/tarefa") + "_cadastro.ashx", "POST", parametros);
                xml = Encoding.UTF8.GetString(resposta);
            }
            Response.Clear();
            StringBuilder formPost = new StringBuilder();
            formPost.Append("<html>");
            formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
            if (string.IsNullOrWhiteSpace(tarefa))
            {
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "atividade.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", pacote);
            }
            else
            {
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "tarefa.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", pacote);
                formPost.AppendFormat(@"<input type=""hidden"" name=""atividade"" value=""{0}"">", atividade);
            }
            formPost.Append("</form>");
            formPost.Append("</body>");
            formPost.Append("</html>");
            Response.Write(formPost.ToString());
        }

        protected void ButtonRepeat1_Click(object sender, EventArgs e)
        {
            string xml = string.Empty;
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "E");
                parametros.Add("votacao", votacao);
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/votacao_cadastro.ashx", "POST", parametros);
                xml = Encoding.UTF8.GetString(resposta);
            }
            Response.Clear();
            StringBuilder formPost = new StringBuilder();
            formPost.Append("<html>");
            formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
            formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "votacao_abertura.aspx");
            formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", pacote);
            formPost.AppendFormat(@"<input type=""hidden"" name=""atividade"" value=""{0}"">", atividade);
            if (!string.IsNullOrWhiteSpace(tarefa))
            {
                formPost.AppendFormat(@"<input type=""hidden"" name=""tarefa"" value=""{0}"">", tarefa);
            }
            formPost.AppendFormat(@"<input type=""hidden"" name=""votacao"" value=""{0}"">", votacao);
            formPost.Append("</form>");
            formPost.Append("</body>");
            formPost.Append("</html>");
            Response.Write(formPost.ToString());
        }
    }
}