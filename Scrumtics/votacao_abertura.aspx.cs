using System;
using System.Configuration;
using System.Net;
using System.Text;

namespace Scrumtics
{
    public partial class votacao_abertura : System.Web.UI.Page
    {
        private string usuario;
        private string pacote;
        public string Pacote { get { return pacote; } }
        private string atividade;
        public string Atividade { get { return atividade; } }
        private string tarefa;
        public string Tarefa { get { return tarefa; } }
        private string votacao;
        public string Votacao { get { return votacao; } }
        private string qrcode;
        public string Qrcode { get { return qrcode; } }

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
            Form1.Visible = false;
            string xml = string.Empty;
            using (WebClient client = new WebClient())
            {
                var parametros = new System.Collections.Specialized.NameValueCollection();
                parametros.Add("modo", "M");
                parametros.Add("id", string.IsNullOrWhiteSpace(tarefa) ? atividade : tarefa);
                parametros.Add("status", "0");
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
                parametros.Add("modo", "V");
                parametros.Add("votacao", votacao);
                parametros.Add("usuario", usuario);
                parametros.Add("etapa", "A");
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/votacao_cadastro.ashx", "POST", parametros);
                xml = Encoding.UTF8.GetString(resposta);
            }
            qrcode = Request.UrlReferrer.ToString().Replace(Request.UrlReferrer.AbsolutePath + Request.UrlReferrer.Query, "/inicio.aspx") + string.Format("?votacao={0}", votacao);
            if (xml.Contains("<mensagem") || !xml.Contains("<votacao>"))
            {
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
            else
            {
                Form1.Visible = true;
                if (ConfigurationManager.AppSettings.Get("NOTIFICAR") == "S")
                {
                    using (WebClient client = new WebClient())
                    {
                        var parametros = new System.Collections.Specialized.NameValueCollection();
                        parametros.Add("modo", "N");
                        parametros.Add("pacote", pacote);
                        parametros.Add("assunto", "[Scrumtics] Abertura da votação " + votacao);
                        parametros.Add("corpo", qrcode);
                        byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/equipe_cadastro.ashx", "POST", parametros);
                    }
                }
            }
        }
    }
}