using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Configuration;
//using Excel = Microsoft.Office.Interop.Excel;



public partial class crud : System.Web.UI.Page
{
  
      

    protected void Page_Load(object sender, EventArgs e)
    {
    

       
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        String datepick = TextBox1.Text;
        Csvtosql("253035" ,datepick);
    }

    public void Csvtosql(string ClientID, string  fdate)
    {
        String line = "";
        int linecount = 0;
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();           

            System.IO.StreamReader file = new System.IO.StreamReader(@" \\tpaa-pfs-cfs01\Users_Home$\gokhank\Desktop\TDI\014137\test.csv");
            while ((line = file.ReadLine()) != null)
            {
                //Console.WriteLine(line);
               // Response.Write(line);

                string[] Ln = line.Split(',');
                if (linecount > 0)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismImport]   " +
                        " (ClientID, EEID, PayDate, SSN, Codepos, HoursPos, AmountPos, Loc, Dept, Div, Job) " +
                          // " VALUES ( '" + ssnno + "', '" + ErrMes + "', '" + tableid + "', '" + Client + "', 'EMP')" +
                          " VALUES ( '"+ ClientID + "','', '" + fdate + "', '" + Ln[0] + "', '" + Ln[1] + "'," + Ln[2] + "," + Ln[3] + ",'" + Ln[4] + "','" + Ln[5] + "','" + Ln[6] + "','" + Ln[7] + "')" +
                        "   ", dbConnection);

                    cmd.ExecuteNonQuery();                    
                }
                linecount++;
            }           
            
            dbConnection.Close();

        }

    }





















   
}
















