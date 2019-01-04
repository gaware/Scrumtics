using System;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;

namespace Scrumtics
{
    public partial class atividade_edicao : System.Web.UI.Page
    {
        private string pacote;
        public string Pacote { get { return pacote; } }
        private string atividade;
        public string Atividade { get { return atividade; } }
        private string mensagem;
        public string Mensagem { get { return mensagem; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            pacote = Request.Form["pacote"];
            atividade = Request.Form["id"];
            Session["ScrumMaster"] = false;
            if (!(bool)Session["Iniciou"] || string.IsNullOrWhiteSpace(Session["UsuarioId"].ToString()))
            {
                Session["Mensagem"] = string.Format(Application["MensagemErro"].ToString(), "Usuário não autorizado.");
                Response.RedirectPermanent("inicio.aspx", true);
            }
            else if (string.IsNullOrWhiteSpace(pacote) || string.IsNullOrWhiteSpace(atividade))
            {
                Response.RedirectPermanent("pacote.aspx", true);
            }
            else if (!IsPostBack)
            {
                string xml = string.Empty;
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "B");
                    parametros.Add("id", atividade);
                    byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/atividade_cadastro.ashx", "POST", parametros);
                    xml = Encoding.UTF8.GetString(resposta);
                }
                if (!xml.Contains("<mensagem") && xml.Contains("<status>"))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    string status = ((int)RegistroAtividadePacote.Status.Pendente).ToString();
                    NewStatus.Items.Add(new ListItem(RegistroAtividadePacote.DescricaoStatus(status), status));
                    status = ((int)RegistroAtividadePacote.Status.Iniciado).ToString();
                    NewStatus.Items.Add(new ListItem(RegistroAtividadePacote.DescricaoStatus(status), status));
                    status = ((int)RegistroAtividadePacote.Status.Teste).ToString();
                    NewStatus.Items.Add(new ListItem(RegistroAtividadePacote.DescricaoStatus(status), status));
                    status = ((int)RegistroAtividadePacote.Status.Concluído).ToString();
                    NewStatus.Items.Add(new ListItem(RegistroAtividadePacote.DescricaoStatus(status), status));
                    status = ((int)RegistroAtividadePacote.Status.Impedimento).ToString();
                    NewStatus.Items.Add(new ListItem(RegistroAtividadePacote.DescricaoStatus(status), status));
                    status = ((int)RegistroAtividadePacote.Status.Removido).ToString();
                    NewStatus.Items.Add(new ListItem(RegistroAtividadePacote.DescricaoStatus(status), status));
                    NewStatus.Text = xmlDoc.SelectSingleNode("/retorno/status").InnerText;
                    NewDescription.Text = xmlDoc.SelectSingleNode("/retorno/descricao").InnerText;
                    NewRealized.Text = xmlDoc.SelectSingleNode("/retorno/realizado").InnerText.Replace(",", ".");
                    NewRealized.Enabled = xmlDoc.SelectSingleNode("/retorno/tarefas").InnerText != "True";
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
                parametros.Add("id", atividade);
                parametros.Add("status", NewStatus.Text);
                parametros.Add("descricao", NewDescription.Text);
                parametros.Add("realizado", NewRealized.Text);
                byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/atividade_cadastro.ashx", "POST", parametros);
                xml = Encoding.UTF8.GetString(resposta);
            }
            if (!xml.Contains("<mensagem") && xml.Contains("<id>"))
            {
                Response.Clear();
                StringBuilder formPost = new StringBuilder();
                formPost.Append("<html>");
                formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "atividade.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", pacote);
                formPost.Append("</form>");
                formPost.Append("</body>");
                formPost.Append("</html>");
                Response.Write(formPost.ToString());
                Response.End();
            }
            else
            {
                mensagem = string.Format(Application["MensagemErro"].ToString(), "Atividade não cadastrada.");
            }
        }
    }
}