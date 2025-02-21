using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;

namespace projectoLocais.utilizador
{
    public partial class editarLocal : System.Web.UI.Page
    {
        string connectionString = @"data source=.\sqlexpress; initial catalog=Locais; integrated security=true;";
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Nome { get; set; }

        void LoadDistritos()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, Nome FROM Distrito", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                DropDownDistrito.DataSource = reader;
                DropDownDistrito.DataTextField = "Nome";
                DropDownDistrito.DataValueField = "Id";
                DropDownDistrito.DataBind();
                // Adicionar item inicial
                DropDownDistrito.Items.Insert(0, "Selecione um Distrito");
            }
        }

        private void LoadConcelhos(int distritoId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Id, Nome FROM Concelho " +
                "WHERE Distrito = @DistritoId", conn);
                cmd.Parameters.AddWithValue("@DistritoId", distritoId);
                SqlDataReader reader = cmd.ExecuteReader();
                DropDownConcelho.DataSource = reader;
                DropDownConcelho.DataTextField = "Nome";
                DropDownConcelho.DataValueField = "Id";
                DropDownConcelho.DataBind();
                // Adicionar item inicial
                DropDownConcelho.Items.Insert(0, "Selecione um Concelho");
            }
        }

        protected void listDistrito_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GetFotosLocal(string local)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT Id, Ficheiro, Legenda FROM Foto WHERE Local = @local";

            //ID do local
            command.Parameters.AddWithValue("@local", local);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            reader.Close();
            connection.Close();
            listFotos.DataSource = table;
            listFotos.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //1 - ler dados do local
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = " SELECT L.Nome, L.Morada, L.Localidade, L.Descricao, L.Latitude, L.Longitude, " + "CAST(L.Concelho AS NVARCHAR), CAST(C.Distrito AS NVARCHAR) " + "FROM Local L JOIN Concelho C ON L.Concelho = C.Id WHERE L.Id = @local";
                command.Parameters.AddWithValue("@local", Session["local"]);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                //variáveis a utilizar na seleção do concelho e respetivo distrito

                string idDistrito = "", idConcelho = "";

                while (reader.Read())
                {
                    nomeLocal.Text = reader.GetString(0);

                    textName.Text = reader.GetString(0);
                    textMorada.Text = reader.GetValue(1)?.ToString() ?? string.Empty;

                    textLocalidade.Text = reader.GetString(2);
                    textDescricao.Text = reader.GetString(3);

                    Nome = reader[0].ToString();
                    Latitude = reader[4].ToString();
                    Longitude = reader[5].ToString();

                    idConcelho = reader.GetString(6).ToString();
                    idDistrito = reader.GetString(7).ToString();
                }
                reader.Close(); connection.Close();

                //2 - carregar distritos
                LoadDistritos();

                //selecionar o distrito
                if (DropDownDistrito.Items.FindByValue(idDistrito) != null)
                {
                    DropDownDistrito.SelectedValue = idDistrito;
                    DropDownDistrito_SelectedIndexChanged(null, null); // atualiza os concelhos
                }
                //3 - selecionar o concelho
                if (DropDownConcelho.Items.FindByValue(idConcelho) != null)
                {
                    DropDownConcelho.SelectedValue = idConcelho;
                }

                //4 - carregar as fotos
                GetFotosLocal(Session["local"].ToString());
            }
        }

        protected void buttonGuardar_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            //definição do Stored Procedure que cria o registo na base de dados
            command.CommandText = "LocalEditar";
            command.CommandType = CommandType.StoredProcedure;

            //definição dos valores a alterar na tabela
            command.Parameters.AddWithValue("@id", Session["local"]);
            command.Parameters.AddWithValue("@nome", textName.Text);
            command.Parameters.AddWithValue("@descricao", textDescricao.Text);
            if (textMorada.Text == "")
                command.Parameters.AddWithValue("@morada", DBNull.Value);
            else
                command.Parameters.AddWithValue("@morada", textMorada.Text);
            command.Parameters.AddWithValue("@localidade", textLocalidade.Text);
            command.Parameters.AddWithValue("@concelho", DropDownConcelho.SelectedValue);

            // Checks if the user has selected anything on the map and if they didn´t, the api
            // gets the latitude and longitude based on the selected Distrito and Concelho

            if (string.IsNullOrEmpty(latitude.Value) && string.IsNullOrEmpty(longitude.Value))
            {

                // Get latitude and longitude if the field is empty
                Datum localizacao = new Datum();
                string key = ConfigurationManager.AppSettings["APIKey"];
                string local = "";


                if (textLocalidade.Text == "")
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT Nome FROM Concelho WHERE Id = @ID", connection);
                    cmd.Parameters.AddWithValue("@ID", DropDownConcelho.SelectedValue);


                    // Execute the query and retrieve the result
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            local = $"{reader["Nome"]},Portugal";
                        }
                    }
                    connection.Close();
                }
                else
                {
                    local = $"{textLocalidade.Text},Portugal";
                }
                WebRequest request = WebRequest.Create($"https://api.positionstack.com/v1/forward?access_key={key}&query={local}");
                WebResponse response = request.GetResponse();

                if (response != null)
                {
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string json = reader.ReadToEnd();
                    Root result = JsonConvert.DeserializeObject<Root>(json);
                    if (result.data != null && result.data.Count > 0)
                    {
                        localizacao = result.data[0];
                        command.Parameters.AddWithValue("@latitude", localizacao.latitude);
                        command.Parameters.AddWithValue("@longitude", localizacao.longitude);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@latitude", DBNull.Value);
                        command.Parameters.AddWithValue("@longitude", DBNull.Value);
                    }
                }
            }

            else
            {
                command.Parameters.AddWithValue("@latitude", latitude.Value);
                command.Parameters.AddWithValue("@longitude", longitude.Value);
            }
            connection.Open();

            ViewState["idLocal"] = command.ExecuteScalar();
            Response.Redirect(Request.RawUrl);

        }

        protected void buttonSelecionar_Command(object sender, CommandEventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            if (e.CommandArgument != null)
            { //colocar em ViewState o Id da foto selecionada
                ViewState["idFoto"] = e.CommandArgument.ToString();

                //selecionar legenda da foto selecionada
                SqlCommand commandFoto = new SqlCommand();
                commandFoto.Connection = connection;
                commandFoto.CommandText = "SELECT Legenda FROM Foto WHERE Id = @id";
                commandFoto.Parameters.AddWithValue("@id", ViewState["idFoto"]);
                connection.Open();
                SqlDataReader reader = commandFoto.ExecuteReader();
                while (reader.Read())
                {
                    textLegenda.Text = reader[0].ToString();
                }
                reader.Close();
            }
        }

        protected void buttonGuardarFoto_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            //ficheiro - nome do controlo FileUpload
            if (uploadFoto.HasFile)
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "LocalFotoCriar";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@local", ViewState["idLocal"]);
                command.Parameters.AddWithValue("@legenda", textLegenda.Text);

                //tipos de ficheiros admitidos
                string[] ext = { ".jpg", ".jpeg", ".png", ".gif", ".tiff" };

                bool ok = false;
                //obter extensão do ficheiro
                string extensao = System.IO.Path.GetExtension(uploadFoto.FileName).ToString();

                //verificar se a extensão se encontra no Array de ficheiros admitidos
                foreach (string item in ext)
                    if (extensao == item)
                        ok = true;
                //se o tipo de ficheiro está correto
                if (ok)
                {
                    //gerar Guid para evitar nomes repetidos
                    Guid g = Guid.NewGuid();
                    string fileName = $"{g}-{uploadFoto.FileName}";
                    uploadFoto.SaveAs(Server.MapPath("../imagens/") + fileName);

                    //definir parâmetro
                    command.Parameters.AddWithValue("@ficheiro", "imagens/" + fileName);
                    //tipo de ficheiro correto - colocar informação na base de dados
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();

                    //atualizar DataList referente às fotos
                    GetFotosLocal(Session["local"].ToString());
                }
                else
                {
                    //ficheiro inválido - cancelar execução do procedimento
                    Response.Write("<script>alert('Selecione um ficheiro do tipo \".jpg\", \".jpeg\", " + "\".png\", \".gif\" ou \".tiff.');</script>");
                    return;
                }
            }
            else
            {
                //não foi selecionado um ficheiro - cancelar execução do procedimento
                Response.Write("<script>alert('Selecione um ficheiro do tipo \".jpg\", \".jpeg\", " + "\".png\", \".gif\" ou \".tiff.');</script>");
                return;
            }
        }

        protected void buttonEditarLegenda_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand commandFoto = new SqlCommand();
            commandFoto.Connection = connection;
            commandFoto.CommandText = "UPDATE Foto SET Legenda = @legenda WHERE Id = @id";
            commandFoto.CommandType = CommandType.Text;

            //ID da foto definido quando a foto é selecionada
            commandFoto.Parameters.AddWithValue("@id", ViewState["idFoto"]);
            commandFoto.Parameters.AddWithValue("@legenda", textLegenda.Text);
            connection.Open();
            commandFoto.ExecuteNonQuery();
            connection.Close();
            textLegenda.Text = string.Empty;
            //atualizar DataList fotos do local
            GetFotosLocal(Session["local"].ToString());
        }

        protected void buttonEliminarFoto_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            //eliminar ficheiro
            SqlCommand commandFoto = new SqlCommand();
            commandFoto.Connection = connection;
            commandFoto.CommandText = "SELECT Ficheiro FROM Foto WHERE Id = @id";

            //ID da foto definido quando a foto é selecionada
            commandFoto.Parameters.AddWithValue("@id", ViewState["idFoto"]);
            connection.Open();

            //obter nome do ficheiro a eliminar
            SqlDataReader reader = commandFoto.ExecuteReader();
            while (reader.Read())
            {
                string ficheiro = Server.MapPath("../" + reader[0].ToString());
                //eliminar ficheiro
                if (File.Exists(ficheiro))
                {
                    File.Delete(ficheiro);
                }
            }
            reader.Close();

            //eliminar dados na tabela
            commandFoto.Parameters.Clear();
            commandFoto.CommandText = "DELETE Foto WHERE Id = @id";
            commandFoto.CommandType = CommandType.Text;
            commandFoto.Parameters.AddWithValue("@id", ViewState["idFoto"]);
            commandFoto.ExecuteNonQuery();
            connection.Close();
            textLegenda.Text = string.Empty;

            //atualizar DataList fotos do local
            GetFotosLocal(Session["local"].ToString());
        }

        protected void buttonCancelarFoto_Click(object sender, EventArgs e)
        {
            textLegenda.Text = string.Empty;
            //o ficheiro selecionado é removido - //o valor do FileUpload não é mantido em ViewState
        }

        protected void buttonCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("areaPessoal.aspx");
        }

        protected void buttonEliminar_Click(object sender, EventArgs e)
        {
            //eliminar ficheiros
            //obter nome dos ficheiros a eliminar
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand commandFotos = new SqlCommand();
            commandFotos.Connection = connection;
            commandFotos.CommandText = "SELECT Ficheiro FROM Foto WHERE Local = @id";
            commandFotos.Parameters.AddWithValue("@id", Session["local"]);
            connection.Open();

            SqlDataReader readerFotos = commandFotos.ExecuteReader();

            while (readerFotos.Read())
            {

                string ficheiro = Server.MapPath("../" + readerFotos[0].ToString());
                //eliminar ficheiro
                if (File.Exists(ficheiro))
                {
                    File.Delete(ficheiro);
                }
            }
            readerFotos.Close();

            //alterei o procedure LocalEditar para eliminar também os comentários
            // ALTER PROCEDURE[dbo].[LocalEliminar]
            //@idLocal INT
            //AS
            //    DELETE FOTO WHERE Foto.Local = @idLocal
            //    DELETE Comentario WHERE Comentario.Local = @idLocal
            //    DELETE Local WHERE Local.Id = @idLocal

            //eliminar dados na base de dados

            SqlCommand commandLocal = new SqlCommand();
            commandLocal.Connection = connection;
            commandLocal.CommandText = "LocalEliminar";
            commandLocal.CommandType = CommandType.StoredProcedure;
            commandLocal.Parameters.AddWithValue("@idLocal", Session["local"]);

            commandLocal.ExecuteNonQuery();

            connection.Close();

            Response.Write("<script>alert('Local eliminado!'); window.location='areaPessoal.aspx';</script>");
            Response.End();
        }

        protected void DropDownDistrito_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownDistrito.SelectedValue != "Selecione um Distrito")
            {
                LoadConcelhos(int.Parse(DropDownDistrito.SelectedValue));
            }
            else
            {
                DropDownConcelho.Items.Clear();
                DropDownConcelho.Items.Insert(0, "Selecione um Concelho");
            }
        }
    }
}
