using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Scrumtics
{
    public class RegistroTarefaAtividade
    {
        private string connectionString;
        private string mensagem;

        public enum Status
        {
            Votação = 0,
            Pendente = 1,
            Iniciado = 2,
            Teste = 3,
            Concluído = 4,
            Impedimento = 5,
            Removido = 6
        }

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        public RegistroTarefaAtividade()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SCRUMTICS"].ConnectionString;
            mensagem = "ERROR";
        }

        public static string DescricaoStatus(string status)
        {
            try
            {
                switch (Convert.ToInt32(status))
                {
                    case (int)Status.Pendente:
                        return Status.Pendente.ToString();
                    case (int)Status.Iniciado:
                        return Status.Iniciado.ToString();
                    case (int)RegistroAtividadePacote.Status.Teste:
                        return Status.Teste.ToString();
                    case (int)RegistroAtividadePacote.Status.Concluído:
                        return Status.Concluído.ToString();
                    case (int)RegistroAtividadePacote.Status.Impedimento:
                        return Status.Impedimento.ToString();
                    case (int)RegistroAtividadePacote.Status.Removido:
                        return Status.Removido.ToString();
                    default:
                        return Status.Votação.ToString();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public long CriaRegistro(long atividade, string descricao)
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
                                                      "insert into TarefaAtividade (Id_atividade, Descricao, Tempo_previsto, Tempo_realizado, Status) " +
                                                      "values ({0},'{1}',0,0,'{2}')" + Environment.NewLine +
                                                      "update AtividadePacote set Possui_tarefas = " +
                                                      "isnull((select min(1) from TarefaAtividade t " + 
                                                      "where t.Id_atividade = {0}), 0) where Id = {0}" + Environment.NewLine + 
                                                      "select scope_identity()" + Environment.NewLine +
                                                      "end",
                                                       atividade.ToString(),
                                                       descricao.TrimEnd().Replace("'", "''"),
                                                       ((int)Status.Pendente).ToString()
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

        public long ModificaRegistro(long tarefa, string status, string descricao, decimal previsto, decimal realizado,
                                     long atividade, long pacote)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            object Id;
            long id = tarefa;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                if (status != ((int)Status.Votação).ToString() && !string.IsNullOrWhiteSpace(descricao))
                {
                    sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                          "begin transaction" + Environment.NewLine +
                                                          "update TarefaAtividade set Status = '{0}', Descricao = '{1}', " +
                                                          "Tempo_realizado = {2} where Id = {3}" + Environment.NewLine +
                                                          "update AtividadePacote set Tempo_realizado = " +
                                                          "(select sum(t.Tempo_realizado) " +
                                                          "from TarefaAtividade t with (nolock) " +
                                                          "where t.Id_atividade = {4}) " +
                                                          "where Id = {4}" + Environment.NewLine +
                                                          "commit transaction" + Environment.NewLine +
                                                          "end",
                                                          status.TrimEnd(),
                                                          descricao.TrimEnd().Replace("'", "''"),
                                                          realizado.ToString().Replace(",", "."),
                                                          tarefa.ToString(),
                                                          atividade.ToString()
                                                          ), sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }
                else if (!string.IsNullOrWhiteSpace(status))
                {
                    if (status == ((int)Status.Votação).ToString())
                    {
                        sqlCmd = new SqlCommand(string.Format("select top 1 Id from TarefaAtividade with (nolock) " +
                                                              "where Id <> {0} and Id_atividade = {1} and Status = '0'",
                                                              tarefa.ToString(),
                                                              atividade.ToString()
                                                              ), sqlConn);
                        Id = sqlCmd.ExecuteScalar();
                    }
                    else
                    {
                        Id = string.Empty;
                    }
                    if (string.IsNullOrWhiteSpace(Convert.ToString(Id)))
                    {
                        sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                              "begin transaction" + Environment.NewLine +
                                                              "update TarefaAtividade set Status = '{0}' " +
                                                              "where Id = {1}" + Environment.NewLine +
                                                              "commit transaction" + Environment.NewLine +
                                                              "end",
                                                              status.TrimEnd(),
                                                              tarefa.ToString()
                                                              ), sqlConn);
                        sqlCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        id = 0;
                    }
                }
                else
                {
                    sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                          "begin transaction" + Environment.NewLine +
                                                          "update TarefaAtividade set Tempo_previsto = {0} " +
                                                          "where Id = {1}" + Environment.NewLine +
                                                          "update AtividadePacote set Tempo_previsto = " +
                                                          "(select sum(t.Tempo_previsto) " + 
                                                          "from TarefaAtividade t with (nolock) " +
                                                          "where t.Id_atividade = {2}) " +
                                                          "where Id = {2}" + Environment.NewLine + 
                                                          "update Pacote set Status = 'L' " +
                                                          "where Id not in (" + Environment.NewLine +
                                                          "select p1.Id from Pacote p1 " +
                                                          "inner join AtividadePacote a1 with(nolock) " +
                                                          "on a1.Id_pacote = p1.Id and a1.Possui_tarefas = 1 " +
                                                          "inner join TarefaAtividade t1 with(nolock) " +
                                                          "on t1.Id_atividade = a1.Id and t1.Tempo_previsto = 0 " +
                                                          "where p1.Id = {3}" + Environment.NewLine +
                                                          "union" + Environment.NewLine +
                                                          "select p2.Id from Pacote p2 " +
                                                          "inner join AtividadePacote a2 with(nolock)" +
                                                          "on a2.Id_pacote = p2.Id and a2.Tempo_previsto = 0 and a2.Possui_tarefas = 0 " +
                                                          "where p2.Id = {3}" + Environment.NewLine +
                                                          ")" + Environment.NewLine +
                                                          "and Id = {3}" + Environment.NewLine +
                                                          "commit transaction" + Environment.NewLine +
                                                          "end",
                                                          previsto.ToString().Replace(",", "."),
                                                          tarefa.ToString(),
                                                          atividade.ToString(),
                                                          pacote.ToString()
                                                          ), sqlConn);
                    sqlCmd.ExecuteNonQuery();
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

        public long ExcluiRegistro(long pacote, long atividade, long tarefa)
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
                                                      "delete from TarefaAtividade where Id = {0}" + Environment.NewLine +
                                                      "update AtividadePacote set Possui_tarefas = " +
                                                      "isnull((select min(1) from TarefaAtividade t " +
                                                      "where t.Id_atividade = {1}), 0) where Id = {1}" + Environment.NewLine +
                                                      "update Pacote set Status = 'L' " +
                                                      "where Id not in (" + Environment.NewLine +
                                                      "select p1.Id from Pacote p1 " +
                                                      "inner join AtividadePacote a1 with(nolock) " +
                                                      "on a1.Id_pacote = p1.Id and a1.Possui_tarefas = 1 " +
                                                      "inner join TarefaAtividade t1 with(nolock) " +
                                                      "on t1.Id_atividade = a1.Id and t1.Tempo_previsto = 0 " +
                                                      "where p1.Id = {2}" + Environment.NewLine +
                                                      "union" + Environment.NewLine +
                                                      "select p2.Id from Pacote p2 " +
                                                      "inner join AtividadePacote a2 with(nolock)" +
                                                      "on a2.Id_pacote = p2.Id and a2.Tempo_previsto = 0 and a2.Possui_tarefas = 0 " +
                                                      "where p2.Id = {2}" + Environment.NewLine +
                                                      ")" + Environment.NewLine +
                                                      "and Id = {2}" + Environment.NewLine +
                                                      "commit transaction" + Environment.NewLine +
                                                      "end",
                                                      tarefa.ToString(),
                                                      atividade.ToString(),
                                                      pacote.ToString()
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

        public string[] BuscaTarefa(long pacote, long usuario, long atividade)
        {
            SqlConnection sqlConn1 = null;
            SqlConnection sqlConn2 = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead1 = null;
            SqlDataReader sqlRead2 = null;
            string[] tarefa;
            List<string> Tarefa = new List<string>();

            try
            {
                sqlConn1 = new SqlConnection(connectionString);
                sqlConn1.Open();
                sqlCmd = new SqlCommand(string.Format("select " +
                                                      "isnull((select e.Id_usuario from EquipePacote e with(nolock) " +
                                                      "where e.Id_pacote = {0} and e.Id_usuario = {1}), 0) as Id_usuario, " +
                                                      "(select e.Id_usuario from EquipePacote e with(nolock) where e.Id = " +
                                                      "(select min(e.Id) from EquipePacote e with(nolock) " +
                                                      "where e.Id_pacote = {0})) as Scrum_master, " +
                                                      "p.Status " +
                                                      "from Pacote p with(nolock) where p.Id = {0}",
                                                      pacote.ToString(),
                                                      usuario.ToString()
                                                      ), sqlConn1);
                sqlRead1 = sqlCmd.ExecuteReader();
                while (sqlRead1.Read())
                {
                    if (Tarefa.Count == 0)
                    {
                        Tarefa.Add(sqlRead1.GetDecimal(1).ToString());
                        Tarefa.Add(sqlRead1.GetString(2));
                    }
                    if (sqlRead1.GetDecimal(0) == usuario)
                    {
                        sqlConn2 = new SqlConnection(connectionString);
                        sqlConn2.Open();
                        sqlCmd = new SqlCommand(string.Format("select Status, Descricao, Tempo_previsto, Tempo_realizado, " +
                                                              "Id from TarefaAtividade with (nolock) " +
                                                              "where Id_atividade = {0}" + Environment.NewLine +
                                                              "order by 1",
                                                              atividade.ToString()
                                                              ), sqlConn2);
                        sqlRead2 = sqlCmd.ExecuteReader();
                        while (sqlRead2.Read())
                        {
                            Tarefa.Add(sqlRead2.GetString(0));
                            Tarefa.Add(sqlRead2.GetString(1));
                            Tarefa.Add(sqlRead2.GetDecimal(2).ToString());
                            Tarefa.Add(sqlRead2.GetDecimal(3).ToString());
                            Tarefa.Add(sqlRead2.GetDecimal(4).ToString());
                        }
                        if (Tarefa.Count == 2)
                        {
                            Tarefa.Add(string.Empty);
                            Tarefa.Add("Nenhuma tarefa criada.");
                            Tarefa.Add(string.Empty);
                            Tarefa.Add(string.Empty);
                            Tarefa.Add(string.Empty);
                        }
                    }
                }
                if (Tarefa.Count == 0)
                {
                    Tarefa.Add("0");
                    Tarefa.Add(" ");
                }
            }
            catch (Exception e)
            {
                mensagem += " | " + e.Message;
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
            tarefa = Tarefa.ToArray();
            return tarefa;
        }

        public string[] BuscaTarefa(long id)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string[] tarefa;
            List<string> Tarefa = new List<string>();

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select Status, Descricao, Tempo_realizado " +
                                                      "from TarefaAtividade with (nolock) " +
                                                      "where Id = {0}",
                                                      id.ToString()
                                                      ), sqlConn);
                sqlRead = sqlCmd.ExecuteReader();
                while (sqlRead.Read())
                {
                    Tarefa.Add(sqlRead.GetString(0));
                    Tarefa.Add(sqlRead.GetString(1));
                    Tarefa.Add(sqlRead.GetDecimal(2).ToString());
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
            tarefa = Tarefa.ToArray();
            return tarefa;
        }
    }
}
