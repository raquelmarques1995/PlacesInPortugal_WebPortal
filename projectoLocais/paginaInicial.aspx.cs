using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace projectoLocais
{
    public partial class paginaInicial : System.Web.UI.Page
    {
        int currentPage;

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

        void GetLocais()
        {
            SqlConnection connection = new SqlConnection(
                @"data source=.\sqlexpress; initial catalog = Locais; integrated security=true;");
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["contador"] = 0;
                GetLocais();
            }

            currentPage = (int)ViewState["contador"];

            //Para colocar os Locais no Top10
            SqlConnection connection = new SqlConnection(@"data source=.\sqlexpress; initial catalog = Locais; integrated security=true;");
            using (SqlCommand command = new SqlCommand("GetTop10Locais", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // colocar dados no DataList
                    listLocaisTop.DataSource = reader;
                    listLocaisTop.DataBind();
                }
                connection.Close();
            }
        }

        protected void linkPrimeira_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            ViewState["contador"] = 0;
            GetLocais();
        }

        protected void linkAnterior_Click(object sender, EventArgs e)
        {
            //obter paginação atual
            currentPage = (int)ViewState["contador"];
            currentPage -= 1;
            ViewState["contador"] = currentPage;

            //utilizar dados do DataTable que se encontram em ViewState
            BindListLocais((DataTable)ViewState["dataSource"]);
        }

        protected void linkSeguinte_Click(object sender, EventArgs e)
        {
            //obter paginação atual
            currentPage = (int)ViewState["contador"];
            currentPage += 1;
            ViewState["contador"] = currentPage;

            //utilizar dados do DataTable que se encontram em ViewState
            BindListLocais((DataTable)ViewState["dataSource"]);
        }

        protected void linkUltima_Click(object sender, EventArgs e)
        {
            //a variável "total" tem o total de páginas do DataList
            currentPage = (int)ViewState["total"] - 1;
            ViewState["contador"] = currentPage;

            //utilizar dados do DataTable que se encontram em ViewState
            BindListLocais((DataTable)ViewState["dataSource"]);
        }

    }
}