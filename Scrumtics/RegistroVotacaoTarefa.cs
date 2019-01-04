using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Scrumtics
{
    public class RegistroVotacaoTarefa
    {
        private string connectionString;
        private string mensagem;

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        public RegistroVotacaoTarefa()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SCRUMTICS"].ConnectionString;
            mensagem = "ERROR";
        }

        public long CriaRegistro(long tarefa, long usuario, long tempo)
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
                                                      "insert into VotacaoTarefa (Id_tarefa, Id_usuario, Tempo) " +
                                                      "values ({0},{1},{2})" + Environment.NewLine +
                                                      "select scope_identity()" + Environment.NewLine +
                                                      "end",
                                                       tarefa.ToString(),
                                                       usuario.ToString(),
                                                       tempo.ToString()
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

        public long ExcluiRegistro(long tarefa)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            long id = tarefa;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                      "begin transaction" + Environment.NewLine +
                                                      "delete from VotacaoTarefa where Id_tarefa = {0}" + Environment.NewLine +
                                                      "commit transaction" + Environment.NewLine +
                                                      "end",
                                                      tarefa.ToString()
                                                      ), sqlConn);
                sqlCmd.ExecuteNonQuery();
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

        public long BuscaVotacaoTarefa(long tarefa, long usuario)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            object Id;
            long id;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select Id from VotacaoTarefa with (nolock) " +
                                                      "where Id_tarefa = {0} and Id_usuario = {1}",
                                                      tarefa.ToString(),
                                                      usuario.ToString()
                                                      ), sqlConn);
                Id = sqlCmd.ExecuteScalar();
                if (string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                {
                    id = 0;
                }
                else
                {
                    id = Convert.ToInt64(Id);
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

        public string VerificaVotacaoTarefa(long tarefa, long usuario)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            object Votacao;
            string _votacao;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();

                sqlCmd = new SqlCommand(string.Format("select 'p' + cast(a.Id_pacote as varchar) + " +
                                                      "'a' + cast(t.Id_atividade as varchar) + 't' + cast(t.Id as varchar) as Votacao " +
                                                      "from AtividadePacote a with(nolock) " +
                                                      "inner join TarefaAtividade t with(nolock) on t.Id_atividade = a.Id and t.Status = '0' and t.Tempo_previsto = 0 and t.Id = {0} " +
                                                      "inner join EquipePacote e with(nolock) on e.Id_pacote = a.Id_pacote and e.Id_usuario = {1} " +
                                                      "where a.Possui_tarefas = 1",
                                                      tarefa.ToString(),
                                                      usuario.ToString()
                                                      ), sqlConn);
                Votacao = sqlCmd.ExecuteScalar();
                if (string.IsNullOrWhiteSpace(Convert.ToString(Votacao)))
                {
                    _votacao = string.Empty;
                }
                else
                {
                    _votacao = Votacao.ToString();
                }
            }
            catch (Exception e)
            {
                mensagem += " | " + e.Message;
                _votacao = string.Empty;
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
            return _votacao;
        }

        public decimal[] GeraResultadoVotacaoTarefa(long tarefa)
        {
            SqlConnection sqlConn1 = null;
            SqlConnection sqlConn2 = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead1 = null;
            SqlDataReader sqlRead2 = null;
            decimal[] resultado;
            List<decimal> Resultado = new List<decimal>();

            try
            {
                sqlConn1 = new SqlConnection(connectionString);
                sqlConn1.Open();
                sqlCmd = new SqlCommand(string.Format("select cast(isnull(avg(Tempo), 0) as numeric(5, 2)) as Media, count(*) as Total " + 
                                                      "from VotacaoTarefa with (nolock) where Id_tarefa = {0}",
                                                      tarefa.ToString()
                                                      ), sqlConn1);
                sqlRead1 = sqlCmd.ExecuteReader();
                while (sqlRead1.Read())
                {
                    Resultado.Add(sqlRead1.GetDecimal(0));
                    Resultado.Add(sqlRead1.GetInt32(1));
                }
                sqlConn2 = new SqlConnection(connectionString);
                sqlConn2.Open();
                sqlCmd = new SqlCommand(string.Format("select Tempo, count(*) as Quantidade " +
                                                      "from VotacaoTarefa with (nolock) where Id_tarefa = {0} " +
                                                      "group by Tempo order by 2 desc, Tempo",
                                                      tarefa.ToString()
                                                      ), sqlConn2);
                sqlRead2 = sqlCmd.ExecuteReader();
                while (sqlRead2.Read())
                {
                    if (Resultado.Count == 2 || Resultado[3] == sqlRead2.GetInt32(1))
                    {
                        Resultado.Add(sqlRead2.GetDecimal(0));
                        if (Resultado.Count == 3)
                        {
                            Resultado.Add(sqlRead2.GetInt32(1));
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (Resultado.Count == 2)
                {
                    Resultado.Add(0);
                    Resultado.Add(0);
                }
            }
            catch (Exception e)
            {
                mensagem += " | " + e.Message;
                Resultado.Clear();
                Resultado.Add(0);
                Resultado.Add(0);
                Resultado.Add(0);
                Resultado.Add(0);
            }
            finally
            {
                if (sqlConn1 != null)
                {
                    sqlConn1.Dispose();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
                if (sqlRead1 != null)
                {
                    sqlRead1.Dispose();
                }
                if (sqlConn2 != null)
                {
                    sqlConn2.Dispose();
                }
                if (sqlRead2 != null)
                {
                    sqlRead2.Dispose();
                }
            }
            resultado = Resultado.ToArray();
            return resultado;
        }
    }
}