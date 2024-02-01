using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.ModelBinding;
using System.Web.UI.WebControls;

public partial class Courier : System.Web.UI.Page
{
    int CID = 0;
    string ClientID = "";
    string ClientName = "";
    string Division = "";
    string City = "";
    string Zip = "";
    string Cod = "";
    string Note = ""; 



    protected void Page_Load(object sender, EventArgs e)
    {
        LabelUserName.Text = Request.LogonUserIdentity.Name;

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  u2_id   FROM [Welland_Export_Cloud].[dbo].[CLIENT_DETAILS]", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //DropDownList1 itm = new ListItem(reader.GetString(0));                  
                        DropDownList1.Items.Add(reader.GetString(0));                    
                }
            }
            dbConnection.Close();
        }

        if (!Page.IsPostBack)
        {
            Loadgrid(" ");


        }



    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
       // TextBox2.Text = DropDownList1.SelectedValue.ToString();

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  [u2_id] ,[Client_Legal_Name] ,[Client_ADDR_City] ,[Client_ADDR_Zip_Code] " +
                " ,[Client_Status_Code]  ,[Delivery_Method_Code] ,[Delivery_Method_Desc]  FROM [Welland_Export_Cloud].[dbo].[CLIENT_DETAILS] " +
                " WHERE u2_id = '" + DropDownList1.SelectedValue.ToString() + "'", dbConnection); 
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //DropDownList1 itm = new ListItem(reader.GetString(0));    
                    TextBox1.Text = reader.GetString(0);
                    TextBox2.Text = reader.GetString(1);
                    TextBox3.Text = reader.GetString(2);
                    TextBox6.Text = reader.GetString(3);

                }
            }
            dbConnection.Close();
        }

    }


    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
       if (TextBox1.Text.Length > 5 && TextBoxDurum.Text == "")
        {
            var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
            using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand("SELECT  [u2_id] ,[Client_Legal_Name] ,[Client_ADDR_City] ,[Client_ADDR_Zip_Code] " +
                    " ,[Client_Status_Code]  ,[Delivery_Method_Code] ,[Delivery_Method_Desc]  FROM [Welland_Export_Cloud].[dbo].[CLIENT_DETAILS] " +
                    " WHERE u2_id = '" + TextBox1.Text + "'", dbConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        //DropDownList1 itm = new ListItem(reader.GetString(0));    
                       // TextBox1.Text = reader.GetString(0);
                        TextBox2.Text = reader.GetString(1);
                        TextBox3.Text = reader.GetString(2);
                        TextBox6.Text = reader.GetString(3);

                    }
                }
                dbConnection.Close();
            }

        }

    }




    //------------------------SAVE -----------------------------------------
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (TextBoxDurum.Text.Length == 0)
        {
            if (TextBox1.Text != "" && TextBox2.Text != "")
            {
                

                ClientID = TextBox1.Text;
                ClientName = TextBox2.Text.Replace("'", "''");
                Division = TextBox7.Text;
                City = TextBox3.Text;
                Zip = TextBox6.Text;
                if (TextBox4.Text == "")
                {
                    Cod = "0.00";
                }
                else
                {
                    Cod = TextBox4.Text;
                }
                
                Note = TextBox5.Text;

                var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
                using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                {

                    dbConnection.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO  [Bridge].[dbo].[Courier] (ClientID ,ClientName, Division, City , Zip ,Cod,Note)  " +
                        "VALUES ( " +
                        " '" + ClientID + "', " +
                        " '" + ClientName + "', " +
                         " '" + Division + "', " +
                        " '" + City + "', " +
                        " '" + Zip + "', " +
                        " " + Cod + ", " +
                        " '" + Note + "' " +
                         " )", dbConnection);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dbConnection.Close();
                }
            }


        }
        else
        {

            var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
            using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[Courier]  SET  " +
                    " ClientID = '" + TextBox1.Text + "', " +
                    " ClientName = '" + TextBox2.Text + "', " +
                     " Division = '" + TextBox7.Text + "', " +
                    " City = '" + TextBox3.Text + "', " +
                    " Zip = '" + TextBox6.Text + "', " +
                    " Cod = " + TextBox4.Text + ", " +
                    " Note = '" + TextBox5.Text + "' " +                  
                    " WHERE ID = '" + TextBoxDurum.Text + "' ", dbConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                dbConnection.Close();
            }

                       
            
        }

        // Response.Redirect(Request.Url.ToString());
        Loadgrid(TextBox8.Text);
    }





    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxDurum.Text = GridView1.SelectedRow.Cells[1].Text;
        TextBox1.Text = GridView1.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        TextBox2.Text = GridView1.SelectedRow.Cells[3].Text.Replace("&#39;", "'");
        TextBox7.Text = GridView1.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
        TextBox3.Text = GridView1.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
        TextBox6.Text = GridView1.SelectedRow.Cells[6].Text.Replace("&nbsp;", "");
        TextBox4.Text = GridView1.SelectedRow.Cells[7].Text.Replace("&nbsp;", "");
        TextBox5.Text = GridView1.SelectedRow.Cells[8].Text.Replace("&nbsp;", "");

    }

    //---------------------CANCEL BUTTON ------
    protected void Button2_Click(object sender, EventArgs e)
    {
        TextBoxDurum.Text = "";
        TextBox1.Text = "";
        TextBox2.Text = "";
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox5.Text = "";
        TextBox6.Text = "";
        TextBox7.Text = "";
        TextBox8.Text = "";

        Loadgrid(TextBox8.Text);
    }

    protected void TextBox8_TextChanged(object sender, EventArgs e)
    {

       

    }

    protected void Button3_Click(object sender, EventArgs e)
    {

        Loadgrid(TextBox8.Text);

    }

     void Loadgrid(string sdata)
      {

        DataTable dt = new DataTable();
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  *  FROM [Bridge].[dbo].[Courier]   " +
                "WHERE ClientName like '%" + sdata + "%'  OR ClientID like  '%" + sdata + "%'  ORDER BY RDate DESC", dbConnection);
            //SqlDataReader reader = cmd.ExecuteReader();
            using (SqlDataAdapter a = new SqlDataAdapter(cmd))
            {
                a.Fill(dt);
            }

            dbConnection.Close();
        }
        GridView1.DataSource = dt;
        GridView1.DataBind();

      }







    protected void GridView1_SelectedIndexChanged1(object sender, EventArgs e)
    {
        TextBoxDurum.Text = GridView1.SelectedRow.Cells[1].Text;
        TextBox1.Text = GridView1.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        TextBox2.Text = GridView1.SelectedRow.Cells[3].Text.Replace("&#39;", "'");
        TextBox7.Text = GridView1.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
        TextBox3.Text = GridView1.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");
        TextBox6.Text = GridView1.SelectedRow.Cells[6].Text.Replace("&nbsp;", "");
        TextBox4.Text = GridView1.SelectedRow.Cells[7].Text.Replace("&nbsp;", "");
        TextBox5.Text = GridView1.SelectedRow.Cells[8].Text.Replace("&nbsp;", "");
    }
}