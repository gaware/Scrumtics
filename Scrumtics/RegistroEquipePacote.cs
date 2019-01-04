using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Scrumtics
{
    public class RegistroEquipePacote
    {
        private string connectionString;
        private string mensagem;

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        public RegistroEquipePacote()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SCRUMTICS"].ConnectionString;
            mensagem = "ERROR";
        }

        public long CriaRegistro(long pacote, long usuario)
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
                                                      "insert into EquipePacote (Id_pacote, Id_usuario) " +
                                                      "values ({0},{1})" + Environment.NewLine +
                                                      "select scope_identity()" + Environment.NewLine +
                                                      "end",
                                                      pacote.ToString(),
                                                      usuario.ToString()
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

        public long ExcluiRegistro(long equipe)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            long id = equipe;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                      "begin transaction" + Environment.NewLine +
                                                      "delete from EquipePacote where Id = {0}" + Environment.NewLine +
                                                      "commit transaction" + Environment.NewLine +
                                                      "end",
                                                      equipe.ToString()
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

        public string[] BuscaEquipe(long pacote)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead = null;
            string[] equipe;
            List<string> Equipe = new List<string>();

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select u1.Login, u1.Nome, u1.Email, 'S' as Vinculado, u1.Id, " +
                                                      "(select e1.Id from EquipePacote e1 with(nolock) " +
                                                      "where e1.Id_pacote = {0} and e1.Id_usuario = u1.Id) as Id_equipe " +
                                                      "from Usuario u1 with(nolock) " +
                                                      "where u1.Id in (select e2.Id_usuario from EquipePacote e2 with(nolock) " +
                                                      "where e2.Id_pacote = {0})" + Environment.NewLine +
                                                      "union all" + Environment.NewLine + 
                                                      "select u2.Login, u2.Nome, u2.Email, 'N' as Vinculado, u2.Id, 0 as Id_equipe " + 
                                                      "from Usuario u2 with(nolock) " +
                                                      "where u2.Id not in (select e3.Id_usuario from EquipePacote e3 with(nolock) " +
                                                      "where e3.Id_pacote = {0})" + Environment.NewLine + 
                                                      "order by 4 desc, 1", 
                                                      pacote.ToString()
                                                      ), sqlConn);
                sqlRead = sqlCmd.ExecuteReader();
                while (sqlRead.Read())
                {
                    Equipe.Add(sqlRead.GetString(0));
                    Equipe.Add(sqlRead.GetString(1));
                    Equipe.Add(sqlRead.GetString(2));
                    Equipe.Add(sqlRead.GetString(3));
                    Equipe.Add(sqlRead.GetDecimal(4).ToString());
                    Equipe.Add(sqlRead.GetDecimal(5).ToString());
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
            equipe = Equipe.ToArray();
            return equipe;
        }
    }
}
