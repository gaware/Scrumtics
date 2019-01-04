using System;

namespace Scrumtics
{
    public partial class inicio : System.Web.UI.Page
    {
        private string mensagem;
        public string Mensagem { get { return mensagem; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Application.Lock();
            Session["Iniciou"] = true;
            Session["UsuarioId"] = string.Empty;
            Session["UsuarioLogin"] = string.Empty;
            Session["ScrumMaster"] = false;
            Application.UnLock();
            if (!string.IsNullOrWhiteSpace(Session["Mensagem"].ToString()))
            {
                mensagem = Session["Mensagem"].ToString();
                Session["Mensagem"] = string.Empty;
            }
            else
            {
                mensagem = string.Format(Application["MensagemInfo"].ToString(), "Efetue o login para ter acesso aos recursos do sistema.");
            }
        }
    }
}