using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace Scrumtics
{
    public class RegistroUsuario
    {
        private string connectionString;
        private string mensagem;

        public string Mensagem
        {
            get { return mensagem; }
            set { mensagem = value; }
        }

        public RegistroUsuario()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SCRUMTICS"].ConnectionString;
            mensagem = "ERROR";
        }

        public static string IniciaisNome(string nome)
        {
            string iniciais;
            Regex expRegIniciais = new Regex(@"(\b[a-zA-Z])[a-zA-Z]* ?");
            iniciais = expRegIniciais.Replace(nome, "$1");
            return iniciais.Substring(0, 1) + iniciais.Substring(iniciais.Length - 1, 1);
        }

        public long CriaRegistro(string login, string senha, string nome, string email, string imagem)
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
                                                      "insert into Usuario (Login, Senha, Nome, Email, Imagem) " + 
                                                      "values ('{0}','{1}','{2}','{3}','{4}')" + Environment.NewLine +
                                                      "select scope_identity()" + Environment.NewLine +
                                                      "end", 
                                                      login.TrimEnd(),
                                                      senha,
                                                      nome.TrimEnd(),
                                                      email.TrimEnd(),
                                                      imagem
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

        public long ModificaRegistro(long usuario, string login, string senha, string nome, string email, string imagem)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            long id = usuario;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("begin" + Environment.NewLine +
                                                      "begin transaction" + Environment.NewLine +
                                                      "update Usuario set Login = '{0}', Senha = '{1}', " +
                                                      "Nome = '{2}', Email = '{3}', Imagem = '{4}' " +
                                                      "where Id = {5}" + Environment.NewLine +
                                                      "commit transaction" + Environment.NewLine +
                                                      "end",
                                                      login.TrimEnd(),
                                                      senha,
                                                      nome.TrimEnd(),
                                                      email.TrimEnd(),
                                                      imagem,
                                                      usuario.ToString()
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

        public long BuscaUsuario(string login, string senha)
        {
            SqlConnection sqlConn = null;
            SqlCommand sqlCmd = null;
            object Id;
            long id;

            try
            {
                sqlConn = new SqlConnection(connectionString);
                sqlConn.Open();
                sqlCmd = new SqlCommand(string.Format("select Id from Usuario with (nolock) " +
                                                      "where Login = '{0}' and Senha = '{1}'",
                                                      login.TrimEnd(), 
                                                      senha
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
        
        public string[] BuscaUsuario(long id)
        {
            SqlConnection sqlConn1 = null;
            SqlConnection sqlConn2 = null;
            SqlCommand sqlCmd = null;
            SqlDataReader sqlRead1 = null;
            SqlDataReader sqlRead2 = null;
            string[] usuario;
            List<string> Usuario = new List<string>();
            string pacotes = string.Empty;

            try
            {
                sqlConn1 = new SqlConnection(connectionString);
                sqlConn1.Open();
                sqlCmd = new SqlCommand(string.Format("select Nome, Email, Imagem " +
                                                      "from Usuario with (nolock) " +
                                                      "where Id = {0}",
                                                      id.ToString()
                                                      ), sqlConn1);
                sqlRead1 = sqlCmd.ExecuteReader();
                while (sqlRead1.Read())
                {
                    Usuario.Add(sqlRead1.GetString(0));
                    Usuario.Add(sqlRead1.GetString(1));
                    Usuario.Add(sqlRead1.GetString(2));
                }
                sqlConn2 = new SqlConnection(connectionString);
                sqlConn2.Open();
                sqlCmd = new SqlCommand(string.Format("select Id_pacote " +
                                                      "from EquipePacote with (nolock) " +
                                                      "where Id_usuario = {0}",
                                                      id.ToString()
                                                      ), sqlConn2);
                sqlRead2 = sqlCmd.ExecuteReader();
                while (sqlRead2.Read())
                {
                    pacotes += (string.IsNullOrWhiteSpace(pacotes) ? 
                        sqlRead2.GetDecimal(0).ToString() : " / " + sqlRead2.GetDecimal(0).ToString());
                }
                Usuario.Add(pacotes);
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
            usuario = Usuario.ToArray();
            return usuario;
        }
    }
}