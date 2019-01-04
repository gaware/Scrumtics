using System;

namespace Scrumtics
{
    public partial class barra_inicio : System.Web.UI.UserControl
    {
        private string acao;
        public string Acao { get { return acao; } }
        private string votacao;
        public string Votacao { get { return votacao; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            acao = string.IsNullOrWhiteSpace(Request.Params["votacao"]) ? "pacote.aspx" : "votacao.aspx";
            votacao = string.IsNullOrWhiteSpace(Request.Params["votacao"]) ? string.Empty : Request.Params["votacao"];
        }
    }
}