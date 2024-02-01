using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using System.Text;



public partial class deneme : System.Web.UI.Page
{

   

    protected void Page_Load(object sender, EventArgs e)
    {
      //  ListBox1.Items.Clear();
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  ssn, ssn + ' ' + firstName + ' ' +  lastName + ' ' + ClientID    FROM[Bridge].[dbo].[NewHireMaintEmployee]", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {               
                while (reader.Read())
                {
                    ListItem itm = new ListItem(reader.GetString(1));
                    if (!ListBox1.Items.Contains(itm))
                    {
                        ListBox1.Items.Add(reader.GetString(1));                       
                    }
                }
            }
            dbConnection.Close();
        }
        LabelUserName.Text = Request.LogonUserIdentity.Name;
                             
    }
    protected void Button1_Click(object sender, EventArgs e)
    {        
        TextBox1.Text = "birgun burda ";
    }




    // LIST BOX CHANGE ********************************************************
    protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //  TextBox1.Text = ListBox1.Text.ToString();
        TextBox1.Text = ListBox1.SelectedItem.Text.ToString();
        TxtFirstName.Text = ListBox1.SelectedItem.Text.Substring(0, 11).ToString();

          ListBox2.Items.Clear();
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT [ErrorMsg]  FROM[Bridge].[dbo].[NewHireMaintErrors] " +
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "' AND " +
                " ClientID = '" + ListBox1.SelectedItem.Text.Substring(ListBox1.SelectedItem.Text.Length - 6, 6).ToString() + "'", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ListItem itm = new ListItem(reader.GetString(0));
                    if (!ListBox2.Items.Contains(itm))
                    {
                        ListBox2.Items.Add(reader.GetString(0));
                    }
                }
            }
            dbConnection.Close();
        }

                
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT ClientID, ssn, firstName, lastName, birthDate, ethnicCode, " +
            "maritalStatus, addressLine1, addressLine2, zipCode, city, stateCode, HomePhone, gender, employeeTypeCode," +
            "workLocationCode, jobCode, origHireDate, lastHireDate, peoStartDate, payMethod, payRate, fedFileStatus, " +
            "benefitsGroup, payGroup, payPeriod,mobilePhone, emailAddress, citizenStatus, ID, HireType, defaultDeptCode, " +
            "standardHours FROM[Bridge].[dbo].[NewHireMaintEmployee] " +
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "' AND " +
                 " ClientID = '" + ListBox1.SelectedItem.Text.Substring(ListBox1.SelectedItem.Text.Length - 6, 6).ToString() + "'", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TextBox2.Text = reader.GetString(0);
                    TxtFirstName.Text = reader.GetString(1);                   
                    TextBox3.Text = reader.GetString(2);
                    TextBox4.Text = reader.GetString(3);
                    TextBox5.Text = reader.GetString(4);
                    TextBox6.Text = reader.GetString(5);
                    TextBox7.Text = reader.GetString(6);
                    TextBox8.Text = reader.GetString(7);
                    TextBox9.Text = reader.GetString(8);
                    TextBox10.Text = reader.GetString(9);
                    TextBox11.Text = reader.GetString(10);
                    TextBox12.Text = reader.GetString(11);
                    TextBox13.Text = reader.GetString(12);
                    TextBox14.Text = reader.GetString(13);
                    TextBox15.Text = reader.GetString(14);
                    TextBox16.Text = reader.GetString(15);
                    TextBox17.Text = reader.GetString(16);
                    TextBox18.Text = reader.GetString(17);
                    TextBox19.Text = reader.GetString(18);
                    TextBox20.Text = reader.GetString(19);
                    TextBox21.Text = reader.GetString(20);
                    TextBox22.Text = reader.GetString(21);
                    TextBox23.Text = reader.GetString(22);
                    TextBox24.Text = reader.GetString(23);
                    TextBox25.Text = reader.GetString(24);
                    TextBox26.Text = reader.GetString(25);
                    TextBoxMobilePhone.Text = reader.GetString(26);
                    TextBox28.Text = reader.GetString(27);
                    TextBox29.Text = reader.GetString(28);                   
                    TextBox30.Text = reader.GetInt32(29).ToString();
                    Label31.Text = reader.GetString(30);
                    TextBox31.Text = reader.GetString(31);
                    TextBox32.Text = reader.GetString(32);
                }
            }
            dbConnection.Close();
        }
    }
    //***********************************************************************************
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
       
    }
    // =========================SAVE BUTTON ======================================================
    protected void Button1_Click1(object sender, EventArgs e)
    {

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintEmployee]   " +
                "SET ssn = '" + TxtFirstName.Text + "' " +
                ",ClientID          = '" + TextBox2.Text + "' " +
                ",firstName         = '" + TextBox3.Text + "' " +
                ",lastName          = '" + TextBox4.Text + "' " +
                ",birthDate         = '" + TextBox5.Text + "' " +
                ",ethnicCode        = '" + TextBox6.Text + "' " +
                ",maritalStatus     = '" + TextBox7.Text + "' " +
                ",addressLine1      = '" + TextBox8.Text + "' " +
                ",addressLine2      = '" + TextBox9.Text + "' " +
                ",zipCode           = '" + TextBox10.Text + "' " +
                ",city              = '" + TextBox11.Text + "' " +
                ",stateCode         = '" + TextBox12.Text + "' " +
                ",homePhone         = '" + TextBox13.Text + "' " +
                ",gender            = '" + TextBox14.Text + "' " +
                ",employeeTypeCode = '" + TextBox15.Text + "' " +
                ",worklocationCode = '" + TextBox16.Text + "' " +
                ",jobCode           = '" + TextBox17.Text + "' " +
                ",origHireDate      = '" + TextBox18.Text + "' " +
                ",lastHireDate      = '" + TextBox19.Text + "' " +              
                ",peoStartDate      = '" + TextBox20.Text + "' " +              
                ",payMethod         = '" + TextBox21.Text + "' " +
                ",payRate           = '" + TextBox22.Text + "' " +
                ",fedFileStatus     = '" + TextBox23.Text + "' " +                
                ",benefitsGroup     = '" + TextBox24.Text + "' " +   
                ",payGroup          = '" + TextBox25.Text + "' " +
                ",payPeriod         = '" + TextBox26.Text + "' " +
                ",mobilePhone       = '" + TextBoxMobilePhone.Text + "' " +
                ",emailAddress      = '" + TextBox28.Text + "' " +
                ",defaultDeptCode   = '" + TextBox31.Text + "' " +
                ",standardHours     = '" + TextBox32.Text + "' " +
                ",FixFlag ='Fixed'" + 
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "'  AND " +
                 " ClientID = '" + ListBox1.SelectedItem.Text.Substring(ListBox1.SelectedItem.Text.Length - 6, 6).ToString() + "'", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            dbConnection.Close();
        }        
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintErrors]   " +
                "SET ssn = '" + TxtFirstName.Text + "' " +
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "'", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            dbConnection.Close();
        }
        Response.Redirect(Request.Url.ToString());
    }





    protected void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
    {

       // TextBox13.Text = ListBox2.SelectedValue
        if (ListBox2.SelectedValue.Contains("PAY GROUP"))
        { TextBox25.BackColor = System.Drawing.ColorTranslator.FromHtml("#f5f542"); }
        else
        { TextBox25.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff"); }

        if (ListBox2.SelectedValue.Contains("filing status"))
        { TextBox23.BackColor = System.Drawing.ColorTranslator.FromHtml("#f5f542"); }
        else
        { TextBox23.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff"); }

        if (ListBox2.SelectedValue.Contains("benefit group"))
        { TextBox24.BackColor = System.Drawing.ColorTranslator.FromHtml("#f5f542"); }
        else
        { TextBox24.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff"); }

        if (ListBox2.SelectedValue.Contains("LOCATION CODE"))
        { TextBox16.BackColor = System.Drawing.ColorTranslator.FromHtml("#f5f542"); }
        else
        { TextBox16.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff"); }

        if (ListBox2.SelectedValue.Contains("JOB CODE"))
        { TextBox17.BackColor = System.Drawing.ColorTranslator.FromHtml("#f5f542"); }
        else
        { TextBox17.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff"); }





    }
  


    public string GetsessionId()
    {
        string uriString = "https://api.prismhr.com/api-1.25/services/rest/login/createPeoSession";
        WebClient myWebClient = new WebClient();
        NameValueCollection myNameValueCollection = new NameValueCollection();
        string username = "apiLIVE";
        string password = "ThisIs13UGze";
        string peoId = "545";
        myNameValueCollection.Add("username", username);
        myNameValueCollection.Add("password", password);
        myNameValueCollection.Add("peoId", peoId);

        byte[] responseArray = myWebClient.UploadValues(uriString, myNameValueCollection);

        string rawst = Encoding.ASCII.GetString(responseArray);
        string sess = rawst.Substring(14, 25);
        return sess;

    }



        public string ErrorMessageProcess(String Apitext, String Client, String ssnno, int tableid)
    {
            string ErrMes = "";
            int gg;
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];


        if (Apitext.IndexOf("Check updateMessage") > 0 || Apitext.IndexOf("see failure messages") > 0)
        {
            if (Apitext.IndexOf("updateMessage") > 0)
                    {
                         gg = Apitext.IndexOf("updateMessage");
                    }
            else if (Apitext.IndexOf("failureMessage") > 0)
                    {
                         gg = Apitext.IndexOf("failureMessage");
                    }
            else
                {
                     gg = 1;
                }

            string mes1 = Apitext.Substring(gg);
            int ilkparant = mes1.IndexOf("[");
            int sonparant = mes1.IndexOf("]");
            string finalmes = mes1.Substring(ilkparant + 1, sonparant - ilkparant - 1);
            finalmes = finalmes.Replace("\"", "");
            TextBox14.Text = gg.ToString();

            if (finalmes.IndexOf("is already assigned to") > 0)
            {
                ErrMes = "";
            }
            else
            {
                ErrMes = finalmes;
            }
        }
        else
        {
            int errno = Apitext.IndexOf("errorMessage");
            string mes1 = Apitext.Substring(errno);
            int ilkparant = mes1.IndexOf(":");
            int sonparant = mes1.IndexOf("extension");
            string finalmes = mes1.Substring(ilkparant + 1, sonparant - ilkparant - 3);
            finalmes = finalmes.Replace("\"", "");
            ErrMes = finalmes;
        }
        // return ErrMes;
        if (ErrMes != "")
        {
            using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {

                dbConnection.Open();
                SqlCommand cmd2 = new SqlCommand("INSERT INTO [Bridge].[dbo].[NewHireMaintErrors]   " +
                    " (ssn, ErrorMsg, EmpKeyID, ClientID, ErrorType) " +
                    " VALUES ( '" + ssnno + "', '" + ErrMes + "', '" + tableid + "', '" + Client + "', 'EMP')" +
                    "   ", dbConnection);

                SqlDataReader reader2 = cmd2.ExecuteReader();
                dbConnection.Close();
            }
        }
        return ErrMes;

    }

    public string DeleteErrMsg(String EEssn)
    {
        string res = "";
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {

            dbConnection.Open();
            SqlCommand cmd2 = new SqlCommand(" DELETE FROM  [Bridge].[dbo].[NewHireMaintErrors] " +
                "WHERE ssn ='"+ EEssn + "' AND ErrorType = 'EMP'  ", dbConnection);

            SqlDataReader reader2 = cmd2.ExecuteReader();
            dbConnection.Close();

            dbConnection.Open();
            SqlCommand cmd4 = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintEmployee]   " +
                "SET FixFlag = ''     Where ssn =  '" + EEssn + "' ", dbConnection);
            SqlDataReader delreader1 = cmd4.ExecuteReader();
            dbConnection.Close();
        }
        return res;
    }






    //****************************************************SUBMIT button**********************************
    protected void Button2_Click(object sender, EventArgs e)
    {

        //Check to Table for fixed sign
       // TextBox8.Text = GetsessionId();
        string batchid = "";
        string errorlist = "";
        string durum = "";
        String Sid = "";
        string sessionID = "";
        string eeid = "";

        //******************************************----NEWHIRE------*************************
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  TOP(1) ClientID, firstName, lastName, middleInitial,birthDate,employerId," +
                " ethnicCode, maritalStatus, addressLine1, addressLine2, zipCode,  city, stateCode, homePhone, gender, ssn," +
                " employeeStatusCode, employeeTypeCode, workLocationCode, jobCode, origHireDate, lastHireDate, peoStartDate, " +
                "payMethod, payRate, standardHours, fedFileStatus, emailAddress, benefitsGroup, payGroup, payPeriod," +
                "mobilePhone, citizenStatus, ID, HireType,  defaultDeptCode, workGroupCode  FROM[Bridge].[dbo].[NewHireMaintEmployee] " +
                " Where FixFlag = 'Fixed' AND HireType = 'NewHire' ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int ID = reader.GetInt32(33);
                    Sid = String.Format("{0:000000}", reader.GetString(0));
                   // TextBox13.Text = Sid;
                    sessionID = GetsessionId();
                    string EEfirstName = reader.GetString(1);
                    string EElastName = reader.GetString(2);
                    string EEssn = reader.GetString(15);
                    string sBaseUrl = "";

                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/importEmployees";

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + sessionID + "\", \n" +
                         "\"clientId\": \"" + reader.GetString(0) + "\", " +
                         "\n \"newHireEmployee\": [\n{\n" +
                        "\"firstName\": \""+ reader.GetString(1) + "\", " +
                        "\n\"lastName\": \"" + reader.GetString(2) + "\"," +
                        "\n\"middleInitial\": \"" + reader.GetString(3) + "\"," +
                        "\n\"birthDate\": \"" + reader.GetString(4) + "\"," +
                        "\n\"employerId\": \"" + reader.GetString(5) + "\"," +
                        "\n\"ethnicCode\": \"" + reader.GetString(6) + "\"," +
                        "\n\"maritalStatus\": \"" + reader.GetString(7) + "\"," +
                        "\n\"addressLine1\": \"" + reader.GetString(8) + "\"," +
                        "\n\"addressLine2\": \"" + reader.GetString(9) + "\"," +
                        "\n\"zipCode\": \"" + reader.GetString(10) + "\"," +
                        "\n\"city\": \"" + reader.GetString(11) + "\"," +
                        "\n\"stateCode\": \"" + reader.GetString(12) + "\"," +
                         "\n\"homePhone\": \"" + reader.GetString(13) + "\"," +
                        "\n\"gender\": \"" + reader.GetString(14) + "\"," +
                        "\n\"ssn\": \"" + reader.GetString(15) + "\"," +
                        "\n\"employeeStatusCode\": \"" + reader.GetString(16) + "\"," +
                        "\n\"employeeTypeCode\": \"" + reader.GetString(17) + "\"," +
                        "\n\"workLocationCode\": \"" + reader.GetString(18) + "\"," +
                        "\n\"jobCode\": \"" + reader.GetString(19) + "\", " +
                        "\n\"origHireDate\": \"" + reader.GetString(20) + "\"," +
                        "\n\"lastHireDate\": \"" + reader.GetString(21) + "\"," +
                        "\n\"peoStartDate\": \"" + reader.GetString(22) + "\"," +
                        "\n\"payMethod\": \"" + reader.GetString(23) + "\"," +
                        "\n\"payRate\": \"" + reader.GetString(24) + "\", " +
                        "\n\"standardHours\": \"" + reader.GetString(25) + "\"," +
                        "\n \"fedFileStatus\": \"" + reader.GetString(26) + "\"," +
                        "\n \"emailAddress\": \"" + reader.GetString(27) + "\"," +
                        "\n\"benefitsGroup\": \"" + reader.GetString(28) + "\"," +
                          "\n\"payGroup\": \"" + reader.GetString(29) + "\"," +
                        "\n\"payPeriod\": \"" + reader.GetString(30) + "\", " +                      
                         "\n\"mobilePhone\": \"" + reader.GetString(31) + "\"," +
                         "\n\"defaultDeptCode\": \"" + reader.GetString(35) + "\"," +
                          "\n\"workGroupCode\": \"" + reader.GetString(36) + "\"," +
                        " \n \"citizenStatus\": \"" + reader.GetString(32) + "\"\n}\n]\n} ";

                      //  string folder = @"C:\test\";
                      //  string fileName = "apiCall.txt";
                      //  string fullPath = folder + fileName;
                    //    File.WriteAllText(@"C:\test\apicall.txt", json);
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                         Textdene.Text = result;
                     //   File.WriteAllText(@"C:\test\result.txt", result);

                        //if is Error
                        string lookerror = @"""importError"":null";
                        if (result.IndexOf(lookerror) != -1)
                        {  // NO ERROR
                            //Get batchId
                            int b = result.IndexOf("importBatchId");
                            string btID = result.Substring(b + 16, 7).Replace("\"", "").Replace(":", "").Replace(",", "");
                            TextBoxMessage.Text = "NOError" + " " + btID;

                            //Clean ERROR Table update fixflag
                            sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/commitEmployees?sessionId=" + sessionID + "&clientId=" + reader.GetString(0) + " &importBatchId=" + btID + "";

                            // TextBox28.Text = sBaseUrl;
                                   HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                                   request.Method = "POST";
                                   request.UnsafeAuthenticatedConnectionSharing = true;
                                   HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                   StreamReader reader2 = new StreamReader(response.GetResponseStream());
                                   string rawst = reader2.ReadToEnd();
                             Textdene.Text = rawst;

                              connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
                            using (SqlConnection dbConnectionf = new SqlConnection(connfromWebconfig.ConnectionString))
                            {
                               /*
                                dbConnectionf.Open();
                                SqlCommand cmdf = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintEmployee]   " +
                                   "Where ssn =  '" + EEssn + "' ", dbConnectionf);
                                SqlDataReader readerf = cmdf.ExecuteReader();
                                dbConnectionf.Close();
                               */
                            

                                dbConnectionf.Open();
                                SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                    "Where ssn =  '" + EEssn + "' ", dbConnectionf);
                                SqlDataReader readerfi = cmd2.ExecuteReader();
                                dbConnectionf.Close();

                            }

                            sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/lookupBySsn?sessionId=" + sessionID + "&ssn=" + EEssn + "";

                             request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                            request.Method = "POST";
                            request.UnsafeAuthenticatedConnectionSharing = true;
                             response = (HttpWebResponse)request.GetResponse();
                             reader2 = new StreamReader(response.GetResponseStream());
                            string EEresult = reader2.ReadToEnd();
                            int s = EEresult.IndexOf("employeeId");
                            String EmpID = EEresult.Substring(s + 13, 6).Replace("\"", "");


                            //HISTORY TABLE ADD
                            using (SqlConnection upConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                                {
                                upConnection.Open();
                                SqlCommand cmd3 = new SqlCommand("INSERT INTO [Bridge].[dbo].[NewHireHistory] " +
                                    "(firstName, lastName, ssn, employeeID, HireType, ProcessDate, ClientID, RecType )  " +
                                    "VALUES ('" + EEfirstName + "' , '" + EElastName + "', '" + EEssn + "',  '" + EmpID + "' , 'NewHire', GETDATE(),'" + Sid + "', 'EMP') ", upConnection);
                                SqlDataReader delreader = cmd3.ExecuteReader();
                                upConnection.Close();
                            }

                        }
                        else //if  Error
                        {
                            int b = result.IndexOf("importBatchId");
                            batchid = result.Substring(b + 16, 7).Replace("\"", "").Replace(":", "").Replace(",", "");
                            TextBoxMessage.Text = "Error" + " " + batchid;

                           // ERROR SHOW CANCEL IMPORTING 
                                        sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/cancelImport?sessionId=" + sessionID + "&clientId=" + reader.GetString(0) + " &importBatchId=" + batchid + "";

                             //TextBox28.Text = sBaseUrl;
                                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                                        request.Method = "POST";
                                        request.UnsafeAuthenticatedConnectionSharing = true;
                                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                        StreamReader reader2 = new StreamReader(response.GetResponseStream());
                                        string rawst = reader2.ReadToEnd();
                            //Textdene.Text = rawst;

                            using (SqlConnection delConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                            {                                                
                                    delConnection.Open();
                                 SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                 "Where ssn =  '" + reader.GetString(15) + "' ", delConnection);
                                  SqlDataReader delreader = cmd2.ExecuteReader();
                                  delConnection.Close();

                                 delConnection.Open();
                                 SqlCommand cmd4 = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintEmployee]   " +
                                "SET FixFlag = ''     Where ssn =  '" + reader.GetString(15) + "' ", delConnection);
                                SqlDataReader delreader1 = cmd4.ExecuteReader();
                                delConnection.Close();  
                            }

                                int ie = result.IndexOf("importError");
                            int ih = result.IndexOf("importedHire");
                           string Errbox = result.Substring(ie + 15, ih - ie-19 ).Replace("\",\"", "|").Replace("'","");
                            Textdene.Text = Errbox;


                            string[] wrdList = Errbox.Split('|');
                            foreach (string wrd in wrdList)
                            {
                                //ListBox2.Items.Add(wrd);
                                using (SqlConnection upConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                                {
                                    upConnection.Open();
                                    SqlCommand cmd3 = new SqlCommand("INSERT INTO [Bridge].[dbo].[NewHireMaintErrors] " +
                                        "(ssn, ErrorMsg, ProcessDate, EmpKeyID, ClientID, ErrorType )  " +
                                        "VALUES ('" + reader.GetString(15) + "' , '" + wrd + "' , GETDATE(), "+ ID +", '"+ Sid +"', 'EMP' ) ", upConnection);
                                    Textdene.Text = cmd3.ToString();
                                    SqlDataReader delreader = cmd3.ExecuteReader();
                                    upConnection.Close();
                                }
                            }
                        }
                     

                    }  
            

                }                               
            }
            dbConnection.Close();

           // Response.Redirect(Request.Url.ToString());
        }
                    

        //****************************************---- REHIRE ---- ***********************

         connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            string ssn = "";
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  TOP(1) ClientID, firstName, lastName, middleInitial,birthDate,employerId," +
                " ethnicCode, maritalStatus, addressLine1, addressLine2, zipCode,  city, stateCode, homePhone, gender, ssn," +
                " employeeStatusCode, employeeTypeCode, workLocationCode, jobCode, origHireDate, lastHireDate, peoStartDate, " +
                "payMethod, payRate, standardHours, fedFileStatus, emailAddress, benefitsGroup, payGroup, payPeriod," +
                "mobilePhone, citizenStatus, ID  FROM[Bridge].[dbo].[NewHireMaintEmployee] " +
                " Where FixFlag = 'Fixed' AND HireType = 'ReHire' ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int ID = reader.GetInt32(33);
                    Sid = String.Format("{0:000000}", reader.GetString(0));
                    //TextBox13.Text = Sid;
                    sessionID = GetsessionId();
                    //TextBox8.Text = sessionID;
                    ssn = reader.GetString(15);
                    int dataID = reader.GetInt32(33);

                    String abc = DeleteErrMsg(ssn);
                    //TextBox9.Text = reader.GetString(15);

                    String tdate = DateTime.Now.ToString("yyyy-MM-dd");
                    // TextBox18.Text = tdate;

                    string sBaseUrl = "";
                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/lookupBySsn?sessionId=" + sessionID + "&ssn=" + reader.GetString(15) + "";

                    // TextBox28.Text = sBaseUrl;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    request.Method = "POST";
                    request.UnsafeAuthenticatedConnectionSharing = true;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader2 = new StreamReader(response.GetResponseStream());
                    string rawst = reader2.ReadToEnd();

                    int i = rawst.IndexOf("employeeId");
                    eeid = rawst.Substring(i + 13, 6);

                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/rehireEmployee";

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + sessionID + "\", \n" +
                       "\"clientId\": \"" + Sid + "\", " +
                      "\"employeeId\": \"" + eeid + "\", " +
                      "\n\"status\": \"A\"," +
                      "\n\"type\": \"" + reader.GetString(17) + "\"," +
                      "\n\"rehireDate\": \"" + tdate + "\"," +
                      "\n\"reason\": \"OTH\"  \n} ";

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();

                        TextBoxMessage.Text = ErrorMessageProcess(result, Sid, ssn, dataID);
                        //String Apitext, String Client, String ssnno, int tableid

                    }

                    //TextBox18.Text = eeid;


                    // get PERSONEL INFO FOR CHECKSUM - UPDATE PERSONEL INFO
                    string resultP = "";
                    //HTTP GET
                    string pURL = "https://api.prismhr.com/api-1.25/services/rest/employee/getEmployee?sessionId=" + sessionID + "&employeeId=" + eeid + "&clientId=" + Sid + "";

                    HttpWebRequest requestP = (HttpWebRequest)WebRequest.Create(pURL);
                    requestP.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    using (HttpWebResponse responseP = (HttpWebResponse)requestP.GetResponse())
                    using (Stream stream = responseP.GetResponseStream())
                    using (StreamReader readerP = new StreamReader(stream))
                    {
                        resultP = readerP.ReadToEnd();
                    }
                    int s = resultP.IndexOf("checksum");
                    string chksum = resultP.Substring(s + 11, 7);


                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/updatePersonalInfo";
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + sessionID + "\", \n" +
                        "\"clientId\": \"" + Sid + "\", " +
                       "\n\"employeeId\": \"" + eeid + "\", " +
                       "\n\"firstName\": \"" + reader.GetString(1) + "\"," +
                       "\n\"lastName\": \"" + reader.GetString(2) + "\"," +
                       "\n\"birthDate\": \"" + reader.GetString(4) + "\"," +
                       "\n\"gender\": \"" + reader.GetString(14) + "\"," +
                       "\n\"ethnicCode\": \"" + reader.GetString(6) + "\"," +
                       "\n\"maritalStatus\": \"" + reader.GetString(7) + "\"," +
                       "\n\"homePhone\": \"" + reader.GetString(13) + "\"," +
                       "\n\"mobilePhone\": \"" + reader.GetString(31) + "\"," +
                       "\n\"emailAddress\": \"" + reader.GetString(27) + "\"," +
                        "\n\"checksum\": \"" + chksum + "\"  \n} ";


                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        // Textdene.Text = result;
                        TextBoxMessage.Text = ErrorMessageProcess(result, Sid, ssn, dataID);
                    }


                    // get ADDRESS INFO UPDATE =========================================

                    //HTTP GET
                    pURL = "https://api.prismhr.com/api-1.25/services/rest/employee/getAddressInfo?sessionId=" + sessionID + "&clientId=" + Sid + "&employeeId=" + eeid + "";

                    HttpWebRequest requestA = (HttpWebRequest)WebRequest.Create(pURL);
                    requestA.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    using (HttpWebResponse responseA = (HttpWebResponse)requestA.GetResponse())
                    using (Stream stream = responseA.GetResponseStream())
                    using (StreamReader readerA = new StreamReader(stream))
                    {
                        resultP = readerA.ReadToEnd();
                    }

                    s = resultP.IndexOf("checksum");
                    chksum = resultP.Substring(s + 11, 7).Replace("\"", "");


                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/updateAddressInfo";
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"checksum\":\"" + chksum + "\", \n" +
                       "\"homeAddress\": {" +
                      "\n\"addressLine1\": \"" + reader.GetString(8) + "\", " +
                      "\n\"addressLine2\": \"" + reader.GetString(9) + "\"," +
                      "\n\"city\": \"" + reader.GetString(11) + "\"," +
                      "\n\"stateAbbr\": \"" + reader.GetString(12) + "\"," +
                      "\n\"zipCode\": \"" + reader.GetString(10) + "\"" +
                      "\n }, " +
                      "\n\"sessionId\": \"" + sessionID + "\"," +
                      "\n\"clientId\": \"" + Sid + "\"," +
                       "\n\"employeeId\": \"" + eeid + "\"  \n} ";

                        // Textdene.Text = json;
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        TextBoxMessage.Text = ErrorMessageProcess(result, Sid, ssn, dataID);
                    }


                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/updatePayRate";
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + sessionID + "\"," +
                      "\n\"clientId\": \"" + Sid + "\"," +
                       "\n\"employeeId\": \"" + eeid + "\"," +
                       "\n\"effectiveDate\": \"" + tdate + "\"," +
                      "\n\"reasonCode\": \"OTH\"," +
                      "\n\"payRate\": \"" + reader.GetString(24) + "\"," +
                      "\n\"payPeriod\": \"" + reader.GetString(30) + "\"," +
                      "\n\"standardHours\": \"" + reader.GetString(25) + "\"" +
                      "\n }";

                        // Textdene.Text = json;
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        TextBoxMessage.Text = ErrorMessageProcess(result, Sid, ssn, dataID);

                    }

                    sBaseUrl = "";
                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/updatePayGroup?sessionId=" + sessionID + "&clientId=" + Sid + "&employeeId=" + eeid + "&payGroup=" + reader.GetString(29) + "";

                    // TextBox28.Text = sBaseUrl;
                    HttpWebRequest requestpg = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    requestpg.Method = "POST";
                    requestpg.UnsafeAuthenticatedConnectionSharing = true;
                    HttpWebResponse responsepg = (HttpWebResponse)requestpg.GetResponse();
                    StreamReader readerpg = new StreamReader(responsepg.GetResponseStream());
                    string resultpg = readerpg.ReadToEnd();


                    TextBoxMessage.Text = ErrorMessageProcess(resultpg, Sid, ssn, dataID);


                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/updateJobCode";
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + sessionID + "\"," +
                      "\n\"clientId\": \"" + Sid + "\"," +
                       "\n\"employeeId\": \"" + eeid + "\"," +
                       "\n\"jobCode\": \"" + reader.GetString(19) + "\"," +
                       "\n\"reasonCode\": \"TRANS\"," +
                       "\n\"effectiveDate\": \"" + tdate + "\"," +
                        "\n\"reviewDate\": \"" + tdate + "\"" +
                      "\n }";

                        // Textdene.Text = json;
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        // Textdene.Text = result;
                        TextBoxMessage.Text = ErrorMessageProcess(result, Sid, ssn, dataID);
                    }

                    //Method ====GET===== Checksum FOR UpdateAssignment Homelocation Benefit Group
                    resultP = "";
                    //HTTP GET
                    pURL = "https://api.prismhr.com/api-1.25/services/rest/employee/getEmployee?sessionId=" + sessionID + "&employeeId=" + eeid + "&clientId=" + Sid + "&options=Client";

                    HttpWebRequest requestC = (HttpWebRequest)WebRequest.Create(pURL);
                    requestC.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    using (HttpWebResponse responseC = (HttpWebResponse)requestC.GetResponse())
                    using (Stream stream = responseC.GetResponseStream())
                    using (StreamReader readerC = new StreamReader(stream))
                    {
                        resultP = readerC.ReadToEnd();
                    }
                    s = resultP.IndexOf("checksum");
                    chksum = resultP.Substring(s + 11, 9).Replace("\"", "");
                    // Textdene.Text = resultP;
                    // TextBox19.Text = chksum;
                    //TextBox20.Text = reader.GetString(16);

                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/updateAssignment";
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"homeLocation\":\"" + reader.GetString(18) + "\"," +
                                    "\n\"benefitGroup\": \"" + reader.GetString(28) + "\"," +
                                    "\n\"sessionId\": \"" + sessionID + "\"," +
                      "\n\"clientId\": \"" + Sid + "\"," +
                       "\n\"employeeId\": \"" + eeid + "\"," +
                        "\n\"checksum\": \"" + chksum + "\"" +
                      "\n }";

                        // Textdene.Text = json;
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        Textdene.Text = result;
                        TextBoxMessage.Text = ErrorMessageProcess(result, Sid, ssn, dataID);
                    }
                }
            }
            dbConnection.Close();

            using (SqlConnection upConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                upConnection.Open();
                SqlCommand cmd3 = new SqlCommand("SELECT ID, ssn, ErrorMsg, EmpKeyID, ClientID  FROM [Bridge].[dbo].[NewHireMaintErrors] " +
                    "WHERE ssn = '"+ ssn + "' AND ErrorType = 'EMP' ", upConnection);
                SqlDataReader delreader = cmd3.ExecuteReader();
                if (delreader.HasRows)
                {

                }
                    upConnection.Close();
            }
            //Check ERROR TABLE for ANY ERROR 

        }

            //***************************************---- XHIRE ************************

            connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  TOP(1) ClientID, firstName, lastName, middleInitial,birthDate,employerId," +
                " ethnicCode, maritalStatus, addressLine1, addressLine2, zipCode,  city, stateCode, homePhone, gender, ssn," +
                " employeeStatusCode, employeeTypeCode, workLocationCode, jobCode, origHireDate, lastHireDate, peoStartDate, " +
                "payMethod, payRate, standardHours, fedFileStatus, emailAddress, benefitsGroup, payGroup, payPeriod," +
                "mobilePhone, citizenStatus, ID, HireType, defaultDeptCode  FROM[Bridge].[dbo].[NewHireMaintEmployee] " +
                " Where FixFlag = 'Fixed' AND HireType = 'CrossHire' ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int ID = reader.GetInt32(33);
                    Sid = String.Format("{0:000000}", reader.GetString(0));
                    string EmpID = "";
                    // TextBox13.Text = Sid;
                    sessionID = GetsessionId();
                    string EEfirstName = reader.GetString(1);
                    string EElastName = reader.GetString(2);
                    string EEssn = reader.GetString(15);
                    string sBaseUrl = "";

                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/lookupBySsn?sessionId=" + sessionID + "&ssn=" + EEssn+"";

                     HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    request.Method = "POST";
                    request.UnsafeAuthenticatedConnectionSharing = true;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader2 = new StreamReader(response.GetResponseStream());
                    string EEresult = reader2.ReadToEnd();
                    int s = EEresult.IndexOf("employeeId");
                    EmpID = EEresult.Substring(s + 13, 6).Replace("\"", "");

                    //TextBoxMessage.Text = Sid;
                  //  Textdene.Text = EmpID;
                   // TextBoxMessage.Text = ErrorMessageProcess(EEresult, Sid, ssn, ID);                                                                        
                    

                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/importEmployeesAllowingCrossHire";

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + sessionID + "\", \n" +
                         "\"clientId\": \"" + reader.GetString(0) + "\", " +
                         "\n \"newHireEmployee\": [\n{\n" +
                        "\"firstName\": \"" + reader.GetString(1) + "\", " +
                        "\n\"lastName\": \"" + reader.GetString(2) + "\"," +
                        "\n\"middleInitial\": \"" + reader.GetString(3) + "\"," +
                        "\n\"birthDate\": \"" + reader.GetString(4) + "\"," +
                        "\n\"employerId\": \"" + reader.GetString(5) + "\"," +
                        "\n\"ethnicCode\": \"" + reader.GetString(6) + "\"," +
                        "\n\"maritalStatus\": \"" + reader.GetString(7) + "\"," +
                        "\n\"addressLine1\": \"" + reader.GetString(8) + "\"," +
                        "\n\"addressLine2\": \"" + reader.GetString(9) + "\"," +
                        "\n\"zipCode\": \"" + reader.GetString(10) + "\"," +
                        "\n\"city\": \"" + reader.GetString(11) + "\"," +
                        "\n\"stateCode\": \"" + reader.GetString(12) + "\"," +
                         "\n\"homePhone\": \"" + reader.GetString(13) + "\"," +
                        "\n\"gender\": \"" + reader.GetString(14) + "\"," +
                        "\n\"ssn\": \"" + reader.GetString(15) + "\"," +
                        "\n\"employeeStatusCode\": \"" + reader.GetString(16) + "\"," +
                        "\n\"employeeTypeCode\": \"" + reader.GetString(17) + "\"," +
                        "\n\"workLocationCode\": \"" + reader.GetString(18) + "\"," +
                        "\n\"jobCode\": \"" + reader.GetString(19) + "\", " +
                        "\n\"origHireDate\": \"" + reader.GetString(20) + "\"," +
                        "\n\"lastHireDate\": \"" + reader.GetString(21) + "\"," +
                        "\n\"peoStartDate\": \"" + reader.GetString(22) + "\"," +
                        "\n\"payMethod\": \"" + reader.GetString(23) + "\"," +
                        "\n\"payRate\": \"" + reader.GetString(24) + "\", " +
                        "\n\"standardHours\": \"" + reader.GetString(25) + "\"," +
                        "\n \"fedFileStatus\": \"" + reader.GetString(26) + "\"," +
                        "\n \"emailAddress\": \"" + reader.GetString(27) + "\"," +
                        "\n\"benefitsGroup\": \"" + reader.GetString(28) + "\"," +
                          "\n\"payGroup\": \"" + reader.GetString(29) + "\"," +
                        "\n\"payPeriod\": \"" + reader.GetString(30) + "\", " +
                         "\n\"mobilePhone\": \"" + reader.GetString(31) + "\"," +
                          "\n\"crossCompanyEid\": \"" + EmpID + "\"," +
                           "\n\"defaultDeptCode\": \"" + reader.GetString(35) + "\"," +
                        " \n \"citizenStatus\": \"" + reader.GetString(32) + "\"\n}\n]\n} ";
                                              

                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        // Textdene.Text = result;
                        // TextBoxMessage.Text = EmpID;
                        // TextBoxMessage.Text = ErrorMessageProcess(result, Sid, EEssn, ID);
                       // if (result.IndexOf("importError") == -1)
                            string lookerror = @"""importError"":null";
                        if (result.IndexOf(lookerror) != -1)

                        {  // NO ERROR
                            //Get batchId
                            int b = result.IndexOf("importBatchId");
                            string btID = result.Substring(b + 16, 6).Replace("\"", "").Replace(":", "").Replace(",", "");
                            TextBoxMessage.Text = "NOError" + " " + btID;

                            sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/commitEmployees?sessionId=" + sessionID + "&clientId=" + Sid + " &importBatchId=" + btID + "";
                             request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                            request.Method = "POST";
                            request.UnsafeAuthenticatedConnectionSharing = true;
                            response = (HttpWebResponse)request.GetResponse();
                             reader2 = new StreamReader(response.GetResponseStream());
                            string rawst = reader2.ReadToEnd();
                            Textdene.Text = rawst;

                            connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
                            using (SqlConnection dbConnectionf = new SqlConnection(connfromWebconfig.ConnectionString))
                            {
                                dbConnectionf.Open();
                                SqlCommand cmdf = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintEmployee]   " +
                                   "Where ssn =  '" + EEssn + "' ", dbConnectionf);
                                SqlDataReader readerf = cmdf.ExecuteReader();
                                dbConnectionf.Close();

                                dbConnectionf.Open();
                                SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                    "Where ssn =  '" + EEssn + "' ", dbConnectionf);
                                SqlDataReader readerfi = cmd2.ExecuteReader();
                                dbConnectionf.Close();
                            }

                            //HISTORY TABLE ADD
                            using (SqlConnection upConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                            {
                                upConnection.Open();
                                SqlCommand cmd3 = new SqlCommand("INSERT INTO [Bridge].[dbo].[NewHireHistory] " +
                                    "(firstName, lastName, ssn, employeeID, HireType, ProcessDate, ClientID, RecType )  " +
                                    "VALUES ('" + EEfirstName + "' , '" + EElastName + "', '" + EEssn + "',  '" + EmpID + "' , 'CrossHire', GETDATE(),'" + Sid + "', 'EMP') ", upConnection);
                                SqlDataReader delreader = cmd3.ExecuteReader();
                                upConnection.Close();
                            }
                        }
                        else                         //IF ERROR HAPPENED
                        {
                            int b = result.IndexOf("importBatchId");
                            batchid = result.Substring(b + 16, 6).Replace("\"", "").Replace(":", "").Replace(",", "");
                            TextBoxMessage.Text = "Error" + " " + batchid;

                            // ERROR SHOW CANCEL IMPORTING 
                            sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/cancelImport?sessionId=" + sessionID + "&clientId=" + Sid + " &importBatchId=" + batchid + "";
                            //TextBox28.Text = sBaseUrl;
                             request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                            request.Method = "POST";
                            request.UnsafeAuthenticatedConnectionSharing = true;
                             response = (HttpWebResponse)request.GetResponse();
                             reader2 = new StreamReader(response.GetResponseStream());
                            string rawst = reader2.ReadToEnd();
                            //Textdene.Text = rawst;
                            using (SqlConnection delConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                            {

                                delConnection.Open();
                                SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                "Where ssn =  '" + EEssn + "' ", delConnection);
                                SqlDataReader delreader = cmd2.ExecuteReader();
                                delConnection.Close();

                                delConnection.Open();
                                SqlCommand cmd4 = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintEmployee]   " +
                               "SET FixFlag = ''     Where ssn =  '" + EEssn + "' ", delConnection);
                                SqlDataReader delreader1 = cmd4.ExecuteReader();
                                delConnection.Close();
                            }

                            int ie = result.IndexOf("importError");
                            int ih = result.IndexOf("importedHire");
                            string Errbox = result.Substring(ie + 15, ih - ie - 19).Replace("\",\"", "|");
                            // Textdene.Text = Errbox;
                            string[] wrdList = Errbox.Split('|');
                            foreach (string wr in wrdList)
                            {
                                string wrd = wr;
                                    wrd = wrd.Replace("'", "");
                                ListBox2.Items.Add(wrd);
                                using (SqlConnection upConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                                {
                                    upConnection.Open();
                                    SqlCommand cmd3 = new SqlCommand("INSERT INTO [Bridge].[dbo].[NewHireMaintErrors] " +
                                        "(ssn, ErrorMsg, ProcessDate, EmpKeyID, ClientID, ErrorType )  " +
                                        "VALUES ('" + EEssn + "' , '" + wrd + "' , GETDATE(), " + ID + ", '" + Sid + "', 'EMP' ) ", upConnection);

                                    Textdene.Text = "INSERT INTO [Bridge].[dbo].[NewHireMaintErrors] " +
                                        "(ssn, ErrorMsg, ProcessDate, EmpKeyID, ClientID, ErrorType )  " +
                                        "VALUES ('" + EEssn + "' , '" + wrd + "' , GETDATE(), " + ID + ", '" + Sid + "', 'EMP' ) ";

                                   SqlDataReader delreader = cmd3.ExecuteReader();
                                    upConnection.Close();
                                }
                            }






                        }



                        /*
                        if (result.IndexOf("importedHire") != -1)
                        {
                            int i = result.IndexOf(",\"importedHire");

                            string d = result.Substring(i);
                            string g = result.Replace(d, "");

                            i = result.IndexOf("importError");
                            if (i > 0)
                            {
                                d = g.Substring(i);
                                errorlist = d.Replace("importError\":[\"", "").Replace("]", "").Replace("\"", "");
                            }

                            //Textdene.Text = errorlist;

                            string[] wrdList = result.Split(',');
                            foreach (string wrd in wrdList)
                            {
                                ListBox2.Items.Add(wrd);

                                //GET BATCHID 
                                if (wrd.IndexOf("importBatchId") != -1)
                                {
                                    batchid = wrd.Replace("importResult", "").Replace("importBatchId", "").Replace("\"", "").Replace("{", "").Replace(":", "");
                                    //  TextBox8.Text = batchid;
                                }
                            }

                            foreach (string wrd in wrdList)
                            {
                                if (wrd.IndexOf("importError") == -1)
                                {
                                    durum = "NoError";
                                }
                            }

                            if (durum == "NoError")
                            {
                                sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/commitEmployees?sessionId=" + sessionID + "&clientId=" + reader.GetString(0) + " &importBatchId=" + batchid + "";

                                // TextBox28.Text = sBaseUrl;
                                request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                                request.Method = "POST";
                                request.UnsafeAuthenticatedConnectionSharing = true;
                                response = (HttpWebResponse)request.GetResponse();
                                reader2 = new StreamReader(response.GetResponseStream());
                                string rawst = reader2.ReadToEnd();
                                // TextBox9.Text = rawst;

                                // var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
                                using (SqlConnection dbConnectionf = new SqlConnection(connfromWebconfig.ConnectionString))
                                {


                                    dbConnectionf.Open();
                                    SqlCommand cmdf = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintEmployee]   " +
                                       "Where ClientID = '" + Sid + "' AND ssn =  '" + reader.GetString(15) + "' ", dbConnectionf);
                                    SqlDataReader readerf = cmdf.ExecuteReader();
                                    dbConnectionf.Close();

                                    dbConnectionf.Open();
                                    SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                        "Where ClientID = '" + Sid + "' AND ssn =  '" + reader.GetString(15) + "' ", dbConnectionf);
                                    SqlDataReader readerfi = cmd2.ExecuteReader();
                                    dbConnectionf.Close();
                                    //Add history
                                    //NEEEEEEEEEEEEEEEEEEEEEEEEED


                                }
                            }


                            foreach (string wrd in wrdList)
                            {
                                if (wrd.IndexOf("importError") != -1)
                                {
                                    //ERROR SHOW CANCEL IMPORTING 
                                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/newHire/cancelImport?sessionId=" + sessionID + "&clientId=" + reader.GetString(0) + " &importBatchId=" + batchid + "";

                                    // TextBox28.Text = sBaseUrl;
                                    request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                                    request.Method = "POST";
                                    request.UnsafeAuthenticatedConnectionSharing = true;
                                    response = (HttpWebResponse)request.GetResponse();
                                    reader2 = new StreamReader(response.GetResponseStream());
                                    string rawst = reader2.ReadToEnd();
                                    // TextBox9.Text = rawst;

                                    using (SqlConnection delConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                                    {
                                        delConnection.Open();
                                        SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                            "Where ClientID = '" + Sid + "' AND ssn =  '" + reader.GetString(15) + "' ", delConnection);
                                        SqlDataReader delreader = cmd2.ExecuteReader();
                                        delConnection.Close();

                                        delConnection.Open();
                                        SqlCommand cmd4 = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintEmployee]   " +
                                            "SET FixFlag = ''     Where ClientID = '" + Sid + "' AND ssn =  '" + reader.GetString(15) + "' ", delConnection);
                                        SqlDataReader delreader1 = cmd4.ExecuteReader();
                                        delConnection.Close();

                                    }
                                }
                            }

                            string[] errList = errorlist.Split(',');
                            foreach (string err in errList)
                            {
                                //ListBox2.Items.Add(err);
                                using (SqlConnection upConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                                {
                                    upConnection.Open();
                                    SqlCommand cmd3 = new SqlCommand("INSERT INTO [Bridge].[dbo].[NewHireMaintErrors] (ssn, ErrorMsg, ProcessDate, EmpKeyID, ClientID )  " +
                                        "VALUES ('" + reader.GetString(15) + "' , '" + err + "' , GETDATE(), " + ID + ", '" + Sid + "') ", upConnection);
                                    SqlDataReader delreader = cmd3.ExecuteReader();
                                    upConnection.Close();
                                }
                            }

                        }

                        */
                    }


                    }
            }
            dbConnection.Close();
          
        }

       // Response.Redirect(Request.Url.ToString());

    }
    
    //remove button
    protected void Button3_Click(object sender, EventArgs e)
    {

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintEmployee]   " +
                "Where ID =  " + TextBox30.Text + " " , dbConnection);           
            SqlDataReader reader = cmd.ExecuteReader();
            dbConnection.Close();

            dbConnection.Open();
            SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                "Where EmpKeyID =  " + TextBox30.Text + " ", dbConnection);

            SqlDataReader reader2 = cmd2.ExecuteReader();
            dbConnection.Close();

        }        

        Response.Redirect(Request.Url.ToString());

    }

    protected void Button4_Click(object sender, EventArgs e)
    {

      
        


    }

    protected void Button5_Click(object sender, EventArgs e)
    {

        TextBoxMessage.Text = GetsessionId();


    }
}