using System;

namespace Scrumtics
{
    public partial class sobre : System.Web.UI.Page
    {
        private string servidor;
        public string Servidor { get { return servidor; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            Application.Lock();
            Session["Iniciou"] = true;
            Session["UsuarioId"] = string.Empty;
            Session["UsuarioLogin"] = string.Empty;
            Session["ScrumMaster"] = false;
            Application.UnLock();
            servidor = TimeSpan.FromMilliseconds(Environment.TickCount).ToString();
            servidor = servidor.Remove(servidor.Length - 8);
        }
    }
}