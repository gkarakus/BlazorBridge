using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;


public partial class autoterm : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(@"Data Source=MBASQL;Integrated Security=false;User ID=Internaluser;Password=Mba123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;MultiSubnetFailover=False ");
    protected void Page_Load(object sender, EventArgs e)
    {      
        LabelUserName.Text = Request.LogonUserIdentity.Name;
        //SelectParameters["UpdateBy"].DefaultValue = User.Identity.Name;
        SqlDataSource1.UpdateParameters["UpdateBy"].DefaultValue = Request.LogonUserIdentity.Name.ToString().Replace("MBASTPETE\\", "");
    }
                 

    protected void Button1_Click(object sender, EventArgs e)
    {
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            try
            {
                dbConnection.Open();
               // Label1.Text = "SQL Connection successful";

                SqlCommand cmd = new SqlCommand("AutotermSetupRefresh", dbConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

               

            }
            catch (SqlException ex)
            {
               // Label1.Text = "Connection FAIL" + ex.Message;
            }

            finally
            {
                dbConnection.Close();
              //  Label1.Text = GridView1.Rows.Count.ToString();
            }
        }
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }




}