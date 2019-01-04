using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Scrumtics
{
    public class RegistroPacote
    {
        private string connectionString;
        private string mensagem;

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        public RegistroPacote()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SCRUMTICS"].ConnectionString;
            mensagem = "ERROR";
        }

        public long CriaRegistro(string descricao)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            long id;
            object identity;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                      "insert into Pacote (Descricao, Status) " +
                                                      "values ('{0}','V')" + Environment.NewLine +
                                                      "select scope_identity()" + Environment.NewLine +
                                                      "end",
                                                      descricao.TrimEnd().Replace("'", "''")
                                                      ), sqlConn);
                identity = sqlCmd.ExecuteScalar();
                if (string.IsNullOrWhiteSpace(Convert.ToString(identity)))
                {
                    id = 0;
                }
                else
                {
                    id = Convert.ToInt64(identity);
                }
            }
            catch (Exception e)
            {
                mensagem += " | " + e.Message;
                id = 0;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Dispose();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
            }
            return id;
        }

        public string BuscaPacote(long pacote)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            object Descricao;
            string descricao;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select Descricao from Pacote with (nolock) " +
                                                      "where Id = {0}",
                                                      pacote.ToString()
                                                      ), sqlConn);
                Descricao = sqlCmd.ExecuteScalar();
                if (string.IsNullOrWhiteSpace(Convert.ToString(Descricao)))
                {
                    descricao = string.Empty;
                }
                else
                {
                    descricao = Descricao.ToString();
                }
            }
            catch (Exception e)
            {
                mensagem += " | " + e.Message;
                descricao = string.Empty;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Dispose();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
            }
            return descricao;
        }

        public string[] GeraEstatisticaPacote(long pacote, bool previsto)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string[] estatistica;
            List<string> Estatistica = new List<string>();

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select 'Ativ. #' + cast(a1.Id as varchar), u1.Id, u1.Nome, " +
                                                      "a1.Tempo_previsto, isnull(va.Tempo, 0), (a1.Tempo_previsto - isnull(va.Tempo, 0)), " +
                                                      "cast(round((100 * abs(a1.Tempo_previsto - isnull(va.Tempo, 0)) / a1.Tempo_previsto), 2) as numeric(7, 2)), " +
                                                      "a1.Tempo_realizado, (a1.Tempo_realizado - isnull(va.Tempo, 0)), " +
                                                      "cast(round((100 * case when a1.Tempo_realizado = 0 then 1 else abs(a1.Tempo_realizado - isnull(va.Tempo, 0)) / a1.Tempo_realizado end), 2) as numeric(7, 2)) " +
                                                      "from AtividadePacote a1 with(nolock) " +
                                                      "inner join EquipePacote e1 with(nolock) on e1.Id_pacote = a1.Id_pacote " +
                                                      "inner join Usuario u1 on u1.Id = e1.Id_usuario " +
                                                      "left join VotacaoAtividade va with(nolock) on va.Id_atividade = a1.Id and va.Id_usuario = e1.Id_usuario " +
                                                      "where a1.Tempo_previsto <> 0 and a1.Possui_tarefas = 0 and a1.Id_pacote = {0} " + Environment.NewLine +
                                                      "union" + Environment.NewLine +
                                                      "select 'Ativ. #' + cast(a2.Id as varchar) + ' / Tar. #' + cast(t1.Id as varchar), u2.Id, u2.Nome, " +
                                                      "t1.Tempo_previsto, isnull(vt.Tempo, 0), (t1.Tempo_previsto - isnull(vt.Tempo, 0)), " +
                                                      "cast(round((100 * abs(t1.Tempo_previsto - isnull(vt.Tempo, 0)) / t1.Tempo_previsto), 2) as numeric(7, 2)), " +
                                                      "t1.Tempo_realizado, (t1.Tempo_realizado - isnull(vt.Tempo, 0)), " +
                                                      "cast(round((100 * case when t1.Tempo_realizado = 0 then 1 else abs(t1.Tempo_realizado - isnull(vt.Tempo, 0)) / t1.Tempo_realizado end), 2) as numeric(7, 2)) " +
                                                      "from AtividadePacote a2 with(nolock) " +
                                                      "inner join TarefaAtividade t1 with(nolock) on t1.Id_atividade = a2.Id and t1.Tempo_previsto <> 0 " +
                                                      "inner join EquipePacote e2 with(nolock) on e2.Id_pacote = a2.Id_pacote " +
                                                      "inner join Usuario u2 on u2.Id = e2.Id_usuario " +
                                                      "left join VotacaoTarefa vt with(nolock) on vt.Id_tarefa = t1.Id and vt.Id_usuario = e2.Id_usuario " +
                                                      "where a2.Possui_tarefas = 1 and a2.Id_pacote = {0} " + Environment.NewLine +
                                                      "order by 1, {1}, 5, 3",
                                                      pacote.ToString(),
                                                      (previsto ? "7" : "10")
                                                      ), sqlConn);
                sqlRead = sqlCmd.ExecuteReader();
                while (sqlRead.Read())
                {
                    Estatistica.Add(sqlRead.GetString(0));
                    Estatistica.Add(sqlRead.GetDecimal(1).ToString());
                    Estatistica.Add(sqlRead.GetString(2));
                    Estatistica.Add(sqlRead.GetDecimal(3).ToString());
                    Estatistica.Add(sqlRead.GetDecimal(4).ToString());
                    Estatistica.Add(sqlRead.GetDecimal(5).ToString());
                    if (sqlRead.GetDecimal(6) > 100)
                    {
                        Estatistica.Add("0.00");
                        Estatistica.Add("100.00");
                    }
                    else
                    {
                        Estatistica.Add((100 - sqlRead.GetDecimal(6)).ToString());
                        Estatistica.Add(sqlRead.GetDecimal(6).ToString());
                    }
                    Estatistica.Add(sqlRead.GetDecimal(7).ToString());
                    Estatistica.Add(sqlRead.GetDecimal(8).ToString());
                    if (sqlRead.GetDecimal(9) > 100)
                    {
                        Estatistica.Add("0.00");
                        Estatistica.Add("100.00");
                    }
                    else
                    {
                        Estatistica.Add((100 - sqlRead.GetDecimal(9)).ToString());
                        Estatistica.Add(sqlRead.GetDecimal(9).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                mensagem += " | " + e.Message;
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Dispose();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
                if (sqlRead != null)
                {
                    sqlRead.Dispose();
                }
            }
            estatistica = Estatistica.ToArray();
            return estatistica;
        }
    }
}