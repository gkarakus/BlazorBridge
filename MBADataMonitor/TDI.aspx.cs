using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Reflection;
using System.Text.RegularExpressions;
using ExcelDataReader;
using System.Globalization;
using System.Diagnostics;

public partial class crud : System.Web.UI.Page
{
    //Master Client ID
    public static string strClientID = string.Empty;

    //SQL Connection Variables
    public static SqlConnection cnn;
    public static string SQLServer = "MBASQL";
    public static string SQLDatabase = "Bridge";      

    protected void Page_Load(object sender, EventArgs e)
    {
        LabelUserName.Text = Request.LogonUserIdentity.Name;
        Label5.Text = "Time Data v.1.12";
     //   Response.Write(System.Security.Principal.WindowsIdentity.GetCurrent().Name + "   " + User.Identity.IsAuthenticated.ToString() + "<br/>");
     //   Response.Write(User.Identity.AuthenticationType + "<br/>");
     //   Response.Write(User.Identity.Name + "<br/>");

        

        Label2.Visible = false;
        FileUpload1.Visible = false;
        MessageBox.Text = "Page Loading Done";
      //  Button3.Enabled = false;

    }

    public static List<string> SqlLookup(string clientno)
    {
        decimal HrRate;
        decimal Sthrs;
        List<string> EE = new List<string>();
        //-------------remove later----------------------
        if (clientno == "014237")
        {
            clientno = "014237";
        }

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection con = new SqlConnection(connfromWebconfig.ConnectionString))
        {     
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT  c.Client_ID  ,cd.u2_id ,cd.Client_Legal_Name ,c.EE_Status_Code, p.Pay_Method_Code , c.EE_ID  " +
                " ,e.EE_First_Name ,e.EE_Last_Name ,p.EE_SSN ,c.Home_Loc_Code ,lc.Loc_Name ,c.Home_Div_Code" +
                ",c.Home_Dept_Code ,c.EE_Job_Code ,c.EE_Last_Hire_Date ,CASE WHEN c.EE_Status_Code = 'T' THEN  c.EE_Status_Date ELSE NULL END as TerminationDate" +
                "  ,c.EE_No ,p.Pay_Hourly_Rate  ,p.Pay_Standard_Hours  " +
                " FROM[Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] c " +
                " INNER JOIN[Welland_Export_Cloud].[dbo].[EMPLOYEE_PAY] p " +
                " ON p.u2_id = c.u2_id INNER JOIN[Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] e ON c.EE_ID = e.u2_id " +
                " LEFT JOIN [Welland_Export_Cloud].[dbo].[LOCATION_CODESS] lc " +
                " ON lc.Client_ID = c.Client_ID AND c.Home_Loc_Code = lc.Loc_Code " +
                " LEFT JOIN [Welland_Export_Cloud].[dbo].[CLIENT_DETAILS] cd  ON c.Client_ID = cd.Client_ID " +
                "WHERE  c.Client_ID IN (" + clientno + ")", con);


            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    if (!reader.IsDBNull(17))
                    {
                        HrRate = reader.GetDecimal(17);
                    }
                    else
                    {
                        HrRate = 0.00m;
                    }
                    if (!reader.IsDBNull(18))
                    {
                        Sthrs = reader.GetDecimal(18);
                    }
                    else
                    {
                        Sthrs = 0.00m;
                    }
                    EE.Add(reader.GetString(1) + "," + reader.GetString(2).Replace(",", "").Replace("'", "") + "," + reader.GetString(3) + "," + reader.GetString(4) + "," + reader.GetString(5) + "," +
                        "" + reader.GetString(6).ToUpper() + "," + reader.GetString(7).ToUpper() + "," + reader.GetString(8) + "," + reader.GetString(9) + "," + reader.GetString(10) + ","
                        + reader.GetString(11) + "," + reader.GetString(12) + "," + reader.GetString(13) + "," + reader.GetDateTime(14) + "," + HrRate +
                        "," + reader.GetDecimal(18) + ",");
                }
            }
        }

        return EE;

    }

    public static List<string> SqlLookupDocford(string clientno)
    {
        // Console.WriteLine("Sql Start");
        decimal HrRate;
        decimal Sthrs;
        List<string> EE = new List<string>();

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection con = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT  c.Client_ID  ,cd.u2_id 	,cd.Client_Legal_Name ,c.EE_Status_Code, p.Pay_Method_Code , c.EE_ID  " +
                " ,e.EE_First_Name ,e.EE_Last_Name ,p.EE_SSN ,c.Home_Loc_Code ,lc.Loc_Name ,c.Home_Div_Code" +
                ",c.Home_Dept_Code ,c.EE_Job_Code ,c.EE_Last_Hire_Date " +
                ",CASE WHEN c.EE_Status_Code = 'T' THEN  c.EE_Status_Date ELSE NULL END as TerminationDate" +
                "  ,c.EE_No ,p.Pay_Hourly_Rate  ,p.Pay_Standard_Hours  " +
                " FROM[Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] c " +
                " LEFT OUTER JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] t " +
                " ON t.EE_ID = c.EE_ID AND c.EE_Status_Date < t.EE_Status_Date  " +

                " INNER JOIN[Welland_Export_Cloud].[dbo].[EMPLOYEE_PAY] p " +
                " ON p.u2_id = c.u2_id INNER JOIN[Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] e ON c.EE_ID = e.u2_id " +
                " LEFT JOIN [Welland_Export_Cloud].[dbo].[LOCATION_CODESS] lc " +
                " ON lc.Client_ID = c.Client_ID AND c.Home_Loc_Code = lc.Loc_Code " +
                " LEFT JOIN [Welland_Export_Cloud].[dbo].[CLIENT_DETAILS] cd  ON c.Client_ID = cd.Client_ID " +
                "WHERE c.EE_Status_Code <> 'T' AND c.Client_ID IN (" + clientno + ") ORDER BY t.EE_Status_Date DESC     ", con);


            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    if (!reader.IsDBNull(17))
                    {
                        HrRate = reader.GetDecimal(17);
                    }
                    else
                    {
                        HrRate = 0.00m;
                    }
                    if (!reader.IsDBNull(18))
                    {
                        Sthrs = reader.GetDecimal(18);
                    }
                    else
                    {
                        Sthrs = 0.00m;
                    }


                    EE.Add(reader.GetString(1) + "," + reader.GetString(2).Replace(",", "").Replace("'", "") + "," + reader.GetString(3) + "," + reader.GetString(4) + "," + reader.GetString(5) + "," +
                        "" + reader.GetString(6).ToUpper() + "," + reader.GetString(7).ToUpper() + "," + reader.GetString(8) + "," + reader.GetString(9) + "," + reader.GetString(10) + ","
                        + reader.GetString(11) + "," + reader.GetString(12) + "," + reader.GetString(13) + "," + reader.GetDateTime(14) + "," + HrRate +
                        "," + reader.GetDecimal(18) + ",");

                    //  Console.WriteLine(reader.GetString(1) + " " + reader.GetString(2) + " " + reader.GetString(3) + " " + reader.GetString(4));
                }
            }

        }
        return EE;

    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        String ff = DropDownList1.SelectedItem.Value.ToString();
       
        TextBox1.Text = ff;
        TextBox3.Text = "";
        Button1.Enabled = true;
        Label2.Visible = true;
         Deletefiles(ff);  //Clear Folder before add new files

        FileUpload1.Visible = true;
        StatusLabel.Text = ""; 
        // MessageBox.Text = "Summary Report Under maintain,  temporarily unavailable";
        ListBox1.Items.Clear();  

    }

    protected void TextBox2_TextChanged(object sender, EventArgs e)
    {

    }
    // ===============================SUMMARY REPORT
    protected void Button2_Click(object sender, EventArgs e)
    {
        string cli = TextBox1.Text;
        DateTime myDate = DateTime.Parse(TextBox2.Text);
        string payday = myDate.ToString("M/d/yyyy");

        Response.Redirect("http://tpaa-pdb-sql01/ReportServer_MBASQL/Pages/ReportViewer.aspx?%2FLIVE%2FSummary1&ClientNo=" + cli + "&PayDate=" + payday + "%2012%3A00%3A00%20AM");
    }


    //===================================EXCEPTION BUTTON 
    protected void Button3_Click(object sender, EventArgs e)
    {
        // string cli = TextBox1.Text;
        DateTime myDate = DateTime.Parse(TextBox2.Text);
        string payday = myDate.ToString("M/d/yyyy");
        string cli = TextBox1.Text;

        Response.Redirect("http://tpaa-pdb-sql01/ReportServer_MBASQL/Pages/ReportViewer.aspx?%2FLIVE%2FException4&ClientNo=" + cli + "&PayDate="+ payday + "%2012%3A00%3A00%20AM" );

    }

    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        Button4.Enabled = true;
        MessageBox.Text = "";
        
        TextBox3.Text = "";
        string line = "";
        string ssn = "";
        string st1 = "";
        string st2 = "";
        string st3 = "";
        string codepos = "";
        string hourpos = "";
        string Amountpos = "";
        string Loc = "";
        string Dept = "";
        string Div = "";
        string Job = "";
        string EEname ="";
        string EE_ID = "";
        string EE_No = "";
        string EEstat = "";
        int co = 0;

        string ClientNo = "";


        //ClientNo = TextBox1.Text;
        if (ListBox1.SelectedValue.Substring(0, 2) == "Pr")
        {
            ClientNo = ListBox1.SelectedValue.Substring(8, 6);
            TextBox1.Text = ClientNo;
        }
        else
        {
            ClientNo = DropDownList1.SelectedValue;
            TextBox1.Text = ClientNo;
        }


        List<string> PE = new List<string>();
            if (ClientNo == "013161_013248_013494")
            {
                PE = SqlLookup("013161,013248,013494");
            }
            else
            {
                if (ClientNo == "DOCFOR")
                {
                PE = SqlLookup("013090,012886,012887,013091,013703, 014034");
                }
                else
                PE = SqlLookup(ClientNo);
            }

            if (ClientNo != "DOCFOR")
            {


            string payday = TextBox2.Text;
            var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];

            //--------Clean exist data on Prismimport2

            using (SqlConnection dbConndel = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                dbConndel.Open();
                SqlCommand cdel = new SqlCommand("DELETE FROM [Bridge].[tdi].[PrismImport2] " +
                "WHERE ClientID ='" + ClientNo + "' AND PayDate = '" + payday + "' ", dbConndel);
                cdel.ExecuteNonQuery();
                dbConndel.Close();
            }

            string selectedfile = ListBox1.SelectedValue;
            string filePath = Server.MapPath(string.Format("~/TDI/" + DropDownList1.SelectedValue + "/{0}", selectedfile));
            co = 0;
            // MessageBox.Text = filePath;
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {
                co++;

                MessageBox.Text += line + "\r\n";
                string[] ex = line.Split(',');

                // if (co > 1 && ssn != ex[0].Trim().Replace("-", ""))
                if (co > 1)
                {
                    ssn = ex[0].Trim().Replace("-", "");
                    codepos = ex[1].Trim();
                    hourpos = ex[2].Trim();
                    Amountpos = ex[3].Trim();
                    Loc = ex[4].Trim();
                    Dept = ex[5].Trim();
                    Div = ex[6].Trim();
                    Job = ex[7].Trim();
                    EEname = ex[11].Trim().Replace("\"", "").Replace("'", "");
                    EE_ID = ex[9].Trim();
                    EE_No = ex[10].Trim();
                    EEstat = ex[12];
                    st1 = "yok";
                    st2 = "term";
                    st3 = "hourly";

                    if (Amountpos == "")
                        Amountpos = "0.00";

                    if (hourpos == "")
                        hourpos = "0.00";
                    //--add on SUMMARY PrismImport2  DIRECTLY ------------------------------------------------------

                    if (ClientID != "013066")
                    {
                        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                        {
                            dbConnection.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismImport2]   " +
                            " (ClientID, EEID,EENo, PayDate, SSN, EEName, Codepos, HoursPos, AmountPos, Loc, Dept, Div, Job, EEstatus) " +
                              " VALUES ( '" + ClientNo + "', '" + EE_ID + "', '" + EE_No + "', '" + payday + "', '" + ssn + "', '" + EEname + "', '" + codepos + "', " + hourpos + ", " + Amountpos + ", '" + Loc + "', '" + Dept + "', '" + Div + "','" + Job + "', '" + EEstat + "')", dbConnection);

                            cmd.ExecuteNonQuery();
                            dbConnection.Close();
                        }
                    }


                    //-------------------------------------------------------------------------------------------------
                    foreach (string f in PE)
                    {
                        string[] p = f.Split(',');
                        if (ssn == p[7].Replace("-", ""))
                        {
                            st1 = "bulundu";

                            if (p[2] != "T")
                            {
                                st2 = "active";
                            }

                            if (p[3] == "S")
                            {
                                st3 = "salary";
                            }

                        }
                    }
                    // ======================================= TERM EMPLOYEE ================ 
                    if (st1 == "bulundu" && st2 == "term")
                    {
                        TextBox3.Text += ex[0] + " " + EE_No + " " + EEname + " Employee Term on Prism  " + "\r\n";

                        using (SqlConnection dbConndel = new SqlConnection(connfromWebconfig.ConnectionString))
                        {
                            dbConndel.Open();
                            SqlCommand cdel = new SqlCommand("DELETE FROM [Bridge].[tdi].[PrismExcept2] " +
                            "WHERE SSN ='" + ssn + "'AND  Job = '" + Job + "' AND  Dept = '" + Dept + "' AND loc = '" + Loc + "' AND Codepos = '" + codepos + "' AND EENo = '" + EE_No + "' AND ClientID ='" + ClientNo + "' AND PayDate = '" + payday + "' ", dbConndel);
                            cdel.ExecuteNonQuery();
                            dbConndel.Close();
                        }

                        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                        {
                            dbConnection.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismExcept2]   " +
                            " (ClientID, EEID,EENo, PayDate, SSN, EEName, Codepos, HoursPos, AmountPos, Loc, Dept, Div, Job, ExceptionReason) " +
                              " VALUES ( '" + ClientNo + "', '" + EE_ID + "', '" + EE_No + "', '" + payday + "', '" + ssn + "', '" + EEname + "', '" + codepos + "', " + hourpos + ", " + Amountpos + ", '" + Loc + "', '" + Dept + "', '" + Div + "','" + Job + "', 'Term Employee')", dbConnection);
                            cmd.ExecuteNonQuery();
                            dbConnection.Close();
                        }
                    }
                    //===================================================================
                    if (st1 == "bulundu" && st3 == "salary")
                    {
                        TextBox3.Text += ex[0] + " " + EE_No + " " + EEname + " Salary Employee  " + "\r\n";
                        using (SqlConnection dbConndel = new SqlConnection(connfromWebconfig.ConnectionString))
                        {
                            dbConndel.Open();
                            SqlCommand cdel = new SqlCommand("DELETE FROM [Bridge].[tdi].[PrismExcept2] " +
                            "WHERE SSN ='" + ssn + "'AND  Job = '" + Job + "' AND  Dept = '" + Dept + "' AND loc = '" + Loc + "' AND Codepos = '" + codepos + "' AND EENo = '" + EE_No + "' AND ClientID ='" + ClientNo + "' AND PayDate = '" + payday + "' ", dbConndel);
                            cdel.ExecuteNonQuery();
                            dbConndel.Close();
                        }

                        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                        {
                            dbConnection.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismExcept2]   " +
                            " (ClientID, EEID,EENo, PayDate, SSN, EEName, Codepos, HoursPos, AmountPos, Loc, Dept, Div, Job, ExceptionReason) " +
                              " VALUES ( '" + ClientNo + "', '" + EE_ID + "', '" + EE_No + "', '" + payday + "', '" + ssn + "', '" + EEname + "', '" + codepos + "', " + hourpos + ", " + Amountpos + ", '" + Loc + "', '" + Dept + "', '" + Div + "','" + Job + "', 'Salary Employee')", dbConnection);
                            cmd.ExecuteNonQuery();
                            dbConnection.Close();
                        }





                    }
                    //=========================================================
                    if (st1 == "yok")
                    {

                        TextBox3.Text += ex[0] + " " + EE_No + " " + EEname + "  Employee Not Found  " + "\r\n";


                        using (SqlConnection dbConndel = new SqlConnection(connfromWebconfig.ConnectionString))
                        {
                            dbConndel.Open();
                            SqlCommand cdel = new SqlCommand("DELETE FROM [Bridge].[tdi].[PrismExcept2] " +
                      "WHERE SSN ='" + ssn + "'AND  Job = '" + Job + "' AND  Dept = '" + Dept + "' AND loc = '" + Loc + "' AND Codepos = '" + codepos + "' AND EENo = '" + EE_No + "' AND ClientID ='" + ClientNo + "' AND PayDate = '" + payday + "' ", dbConndel);
                            cdel.ExecuteNonQuery();
                            dbConndel.Close();
                        }

                        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                        {
                            dbConnection.Open();
                            SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismExcept2]   " +
                            " (ClientID, EEID, EEno, PayDate, SSN, EEName, Codepos, HoursPos, AmountPos, Loc, Dept, Div, Job, ExceptionReason) " +
                             " VALUES (  '" + ClientNo + "', '" + EE_ID + "', '" + EE_No + "', '" + payday + "', '" + ssn + "', '" + EEname + "', '" + codepos + "', " + hourpos + ", " + Amountpos + ", '" + Loc + "', '" + Dept + "', '" + Div + "','" + Job + "', 'Employee Not Found')", dbConnection);

                            cmd.ExecuteNonQuery();
                            dbConnection.Close();
                        }




                    }

                }

            }

            file.Close();

            }

    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        //string cname = TextBox1.Text;
        string cname = DropDownList1.SelectedValue;
        string fname = ListBox1.SelectedValue;

        FileDownload2(cname, fname);

        MessageBox.Text = ListBox1.SelectedValue;
    }

    public void Fillbox(string ClientNo)
    {
        ListBox1.Items.Clear();
       

        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientNo;
        string eachfilenam = "";

        string[] files = Directory.GetFiles(bridgepath);
        foreach (string fil in files)
        {
            eachfilenam = Path.GetFileName(fil);  //just filename no path

            if (eachfilenam.Contains("PrImport") | eachfilenam.Contains("Exception"))
            {
                ListBox1.Items.Add(eachfilenam);
            }
        }




    }


    public void FileDownload2(string ClientNo, string eachfileName)
    {
        List<string> downfile = new List<string>();
        string fline = "";
        string filePath = Server.MapPath(string.Format("~/TDI/" + ClientNo + "/{0}", eachfileName));
        string dfilename = eachfileName.Replace(".csv", "d.csv");
        string filePathdown = Server.MapPath(string.Format("~/TDI/" + ClientNo + "/{0}", dfilename));
        TextBox4.Text = filePathdown;

        if (ClientNo == "013126" || ClientNo == "DOCFOR" || ClientNo == "014237")
        {
            if (ClientNo == "DOCFOR")
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                while ((fline = file.ReadLine()) != null)
                {
                    string[] ex = fline.Split(',');
                    downfile.Add(ex[0] + "," + ex[1] + "," + ex[2] + "," + ex[3] + "," + ex[4] + "," + ex[5] + "," + ex[6] + "," + ex[7] + "," + ex[8] + "," + ex[9] + "," + ex[10] + "," + ex[11] + "," + ex[12] + "," + ex[13]);
                }
                file.Close();
            }


            if (ClientNo == "014237")
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                while ((fline = file.ReadLine()) != null)
                {
                    string[] ex = fline.Split(',');

                    downfile.Add(ex[0] + "," + ex[1] + "," + ex[2] + "," + ex[3] + "," + ex[4] + "," + ex[5] + "," + ex[6] + "," + ex[7] + "," + ex[13]);
                }
                file.Close();
            }


            if (ClientNo == "013126")
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filePath);
                while ((fline = file.ReadLine()) != null)
                {
                    string[] ex = fline.Split(',');

                    downfile.Add(ex[0] + "," + ex[1] + "," + ex[2] + "," + ex[3] + "," + ex[4] + "," + ex[5] + "," + ex[6] + "," + ex[7] + "," + ex[13]);
                }
                file.Close();
            }




        }


        else
        {
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((fline = file.ReadLine()) != null)
            {
                string[] ex = fline.Split(',');
                downfile.Add(ex[0] + "," + ex[1] + "," + ex[2] + "," + ex[3] + "," + ex[4] + "," + ex[5] + "," + ex[6] + "," + ex[7]);
            }
            file.Close();

        }

        using (StreamWriter sw = new StreamWriter(filePathdown))
        {
            foreach (string value in downfile)
            {
                sw.WriteLine(value);
            }
            sw.Close();
        }

        Response.ContentType = "application/csv";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + dfilename);

        Response.WriteFile(filePathdown);
        //Flushing the Response.
        Response.Flush();
        //Deleting the File and ending the Response.
        System.Threading.Thread.Sleep(1000);
        Response.End();
       // MessageBox.Text = eachfileName + " Downloaded Please check your download folder  or Web browser ";
       
        File.Delete(filePathdown);
        return;
    }




    public void FileDownload(string ClientNo, string eachfileName)
    {
      //old not use 
        string filePath = Server.MapPath(string.Format("~/TDI/" + ClientNo + "/{0}", eachfileName));
       

            Response.ContentType = "application/csv";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + eachfileName);

        Response.WriteFile(filePath);
        //Flushing the Response.
        Response.Flush();
        //Deleting the File and ending the Response.
        System.Threading.Thread.Sleep(1000);

        Response.End();
        MessageBox.Text = eachfileName + " Downloaded Please check your download folder  or Web browser ";
        return;



    }

    public void Deletefiles(string ClientNo)
    {
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientNo;
        string[] files = Directory.GetFiles(bridgepath);
        foreach (string fil in files)
        {
            File.Delete(fil);
        }
        return;
    }

    public void DownloadPr(string ClientNo)
    {
        string fileName = "PrImport" + ClientNo + ".csv";
        string filePath = Server.MapPath(string.Format("~/TDI/" + ClientNo + "/{0}", fileName));

        //Content Type and Header.
        Response.ContentType = "application/csv";
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);

        //Writing the File to Response Stream.
        Response.WriteFile(filePath);

        //Flushing the Response.
        Response.Flush();

        //Deleting the File and ending the Response.
        System.Threading.Thread.Sleep(1000);
        File.Delete(filePath);

        System.Threading.Thread.Sleep(1000);
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientNo;
        string[] files = Directory.GetFiles(bridgepath);
        foreach (string fil in files)
        {
            File.Delete(fil);
        }

        Response.End();
        MessageBox.Text = fileName + " Downloaded Please check your download folder  or Web browser ";
        return;
    }

    public void CopyRawFile(string clientno, string fdate)
    {
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + clientno;
        string[] files = Directory.GetFiles(bridgepath);

        foreach (string fil in files)
        {
            string tdifilename = Path.GetFileName(fil);   //just file name not directory
            string Pathfrom = fil;
            //  String inFilename = "Raw" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + tdifilename;
            String inFilename = "Raw" + fdate + "-" + tdifilename;
            string PathTo = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\TDI-Archive\" + clientno + @"\" + inFilename;
            System.IO.File.Copy(Pathfrom, PathTo, true);            
        }

        return;
    }

    public void NotUSEPcsvtoSQL(string ClientID, string fdate)
    {
        String line = "";
        int linecount = 0;
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            // DELETE if duplicate 
            SqlCommand cdel = new SqlCommand("DELETE FROM [Bridge].[tdi].[PrismImport] " +
                "WHERE ClientID ='" + ClientID + "' AND  PayDate = '" + fdate + "' ", dbConnection);
            cdel.ExecuteNonQuery();
            dbConnection.Close();

            dbConnection.Open();
            System.IO.StreamReader file = new System.IO.StreamReader(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientID + "\\PrImport" + ClientID + ".csv");
            while ((line = file.ReadLine()) != null)
            {
                //Console.WriteLine(line);
                // Response.Write(line);

                string[] Ln = line.Split(',');
                if (linecount > 0)

                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismImport]   " +
                        " (ClientID, EEID, PayDate, SSN, Codepos, HoursPos, AmountPos, Loc, Dept, Div, Job) " +
                          " VALUES ( '" + ClientID + "','', '" + fdate + "', '" + Ln[0] + "', '" + Ln[1] + "', " + Ln[2] + ", " + Ln[3] + ", '" + Ln[4] + "', '" + Ln[5] + "', '" + Ln[6] + "','" + Ln[7] + "')" , dbConnection);
                    

                    cmd.ExecuteNonQuery();
                }
                linecount++;
            }
            file.Close();
            dbConnection.Close();
        }
        return;
    }

    public void CsvtoSQL(string ClientNo, string fdate)
    {
        // GET EACH FILE 
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientNo;
        string eachfile = "";
        string line = "";
        string ssn = "";
        string cpos = "";
        decimal hour = 0.00m;
        decimal amount = 0.00m;

        MessageBox.Text = "";
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cdel = new SqlCommand("DELETE FROM [Bridge].[tdi].[PrismImport] " +
              "WHERE ClientID ='" + ClientNo + "' AND  PayDate = '" + fdate + "' ", dbConnection);
            cdel.ExecuteNonQuery();
            dbConnection.Close();
        } 


           // var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {

            dbConnection.Open();

            string[] files = Directory.GetFiles(bridgepath);
            foreach (string fil in files)
            {
                eachfile = Path.GetFileName(fil);  //just filename no path

                if (eachfile.Contains("PrImport"))
                {
                    int headerflag = 0;
                    string cname = eachfile.Substring(8, 6);
                    string EEname = "";
                    System.IO.StreamReader file = new System.IO.StreamReader(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientNo + "\\" + eachfile + "");
                    while ((line = file.ReadLine()) != null)
                    {
                        if (headerflag != 0)
                        {
                            // MessageBox.Text += line + "\r\n";
                            // MessageBox.Text += cname + "\r\n";
                            string[] Ln = line.Split(',');

                            ssn = Ln[0].Replace("-", "");
                            EEname = Ln[11].Replace("'", "").Replace("\"", "");
                            cpos = Ln[1];
                            if (Ln[2] != "" )
                            {                               
                                hour = decimal.Parse(Ln[2], CultureInfo.InvariantCulture.NumberFormat);
                            }
                            else
                            {
                                hour = 0.00m;
                            }
                            // amount

                            if (Ln[3] != "")
                            {                             
                                amount = decimal.Parse(Ln[3], CultureInfo.InvariantCulture.NumberFormat);
                            }
                            else
                            {
                                amount = 0.00m;
                            }

                            SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismImport]   " +
                       " (ClientID, PayDate, SSN, EEName, Codepos, HoursPos, AmountPos, Loc, Dept, Div, Job  ) " +
                       " VALUES ('" + cname + "', '" + fdate + "', '" + ssn + "', '" + EEname + "', '" +  cpos + "' , @col1, @col2, '" + Ln[4] + "', '" + Ln[5] + "', '" + Ln[6] + "', '" + Ln[7] +"' )", dbConnection);
                                                      
                            cmd.Parameters.AddWithValue("@col1", hour);
                            cmd.Parameters.AddWithValue("@col2", amount);
                            cmd.ExecuteNonQuery();                           

                        }

                        headerflag++;

                    }
                    file.Close();
                }
            }

            dbConnection.Close();

        }
        return;
    }


    public void CsvtoSQLEX(string ClientNo, string fdate)
    {
        // GET EACH FILE 
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientNo;
        string eachfile = "";
        string line = "";
        string ssn = "";
        string cpos = "";
        //decimal hour = 0.00m;
        //decimal amount = 0.00m;

        MessageBox.Text = "";
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            string[] files = Directory.GetFiles(bridgepath);
            foreach (string fil in files)
            {
                eachfile = Path.GetFileName(fil);  //just filename no path
                if (eachfile.Contains("Exception"))
                {
                    int headerflag = 0;
                    string cname = eachfile.Substring(9, 6);
                    System.IO.StreamReader file = new System.IO.StreamReader(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\" + ClientNo + "\\" + eachfile + "");
                    while ((line = file.ReadLine()) != null)
                    {
                        if (headerflag != 0)
                        {
                            // MessageBox.Text += line + "\r\n";
                            // MessageBox.Text += cname + "\r\n";
                            string[] Ln = line.Split(',');

                           // ssn = Ln[0].Replace("-", "");
                           /// cpos = Ln[1];                          

                            SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[tdi].[PrismExcept]   " +
                                     " (ClientID, SSN, EE_ID, EE_NO, EmpName, PayDate, Loc, Dept, Div, Job, ReasonForException  ) " +
                                        " VALUES ('" + cname + "', '" + Ln[1] + "', '" + Ln[2] + "', '" + Ln[3] + "', '" + Ln[4] + "', '" + fdate + "','" + Ln[5] + "', '" + Ln[6] +"', '" + Ln[7] + "', '"+ Ln[8] + "', '" + Ln[9] + "'    )", dbConnection);
                                                      
                            cmd.ExecuteNonQuery();
                        }

                        headerflag++;
                    }
                    file.Close();
                }
            }
            dbConnection.Close();
        }
        return;
    }


    protected void Button5_Click(object sender, EventArgs e)  // ---------------test 
    {
        DateTime myDate = DateTime.Parse(TextBox2.Text);
        TextBox3.Text = myDate.ToString("M/d/yyyy");

    }

    // UPLOAD AND MAIN PROGRESS
    protected void Button1_Click(object sender, EventArgs e)
    {
        String ff = DropDownList1.SelectedItem.Value.ToString();
        
        String Pdate = TextBox2.Text;
        string uploadpath = "~/TDI/" + ff + "/";
     
        foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
        {
            string fileName = Path.GetFileName(postedFile.FileName);
            postedFile.SaveAs(Server.MapPath(uploadpath) + fileName);
        }
        StatusLabel.Text = string.Format("{0} files have been uploaded successfully.", FileUpload1.PostedFiles.Count);
      

        TextBox1.Text = ff;
            string Pr = "C" + ff.Substring(0, 6);  //Only 6 Character file and folder name allow 
            switch (Pr)
            {

            case "CDOCFOR":
                CopyRawFile("DOCFOR", Pdate);
                CDOCFOR();
                Fillbox("DOCFOR");
                break;


            case "C014237":
                CopyRawFile("014237", Pdate);
                C014237();    // TDI new 11-03-2021
                Fillbox("014237");
                break;

                /*
            case "C01423S":
                CopyRawFile("01423S", Pdate);
                C01423S();    // TDI new 11-03-2021
                Fillbox("01423S");
                break;
                */



            case "C013126":
                CopyRawFile("013126", Pdate);
                C013126();    // TDI new 6-16-2021
                Fillbox("013126");
                break;








            case "C013090":
                CopyRawFile("013090", Pdate);
                C013090();    // TDI Progress gokhan
                Fillbox("013090");
             //  CsvtoSQL("013090", Pdate);
                break;


            case "C013738":                   
                    CopyRawFile("013738", Pdate);
                    C013738();    // TDI Progress gokhan
                    Fillbox("013738");
                  //   CsvtoSQL("013738", Pdate);                       
                    break;

             case "C012995":
                CopyRawFile("012995", Pdate);
                C012995();    // TDI Progress Coody original Roadhous Gokhan
                Fillbox("012995");
               // CsvtoSQL("012995", Pdate);
                break;


             case "C013874":
                CopyRawFile("013874", Pdate);
                C013874();    //  Harbor manange  gokhan               
                Fillbox("013874");
               // CsvtoSQL("013874", Pdate);
                break;

              case "C218113":
                    CopyRawFile("218113", Pdate);
                    C218113();    // TDI Progress
                    Fillbox("218113");
                //    CsvtoSQL("218113", Pdate);
                   
                    break;
                 
              case "C011820":
                    CopyRawFile("011820", Pdate);
                    C011820();    // TDI Progress                                  
                    Fillbox("011820");
                  //  CsvtoSQL("011820", Pdate);
                break;

             case "C014137":
                    CopyRawFile("014137", Pdate);
                    C014137();    // TDI Progress                            
                    Fillbox("014137");
                   // CsvtoSQL("014137", Pdate);
                break;
           
             case "C218230":
                 
                    CopyRawFile("218230", Pdate);
                    C218230();    // TDI Progress                              
                    Fillbox("218230");
                  //  CsvtoSQL("218230", Pdate);
                //    CsvtoSQLEX("218230", Pdate);
                break;

             case "C013780":
              
                CopyRawFile("013780", Pdate);
                    C013780();    // TDI Progress
                    Fillbox("013780");
                  //  CsvtoSQL("013780", Pdate);
                 //   CsvtoSQLEX("013780", Pdate);
                break;

             case "C013925":
              
                    CopyRawFile("013925", Pdate);               
                    C013925();    // TDI REHAB
                    Fillbox("013925");
                 //   CsvtoSQL("013925", Pdate);
              //  CsvtoSQLEX("013925", Pdate);
                break;


            case "C012804":
                CopyRawFile("012804", Pdate);  //new excel one 
                C012804();
                Fillbox("012804");
               // CsvtoSQL("012804", Pdate);
                break;

                
             case "C013455":
               
                CopyRawFile("013455", Pdate);  //new excel one 
                C013455();
                Fillbox("013455");
               // CsvtoSQL("013455", Pdate);
               // CsvtoSQLEX("013455", Pdate);

                break;

              
             case "C013623":
             
                CopyRawFile("013623", Pdate);  //new excel one 
                C013623();
                Fillbox("013623");
               // CsvtoSQL("013623", Pdate);
               // CsvtoSQLEX("013623", Pdate);
                break;

            case "C013487":
              
                CopyRawFile("013487", Pdate);  //new excel one 
                C013487();
                CsvtoSQL("013487", Pdate);
                Fillbox("013487");
               // CsvtoSQLEX("013487", Pdate);
                break;
               

             case "C013397":              
                CopyRawFile("013397", Pdate);  
                C013397();               
                Fillbox("013397");
              //  CsvtoSQL("013397", Pdate);
              //  CsvtoSQLEX("013397", Pdate);
                break;


            case "C013345":
                CopyRawFile("013345", Pdate);
                C013345();
                Fillbox("013345");
              //  CsvtoSQL("013345", Pdate);
              //  CsvtoSQLEX("013345", Pdate);
                break;

            case "C013350":
                CopyRawFile("013350", Pdate);
                C013350();
                Fillbox("013350");
              //  CsvtoSQL("013350", Pdate);
              //  CsvtoSQLEX("013350", Pdate);
                break;






            case "C218260":
                 CopyRawFile("218260", Pdate);
                 C218260();
                Fillbox("218260");
              //  CsvtoSQL("218260", Pdate);
                break;

            case "C013164":
                CopyRawFile("013164", Pdate);
                C013164();              
                Fillbox("013164");
              //  CsvtoSQL("013164", Pdate);
                break;

               
            case "C013743":
              
                CopyRawFile("013743", Pdate);
                C013743();
                Fillbox("013743");              
              //  CsvtoSQL("013743", Pdate);
              //  CsvtoSQLEX("013743", Pdate);
                break;
                
            case "C218197":
                CopyRawFile("218197", Pdate);
                C218197();
                Fillbox("218197");
              //  CsvtoSQL("218197", Pdate);
                break;               

             case "C012805":
                CopyRawFile("012805", Pdate);
                C012805();
                Fillbox("012805");
              //  CsvtoSQL("012805", Pdate);
                break;                

             case "C012803":
                CopyRawFile("012803", Pdate);
                C012803();
                Fillbox("012803");
              //  CsvtoSQL("012803", Pdate);
                break;                

             case "C013843":             
                CopyRawFile("013843", Pdate);
                C013843();
                Fillbox("013843");
              //  CsvtoSQL("013843", Pdate);
               // CsvtoSQLEX("013843", Pdate);
                break;
               
             case "C013603":
                CopyRawFile("013603", Pdate);
                C013603();
                Fillbox("013603");
              //  CsvtoSQL("013603", Pdate);
                break;                

            case "C013066":            
                CopyRawFile("013066", Pdate);
                C013066();
                Fillbox("013066");
                //DownloadPr("013066");
               // CsvtoSQL("013066", Pdate);
              //  CsvtoSQLEX("013066", Pdate);
                break;

               // "C013161_013248_013494"
            case "C013161":              
                CopyRawFile("013161_013248_013494", Pdate);
                C013161_013248_013494();
                Fillbox("013161_013248_013494");
              //  CsvtoSQL("013161_013248_013494", Pdate);
             //   CsvtoSQLEX("013161_013248_013494", Pdate);
                break;
                
             case "C013754":             
                CopyRawFile("013754", Pdate);
                C013754();
                Fillbox("013754");             
                break;

            case "C014217":
                CopyRawFile("014217", Pdate);
                C014217();
                Fillbox("014217");
                break;




            case "C013750":
                CopyRawFile("013750", Pdate);
                C013750();
                Fillbox("013750");
                break;

            case "C013745":
                CopyRawFile("013745", Pdate);
                C013745();
                Fillbox("013745");
                break;

            case "C014269":
                CopyRawFile("014269", Pdate);
                C014269();
                Fillbox("014269");
                break;



            case "C013751":
                CopyRawFile("013751", Pdate);
                C013751();
                Fillbox("013751");
                break;

            case "C013736":
                CopyRawFile("013736", Pdate);
                C013736();
                Fillbox("013736");
                break;

            case "C013749":
                CopyRawFile("013749", Pdate);
                C013749();
                Fillbox("013749");
              //  CsvtoSQL("013749", Pdate);
                //   CsvtoSQLEX("013749", Pdate);
                break;


            case "C013429":
                CopyRawFile("013429", Pdate);
                C013429();
                Fillbox("013429");
              //  CsvtoSQL("013429", Pdate);
                break;

            case "C013143":
                CopyRawFile("013143", Pdate);
                C013143();
                Fillbox("013143");             
                break;

            case "C218064":
                CopyRawFile("218064", Pdate);
                C218064();
                Fillbox("218064");
                break;

            case "C218199":
                CopyRawFile("218199", Pdate);
                C218199();
                Fillbox("218199");
                break;


            }
    }



    ///------------------------------------------------------- Client TDI Process -----------------------------------------

    protected void CDOCFOR()
    {
      
        string line = "";
        string datestring = "";
        DateTime minDate = DateTime.Today;
        DateTime w1start = DateTime.Today;
        DateTime w2start = DateTime.Today;
        string time1 = "";
        string time2 = "";
        string ttime = "";
        string Cl = "";
        string wk = "";
        string EEE = "";
        string hr = "";
        string job = "";
        string dept = "";

        string EEname = "";
        string EENo = "";
        string EESSn = "";
        string store = "";
        string storename = "";
        string homestore = "";
        string wkdate = "";
        string EEstatus = "";
        string EESalhour = "";
        string prate = "";
        string phour = "";
        string pos = "";



        decimal h1, h2, hrt;
        decimal tp, t1, t2, t3, t4;
        List<string> exception = new List<string>();
        List<string> alltxt = new List<string>();
        List<string> timeadd = new List<string>();
        List<string> Finalcsv = new List<string>();
        List<string> Finalfix = new List<string>();
        List<string> Finalcsv2 = new List<string>();
        List<string> Payroll = new List<string>();
        List<string> Payroll2 = new List<string>();
        List<string> Payrollfix = new List<string>();

        List<string> EEcsv = new List<string>();
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\DOCFOR";
        //string bridgepath = @"C:\TDI\ALLDFWEEK1";
        string tempFile = "mast.csv";
        List<string> PP = new List<string>();
        PP = SqlLookupDocford("013090, 012886, 012887,013091, 013703, 014034");

        // DELETE Temp File if exist  path    
        if (File.Exists(Path.Combine(bridgepath, tempFile)))
        {
            File.Delete(Path.Combine(bridgepath, tempFile));
        }

        //==================Merge All Files to one 
        string[] files = Directory.GetFiles(bridgepath);
        foreach (string fil in files)
        {

            string tdifilename = Path.GetFileName(fil);   //just file name not directory                         
                                                          // Console.WriteLine(tdifile);
            System.IO.StreamReader file = new System.IO.StreamReader(fil);

            while ((line = file.ReadLine()) != null)
            {
                alltxt.Add(tdifilename + "," + line);
              //  Console.WriteLine(tdifilename + "," + line);
            }
            file.Close();
        }
        
        //--------------------------------BASIC PAYROLL---------------------------------------
        pos = "";
        foreach (string li in alltxt)
        {
            string[] tList = li.Split(',');

            if (tList[2].Trim() == "0")
            {
                EEname = tList[3].Trim().Replace("\"", "") + " " + tList[4].Trim().Replace("\"", "");
                EENo = tList[5].Replace("\"", "");
                EESSn = tList[5].Replace("\"", "");
                store = tList[0];
                decimal des;
                if (Decimal.TryParse(tList[15], out des ))
                {
                    t1 = des;
                }
                else
                {
                    t1 = 0;
                }
                
               // t1 = decimal.Parse(tList[15]);
                t2 = decimal.Parse(tList[20]);
                t3 = decimal.Parse(tList[22]);
                t4 = decimal.Parse(tList[18]);
                tp = t1 - t2 + t3+ t4;
                pos = "e";
                homestore = "not found";
                string ggk = EENo;
                foreach (string gg in PP)
                {
                    string[] glist = gg.Split(',');
                    if (glist[4] == ggk)
                    {
                        homestore = glist[0];

                        EEstatus = glist[2];
                        EESalhour = glist[3];
                        /// -----------------------------------add yeni----------------------
                        int indexdash = 0;
                        int ttlengt = 0;
                        int ttleft = 0;
                        indexdash = store.IndexOf('-');
                        ttlengt = store.Length;
                        ttleft = ttlengt - indexdash;

                        wkdate = store.Substring(indexdash, ttleft);


                        //wkdate = store.Substring(store.Length - 12, 12);
                        wkdate = wkdate.Replace(".csv", "").Replace(".", "/").Replace("-", "");
                        wkdate = wkdate.Replace("/CSV", "").Replace(".", "/").Replace("-", "");
                        ///-------------------------------------go---------------------------

                    }
                }

                if (store.IndexOf("CAPTIVA") != -1)
                { storename = "CAPTIVA"; }

                if (store.IndexOf("DIXIE") != -1)
                { storename = "DIXIE"; }

                if (store.IndexOf("FMB") != -1)
                { storename = "FMB"; }

                if (store.IndexOf("SANIBEL") != -1)
                { storename = "SANIBEL"; }

                if (store.IndexOf("ST PETE") != -1)
                { storename = "ST PETE"; }

                if (store.IndexOf("WHALE") != -1)
                { storename = "WHALE"; }

                if (tp != 0)
                {
                   // Console.WriteLine(store + "," + EEname + "," + EENo + "," + "TIPS,," + tp.ToString() + ",1,,,");
                    Payrollfix.Add(homestore + "," + storename + "," + EEname + "," + EEstatus + "," + EESalhour + "," + EENo + "," + "TIPS,," + tp.ToString() + ",1,,,," + wkdate);

                }
            }

            if (tList[2].Trim() == "1" && pos == "e")
            {

                if (tList[5].Trim() == "1")
                {
                    prate = tList[7].Trim();
                    phour = tList[6].Trim();
                  // Console.WriteLine(store + "," + EEname + "," + EENo + "," + "HOURLY" + "," + phour + ", " + prate + ",1," + tList[4].Trim() + ",," + tList[3].Trim());
                    Payrollfix.Add(homestore + "," + storename + "," + EEname + "," + EEstatus + "," + EESalhour + "," + EENo + "," + "HOURLY" + "," + phour + ", " + prate + ",1," + tList[4].Trim() + ",," + tList[3].Trim() + "," + wkdate);
                }
                if (tList[5].Trim() == "3")
                {
                    phour = tList[6].Trim();
                  //  Console.WriteLine(store + "," + EEname + "," + EENo + "," + "OT10" + "," + phour + ", " + prate + ",1," + tList[4].Trim() + ",," + tList[3].Trim());
                    Payrollfix.Add(homestore + "," + storename + "," + EEname + "," + EEstatus + "," + EESalhour + "," + EENo + "," + "OT10" + "," + phour + ", " + prate + ",1," + tList[4].Trim() + ",," + tList[3].Trim() + "," + wkdate);
                }


                if (tList[2].Trim() == "2")
                {
                    pos = "";
                    prate = "";
                    phour = "";
                    EEname = "";
                    EENo = "";
                    EESSn = "";
                    store = "";

                }

                // Console.WriteLine(store + " " + EEname + "," + EENo + "HOURLY, + tp.ToString() + ",1,,,");

            }

        }

          using (StreamWriter sw = new StreamWriter(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\DOCFOR\PrImportDOCFOR.csv"))
        {
            sw.WriteLine("PrismClient, Source, EmpName, EmpStatus, Paystatus, EEID, PayCode, HourCode, AmountCode, Loc, Dept,Div,JobCode,WkDate");
            foreach (string value in Payrollfix)
            {
              
                sw.WriteLine(value);
            }
            sw.Dispose();
            sw.Close();
        }

        return;
    }


    protected void C014237OLD()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014237\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\014237\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '014237' AND ssn.EE_SSN='" + Convert.ToInt32(CsvData.ItemArray[0].ToString()).ToString("###-##-####").PadLeft(11, '0') + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = Convert.ToInt32(CsvData.ItemArray[0].ToString()).ToString("###-##-####").PadLeft(11, '0');
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0");
                        drReg["Loc"] = CsvData.ItemArray[1].ToString().PadLeft(6, '0');

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") * 1.5);
                        drOT["Loc"] = CsvData.ItemArray[1].ToString().PadLeft(6, '0');

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0") > 0)
                    {
                        DataRow drVAC = dtPrismTDI.NewRow();
                        drVAC["Ssn"] = drEXC["SSN"];
                        drVAC["CodePos"] = "VACATION";
                        drVAC["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        drVAC["AmountPos"] = string.Empty;
                        drVAC["Loc"] = CsvData.ItemArray[1].ToString().PadLeft(6, '0');

                        drVAC["ClientID"] = drEXC["ClientID"];
                        drVAC["EE_ID"] = drEXC["EE_ID"];
                        drVAC["EE_NO"] = drEXC["EE_NO"];
                        drVAC["EmpName"] = drEXC["EmpName"];
                        drVAC["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drVAC);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014237\PrImport014237.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\014237\PrImport014237.csv");

        //Close SQL connection
        SQLConnectionClose();
    }


    protected void C014237()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014237\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\014237\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                //Update GK 4-1-2022

                string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                     "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                     "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                     " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                     " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                     " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                     " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                     " ON com.[EE_ID]=per.[EE_ID] " +
                     " WHERE com.Client_ID= '014237' ";
                DataTable dtLookUpList = SQLGetTableData(strSQLQuery);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    DataRow[] drLookUp = dtLookUpList.Select("EE_SSN='" + CsvData.ItemArray[0].ToString() + "'");

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((drLookUp.Length == 0) || (drLookUp.Length == 1 && drLookUp[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (drLookUp.Length == 0)
                        {
                            drEXC["SSN"] = CsvData.ItemArray[0].ToString();
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (drLookUp.Length == 1 && drLookUp[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = drLookUp[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = drLookUp[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = drLookUp[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = drLookUp[0].ItemArray[7].ToString().Trim() + " " + drLookUp[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = drLookUp[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = drLookUp[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = drLookUp[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = drLookUp[0].ItemArray[7].ToString().Trim() + " " + drLookUp[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") > 0)
                    {
                        DataRow drPayRec = dtPrismTDI.NewRow();
                        drPayRec["Ssn"] = drEXC["SSN"];
                        //                            drPayRec["CodePos"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? CsvData.ItemArray[2].ToString() : "BADPayType";
                        drPayRec["CodePos"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? (CsvData.ItemArray[2].ToString().Contains("SD-EVE") ? "HOURLY" : CsvData.ItemArray[2].ToString()) : "BADPayType";
                        drPayRec["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                        drPayRec["AmountPos"] = "";
                        drPayRec["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drPayRec["Shift"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? (CsvData.ItemArray[2].ToString().Contains("SD-EVE") ? CsvData.ItemArray[2].ToString() : "") : "BADPayType";

                        drPayRec["ClientID"] = drEXC["ClientID"];
                        drPayRec["EE_ID"] = drEXC["EE_ID"];
                        drPayRec["EE_NO"] = drEXC["EE_NO"];
                        drPayRec["EmpName"] = drEXC["EmpName"];
                        drPayRec["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drPayRec);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014237\PrImport014237.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\014237\PrImport014237.csv");

        //Close SQL connection
        SQLConnectionClose();



    }


    protected void C013143()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013143\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013143\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();
                resultDT.Clear();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013143' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013143\PrImport013143.csv");
       
        SQLConnectionClose();


    }

    protected void C218064()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218064\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\218064\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();
                resultDT.Clear();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '218064' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218064\PrImport218064.csv");

        SQLConnectionClose();

    }

    protected void C218199()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218199\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\218199\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();
                resultDT.Clear();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '218199' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218199\PrImport218199.csv");

        SQLConnectionClose();


    }







    protected void C013126()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013126\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013126\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                //no headers to remove

                string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                     "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                     "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                     " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                     " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                     " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                     " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                     " ON com.[EE_ID]=per.[EE_ID] " +
                     " WHERE com.Client_ID= '013126' ";
                DataTable dtLookUpList = SQLGetTableData(strSQLQuery);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    DataRow[] drLookUp = dtLookUpList.Select("EE_SSN='" + CsvData.ItemArray[0].ToString() + "'");

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((drLookUp.Length == 0) || (drLookUp.Length == 1 && drLookUp[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (drLookUp.Length == 0)
                        {
                            drEXC["SSN"] = CsvData.ItemArray[0].ToString();
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (drLookUp.Length == 1 && drLookUp[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = drLookUp[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = drLookUp[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = drLookUp[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = drLookUp[0].ItemArray[7].ToString().Trim() + " " + drLookUp[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = drLookUp[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = drLookUp[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = drLookUp[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = drLookUp[0].ItemArray[7].ToString().Trim() + " " + drLookUp[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") > 0)
                    {
                        DataRow drPayRec = dtPrismTDI.NewRow();
                        drPayRec["Ssn"] = drEXC["SSN"];
                        //                            drPayRec["CodePos"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? CsvData.ItemArray[2].ToString() : "BADPayType";
                        drPayRec["CodePos"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? (CsvData.ItemArray[2].ToString().Contains("SD-EVE") ? "HOURLY" : CsvData.ItemArray[2].ToString()) : "BADPayType";
                        drPayRec["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                        drPayRec["AmountPos"] = "";
                        drPayRec["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(5, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(5, '0') : string.Empty;

                        drPayRec["Shift"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? (CsvData.ItemArray[2].ToString().Contains("SD-EVE") ? CsvData.ItemArray[2].ToString() : "") : "BADPayType";

                        drPayRec["ClientID"] = drEXC["ClientID"];
                        drPayRec["EE_ID"] = drEXC["EE_ID"];
                        drPayRec["EE_NO"] = drEXC["EE_NO"];
                        drPayRec["EmpName"] = drEXC["EmpName"];
                        drPayRec["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drPayRec);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013126\PrImport013126.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013126\PrImport013126.csv");

        //Close SQL connection
        SQLConnectionClose();


    }





    protected void C013090()
    {
      

        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013090";

        string tdifilename = "";
        string line = "";
        int co = 0;
        string Client = "013090";
        string EEID = "";
        string EEstat = "";
        string Hourly = "";
        string ssn = "";
        string Fname = "";
        string Lname = "";
        string Eloc = "";
        string Ejob = "";
        decimal t1, t2, tp;
        string findflag = "";

        List<string> PP = new List<string>();
        PP = SqlLookup("012886");

        string[] files = Directory.GetFiles(bridgepath);
        tdifilename = Path.GetFileName(files[0]);   //just file name not directory  

        var finalcsv = new List<string>() { "Ssn,CodePos,HoursPos,AmountPos,Loc,Dept,Div,Job" };
        var Exceptioncsv = new List<string>() { "ClientID,SSN,EE_ID,EE_NO,EmpName,PayDate,Loc,Dept,Div,Job, ReasonForException,Shift" };

        var JobList = new List<string>() { "BOS","SERVER","TRAINER SERVER","KEY","BARTENDER","BUSSER","DonotUSE", "BAR COCKTAIL", "LINE COOK",
            "PREP", "DISHWASHER","MAINTANCE", "MANAGER","TRAINEE BAR", "TRAINER BUSSER", "TRAINEE SERVER", "RUNNER", "CLEANING", "MEETING", "HOST",
            "TRAINEE HOST", "ASST MANEGER", "KITCHEN", "TRAINER BAR", "EXPO", "TRAINER HOST", "TRAINER RUNNER", "CATERING", "CLASSROOM", "BAR DINKINS",
            "VALET", "TRAINEE BUSSER", "TRAINEE RUNNER", "BAR TOGO", "BAGGER" };
       var DeptList = new List<string>() { "BOS", "SERVER", "BAR", "KITCHEN", "ADMIN", "HOST", "ASSIST", "HOUSE", "CATERING", "VALET" };

        System.IO.StreamReader file = new System.IO.StreamReader(files[0]);
        while ((line = file.ReadLine()) != null)
        {
            string[] lineList = line.Split(',');

            if (lineList[1].Replace(" ","") == "0")  //EE name row
            {
                Fname = "";
                Lname = "";
                ssn = "";
                EEID = "";
                EEstat = "";
                Hourly = "";
                Eloc = "";
                Ejob = "";
                findflag = "noEE";
            }

            if (lineList[1].Replace(" ","") == "0")  //EE name row
            {
                Fname = lineList[3].Replace("\"", "").ToUpper();
                Lname = lineList[2].Replace("\"", "").ToUpper();
                EEID = lineList[4].Replace("\"", "").ToUpper();
                ssn = lineList[5].Replace("\"", "").ToUpper();

                if (ssn == "000-00-0000")
                {
                    if (EEID == "") //ssn ve EEid yoksa
                    {
                        foreach (string h in PP)  //name Find
                        {
                            if ((h.ToUpper().Contains(Fname)) && (h.ToUpper().Contains(Lname)))
                            {
                                //  Console.WriteLine(h);
                                string[] sqlList = h.Split(',');
                                ssn = sqlList[5];
                                EEID = sqlList[2];
                                EEstat = sqlList[0];
                                Hourly = sqlList[1];
                                Eloc = sqlList[6];
                                Ejob = sqlList[7];
                                //  Console.WriteLine(EEstat + " " + Hourly + " " + ssn + " " + EEID + " " + Fname + " " + Lname);
                                findflag = "ok";
                            }
                        }
                    }
                    else   //EE id varsa 
                    {
                        foreach (string h in PP)  //name Find
                        {
                            if (h.ToUpper().Contains(EEID))
                            {
                                //  Console.WriteLine(h);
                                string[] sqlList = h.Split(',');
                                ssn = sqlList[5];
                                EEID = sqlList[2];
                                EEstat = sqlList[0];
                                Hourly = sqlList[1];
                                Eloc = sqlList[6];
                                Ejob = sqlList[7];
                                //   Console.WriteLine(EEstat + " " + Hourly + " " + ssn + " " + EEID + " " + Fname + " " + Lname);
                                findflag = "ok";
                            }
                        }
                    }
                }
                else
                {
                    foreach (string h in PP)  //name Find
                    {
                        if (h.ToUpper().Contains(ssn))
                        {
                            //  Console.WriteLine(h);
                            string[] sqlList = h.Split(',');
                            ssn = sqlList[5];
                            EEID = sqlList[2];
                            EEstat = sqlList[0];
                            Hourly = sqlList[1];
                            Eloc = sqlList[6];
                            Ejob = sqlList[7];
                            //  Console.WriteLine(EEstat + " " + Hourly + " " + ssn + " " + EEID + " " + Fname + " " + Lname);
                            findflag = "ok";
                        }
                    }
                }

                if (findflag == "noEE")
                {
                    //Exceptioncsv = new List<string>() { "ClientID,SSN,EE_ID,EE_NO,EmpName,PayDate,Loc,Dept,Div,Job, ReasonForException" };
                    ssn.Replace("-", "");
                    Exceptioncsv.Add(Client + "," + ssn + "," + EEID + ",," + Fname + " " + Lname + ",,,,,Employee Not Found Or Wrong SSN/ID");
                    //  Console.WriteLine(Client + "," + ssn + "," + EEID + ",," + Fname + " " + Lname + ",,,,,Employee Not Found");
                }


                if (lineList[14] != "")   // TIPS 
                {
                    tp = decimal.Parse(lineList[14]);
                    t1 = decimal.Parse(lineList[19]);
                    t2 = decimal.Parse(lineList[21]);
                    tp = tp - t1 + t2;
                    if (tp > 0)
                    {
                        ssn = ssn.Replace("-", "");
                        //{ "ssn,codepos,hourspos,amountpos,loc,dept,div,job,ratepos" };
                        //    Console.WriteLine(ssn + ",TIPS," + "," + tp.ToString() + ",1,,,");                      
                        finalcsv.Add(EEID + ",TIPS," + "," + tp.ToString() + ","+ Eloc +",,,"+Ejob);
                    }
                }

            }


            if (lineList[1].Replace(" ","") == "1")  //EE name row
            {
                if (lineList[4].Replace(" ", "") == "1")    //HOURLY 
                {
                    //   Console.WriteLine(ssn + ",HOURLY," + lineList[5] + "," + lineList[6] + ",1");
                   // string div1 = lineList[2].Replace(" ", "");
                   // int myInt1 = int.Parse(div1);
                    string div2 = lineList[3].Replace(" ", "");
                    int myInt2 = int.Parse(div2);
                    // ssn = ssn.Replace("-", "");
                    // finalcsv.Add(ssn + ",HOURLY," + lineList[5] + "," + lineList[6] + "," + Eloc + "," + DeptList[myInt2] + "," + JobList[myInt1] + "," + Ejob);

                   

                    if (lineList[6].Replace(" ", "") != "0")
                    {
                        finalcsv.Add(EEID + ",HOURLY," + lineList[5] + "," + lineList[6] + "," + Eloc + "," + DeptList[myInt2] + ",," + Ejob);
                    }

                }

                if (lineList[4].Replace(" ", "") == "3")    //OT
                {
                   // string div1 = lineList[2].Replace(" ", "");
                  //  int myInt1 = int.Parse(div1);
                    string div2 = lineList[3].Replace(" ", "");
                    int myInt2 = int.Parse(div2);
                    // ssn = ssn.Replace("-", "");
                    // finalcsv.Add(ssn + ",OT10," + lineList[5] + "," + lineList[6] + "," + Eloc + "," + DeptList[myInt2] + "," + JobList[myInt1] + "," + Ejob);
                  

                    if (lineList[6].Replace(" ", "") != "0")                    
                    { 
                        finalcsv.Add(EEID + ",OT10," + lineList[5] + "," + lineList[6] + "," + Eloc + "," + DeptList[myInt2] + ",," + Ejob);
                    }
                }
            }
        }



        file.Close();

        using (StreamWriter sw = new StreamWriter(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013090\PrImport013090.csv"))
        {
            foreach (string value in finalcsv)
            {
                sw.WriteLine(value);
            }
            sw.Dispose();
            sw.Close();
        }

        using (StreamWriter sw = new StreamWriter(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013090\Exception013090.csv"))
        {
            foreach (string value in Exceptioncsv)
            {
                sw.WriteLine(value);
            }
            sw.Dispose();
            sw.Close();
        }

        return;
    }

    protected void C013738()
    {
        string line = "";
       
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013738";
     
        string tdifilename = "";
        string EEname = "";
        string ssn = "";
        decimal t1, t2, tp;

        string[] files = Directory.GetFiles(bridgepath);
        tdifilename = Path.GetFileName(files[0]);   //just file name not directory  

        var finalcsv = new List<string>() { "Ssn,CodePos,HoursPos,AmountPos,Loc,Dept,Div,Job,ClientID,EE_ID,EE_NO,EmpName,ReasonForException,Shift" };       

        System.IO.StreamReader file = new System.IO.StreamReader(files[0]);
        while ((line = file.ReadLine()) != null)
        {          

            if (line.Contains("/") == false)   //no date line 
            {
                if (line.Contains("-") == true)  // ssn  satiri mi  
                {
                    string[] lineList = line.Split(',');
                  
                    ssn = lineList[5].Replace("\"", "").Replace("-", "");
                    EEname = lineList[2].Replace("\"", "") + " " + lineList[3].Replace("\"", "");
                    t1 = decimal.Parse(lineList[10]);
                    t2 = decimal.Parse(lineList[11]);
                    tp = t1 + t2;

                    if (tp != 0)  // if any TIPS on same line 
                    {                      
                        finalcsv.Add(ssn + ",TIPS," + "," + tp.ToString() + ",1,,,,013738,,," + EEname + ",");
                        t2 = 0;
                        tp = 0;
                    }
                }
                else
                {
                    string[] lineAfter = line.Split(',');
                    if (lineAfter[4] == " 1")    //HOURLY 
                    {                       
                        finalcsv.Add(ssn + ",HOURLY," + lineAfter[5].Trim() + "," + lineAfter[9].Trim() + ",1,,,,013738,,," + EEname + ",");
                    }
                    if (lineAfter[4] == " 3")    //OT10
                    {                      
                        finalcsv.Add(ssn + ",OT10," + lineAfter[5].Trim() + "," + lineAfter[9].Trim() + ",1,,,,013738,,," + EEname + ",");
                    }
                }
            }          
        }

        file.Close();
       
        using (StreamWriter sw = new StreamWriter(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013738\PrImport013738.csv"))
        {
            foreach (string value in finalcsv)
            {
                sw.WriteLine(value);
            }
            sw.Dispose();
            sw.Close();
        }          
        
        return;
    }

    protected void C012995()
    {
        string line = "";      
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012995";
        string tdifilename = "";

        string ssn = "";
        string EEname = "";
        decimal t1, t2, tp;

        string[] files = Directory.GetFiles(bridgepath);
        tdifilename = Path.GetFileName(files[0]);   //just file name not directory  

        var finalcsv = new List<string>() { "ssn,codepos,hourspos,amountpos,loc,dept,div,job,ClientID,EE_ID,EE_NO,EmpName,ReasonForException" };

        System.IO.StreamReader file = new System.IO.StreamReader(files[0]);
        while ((line = file.ReadLine()) != null)
        {

            if (line.Contains("/") == false)   //no date line 
            {
                if (line.Contains("-") == true)  // ssn  satiri mi  
                {
                    string[] lineList = line.Split(',');

                    ssn = lineList[5].Replace("\"", "").Replace("-", "");
                    EEname = lineList[2].Replace("\"", "") + " " + lineList[3].Replace("\"", "");
                    t1 = decimal.Parse(lineList[10]);
                    t2 = decimal.Parse(lineList[11]);
                    tp = t1 + t2;

                    if (tp != 0)  // if any TIPS on same line 
                    {
                        finalcsv.Add(ssn + ",TIPS," + "," + tp.ToString() + ",1,,,,012995,,," + EEname + ",");
                        t1 = 0;
                        t2 = 0;
                        tp = 0;
                    }
                }
                else
                {
                    string[] lineAfter = line.Split(',');
                    if (lineAfter[4] == " 1")    //HOURLY 
                    {
                        finalcsv.Add(ssn + ",HOURLY," + lineAfter[5].Trim() + "," + lineAfter[9].Trim() + ",1,,,,012995,,," + EEname + ",");
                    }

                    if (lineAfter[4] == " 3")    //OT10
                    {
                        finalcsv.Add(ssn + ",OT10," + lineAfter[5].Trim() + "," + lineAfter[9].Trim() + ",1,,,,012995,,," + EEname + ",");
                    }
                }
            }
        }

        file.Close();

        using (StreamWriter sw = new StreamWriter(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012995\PrImport012995.csv"))
        {
            foreach (string value in finalcsv)
            {
                sw.WriteLine(value);
            }
        }

        return;
    }

    protected void C013874()
    {
        string line = "";
        string bridgepath = @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013874";
        string tdifilename = "";
        string EEname = "";
        string ssn = "";
        decimal t1, t2, tp;

        string[] files = Directory.GetFiles(bridgepath);
        tdifilename = Path.GetFileName(files[0]);   //just file name not directory  

        var finalcsv = new List<string>() { "ssn,codepos,hourspos,amountpos,loc,dept,div,job,ClientID,EE_ID,EE_NO,EmpName,ReasonForException" };

        System.IO.StreamReader file = new System.IO.StreamReader(files[0]);
        while ((line = file.ReadLine()) != null)
        {

            if (line.Contains("/") == false)   //no date line 
            {
                if (line.Contains("-") == true)  // ssn  satiri mi  
                {
                    string[] lineList = line.Split(',');

                    ssn = lineList[5].Replace("\"", "").Replace("-", "");
                    EEname = lineList[2].Replace("\"", "").Replace("'","") + " " + lineList[3].Replace("\"", "");
                    t1 = decimal.Parse(lineList[10]);
                    t2 = decimal.Parse(lineList[11]);
                    tp = t1 + t2;

                    if (tp != 0)  // if any TIPS on same line 
                    {
                        finalcsv.Add(ssn + ",TIPS," + "," + tp.ToString() + ",1,,,,013874,,," + EEname + ",");
                        t1 = 0;
                        t2 = 0;
                        tp = 0;
                    }
                }
                else
                {
                    string[] lineAfter = line.Split(',');
                    if (lineAfter[4] == " 1")    //HOURLY 
                    {
                        finalcsv.Add(ssn + ",HOURLY," + lineAfter[5].Trim() + "," + lineAfter[9].Trim() + ",1,,,,013874,,," + EEname + ",");
                    }

                    if (lineAfter[4] == " 3")    //OT10
                    {
                        finalcsv.Add(ssn + ",OT10," + lineAfter[5].Trim() + "," + lineAfter[9].Trim() + ",1,,,,013874,,," + EEname + ",");
                    }
                }
            }
        }

        file.Close();

        using (StreamWriter sw = new StreamWriter(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013874\PrImport013874.csv"))
        {
            foreach (string value in finalcsv)
            {
                sw.WriteLine(value);
            }
        }



        return;


    }

    static void C012804()
    {
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012804\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\012804\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 1 header
                newTDITable.Rows[0].Delete();
                newTDITable.AcceptChanges();

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '012804' AND ssn.EE_SSN='" + CsvData.ItemArray[1].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = CsvData.ItemArray[1].ToString();
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[2].ToString().Trim() + " " + CsvData.ItemArray[3].ToString().Trim();
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drReg["Loc"] = "1";

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        //test if daily rate present or not
                        if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                            && (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 0)
                            )
                        {
                            drOT["CodePos"] = "OT05";
                            drOT["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        }
                        else
                        {
                            drOT["CodePos"] = "OT";
                            drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") * 1.5);
                        }
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0");
                        drOT["Loc"] = "1";

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                    {
                        DataRow drVacHrs = dtPrismTDI.NewRow();
                        drVacHrs["Ssn"] = drEXC["SSN"];
                        drVacHrs["CodePos"] = "VACATION";
                        drVacHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                        drVacHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drVacHrs["Loc"] = "1";

                        drVacHrs["ClientID"] = drEXC["ClientID"];
                        drVacHrs["EE_ID"] = drEXC["EE_ID"];
                        drVacHrs["EE_NO"] = drEXC["EE_NO"];
                        drVacHrs["EmpName"] = drEXC["EmpName"];
                        drVacHrs["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drVacHrs);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0") > 0)
                    {
                        DataRow drHolHrs = dtPrismTDI.NewRow();
                        drHolHrs["Ssn"] = drEXC["SSN"];
                        drHolHrs["CodePos"] = "HOLIDAY";
                        drHolHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0");
                        drHolHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drHolHrs["Loc"] = "1";

                        drHolHrs["ClientID"] = drEXC["ClientID"];
                        drHolHrs["EE_ID"] = drEXC["EE_ID"];
                        drHolHrs["EE_NO"] = drEXC["EE_NO"];
                        drHolHrs["EmpName"] = drEXC["EmpName"];
                        drHolHrs["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drHolHrs);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0") > 0)
                    {
                        DataRow drSickHrs = dtPrismTDI.NewRow();
                        drSickHrs["Ssn"] = drEXC["SSN"];
                        drSickHrs["CodePos"] = "SICK-HRLY";
                        drSickHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0");
                        drSickHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drSickHrs["Loc"] = "1";

                        drSickHrs["ClientID"] = drEXC["ClientID"];
                        drSickHrs["EE_ID"] = drEXC["EE_ID"];
                        drSickHrs["EE_NO"] = drEXC["EE_NO"];
                        drSickHrs["EmpName"] = drEXC["EmpName"];
                        drSickHrs["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drSickHrs);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                    {
                        DataRow drCommAmt = dtPrismTDI.NewRow();
                        drCommAmt["Ssn"] = drEXC["SSN"];
                        drCommAmt["CodePos"] = "COMM-R";
                        drCommAmt["HoursPos"] = string.Empty;
                        drCommAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        drCommAmt["Loc"] = "1";

                        drCommAmt["ClientID"] = drEXC["ClientID"];
                        drCommAmt["EE_ID"] = drEXC["EE_ID"];
                        drCommAmt["EE_NO"] = drEXC["EE_NO"];
                        drCommAmt["EmpName"] = drEXC["EmpName"];
                        drCommAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drCommAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0") > 0)
                    {
                        DataRow drExpAmt = dtPrismTDI.NewRow();
                        drExpAmt["Ssn"] = drEXC["SSN"];
                        drExpAmt["CodePos"] = "EXPREIM";
                        drExpAmt["HoursPos"] = string.Empty;
                        drExpAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0");
                        drExpAmt["Loc"] = "1";

                        drExpAmt["ClientID"] = drEXC["ClientID"];
                        drExpAmt["EE_ID"] = drEXC["EE_ID"];
                        drExpAmt["EE_NO"] = drEXC["EE_NO"];
                        drExpAmt["EmpName"] = drEXC["EmpName"];
                        drExpAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drExpAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                    {
                        DataRow drProdBonusAmt = dtPrismTDI.NewRow();
                        drProdBonusAmt["Ssn"] = drEXC["SSN"];
                        drProdBonusAmt["CodePos"] = "BONUSN-RT";
                        drProdBonusAmt["HoursPos"] = string.Empty;
                        drProdBonusAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drProdBonusAmt["Loc"] = "1";

                        drProdBonusAmt["ClientID"] = drEXC["ClientID"];
                        drProdBonusAmt["EE_ID"] = drEXC["EE_ID"];
                        drProdBonusAmt["EE_NO"] = drEXC["EE_NO"];
                        drProdBonusAmt["EmpName"] = drEXC["EmpName"];
                        drProdBonusAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drProdBonusAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0") > 0)
                    {
                        DataRow drDiscrBonusAmt = dtPrismTDI.NewRow();
                        drDiscrBonusAmt["Ssn"] = drEXC["SSN"];
                        drDiscrBonusAmt["CodePos"] = "BONUSD-ST";
                        drDiscrBonusAmt["HoursPos"] = string.Empty;
                        drDiscrBonusAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0");
                        drDiscrBonusAmt["Loc"] = "1";

                        drDiscrBonusAmt["ClientID"] = drEXC["ClientID"];
                        drDiscrBonusAmt["EE_ID"] = drEXC["EE_ID"];
                        drDiscrBonusAmt["EE_NO"] = drEXC["EE_NO"];
                        drDiscrBonusAmt["EmpName"] = drEXC["EmpName"];
                        drDiscrBonusAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drDiscrBonusAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                    {
                        DataRow drDailyHrs = dtPrismTDI.NewRow();
                        drDailyHrs["Ssn"] = drEXC["SSN"];
                        drDailyHrs["CodePos"] = "DAILYRATE";
                        drDailyHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drDailyHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                        drDailyHrs["Loc"] = "1";

                        drDailyHrs["ClientID"] = drEXC["ClientID"];
                        drDailyHrs["EE_ID"] = drEXC["EE_ID"];
                        drDailyHrs["EE_NO"] = drEXC["EE_NO"];
                        drDailyHrs["EmpName"] = drEXC["EmpName"];
                        drDailyHrs["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drDailyHrs);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drSal = dtPrismTDI.NewRow();
                        drSal["Ssn"] = drEXC["SSN"];
                        drSal["CodePos"] = "SALARY";
                        drSal["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drSal["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drSal["Loc"] = "1";

                        drSal["ClientID"] = drEXC["ClientID"];
                        drSal["EE_ID"] = drEXC["EE_ID"];
                        drSal["EE_NO"] = drEXC["EE_NO"];
                        drSal["EmpName"] = drEXC["EmpName"];
                        drSal["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drSal);
                    }

                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012804\PrImport012804.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\012804\PrImport012804.csv");

        //Close SQL connection
        SQLConnectionClose();



    }

    static void C218113()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218113\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\218113\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, false, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '218113' AND ssn.EE_SSN='" + CsvData.ItemArray[1].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = CsvData.ItemArray[1].ToString();
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim() + " " + CsvData.ItemArray[2].ToString().Trim();
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                        drReg["Loc"] = "1";

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0");
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") * 1.5);
                        drOT["Loc"] = "1";

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : "0") > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : "0");
                        drTIPS["Loc"] = "1";

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218113\PrImport218113.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\218113\PrImport218113.csv");

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C011820()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\011820\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\011820\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);
                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '011820' AND ssn.EE_SSN='" + Convert.ToInt32(CsvData.ItemArray[3].ToString()).ToString("###-##-####").PadLeft(11, '0') + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = Convert.ToInt32(CsvData.ItemArray[3].ToString()).ToString("###-##-####").PadLeft(11, '0');
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = CsvData.ItemArray[2].ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                        drReg["AmountPos"] = string.Empty;
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOT["AmountPos"] = string.Empty;
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\011820\PrImport011820.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\011820\PrImport011820.csv");

        //Close SQL connection
        SQLConnectionClose();
    }

    static void C014137()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014137\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\014137\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);
                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '014137' AND ssn.EE_SSN='" + Convert.ToInt32(CsvData.ItemArray[3].ToString()).ToString("###-##-####").PadLeft(11, '0') + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = Convert.ToInt32(CsvData.ItemArray[3].ToString()).ToString("###-##-####").PadLeft(11, '0');
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = CsvData.ItemArray[2].ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                        drReg["AmountPos"] = string.Empty;
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOT["AmountPos"] = string.Empty;
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014137\PrImport014137.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\014137\PrImport014137.csv");

        //Close SQL connection
        SQLConnectionClose();



    }

    static void C218230()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        //PrismImport
        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218230\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\218230\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    //load SSN and LOC
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '218230' AND com.EE_No='" + CsvData.ItemArray[2].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = CsvData.ItemArray[2].ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "File # (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                        //                            drReg["Loc"] = !string.IsNullOrWhiteSpace(dtLookUp.Rows[0].ItemArray[2].ToString()) ? dtLookUp.Rows[0].ItemArray[2].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drOT["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                        //                            drOT["Loc"] = !string.IsNullOrWhiteSpace(dtLookUp.Rows[0].ItemArray[2].ToString()) ? dtLookUp.Rows[0].ItemArray[2].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0") > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        //                            drTIPS["Loc"] = !string.IsNullOrWhiteSpace(dtLookUp.Rows[0].ItemArray[2].ToString()) ? dtLookUp.Rows[0].ItemArray[2].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                    {
                        DataRow drMEMO = dtPrismTDI.NewRow();
                        drMEMO["Ssn"] = drEXC["SSN"];
                        drMEMO["CodePos"] = "MEMO";
                        drMEMO["HoursPos"] = string.Empty;
                        drMEMO["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        //                            drMEMO["Loc"] = !string.IsNullOrWhiteSpace(dtLookUp.Rows[0].ItemArray[2].ToString()) ? dtLookUp.Rows[0].ItemArray[2].ToString() : string.Empty;
                        drMEMO["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : string.Empty;

                        drMEMO["ClientID"] = drEXC["ClientID"];
                        drMEMO["EE_ID"] = drEXC["EE_ID"];
                        drMEMO["EE_NO"] = drEXC["EE_NO"];
                        drMEMO["EmpName"] = drEXC["EmpName"];
                        drMEMO["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drMEMO);
                    }
                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218230\PrImport218230.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\218230\PrImport218230.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218230\Exception218230.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\218230\\Exception218230.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C013455()
    {
        Dictionary<string, string> dictOtherHours = new Dictionary<string, string>
            {
                {"H", "HOLIDAY"},
                {"F", "BEREAVEM"},
                {"J", "JURY"},
                {"P", "PTO"},
                {"E", "NONPAYHRS3"},
              //  {"10", "MILEAGE"},
                {"15", "NONPAYHRS"},
                {"20", "BACKPAY"},
                {"25", "BACKPAYOT"},
                {"128", "HOURLY2"},
                {"127", "HOURLY1"}

// do not add - payroll will add manually 
//                {"19", "COVID"}
            };

        Dictionary<string, string> dictOtherEarnings = new Dictionary<string, string>
            {
                {"E", "MISCPAY"},
                {"10", "EXPREIM4"},
                {"15", "MISCPAY3"},
                {"20", "RETRONH"},
                {"25", "RETRONH"},
                {"30", "RETRONH"},
                {"59", "ONCALL"},
                {"219", "SEVERANCE"}
            };


        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013455\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013455\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    //load SSN and LOC
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013455' AND com.EE_No='" + CsvData.ItemArray[2].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = CsvData.ItemArray[2].ToString();
                            drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim() + " " + CsvData.ItemArray[4].ToString().Trim();
                            drEXC["ReasonForException"] = "File # (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drReg["AmountPos"] = string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drOT["AmountPos"] = string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs1 = dtPrismTDI.NewRow();
                        drOthHrs1["Ssn"] = drEXC["SSN"];
                        drOthHrs1["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty];
                        drOthHrs1["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        drOthHrs1["AmountPos"] = string.Empty;
                        drOthHrs1["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drOthHrs1["ClientID"] = drEXC["ClientID"];
                        drOthHrs1["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs1["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs1["EmpName"] = drEXC["EmpName"];
                        drOthHrs1["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOthHrs1);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs2 = dtPrismTDI.NewRow();
                        drOthHrs2["Ssn"] = drEXC["SSN"];
                        drOthHrs2["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty];
                        drOthHrs2["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drOthHrs2["AmountPos"] = string.Empty;
                        drOthHrs2["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drOthHrs2["ClientID"] = drEXC["ClientID"];
                        drOthHrs2["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs2["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs2["EmpName"] = drEXC["EmpName"];
                        drOthHrs2["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOthHrs2);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs3 = dtPrismTDI.NewRow();
                        drOthHrs3["Ssn"] = drEXC["SSN"];
                        drOthHrs3["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty];
                        drOthHrs3["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                        drOthHrs3["AmountPos"] = string.Empty;
                        drOthHrs3["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drOthHrs3["ClientID"] = drEXC["ClientID"];
                        drOthHrs3["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs3["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs3["EmpName"] = drEXC["EmpName"];
                        drOthHrs3["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOthHrs3);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn1 = dtPrismTDI.NewRow();
                        drOthEarn1["Ssn"] = drEXC["SSN"];
                        drOthEarn1["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty];
                        drOthEarn1["HoursPos"] = string.Empty;
                        drOthEarn1["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0");
                        drOthEarn1["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drOthEarn1["ClientID"] = drEXC["ClientID"];
                        drOthEarn1["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn1["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn1["EmpName"] = drEXC["EmpName"];
                        drOthEarn1["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOthEarn1);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn2 = dtPrismTDI.NewRow();
                        drOthEarn2["Ssn"] = drEXC["SSN"];
                        drOthEarn2["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : string.Empty];
                        drOthEarn2["HoursPos"] = string.Empty;
                        drOthEarn2["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0");
                        drOthEarn2["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drOthEarn2["ClientID"] = drEXC["ClientID"];
                        drOthEarn2["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn2["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn2["EmpName"] = drEXC["EmpName"];
                        drOthEarn2["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOthEarn2);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn3 = dtPrismTDI.NewRow();
                        drOthEarn3["Ssn"] = drEXC["SSN"];
                        drOthEarn3["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[20].ToString()) ? CsvData.ItemArray[20].ToString() : string.Empty];
                        drOthEarn3["HoursPos"] = string.Empty;
                        drOthEarn3["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0");
                        drOthEarn3["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[6].ToString().PadLeft(6, '0') : string.Empty;

                        drOthEarn3["ClientID"] = drEXC["ClientID"];
                        drOthEarn3["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn3["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn3["EmpName"] = drEXC["EmpName"];
                        drOthEarn3["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOthEarn3);
                    }

                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013455\PrImport013455.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013455\PrImport013455.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013455\Exception013455.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013455\Exception013455.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();


    }




    static void C013623OLD()
    {
        Dictionary<string, string> dictOtherHours = new Dictionary<string, string>
            {
                {"H", "HOLIDAY"},
                {"F", "BEREAVEM"},
                {"J", "JURY"},
                {"P", "PTO"},
                {"E", "NONPAYHRS3"},
                {"10", "MILEAGE"},
                {"15", "NONPAYHRS"},
                {"20", "BACKPAY"},
                {"25", "BACKPAYOT"},
                {"128", "HOURLY2"},
                {"127", "HOURLY1"}

// do not add - payroll will add manually 
//                {"19", "COVID"}
            };

        Dictionary<string, string> dictOtherEarnings = new Dictionary<string, string>
            {
                {"E", "MISCPAY"},
                {"10", "EXPREIM4"},
                {"15", "MISCPAY3"},
                {"20", "RETRONH"},
                {"25", "RETRONH"},
                {"30", "RETRONH"},
                {"59", "ONCALL"},
                {"219", "SEVERANCE"}
            };


        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013623\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013623\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    //load SSN and LOC
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013623' AND com.EE_No='" + CsvData.ItemArray[2].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = CsvData.ItemArray[2].ToString();
                            drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim() + " " + CsvData.ItemArray[4].ToString().Trim();
                            drEXC["ReasonForException"] = "File # (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drReg["Loc"] = "1";

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drOT["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOT["Loc"] = "1";

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs1 = dtPrismTDI.NewRow();
                        drOthHrs1["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs1["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[10].ToString().Trim()) && (CsvData.ItemArray[10].ToString().Trim().Length > 0))
                        {
                            drOthHrs1["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "BADHoursCode"];
                            drOthHrs1["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs1["CodePos"] = "BADHoursCode";
                            drOthHrs1["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs1["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        drOthHrs1["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOthHrs1["Loc"] = "1";

                        drOthHrs1["ClientID"] = drEXC["ClientID"];
                        drOthHrs1["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs1["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs1["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs1);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs2 = dtPrismTDI.NewRow();
                        drOthHrs2["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs2["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[12].ToString().Trim()) && (CsvData.ItemArray[12].ToString().Trim().Length > 0))
                        {
                            drOthHrs2["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "BADHoursCode"];
                            drOthHrs2["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs2["CodePos"] = "BADHoursCode";
                            drOthHrs2["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs2["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drOthHrs2["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOthHrs2["Loc"] = "1";

                        drOthHrs2["ClientID"] = drEXC["ClientID"];
                        drOthHrs2["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs2["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs2["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs2);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs3 = dtPrismTDI.NewRow();
                        drOthHrs3["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs3["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[14].ToString().Trim()) && (CsvData.ItemArray[14].ToString().Trim().Length > 0))
                        {
                            drOthHrs3["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "BADHoursCode"];
                            drOthHrs3["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs3["CodePos"] = "BADHoursCode";
                            drOthHrs3["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs3["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                        drOthHrs3["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOthHrs3["Loc"] = "1";

                        drOthHrs3["ClientID"] = drEXC["ClientID"];
                        drOthHrs3["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs3["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs3["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs3);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn1 = dtPrismTDI.NewRow();
                        drOthEarn1["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn1["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[16].ToString().Trim()) && (CsvData.ItemArray[16].ToString().Trim().Length > 0))
                        {
                            drOthEarn1["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "BADEarningsCode"];
                            drOthEarn1["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn1["CodePos"] = "BADEarningsCode";
                            drOthEarn1["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn1["HoursPos"] = "0";
                        drOthEarn1["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0");
                        drOthEarn1["Loc"] = "1";

                        drOthEarn1["ClientID"] = drEXC["ClientID"];
                        drOthEarn1["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn1["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn1["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn1);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn2 = dtPrismTDI.NewRow();
                        drOthEarn2["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn2["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[18].ToString().Trim()) && (CsvData.ItemArray[18].ToString().Trim().Length > 0))
                        {
                            drOthEarn2["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : "BADEarningsCode"];
                            drOthEarn2["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn2["CodePos"] = "BADEarningsCode";
                            drOthEarn2["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn2["HoursPos"] = "0";
                        drOthEarn2["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0");
                        drOthEarn2["Loc"] = "1";

                        drOthEarn2["ClientID"] = drEXC["ClientID"];
                        drOthEarn2["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn2["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn2["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn2);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn3 = dtPrismTDI.NewRow();
                        drOthEarn3["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn3["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[20].ToString()) ? CsvData.ItemArray[20].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[20].ToString().Trim()) && (CsvData.ItemArray[20].ToString().Trim().Length > 0))
                        {
                            drOthEarn3["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[20].ToString()) ? CsvData.ItemArray[20].ToString() : "BADEarningsCode"];
                            drOthEarn3["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn3["CodePos"] = "BADEarningsCode";
                            drOthEarn3["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn3["HoursPos"] = "0";
                        drOthEarn3["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0");
                        drOthEarn3["Loc"] = "1";
                        drOthEarn3["ClientID"] = drEXC["ClientID"];
                        drOthEarn3["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn3["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn3["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn3);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013623\PrImport013623.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013623\PrImport013623.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013623\Exception013623.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013623\Exception013623.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();

    }


    static void C013623()
    {
        Dictionary<string, string> dictOtherHours = new Dictionary<string, string>
            {
                {"H", "HOLIDAY"},
                {"F", "BEREAVEM"},
                {"J", "JURY"},
                {"P", "PTO"},
                {"E", "NONPAYHRS3"},
                {"10", "MILEAGE"},
                {"15", "NONPAYHRS"},
                {"20", "BACKPAY"},
                {"25", "BACKPAYOT"},
                {"128", "HOURLY2"},
                {"127", "HOURLY1"}

// do not add - payroll will add manually 
//                {"19", "COVID"}
            };

        Dictionary<string, string> dictOtherEarnings = new Dictionary<string, string>
            {
                {"E", "MISCPAY"},
                {"10", "EXPREIM4"},
                {"15", "MISCPAY3"},
                {"20", "RETRONH"},
                {"25", "RETRONH"},
                {"30", "RETRONH"},
                {"59", "ONCALL"},
                {"219", "SEVERANCE"}
            };


        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013623\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013623\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    //load SSN and LOC
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013623' AND com.EE_ID='" + CsvData.ItemArray[2].ToString().Trim() + "'";
                    //                        " WHERE com.Client_ID= '013623' AND com.EE_No='" + CsvData.ItemArray[2].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = CsvData.ItemArray[2].ToString().Trim();
                            drEXC["EE_NO"] = "";
                            //                                drEXC["EE_ID"] = "";
                            //                                drEXC["EE_NO"] = CsvData.ItemArray[2].ToString();
                            drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim() + " " + CsvData.ItemArray[4].ToString().Trim();
                            drEXC["ReasonForException"] = "File # (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drReg["Loc"] = "1";

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drOT["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOT["Loc"] = "1";

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs1 = dtPrismTDI.NewRow();
                        drOthHrs1["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs1["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[10].ToString().Trim()) && (CsvData.ItemArray[10].ToString().Trim().Length > 0))
                        {
                            drOthHrs1["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "BADHoursCode"];
                            drOthHrs1["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs1["CodePos"] = "BADHoursCode";
                            drOthHrs1["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs1["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        drOthHrs1["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOthHrs1["Loc"] = "1";

                        drOthHrs1["ClientID"] = drEXC["ClientID"];
                        drOthHrs1["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs1["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs1["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs1);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs2 = dtPrismTDI.NewRow();
                        drOthHrs2["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs2["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[12].ToString().Trim()) && (CsvData.ItemArray[12].ToString().Trim().Length > 0))
                        {
                            drOthHrs2["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "BADHoursCode"];
                            drOthHrs2["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs2["CodePos"] = "BADHoursCode";
                            drOthHrs2["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs2["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drOthHrs2["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOthHrs2["Loc"] = "1";

                        drOthHrs2["ClientID"] = drEXC["ClientID"];
                        drOthHrs2["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs2["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs2["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs2);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs3 = dtPrismTDI.NewRow();
                        drOthHrs3["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs3["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[14].ToString().Trim()) && (CsvData.ItemArray[14].ToString().Trim().Length > 0))
                        {
                            drOthHrs3["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "BADHoursCode"];
                            drOthHrs3["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs3["CodePos"] = "BADHoursCode";
                            drOthHrs3["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs3["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                        drOthHrs3["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        drOthHrs3["Loc"] = "1";

                        drOthHrs3["ClientID"] = drEXC["ClientID"];
                        drOthHrs3["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs3["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs3["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs3);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn1 = dtPrismTDI.NewRow();
                        drOthEarn1["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn1["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[16].ToString().Trim()) && (CsvData.ItemArray[16].ToString().Trim().Length > 0))
                        {
                            drOthEarn1["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "BADEarningsCode"];
                            drOthEarn1["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn1["CodePos"] = "BADEarningsCode";
                            drOthEarn1["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn1["HoursPos"] = "0";
                        drOthEarn1["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0");
                        drOthEarn1["Loc"] = "1";

                        drOthEarn1["ClientID"] = drEXC["ClientID"];
                        drOthEarn1["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn1["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn1["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn1);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn2 = dtPrismTDI.NewRow();
                        drOthEarn2["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn2["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[18].ToString().Trim()) && (CsvData.ItemArray[18].ToString().Trim().Length > 0))
                        {
                            drOthEarn2["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : "BADEarningsCode"];
                            drOthEarn2["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn2["CodePos"] = "BADEarningsCode";
                            drOthEarn2["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn2["HoursPos"] = "0";
                        drOthEarn2["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0");
                        drOthEarn2["Loc"] = "1";

                        drOthEarn2["ClientID"] = drEXC["ClientID"];
                        drOthEarn2["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn2["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn2["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn2);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn3 = dtPrismTDI.NewRow();
                        drOthEarn3["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn3["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[20].ToString()) ? CsvData.ItemArray[20].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[20].ToString().Trim()) && (CsvData.ItemArray[20].ToString().Trim().Length > 0))
                        {
                            drOthEarn3["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[20].ToString()) ? CsvData.ItemArray[20].ToString() : "BADEarningsCode"];
                            drOthEarn3["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn3["CodePos"] = "BADEarningsCode";
                            drOthEarn3["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn3["HoursPos"] = "0";
                        drOthEarn3["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0");
                        drOthEarn3["Loc"] = "1";

                        drOthEarn3["ClientID"] = drEXC["ClientID"];
                        drOthEarn3["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn3["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn3["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn3);
                    }


                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013623\PrImport013623.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013623\PrImport013623.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013623\Exception013623.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013623\Exception013623.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();

    }


    static void C013487()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        Boolean blnFirstRow = true;
        DataTable projTDITable = new DataTable();

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013487\", "*.xls");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013487\", "*.xls");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 12 headers
                for (int i = 11; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    dr.Delete();
                }
                newTDITable.AcceptChanges();

                //add location column and populate it with filename (aka location)
                newTDITable.Columns.Add(new DataColumn("LocationCode", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("SSN", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("EEID", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("EENO", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("ExceptionReason", typeof(string)));
                DataColumn colLOC = newTDITable.Columns["LocationCode"];
                DataColumn colSSN = newTDITable.Columns["SSN"];
                DataColumn colEEID = newTDITable.Columns["EEID"];
                DataColumn colEENO = newTDITable.Columns["EENO"];
                DataColumn colEXCRSN = newTDITable.Columns["ExceptionReason"];
                DataColumn colName = newTDITable.Columns[4];
                foreach (DataRow row in newTDITable.Rows)
                    row[colLOC] = Path.GetFileNameWithoutExtension(empFile);

                //loop to add each empname to all rows for that employee
                string strEmpName = string.Empty;
                string strLastName = string.Empty;
                string strFirstName = string.Empty;
                string strSSN = string.Empty;

                DataRow drEXC = dtPrismTDIEXC.NewRow();

                foreach (DataRow row in newTDITable.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row.ItemArray[4].ToString()))
                    {
                        strEmpName = row.ItemArray[4].ToString();
                        strLastName = strEmpName.Substring(0, strEmpName.IndexOf(","));
                        strFirstName = strEmpName.Substring(strEmpName.IndexOf(",") + 2);
                        ////load SSN and LOC
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013487' and per.EE_Last_Name='" + strLastName.ToString().Trim() + "' and per.EE_First_Name='" + strFirstName.ToString().Trim() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //********************
                        //if empno not found or found but termed, save exception
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = strLastName.ToString().Trim() + " " + strFirstName.ToString().Trim();
                                drEXC["ReasonForException"] = "Employee Name (ee_last_name, ee_first_name) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        //********************
                        row[colName] = drEXC["EmpName"];
                        row[colSSN] = drEXC["SSN"];
                        row[colEEID] = drEXC["EE_ID"];
                        row[colEENO] = drEXC["EE_NO"];
                        row[colEXCRSN] = drEXC["ReasonForException"];
                    }
                    else
                    {
                        row[colName] = drEXC["EmpName"];
                        row[colSSN] = drEXC["SSN"];
                        row[colEEID] = drEXC["EE_ID"];
                        row[colEENO] = drEXC["EE_NO"];
                        row[colEXCRSN] = drEXC["ReasonForException"];
                    }
                }

                //delete totals, emp name hdrs, authorized time and grand total lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if ((dr.ItemArray[14].ToString() == "Total:")
                        || (!string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString()))
                        || (dr.ItemArray[6].ToString() == "Grand Total:")
                        || (dr.ItemArray[6].ToString() == "Authorized Time:"))
                        dr.Delete();
                }
                newTDITable.AcceptChanges();

                //sort the remaining datatable by employee name, by jobcode, by payrate
                DataView dv = newTDITable.DefaultView;
                //sort by empname, by jobcode, by rate
                dv.Sort = "Column5, Column12, Column15";
                DataTable sortedDT = dv.ToTable();

                bool blnOverTime = false;
                double dblCalcHours = 0;
                double dblHoldRate = 0;
                string strHoldEmpName = string.Empty;
                string strHoldSSN = string.Empty;
                string strHoldEEID = string.Empty;
                string strHoldEENO = string.Empty;
                string strHoldEXCRSN = string.Empty;
                string strHoldLoc = string.Empty;
                string strHoldJob = string.Empty;
                blnFirstRow = true;

                foreach (DataRow CsvData in sortedDT.Rows)
                {
                    if (blnFirstRow)
                    {
                        dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString().Replace("$", "") : "0");
                        strHoldEmpName = CsvData.ItemArray[4].ToString();
                        strHoldSSN = CsvData["SSN"].ToString();
                        strHoldEEID = CsvData["EEID"].ToString();
                        strHoldEENO = CsvData["EENO"].ToString();
                        strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                        strHoldLoc = CsvData["LocationCode"].ToString();
                        strHoldJob = !string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : string.Empty;
                        if ((!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty) == "*Overtime*")
                        {
                            blnOverTime = true;
                        }
                        blnFirstRow = false;
                        continue;
                    }

                    if (
                        (strHoldEmpName == CsvData.ItemArray[4].ToString())
                        && (strHoldJob == (!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : string.Empty))
                        && dblHoldRate == double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString().Replace("$", "") : "0")
                        )
                    {
                        dblCalcHours += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                    }
                    else
                    {
                        //test for hourly data
                        if (dblCalcHours > 0)
                        {
                            DataRow drPayReg = dtPrismTDI.NewRow();
                            drPayReg["Ssn"] = strHoldSSN;
                            drPayReg["CodePos"] = blnOverTime ? "OT05" : "HOURLY";
                            drPayReg["HoursPos"] = dblCalcHours;
                            drPayReg["AmountPos"] = dblHoldRate;
                            drPayReg["Loc"] = strHoldLoc;
                            drPayReg["Job"] = strHoldJob;

                            drPayReg["ClientID"] = strClientID;
                            drPayReg["EE_ID"] = strHoldEEID;
                            drPayReg["EE_NO"] = strHoldEENO;
                            drPayReg["EmpName"] = strHoldEmpName;
                            drPayReg["ReasonForException"] = strHoldEXCRSN;

                            dtPrismTDI.Rows.Add(drPayReg);
                        }
                        //load current next data
                        dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString().Replace("$", "") : "0");
                        strHoldEmpName = CsvData.ItemArray[4].ToString();
                        strHoldSSN = CsvData["SSN"].ToString();
                        strHoldEEID = CsvData["EEID"].ToString();
                        strHoldEENO = CsvData["EENO"].ToString();
                        strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                        strHoldLoc = CsvData["LocationCode"].ToString();
                        strHoldJob = !string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : string.Empty;
                        if ((!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty) == "*Overtime*")
                        {
                            blnOverTime = true;
                        }
                        else
                        {
                            blnOverTime = false;
                        }
                    }
                }
                //test for hourly data for last record
                if (dblCalcHours > 0)
                {
                    DataRow drPayReg = dtPrismTDI.NewRow();
                    drPayReg["Ssn"] = strHoldSSN;
                    drPayReg["CodePos"] = blnOverTime ? "OT05" : "HOURLY";
                    drPayReg["HoursPos"] = dblCalcHours;
                    drPayReg["AmountPos"] = dblHoldRate;
                    drPayReg["Loc"] = strHoldLoc;
                    drPayReg["Job"] = strHoldJob;

                    drPayReg["ClientID"] = strClientID;
                    drPayReg["EE_ID"] = strHoldEEID;
                    drPayReg["EE_NO"] = strHoldEENO;
                    drPayReg["EmpName"] = strHoldEmpName;
                    drPayReg["ReasonForException"] = strHoldEXCRSN;

                    dtPrismTDI.Rows.Add(drPayReg);
                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013487\PrImport013487.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013487\PrImport013487.csv");

        ////save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013487\Exception013487.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013487\Exception013487.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C013397()
    {
        //open sql connection    NEW
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013397\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013397\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 4);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    //make sure employee name and employee id are not empty and hours > 0
                    if (
                        !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString())
                        && !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString())
                        && (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        )
                    {
                        //load SSN and LOC
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013397' AND com.EE_ID='" + CsvData.ItemArray[1].ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = CsvData.ItemArray[1].ToString();
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = CsvData.ItemArray[0].ToString().Trim();
                                drEXC["ReasonForException"] = "EMPLOYEE PAYROLL ID (ee_id) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drReg["AmountPos"] = "0";

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                    }

                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013397\PrImport013397.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013397\PrImport013397.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013397\Exception013397.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013397\Exception013397.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();
    }

    static void C013350()
    {
        //open sql connection    NEW
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013350\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013350\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 4);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    //make sure employee name and employee id are not empty and hours > 0
                    if (
                        !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString())
                        && !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString())
                        && (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        )
                    {
                        //load SSN and LOC
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013350' AND com.EE_ID='" + CsvData.ItemArray[1].ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = CsvData.ItemArray[1].ToString();
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = CsvData.ItemArray[0].ToString().Trim();
                                drEXC["ReasonForException"] = "EMPLOYEE PAYROLL ID (ee_id) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drReg["AmountPos"] = "0";

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                    }

                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013350\PrImport013350.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013350\PrImport013350.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013350\Exception013350.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013350\Exception013350.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();
    }

    static void C013345()
    {
        
        //open sql connection  NEW
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013345\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013345\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 4);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    //make sure employee name and employee id are not empty and hours > 0
                    if (
                        !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString())
                        && !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString())
                        && (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        )
                    {
                        //load SSN and LOC
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013345' AND com.EE_ID='" + CsvData.ItemArray[1].ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = CsvData.ItemArray[1].ToString();
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = CsvData.ItemArray[0].ToString().Trim();
                                drEXC["ReasonForException"] = "EMPLOYEE PAYROLL ID (ee_id) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drReg["AmountPos"] = "0";

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                    }

                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013345\PrImport013345.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013345\PrImport013345.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013345\Exception013345.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013345\Exception013345.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C013780()
    {      
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013780\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013780\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    //load SSN and LOC
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013780' AND com.EE_No='" + CsvData.ItemArray[4].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = CsvData.ItemArray[4].ToString();
                            drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim();
                            drEXC["ReasonForException"] = "EmployeeExtRef (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0");
                        drReg["AmountPos"] = "0";
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drOT["AmountPos"] = "0";
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }
                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013780\PrImport013780.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013780\PrImport013780.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count>0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013780\Exception013780.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013780\Exception013780.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C013925OLD()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013925\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013925\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    if (
                        string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString().Trim())
                        && string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString().Trim())
                        && string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString().Trim())
                        )
                    {
                        //row is blank, go to next line
                        continue;
                    }
                    //load SSN and LOC
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013925' AND com.EE_ID='" + CsvData.ItemArray[2].ToString().Trim() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = CsvData.ItemArray[2].ToString().Trim();
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim() + " " + CsvData.ItemArray[4].ToString().Trim();
                            drEXC["ReasonForException"] = "File # (ee_id) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                    {
                        DataRow drNonPayHrs3 = dtPrismTDI.NewRow();
                        drNonPayHrs3["Ssn"] = drEXC["SSN"];
                        drNonPayHrs3["CodePos"] = "NONPAYHRS3";
                        drNonPayHrs3["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drNonPayHrs3["AmountPos"] = "0";
                        drNonPayHrs3["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drNonPayHrs3["ClientID"] = drEXC["ClientID"];
                        drNonPayHrs3["EE_ID"] = drEXC["EE_ID"];
                        drNonPayHrs3["EE_NO"] = drEXC["EE_NO"];
                        drNonPayHrs3["EmpName"] = drEXC["EmpName"];
                        drNonPayHrs3["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drNonPayHrs3);
                    }


                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString().Replace("$".ToString(), string.Empty)) ? CsvData.ItemArray[19].ToString().Replace("$".ToString(), string.Empty) : "0") > 0)
                    {
                        DataRow drMiscPay = dtPrismTDI.NewRow();
                        drMiscPay["Ssn"] = drEXC["SSN"];
                        drMiscPay["CodePos"] = "MISCPAY";
                        drMiscPay["HoursPos"] = string.Empty;
                        drMiscPay["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString().Replace("$".ToString(), string.Empty)) ? CsvData.ItemArray[19].ToString().Replace("$".ToString(), string.Empty) : "0");
                        drMiscPay["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drMiscPay["ClientID"] = drEXC["ClientID"];
                        drMiscPay["EE_ID"] = drEXC["EE_ID"];
                        drMiscPay["EE_NO"] = drEXC["EE_NO"];
                        drMiscPay["EmpName"] = drEXC["EmpName"];
                        drMiscPay["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drMiscPay);
                    }

                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013925\PrImport013925.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013925\PrImport013925.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013925\Exception013925.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013925\Exception013925.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();

    }


    static void C013925()
    {
        Dictionary<string, string> dictOtherHours = new Dictionary<string, string>
            {
                {"H", "HOLIDAY"},
                {"F", "BEREAVEM"},
                {"J", "JURY"},
                {"P", "PTO"},
                {"E", "NONPAYHRS3"},
                {"10", "MILEAGE"},
                {"15", "NONPAYHRS"},
                {"20", "BACKPAY"},
                {"25", "BACKPAYOT"},
                {"128", "HOURLY2"},
                {"127", "HOURLY1"}

// do not add - payroll will add manually 
//                {"19", "COVID"}
            };

        Dictionary<string, string> dictOtherEarnings = new Dictionary<string, string>
            {
                {"E", "MISCPAY"},
                {"10", "EXPREIM4"},
                {"15", "MISCPAY3"},
                {"20", "RETRONH"},
                {"25", "RETRONH"},
                {"30", "RETRONH"},
                {"59", "ONCALL"},
                {"219", "SEVERANCE"}
            };


        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013925\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013925\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    //load SSN and LOC
                    //                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                    //                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                    //                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                    //                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                    //                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                    //                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                    //                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                    //                             " ON com.[EE_ID]=per.[EE_ID] " +
                    //                             " WHERE com.Client_ID= '013925' AND com.EE_ID='" + CsvData.ItemArray[2].ToString().Trim() + "'";

                    //load SSN and LOC
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013925' AND com.EE_ID='" + CsvData.ItemArray[2].ToString().Trim() + "'";
                    //" WHERE com.Client_ID= '013623' AND com.EE_No='" + CsvData.ItemArray[2].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = CsvData.ItemArray[2].ToString().Trim();
                            drEXC["EE_NO"] = "";
                            //                                drEXC["EE_ID"] = "";
                            //                                drEXC["EE_NO"] = CsvData.ItemArray[2].ToString();
                            drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim() + " " + CsvData.ItemArray[4].ToString().Trim();
                            drEXC["ReasonForException"] = "File # (ee_id) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        //                            drReg["Loc"] = "1";
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drOT["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        //                            drOT["Loc"] = "1";
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs1 = dtPrismTDI.NewRow();
                        drOthHrs1["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs1["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[10].ToString().Trim()) && (CsvData.ItemArray[10].ToString().Trim().Length > 0))
                        {
                            drOthHrs1["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "BADHoursCode"];
                            drOthHrs1["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs1["CodePos"] = "BADHoursCode";
                            drOthHrs1["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs1["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        drOthHrs1["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        //                            drOthHrs1["Loc"] = "1";
                        drOthHrs1["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drOthHrs1["ClientID"] = drEXC["ClientID"];
                        drOthHrs1["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs1["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs1["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs1);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs2 = dtPrismTDI.NewRow();
                        drOthHrs2["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs2["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[12].ToString().Trim()) && (CsvData.ItemArray[12].ToString().Trim().Length > 0))
                        {
                            drOthHrs2["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "BADHoursCode"];
                            drOthHrs2["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs2["CodePos"] = "BADHoursCode";
                            drOthHrs2["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs2["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drOthHrs2["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        //                            drOthHrs2["Loc"] = "1";
                        drOthHrs2["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drOthHrs2["ClientID"] = drEXC["ClientID"];
                        drOthHrs2["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs2["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs2["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs2);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                        && (!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty) != "19")
                    {
                        DataRow drOthHrs3 = dtPrismTDI.NewRow();
                        drOthHrs3["Ssn"] = drEXC["SSN"];
                        //                            drOthHrs3["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : string.Empty];
                        //testing for valid hours code - forcing exception code if invalid
                        if (dictOtherHours.ContainsKey(CsvData.ItemArray[14].ToString().Trim()) && (CsvData.ItemArray[14].ToString().Trim().Length > 0))
                        {
                            drOthHrs3["CodePos"] = dictOtherHours[!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "BADHoursCode"];
                            drOthHrs3["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthHrs3["CodePos"] = "BADHoursCode";
                            drOthHrs3["ReasonForException"] = "BADHoursCode";
                        }
                        drOthHrs3["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                        drOthHrs3["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                        //                            drOthHrs3["Loc"] = "1";
                        drOthHrs3["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drOthHrs3["ClientID"] = drEXC["ClientID"];
                        drOthHrs3["EE_ID"] = drEXC["EE_ID"];
                        drOthHrs3["EE_NO"] = drEXC["EE_NO"];
                        drOthHrs3["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthHrs3);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn1 = dtPrismTDI.NewRow();
                        drOthEarn1["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn1["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[16].ToString().Trim()) && (CsvData.ItemArray[16].ToString().Trim().Length > 0))
                        {
                            drOthEarn1["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "BADEarningsCode"];
                            drOthEarn1["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn1["CodePos"] = "BADEarningsCode";
                            drOthEarn1["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn1["HoursPos"] = "0";
                        drOthEarn1["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0");
                        //                            drOthEarn1["Loc"] = "1";
                        drOthEarn1["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drOthEarn1["ClientID"] = drEXC["ClientID"];
                        drOthEarn1["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn1["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn1["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn1);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn2 = dtPrismTDI.NewRow();
                        drOthEarn2["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn2["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[18].ToString().Trim()) && (CsvData.ItemArray[18].ToString().Trim().Length > 0))
                        {
                            drOthEarn2["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : "BADEarningsCode"];
                            drOthEarn2["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn2["CodePos"] = "BADEarningsCode";
                            drOthEarn2["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn2["HoursPos"] = "0";
                        drOthEarn2["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : "0");
                        //                            drOthEarn2["Loc"] = "1";
                        drOthEarn2["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drOthEarn2["ClientID"] = drEXC["ClientID"];
                        drOthEarn2["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn2["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn2["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn2);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0") > 0)
                    {
                        DataRow drOthEarn3 = dtPrismTDI.NewRow();
                        drOthEarn3["Ssn"] = drEXC["SSN"];
                        //                            drOthEarn3["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[20].ToString()) ? CsvData.ItemArray[20].ToString() : string.Empty];
                        //testing for valid earnings code - forcing exception code if invalid
                        if (dictOtherEarnings.ContainsKey(CsvData.ItemArray[20].ToString().Trim()) && (CsvData.ItemArray[20].ToString().Trim().Length > 0))
                        {
                            drOthEarn3["CodePos"] = dictOtherEarnings[!string.IsNullOrWhiteSpace(CsvData.ItemArray[20].ToString()) ? CsvData.ItemArray[20].ToString() : "BADEarningsCode"];
                            drOthEarn3["ReasonForException"] = drEXC["ReasonForException"];
                        }
                        else
                        {
                            drOthEarn3["CodePos"] = "BADEarningsCode";
                            drOthEarn3["ReasonForException"] = "BADEarningsCode";
                        }
                        drOthEarn3["HoursPos"] = "0";
                        drOthEarn3["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[21].ToString()) ? CsvData.ItemArray[21].ToString() : "0");
                        //                            drOthEarn3["Loc"] = "1";
                        drOthEarn3["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : string.Empty;

                        drOthEarn3["ClientID"] = drEXC["ClientID"];
                        drOthEarn3["EE_ID"] = drEXC["EE_ID"];
                        drOthEarn3["EE_NO"] = drEXC["EE_NO"];
                        drOthEarn3["EmpName"] = drEXC["EmpName"];

                        dtPrismTDI.Rows.Add(drOthEarn3);
                    }


                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013925\PrImport013925.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013925\PrImport013925.csv");
        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013925\Exception013925.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013925\Exception013925.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C218260()
    {
        //open sql connection  new
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218260\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\218260\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataTable newTDITable = ReadCsv(empFile, true, 0);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '218260' AND ssn.EE_SSN='" + CsvData.ItemArray[2].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = CsvData.ItemArray[2].ToString();
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[7].ToString().Trim();
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        drReg["AmountPos"] = double.Parse("0");
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[1].ToString().PadLeft(6, '0') : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0");
                        drOT["AmountPos"] = double.Parse("0");
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[1].ToString().PadLeft(6, '0') : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0") > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPSRETINC";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0");
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[1].ToString().PadLeft(6, '0') : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0") > 0)
                    {
                        DataRow drBonus = dtPrismTDI.NewRow();
                        drBonus["Ssn"] = drEXC["SSN"];
                        drBonus["CodePos"] = "BONUSN-ST";
                        drBonus["HoursPos"] = string.Empty;
                        drBonus["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0");
                        drBonus["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[1].ToString().PadLeft(6, '0') : string.Empty;
                        drBonus["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : string.Empty;

                        drBonus["ClientID"] = drEXC["ClientID"];
                        drBonus["EE_ID"] = drEXC["EE_ID"];
                        drBonus["EE_NO"] = drEXC["EE_NO"];
                        drBonus["EmpName"] = drEXC["EmpName"];
                        drBonus["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drBonus);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0") > 0)
                    {
                        DataRow drMiles = dtPrismTDI.NewRow();
                        drMiles["Ssn"] = drEXC["SSN"];
                        drMiles["CodePos"] = "MILEEXCESS";
                        drMiles["HoursPos"] = string.Empty;
                        drMiles["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0");
                        drMiles["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString().PadLeft(6, '0')) ? CsvData.ItemArray[1].ToString().PadLeft(6, '0') : string.Empty;
                        drMiles["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[19].ToString()) ? CsvData.ItemArray[19].ToString() : string.Empty;

                        drMiles["ClientID"] = drEXC["ClientID"];
                        drMiles["EE_ID"] = drEXC["EE_ID"];
                        drMiles["EE_NO"] = drEXC["EE_NO"];
                        drMiles["EmpName"] = drEXC["EmpName"];
                        drMiles["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drMiles);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218260\PrImport218260.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\218260\PrImport218260.csv");

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C013164old()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013164\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013164\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 4 headers
                for (int i = 3; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    dr.Delete();
                }
                newTDITable.AcceptChanges();

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    if (!CsvData.ItemArray[3].ToString().Contains("Totals"))
                    {

                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013164' AND ssn.EE_SSN='" + CsvData.ItemArray[2].ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = CsvData.ItemArray[2].ToString();
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim().Replace(",", "");
                                drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0") > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0");
                            drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drReg["Loc"] = "1";
                            drReg["Dept"] = string.Empty;

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                        {
                            DataRow drOT = dtPrismTDI.NewRow();
                            drOT["Ssn"] = drEXC["SSN"];
                            drOT["CodePos"] = "OT";
                            drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                            drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * 1.5);
                            drOT["Loc"] = "1";
                            drOT["Dept"] = "OT";

                            drOT["ClientID"] = drEXC["ClientID"];
                            drOT["EE_ID"] = drEXC["EE_ID"];
                            drOT["EE_NO"] = drEXC["EE_NO"];
                            drOT["EmpName"] = drEXC["EmpName"];
                            drOT["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drOT);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                        {
                            DataRow drAddHrs = dtPrismTDI.NewRow();
                            drAddHrs["Ssn"] = drEXC["SSN"];
                            drAddHrs["CodePos"] = "HOURLY1";
                            drAddHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                            drAddHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drAddHrs["Loc"] = "1";
                            drAddHrs["Dept"] = "ADD";

                            drAddHrs["ClientID"] = drEXC["ClientID"];
                            drAddHrs["EE_ID"] = drEXC["EE_ID"];
                            drAddHrs["EE_NO"] = drEXC["EE_NO"];
                            drAddHrs["EmpName"] = drEXC["EmpName"];
                            drAddHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drAddHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") > 0)
                        {
                            DataRow drInclHrs = dtPrismTDI.NewRow();
                            drInclHrs["Ssn"] = drEXC["SSN"];
                            drInclHrs["CodePos"] = "HOURLY3";
                            drInclHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                            drInclHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drInclHrs["Loc"] = "1";
                            drInclHrs["Dept"] = "ICLU";

                            drInclHrs["ClientID"] = drEXC["ClientID"];
                            drInclHrs["EE_ID"] = drEXC["EE_ID"];
                            drInclHrs["EE_NO"] = drEXC["EE_NO"];
                            drInclHrs["EmpName"] = drEXC["EmpName"];
                            drInclHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drInclHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0") > 0)
                        {
                            DataRow drBonusHrs = dtPrismTDI.NewRow();
                            drBonusHrs["Ssn"] = drEXC["SSN"];
                            drBonusHrs["CodePos"] = "HOURLY2";
                            drBonusHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                            drBonusHrs["AmountPos"] = ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0")
                                                        + double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0"))
                                                        / (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0")));
                            drBonusHrs["Loc"] = "1";
                            drBonusHrs["Dept"] = "BONU";

                            drBonusHrs["ClientID"] = drEXC["ClientID"];
                            drBonusHrs["EE_ID"] = drEXC["EE_ID"];
                            drBonusHrs["EE_NO"] = drEXC["EE_NO"];
                            drBonusHrs["EmpName"] = drEXC["EmpName"];
                            drBonusHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drBonusHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0") > 0)
                        {
                            DataRow drPtoHrs = dtPrismTDI.NewRow();
                            drPtoHrs["Ssn"] = drEXC["SSN"];
                            drPtoHrs["CodePos"] = "PTO";
                            drPtoHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0");
                            drPtoHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drPtoHrs["Loc"] = "1";
                            drPtoHrs["Dept"] = "PTO";

                            drPtoHrs["ClientID"] = drEXC["ClientID"];
                            drPtoHrs["EE_ID"] = drEXC["EE_ID"];
                            drPtoHrs["EE_NO"] = drEXC["EE_NO"];
                            drPtoHrs["EmpName"] = drEXC["EmpName"];
                            drPtoHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drPtoHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : "0") > 0)
                        {
                            DataRow drHolHrs = dtPrismTDI.NewRow();
                            drHolHrs["Ssn"] = drEXC["SSN"];
                            drHolHrs["CodePos"] = "HOLIDAY";
                            drHolHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[18].ToString()) ? CsvData.ItemArray[18].ToString() : "0");
                            drHolHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drHolHrs["Loc"] = "1";
                            drHolHrs["Dept"] = "HOL";

                            drHolHrs["ClientID"] = drEXC["ClientID"];
                            drHolHrs["EE_ID"] = drEXC["EE_ID"];
                            drHolHrs["EE_NO"] = drEXC["EE_NO"];
                            drHolHrs["EmpName"] = drEXC["EmpName"];
                            drHolHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drHolHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                        {
                            DataRow drLottoAmt = dtPrismTDI.NewRow();
                            drLottoAmt["Ssn"] = drEXC["SSN"];
                            drLottoAmt["CodePos"] = "COMM2-R";
                            drLottoAmt["HoursPos"] = string.Empty;
                            drLottoAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                            drLottoAmt["Loc"] = "1";
                            drLottoAmt["Dept"] = "BONU";

                            drLottoAmt["ClientID"] = drEXC["ClientID"];
                            drLottoAmt["EE_ID"] = drEXC["EE_ID"];
                            drLottoAmt["EE_NO"] = drEXC["EE_NO"];
                            drLottoAmt["EmpName"] = drEXC["EmpName"];
                            drLottoAmt["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drLottoAmt);
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013164\PrImport013164.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013164\PrImport013164.csv");

        //Close SQL connection
        SQLConnectionClose();
    }

    static void C013164()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013164\", "*.xlsx");
        //            string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013164\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 4 headers - new layout only has 3 headers so changed loop count below from 3 to 2
                //                    for (int i = 3; i >= 0; i--)
                for (int i = 2; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    dr.Delete();
                }
                newTDITable.AcceptChanges();

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    if (!CsvData.ItemArray[3].ToString().Contains("Totals"))
                    {

                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013164' AND ssn.EE_SSN='" + CsvData.ItemArray[2].ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = CsvData.ItemArray[2].ToString();
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = CsvData.ItemArray[3].ToString().Trim().Replace(",", "");
                                drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            drReg["Loc"] = "1";
                            drReg["Dept"] = string.Empty;

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0") > 0)
                        {
                            DataRow drOT = dtPrismTDI.NewRow();
                            drOT["Ssn"] = drEXC["SSN"];
                            drOT["CodePos"] = "OT";
                            drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                            drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * 1.5);
                            drOT["Loc"] = "1";
                            drOT["Dept"] = "OT";

                            drOT["ClientID"] = drEXC["ClientID"];
                            drOT["EE_ID"] = drEXC["EE_ID"];
                            drOT["EE_NO"] = drEXC["EE_NO"];
                            drOT["EmpName"] = drEXC["EmpName"];
                            drOT["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drOT);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0") > 0)
                        {
                            DataRow drAddHrs = dtPrismTDI.NewRow();
                            drAddHrs["Ssn"] = drEXC["SSN"];
                            drAddHrs["CodePos"] = "HOURLY1";
                            drAddHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0");
                            drAddHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            drAddHrs["Loc"] = "1";
                            drAddHrs["Dept"] = "ADD";

                            drAddHrs["ClientID"] = drEXC["ClientID"];
                            drAddHrs["EE_ID"] = drEXC["EE_ID"];
                            drAddHrs["EE_NO"] = drEXC["EE_NO"];
                            drAddHrs["EmpName"] = drEXC["EmpName"];
                            drAddHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drAddHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                        {
                            DataRow drInclHrs = dtPrismTDI.NewRow();
                            drInclHrs["Ssn"] = drEXC["SSN"];
                            drInclHrs["CodePos"] = "HOURLY3";
                            drInclHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                            drInclHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            drInclHrs["Loc"] = "1";
                            drInclHrs["Dept"] = "ICLU";

                            drInclHrs["ClientID"] = drEXC["ClientID"];
                            drInclHrs["EE_ID"] = drEXC["EE_ID"];
                            drInclHrs["EE_NO"] = drEXC["EE_NO"];
                            drInclHrs["EmpName"] = drEXC["EmpName"];
                            drInclHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drInclHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") > 0)
                        {
                            DataRow drBonusHrs = dtPrismTDI.NewRow();
                            drBonusHrs["Ssn"] = drEXC["SSN"];
                            drBonusHrs["CodePos"] = "HOURLY2";
                            drBonusHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                            drBonusHrs["AmountPos"] = (((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0")
                                                        * double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0"))
                                                        + double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0"))
                                                        / (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0")));
                            drBonusHrs["Loc"] = "1";
                            drBonusHrs["Dept"] = "BONU";

                            drBonusHrs["ClientID"] = drEXC["ClientID"];
                            drBonusHrs["EE_ID"] = drEXC["EE_ID"];
                            drBonusHrs["EE_NO"] = drEXC["EE_NO"];
                            drBonusHrs["EmpName"] = drEXC["EmpName"];
                            drBonusHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drBonusHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0") > 0)
                        {
                            DataRow drPtoHrs = dtPrismTDI.NewRow();
                            drPtoHrs["Ssn"] = drEXC["SSN"];
                            drPtoHrs["CodePos"] = "PTO";
                            drPtoHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0");
                            drPtoHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            drPtoHrs["Loc"] = "1";
                            drPtoHrs["Dept"] = "PTO";

                            drPtoHrs["ClientID"] = drEXC["ClientID"];
                            drPtoHrs["EE_ID"] = drEXC["EE_ID"];
                            drPtoHrs["EE_NO"] = drEXC["EE_NO"];
                            drPtoHrs["EmpName"] = drEXC["EmpName"];
                            drPtoHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drPtoHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                        {
                            DataRow drHolHrs = dtPrismTDI.NewRow();
                            drHolHrs["Ssn"] = drEXC["SSN"];
                            drHolHrs["CodePos"] = "HOLIDAY";
                            drHolHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                            drHolHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            drHolHrs["Loc"] = "1";
                            drHolHrs["Dept"] = "HOL";

                            drHolHrs["ClientID"] = drEXC["ClientID"];
                            drHolHrs["EE_ID"] = drEXC["EE_ID"];
                            drHolHrs["EE_NO"] = drEXC["EE_NO"];
                            drHolHrs["EmpName"] = drEXC["EmpName"];
                            drHolHrs["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drHolHrs);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                        {
                            DataRow drLottoAmt = dtPrismTDI.NewRow();
                            drLottoAmt["Ssn"] = drEXC["SSN"];
                            drLottoAmt["CodePos"] = "COMM2-R";
                            drLottoAmt["HoursPos"] = string.Empty;
                            drLottoAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                            drLottoAmt["Loc"] = "1";
                            drLottoAmt["Dept"] = "BONU";

                            drLottoAmt["ClientID"] = drEXC["ClientID"];
                            drLottoAmt["EE_ID"] = drEXC["EE_ID"];
                            drLottoAmt["EE_NO"] = drEXC["EE_NO"];
                            drLottoAmt["EmpName"] = drEXC["EmpName"];
                            drLottoAmt["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drLottoAmt);
                        }

                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013164\PrImport013164.csv");
        //            SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013164\PrImport013164.csv");
        //Close SQL connection
        SQLConnectionClose();
    }

    static void C013743()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013743\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013743\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                newTDITable.Rows[0].Delete();
                newTDITable.AcceptChanges();

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    if (
                        !string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString())
                        && !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString())
                        && !string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString())
                        //&& (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 0)
                       
                        )
                    {

                        //load SSN and LOC
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013743' AND com.EE_No='" + CsvData.ItemArray[1].ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = CsvData.ItemArray[1].ToString();
                                drEXC["EmpName"] = CsvData.ItemArray[0].ToString().Trim();
                                drEXC["ReasonForException"] = "Clock Number (ee_no) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            drReg["AmountPos"] = double.Parse("0");
                            drReg["Loc"] = "1";

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        {
                            DataRow drOT = dtPrismTDI.NewRow();
                            drOT["Ssn"] = drEXC["SSN"];
                            drOT["CodePos"] = "OT";
                            drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            drOT["AmountPos"] = double.Parse("0");
                            drOT["Loc"] = "1";

                            drOT["ClientID"] = drEXC["ClientID"];
                            drOT["EE_ID"] = drEXC["EE_ID"];
                            drOT["EE_NO"] = drEXC["EE_NO"];
                            drOT["EmpName"] = drEXC["EmpName"];
                            drOT["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drOT);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0") > 0)
                        {
                            DataRow drVac = dtPrismTDI.NewRow();
                            drVac["Ssn"] = drEXC["SSN"];
                            drVac["CodePos"] = "VACATION";
                            drVac["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0");
                            drVac["AmountPos"] = double.Parse("0");
                            drVac["Loc"] = "1";

                            drVac["ClientID"] = drEXC["ClientID"];
                            drVac["EE_ID"] = drEXC["EE_ID"];
                            drVac["EE_NO"] = drEXC["EE_NO"];
                            drVac["EmpName"] = drEXC["EmpName"];
                            drVac["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drVac);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                        {
                            DataRow drHol = dtPrismTDI.NewRow();
                            drHol["Ssn"] = drEXC["SSN"];
                            drHol["CodePos"] = "HOLIDAY";
                            drHol["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                            drHol["AmountPos"] = double.Parse("0");
                            drHol["Loc"] = "1";

                            drHol["ClientID"] = drEXC["ClientID"];
                            drHol["EE_ID"] = drEXC["EE_ID"];
                            drHol["EE_NO"] = drEXC["EE_NO"];
                            drHol["EmpName"] = drEXC["EmpName"];
                            drHol["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drHol);
                        }
                    }
                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013743\PrImport013743.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013743\PrImport013743.csv");

        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013743\Exception013743.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013743\Exception013743.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();




    } //new one 

    static void C218197slow()
    {

        Dictionary<string, string> dictPayCodes = new Dictionary<string, string>
            {
                {"1", PayType.Hourly},
                {"3", PayType.Hourly},
                {"23", PayType.Hourly},
                {"2", PayType.Overtime},
                {"4", PayType.Overtime},
                {"24", PayType.Overtime},
                {"47", PayType.Vacation},
                {"48", PayType.Vacation},
                {"57", PayType.Hourly1},
                {"59", PayType.Hourly1},
                {"9", PayType.Overtime1},
                {"58", PayType.Overtime1},
                {"05", PayType.DailyRate},
                {"60", PayType.DailyRate},
                {"74", PayType.DailyRate},
                {"99", PayType.DailyRate},
                {"39", PayType.PaidTimeOff},
                {"63", PayType.PaidTimeOff},
                {"64", PayType.PaidTimeOff},
                {"65", PayType.PaidTimeOff},
                {"14", PayType.BackPay},
                {"22", PayType.Bonus},
                {"56", PayType.Bonus},
                {"61", PayType.SickHrly}
            };

        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218197\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\218197\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //no headers to remove

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '218197' AND ssn.EE_SSN='" + Convert.ToInt32(CsvData.ItemArray[0].ToString()).ToString("###-##-####").PadLeft(11, '0') + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = Convert.ToInt32(CsvData.ItemArray[0].ToString()).ToString("###-##-####").PadLeft(11, '0');
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[1].ToString().Trim().Replace(",", "");
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") > 0)
                    {
                        DataRow drPayRec = dtPrismTDI.NewRow();
                        drPayRec["Ssn"] = drEXC["SSN"];
                        drPayRec["CodePos"] = dictPayCodes[!string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? CsvData.ItemArray[2].ToString() : string.Empty];
                        drPayRec["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                        drPayRec["AmountPos"] = double.Parse("0");
                        drPayRec["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString().PadLeft(5, '0')) ? CsvData.ItemArray[9].ToString().PadLeft(5, '0') : string.Empty;

                        drPayRec["ClientID"] = drEXC["ClientID"];
                        drPayRec["EE_ID"] = drEXC["EE_ID"];
                        drPayRec["EE_NO"] = drEXC["EE_NO"];
                        drPayRec["EmpName"] = drEXC["EmpName"];
                        drPayRec["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drPayRec);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218197\PrImport218197.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\218197\PrImport218197.csv");

        //Close SQL connection
        SQLConnectionClose();



    }

    static void C218197()
    {

        Dictionary<string, string> dictPayCodes = new Dictionary<string, string>
            {
                {"1", PayType.Hourly},
                {"3", PayType.Hourly},
                {"23", PayType.Hourly},
                {"2", PayType.Overtime},
                {"4", PayType.Overtime},
                {"24", PayType.Overtime},
                {"47", PayType.Vacation},
                {"48", PayType.Vacation},
                {"57", PayType.Hourly1},
                {"59", PayType.Hourly1},
                {"9", PayType.Overtime1},
                {"58", PayType.Overtime1},
                {"05", PayType.DailyRate},
                {"60", PayType.DailyRate},
                {"74", PayType.DailyRate},
                {"99", PayType.DailyRate},
                {"39", PayType.PaidTimeOff},
                {"63", PayType.PaidTimeOff},
                {"64", PayType.PaidTimeOff},
                {"65", PayType.PaidTimeOff},
                {"14", PayType.BackPay},
                {"22", PayType.Bonus},
                {"56", PayType.Bonus},
                {"61", PayType.SickHrly}
            };

        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218197\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\218197\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //no headers to remove

                string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                     "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                     "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                     " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                     " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                     " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                     " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                     " ON com.[EE_ID]=per.[EE_ID] " +
                     " WHERE com.Client_ID= '218197' ";
                DataTable dtLookUpList = SQLGetTableData(strSQLQuery);

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    DataRow[] drLookUp = dtLookUpList.Select("EE_SSN='" + Convert.ToInt32(CsvData.ItemArray[0].ToString()).ToString("###-##-####").PadLeft(11, '0') + "'");

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((drLookUp.Length == 0) || (drLookUp.Length == 1 && drLookUp[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (drLookUp.Length == 0)
                        {
                            drEXC["SSN"] = Convert.ToInt32(CsvData.ItemArray[0].ToString()).ToString("###-##-####").PadLeft(11, '0');
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[1].ToString().Trim().Replace(",", "");
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (drLookUp.Length == 1 && drLookUp[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = drLookUp[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = drLookUp[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = drLookUp[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = drLookUp[0].ItemArray[7].ToString().Trim() + " " + drLookUp[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = drLookUp[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = drLookUp[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = drLookUp[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = drLookUp[0].ItemArray[7].ToString().Trim() + " " + drLookUp[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") > 0)
                    {
                        DataRow drPayRec = dtPrismTDI.NewRow();
                        drPayRec["Ssn"] = drEXC["SSN"];
                        //testing for valid paytype - forcing exception code if invalid
                        if (dictPayCodes.ContainsKey(CsvData.ItemArray[2].ToString().Trim()))
                        {
                            drPayRec["CodePos"] = dictPayCodes[!string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? CsvData.ItemArray[2].ToString() : "BADPayType"];
                        }
                        else
                        {
                            drPayRec["CodePos"] = "BADPayType";
                        }


                       // drPayRec["CodePos"] = dictPayCodes[!string.IsNullOrWhiteSpace(CsvData.ItemArray[2].ToString()) ? CsvData.ItemArray[2].ToString() : string.Empty];
                        drPayRec["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                        drPayRec["AmountPos"] = double.Parse("0");
                        drPayRec["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString().PadLeft(5, '0')) ? CsvData.ItemArray[9].ToString().PadLeft(5, '0') : string.Empty;

                        drPayRec["ClientID"] = drEXC["ClientID"];
                        drPayRec["EE_ID"] = drEXC["EE_ID"];
                        drPayRec["EE_NO"] = drEXC["EE_NO"];
                        drPayRec["EmpName"] = drEXC["EmpName"];
                        drPayRec["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drPayRec);
                    }
                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\218197\PrImport218197.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\218197\PrImport218197.csv");

        //Close SQL connection
        SQLConnectionClose();
    }





    static void C012805()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012805\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\012805\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 1 header
                newTDITable.Rows[0].Delete();
                newTDITable.AcceptChanges();

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '012805' AND ssn.EE_SSN='" + CsvData.ItemArray[1].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = CsvData.ItemArray[1].ToString();
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[2].ToString().Trim() + " " + CsvData.ItemArray[3].ToString().Trim();
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drReg["Loc"] = "1";

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[10].ToString()) ? CsvData.ItemArray[10].ToString() : "0");
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0") * 1.5);
                        drOT["Loc"] = "1";

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0") > 0)
                    {
                        DataRow drVacHrs = dtPrismTDI.NewRow();
                        drVacHrs["Ssn"] = drEXC["SSN"];
                        drVacHrs["CodePos"] = "VACATION";
                        drVacHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[15].ToString()) ? CsvData.ItemArray[15].ToString() : "0");
                        drVacHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drVacHrs["Loc"] = "1";

                        drVacHrs["ClientID"] = drEXC["ClientID"];
                        drVacHrs["EE_ID"] = drEXC["EE_ID"];
                        drVacHrs["EE_NO"] = drEXC["EE_NO"];
                        drVacHrs["EmpName"] = drEXC["EmpName"];
                        drVacHrs["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drVacHrs);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0") > 0)
                    {
                        DataRow drHolHrs = dtPrismTDI.NewRow();
                        drHolHrs["Ssn"] = drEXC["SSN"];
                        drHolHrs["CodePos"] = "HOLIDAY";
                        drHolHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : "0");
                        drHolHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drHolHrs["Loc"] = "1";

                        drHolHrs["ClientID"] = drEXC["ClientID"];
                        drHolHrs["EE_ID"] = drEXC["EE_ID"];
                        drHolHrs["EE_NO"] = drEXC["EE_NO"];
                        drHolHrs["EmpName"] = drEXC["EmpName"];
                        drHolHrs["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drHolHrs);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0") > 0)
                    {
                        DataRow drSickHrs = dtPrismTDI.NewRow();
                        drSickHrs["Ssn"] = drEXC["SSN"];
                        drSickHrs["CodePos"] = "SICK-HRLY";
                        drSickHrs["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[17].ToString()) ? CsvData.ItemArray[17].ToString() : "0");
                        drSickHrs["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drSickHrs["Loc"] = "1";

                        drSickHrs["ClientID"] = drEXC["ClientID"];
                        drSickHrs["EE_ID"] = drEXC["EE_ID"];
                        drSickHrs["EE_NO"] = drEXC["EE_NO"];
                        drSickHrs["EmpName"] = drEXC["EmpName"];
                        drSickHrs["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drSickHrs);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                    {
                        DataRow drCommAmt = dtPrismTDI.NewRow();
                        drCommAmt["Ssn"] = drEXC["SSN"];
                        drCommAmt["CodePos"] = "COMM-R";
                        drCommAmt["HoursPos"] = string.Empty;
                        drCommAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        drCommAmt["Loc"] = "1";

                        drCommAmt["ClientID"] = drEXC["ClientID"];
                        drCommAmt["EE_ID"] = drEXC["EE_ID"];
                        drCommAmt["EE_NO"] = drEXC["EE_NO"];
                        drCommAmt["EmpName"] = drEXC["EmpName"];
                        drCommAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drCommAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0") > 0)
                    {
                        DataRow drExpAmt = dtPrismTDI.NewRow();
                        drExpAmt["Ssn"] = drEXC["SSN"];
                        drExpAmt["CodePos"] = "EXPREIM";
                        drExpAmt["HoursPos"] = string.Empty;
                        drExpAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[12].ToString()) ? CsvData.ItemArray[12].ToString() : "0");
                        drExpAmt["Loc"] = "1";

                        drExpAmt["ClientID"] = drEXC["ClientID"];
                        drExpAmt["EE_ID"] = drEXC["EE_ID"];
                        drExpAmt["EE_NO"] = drEXC["EE_NO"];
                        drExpAmt["EmpName"] = drEXC["EmpName"];
                        drExpAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drExpAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0") > 0)
                    {
                        DataRow drProdBonusAmt = dtPrismTDI.NewRow();
                        drProdBonusAmt["Ssn"] = drEXC["SSN"];
                        drProdBonusAmt["CodePos"] = "BONUSN-RT";
                        drProdBonusAmt["HoursPos"] = string.Empty;
                        drProdBonusAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[13].ToString()) ? CsvData.ItemArray[13].ToString() : "0");
                        drProdBonusAmt["Loc"] = "1";

                        drProdBonusAmt["ClientID"] = drEXC["ClientID"];
                        drProdBonusAmt["EE_ID"] = drEXC["EE_ID"];
                        drProdBonusAmt["EE_NO"] = drEXC["EE_NO"];
                        drProdBonusAmt["EmpName"] = drEXC["EmpName"];
                        drProdBonusAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drProdBonusAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0") > 0)
                    {
                        DataRow drDiscrBonusAmt = dtPrismTDI.NewRow();
                        drDiscrBonusAmt["Ssn"] = drEXC["SSN"];
                        drDiscrBonusAmt["CodePos"] = "BONUSD-ST";
                        drDiscrBonusAmt["HoursPos"] = string.Empty;
                        drDiscrBonusAmt["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString() : "0");
                        drDiscrBonusAmt["Loc"] = "1";

                        drDiscrBonusAmt["ClientID"] = drEXC["ClientID"];
                        drDiscrBonusAmt["EE_ID"] = drEXC["EE_ID"];
                        drDiscrBonusAmt["EE_NO"] = drEXC["EE_NO"];
                        drDiscrBonusAmt["EmpName"] = drEXC["EmpName"];
                        drDiscrBonusAmt["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drDiscrBonusAmt);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drSal = dtPrismTDI.NewRow();
                        drSal["Ssn"] = drEXC["SSN"];
                        drSal["CodePos"] = "SALARY";
                        drSal["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drSal["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        drSal["Loc"] = "1";

                        drSal["ClientID"] = drEXC["ClientID"];
                        drSal["EE_ID"] = drEXC["EE_ID"];
                        drSal["EE_NO"] = drEXC["EE_NO"];
                        drSal["EmpName"] = drEXC["EmpName"];
                        drSal["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drSal);
                    }

                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012805\PrImport012805.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\012805\PrImport012805.csv");

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C012803()
    {

        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        Boolean firstRow = true;
        int intSkipExtraHdrs = 2;
        DataTable projTDITable = new DataTable();

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012803\", "*.xls");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\012803\", "*.xls");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 1 header
                newTDITable.Rows[0].Delete();
                newTDITable.AcceptChanges();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    if (!CsvData.ItemArray[0].ToString().ToUpper().Contains("TOTAL"))
                    {

                        if (firstRow)
                        {
                            //preparing table for projects
                            projTDITable = newTDITable.Clone();
                            DataRow drPROJ = projTDITable.NewRow();
                            drPROJ.ItemArray = CsvData.ItemArray;
                            projTDITable.Rows.Add(drPROJ);
                            firstRow = false;
                            continue;
                        }
                        // skips any extra headers based on variable value
                        if (intSkipExtraHdrs > 0)
                        {
                            intSkipExtraHdrs--;
                            continue;
                        }
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '012803' AND ssn.EE_SSN='" + CsvData.ItemArray[1].ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = CsvData.ItemArray[1].ToString();
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = CsvData.ItemArray[0].ToString().Trim().Replace(",", "");
                                drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        for (int i = 4; i <= (CsvData.ItemArray.Length - 1); i += 2)
                        {
                            //tests for a valid project - if none, go one to next column
                            if (!string.IsNullOrWhiteSpace(projTDITable.Rows[0].ItemArray[i].ToString()))
                            {
                                //test for hourly data under this project
                                if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[i].ToString()) ? CsvData.ItemArray[i].ToString() : "0") > 0)
                                {
                                    DataRow drPayReg = dtPrismTDI.NewRow();
                                    drPayReg["Ssn"] = drEXC["SSN"];
                                    drPayReg["CodePos"] = "HOURLY";
                                    drPayReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[i].ToString()) ? CsvData.ItemArray[i].ToString() : "0");
                                    drPayReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                                    drPayReg["Job"] = !string.IsNullOrWhiteSpace(projTDITable.Rows[0].ItemArray[i].ToString()) ? projTDITable.Rows[0].ItemArray[i].ToString() : string.Empty;

                                    drPayReg["ClientID"] = drEXC["ClientID"];
                                    drPayReg["EE_ID"] = drEXC["EE_ID"];
                                    drPayReg["EE_NO"] = drEXC["EE_NO"];
                                    drPayReg["EmpName"] = drEXC["EmpName"];
                                    drPayReg["ReasonForException"] = drEXC["ReasonForException"];

                                    dtPrismTDI.Rows.Add(drPayReg);
                                }

                                //test for overtime data under this project
                                if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[(i + 1)].ToString()) ? CsvData.ItemArray[(i + 1)].ToString() : "0") > 0)
                                {
                                    DataRow drPayOT = dtPrismTDI.NewRow();
                                    drPayOT["Ssn"] = drEXC["SSN"];
                                    drPayOT["CodePos"] = "OT";
                                    drPayOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[(i + 1)].ToString()) ? CsvData.ItemArray[(i + 1)].ToString() : "0");
                                    drPayOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * 1.5);
                                    drPayOT["Job"] = !string.IsNullOrWhiteSpace(projTDITable.Rows[0].ItemArray[i].ToString()) ? projTDITable.Rows[0].ItemArray[i].ToString() : string.Empty;

                                    drPayOT["ClientID"] = drEXC["ClientID"];
                                    drPayOT["EE_ID"] = drEXC["EE_ID"];
                                    drPayOT["EE_NO"] = drEXC["EE_NO"];
                                    drPayOT["EmpName"] = drEXC["EmpName"];
                                    drPayOT["ReasonForException"] = drEXC["ReasonForException"];

                                    dtPrismTDI.Rows.Add(drPayOT);
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\012803\PrImport012803.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\012803\PrImport012803.csv");

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C013843()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        Boolean blnFirstRow = true;
        DataTable projTDITable = new DataTable();

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013843\", "*.xls");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013843\", "*.xls");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 12 headers
                for (int i = 11; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    dr.Delete();
                }
                newTDITable.AcceptChanges();

                //add location column and populate it with filename (aka location)
                newTDITable.Columns.Add(new DataColumn("LocationCode", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("SSN", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("EEID", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("EENO", typeof(string)));
                newTDITable.Columns.Add(new DataColumn("ExceptionReason", typeof(string)));
                DataColumn colLOC = newTDITable.Columns["LocationCode"];
                DataColumn colSSN = newTDITable.Columns["SSN"];
                DataColumn colEEID = newTDITable.Columns["EEID"];
                DataColumn colEENO = newTDITable.Columns["EENO"];
                DataColumn colEXCRSN = newTDITable.Columns["ExceptionReason"];
                DataColumn colName = newTDITable.Columns[4];
                foreach (DataRow row in newTDITable.Rows)
                    row[colLOC] = Path.GetFileNameWithoutExtension(empFile);

                //loop to add each empname to all rows for that employee
                string strEmpName = string.Empty;
                string strLastName = string.Empty;
                string strFirstName = string.Empty;
                string strSSN = string.Empty;

                DataRow drEXC = dtPrismTDIEXC.NewRow();

                foreach (DataRow row in newTDITable.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row.ItemArray[4].ToString()))
                    {
                        strEmpName = row.ItemArray[4].ToString();
                        strLastName = strEmpName.Substring(0, strEmpName.IndexOf(","));
                        strFirstName = strEmpName.Substring(strEmpName.IndexOf(",") + 2);
                        ////load SSN and LOC
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013843' and per.EE_Last_Name='" + strLastName.ToString() + "' and per.EE_First_Name='" + strFirstName.ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //********************
                        //if empno not found or found but termed, save exception
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = "";
                                drEXC["EmpName"] = strLastName.ToString().Trim() + " " + strFirstName.ToString().Trim();
                                drEXC["ReasonForException"] = "Employee Name (ee_last_name, ee_first_name) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        //********************
                        row[colName] = drEXC["EmpName"];
                        row[colSSN] = drEXC["SSN"];
                        row[colEEID] = drEXC["EE_ID"];
                        row[colEENO] = drEXC["EE_NO"];
                        row[colEXCRSN] = drEXC["ReasonForException"];

                    }
                    else
                    {
                        row[colName] = drEXC["EmpName"];
                        row[colSSN] = drEXC["SSN"];
                        row[colEEID] = drEXC["EE_ID"];
                        row[colEENO] = drEXC["EE_NO"];
                        row[colEXCRSN] = drEXC["ReasonForException"];
                    }
                }

                //delete totals, emp name hdrs, authorized time and grand total lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if ((dr.ItemArray[14].ToString() == "Total:")
                        || (!string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString()))
                        || (dr.ItemArray[6].ToString() == "Grand Total:")
                        || (dr.ItemArray[6].ToString() == "Authorized Time:"))
                        dr.Delete();
                }
                newTDITable.AcceptChanges();

                //sort the remaining datatable by employee name, by jobcode, by payrate
                DataView dv = newTDITable.DefaultView;
                //sort by empname, by jobcode, by rate
                dv.Sort = "Column5, Column12, Column15";
                DataTable sortedDT = dv.ToTable();

                bool blnOverTime = false;
                double dblCalcHours = 0;
                double dblHoldRate = 0;
                string strHoldEmpName = string.Empty;
                string strHoldSSN = string.Empty;
                string strHoldEEID = string.Empty;
                string strHoldEENO = string.Empty;
                string strHoldEXCRSN = string.Empty;
                string strHoldLoc = string.Empty;
                string strHoldJob = string.Empty;
                blnFirstRow = true;

                foreach (DataRow CsvData in sortedDT.Rows)
                {
                    if (blnFirstRow)
                    {
                        dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString().Replace("$", "") : "0");
                        strHoldEmpName = CsvData.ItemArray[4].ToString();
                        strHoldSSN = CsvData["SSN"].ToString();
                        strHoldEEID = CsvData["EEID"].ToString();
                        strHoldEENO = CsvData["EENO"].ToString();
                        strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                        strHoldLoc = CsvData["LocationCode"].ToString();
                        strHoldJob = !string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : string.Empty;
                        if ((!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty) == "*Overtime*")
                        {
                            blnOverTime = true;
                        }
                        blnFirstRow = false;
                        continue;
                    }

                    if (
                        (strHoldEmpName == CsvData.ItemArray[4].ToString())
                        && (strHoldJob == (!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : string.Empty))
                        && dblHoldRate == double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString().Replace("$", "") : "0")
                        )
                    {
                        dblCalcHours += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                    }
                    else
                    {
                        //test for hourly data
                        if (dblCalcHours > 0)
                        {
                            DataRow drPayReg = dtPrismTDI.NewRow();
                            drPayReg["Ssn"] = strHoldSSN;
                            drPayReg["CodePos"] = blnOverTime ? "OT05" : "HOURLY";
                            drPayReg["HoursPos"] = dblCalcHours;
                            drPayReg["AmountPos"] = dblHoldRate;
                            drPayReg["Loc"] = strHoldLoc;
                            drPayReg["Job"] = strHoldJob;

                            drPayReg["ClientID"] = strClientID;
                            drPayReg["EE_ID"] = strHoldEEID;
                            drPayReg["EE_NO"] = strHoldEENO;
                            drPayReg["EmpName"] = strHoldEmpName;
                            drPayReg["ReasonForException"] = strHoldEXCRSN;

                            dtPrismTDI.Rows.Add(drPayReg);
                        }
                        //load current next data
                        dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[9].ToString()) ? CsvData.ItemArray[9].ToString() : "0");
                        dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[14].ToString()) ? CsvData.ItemArray[14].ToString().Replace("$", "") : "0");
                        strHoldEmpName = CsvData.ItemArray[4].ToString();
                        strHoldSSN = CsvData["SSN"].ToString();
                        strHoldEEID = CsvData["EEID"].ToString();
                        strHoldEENO = CsvData["EENO"].ToString();
                        strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                        strHoldLoc = CsvData["LocationCode"].ToString();
                        strHoldJob = !string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : string.Empty;
                        if ((!string.IsNullOrWhiteSpace(CsvData.ItemArray[16].ToString()) ? CsvData.ItemArray[16].ToString() : string.Empty) == "*Overtime*")
                        {
                            blnOverTime = true;
                        }
                        else
                        {
                            blnOverTime = false;
                        }
                    }
                }
                //test for hourly data for last record
                if (dblCalcHours > 0)
                {
                    DataRow drPayReg = dtPrismTDI.NewRow();
                    drPayReg["Ssn"] = strHoldSSN;
                    drPayReg["CodePos"] = blnOverTime ? "OT05" : "HOURLY";
                    drPayReg["HoursPos"] = dblCalcHours;
                    drPayReg["AmountPos"] = dblHoldRate;
                    drPayReg["Loc"] = strHoldLoc;
                    drPayReg["Job"] = strHoldJob;

                    drPayReg["ClientID"] = strClientID;
                    drPayReg["EE_ID"] = strHoldEEID;
                    drPayReg["EE_NO"] = strHoldEENO;
                    drPayReg["EmpName"] = strHoldEmpName;
                    drPayReg["ReasonForException"] = strHoldEXCRSN;

                    dtPrismTDI.Rows.Add(drPayReg);
                }

            }
        }
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013843\PrImport013843.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013843\PrImport013843.csv");

        ////save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //                SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013843\Exception013843.csv");
        //SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013843\Exception013843.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C013603()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }


        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013603\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013603\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                DataSet newTDIDS = ReadExcel(empFile);

                DataTable newTDITable = newTDIDS.Tables[0].Copy();
                //remove 6 headers
                for (int i = 5; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    dr.Delete();
                }
                newTDITable.AcceptChanges();


                //delete bottom SUMMARY section
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (!(dr.ItemArray[0].ToString().ToUpper().Contains("SUMMARY")))
                    {
                        dr.Delete();
                    }
                    else
                    {
                        break;
                    }
                }
                newTDITable.AcceptChanges();

                //delete all BLANK lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (
                        (string.IsNullOrWhiteSpace(dr.ItemArray[0].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[1].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[2].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[4].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[5].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[6].ToString()))
                        )
                    {
                        dr.Delete();
                    }
                }
                newTDITable.AcceptChanges();

                //delete all BOTTOM header lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (
                        (dr.ItemArray[0].ToString().ToUpper().Contains("EMP #"))
                        && (dr.ItemArray[1].ToString().ToUpper().Contains("EMPLOYEE NAME"))
                        && (dr.ItemArray[2].ToString().ToUpper().Contains("EMP SS#"))
                        && (dr.ItemArray[3].ToString().ToUpper().Contains("RATE"))
                        && (dr.ItemArray[4].ToString().ToUpper().Contains("HRS"))
                        && (dr.ItemArray[5].ToString().ToUpper().Contains("PAY"))
                        && (dr.ItemArray[6].ToString().ToUpper().Contains("RATE"))
                        )
                    {
                        dr.Delete();
                    }
                }
                newTDITable.AcceptChanges();

                //delete all TOP header lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (
                        (string.IsNullOrWhiteSpace(dr.ItemArray[0].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[1].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[2].ToString()))
                        && (dr.ItemArray[3].ToString().ToUpper().Contains("REG."))
                        && (dr.ItemArray[4].ToString().ToUpper().Contains("REG."))
                        && (dr.ItemArray[5].ToString().ToUpper().Contains("REG."))
                        && (dr.ItemArray[6].ToString().ToUpper().Contains("OT"))
                        )
                    {
                        dr.Delete();
                    }
                }
                newTDITable.AcceptChanges();

                //delete all MISC header lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (
                        (dr.ItemArray[0].ToString().ToUpper().Contains("***"))
                        || (dr.ItemArray[0].ToString().ToUpper().Contains("TTL EMP"))
                        || (dr.ItemArray[0].ToString().ToUpper().Contains("TOTALS:"))
                        || (dr.ItemArray[0].ToString().ToUpper().Contains("SUMMARY"))
                        )
                    {
                        dr.Delete();
                    }
                }
                newTDITable.AcceptChanges();

                //delete LAST BOTTOM total lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (
                        (string.IsNullOrWhiteSpace(dr.ItemArray[0].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[1].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[2].ToString()))
                        && (dr.ItemArray[3].ToString().ToUpper().Contains("------------"))
                        && (dr.ItemArray[4].ToString().ToUpper().Contains("------------"))
                        && (dr.ItemArray[5].ToString().ToUpper().Contains("------------"))
                        && (dr.ItemArray[6].ToString().ToUpper().Contains("------------"))
                        )
                    {
                        dr.Delete();
                        break;
                    }
                    else
                    {
                        dr.Delete();
                    }
                }
                newTDITable.AcceptChanges();


                //loop in reverse to add each empname and ssn to all rows for that employee
                string strSSN = string.Empty;
                DataColumn colSSN = newTDITable.Columns["Column2"];

                //update SSN from bottom up for every line per SSN
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (!string.IsNullOrWhiteSpace(dr.ItemArray[2].ToString()))
                    {
                        strSSN = dr.ItemArray[2].ToString();
                        continue;
                    }
                    else
                    {
                        dr[colSSN] = strSSN;
                    }
                }

                //loop forward to add each jobcode to all rows for that employee
                string strJOB = string.Empty;
                DataColumn colJOB = newTDITable.Columns["Column0"];

                //update SSN from bottom up for every line per SSN
                for (int i = 0; i <= newTDITable.Rows.Count - 1; i++)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (!string.IsNullOrWhiteSpace(dr.ItemArray[0].ToString()))
                    {
                        strJOB = dr.ItemArray[0].ToString();
                        continue;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(dr.ItemArray[1].ToString()))
                            dr[colJOB] = strJOB;
                    }
                }

                //delete all SPECIFIC NAME SUMMARY lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (
                        (string.IsNullOrWhiteSpace(dr.ItemArray[0].ToString()))
                        && (!string.IsNullOrWhiteSpace(dr.ItemArray[1].ToString()))
                        && (!string.IsNullOrWhiteSpace(dr.ItemArray[2].ToString()))
                        && (!string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString()))
                        && (!string.IsNullOrWhiteSpace(dr.ItemArray[4].ToString()))
                        && (!string.IsNullOrWhiteSpace(dr.ItemArray[5].ToString()))
                        && (!string.IsNullOrWhiteSpace(dr.ItemArray[6].ToString()))
                        )
                    {
                        dr.Delete();
                    }
                }
                newTDITable.AcceptChanges();

                //delete all JOB and SSN BLANK lines
                for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = newTDITable.Rows[i];
                    if (
                        (!string.IsNullOrWhiteSpace(dr.ItemArray[0].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[1].ToString()))
                        && (!string.IsNullOrWhiteSpace(dr.ItemArray[2].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[4].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[5].ToString()))
                        && (string.IsNullOrWhiteSpace(dr.ItemArray[6].ToString()))
                        )
                    {
                        dr.Delete();
                    }
                }
                newTDITable.AcceptChanges();

                foreach (DataRow CsvData in newTDITable.Rows)
                {
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013603' AND ssn.EE_SSN='" + CsvData.ItemArray[2].ToString() + "'";
                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = CsvData.ItemArray[2].ToString();
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = "";
                            drEXC["EmpName"] = CsvData.ItemArray[1].ToString().Trim().Replace(",", "");
                            drEXC["ReasonForException"] = "SSN NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                        drReg["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                        drReg["Loc"] = "1";

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT";
                        drOT["HoursPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0");
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[6].ToString()) ? CsvData.ItemArray[6].ToString() : "0") * 1.5);
                        drOT["Loc"] = "1";

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0") > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[11].ToString()) ? CsvData.ItemArray[11].ToString() : "0");
                        drTIPS["Loc"] = "1";

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }
                }
            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013603\PrImport013603.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013603\PrImport013603.csv");

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C013066()
    {

        Dictionary<string, string> dictLocations = new Dictionary<string, string>
            {
            {"24-7 Restaurant 1", "001"},
            {"East Pines 143", "143"},
            {"West Pines 334", "334"},
            {"Davie 533", "533"},
            {"Tamarac 736", "736"},
            {"Lake Worth 737", "737"},
            {"North Beach  885", "885"}
            };


        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string strPrismImportLOC = string.Empty;

        Boolean blnFirstRow = true;
        DataTable projTDITable = new DataTable();

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013066\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013066\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                //..................................................
                DataSet newTDIDS = ReadExcel(empFile);

                foreach (DataTable table in newTDIDS.Tables)
                {

                    DataTable newTDITable = table.Copy();
                    //remove 2 headers
                    for (int i = 1; i >= 0; i--)
                    {
                        DataRow dr = newTDITable.Rows[i];
                        dr.Delete();
                    }
                    newTDITable.AcceptChanges();


                    //delete totals, headers, misc other lines
                    for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = newTDITable.Rows[i];
                        if (!((dr.ItemArray[1].ToString().ToUpper().Contains("WEEK")) && (dr.ItemArray[1].ToString().Contains("1")))
                            && !((dr.ItemArray[1].ToString().ToUpper().Contains("WEEK")) && (dr.ItemArray[1].ToString().Contains("2")))
                            && !((dr.ItemArray[1].ToString().ToUpper().Contains("EXTRA"))))
                        {
                            dr.Delete();
                        }

                    }
                    newTDITable.AcceptChanges();

                    //add location column and populate it with filename (aka location)
                    newTDITable.Columns.Add(new DataColumn("LocationCode", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("SSN", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("CLIENT", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("EMPNAME", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("EENO", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("ExceptionReason", typeof(string)));

                    DataColumn colLOC = newTDITable.Columns["LocationCode"];
                    DataColumn colSSN = newTDITable.Columns["SSN"];
                    DataColumn colCLIENT = newTDITable.Columns["CLIENT"];
                    DataColumn colEMPNAME = newTDITable.Columns["EMPNAME"];
                    DataColumn colEENO = newTDITable.Columns["EENO"];
                    DataColumn colEXCRSN = newTDITable.Columns["ExceptionReason"];
                    DataColumn colEEID = newTDITable.Columns[0];

                    //load LOC and CLIENT to each row
                    strPrismImportLOC = dictLocations[!string.IsNullOrWhiteSpace(newTDITable.TableName.ToString().Trim()) ? newTDITable.TableName.ToString().Trim() : string.Empty];
                    foreach (DataRow row in newTDITable.Rows)
                    {
                        row[colLOC] = strPrismImportLOC;
                        row[colCLIENT] = strClientID;
                    }

                    //clean up junk characters in rate column
                    DataColumn colRateCLEAR = newTDITable.Columns["Column4"];
                    foreach (DataRow row in newTDITable.Rows)
                    {
                        if ((row.ItemArray[4].ToString() == ".")
                            || (row.ItemArray[4].ToString() == ",")
                            || (row.ItemArray[4].ToString() == "-")
                            || (row.ItemArray[4].ToString() == "*")
                            || (row.ItemArray[4].ToString() == "+")
                            || (row.ItemArray[4].ToString() == "/")
                            || (row.ItemArray[4].ToString() == "$"))
                        {
                            row[colRateCLEAR] = 0;
                        }
                    }

                    //loop to add each empname to all rows for that employee
                    string strEmpName = string.Empty;
                    string strEEID = string.Empty;
                    string strSSN = string.Empty;

                    DataRow drEXC = dtPrismTDIEXC.NewRow();

                    foreach (DataRow row in newTDITable.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(row.ItemArray[0].ToString()))
                        {
                            strEmpName = row.ItemArray[2].ToString();
                            strEEID = row.ItemArray[0].ToString();
                            ////load SSN and LOC
                            string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                                 "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                                 "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                                 " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                                 " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                                 " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                                 " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                                 " ON com.[EE_ID]=per.[EE_ID] " +
                                 " WHERE com.Client_ID= '013066' AND com.EE_ID='" + strEEID.ToString() + "'";

                            DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                            //********************
                            //if empno not found or found but termed, save exception
                            if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                            {
                                drEXC["ClientID"] = strClientID;
                                if (dtLookUp.Rows.Count == 0)
                                {
                                    drEXC["SSN"] = "000-00-0000";
                                    drEXC["EE_ID"] = strEEID;
                                    drEXC["EE_NO"] = "";
                                    drEXC["EmpName"] = strEmpName.ToString().Trim();
                                    drEXC["ReasonForException"] = "EMPNO (ee_id) NOT FOUND in Prism";
                                }
                                else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                                {
                                    drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                    drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                    drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                    drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                    drEXC["ReasonForException"] = "Employee TERMED in Prism";
                                }
                            }
                            else
                            {
                                drEXC["ClientID"] = strClientID;
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Valid Employee";
                            }

                            //********************
                            row[colEMPNAME] = drEXC["EmpName"];
                            row[colSSN] = drEXC["SSN"];
                            row[colEEID] = drEXC["EE_ID"];
                            row[colEENO] = drEXC["EE_NO"];
                            row[colEXCRSN] = drEXC["ReasonForException"];

                        }
                        else
                        {
                            row[colEMPNAME] = drEXC["EmpName"];
                            row[colSSN] = drEXC["SSN"];
                            row[colEEID] = drEXC["EE_ID"];
                            row[colEENO] = drEXC["EE_NO"];
                            row[colEXCRSN] = drEXC["ReasonForException"];
                        }
                    }

                    //sort the remaining datatable by employee ID, by rate descending
                    DataView dv = newTDITable.DefaultView;
                    //sort by empname, by jobcode, by rate
                    dv.Sort = "Column0, Column4 DESC";
                    DataTable sortedDT = dv.ToTable();

                    DataColumn colRate = sortedDT.Columns["Column4"];
                    //loop to fill rate to all empty rows for that employee
                    double dblRate = 0;
                    strEEID = string.Empty;
                    blnFirstRow = true;
                    foreach (DataRow row in sortedDT.Rows)
                    {
                        if (strEEID != row.ItemArray[0].ToString())
                        {
                            strEEID = row.ItemArray[0].ToString();
                            dblRate = double.Parse(!string.IsNullOrWhiteSpace(row.ItemArray[4].ToString()) ? row.ItemArray[4].ToString() : "0");
                            continue;
                        }

                        if ((double.Parse(!string.IsNullOrWhiteSpace(row.ItemArray[4].ToString()) ? row.ItemArray[4].ToString() : "0") == 0))
                        {
                            row[colRate] = dblRate;
                        }

                    }

                    //sort the remaining datatable by employee ID, by rate descending
                    DataView dv2 = sortedDT.DefaultView;
                    //sort by empname, by jobcode, by rate
                    dv2.Sort = "Column0, Column3 DESC";
                    DataTable sortedDT2 = dv2.ToTable();

                    strEEID = string.Empty;
                    //delete totals, headers, misc other lines
                    for (int i = 0; i <= sortedDT2.Rows.Count - 1; i++)
                    {
                        DataRow dr = sortedDT2.Rows[i];
                        if ((dr.ItemArray[3].ToString().ToUpper().Contains("SALARY"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("NO HOURS"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("K"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("VAC"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("DAY"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("WEEK"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains(","))
                            || ((Regex.IsMatch(dr.ItemArray[3].ToString().Trim(), @"^[a-zA-Z]+$")) && ((!string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString())) || (!string.IsNullOrWhiteSpace(dr.ItemArray[5].ToString())) || (!string.IsNullOrWhiteSpace(dr.ItemArray[8].ToString()))))
                            || strEEID == dr.ItemArray[0].ToString())
                        {
                            strEEID = dr.ItemArray[0].ToString();
                            dr.Delete();
                            i--;
                            continue;
                        }
                        else if ((string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString())) && (string.IsNullOrWhiteSpace(dr.ItemArray[5].ToString())) && (string.IsNullOrWhiteSpace(dr.ItemArray[8].ToString())))
                        {
                            dr.Delete();
                            i--;
                        }
                    }
                    sortedDT2.AcceptChanges();

                    //sort the remaining datatable by employee ID, by rate descending
                    DataView dv3 = sortedDT2.DefaultView;
                    //sort by empname, by jobcode, by rate
                    dv3.Sort = "Column0, Column4 DESC";
                    DataTable sortedDT3 = dv3.ToTable();

                    double dblCalcHours = 0;
                    double dblCalcOT = 0;
                    double dblCalcTIPS = 0;
                    double dblHoldRate = 0;

                    string strHoldEmpName = string.Empty;
                    string strHoldEENO = string.Empty;
                    string strHoldEXCRSN = string.Empty;

                    string strHoldEmpID = string.Empty;
                    string strHoldSSN = string.Empty;
                    string strHoldLoc = string.Empty;
                    string strHoldClient = string.Empty;
                    blnFirstRow = true;

                    foreach (DataRow CsvData in sortedDT3.Rows)
                    {
                        if (blnFirstRow)
                        {
                            dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                            dblCalcOT = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            dblCalcTIPS = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                            dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            strHoldEmpID = CsvData.ItemArray[0].ToString();
                            strHoldSSN = CsvData["SSN"].ToString();
                            strHoldEmpName = CsvData["EMPNAME"].ToString();
                            strHoldEENO = CsvData["EENO"].ToString();
                            strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                            strHoldLoc = CsvData["LocationCode"].ToString();
                            strHoldClient = CsvData["CLIENT"].ToString();
                            blnFirstRow = false;
                            continue;
                        }

                        if (
                            (strHoldEmpID == CsvData.ItemArray[0].ToString())
                            && dblHoldRate == double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0")
                            )
                        {
                            dblCalcHours += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                            dblCalcOT += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            dblCalcTIPS += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        }
                        else
                        {
                            //test for hourly data
                            if (dblCalcHours > 0)
                            {
                                DataRow drPayReg = dtPrismTDI.NewRow();
                                drPayReg["Ssn"] = strHoldSSN;
                                drPayReg["CodePos"] = "HOURLY";
                                drPayReg["HoursPos"] = dblCalcHours;
                                drPayReg["AmountPos"] = dblHoldRate;
                                drPayReg["Loc"] = strHoldLoc;

                                drPayReg["ClientID"] = strHoldClient;
                                drPayReg["EE_ID"] = strHoldEmpID;
                                drPayReg["EE_NO"] = strHoldEENO;
                                drPayReg["EmpName"] = strHoldEmpName;
                                drPayReg["ReasonForException"] = strHoldEXCRSN;

                                dtPrismTDI.Rows.Add(drPayReg);
                            }

                            //test for OT data
                            if (dblCalcOT > 0)
                            {
                                DataRow drOT = dtPrismTDI.NewRow();
                                drOT["Ssn"] = strHoldSSN;
                                drOT["CodePos"] = "OT10";
                                drOT["HoursPos"] = dblCalcOT;
                                drOT["AmountPos"] = dblHoldRate;
                                drOT["Loc"] = strHoldLoc;

                                drOT["ClientID"] = strHoldClient;
                                drOT["EE_ID"] = strHoldEmpID;
                                drOT["EE_NO"] = strHoldEENO;
                                drOT["EmpName"] = strHoldEmpName;
                                drOT["ReasonForException"] = strHoldEXCRSN;

                                dtPrismTDI.Rows.Add(drOT);
                            }

                            //test for TIPS data
                            if (dblCalcTIPS > 0)
                            {
                                DataRow drTips = dtPrismTDI.NewRow();
                                drTips["Ssn"] = strHoldSSN;
                                drTips["CodePos"] = "TIPS";
                                drTips["HoursPos"] = string.Empty;
                                drTips["AmountPos"] = dblCalcTIPS;
                                drTips["Loc"] = strHoldLoc;

                                drTips["ClientID"] = strHoldClient;
                                drTips["EE_ID"] = strHoldEmpID;
                                drTips["EE_NO"] = strHoldEENO;
                                drTips["EmpName"] = strHoldEmpName;
                                drTips["ReasonForException"] = strHoldEXCRSN;

                                dtPrismTDI.Rows.Add(drTips);
                            }

                            //load current next data
                            dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                            dblCalcOT = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            dblCalcTIPS = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                            dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            strHoldEmpID = CsvData.ItemArray[0].ToString();
                            strHoldSSN = CsvData["SSN"].ToString();
                            strHoldEmpName = CsvData["EMPNAME"].ToString();
                            strHoldEENO = CsvData["EENO"].ToString();
                            strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                            strHoldLoc = CsvData["LocationCode"].ToString();
                            strHoldClient = CsvData["CLIENT"].ToString();
                        }
                    }

                    //test for hourly data for last record
                    //test for hourly data
                    if (dblCalcHours > 0)
                    {
                        DataRow drPayReg = dtPrismTDI.NewRow();
                        drPayReg["Ssn"] = strHoldSSN;
                        drPayReg["CodePos"] = "HOURLY";
                        drPayReg["HoursPos"] = dblCalcHours;
                        drPayReg["AmountPos"] = dblHoldRate;
                        drPayReg["Loc"] = strHoldLoc;

                        drPayReg["ClientID"] = strHoldClient;
                        drPayReg["EE_ID"] = strHoldEmpID;
                        drPayReg["EE_NO"] = strHoldEENO;
                        drPayReg["EmpName"] = strHoldEmpName;
                        drPayReg["ReasonForException"] = strHoldEXCRSN;

                        dtPrismTDI.Rows.Add(drPayReg);
                    }

                    //test for OT data
                    if (dblCalcOT > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = strHoldSSN;
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = dblCalcOT;
                        drOT["AmountPos"] = dblHoldRate;
                        drOT["Loc"] = strHoldLoc;

                        drOT["ClientID"] = strHoldClient;
                        drOT["EE_ID"] = strHoldEmpID;
                        drOT["EE_NO"] = strHoldEENO;
                        drOT["EmpName"] = strHoldEmpName;
                        drOT["ReasonForException"] = strHoldEXCRSN;

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    //test for TIPS data
                    if (dblCalcTIPS > 0)
                    {
                        DataRow drTips = dtPrismTDI.NewRow();
                        drTips["Ssn"] = strHoldSSN;
                        drTips["CodePos"] = "TIPS";
                        drTips["HoursPos"] = string.Empty;
                        drTips["AmountPos"] = dblCalcTIPS;
                        drTips["Loc"] = strHoldLoc;

                        drTips["ClientID"] = strHoldClient;
                        drTips["EE_ID"] = strHoldEmpID;
                        drTips["EE_NO"] = strHoldEENO;
                        drTips["EmpName"] = strHoldEmpName;
                        drTips["ReasonForException"] = strHoldEXCRSN;

                        dtPrismTDI.Rows.Add(drTips);
                    }


                    //    MOVED THIS TO HERE (INSIDE THE LOOP) FOR THIS CLIENT SINCE INDIVIDUAL LOCATION FILES ARE NEEDED FOR INVOICING
                    SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013066\PrImport013066"  + strPrismImportLOC + ".csv");
                    //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013066\PrImport" + strClientID + strPrismImportLOC + ".csv");
                    dtPrismTDI.Clear();

                    //save exceptions
                    //                        if (dtPrismTDIEXC.Rows.Count > 0)
                    //                        {
                    //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013066\Exception" + strClientID + strPrismImportLOC + ".csv");
                    //                            SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013066\Exception" + strClientID + strPrismImportLOC + ".csv");
                    //                            dtPrismTDIEXC.Clear();
                    //                        }

                }
                //..................................................

            }

        }

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C013161_013248_013494()
    {
        Dictionary<string, string> dictLocations = new Dictionary<string, string>
            {
            {"West Flagler 462", "462"},
            {"Oakland Park 464", "464"},
            {"Virginia Gardens 467", "467"},
            {"Palm Coast 106", "106"},
            {"St Augustine 047", "047"},
            {"West Columbia 245", "245"},
            {"Columbia  181", "181"}
            };

        Dictionary<string, string> dictClients = new Dictionary<string, string>
            {
            {"West Flagler 462", "013161"},
            {"Oakland Park 464", "013161"},
            {"Virginia Gardens 467", "013161"},
            {"Palm Coast 106", "013494"},
            {"St Augustine 047", "013494"},
            {"West Columbia 245", "013248"},
            {"Columbia  181", "013248"}
            };

        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string strPrismImportLOC = string.Empty;

        Boolean blnFirstRow = true;
        DataTable projTDITable = new DataTable();

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013161_013248_013494\", "*.xlsx");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013161_013248_013494\", "*.xlsx");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                //..................................................
                DataSet newTDIDS = ReadExcel(empFile);

                foreach (DataTable table in newTDIDS.Tables)
                {
                    //added this test to get around a hidden excel chart
                    if (!dictLocations.ContainsKey(table.TableName.ToString().Trim()))
                    {
                        continue;
                    }

                    DataTable newTDITable = table.Copy();
                    //remove 2 headers
                    for (int i = 1; i >= 0; i--)
                    {
                        DataRow dr = newTDITable.Rows[i];
                        dr.Delete();
                    }
                    newTDITable.AcceptChanges();



                    //delete totals, headers, misc other lines
                    for (int i = newTDITable.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = newTDITable.Rows[i];
                        if (!((dr.ItemArray[1].ToString().ToUpper().Contains("WEEK")) && (dr.ItemArray[1].ToString().Contains("1")))
                            && !((dr.ItemArray[1].ToString().ToUpper().Contains("WEEK")) && (dr.ItemArray[1].ToString().Contains("2")))
                            && !((dr.ItemArray[1].ToString().ToUpper().Contains("EXTRA"))))
                        {
                            dr.Delete();
                        }

                    }
                    newTDITable.AcceptChanges();

                    //add location column and populate it with filename (aka location)
                    newTDITable.Columns.Add(new DataColumn("LocationCode", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("SSN", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("CLIENT", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("EMPNAME", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("EENO", typeof(string)));
                    newTDITable.Columns.Add(new DataColumn("ExceptionReason", typeof(string)));

                    DataColumn colLOC = newTDITable.Columns["LocationCode"];
                    DataColumn colSSN = newTDITable.Columns["SSN"];
                    DataColumn colCLIENT = newTDITable.Columns["CLIENT"];
                    DataColumn colEMPNAME = newTDITable.Columns["EMPNAME"];
                    DataColumn colEENO = newTDITable.Columns["EENO"];
                    DataColumn colEXCRSN = newTDITable.Columns["ExceptionReason"];
                    DataColumn colEEID = newTDITable.Columns[0];

                    //load LOC and CLIENT to each row
                    strPrismImportLOC = dictLocations[!string.IsNullOrWhiteSpace(newTDITable.TableName.ToString().Trim()) ? newTDITable.TableName.ToString().Trim() : string.Empty];
                    strClientID = dictClients[!string.IsNullOrWhiteSpace(newTDITable.TableName.ToString().Trim()) ? newTDITable.TableName.ToString().Trim() : string.Empty];
                    foreach (DataRow row in newTDITable.Rows)
                    {
                        row[colLOC] = strPrismImportLOC;
                        row[colCLIENT] = strClientID;
                    }

                    //clean up junk characters in rate column
                    DataColumn colRateCLEAR = newTDITable.Columns["Column4"];
                    foreach (DataRow row in newTDITable.Rows)
                    {
                        if ((row.ItemArray[4].ToString() == ".")
                            || (row.ItemArray[4].ToString() == ",")
                            || (row.ItemArray[4].ToString() == "-")
                            || (row.ItemArray[4].ToString() == "*")
                            || (row.ItemArray[4].ToString() == "+")
                            || (row.ItemArray[4].ToString() == "/")
                            || (row.ItemArray[4].ToString() == "$"))
                        {
                            row[colRateCLEAR] = 0;
                        }
                    }

                    //loop to add each empname to all rows for that employee
                    string strEmpName = string.Empty;
                    string strEEID = string.Empty;
                    string strSSN = string.Empty;

                    DataRow drEXC = dtPrismTDIEXC.NewRow();

                    foreach (DataRow row in newTDITable.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(row.ItemArray[0].ToString()))
                        {
                            strEmpName = row.ItemArray[2].ToString();
                            strEEID = row.ItemArray[0].ToString();
                            ////load SSN and LOC
                            string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                                 "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                                 "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                                 " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                                 " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                                 " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                                 " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                                 " ON com.[EE_ID]=per.[EE_ID] " +
                                 " WHERE com.Client_ID= '" + strClientID + "' AND com.EE_ID='" + strEEID.ToString() + "'";

                            DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                            //********************
                            //if empno not found or found but termed, save exception
                            if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                            {
                                drEXC["ClientID"] = strClientID;
                                if (dtLookUp.Rows.Count == 0)
                                {
                                    drEXC["SSN"] = "000-00-0000";
                                    drEXC["EE_ID"] = strEEID;
                                    drEXC["EE_NO"] = "";
                                    drEXC["EmpName"] = strEmpName.ToString().Trim();
                                    drEXC["ReasonForException"] = "EMPNO (ee_id) NOT FOUND in Prism";
                                }
                                else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                                {
                                    drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                    drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                    drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                    drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                    drEXC["ReasonForException"] = "Employee TERMED in Prism";
                                }
                            }
                            else
                            {
                                drEXC["ClientID"] = strClientID;
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Valid Employee";
                            }

                            //********************
                            row[colEMPNAME] = drEXC["EmpName"];
                            row[colSSN] = drEXC["SSN"];
                            row[colEEID] = drEXC["EE_ID"];
                            row[colEENO] = drEXC["EE_NO"];
                            row[colEXCRSN] = drEXC["ReasonForException"];

                        }
                        else
                        {
                            row[colEMPNAME] = drEXC["EmpName"];
                            row[colSSN] = drEXC["SSN"];
                            row[colEEID] = drEXC["EE_ID"];
                            row[colEENO] = drEXC["EE_NO"];
                            row[colEXCRSN] = drEXC["ReasonForException"];
                        }
                    }

                    //sort the remaining datatable by employee ID, by rate descending
                    DataView dv = newTDITable.DefaultView;
                    //sort by empname, by jobcode, by rate
                    dv.Sort = "Column0, Column4 DESC";
                    DataTable sortedDT = dv.ToTable();

                    DataColumn colRate = sortedDT.Columns["Column4"];
                    //loop to fill rate to all empty rows for that employee
                    double dblRate = 0;
                    strEEID = string.Empty;
                    blnFirstRow = true;
                    foreach (DataRow row in sortedDT.Rows)
                    {
                        if (strEEID != row.ItemArray[0].ToString())
                        {
                            strEEID = row.ItemArray[0].ToString();
                            dblRate = double.Parse(!string.IsNullOrWhiteSpace(row.ItemArray[4].ToString()) ? row.ItemArray[4].ToString() : "0");
                            continue;
                        }

                        if ((double.Parse(!string.IsNullOrWhiteSpace(row.ItemArray[4].ToString()) ? row.ItemArray[4].ToString() : "0") == 0))
                        {
                            row[colRate] = dblRate;
                        }

                    }

                    //sort the remaining datatable by employee ID, by rate descending
                    DataView dv2 = sortedDT.DefaultView;
                    //sort by empname, by jobcode, by rate
                    dv2.Sort = "Column0, Column3 DESC";
                    DataTable sortedDT2 = dv2.ToTable();

                    strEEID = string.Empty;
                    //delete totals, headers, misc other lines
                    for (int i = 0; i <= sortedDT2.Rows.Count - 1; i++)
                    {
                        DataRow dr = sortedDT2.Rows[i];
                        if ((dr.ItemArray[3].ToString().ToUpper().Contains("SALARY"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("NO HOURS"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("K"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("VAC"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("DAY"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains("WEEK"))
                            || (dr.ItemArray[3].ToString().ToUpper().Contains(","))
                            || ((Regex.IsMatch(dr.ItemArray[3].ToString().Trim(), @"^[a-zA-Z]+$")) && ((!string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString())) || (!string.IsNullOrWhiteSpace(dr.ItemArray[5].ToString())) || (!string.IsNullOrWhiteSpace(dr.ItemArray[8].ToString()))))
                            || strEEID == dr.ItemArray[0].ToString())
                        {
                            strEEID = dr.ItemArray[0].ToString();
                            dr.Delete();
                            i--;
                            continue;
                        }
                        else if ((string.IsNullOrWhiteSpace(dr.ItemArray[3].ToString())) && (string.IsNullOrWhiteSpace(dr.ItemArray[5].ToString())) && (string.IsNullOrWhiteSpace(dr.ItemArray[8].ToString())))
                        {
                            dr.Delete();
                            i--;
                        }
                    }
                    sortedDT2.AcceptChanges();

                    //sort the remaining datatable by employee ID, by rate descending
                    DataView dv3 = sortedDT2.DefaultView;
                    //sort by empname, by jobcode, by rate
                    dv3.Sort = "Column0, Column4 DESC";
                    DataTable sortedDT3 = dv3.ToTable();

                    double dblCalcHours = 0;
                    double dblCalcOT = 0;
                    double dblCalcTIPS = 0;
                    double dblHoldRate = 0;

                    string strHoldEmpName = string.Empty;
                    string strHoldEENO = string.Empty;
                    string strHoldEXCRSN = string.Empty;

                    string strHoldEmpID = string.Empty;
                    string strHoldSSN = string.Empty;
                    string strHoldLoc = string.Empty;
                    string strHoldClient = string.Empty;
                    blnFirstRow = true;

                    foreach (DataRow CsvData in sortedDT3.Rows)
                    {
                        if (blnFirstRow)
                        {
                            dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                            dblCalcOT = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            dblCalcTIPS = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                            dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            strHoldEmpID = CsvData.ItemArray[0].ToString();
                            strHoldSSN = CsvData["SSN"].ToString();
                            strHoldEmpName = CsvData["EMPNAME"].ToString();
                            strHoldEENO = CsvData["EENO"].ToString();
                            strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                            strHoldLoc = CsvData["LocationCode"].ToString();
                            strHoldClient = CsvData["CLIENT"].ToString();
                            blnFirstRow = false;
                            continue;
                        }

                        if (
                            (strHoldEmpID == CsvData.ItemArray[0].ToString())
                            && dblHoldRate == double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0")
                            )
                        {
                            dblCalcHours += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                            dblCalcOT += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            dblCalcTIPS += double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                        }
                        else
                        {
                            //test for hourly data
                            if (dblCalcHours > 0)
                            {
                                DataRow drPayReg = dtPrismTDI.NewRow();
                                drPayReg["Ssn"] = strHoldSSN;
                                drPayReg["CodePos"] = "HOURLY";
                                drPayReg["HoursPos"] = dblCalcHours;
                                drPayReg["AmountPos"] = dblHoldRate;
                                drPayReg["Loc"] = strHoldLoc;

                                drPayReg["ClientID"] = strHoldClient;
                                drPayReg["EE_ID"] = strHoldEmpID;
                                drPayReg["EE_NO"] = strHoldEENO;
                                drPayReg["EmpName"] = strHoldEmpName;
                                drPayReg["ReasonForException"] = strHoldEXCRSN;

                                dtPrismTDI.Rows.Add(drPayReg);
                            }

                            //test for OT data
                            if (dblCalcOT > 0)
                            {
                                DataRow drOT = dtPrismTDI.NewRow();
                                drOT["Ssn"] = strHoldSSN;
                                drOT["CodePos"] = "OT10";
                                drOT["HoursPos"] = dblCalcOT;
                                drOT["AmountPos"] = dblHoldRate;
                                drOT["Loc"] = strHoldLoc;

                                drOT["ClientID"] = strHoldClient;
                                drOT["EE_ID"] = strHoldEmpID;
                                drOT["EE_NO"] = strHoldEENO;
                                drOT["EmpName"] = strHoldEmpName;
                                drOT["ReasonForException"] = strHoldEXCRSN;

                                dtPrismTDI.Rows.Add(drOT);
                            }

                            //test for TIPS data
                            if (dblCalcTIPS > 0)
                            {
                                DataRow drTips = dtPrismTDI.NewRow();
                                drTips["Ssn"] = strHoldSSN;
                                drTips["CodePos"] = "TIPS";
                                drTips["HoursPos"] = string.Empty;
                                drTips["AmountPos"] = dblCalcTIPS;
                                drTips["Loc"] = strHoldLoc;

                                drTips["ClientID"] = strHoldClient;
                                drTips["EE_ID"] = strHoldEmpID;
                                drTips["EE_NO"] = strHoldEENO;
                                drTips["EmpName"] = strHoldEmpName;
                                drTips["ReasonForException"] = strHoldEXCRSN;

                                dtPrismTDI.Rows.Add(drTips);
                            }

                            //load current next data
                            dblCalcHours = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0");
                            dblCalcOT = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0");
                            dblCalcTIPS = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : "0");
                            dblHoldRate = double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0");
                            strHoldEmpID = CsvData.ItemArray[0].ToString();
                            strHoldSSN = CsvData["SSN"].ToString();
                            strHoldEmpName = CsvData["EMPNAME"].ToString();
                            strHoldEENO = CsvData["EENO"].ToString();
                            strHoldEXCRSN = CsvData["ExceptionReason"].ToString();
                            strHoldLoc = CsvData["LocationCode"].ToString();
                            strHoldClient = CsvData["CLIENT"].ToString();
                        }
                    }

                    //test for hourly data for last record
                    //test for hourly data
                    if (dblCalcHours > 0)
                    {
                        DataRow drPayReg = dtPrismTDI.NewRow();
                        drPayReg["Ssn"] = strHoldSSN;
                        drPayReg["CodePos"] = "HOURLY";
                        drPayReg["HoursPos"] = dblCalcHours;
                        drPayReg["AmountPos"] = dblHoldRate;
                        drPayReg["Loc"] = strHoldLoc;

                        drPayReg["ClientID"] = strHoldClient;
                        drPayReg["EE_ID"] = strHoldEmpID;
                        drPayReg["EE_NO"] = strHoldEENO;
                        drPayReg["EmpName"] = strHoldEmpName;
                        drPayReg["ReasonForException"] = strHoldEXCRSN;

                        dtPrismTDI.Rows.Add(drPayReg);
                    }

                    //test for OT data
                    if (dblCalcOT > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = strHoldSSN;
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = dblCalcOT;
                        drOT["AmountPos"] = dblHoldRate;
                        drOT["Loc"] = strHoldLoc;

                        drOT["ClientID"] = strHoldClient;
                        drOT["EE_ID"] = strHoldEmpID;
                        drOT["EE_NO"] = strHoldEENO;
                        drOT["EmpName"] = strHoldEmpName;
                        drOT["ReasonForException"] = strHoldEXCRSN;

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    //test for TIPS data
                    if (dblCalcTIPS > 0)
                    {
                        DataRow drTips = dtPrismTDI.NewRow();
                        drTips["Ssn"] = strHoldSSN;
                        drTips["CodePos"] = "TIPS";
                        drTips["HoursPos"] = string.Empty;
                        drTips["AmountPos"] = dblCalcTIPS;
                        drTips["Loc"] = strHoldLoc;

                        drTips["ClientID"] = strHoldClient;
                        drTips["EE_ID"] = strHoldEmpID;
                        drTips["EE_NO"] = strHoldEENO;
                        drTips["EmpName"] = strHoldEmpName;
                        drTips["ReasonForException"] = strHoldEXCRSN;

                        dtPrismTDI.Rows.Add(drTips);
                    }


                    //    MOVED THIS TO HERE (INSIDE THE LOOP) FOR THIS CLIENT SINCE INDIVIDUAL LOCATION FILES ARE NEEDED FOR INVOICING
                    SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013161_013248_013494\PrImport" + strClientID + strPrismImportLOC + ".csv");
                    //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013161_013248_013494\PrImport" + strClientID + strPrismImportLOC + ".csv");
                    dtPrismTDI.Clear();

                    //save exceptions
                    //                        if (dtPrismTDIEXC.Rows.Count > 0)
                    //                        {
                    //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013161_013248_013494\Exception" + strClientID + strPrismImportLOC + ".csv");
                    //                            SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013161_013248_013494\Exception" + strClientID + strPrismImportLOC + ".csv");
                    //                            dtPrismTDIEXC.Clear();
                    //                        }
                }

            }
            //..................................................

        }

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C013749()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013749\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013749\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();
                resultDT.Clear();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013749' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }
        
        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013749\PrImport013749.csv");

        SQLConnectionClose();

    }

    static void C014217()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014217\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\014217\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '014217' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014217\PrImport014217.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\014217\PrImport014217.csv");

        //Close SQL connection
        SQLConnectionClose();

    }






    static void C013754()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013754\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013754\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013754' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013754\PrImport013754.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013754\PrImport013754.csv");

        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013754\Exception013754.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013754\Exception013754.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();
    }


    static void C013736()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013736\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013736\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013736' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013736\PrImport013736.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013736\PrImport013736.csv");

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C013751()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013751\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013751\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013751' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013751\PrImport013751.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013751\PrImport013751.csv");

        //Close SQL connection
        SQLConnectionClose();


    }

    static void C013745()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013745\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013745\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013745' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }
                }
            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013745\PrImport013745.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013745\PrImport013745.csv");

        //Close SQL connection
        SQLConnectionClose();

    }

    static void C014269()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }
        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014269\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\014269\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '014269' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }
                }
            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014269\PrImport014269.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\014269\PrImport014269.csv");

        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\014269\Exception014269.csv");
        //                SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\014269\Exception014269.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();



    }




        static void C013750()
    {
        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //......................................................................................................
        DataTable resultDT = new DataTable("csvDataTable");
        resultDT.Columns.Add(new DataColumn("EmpNo", typeof(string)));
        resultDT.Columns.Add(new DataColumn("JobCode", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SalaryFlag", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("RegRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTHrs", typeof(string)));
        resultDT.Columns.Add(new DataColumn("OTRate", typeof(string)));
        resultDT.Columns.Add(new DataColumn("CashTips", typeof(string)));
        resultDT.Columns.Add(new DataColumn("Location", typeof(string)));
        resultDT.Columns.Add(new DataColumn("SSN", typeof(string)));

        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013750\", "*.txt");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013750\", "*.txt");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {
                using (StreamReader reader = new StreamReader(empFile))
                {
                    while (true)
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                        {
                            break;
                        }
                        DataRow row = resultDT.NewRow();
                        row["EmpNo"] = line.Substring(2, 10);
                        row["JobCode"] = line.Substring(24, 5);
                        row["SalaryFlag"] = line.Substring(29, 1);
                        row["RegHrs"] = line.Substring(30, 6);
                        row["RegRate"] = line.Substring(37, 6);
                        row["OTHrs"] = line.Substring(44, 6);
                        row["OTRate"] = line.Substring(51, 6);
                        row["CashTips"] = line.Substring(119, 8);
                        row["Location"] = Path.GetFileNameWithoutExtension(empFile);
                        row["SSN"] = string.Empty;
                        resultDT.Rows.Add(row);
                    }
                }
                //......................................................................................................

                DataTable newTDITable = resultDT.Copy();

                foreach (DataRow CsvData in newTDITable.Rows)
                {

                    string strEENO = CsvData.ItemArray[0].ToString();
                    string strLastFour = strEENO.Substring(strEENO.Length - 4);
                    //load SSN 
                    string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                         "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                         "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                         " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                         " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                         " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                         " ON com.[EE_ID]=per.[EE_ID] " +
                         " WHERE com.Client_ID= '013750' AND com.EE_No='" + strLastFour.ToString() + "'";

                    DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                    //if empno not found or found but termed, save exception
                    DataRow drEXC = dtPrismTDIEXC.NewRow();
                    if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                    {
                        drEXC["ClientID"] = strClientID;
                        if (dtLookUp.Rows.Count == 0)
                        {
                            drEXC["SSN"] = "000-00-0000";
                            drEXC["EE_ID"] = "";
                            drEXC["EE_NO"] = strLastFour.ToString();
                            drEXC["EmpName"] = "No Name Provided";
                            drEXC["ReasonForException"] = "LastFourSSN (ee_no) NOT FOUND in Prism";
                        }
                        else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                        {
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Employee TERMED in Prism";
                        }
                    }
                    else
                    {
                        drEXC["ClientID"] = strClientID;
                        drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                        drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                        drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                        drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                        drEXC["ReasonForException"] = "Valid Employee";
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01) > 0)
                    {
                        DataRow drReg = dtPrismTDI.NewRow();
                        drReg["Ssn"] = drEXC["SSN"];
                        drReg["CodePos"] = "HOURLY";
                        drReg["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") * .01);
                        drReg["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drReg["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drReg["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drReg["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drReg["ClientID"] = drEXC["ClientID"];
                        drReg["EE_ID"] = drEXC["EE_ID"];
                        drReg["EE_NO"] = drEXC["EE_NO"];
                        drReg["EmpName"] = drEXC["EmpName"];
                        drReg["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drReg);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01) > 0)
                    {
                        DataRow drOT = dtPrismTDI.NewRow();
                        drOT["Ssn"] = drEXC["SSN"];
                        drOT["CodePos"] = "OT10";
                        drOT["HoursPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") * .01);
                        drOT["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") * .001);
                        drOT["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drOT["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drOT["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drOT["ClientID"] = drEXC["ClientID"];
                        drOT["EE_ID"] = drEXC["EE_ID"];
                        drOT["EE_NO"] = drEXC["EE_NO"];
                        drOT["EmpName"] = drEXC["EmpName"];
                        drOT["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drOT);
                    }

                    if ((double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01) > 0)
                    {
                        DataRow drTIPS = dtPrismTDI.NewRow();
                        drTIPS["Ssn"] = drEXC["SSN"];
                        drTIPS["CodePos"] = "TIPS";
                        drTIPS["HoursPos"] = string.Empty;
                        drTIPS["AmountPos"] = (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[7].ToString()) ? CsvData.ItemArray[7].ToString() : "0") * .01);
                        drTIPS["Loc"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[8].ToString()) ? CsvData.ItemArray[8].ToString() : string.Empty;
                        drTIPS["Dept"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;
                        drTIPS["Job"] = !string.IsNullOrWhiteSpace(CsvData.ItemArray[1].ToString()) ? CsvData.ItemArray[1].ToString().Substring(2, 3) : string.Empty;

                        drTIPS["ClientID"] = drEXC["ClientID"];
                        drTIPS["EE_ID"] = drEXC["EE_ID"];
                        drTIPS["EE_NO"] = drEXC["EE_NO"];
                        drTIPS["EmpName"] = drEXC["EmpName"];
                        drTIPS["ReasonForException"] = drEXC["ReasonForException"];

                        dtPrismTDI.Rows.Add(drTIPS);
                    }

                }

            }
        }

        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013750\PrImport013750.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013750\PrImport013750.csv");

        //Close SQL connection
        SQLConnectionClose();


    }



    static void C013429old()
    {
        //AUTOMATED ZIP FILE PROCESS - NEEDS TO BE TWEAKED
        /*
                    //string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013429\", "*.zip");
                    string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013429\", "*.zip");
                    foreach (string empFile in empFiles)
                    {
                        if (File.Exists(empFile))
                        {
        //                    var source = @"E:\0112_HACP_025613.zip";  //Location of Source Files
        //                    var destination = @"E:\extract"; //Location of files to be worked on in Bridge


                            var source = empFile;  //Location of Source Files
                            var destination = Path.GetDirectoryName(empFile)+"\\extract"; //Location of files to be worked on in Bridge

                            ExtractFile(source, destination);
                            DeleteFolders(destination);
                            VerifySites(destination);
                            GetFiles(destination);

                        }
                    }
        */
        Dictionary<string, string> dictLocations = new Dictionary<string, string>
            {
            {"1500981", "1"},
            {"1500724", "2"},
            {"1500975", "3"},
            {"1502793", "4"},
            {"1500671", "5"},
            {"1506170", "6"},
            {"1506316", "7"},
            {"1506397", "8"},
            {"1506418", "9"},
            {"1506400", "10"}
            };

        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        //PROCESS HOURS FILES
        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013429\", "*_LaborHours.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013429\", "*_LaborHours.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                DataTable newTDITable = ReadCsv(empFile, false, 4);

                if (!(newTDITable.Rows.Count == 0))
                {

                    foreach (DataRow CsvData in newTDITable.Rows)
                    {

                        string strEENO = CsvData.ItemArray[2].ToString().PadLeft(8, '0');
                        //load SSN 
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013429' AND com.EE_No='" + strEENO.ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = strEENO.ToString();
                                drEXC["EmpName"] = "No Name Provided";
                                drEXC["ReasonForException"] = "Column3 (ee_no) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }

                        double regHrs = 0;
                        double otHrs = 0;
                        //if hours worked are greater than 40 >40 - they have to be split to reg and ot
                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[4].ToString()) ? CsvData.ItemArray[4].ToString() : "0") > 40)
                        {
                            regHrs = 40;
                            otHrs = (double.Parse(CsvData.ItemArray[4].ToString()) - 40);
                        }
                        else
                        {
                            regHrs = (double.Parse(CsvData.ItemArray[4].ToString()));
                            otHrs = 0;

                        }

                        if (regHrs > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = regHrs;
                            drReg["AmountPos"] = double.Parse("0");
                            drReg["Loc"] = dictLocations[!string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty];

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                        if (otHrs > 0)
                        {
                            DataRow drOT = dtPrismTDI.NewRow();
                            drOT["Ssn"] = drEXC["SSN"];
                            drOT["CodePos"] = "OT";
                            drOT["HoursPos"] = otHrs;
                            drOT["AmountPos"] = double.Parse("0");
                            drOT["Loc"] = dictLocations[!string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty];

                            drOT["ClientID"] = drEXC["ClientID"];
                            drOT["EE_ID"] = drEXC["EE_ID"];
                            drOT["EE_NO"] = drEXC["EE_NO"];
                            drOT["EmpName"] = drEXC["EmpName"];
                            drOT["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drOT);
                        }
                    }
                }
            }
        }


        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013429\PrImport013429.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013429\PrImport013429.csv");

        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //                SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013429\Exceptions.csv");
        //SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013429\Exceptions.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();


    }

    //-------PrImport012804--------------------------------------------------Client TDI ENDS--------------------------------------------------

    static void C013429()
    {
        Dictionary<string, string> dictLocations = new Dictionary<string, string>
            {
            {"1500981", "1"},
            {"1500724", "2"},
            {"1500975", "3"},
            {"1502793", "4"},
            {"1500671", "5"},
            {"1506170", "6"},
            {"1506316", "7"},
            {"1506397", "8"},
            {"1506418", "9"},
            {"1506400", "10"}
            };

        //open sql connection
        SQLConnectionOpen(SQLServer, SQLDatabase);

        Type PrismTDI = typeof(PrismTimeDataImport);
        PropertyInfo[] properties = PrismTDI.GetProperties();
        DataTable dtPrismTDI = new DataTable();
        foreach (PropertyInfo pi in properties)
        {
            dtPrismTDI.Columns.Add(pi.Name);
        }

        //TDIException
        Type PrismTDIEXC = typeof(TDIException);
        PropertyInfo[] propertiesEXC = PrismTDIEXC.GetProperties();
        DataTable dtPrismTDIEXC = new DataTable();
        foreach (PropertyInfo pi in propertiesEXC)
        {
            dtPrismTDIEXC.Columns.Add(pi.Name);
        }

        string strPrismImportLOC = string.Empty;

        //PROCESS FILES
        string[] empFiles = Directory.GetFiles(@"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013429\", "*.csv");
        //string[] empFiles = Directory.GetFiles(@"K:\Payroll\TDI\013429\", "*.csv");
        foreach (string empFile in empFiles)
        {
            if (File.Exists(empFile))
            {

                DataTable newTDITable = ReadCsv(empFile, false, 6);

                if (!(newTDITable.Rows.Count == 0))
                {
                    //add location column and populate it with filename (aka location)
                    newTDITable.Columns.Add(new DataColumn("LocationCode", typeof(string)));
                    DataColumn colLOC = newTDITable.Columns["LocationCode"];
                    strPrismImportLOC = dictLocations[!string.IsNullOrWhiteSpace(Path.GetFileNameWithoutExtension(empFile).Substring(0, 7).ToString().Trim()) ? Path.GetFileNameWithoutExtension(empFile).Substring(0, 7).ToString().Trim() : string.Empty];
                    foreach (DataRow row in newTDITable.Rows)
                        row[colLOC] = strPrismImportLOC;

                    foreach (DataRow CsvData in newTDITable.Rows)
                    {
                        //force end of loop if totals line is reached
                        if (CsvData.ItemArray[0].ToString().Contains("Totals"))
                        {
                            continue;
                        }
                        string strNAME = CsvData.ItemArray[0].ToString().Substring(0, CsvData.ItemArray[0].ToString().IndexOf(" - "));
                        string strEENO = CsvData.ItemArray[0].ToString().Substring(CsvData.ItemArray[0].ToString().IndexOf(" - ") + " - ".Length);
                        //load SSN 
                        string strSQLQuery = "SELECT com.[Client_ID],com.[EE_ID],com.[Home_Loc_Code],ssn.EE_SSN," +
                             "com.[EE_Status_Code],com.[EE_No],per.[EE_First_Name],per.[EE_Last_Name], " +
                             "com.[Home_Dept_Code],com.[Home_Div_Code],com.[EE_Job_Code] " +
                             " FROM [Welland_Export_Cloud].[dbo].[EMPLOYEE_COM] com " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_SSN] ssn " +
                             " ON com.[Client_ID]=ssn.[Client_ID] and com.[EE_ID]=ssn.[EE_ID] " +
                             " LEFT JOIN [Welland_Export_Cloud].[dbo].[EMPLOYEE_PER] per " +
                             " ON com.[EE_ID]=per.[EE_ID] " +
                             " WHERE com.Client_ID= '013429' AND com.EE_No='" + strEENO.ToString() + "'";
                        DataTable dtLookUp = SQLGetTableData(strSQLQuery);

                        //if empno not found or found but termed, save exception
                        DataRow drEXC = dtPrismTDIEXC.NewRow();
                        if ((dtLookUp.Rows.Count == 0) || (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T"))
                        {
                            drEXC["ClientID"] = strClientID;
                            if (dtLookUp.Rows.Count == 0)
                            {
                                drEXC["SSN"] = "000-00-0000";
                                drEXC["EE_ID"] = "";
                                drEXC["EE_NO"] = strEENO.ToString();
                                drEXC["EmpName"] = strNAME.ToString();
                                drEXC["ReasonForException"] = "Column0 (ee_no) NOT FOUND in Prism";
                            }
                            else if (dtLookUp.Rows.Count == 1 && dtLookUp.Rows[0].ItemArray[4].ToString() == "T")
                            {
                                drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                                drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                                drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                                drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                                drEXC["ReasonForException"] = "Employee TERMED in Prism";
                            }
                        }
                        else
                        {
                            drEXC["ClientID"] = strClientID;
                            drEXC["SSN"] = dtLookUp.Rows[0].ItemArray[3].ToString();
                            drEXC["EE_ID"] = dtLookUp.Rows[0].ItemArray[1].ToString();
                            drEXC["EE_NO"] = dtLookUp.Rows[0].ItemArray[5].ToString();
                            drEXC["EmpName"] = dtLookUp.Rows[0].ItemArray[7].ToString().Trim() + " " + dtLookUp.Rows[0].ItemArray[6].ToString().Trim();
                            drEXC["ReasonForException"] = "Valid Employee";
                        }


                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[3].ToString()) ? CsvData.ItemArray[3].ToString() : "0") > 0)
                        {
                            DataRow drReg = dtPrismTDI.NewRow();
                            drReg["Ssn"] = drEXC["SSN"];
                            drReg["CodePos"] = "HOURLY";
                            drReg["HoursPos"] = double.Parse(CsvData.ItemArray[3].ToString());
                            drReg["AmountPos"] = double.Parse("0");
                            drReg["Loc"] = CsvData["LocationCode"].ToString();
                            //                                    dictLocations[!string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty];

                            drReg["ClientID"] = drEXC["ClientID"];
                            drReg["EE_ID"] = drEXC["EE_ID"];
                            drReg["EE_NO"] = drEXC["EE_NO"];
                            drReg["EmpName"] = drEXC["EmpName"];
                            drReg["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drReg);
                        }

                        if (double.Parse(!string.IsNullOrWhiteSpace(CsvData.ItemArray[5].ToString()) ? CsvData.ItemArray[5].ToString() : "0") > 0)
                        {
                            DataRow drOT = dtPrismTDI.NewRow();
                            drOT["Ssn"] = drEXC["SSN"];
                            drOT["CodePos"] = "OT";
                            drOT["HoursPos"] = double.Parse(CsvData.ItemArray[5].ToString());
                            drOT["AmountPos"] = double.Parse("0");
                            drOT["Loc"] = CsvData["LocationCode"].ToString();
                            //                                    dictLocations[!string.IsNullOrWhiteSpace(CsvData.ItemArray[0].ToString()) ? CsvData.ItemArray[0].ToString() : string.Empty];

                            drOT["ClientID"] = drEXC["ClientID"];
                            drOT["EE_ID"] = drEXC["EE_ID"];
                            drOT["EE_NO"] = drEXC["EE_NO"];
                            drOT["EmpName"] = drEXC["EmpName"];
                            drOT["ReasonForException"] = drEXC["ReasonForException"];

                            dtPrismTDI.Rows.Add(drOT);
                        }
                    }
                }
            }
        }


        SaveToCSV(dtPrismTDI, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013429\PrImport013429.csv");
        //SaveToCSV(dtPrismTDI, @"K:\Payroll\TDI\013429\PrImport013429.csv");

        //save exceptions
        //            if (dtPrismTDIEXC.Rows.Count > 0)
        //            {
        //                SaveToCSV(dtPrismTDIEXC, @"\\tpaa-pwb-web02\c$\inetpub\wwwroot\bridge\TDI\013429\Exceptions.csv");
        //SaveToCSV(dtPrismTDIEXC, @"K:\Payroll\TDI\013429\Exceptions.csv");
        //            }

        //Close SQL connection
        SQLConnectionClose();

    }









    public static DataTable ReadAllExcelRows2(string file, bool containsHeaderRow)
    {
        var fileBytes = File.ReadAllBytes(@file);
        DataTable rowsDT = new DataTable();
        DataSet dataSet;

        var stream = new MemoryStream(fileBytes);
        stream.Write(fileBytes, 0, file.Length);

        using (stream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                dataSet = reader.AsDataSet();
            }
        }

        if (dataSet == null) return rowsDT;

        foreach (DataTable table in dataSet.Tables)
        {
            rowsDT = table.Copy();
            //                foreach (DataRow row in table.Rows)
            //                {
            //                    rows.Add(string.Join(",", row.ItemArray));
            //                }
        }

        if (containsHeaderRow)
        {
            //                rows.RemoveAt(0);
        }

        return rowsDT;
    }

    //ReadExcel
    public static DataSet ReadExcel(string file)
    {
        var fileBytes = File.ReadAllBytes(@file);

        DataSet dataSet = new DataSet();

        var stream = new MemoryStream(fileBytes);
        stream.Write(fileBytes, 0, file.Length);

        using (stream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                dataSet = reader.AsDataSet();
            }
        }

        return dataSet;
    }


    //ReadCSV
    public static DataTable ReadCsv(string strFileName, bool blnHasFirstRow, int intSkipRows)
    {
        string csvDataModel = "CSVData";
        csvDataModel = csvDataModel + strClientID;
        DataTable resultDT = new DataTable(csvDataModel);

        bool blnSkipRows = intSkipRows > 0 ? true : false;
        using (StreamReader reader = new StreamReader(strFileName))
        {
            string pattern = ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))";  //Should be commas that are not encapsulated in quotation marks
            Regex r = new Regex(pattern);
            while (true)
            {
                if (blnSkipRows)
                {
                    //skip headers rows
                    for (int s = 1; s <= intSkipRows; s++)
                    {
                        string lineskip = reader.ReadLine();
                    }
                    blnSkipRows = false;
                }

                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                string[] fields = r.Split(line);

                if (blnHasFirstRow)
                {
                    int dupCol = 0;
                    string dupColName = string.Empty;

                    foreach (string field in fields)
                    {
                        if (!resultDT.Columns.Contains(field))
                        {
                            resultDT.Columns.Add(new DataColumn(field, typeof(string)));
                        }
                        else
                        {
                            dupColName = field + dupCol.ToString();
                            resultDT.Columns.Add(new DataColumn(dupColName, typeof(string)));
                            dupCol++;
                        }
                    }
                    blnHasFirstRow = false;
                }
                else
                {
                    if (resultDT.Columns.Count == 0)
                    {
                        int noCol = 0;
                        string colname = "Field";
                        foreach (string field in fields)
                        {
                            colname = "Field" + noCol.ToString();
                            resultDT.Columns.Add(new DataColumn(colname, typeof(string)));
                            noCol++;
                        }
                    }
                    int i = 0;
                    DataRow row = resultDT.NewRow();
                    foreach (string field in fields)
                    {
                        row[i++] = field.Replace("'", "").Replace("\"", "");
                    }
                    resultDT.Rows.Add(row);
                }
            }
        }
        return resultDT;
    }


    //SaveToCSV
    public static void SaveToCSV(DataTable dtDataTable, string strFilePath)
    {
        StreamWriter sw = new StreamWriter(strFilePath, false);
        //headers    
        for (int i = 0; i < dtDataTable.Columns.Count; i++)
        {
            sw.Write(dtDataTable.Columns[i]);
            if (i < dtDataTable.Columns.Count - 1)
            {
                sw.Write(",");
            }
        }
        sw.Write(sw.NewLine);
        foreach (DataRow dr in dtDataTable.Rows)
        {
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                if (!Convert.IsDBNull(dr[i]))
                {
                    string value = dr[i].ToString();
                    if (value.Contains(','))
                    {
                        value = String.Format("\"{0}\"", value);
                        sw.Write(value);
                    }
                    else
                    {
                        sw.Write(dr[i].ToString());
                    }
                }
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
        }
      
        sw.Close();
        return;
    }

    //SQL Connection
  
    public static void SQLConnectionOpen(string sqlServer, string sqlDatabase)
    {
        string connectionString;
        connectionString = string.Format(@"Data Source=MBASQL;Integrated Security=false;User ID=Internaluser;Password=Mba123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;MultiSubnetFailover=False", sqlServer, sqlDatabase);

        cnn = new SqlConnection(connectionString);
        cnn.Open();
    }


    public static void SQLConnectionClose()
    {
        cnn.Close();
    }

    public static DataTable SQLGetTableData(string sqlQuery)
    {
        DataTable dtSQL = new DataTable();
        SqlCommand sqlCommand;
        string sql = "";
        sql = sqlQuery;
        sqlCommand = new SqlCommand(sql, cnn);
        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
        {
            sqlDataAdapter.Fill(dtSQL);
        }

        return dtSQL;
    }


    //......................................... Classes ............................................

    public class PrismTimeDataImport
    {
        public string Ssn { get; set; }
        public string CodePos { get; set; }
        public string HoursPos { get; set; }
        public string AmountPos { get; set; }
        public string Loc { get; set; }
        public string Dept { get; set; }
        public string Div { get; set; }
        public string Job { get; set; }
        public string ClientID { get; set; }
        public string EE_ID { get; set; }
        public string EE_NO { get; set; }
        public string EmpName { get; set; }
        public string ReasonForException { get; set; }
        public string Shift { get; set; }
    }

    public class TDIException
    {
        public string ClientID { get; set; }
        public string SSN { get; set; }
        public string EE_ID { get; set; }
        public string EE_NO { get; set; }
        public string EmpName { get; set; }
        public string ReasonForException { get; set; }
    }








    /*     old one 
    public class PrismTimeDataImport
    {
        public string Ssn { get; set; }
        public string CodePos { get; set; }
        public string HoursPos { get; set; }
        public string AmountPos { get; set; }
        public string Loc { get; set; }
        public string Dept { get; set; }
        public string Div { get; set; }
        public string Job { get; set; }
    }

    public class TDIException
    {
        public string ClientID { get; set; }
        public string SSN { get; set; }
        public string EE_ID { get; set; }
        public string EE_NO { get; set; }
        public string EmpName { get; set; }
        public string Loc { get; set; }
        public string Dept { get; set; }
        public string Div { get; set; }
        public string Job { get; set; }
        public string ReasonForException { get; set; }
    }
    */





    public static class PayType
    {
        public const string Hourly = "HOURLY";
        public const string Overtime = "OT";
        public const string Overtime1 = "OT1";
        public const string Overtime5 = "OT05";
        public const string Overtime10 = "OT10";
        public const string Vacation = "VACATION";
        public const string PaidTimeOff = "PTO";
        public const string Holiday = "HOLIDAY";
        public const string PieceWork = "PIECE";
        public const string Commission = "COMM-R";
        public const string Commission2 = "COMM2-R";
        public const string Bonus = "BONUSN-RT";
        public const string Expenses = "EXPREIM";
        public const string Tips = "TIPS";
        public const string MileageExpense = "MILEEXCESS";
        public const string Hourly1 = "HOURLY1";
        public const string Hourly2 = "HOURLY2";
        public const string Hourly3 = "HOURLY3";
        public const string Memo = "MEMO";
        public const string TipCC = "TIPCC";
        public const string MiscPay = "MISCPAY";
        public const string Salary = "SALARY";
        public const string Bereavement = "BEREAVEMENT";
        public const string NonPayHours3 = "NONPAYHRS3";
        public const string DailyRate = "DAILYRATE";
        public const string BackPay = "BACKPAY";
        public const string BackPayOT = "BACKPAYOT";
        public const string SickHrly = "SICK-HRLY";
    }



   









   
}
















