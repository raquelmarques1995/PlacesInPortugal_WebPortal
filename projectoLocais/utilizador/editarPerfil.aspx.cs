using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace projectoLocais.utilizador
{
    public partial class editarPerfil : System.Web.UI.Page
    {
        string connectionString = @"data source=.\sqlexpress; initial catalog=Locais; integrated security=true;";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 1 - Ler dados do utilizador
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT Ut.Nome, Ut.Email, Us.UserName FROM Utilizador Ut JOIN Users Us ON Ut.Id = Us.UserId WHERE Ut.Id = @utilizador";
                command.Parameters.AddWithValue("@utilizador", Session["id_utilizador"]);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    textName.Text = reader.GetString(0);
                    textEmail.Text = reader.GetString(1);
                    textUsername.Text = reader.GetString(2);
                }
                reader.Close();
                connection.Close();
            }
        }

        protected void ButtonEditarPerfil_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;
            // Atualizar a tabela Utilizador
            command.CommandText = "Update Utilizador SET Nome = @name, Email = @user_email WHERE Id = @user_id";
            command.Parameters.AddWithValue("@name", textName.Text);
            command.Parameters.AddWithValue("@user_email", textEmail.Text);
            command.Parameters.AddWithValue("@user_id", Session["id_utilizador"]);
            command.ExecuteNonQuery();

            // Atualizar a tabela Users
            command.CommandText = "Update Users SET UserName = @user_name WHERE UserId = @user_id";
            command.Parameters.Clear(); // Limpar parâmetros anteriores
            command.Parameters.AddWithValue("@user_name", textUsername.Text);
            command.Parameters.AddWithValue("@user_id", Session["id_utilizador"]);
            command.ExecuteNonQuery();

            // Atualizar a tabela Memberships
            command.CommandText = "Update Memberships SET Email = @user_email WHERE UserId = @user_id";
            command.Parameters.Clear(); // Limpar parâmetros anteriores
            command.Parameters.AddWithValue("@user_email", textEmail.Text);
            command.Parameters.AddWithValue("@user_id", Session["id_utilizador"]);
            command.ExecuteNonQuery();

            // Confirmar a transação
            transaction.Commit();
            connection.Close();

            Response.Write("<script>alert('BLA BLA')</script>");
        }
    }
}

