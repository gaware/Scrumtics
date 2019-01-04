using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Scrumtics
{
    public class RegistroAtividadePacote
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

        public RegistroAtividadePacote()
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
        
        public long CriaRegistro(long pacote, string descricao)
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
                                                      "insert into AtividadePacote (Id_pacote, Descricao, Tempo_previsto, Tempo_realizado, Possui_tarefas, Status) " +
                                                      "values ({0},'{1}',0,0,0,'{2}')" + Environment.NewLine +
                                                      "select scope_identity()" + Environment.NewLine +
                                                      "end",
                                                       pacote.ToString(),
                                                       descricao.TrimEnd().Replace("'","''"),
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

        public long ModificaRegistro(long atividade, string status, string descricao, decimal previsto, decimal realizado,
                                     long pacote)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            object Id;
            long id = atividade;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                if (status != ((int)Status.Votação).ToString() && !string.IsNullOrWhiteSpace(descricao))
                {
                    sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                          "begin transaction" + Environment.NewLine +
                                                          "update AtividadePacote set Status = '{0}', Descricao = '{1}', " +
                                                          "Tempo_realizado = {2} where Id = {3}" + Environment.NewLine +
                                                          "commit transaction" + Environment.NewLine +
                                                          "end",
                                                          status.TrimEnd(),
                                                          descricao.TrimEnd().Replace("'", "''"),
                                                          realizado.ToString().Replace(",", "."),
                                                          atividade.ToString()
                                                          ), sqlConn);
                    sqlCmd.ExecuteNonQuery();
                }
                else if (!string.IsNullOrWhiteSpace(status))
                {
                    if (status == ((int)Status.Votação).ToString())
                    {
                        sqlCmd = new SqlCommand(string.Format("select top 1 Id from AtividadePacote with (nolock) " +
                                                              "where Id <> {0} and Id_pacote = {1} and Status = '0'",
                                                              atividade.ToString(),
                                                              pacote.ToString()
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
                                                              "update AtividadePacote set Status = '{0}' " +
                                                              "where Id = {1}" + Environment.NewLine +
                                                              "commit transaction" + Environment.NewLine +
                                                              "end",
                                                              status.TrimEnd(),
                                                              atividade.ToString()
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
                                                          "update AtividadePacote set Tempo_previsto = {0} " + 
                                                          "where Id = {1}" + Environment.NewLine +
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
                                                          "inner join AtividadePacote a2 with(nolock) " +
                                                          "on a2.Id_pacote = p2.Id and a2.Tempo_previsto = 0 and a2.Possui_tarefas = 0 " +
                                                          "where p2.Id = {2}" + Environment.NewLine +
                                                          ")" + Environment.NewLine +
                                                          "and Id = {2}" + Environment.NewLine +
                                                          "commit transaction" + Environment.NewLine +
                                                          "end",
                                                          previsto.ToString().Replace(",", "."),
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

        public long ExcluiRegistro(long pacote, long atividade)
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
                                                      "delete from AtividadePacote where Id = {0}" + Environment.NewLine +
                                                      "update Pacote set Status = 'L' " +
                                                      "where Id not in (" + Environment.NewLine +
                                                      "select p1.Id from Pacote p1 " +
                                                      "inner join AtividadePacote a1 with(nolock) " +
                                                      "on a1.Id_pacote = p1.Id and a1.Possui_tarefas = 1 " +
                                                      "inner join TarefaAtividade t1 with(nolock) " +
                                                      "on t1.Id_atividade = a1.Id and t1.Tempo_previsto = 0 " +
                                                      "where p1.Id = {1}" + Environment.NewLine +
                                                      "union" + Environment.NewLine +
                                                      "select p2.Id from Pacote p2 " +
                                                      "inner join AtividadePacote a2 with(nolock) " +
                                                      "on a2.Id_pacote = p2.Id and a2.Tempo_previsto = 0 and a2.Possui_tarefas = 0 " +
                                                      "where p2.Id = {1}" + Environment.NewLine +
                                                      ")" + Environment.NewLine +
                                                      "and Id = {1} " +
                                                      "and exists(select 1 from AtividadePacote a3 with(nolock) where a3.Id_pacote = {1})" + Environment.NewLine +
                                                      "commit transaction" + Environment.NewLine +
                                                      "end",
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

        public string[] BuscaAtividade(long pacote, long usuario)
        {
            SqlConnection sqlConn1 = null;
            SqlConnection sqlConn2 = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead1 = null;
            SqlDataReader sqlRead2 = null;
            string[] atividade;
            List<string> Atividade = new List<string>();

            try
            {
                sqlConn1 = new SqlConnection(connectionString);
                sqlConn1.Open();
                sqlCmd = new SqlCommand(string.Format("select " +
                                                      "isnull((select e1.Id_usuario from EquipePacote e1 with(nolock) " +
                                                      "where e1.Id_pacote = {0} and e1.Id_usuario = {1}), 0) as Id_usuario, " +
                                                      "(select e2.Id_usuario from EquipePacote e2 with(nolock) where e2.Id = " +
                                                      "(select min(e3.Id) from EquipePacote e3 with(nolock) " +
                                                      "where e3.Id_pacote = {0})) as Scrum_master, " +
                                                      "p.Status " +
                                                      "from Pacote p with(nolock) where p.Id = {0}",
                                                      pacote.ToString(),
                                                      usuario.ToString()
                                                      ), sqlConn1);
                sqlRead1 = sqlCmd.ExecuteReader();
                while (sqlRead1.Read())
                {
                    if (Atividade.Count == 0)
                    {
                        Atividade.Add(sqlRead1.GetDecimal(1).ToString());
                        Atividade.Add(sqlRead1.GetString(2));
                    }
                    if (sqlRead1.GetDecimal(0) == usuario)
                    {
                        sqlConn2 = new SqlConnection(connectionString);
                        sqlConn2.Open();
                        sqlCmd = new SqlCommand(string.Format("select Status, Descricao, Tempo_previsto, Tempo_realizado, " +
                                                              "Possui_tarefas, Id from AtividadePacote with (nolock) " +
                                                              "where Id_pacote = {0}" + Environment.NewLine +
                                                              "order by 1",
                                                              pacote.ToString()
                                                              ), sqlConn2);
                        sqlRead2 = sqlCmd.ExecuteReader();
                        while (sqlRead2.Read())
                        {
                            Atividade.Add(sqlRead2.GetString(0));
                            Atividade.Add(sqlRead2.GetString(1));
                            Atividade.Add(sqlRead2.GetDecimal(2).ToString());
                            Atividade.Add(sqlRead2.GetDecimal(3).ToString());
                            Atividade.Add(sqlRead2.GetBoolean(4).ToString());
                            Atividade.Add(sqlRead2.GetDecimal(5).ToString());
                        }
                        if (Atividade.Count == 2)
                        {
                            Atividade.Add(string.Empty);
                            Atividade.Add("Nenhuma atividade criada.");
                            Atividade.Add(string.Empty);
                            Atividade.Add(string.Empty);
                            Atividade.Add(string.Empty);
                            Atividade.Add(string.Empty);
                        }
                    }
                }
                if (Atividade.Count == 0)
                {
                    Atividade.Add("0");
                    Atividade.Add(" ");
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
            atividade = Atividade.ToArray();
            return atividade;
        }

        public string[] BuscaAtividade(long id)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string[] atividade;
            List<string> Atividade = new List<string>();

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select Status, Descricao, Tempo_realizado, Possui_tarefas " +
                                                      "from AtividadePacote with (nolock) " +
                                                      "where Id = {0}",
                                                      id.ToString()
                                                      ), sqlConn);
                sqlRead = sqlCmd.ExecuteReader();
                while (sqlRead.Read())
                {
                    Atividade.Add(sqlRead.GetString(0));
                    Atividade.Add(sqlRead.GetString(1));
                    Atividade.Add(sqlRead.GetDecimal(2).ToString());
                    Atividade.Add(sqlRead.GetBoolean(3).ToString());
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
            atividade = Atividade.ToArray();
            return atividade;
        }
    }
}
