using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Scrumtics
{
    /// <summary>
    /// Summary description for pacote_cadastro
    /// </summary>
    public class pacote_cadastro : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            RegistroPacote cadPacote = new RegistroPacote();

            if (context.Request.Form["modo"] == "C")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                context.Response.Write("<id>" + 
                                        cadPacote.CriaRegistro(
                                            context.Request.Form["descricao"]
                                            ) + "</id>" + Environment.NewLine);
                if (cadPacote.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadPacote.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "B")
            {
                context.Response.ContentType = "text/xml";
                context.Response.Write("<?xml version=\"1.0\"?>" + Environment.NewLine);
                context.Response.Write("<retorno>" + Environment.NewLine);
                context.Response.Write("<descricao>" +
                                       cadPacote.BuscaPacote(
                                           Convert.ToInt64(context.Request.Form["id"])
                                           ).Replace("<", "&lt;").Replace(">", "&gt;") + "</descricao>" + Environment.NewLine);
                if (cadPacote.Mensagem.Length > 5)
                {
                    context.Response.Write("<mensagem>" + cadPacote.Mensagem.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;") + "</mensagem>" + Environment.NewLine);
                }
                context.Response.Write("</retorno>");
            }
            else if (context.Request.Form["modo"] == "G")
            {
                context.Response.ContentType = "text/html";
                bool previsto = context.Request.Form["previsto"] == "S";
                string[] estatistica = cadPacote.GeraEstatisticaPacote(
                                           Convert.ToInt64(context.Request.Form["id"]),
                                           previsto
                                           );
                string atividadeTarefa = string.Empty;
                string diferenca = string.Empty;
                Dictionary<string, string> UsuarioSoma = new Dictionary<string, string>();
                Dictionary<string, string> UsuarioCor = new Dictionary<string, string>();
                SortedDictionary<string, string> UsuarioRanking = new SortedDictionary<string, string>();
                int cor = (previsto ? 0 : 6);
                string[] cores =
                {
                    "darksalmon",
                    "darkolivegreen",
                    "darkslateblue",
                    "darkgoldenrod",
                    "darkkhaki",
                    "darkgray",
                    "darkred",
                    "darkgreen",
                    "darkblue",
                    "darkorange",
                    "darkcyan",
                    "darkorchid"
                };
                string estatisticas = string.Empty;
                string usuarios = string.Empty;
                for (int i = 11; i < estatistica.Length; i = i + 12)
                {
                    if (atividadeTarefa != estatistica[i - 11] || diferenca == estatistica[i - (previsto ? 4 : 0)])
                    {
                        if (atividadeTarefa != estatistica[i - 11] && !string.IsNullOrWhiteSpace(atividadeTarefa))
                        {
                            estatisticas += "<tr>" + Environment.NewLine +
                                           @"<td colspan=""4"">&nbsp;</td>" + Environment.NewLine +
                                            "</tr>" + Environment.NewLine;
                        }
                        atividadeTarefa = estatistica[i - 11];
                        diferenca = estatistica[i - (previsto ? 4 : 0)];
                        if (!UsuarioSoma.ContainsKey(estatistica[i - 10]))
                        {
                            UsuarioSoma.Add(estatistica[i - 10], "1;" + (i - 9));
                        }
                        else
                        {
                            UsuarioSoma[estatistica[i - 10]] = 
                                (Convert.ToInt32(UsuarioSoma[estatistica[i - 10]].Split(';')[0]) + 1) + ";" +
                                UsuarioSoma[estatistica[i - 10]].Split(';')[1];
                        }
                    }
                    if (!UsuarioCor.ContainsKey(estatistica[i - 10]))
                    {
                        UsuarioCor.Add(estatistica[i - 10], cores[cor]);
                        cor++;
                        if (cor == 12)
                        {
                            cor = 0;
                        }
                    }
                    estatisticas += "<tr>" + Environment.NewLine +
                                    "<td>" + atividadeTarefa + "</td>" + Environment.NewLine +
                                    "<td>" + RegistroUsuario.IniciaisNome(estatistica[i - 9]) + "</td>" + Environment.NewLine +
                                   @"<td style=""width:66%"">" + Environment.NewLine +
                                   @"<div class=""progress"">" + Environment.NewLine +
                                   @"<div class=""progress-bar"" style=""background-image:none;background-color:" + 
                                    UsuarioCor[estatistica[i - 10]] + ";width:" +
                                    estatistica[i - (previsto ? 5 : 1)].Replace(",",".") + @"%""><span>" +
                                    estatistica[i - 7] + 
                                    "</span></div>" + Environment.NewLine +
                                   @"<div class=""progress-bar progress-bar-striped"" style=""width:" +
                                    estatistica[i - (previsto ? 4 : 0)].Replace(",", ".") + @"%""><span>" + 
                                    estatistica[i - (previsto ? 6 : 2)] +
                                    "</span></div>" + Environment.NewLine +
                                    "</div>" + Environment.NewLine +
                                    "</td>" + Environment.NewLine +
                                    "<td>" + estatistica[i - (previsto ? 8 : 3)] + "</td>" + Environment.NewLine +
                                    "</tr>" + Environment.NewLine;
                }
                foreach (KeyValuePair<string, string> kvp in UsuarioSoma)
                {
                    if (!UsuarioRanking.ContainsKey(kvp.Value.Split(';')[0]))
                    {
                        UsuarioRanking.Add(kvp.Value.Split(';')[0],
                            estatistica[Convert.ToInt32(kvp.Value.Split(';')[1])]
                            );
                    }
                    else
                    {
                        UsuarioRanking[kvp.Value.Split(';')[0]] += " / " +
                            estatistica[Convert.ToInt32(kvp.Value.Split(';')[1])];
                    }
                }
                foreach (KeyValuePair<string, string> kvp in UsuarioRanking)
                {
                    usuarios = "<tr>" + Environment.NewLine +
                               "<td>" + kvp.Value + "</td>" + Environment.NewLine +
                               "<td>" + kvp.Key + "</td>" + Environment.NewLine +
                               "</tr>" + Environment.NewLine + 
                               usuarios; 
                }
                context.Response.Write("<!-- estatisticas; -->" + Environment.NewLine);
                context.Response.Write(estatisticas);
                context.Response.Write("<!-- usuarios; -->" + Environment.NewLine);
                context.Response.Write(usuarios);
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