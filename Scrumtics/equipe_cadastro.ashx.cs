using System;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Scrumtics
{
    /// <summary>
    /// Summary description for equipe_cadastro
    /// </summary>
    public class equipe_cadastro : IHttpHandler, IRequiresSessionState, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            RegistroEquipePacote cadEquipe = new RegistroEquipePacote();

            if (context.Request.Form["modo"] == "C")
            {
                cadEquipe.CriaRegistro(
                    Convert.ToInt64(context.Request.Form["pacote"]),
                    Convert.ToInt64(context.Request.Form["usuario"])
                    );
                if (context.Request.Form["usuario"] != context.Request.Form["sm"])
                {
                    context.Response.ContentType = "text/html";
                    context.Response.Clear();
                    StringBuilder formPost = new StringBuilder();
                    formPost.Append("<html>");
                    formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
                    formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "equipe.aspx");
                    formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", context.Request.Form["pacote"]);
                    formPost.Append("</form>");
                    formPost.Append("</body>");
                    formPost.Append("</html>");
                    context.Response.Write(formPost.ToString());
                }
            }
            else if (context.Request.Form["modo"] == "E")
            {
                cadEquipe.ExcluiRegistro(Convert.ToInt64(context.Request.Form["id"]));
                context.Response.ContentType = "text/html";
                context.Response.Clear();
                StringBuilder formPost = new StringBuilder();
                formPost.Append("<html>");
                formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "equipe.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", context.Request.Form["pacote"]);
                formPost.Append("</form>");
                formPost.Append("</body>");
                formPost.Append("</html>");
                context.Response.Write(formPost.ToString());
            }
            else if (context.Request.Form["modo"] == "B")
            {
                context.Response.ContentType = "text/html";
                string[] equipe = cadEquipe.BuscaEquipe(Convert.ToInt64(context.Request.Form["pacote"]));
                string foco = context.Request.Form["foco"] == "S" ? @" autofocus=""autofocus""" : string.Empty;
                string sm = string.Empty;
                string usuarios = string.Empty;
                for (int i = 5; i < equipe.Length; i = i + 6)
                {
                    if (equipe[i - 1] == context.Request.Form["sm"])
                    {
                        sm = "<tr>" + Environment.NewLine +
                             "<td>" + equipe[i - 5] + "</td>" + Environment.NewLine +
                             "<td>" + equipe[i - 4] + "</td>" + Environment.NewLine +
                             "<td>" + equipe[i - 3] + "</td>" + Environment.NewLine +
                            @"<td><button type=""submit"" class=""btn-link"" disabled=""disabled"">SM</button></td>" + Environment.NewLine +
                             "</tr>" + Environment.NewLine;
                    }
                    else
                    {
                        usuarios += "<tr>" + Environment.NewLine +
                                    "<td>" + equipe[i - 5] + "</td>" + Environment.NewLine +
                                    "<td>" + equipe[i - 4] + "</td>" + Environment.NewLine +
                                    "<td>" + equipe[i - 3] + "</td>" + Environment.NewLine;
                        if (equipe[i - 2] == "S")
                        {
                            usuarios += @"<td><form action=""equipe_cadastro.ashx"" method=""post"">" + Environment.NewLine +
                                        @"<input name=""modo"" type=""hidden"" value=""E"">" + Environment.NewLine +
                                        @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                        @"<input name=""id"" type=""hidden"" value=""" + equipe[i] + @""">" + Environment.NewLine +
                                        @"<button type=""submit"" class=""btn-link""" + foco + ">Desvincular</button>" + Environment.NewLine +
                                        @"</form></td>" + Environment.NewLine;
                            foco = string.Empty;
                        }
                        else
                        {
                            usuarios += @"<td><form action=""equipe_cadastro.ashx"" method=""post"">" + Environment.NewLine +
                                        @"<input name=""modo"" type=""hidden"" value=""C"">" + Environment.NewLine + 
                                        @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                        @"<input name=""usuario"" type=""hidden"" value=""" + equipe[i - 1] + @""">" + Environment.NewLine +
                                        @"<button type=""submit"" class=""btn-link""" + foco + ">Vincular</button>" + Environment.NewLine +
                                        @"</form></td>" + Environment.NewLine;
                            foco = string.Empty;
                        }
                        usuarios += "</tr>" + Environment.NewLine;
                    }
                }
                context.Response.Write(sm);
                context.Response.Write(usuarios);
            }
            else if (context.Request.Form["modo"] == "N")
            {
                string[] equipe = cadEquipe.BuscaEquipe(Convert.ToInt64(context.Request.Form["pacote"]));
                for (int i = 5; i < equipe.Length; i = i + 6)
                {
                    if (equipe[i - 2] == "S")
                    {
                        Email.EnviaNoticacao(equipe[i - 3], context.Request.Form["assunto"], context.Request.Form["corpo"]);
                    }
                }
            }
            else
            {
                context.Response.RedirectPermanent("inicio.aspx");
            }
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}