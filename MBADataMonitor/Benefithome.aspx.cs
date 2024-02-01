using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Benefithome : System.Web.UI.Page
{

    SqlConnection con = new SqlConnection(@"Data Source=MBASQL;Integrated Security=false;User ID=Internaluser;Password=Mba123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;MultiSubnetFailover=False ");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();

       // Label1.Text = System.Web.HttpContext.Current.User.ToString();
        Label1.Text = Environment.UserName.ToString();
       // Label1.Text = "gokhan";

    }

    

    protected void Button1_Click1(object sender, EventArgs e)
    {
        SqlCommand cmd = con.CreateCommand();
        cmd.CommandType = CommandType.Text;
        cmd.CommandText = "SELECT TOP(100)  [Client] ,[EE_Status_Code] ,[EE_ID] " +
            ",[EE_SSN] ,[EE_Last_Name] ,[EE_Middle_Initial] ,[EE_First_Name] ,[Birth_Date], " +
            "[EE_No], [TermDate] FROM  [Welland_Views].[dbo].[vEmployee] ";
        cmd.ExecuteNonQuery();
       // DataTable dt = new DataTable();
       // SqlDataAdapter da = new SqlDataAdapter(cmd);
       // da.Fill(dt);
       // GridView1.DataSource = dt;
       // GridView1.DataBind();
       // ListBox1.DataSource = dt;
       // ListBox1.DataBind();

    }
}