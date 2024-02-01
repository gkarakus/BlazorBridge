using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Configuration;

public partial class _Default : System.Web.UI.Page
{



    protected string link1, linktxt1, link2, linktxt2, link3, linktxt3, link4, linktxt4, link5, linktxt5;
    protected string ActClient, ActEE, NewhireW, PaycheckW, GrossW; 

    //protected ArrayList a1;
    //protected ArrayList b1;
    protected ArrayList a1 = new ArrayList();
    protected ArrayList b1 = new ArrayList();
    string username = "";

  
    SqlConnection con = new SqlConnection(@"Data Source=MBASQL;Integrated Security=false;User ID=Internaluser;Password=Mba123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;MultiSubnetFailover=False ");
       
    
    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        

        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        con.Open();

        LabelUserName.Text = Request.LogonUserIdentity.Name;
        username = Request.LogonUserIdentity.Name.Replace("MBASTPETE\\","");

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd2 = new SqlCommand("INSERT INTO [Bridge].[dbo].[BridgeUserLog]   " +
                "  (Username, EnterDate, PageVisit) " +
                " VALUES ( '" + username + "', GETDATE(), 'defaultpage')" +
                "   ", dbConnection);

            SqlDataReader reader2 = cmd2.ExecuteReader();
            dbConnection.Close();
        }

        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as totalc  FROM[Welland_Views].[dbo].[vClient]  " +
                "WHERE Client_Status_Code = 'A' ", dbConnection);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ActClient =  reader.GetInt32(0).ToString();
                }
            }           
        }

        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT Count(*) as totalEE  FROM[Welland_Views].[dbo].[vEmployee]  " +
                "WHERE EE_Status_Code <> 'T' ", dbConnection);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ActEE = reader.GetInt32(0).ToString();
                }
            }
        }

        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT Count(*) as totalEE  FROM [Welland_Views].[dbo].[vEmployee]  " +
                "Where Last_Hire_Date > DATEADD(dd, -(DATEPART(dw, GETDATE())-1), GETDATE()) " +
                "AND Last_Hire_Date < DATEADD(dd, 7-(DATEPART(dw, GETDATE())), GETDATE()) ", dbConnection);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    NewhireW = reader.GetInt32(0).ToString();
                }
            }
        }

        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  Count(*) as totalCheck  FROM  [Welland_Views].[dbo].[vEPV_Pay]  " +
                " Where Process_Date > DATEADD(dd, -(DATEPART(dw, GETDATE())-1), GETDATE()) " +
                "AND Process_Date < DATEADD(dd, 7-(DATEPART(dw, GETDATE())), GETDATE()) ", dbConnection);

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    PaycheckW = reader.GetInt32(0).ToString();
                }
            }

        }
        Page.DataBind();   


    } 
    //page load
    


}