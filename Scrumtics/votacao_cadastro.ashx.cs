using System;
using System.Text;
using System.Web;

namespace Scrumtics
{
    /// <summary>
    /// Summary description for votacao_cadastro
    /// </summary>
    public class votacao_cadastro : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            RegistroVotacaoAtividade cadVotacaoAtividade = new RegistroVotacaoAtividade();
            RegistroVotacaoTarefa cadVotacaoTarefa = new RegistroVotacaoTarefa();
            string votacao = context.Request.Form["votacao"];
            long atividade;
            long tarefa;

            try
            {
                atividade = Convert.ToInt64(votacao.Substring(votacao.IndexOf('a') + 1, votacao.Length - (votacao.IndexOf('a') + 1)));
            }
            catch (Exception)
            {
                atividade = 0;
            }
            if (atividade != 0)
            {
                tarefa = 0;
            }
            else
            {
                try
                {
                    tarefa = Convert.ToInt64(votacao.Substring(votacao.IndexOf('t') + 1, votacao.Length - (votacao.IndexOf('t') + 1)));
                }
                catch (Exception)
                {
                    tarefa = 0;
                }
            }
            if (context.Request.Form["modo"] == "C" && (atividade != 0 || tarefa != 0))
            {
                context.Response.ContentType = "text/html";
                context.Response.Clear();
                if (atividade != 0)
                {
                    cadVotacaoAtividade.CriaRegistro(
                        atividade,
                        Convert.ToInt64(context.Request.Form["usuario"]),
                        Convert.ToInt64(context.Request.Form["tempo"])
                        );
                }
                else if (tarefa != 0)
                {
                    cadVotacaoTarefa.CriaRegistro(
                        tarefa,
                        Convert.ToInt64(context.Request.Form["usuario"]),
                        Convert.ToInt64(context.Request.Form["tempo"])
                        );
                }
                StringBuilder formPost = new StringBuilder();
                formPost.Append("<html>");
                formPost.AppendFormat(@"<body onload=""document.forms['form'].submit()"">");
                formPost.AppendFormat(@"<form name=""form"" action=""{0}"" method=""post"">", "votacao.aspx");
                formPost.AppendFormat(@"<input type=""hidden"" name=""votacao"" value=""{0}"">", votacao);
                formPost.Append("</form>");
                formPost.Append("</body>");
                formPost.Append("</html>");
                context.Response.Write(formPost.ToString());
            }
            else if (context.Request.Form["modo"] == "E" && (atividade != 0 || tarefa != 0))
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                if (atividade != 0)
                {
                    context.Response.Write("<id>" +
                                           cadVotacaoAtividade.ExcluiRegistro(atividade) + "</id>" + Environment.NewLine);
                    if (cadVotacaoAtividade.Mensagem.Length > 5)
                    {
                        context.Response.Write("<mensagem>" + cadVotacaoAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                    }
                }
                else if (tarefa != 0)
                {
                    context.Response.Write("<id>" +
                                           cadVotacaoTarefa.ExcluiRegistro(tarefa) + "</id>" + Environment.NewLine);
                    if (cadVotacaoTarefa.Mensagem.Length > 5)
                    {
                        context.Response.Write("<mensagem>" + cadVotacaoTarefa.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                    }
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "B")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                if (atividade != 0)
                {
                    context.Response.Write("<id>" +
                                           cadVotacaoAtividade.BuscaVotacaoAtividade(
                                               atividade,
                                               Convert.ToInt64(context.Request.Form["usuario"])
                                               ) + "</id>" + Environment.NewLine);
                    if (cadVotacaoAtividade.Mensagem.Length > 5)
                    {
                        context.Response.Write("<mensagem>" + cadVotacaoAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                    }
                }
                else if (tarefa != 0)
                {
                    context.Response.Write("<id>" +
                                           cadVotacaoTarefa.BuscaVotacaoTarefa(
                                               tarefa,
                                               Convert.ToInt64(context.Request.Form["usuario"])
                                               ) + "</id>" + Environment.NewLine);
                    if (cadVotacaoTarefa.Mensagem.Length > 5)
                    {
                        context.Response.Write("<mensagem>" + cadVotacaoTarefa.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                    }
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "V")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                if (context.Request.Form["etapa"] != "E") 
                {
                    if (atividade != 0)
                    {
                        if (votacao == cadVotacaoAtividade.VerificaVotacaoAtividade(
                                           atividade,
                                           Convert.ToInt64(context.Request.Form["usuario"])
                                           ))
                        {
                            context.Response.Write("<votacao>" + votacao + "</votacao>" + Environment.NewLine);
                        }
                        if (cadVotacaoAtividade.Mensagem.Length > 5)
                        {
                            context.Response.Write("<mensagem>" + cadVotacaoAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                        }
                    }
                    else if (tarefa != 0)
                    {
                        if (votacao == cadVotacaoTarefa.VerificaVotacaoTarefa(
                                           tarefa,
                                           Convert.ToInt64(context.Request.Form["usuario"])
                                           ))
                        {
                            context.Response.Write("<votacao>" + votacao + "</votacao>" + Environment.NewLine);
                        }
                        if (cadVotacaoTarefa.Mensagem.Length > 5)
                        {
                            context.Response.Write("<mensagem>" + cadVotacaoTarefa.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                        }
                    }
                }
                else
                {
                    long pacote;
                    string retorno;
                    try
                    {
                        pacote = Convert.ToInt64(votacao.Substring(votacao.IndexOf('p') + 1, votacao.IndexOf('a') - 1));
                    }
                    catch (Exception)
                    {
                        pacote = 0;
                    }
                    if (pacote != 0 && (atividade != 0 || tarefa != 0))
                    {
                        retorno = cadVotacaoAtividade.VerificaPacote(
                                      pacote,
                                      Convert.ToInt64(context.Request.Form["usuario"]),
                                      atividade,
                                      tarefa
                                      );
                        if (retorno == "L")
                        {
                            context.Response.Write("<pacote>" + pacote + "</pacote>" + Environment.NewLine);
                        }
                        else if (retorno != "V" && !string.IsNullOrWhiteSpace(retorno))
                        {
                            context.Response.Write("<votacao>" + retorno + "</votacao>" + Environment.NewLine);
                        }
                        if (cadVotacaoTarefa.Mensagem.Length > 5)
                        {
                            context.Response.Write("<mensagem>" + cadVotacaoAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                        }
                    }
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "G")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                if (atividade != 0 || tarefa != 0)
                {
                    decimal[] resultado;
                    if (atividade != 0)
                    {
                        resultado = cadVotacaoAtividade.GeraResultadoVotacaoAtividade(atividade);
                    }
                    else 
                    {
                        resultado = cadVotacaoTarefa.GeraResultadoVotacaoTarefa(tarefa);
                    }
                    context.Response.Write("<media>" + resultado[0] + "</media>" + Environment.NewLine);
                    context.Response.Write("<total>" + resultado[1] + "</total>" + Environment.NewLine);
                    if (resultado.Length > 4)
                    {
                        string maiorias = string.Empty;
                        for (int i = 2; i < resultado.Length; i++)
                        {
                            if (i != 3)
                            {
                                maiorias += (i == 2 ? maiorias + resultado[i] : " / " + resultado[i]);
                            }
                        }
                        context.Response.Write("<maioria>" + maiorias + "</maioria>" + Environment.NewLine);
                    }
                    else
                    {
                        context.Response.Write("<maioria>" + resultado[2] + "</maioria>" + Environment.NewLine);
                    }
                    context.Response.Write("<quantidade>" + resultado[3] + "</quantidade>" + Environment.NewLine);
                    int decimais = Convert.ToInt32((resultado[0] - Math.Truncate(resultado[0])) * 100);
                    resultado[0] = resultado[0] - (resultado[0] - Math.Truncate(resultado[0]));
                    if (decimais > 75)
                    {
                        resultado[0] = resultado[0] + 1;
                    }
                    else if (decimais > 50)
                    {
                        resultado[0] = resultado[0] + Convert.ToDecimal(0.75);
                    }
                    else if (decimais > 25)
                    {
                        resultado[0] = resultado[0] + Convert.ToDecimal(0.5);
                    }
                    else if (decimais > 0)
                    {
                        resultado[0] = resultado[0] + Convert.ToDecimal(0.25);
                    }
                    context.Response.Write("<previsto>" + resultado[0] + "</previsto>" + Environment.NewLine);
                    if (atividade != 0)
                    {
                        if (cadVotacaoAtividade.Mensagem.Length > 5)
                        {
                            context.Response.Write("<mensagem>" + cadVotacaoAtividade.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        if (cadVotacaoTarefa.Mensagem.Length > 5)
                        {
                            context.Response.Write("<mensagem>" + cadVotacaoTarefa.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                        }
                    }
                }
                context.Response.Write("</retorno>");
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