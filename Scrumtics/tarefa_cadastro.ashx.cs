using System;
using System.Text;
using System.Web;

namespace Scrumtics
{
    /// <summary>
    /// Summary description for tarefa_cadastro
    /// </summary>
    public class tarefa_cadastro : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            RegistroTarefaAtividade cadTarefa = new RegistroTarefaAtividade();

            if (context.Request.Form["modo"] == "C")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                context.Response.Write("<id>" +
                                        cadTarefa.CriaRegistro(
                                            Convert.ToInt64(context.Request.Form["atividade"]),
                                            context.Request.Form["descricao"]
                                            ) + "</id>" + Environment.NewLine);
                if (cadTarefa.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadTarefa.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "M")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                if (context.Request.Form["status"] != ((int)RegistroTarefaAtividade.Status.Votação).ToString() &&
                    !string.IsNullOrWhiteSpace(context.Request.Form["descricao"]))
                {
                    context.Response.Write("<id>" +
                                           cadTarefa.ModificaRegistro(
                                               Convert.ToInt64(context.Request.Form["id"]),
                                               context.Request.Form["status"],
                                               context.Request.Form["descricao"],
                                               decimal.Zero,
                                               Convert.ToDecimal(context.Request.Form["realizado"].Replace(".", ",")),
                                               Convert.ToInt64(context.Request.Form["atividade"]),
                                               0
                                               ) + "</id>" + Environment.NewLine);
                }
                else if (!string.IsNullOrWhiteSpace(context.Request.Form["status"]))
                {
                    context.Response.Write("<id>" +
                                           cadTarefa.ModificaRegistro(
                                               Convert.ToInt64(context.Request.Form["id"]),
                                               context.Request.Form["status"],
                                               string.Empty,
                                               decimal.Zero,
                                               decimal.Zero,
                                               Convert.ToInt64(context.Request.Form["atividade"]),
                                               0
                                               ) + "</id>" + Environment.NewLine);
                }
                else
                {
                    context.Response.Write("<id>" +
                                           cadTarefa.ModificaRegistro(
                                               Convert.ToInt64(context.Request.Form["id"]),
                                               string.Empty,
                                               string.Empty,
                                               Convert.ToDecimal(context.Request.Form["previsto"].Replace(".", ",")),
                                               decimal.Zero,
                                               Convert.ToInt64(context.Request.Form["atividade"]),
                                               Convert.ToInt64(context.Request.Form["pacote"])
                                               ) + "</id>" + Environment.NewLine);
                }
                if (cadTarefa.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadTarefa.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "E")
            {
                cadTarefa.ExcluiRegistro(
                    Convert.ToInt64(context.Request.Form["pacote"]),
                    Convert.ToInt64(context.Request.Form["atividade"]),
                    Convert.ToInt64(context.Request.Form["id"])
                    );
                context.Response.ContentType = "text/html";
                context.Response.Clear();
                StringBuilder formPost = new StringBuilder();
                formPost.Append("<html>");
                formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "tarefa.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""pacote"" value=""{0}"">", context.Request.Form["pacote"]);
                formPost.AppendFormat(@"<input type=""hidden"" name=""atividade"" value=""{0}"">", context.Request.Form["atividade"]);
                formPost.Append("</form>");
                formPost.Append("</body>");
                formPost.Append("</html>");
                context.Response.Write(formPost.ToString());
            }
            else if (context.Request.Form["modo"] == "B")
            {
                if (string.IsNullOrWhiteSpace(context.Request.Form["id"]))
                {
                    context.Response.ContentType = "text/html";
                    string[] tarefa = cadTarefa.BuscaTarefa(
                                                        Convert.ToInt64(context.Request.Form["pacote"]),
                                                        Convert.ToInt64(context.Request.Form["usuario"]),
                                                        Convert.ToInt64(context.Request.Form["atividade"])
                                                        );
                    string foco = context.Request.Form["foco"] == "S" ? @" autofocus=""autofocus""" : string.Empty;
                    string tarefas = string.Format("<!-- sm={0}; -->", tarefa[0]) + Environment.NewLine;
                    tarefas += string.Format("<!-- status={0}; -->", tarefa[1]) + Environment.NewLine;
                    for (int i = 6; i < tarefa.Length; i = i + 5)
                    {
                        tarefas +=  "<tr>" + Environment.NewLine +
                                    "<td>" + RegistroAtividadePacote.DescricaoStatus(tarefa[i - 4]) + "</td>" + Environment.NewLine +
                                    "<td>" + tarefa[i - 3].Replace("<", "&lt;").Replace(">", "&gt;") + "</td>" + Environment.NewLine +
                                    (context.Request.Form["usuario"] == tarefa[0] &&
                                     tarefa[i - 2] == "0,00" ?
                                   @"<td><form action=""votacao_abertura.aspx"" method=""post"">" + Environment.NewLine +
                                   @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                   @"<input name=""atividade"" type=""hidden"" value=""" + context.Request.Form["atividade"] + @""">" + Environment.NewLine +
                                   @"<input name=""tarefa"" type=""hidden"" value=""" + tarefa[i] + @""">" + Environment.NewLine +
                                   @"<input name=""votacao"" type=""hidden"" value=""p" + context.Request.Form["pacote"] + "a" + context.Request.Form["atividade"] + "t" + tarefa[i] + @""">" + Environment.NewLine +
                                   @"<button type=""submit"" class=""btn-link""" + foco + ">Votação</button>" + Environment.NewLine +
                                   @"</form></td>" + Environment.NewLine :
                                   @"<td><button type=""button"" class=""btn-link"" disabled=""disabled"">" + tarefa[i - 2] + "</button></td>" + Environment.NewLine) +
                                    "<td>" + tarefa[i - 1] + "</td>" + Environment.NewLine +
                                    (string.IsNullOrWhiteSpace(tarefa[i]) ?  
                                    "<td>&nbsp;</td>" + Environment.NewLine :
                                    (tarefa[i - 2] == "0,00" ?
                                    (context.Request.Form["usuario"] != tarefa[0] ?
                                    "<td>&nbsp;</td>" + Environment.NewLine :
                                    @"<td><form action=""tarefa_cadastro.ashx"" method=""post"">" + Environment.NewLine +
                                   @"<input name=""modo"" type=""hidden"" value=""E"">" + Environment.NewLine +
                                   @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                   @"<input name=""atividade"" type=""hidden"" value=""" + context.Request.Form["atividade"] + @""">" + Environment.NewLine +
                                   @"<input name=""id"" type=""hidden"" value=""" + tarefa[i] + @""">" + Environment.NewLine +
                                   @"<button type=""submit"" class=""btn-link""" + foco + ">Excluir</button>" + Environment.NewLine +
                                   @"</form></td>" + Environment.NewLine) :
                                    @"<td><form action=""tarefa_edicao.aspx"" method=""post"">" + Environment.NewLine +
                                   @"<input name=""pacote"" type=""hidden"" value=""" + context.Request.Form["pacote"] + @""">" + Environment.NewLine +
                                   @"<input name=""atividade"" type=""hidden"" value=""" + context.Request.Form["atividade"] + @""">" + Environment.NewLine +
                                   @"<input name=""id"" type=""hidden"" value=""" + tarefa[i] + @""">" + Environment.NewLine +
                                   @"<button type=""submit"" class=""btn-link""" + foco + ">Editar</button>" + Environment.NewLine +
                                   @"</form></td>" + Environment.NewLine)) +
                                    "<td>" + tarefa[i] + "</td>" + Environment.NewLine +
                                    "</tr>" + Environment.NewLine;
                        foco = string.Empty;
                    }
                    context.Response.Write(tarefas);
                }
                else
                {
                    context.Response.ContentType = "text/xml";
                    context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                    context.Response.Write("<retorno>" + Environment.NewLine);
                    string[] tarefa = cadTarefa.BuscaTarefa(
                                                        Convert.ToInt64(context.Request.Form["id"])
                                                        );
                    for (int i = 2; i < tarefa.Length; i = i + 3)
                    {
                        context.Response.Write("<status>" + tarefa[i - 2] +
                                               "</status>" + Environment.NewLine);
                        context.Response.Write("<descricao>" + tarefa[i - 1].Replace("<", "&lt;").Replace(">", "&gt;") +
                                               "</descricao>" + Environment.NewLine);
                        context.Response.Write("<realizado>" + tarefa[i] +
                                               "</realizado>" + Environment.NewLine);
                    }
                    if (cadTarefa.Mensagem.Length > 5)
                    {
                        context.Response.Write("<mensagem>" + cadTarefa.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
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