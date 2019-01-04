using System;
using System.Net;
using System.Text;
using System.Xml;

namespace Scrumtics
{
    public partial class usuario : System.Web.UI.Page
    {
        private string login;
        private string id;
        private string mensagem;
        public string Mensagem { get { return mensagem; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            login = Session["UsuarioLogin"].ToString();
            id = Session["UsuarioId"].ToString();
            Session["ScrumMaster"] = false;
            if (!(bool)Session["Iniciou"] )
            {
                Response.RedirectPermanent("inicio.aspx", true);
            }
            else if (!string.IsNullOrWhiteSpace(login) && !IsPostBack)
            {
                TopNavbarIni1.Visible = false;
                TopNavbarUsu1.Visible = true;
                NewLogin.Text = login;
                NewLogin.Enabled = false;
                string xml = string.Empty;
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", "B");
                    parametros.Add("id", id);
                    byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/usuario_cadastro.ashx", "POST", parametros);
                    xml = Encoding.UTF8.GetString(resposta);
                }
                if (!xml.Contains("<mensagem") && xml.Contains("<imagem>"))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xml);
                    NewName.Text = xmlDoc.SelectSingleNode("/retorno/nome").InnerText;
                    NewEmail.Text = xmlDoc.SelectSingleNode("/retorno/email").InnerText;
                    if (!string.IsNullOrWhiteSpace(xmlDoc.SelectSingleNode("/retorno/pacotes").InnerText))
                    {
                        mensagem = string.Format(Application["MensagemAviso"].ToString(), "Pacotes vinculados ao usuário: " +
                            xmlDoc.SelectSingleNode("/retorno/pacotes").InnerText + ".");
                    }
                    else
                    {
                        mensagem = string.Format(Application["MensagemAviso"].ToString(), "Pacotes vinculados ao usuário: nenhum.");
                    }
                }
                ButtonConfirm1.Text = "Modificar usuário";
            }
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
                bool sucesso = false;
                using (WebClient client = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("modo", string.IsNullOrWhiteSpace(login) ? "C" : "M");
                    parametros.Add("login", NewLogin.Text);
                    parametros.Add("senha", NewPassword.Text);
                    parametros.Add("nome", NewName.Text);
                    parametros.Add("email", NewEmail.Text);
                    parametros.Add("imagem", 
                        NewImage.FileName.ToLower().EndsWith(".png") ?
                        "PNG_" + Convert.ToBase64String(NewImage.FileBytes) : 
                        (NewImage.FileName.ToLower().EndsWith(".jpg") ?
                        "JPG_" + Convert.ToBase64String(NewImage.FileBytes) :
                        string.Empty));
                    parametros.Add("id", id);
                    byte[] resposta = client.UploadValues(Request.Url.Scheme + "://localhost:" + Request.Url.Port + "/usuario_cadastro.ashx", "POST", parametros);
                    xml = Encoding.UTF8.GetString(resposta);
                }
                if (!xml.Contains("<mensagem") && xml.Contains("<id>"))
                {
                    sucesso = true;
                    if (!string.IsNullOrWhiteSpace(login) && xml.Contains("<id>"))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(xml);
                        sucesso = xmlDoc.SelectSingleNode("/retorno/id").InnerText == id;
                    }
                }
                if (sucesso)
                {
                    mensagem = string.Format(Application["MensagemSucesso"].ToString(),
                        string.Format("Usuário {0} {1} com sucesso.", NewLogin.Text.TrimEnd(), string.IsNullOrWhiteSpace(login) ? "criado" : "modificado"));
                    NewLogin.Text = string.Empty;
                    NewPassword.Text = string.Empty;
                    NewName.Text = string.Empty;
                    NewEmail.Text = string.Empty;
                    NewImage = null;
                    if (!string.IsNullOrWhiteSpace(login))
                    {
                        Session["UsuarioId"] = string.Empty;
                        Session["UsuarioLogin"] = string.Empty;
                        TopNavbarIni1.Visible = true;
                        TopNavbarUsu1.Visible = false;
                        NewLogin.Enabled = true;
                        ButtonConfirm1.Text = "Criar usuário";
                    }
                }
                else
                {
                    mensagem = string.Format(Application["MensagemErro"].ToString(), 
                        string.Format("Usuário {0}.", string.IsNullOrWhiteSpace(login) ? "já cadastrado" : "não modificado"));
                }
            }
        }
    }
}