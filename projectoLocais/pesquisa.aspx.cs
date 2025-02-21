using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace projectoLocais
{
    public partial class pesquisa : System.Web.UI.Page
    {
        int currentPage;
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
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                DropDownDistrito.Items.Insert(0, "Escolha um Distrito primeiro");
                GetLocais();
                LoadDistritos();
                ViewState["contador"] = 0;
            }

            currentPage = (int)ViewState["contador"];
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

        protected void buttonPesquisar_Click(object sender, EventArgs e)
        {

            string command = "SELECT L.Id AS LocalID, L.Nome AS NomeLocal, C.Nome AS Concelho, " +
                            "D.Nome AS Distrito, F.Ficheiro AS PrimeiraFoto " +
                            "FROM Local L INNER JOIN Concelho C ON L.Concelho = C.Id " +
                            "INNER JOIN Distrito D ON C.Distrito = D.Id " +
                            "LEFT JOIN (SELECT Local, MIN(Id) AS PrimeiraFotoId FROM Foto " +
                            "GROUP BY Local) FP ON L.Id = FP.Local " +
                            "LEFT JOIN Foto F ON FP.PrimeiraFotoId = F.Id ";

            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(textName.Text) ||
                DropDownDistrito.SelectedValue != "Selecione um Distrito" ||
                (!string.IsNullOrEmpty(DropDownConcelho.SelectedValue) && DropDownConcelho.SelectedValue != "Selecione um Concelho"))
            {
                command += " WHERE ";
                if (!string.IsNullOrWhiteSpace(textName.Text))
                {

                    command += "L.Nome LIKE @Nome";
                    parameters.Add(new SqlParameter("@Nome", "%" + textName.Text.Trim() + "%"));
                }
                if (DropDownDistrito.SelectedValue != "Selecione um Distrito")
                {
                    if (parameters.Count > 0) command += " AND ";
                    command += "D.Id = @DistritoId";
                    parameters.Add(new SqlParameter("@DistritoId", DropDownDistrito.SelectedValue));
                }
                if (!string.IsNullOrEmpty(DropDownConcelho.SelectedValue) && (DropDownConcelho.SelectedValue != "Selecione um Concelho" &&
                    DropDownConcelho.SelectedValue != "Escolha um Distrito primeiro"))
                {
                    if (parameters.Count > 0) command += " AND ";
                    command += "C.Id = @ConcelhoId";
                    parameters.Add(new SqlParameter("@ConcelhoId", DropDownConcelho.SelectedValue));
                }
            }

            DataTable resultadoPesquisa = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(command, conn);
                cmd.Parameters.AddRange(parameters.ToArray());
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                resultadoPesquisa.Load(reader);
                reader.Close();
            }
            BindListLocais(resultadoPesquisa);
        }


        // DataList

        void GetLocais()
        {
            SqlConnection connection = new SqlConnection
            (@"data source=.\sqlexpress; initial catalog = Locais; integrated security=true;");

            SqlCommand command = new SqlCommand();
            command.CommandText = "GetLocais";
            command.CommandType = CommandType.StoredProcedure;

            command.Connection = connection;
            SqlDataReader reader;
            DataTable table = new DataTable();
            connection.Open();
            reader = command.ExecuteReader();
            table.Load(reader);
            connection.Close();
            BindListLocais(table);
        }

        void BindListLocais(DataTable table)
        {
            PagedDataSource paged = new PagedDataSource
            {
                DataSource = table.DefaultView,
                PageSize = 12, // Valor do itemsPerPage
                AllowPaging = true,
                CurrentPageIndex = currentPage
            };

            linkPrimeira.Enabled = !paged.IsFirstPage;
            linkAnterior.Enabled = !paged.IsFirstPage;
            linkSeguinte.Enabled = !paged.IsLastPage;
            linkUltima.Enabled = !paged.IsLastPage;

            ViewState["total"] = paged.PageCount;
            ListaLocais.DataSource = paged;
            ListaLocais.DataBind();
            ViewState["dataSource"] = table;

            // Passar o PageSize para o atributo `data-pagesize` no elemento principal
            ListaLocais.Attributes["data-pagesize"] = paged.PageSize.ToString();
        }


        protected void linkFirst_click(object sender, EventArgs e)
        {
            currentPage = 0;
            ViewState["contador"] = currentPage;
            GetLocais();
        }

        protected void linkPrevious_click(object sender, EventArgs e)
        {
            currentPage = (int)ViewState["contador"];
            currentPage -= 1;
            ViewState["contador"] = currentPage;
            BindListLocais((DataTable)ViewState["dataSource"]);
        }

        protected void linkNext_click(object sender, EventArgs e)
        {
            currentPage = (int)ViewState["contador"];
            currentPage += 1;
            ViewState["contador"] = currentPage;
            BindListLocais((DataTable)ViewState["dataSource"]);
        }

        protected void linkLast_click(object sender, EventArgs e)
        {
            currentPage = (int)ViewState["total"] - 1;
            currentPage += 1;
            ViewState["contador"] = currentPage;
            BindListLocais((DataTable)ViewState["dataSource"]);
        }

    }
}