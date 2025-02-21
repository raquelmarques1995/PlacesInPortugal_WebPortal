using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;


namespace projectoLocais.utilizador
{
    public partial class criarLocal : System.Web.UI.Page
    {
        string connectionString = @"data source=.\sqlexpress; initial catalog=Locais; integrated security=true;";

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
                conn.Close();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownDistrito.Items.Insert(0, "Escolha um Distrito primeiro");
                LoadDistritos();
                ViewState["idLocal"] = 0;
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

        protected void buttonGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                //definição do Stored Procedure que cria o registo na base de dados
                command.CommandText = "LocalCriar";
                command.CommandType = CommandType.StoredProcedure;

                //definição dos valores a inserir na tabela
                command.Parameters.AddWithValue("@nome", textName.Text);
                command.Parameters.AddWithValue("@descricao", textDescricao.Text);

                if (textMorada.Text == "")
                    command.Parameters.AddWithValue("@morada", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@morada", textMorada.Text);

                command.Parameters.AddWithValue("@localidade", textLocalidade.Text);
                command.Parameters.AddWithValue("@concelho", DropDownConcelho.SelectedValue);
                command.Parameters.AddWithValue("@utilizador", Session["id_utilizador"]);

                //obter latitude e longitude do mapa

                Datum localizacao = new Datum();
                string key = ConfigurationManager.AppSettings["APIkey"];
                string local = $"{textLocalidade.Text},Portugal";
                WebRequest request = WebRequest.Create

                ($"https://api.positionstack.com/v1/forward?access_key={key}&query={local}");
                WebResponse response = request.GetResponse();
                if (response != null)
                {
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string json = reader.ReadToEnd();

                    Root result = JsonConvert.DeserializeObject<Root>(json);

                    // obter latitude e longitude
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

                connection.Open();
                
                //guardar em ViewState o ID atribuido ao local
                ViewState["idLocal"] = command.ExecuteScalar();

                // Ativar o button para salvar foto
                buttonGuardarFoto.Enabled = true;
                connection.Close();
            }
        }



        protected void GetFotosLocal()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT Id, Ficheiro, Legenda FROM Foto WHERE Local = @local";

                command.Parameters.AddWithValue("@local", ViewState["idLocal"]);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                reader.Close();

                listFotos.DataSource = table;
                listFotos.DataBind();
            }
        }

        protected void buttonGuardarFoto_Click(object sender, EventArgs e)
        {
            if (uploadFoto.HasFile)
            {
                if (ViewState["idLocal"] == null)
                {
                    // No photo selected - cancel sql command and show error message
                    Response.Write("<script>alert('Não tem nehum local selecionado. Caso queira adicionar fotos a locais que tenha adicionado anteriormente " +
                        "por favor faça-o desde a sua área pessonal.');</script>");
                    return;
                }

                else
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;
                        command.CommandText = "LocalFotoCriar";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@local", ViewState["idLocal"]);
                        command.Parameters.AddWithValue("@legenda", textLegenda.Text);

                        //Types of files allowed
                        string[] ext = { ".jpg", ".jpeg", ".png", ".gif", ".tiff" };
                        bool ok = false;

                        // Get the file extension
                        string extensao = System.IO.Path.GetExtension(uploadFoto.FileName).ToString();

                        // Check if the extension is valid
                        foreach (string item in ext)
                            if (extensao == item)
                                ok = true;


                        if (ok)
                        {
                            // Generates a GUID to stop reapeating names 
                            Guid g = Guid.NewGuid();
                            string fileName = $"{g}-{uploadFoto.FileName}";
                            uploadFoto.SaveAs(Server.MapPath("../imagens/") + fileName);

                            command.Parameters.AddWithValue("@ficheiro", "imagens/" + fileName);

                            connection.Open();
                            command.ExecuteNonQuery();

                            // Updates the DataList with the photos from the Local
                            GetFotosLocal();
                        }
                        else
                        {
                            // Invalid file type- cancel sql command and show error message
                            Response.Write("<script>alert('Selecione um ficheiro do tipo \".jpg\", \".jpeg\", "
                           + "\".png\", \".gif\" ou \".tiff.');</script>");
                            return;
                        }
                    }
                }
            }

            else
            {
                // No photo selected - cancel sql command and show error message
                Response.Write("<script>alert('Selecione um ficheiro.');</script>");
                return;
            }
        }

        protected void button_save_local(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "LocalCriar";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@nome", textName.Text);
                command.Parameters.AddWithValue("@descricao", textDescricao.Text);

                if (textMorada.Text == "")
                    command.Parameters.AddWithValue("@morada", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@morada", textMorada.Text);

                command.Parameters.AddWithValue("@localidade", textLocalidade.Text);
                command.Parameters.AddWithValue("@concelho", DropDownConcelho.SelectedValue);
                command.Parameters.AddWithValue("@utilizador", Session["id_utilizador"]);


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

                // Activates the save photo button
                buttonGuardarFoto.Enabled = true;
            }
        }

        protected void buttonSelecionar_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    // Puts the photo ID on the viewstate
                    ViewState["idFoto"] = e.CommandArgument.ToString();

                    // Selects the Legenda fro mthe selected photo
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
        }

        protected void buttonEditarLegenda_Click(object sender, EventArgs e)
        {
            if (ViewState["idFoto"] == null)
            {

                // No photo selected - cancel sql command and show error message
                Response.Write("<script>alert('Selecione uma foto.');</script>");
                return;
            }

            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand commandFoto = new SqlCommand();
                    commandFoto.Connection = connection;
                    commandFoto.CommandText = "UPDATE Foto SET Legenda = @legenda WHERE Id = @id";
                    commandFoto.CommandType = CommandType.Text;

                    //ID da foto definido quando a foto é selecionada
                    commandFoto.Parameters.AddWithValue("@id", ViewState["idFoto"]);
                    commandFoto.Parameters.AddWithValue("@legenda", textLegenda.Text);
                    connection.Open();
                    commandFoto.ExecuteNonQuery();

                    textLegenda.Text = string.Empty;

                    // Updates the DataList with the photos from the Local
                    GetFotosLocal();
                }
            }
        }

        protected void buttonEliminarFoto_Click(object sender, EventArgs e)
        {
            if (ViewState["idFoto"] == null)
            {

                // No photo selected - cancel sql command and show error message
                Response.Write("<script>alert('Selecione uma foto.');</script>");
                return;
            }

            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Eliminates the file
                    SqlCommand commandFoto = new SqlCommand();
                    commandFoto.Connection = connection;
                    commandFoto.CommandText = "SELECT Ficheiro FROM Foto WHERE Id = @id";

                    // ID of the photo definited when the photo is selected
                    commandFoto.Parameters.AddWithValue("@id", ViewState["idFoto"]);
                    connection.Open();

                    // Get the name of the file
                    SqlDataReader reader = commandFoto.ExecuteReader();
                    while (reader.Read())
                    {
                        string ficheiro = Server.MapPath("../" + reader[0].ToString());
                        //eliminar ficheiro
                        if (File.Exists(ficheiro))
                            File.Delete(ficheiro);
                    }
                    reader.Close();

                    // Eliminates the data on the databse table
                    commandFoto.Parameters.Clear();
                    commandFoto.CommandText = "DELETE Foto WHERE Id = @id";
                    commandFoto.CommandType = CommandType.Text;
                    commandFoto.Parameters.AddWithValue("@id", ViewState["idFoto"]);
                    commandFoto.ExecuteNonQuery();

                    textLegenda.Text = string.Empty;

                    // Updates the DataList with the photos from the Local
                    GetFotosLocal();
                }
            }
        }

        protected void buttonCancelar_Click(object sender, EventArgs e)
        {
            textName.Text = string.Empty;
            textDescricao.Text = string.Empty;
            textMorada.Text = string.Empty;
            textLocalidade.Text = string.Empty;
            textLocalidade.Text = string.Empty;


            DropDownConcelho.Items.Clear();
            DropDownConcelho.Items.Insert(0, "Escolha um Distrito primeiro");
            LoadDistritos();
            ViewState["idLocal"] = 0;
        }

        protected void buttonCancelarFoto_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                if (ViewState["idLocal"] == null || (int)ViewState["idLocal"] == 0)
                {

                    // No Local selected - clear fields of the page
                    buttonCancelar_Click(sender, e);
                    return;
                }

                else
                {
                    // Gets the name of the files associated with the Local
                    SqlCommand commandFotos = new SqlCommand();
                    commandFotos.Connection = connection;
                    commandFotos.CommandText = "SELECT Ficheiro FROM Foto WHERE Local = @local";
                    commandFotos.Parameters.AddWithValue("@local", ViewState["idLocal"]);

                    Response.Write("Current idLocal: " + ViewState["idLocal"]?.ToString());

                    connection.Open();
                    SqlDataReader reader = commandFotos.ExecuteReader();
                    while (reader.Read())
                    {
                        string file = Server.MapPath("../" + reader[0].ToString());
                        // Eleminates the file
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                    reader.Close();

                    // Eliminates the data from the database tables
                    SqlCommand commandLocal = new SqlCommand();
                    commandLocal.Connection = connection;
                    commandLocal.CommandText = "LocalEliminar";
                    commandLocal.CommandType = CommandType.StoredProcedure;


                    if (ViewState["idLocal"] == null)
                    {
                        Response.Write("Error: idLocal is null");
                        return;
                    }
                    else
                    {
                        Response.Write("Current idLocal: " + ViewState["idLocal"]?.ToString());
                        commandLocal.Parameters.AddWithValue("@idlocal", ViewState["idLocal"]);
                        commandLocal.ExecuteNonQuery();
                        ViewState["idLocal"] = null;
                    }
                }
            }
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

    public class Datum
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }

    public class Root
    {
        public List<Datum> data { get; set; }
    }
}

