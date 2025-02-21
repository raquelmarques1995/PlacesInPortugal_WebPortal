using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace projectoLocais.utilizador
{
    public partial class editarPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {

            MembershipUser currentUser = Membership.GetUser();
            if (currentUser != null)
            {
                bool changeSuccess = currentUser.ChangePassword(txtCurrentPassword.Text, txtNewPassword.Text);

                if (changeSuccess)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Palavra-passe alterada com sucesso!";
                }
                else
                {
                    lblMessage.Text = "Erro ao alterar a palavra-passe. Verifique se a atual está correta.";
                }
            }
            else
            {
                lblMessage.Text = "Utilizador não encontrado!";
            }
        }
    }
}