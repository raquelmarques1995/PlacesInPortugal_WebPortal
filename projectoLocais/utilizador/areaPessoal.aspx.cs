using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Web.Providers.Entities;

namespace projectoLocais.utilizador
{
    public partial class WebForm1 : System.Web.UI.Page
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["contador"] = 0;
                CarregarLocais();
                userName.Text = User.Identity.Name;
                
            }
            currentPage = (int)ViewState["contador"];
        }
       
        private void CarregarLocais()
        {
            SqlConnection connection = new SqlConnection(@"data source=.\Sqlexpress; initial catalog = Locais; integrated security = true;");
            SqlCommand command = new SqlCommand("GetLocaisByUtilizador", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Connection = connection;

            //definição do Stored Procedure a executar
            command.CommandText = "GetLocaisByUtilizador"; 
            command.Parameters.AddWithValue("@utilizador", Session["id_utilizador"].ToString()); 
            connection.Open(); 
            SqlDataReader reader = command.ExecuteReader(); 
            DataTable table = new DataTable(); 
            table.Load(reader); 
            reader.Close(); 
            connection.Close();

            //mostrar dados no controloDataList
            BindListLocais(table);

            // Esconder ou mostrar a div navegadores
            navegadores.Visible = table.Rows.Count > 0;
        }

        protected void linkDetalhes_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            if (e.CommandArgument != null)
            {
                Session["local"] = e.CommandArgument.ToString();
                Response.Redirect("editarLocal.aspx");
            }
        }


        protected void linkCriarLocal_Click(object sender, EventArgs e)
        {
            Response.Redirect("criarLocal.aspx");
        }

        protected void linkPrimeira_Click(object sender, EventArgs e)
        {
            currentPage = 0;
            ViewState["contador"] = 0;
            CarregarLocais();
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