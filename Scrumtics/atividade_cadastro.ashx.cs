using System;
using System.Text;
using System.Web;

namespace Scrumtics
{
    /// <summary>
    /// Summary description for atividade_cadastro
    /// </summary>
    public class atividade_cadastro : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            RegistroAtividadePacote cadAtividade = new RegistroAtividadePacote();

            if (context.Request.Form["modo"] == "C")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                context.Response.Write("<id>" + 
                                        cadAtividade.CriaRegistro(
                                            Convert.ToInt64(context.Request.Form["pacote"]),
                                            context.Request.Form["descricao"]
                                            ) + "</id>" + Environment.NewLine);
                if (cadAtividade.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "M")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                if (context.Request.Form["status"] != ((int)RegistroAtividadePacote.Status.Votação).ToString() &&
                    !string.IsNullOrWhiteSpace(context.Request.Form["descricao"]))
                {
                    context.Response.Write("<id>" +
                                           cadAtividade.ModificaRegistro(
                                               Convert.ToInt64(context.Request.Form["id"]),
                                               context.Request.Form["status"],
                                               context.Request.Form["descricao"],
                                               decimal.Zero,
                                               Convert.ToDecimal(context.Request.Form["realizado"].Replace(".", ",")),
                                               0
                                               ) + "</id>" + Environment.NewLine);
                }
                else if (!string.IsNullOrWhiteSpace(context.Request.Form["status"]))
                {
                    context.Response.Write("<id>" +
                                           cadAtividade.ModificaRegistro(
                                               Convert.ToInt64(context.Request.Form["id"]),
                                               context.Request.Form["status"], 
                                               string.Empty,
                                               decimal.Zero,
                                               decimal.Zero,
                                               Convert.ToInt64(context.Request.Form["pacote"])
                                               ) + "</id>" + Environment.NewLine);
                }
                else
                {
                    context.Response.Write("<id>" +
                                           cadAtividade.ModificaRegistro(
                                               Convert.ToInt64(context.Request.Form["id"]),
                                               string.Empty,
                                               string.Empty,
                                               Convert.ToDecimal(context.Request.Form["previsto"].Replace(".", ",")),
                                               decimal.Zero,
                                               Convert.ToInt64(context.Request.Form["pacote"])
                                               ) + "</id>" + Environment.NewLine);
                }
                if (cadAtividade.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "E")
            {
                cadAtividade.ExcluiRegistro(
                    Convert.ToInt64(context.Request.Form["pacote"]),
                    Convert.ToInt64(context.Request.Form["id"])
                    );
                context.Response.ContentType = "text/html";
                context.Response.Clear();
                StringBuilder formPost = new StringBuilder();
                formPost.Append("<html>");
                formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "atividade.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", context.Request.Form["pacote"]);
                formPost.Append("</form>");
                formPost.Append("</body>");
                formPost.Append("</html>");
                context.Response.Write(formPost.ToString());
            }
            else if (context.Request.Form["modo"] == "B")
            {
                if(string.IsNullOrWhiteSpace(context.Request.Form["id"]))
                {
                    context.Response.ContentType = "text/html";
                    string[] atividade = cadAtividade.BuscaAtividade(
                                             Convert.ToInt64(context.Request.Form["pacote"]),
                                             Convert.ToInt64(context.Request.Form["usuario"])
                                             );
                    string foco = context.Request.Form["foco"] == "S" ? @" autofocus=""autofocus""" : string.Empty;
                    bool previstos = false;
                    string atividades = string.Format("<!-- sm={0}; -->", atividade[0]) + Environment.NewLine;
                    atividades += string.Format("<!-- status={0}; -->", atividade[1]) + Environment.NewLine;
                    for (int i = 7; i < atividade.Length; i = i + 6)
                    {
                        atividades += "<tr>" + Environment.NewLine +
                                    "<td>" + RegistroAtividadePacote.DescricaoStatus(atividade[i - 5]) + "</td>" + Environment.NewLine +
                                    "<td>" + atividade[i - 4].Replace("<", "&lt;").Replace(">", "&gt;") + "</td>" + Environment.NewLine +
                                    (context.Request.Form["usuario"] == atividade[0] && 
                                     atividade[i - 3] == "0,00" && atividade[i - 1] != "True" ?
                                   @"<td><form action=""votacao_abertura.aspx"" method=""post"">" + Environment.NewLine +
                                   @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                   @"<input name=""atividade"" type=""hidden"" value=""" + atividade[i] + @""">" + Environment.NewLine +
                                   @"<input name=""votacao"" type=""hidden"" value=""p" + context.Request.Form["pacote"] + "a" + atividade[i] + @""">" + Environment.NewLine +
                                   @"<button type=""submit"" class=""btn-link""" + foco + ">Votação</button>" + Environment.NewLine +
                                   @"</form></td>" + Environment.NewLine :
                                   @"<td><button type=""button"" class=""btn-link"" disabled=""disabled"">" + atividade[i - 3] + "</button></td>" + Environment.NewLine) +
                                    "<td>" + atividade[i - 2] + "</td>" + Environment.NewLine +
                                    (string.IsNullOrWhiteSpace(atividade[i - 1]) ?
                                    "<td>&nbsp;</td>" + Environment.NewLine +
                                    "<td>&nbsp;</td>" + Environment.NewLine :
                                    (atividade[i - 5] != ((int)RegistroAtividadePacote.Status.Votação).ToString() && 
                                     atividade[i - 3] == "0,00" || atividade[i - 1] == "True" ?
                                   @"<td><form action=""tarefa.aspx"" method=""post"">" + Environment.NewLine +
                                   @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                   @"<input name=""atividade"" type=""hidden"" value=""" + atividade[i] + @""">" + Environment.NewLine +
                                   @"<button type=""submit"" class=""btn-link""" + foco + (atividade[i - 1] == "True" ? ">P" : ">Não p") + "ossui tarefas</button>" + Environment.NewLine +
                                   @"</form></td>" + Environment.NewLine :
                                   @"<td><button type=""button"" class=""btn-link"" disabled=""disabled"">Não possui tarefas</button></td>" + Environment.NewLine) +
                                    (atividade[i - 3] == "0,00" ?
                                    (context.Request.Form["usuario"] != atividade[0] || atividade[i - 1] == "True" ?
                                    "<td>&nbsp;</td>" + Environment.NewLine :
                                    @"<td><form action=""atividade_cadastro.ashx"" method=""post"">" + Environment.NewLine +
                                   @"<input name=""modo"" type=""hidden"" value=""E"">" + Environment.NewLine +
                                   @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                   @"<input name=""id"" type=""hidden"" value=""" + atividade[i] + @""">" + Environment.NewLine +
                                   @"<button type=""submit"" class=""btn-link""" + foco + ">Excluir</button>" + Environment.NewLine +
                                   @"</form></td>" + Environment.NewLine) :
                                    @"<td><form action=""atividade_edicao.aspx"" method=""post"">" + Environment.NewLine +
                                   @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                   @"<input name=""id"" type=""hidden"" value=""" + atividade[i] + @""">" + Environment.NewLine +
                                   @"<button type=""submit"" class=""btn-link""" + foco + ">Editar</button>" + Environment.NewLine +
                                   @"</form></td>" + Environment.NewLine)) +
                                    "<td>" + atividade[i] + "</td>" + Environment.NewLine +
                                    "</tr>" + Environment.NewLine;
                        foco = string.Empty;
                        if (!string.IsNullOrWhiteSpace(atividade[i - 3]) && atividade[i - 3] != "0,00")
                        {
                            previstos = true;
                        }
                    }
                    atividades += string.Format("<!-- previstos={0}; -->", (previstos ? "S" : "N")) + Environment.NewLine;
                    context.Response.Write(atividades);
                }
                else
                {
                    context.Response.ContentType = "text/xml";
                    context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                    context.Response.Write("<retorno>" + Environment.NewLine);
                    string[] atividade = cadAtividade.BuscaAtividade(
                                             Convert.ToInt64(context.Request.Form["id"])
                                             );
                    for (int i = 3; i < atividade.Length; i = i + 4)
                    {
                        context.Response.Write("<status>" + atividade[i - 3] +
                                               "</status>" + Environment.NewLine);
                        context.Response.Write("<descricao>" + atividade[i - 2].Replace("<", "&lt;").Replace(">", "&gt;") +
                                               "</descricao>" + Environment.NewLine);
                        context.Response.Write("<realizado>" + atividade[i - 1] +
                                               "</realizado>" + Environment.NewLine);
                        context.Response.Write("<tarefas>" + atividade[i] +
                                               "</tarefas>" + Environment.NewLine);
                    }
                    if (cadAtividade.Mensagem.Length > 5)
                    {
                        context.Response.Write("<mensagem>" + cadAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                    }
                    context.Response.Write("</retorno>");
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