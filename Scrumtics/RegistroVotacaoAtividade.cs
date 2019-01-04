using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Scrumtics
{
    public class RegistroVotacaoAtividade
    {
        private string connectionString;
        private string mensagem;

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        public RegistroVotacaoAtividade()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SCRUMTICS"].ConnectionString;
            mensagem = "ERROR";
        }

        public long CriaRegistro(long atividade, long usuario, long tempo)
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
                                                      "insert into VotacaoAtividade (Id_atividade, Id_usuario, Tempo) " +
                                                      "values ({0},{1},{2})" + Environment.NewLine +
                                                      "select scope_identity()" + Environment.NewLine +
                                                      "end",
                                                       atividade.ToString(),
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

        public long ExcluiRegistro(long atividade)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            long id = atividade;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                      "begin transaction" + Environment.NewLine +
                                                      "delete from VotacaoAtividade where Id_atividade = {0}" + Environment.NewLine +
                                                      "commit transaction" + Environment.NewLine +
                                                      "end",
                                                      atividade.ToString()
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

        public long BuscaVotacaoAtividade(long atividade, long usuario)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            object Id;
            long id;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select Id from VotacaoAtividade with (nolock) " +
                                                      "where Id_atividade = {0} and Id_usuario = {1}",
                                                      atividade.ToString(),
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

        public string VerificaVotacaoAtividade(long atividade, long usuario)
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
                                                      "'a' + cast(a.Id as varchar) as Votacao " +
                                                      "from AtividadePacote a with(nolock) " +
                                                      "inner join EquipePacote e with(nolock) on e.Id_pacote = a.Id_pacote and e.Id_usuario = {0} " +
                                                      "where a.Status = '0' and a.Tempo_previsto = 0 and a.Possui_tarefas = 0 and a.Id = {1}",
                                                      usuario.ToString(),
                                                      atividade.ToString()
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

        public decimal[] GeraResultadoVotacaoAtividade(long atividade)
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
                                                      "from VotacaoAtividade with (nolock) where Id_atividade = {0}",
                                                      atividade.ToString()
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
                                                      "from VotacaoAtividade with (nolock) where Id_atividade = {0} " +
                                                      "group by Tempo order by 2 desc, Tempo",
                                                      atividade.ToString()
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

        public string VerificaPacote(long pacote, long usuario, long atividade, long tarefa)
        {
            SqlConnection sqlConn1 = null;
            SqlConnection sqlConn2 = null;
            SqlCommand sqlCmd = null;
            object Retorno;
            string retorno;

            try
            {
                sqlConn1 = new SqlConnection(connectionString);
                sqlConn1.Open();
                sqlCmd = new SqlCommand(string.Format("select 'p' + cast(a1.Id_pacote as varchar) + " +
                                                      "'a' + cast(a1.Id as varchar) as Votacao " +
                                                      "from AtividadePacote a1 with(nolock) " +
                                                      "inner join EquipePacote e1 with(nolock) on e1.Id_pacote = a1.Id_pacote and e1.Id_usuario = {0} " +
                                                      "where a1.Status = '0' and a1.Tempo_previsto = 0 and a1.Possui_tarefas = 0 and a1.Id <> {1} and a1.Id_pacote = {2} " +
                                                      "and not exists (select 1 from VotacaoAtividade va with(nolock) where va.Id_atividade = a1.Id and va.Id_usuario = {0})" +Environment.NewLine +
                                                      "union" + Environment.NewLine +
                                                      "select 'p' + cast(a2.Id_pacote as varchar) + " +
                                                      "'a' + cast(t1.Id_atividade as varchar) + 't' + cast(t1.Id as varchar) as Votacao " +
                                                      "from AtividadePacote a2 with(nolock) " +
                                                      "inner join TarefaAtividade t1 with(nolock) on t1.Id_atividade = a2.Id and t1.Status = '0' and t1.Tempo_previsto = 0 and t1.Id <> {3} " +
                                                      "inner join EquipePacote e2 with(nolock) on e2.Id_pacote = a2.Id_pacote and e2.Id_usuario = {0} " +
                                                      "where a2.Possui_tarefas = 1 and a2.Id_pacote = {2} " +
                                                      "and not exists (select 1 from VotacaoTarefa vt with(nolock) where vt.Id_tarefa = t1.Id and vt.Id_usuario = {0})",
                                                      usuario.ToString(),
                                                      atividade.ToString(),
                                                      pacote.ToString(),
                                                      tarefa.ToString()
                                                      ), sqlConn1);
                Retorno = sqlCmd.ExecuteScalar();
                if (string.IsNullOrWhiteSpace(Convert.ToString(Retorno)))
                {
                    sqlConn2 = new SqlConnection(connectionString);
                    sqlConn2.Open();
                    sqlCmd = new SqlCommand(string.Format("select p.Status from Pacote p with(nolock) where p.Id = {0}",
                                                          pacote.ToString()
                                                          ), sqlConn2);
                    Retorno = sqlCmd.ExecuteScalar();
                }
                if (string.IsNullOrWhiteSpace(Convert.ToString(Retorno)))
                {
                    retorno = string.Empty;
                }
                else
                {
                    retorno = Retorno.ToString();
                }
            }
            catch (Exception e)
            {
                mensagem += " | " + e.Message;
                retorno = string.Empty;
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
                if (sqlConn2 != null)
                {
                    sqlConn2.Dispose();
                }
            }
            return retorno;
        }
    }
}