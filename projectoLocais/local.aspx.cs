using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace projectoLocais
{
    public partial class local1 : System.Web.UI.Page
    {
        int currentPage;

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Nome { get; set; }

        public bool mostrarComentario;

        protected void Page_Load(object sender, EventArgs e)
        {
            {
                if (!IsPostBack)
                {
                    // Verificar se o utilizador está autenticado antes de verificar se pode comentar
                    if (Session["id_utilizador"] != null)
                    {
                        mostrarComentario = PodeComentar();
                    }
                    else
                    {
                        mostrarComentario = false; // Se não estiver autenticado, não pode comentar
                    }

                    ObterComentarios();                   

                    //mostrar/ocultar DIV que permite comentar
                    divComentario.Visible = HttpContext.Current.User.Identity.IsAuthenticated && PodeComentar();


                    using (SqlConnection connection = new SqlConnection(@"data source=.\sqlexpress; initial catalog = Locais; integrated security=true;"))
                    {
                        // Obter as fotos
                        string query = "SELECT Id, Legenda, Ficheiro FROM Foto WHERE Local = @id";

                        using (SqlCommand commandFotos = new SqlCommand(query, connection))
                        {
                            commandFotos.Parameters.AddWithValue("@id", Request.QueryString["id"]);
                            SqlDataAdapter adapter = new SqlDataAdapter(commandFotos);
                            DataTable fotos = new DataTable();
                            adapter.Fill(fotos);

                            // Associar dados ao repeater de fotos
                            rptFotos.DataSource = fotos;
                            rptFotos.DataBind();

                            // Associar dados ao repeater de indicadores
                            RepeaterIndicators.DataSource = fotos;
                            RepeaterIndicators.DataBind();
                        }

                        // Obter dados do local
                        query = "SELECT L.Nome, L.Descricao, L.Localidade, C.Nome 'Concelho', D.Nome 'Distrito', D.Id 'ID_distrito', L.Latitude, L.Longitude " +
                                "FROM Local L JOIN Concelho C ON L.Concelho = C.Id JOIN Distrito D ON C.Distrito = D.Id WHERE L.Id = @id";

                        using (SqlCommand commandDados = new SqlCommand(query, connection))
                        {
                            commandDados.Parameters.AddWithValue("@id", Request.QueryString["id"]);
                            connection.Open();
                            SqlDataReader reader = commandDados.ExecuteReader();
                            while (reader.Read())
                            {
                                nomeLocal.Text = reader[0].ToString();
                                descricao.Text = reader[1].ToString();
                                localizacao.Text = $"{reader[2]} {reader[3]} {reader[4]}";
                                IDdistrito.Text = reader[5].ToString();

                                Nome = reader[0].ToString();
                                Latitude = reader[6].ToString();
                                Longitude = reader[7].ToString();
                            }
                            reader.Close();
                        }
                    }
                }
            }
        }

        void ObterComentarios()
        {
            SqlConnection connection = new SqlConnection(
                @"data source=.\sqlexpress; initial catalog = Locais; integrated security=true;");
            using (SqlCommand commandComentarios = new SqlCommand("LocalComentarios", connection))
            {
                commandComentarios.CommandType = CommandType.StoredProcedure;
                commandComentarios.Parameters.AddWithValue("@local", Request.QueryString["id"]);
                SqlDataReader reader;
                DataTable table = new DataTable();
                connection.Open();
                reader = commandComentarios.ExecuteReader();
                table.Load(reader);
                connection.Close();
                BindListComentarios(table);
            }
        }

        void BindListComentarios(DataTable table)
        {
            PagedDataSource paged = new PagedDataSource();
            paged.DataSource = table.DefaultView;
            paged.PageSize = 5; paged.AllowPaging = true;
            paged.CurrentPageIndex = currentPage;

            linkPrimeira.Enabled = !paged.IsFirstPage;
            linkAnterior.Enabled = !paged.IsFirstPage;
            linkSeguinte.Enabled = !paged.IsLastPage;
            linkUltima.Enabled = !paged.IsLastPage;
            ViewState["total"] = paged.PageCount;
            listComentarios.DataSource = paged;
            listComentarios.DataBind();
            ViewState["dataSource"] = table;
        }

        protected void linkPrimeira_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            ViewState["contador"] = 0;
            ObterComentarios();
        }

        protected void linkAnterior_Click(object sender, EventArgs e)
        {
            //obter paginação atual
            currentPage = (int)ViewState["contador"];
            currentPage -= 1;
            ViewState["contador"] = currentPage;
            ViewState["contador"] = currentPage;

            //utilizar dados do DataTable que se encontram em ViewState
            BindListComentarios((DataTable)ViewState["dataSource"]);
        }

        protected void linkSeguinte_Click(object sender, EventArgs e)
        {
            //obter paginação atual
            currentPage = (int)ViewState["contador"];
            currentPage += 1;
            ViewState["contador"] = currentPage;

            //utilizar dados do DataTable que se encontram em ViewState
            BindListComentarios((DataTable)ViewState["dataSource"]);
        }

        protected void linkUltima_Click(object sender, EventArgs e)
        {
            //a variável "total" tem o total de páginas do DataList
            currentPage = (int)ViewState["total"] - 1;
            ViewState["contador"] = currentPage;

            //utilizar dados do DataTable que se encontram em ViewState
            BindListComentarios((DataTable)ViewState["dataSource"]);
        }

        bool PodeComentar()
        { //verificar se já comentou
            SqlConnection connection = new SqlConnection(
                @"data source=.\sqlexpress; initial catalog = Locais; integrated security=true;");

            SqlCommand commandComentarios = new SqlCommand();
            commandComentarios.Connection = connection;
            commandComentarios.CommandText = "SELECT COUNT(*) FROM Comentario C JOIN Local L ON C.Local = L.Id " + "WHERE C.Utilizador = @utilizador AND L.Id = @local";
            commandComentarios.Parameters.AddWithValue("@utilizador", Session["id_utilizador"]);
            commandComentarios.Parameters.AddWithValue("@local", Request.QueryString["id"]);
            connection.Open();
            int i = (int)commandComentarios.ExecuteScalar();
            connection.Close();
            if (i > 0)
                return false;

            //verificar se é autor do local
            SqlCommand commandAutorLocal = new SqlCommand();
            commandAutorLocal.Connection = connection;
            commandAutorLocal.CommandText = "SELECT COUNT(*) FROM Local L JOIN Utilizador U " + "ON L.Utilizador = U.ID WHERE L.Id = @id AND L.Utilizador = @utilizador";
            commandAutorLocal.Parameters.AddWithValue("@id", Request.QueryString["id"]);
            commandAutorLocal.Parameters.AddWithValue("@utilizador", Session["id_utilizador"]);
            connection.Open();
            i = (int)commandAutorLocal.ExecuteScalar();
            connection.Close();
            if (i > 0)
                return false;
            return true;
        }

        protected void buttonComentar_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(
                @"data source=.\sqlexpress; initial catalog = Locais; integrated security=true;");
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT Comentario(Comentario, Classificacao, Data, Local, Utilizador) " + "VALUES(@comentario, @classificacao, @data, @local, @utilizador)";

            command.Parameters.AddWithValue("@comentario", textComentario.Text);
            command.Parameters.AddWithValue("@classificacao", listClassificacao.SelectedValue);
            command.Parameters.AddWithValue("@data", DateTime.Now);
            command.Parameters.AddWithValue("@local", Request.QueryString["id"]);
            command.Parameters.AddWithValue("@utilizador", Session["id_utilizador"]);
            connection.Open();
            command.ExecuteNonQuery(); connection.Close();

            //atualizar comentários
            ObterComentarios();

        }

    }
}