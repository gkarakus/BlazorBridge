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
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;

public partial class deneme : System.Web.UI.Page
{



    protected void Page_Load(object sender, EventArgs e)
    {
      //  ListBox1.Items.Clear();
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT  ssn, ssn + ' ' + firstName + ' ' +  lastName + ' ' + ClientID  FROM[Bridge].[dbo].[NewHireMaintDirectDeposit]", dbConnection);
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
                " Where ErrorType = 'DD' AND   ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "'", dbConnection);
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
            SqlCommand cmd = new SqlCommand("SELECT ID, PayType,firstName, lastName, ssn, employeeID, transitNum,  " +
            "accountNum, accountType, method, amount, limit, status, ProcessDate, ClientID, FixFlag  FROM[Bridge].[dbo].[NewHireMaintDirectDeposit] " +
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "' ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TextBox2.Text = reader.GetString(14);  // clientID
                    TxtFirstName.Text = reader.GetString(4); //snn                   
                    TextBox3.Text = reader.GetString(2);  //firstname
                    TextBox4.Text = reader.GetString(3);  //lastname
                    TextBox31.Text = reader.GetString(5); //EmployeeID
                    TextBox32.Text = reader.GetString(6); //routing 
                    TextBox33.Text = reader.GetString(7); // AccountNumber
                    TextBox34.Text = reader.GetString(8); // Acc type
                    TextBox35.Text = reader.GetString(9); // method
                    TextBox36.Text = reader.GetDouble(10).ToString(); //Amount 
                    TextBox37.Text = reader.GetInt32(11).ToString();     //limit
                    TextBox38.Text = reader.GetString(12); //status
                    TextBox30.Text = reader.GetString(1); //Paytype 
                }
            }
            dbConnection.Close();
        }
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
       
    }
    //======================================SAVE BUTTON ===========================================
    protected void Button1_Click1(object sender, EventArgs e)
    {
        if (TextBox3.Text != "")
        {
            var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
            using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintDirectDeposit]   " +
                    "SET ssn = '" + TxtFirstName.Text + "' " +
                    ",ClientID          = '" + TextBox2.Text + "' " +
                    ",firstName         = '" + TextBox3.Text + "' " +
                    ",lastName          = '" + TextBox4.Text + "' " +
                    ",EmployeeID        = '" + TextBox31.Text + "' " +
                    ",transitNum        = '" + TextBox32.Text + "' " +
                    ",accountNum        = '" + TextBox33.Text + "' " +
                    ",accountType       = '" + TextBox34.Text + "' " +
                    ",method            = '" + TextBox35.Text + "' " +
                    ",amount            = " + TextBox36.Text + " " +
                    ",limit             = '" + TextBox37.Text + "' " +
                    ",status            = '" + TextBox38.Text + "' " +

                    ",FixFlag ='Fixed'" +
                    " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "'", dbConnection);
                SqlDataReader reader = cmd.ExecuteReader();

                dbConnection.Close();
            }
        }
        Response.Redirect(Request.Url.ToString());
    }



    
    protected void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
    {

       // TextBox13.Text = ListBox2.SelectedValue
    }
    // Submit button 




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


    //SUBMIT BUTTON *************************************************
    protected void Button2_Click(object sender, EventArgs e)
    {

        //Check to Table for fixed sign
        String cid = "";
        string sessionID = "";
        string eeid = "";
        string ssn = "";

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
                             
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT ID, PayType,firstName, lastName, ssn, employeeID, transitNum,  " +
            "accountNum, accountType, method, amount, limit, status, ProcessDate, ClientID, FixFlag  FROM[Bridge].[dbo].[NewHireMaintDirectDeposit] " +
                " Where FixFlag = 'Fixed' ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int ID = reader.GetInt32(0);
                    sessionID = GetsessionId();

                    cid = String.Format("{0:000000}", reader.GetString(14));
                    eeid = reader.GetString(5);
                    ssn = reader.GetString(4);
                    string resultP = "";
//*************************************GET CHECKSUM 

                    string pURL = "https://api.prismhr.com/api-1.25/services/rest/employee/getEmployee?sessionId=" + sessionID + "&employeeId=" + eeid + "&clientId=" + cid + "&options=DirectDeposit";

                    HttpWebRequest requestP = (HttpWebRequest)WebRequest.Create(pURL);
                    requestP.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    using (HttpWebResponse responseP = (HttpWebResponse)requestP.GetResponse())
                    using (Stream stream = responseP.GetResponseStream())
                    using (StreamReader readerP = new StreamReader(stream))
                    {
                      resultP = readerP.ReadToEnd();
                    }
                    testmsg.Text = resultP;
                    // BUG BUG  BUG ------------------------------------------------need ATTENCION 
                    int s = resultP.IndexOf("checksum");                    
                    string chksum = resultP.Substring(s + 11, 7);
                    //Cut existing Account info if exist.
                    int x = resultP.IndexOf("directDeposit");
                    
                    string xAcc = resultP.Substring(x -1, s - x-2);
                    int p = xAcc.IndexOf("[");
                    string xAccd = xAcc.Substring(0, p+1 );

                   // TextBox32.Text = sessionID;
                  //  TextBox39.Text = chksum;
                   // testmsg.Text = xAcc;                    

                    //Update Account Call Update API
                   

                    pURL = "https://api.prismhr.com/api-1.25/services/rest/employee/updateDirectDeposit";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(pURL);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + sessionID + "\", \n" +
                         "\"clientId\": \"" + cid + "\", " +
                         "\n\"employeeId\": \"" + eeid + "\", " +                                                    
                         "\n " + xAccd + "" +
                          "\n{\"transitNum\": \"" + reader.GetString(6) + "\"," +
                        "\n\"accountNum\": \"" + reader.GetString(7) + "\"," +
                        "\n\"accountType\": \"" + reader.GetString(8) + "\", " +
                        "\n\"method\": \"" + reader.GetString(9) + "\"," +
                        //---- "\n \"amount\": \"" + reader.GetDouble(10).ToString() + "\"," +
                        //---   "\n \"limit\": \"" + reader.GetInt32(11).ToString() + "\"," +
                        "\n \"amount\": "  + reader.GetDouble(10).ToString() + "," +
                        "\n \"limit\": "  + reader.GetInt32(11).ToString() + "," +

                          "\n \"status\": \"" + reader.GetString(12) + "\"," +
                         "\n \"voucherTypeOverride\": \"\"}\n], " +

                         "\n \"checksum\": \"" + chksum + "\"\n}\n} ";

                      //  testmsg.Text = json;
                        streamWriter.Write(json);

                       // File.WriteAllText(@"C:\test\dd.json", json);
                        
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                   // return;

                    //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                    //var htttResponse = (HttpWebResponse)htttResponse.GetResponseStream();
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        testmsg.Text = result;

                        if (result.IndexOf("Updated Successfully") == -1)
                        {

                            int er = result.IndexOf("errorMessage");
                            int ext = result.IndexOf("extension");
                            string errmes = result.Substring(er + 15, ext - er);
                           // errmes = errmes.Replace("\"extension\":null};", "");
                            
                            
                            using (SqlConnection delConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                            {
                                delConnection.Open();
                                SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                "Where ErrorType = 'DD' AND  ssn =  '" + ssn + "' ", delConnection);
                                SqlDataReader delreader = cmd2.ExecuteReader();
                                delConnection.Close();

                                delConnection.Open();
                                SqlCommand cmd4 = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintDirectDeposit]   " +
                               "SET FixFlag = ''     Where ssn =  '" + ssn + "' ", delConnection);
                                SqlDataReader delreader1 = cmd4.ExecuteReader();
                                delConnection.Close();
                            }
                            //Add new error on SQL

                            using (SqlConnection upConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                            {
                                upConnection.Open();
                                SqlCommand cmd3 = new SqlCommand("INSERT INTO [Bridge].[dbo].[NewHireMaintErrors] " +
                                    "(ssn, ErrorMsg, ProcessDate, EmpKeyID, ClientID, ErrorType )  " +
                                    "VALUES ('" + ssn + "' , '" + errmes + "' , GETDATE(), "+ ID +", '" + cid + "', 'DD' ) ", upConnection);
                                SqlDataReader delreader = cmd3.ExecuteReader();
                                upConnection.Close();
                            }  




                        }
                        else
                        {

                            using (SqlConnection delConnection = new SqlConnection(connfromWebconfig.ConnectionString))
                            {
                                delConnection.Open();
                                SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                                "Where ErrorType = 'DD' AND  ssn =  '" + ssn + "' ", delConnection);
                                SqlDataReader delreader = cmd2.ExecuteReader();
                                delConnection.Close();

                                delConnection.Open();
                                SqlCommand cmd4 = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireMaintDirectDeposit]   " +
                               "SET FixFlag = ''     Where ssn =  '" + ssn + "' ", delConnection);
                                SqlDataReader delreader1 = cmd4.ExecuteReader();
                                delConnection.Close();
                            }

                            testmsg.Text = "no Error";
                        }

                    }                  

                }
            }
            dbConnection.Close();
        }                            
       


    }
   
    //TEST BUTTON
    protected void Button3_Click1(object sender, EventArgs e)
    {

        string sessionID = GetsessionId();
        string eeid = "A46570";
        string cid = "000100";

        TextBox39.Text = sessionID;
        
      
        string resultP = "";
        //HTTP GET
        string pURL = "https://api.prismhr.com/api-1.25/services/rest/employee/getEmployee?sessionId=" + sessionID + "&employeeId=" + eeid + "&clientId=" + cid + "&options=DirectDeposit";

        HttpWebRequest requestP = (HttpWebRequest)WebRequest.Create(pURL);
        requestP.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        using (HttpWebResponse responseP = (HttpWebResponse)requestP.GetResponse())
        using (Stream stream = responseP.GetResponseStream())
        using (StreamReader readerP = new StreamReader(stream))
        {
            resultP = readerP.ReadToEnd();
        }
        testmsg.Text = resultP;

        int s = resultP.IndexOf("checksum");
        string chksum = resultP.Substring(s + 11, 7);
        TextBox32.Text = sessionID;
        TextBox39.Text = chksum;
        

    }

    protected void BtnRemove_Click(object sender, EventArgs e)
    {
        if (TextBox1.Text != "")
        {



            var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
            using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintDirectDeposit]   " +
                   "Where employeeID =  '" + TextBox31.Text + "' ", dbConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                dbConnection.Close();

                dbConnection.Open();
                SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireMaintErrors]   " +
                    "Where ErrorType = 'DD' AND ssn =  '" + TxtFirstName.Text + "' ", dbConnection);

                SqlDataReader reader2 = cmd2.ExecuteReader();
                dbConnection.Close();

                TextBox1.Text = TextBox31.Text + " EEID  Removed ";
                dbConnection.Open();
                 cmd = new SqlCommand("SELECT  ssn, ssn + ' ' + firstName + ' ' +  lastName + ' ' + ClientID  FROM[Bridge].[dbo].[NewHireMaintDirectDeposit]", dbConnection);
                 reader = cmd.ExecuteReader();
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

        }

        Response.Redirect(Request.RawUrl);


    }
}