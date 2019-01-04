using System;

namespace Scrumtics
{
    public partial class barra_usuario : System.Web.UI.UserControl
    {
        private string usuario;
        public string Usuario { get { return usuario; } }
        private string sm;
        public string SM { get { return sm; } }
        private string imagem;
        public string Imagem { get { return imagem; } }
        private string pacote;
        public string Pacote { get { return pacote; } }
        private string votacao;
        public string Votacao { get { return votacao; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            usuario = Session["UsuarioLogin"].ToString();
            sm = (bool)Session["ScrumMaster"] ? " (SM)" : string.Empty;
            imagem = Session["UsuarioId"].ToString();
            pacote =  Request.Form["pacote"];
            votacao = Request.Form["votacao"];
        }
    }
}