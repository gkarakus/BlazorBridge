using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Globalization;

public partial class AutoTermLog : System.Web.UI.Page
{
   
    protected string message1;

    protected void Page_Load(object sender, EventArgs e)
    {
        LabelUserName.Text = Request.LogonUserIdentity.Name;
        // SqlInsert();
        //SqlShow();
        UpdateRapidTable();
    }

   

    

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        TextBox1.Text = System.DateTime.Now.ToString("hh:mm:ss");
        GetEEforRapid();
        System.Threading.Thread.Sleep(1000);
        CallRapidPRO(TextBox3.Text,TextBox1.Text);
        System.Threading.Thread.Sleep(1000);
        UpdateRapid();
    }


    public void SqlInsert()
    {
     
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();                
                
                    SqlCommand cmd = new SqlCommand("INSERT INTO [Bridge].[dbo].[PapidpayTest]   " +
                        " (ClientID, ClientName) " +
                          " VALUES ('121212','Gokhan company')", dbConnection);

                    cmd.ExecuteNonQuery();                       
          
            dbConnection.Close();
        }

        return;
    }

    public void SqlShow()
    {
        TextBox2.Text = "";
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();

            SqlCommand cmd = new SqlCommand("SELECT  ClientID, ClientName FROM [Bridge].[dbo].[PapidpayTest]", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                TextBox2.Text += reader.GetString(0)+ "  " + reader.GetString(1) + "\r\n"; 
            }
            reader.Close();
            dbConnection.Close();
        }
    }


    protected void Button2_Click(object sender, EventArgs e)
    {
        CallRapidPRO(TextBox3.Text, TextBox1.Text);
    }



    public void SqlUpdate()
    {
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            //          SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[PapidpayTest]   " +
            //            "SET ssn = '" + TxtFirstName.Text + "' " +
            //          ",ClientID          = '" + TextBox2.Text + "' " +             
            //          " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "'  AND " +
            //          " ClientID = '" + ListBox1.SelectedItem.Text.Substring(ListBox1.SelectedItem.Text.Length - 6, 6).ToString() + "'", dbConnection);
            //     SqlDataReader reader = cmd.ExecuteReader();
            dbConnection.Close();
        }
    }


    public void UpdateRapidTable()
    {
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SpRapidPayAutoUpdate", dbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@firstname", txtfirstname.Text);
                dbConnection.Open();
                cmd.ExecuteNonQuery();
            }

            dbConnection.Close();
        }

        return;

    }

    public void GetEEforRapid()
    {
        
        string Acc = "";
        string lastname = "";
        string firstname = "";
        string middlename = "";
        string Addr1 = "";
        string Addr2 = "";
        string City = "";
        string St = "";
        string Country = "";
        string Zip = "";
        string Dob = ""; 
        string ssn = "";
        string email = "";
        string phone1 = "";
        string phone2 = "";

        string addtxt = "";

        List<string> ClientNumber = new List<string>();
        List<string> EEID = new List<string>();
        List<string> EEinformation = new List<string>();
        List<string> APIresponse = new List<string>();

        TextBox2.Text = "";
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
          

            SqlCommand cmd = new SqlCommand("SELECT TOP (1) Acct_No, LastName, " +
                "FirstName, MiddleName, Address1, Address2, City, State, Zip, Birthdate, SSN, Email, Phone1, Phone2, " +
                "ClientID, EmpID, RapidStatus FROM [Bridge].[dbo].[PapidpayEmp] WHERE RapidStatus IN ('open')  ORDER BY NEWID() ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                // TextBox2.Text += reader.GetString(0) + "  " + reader.GetString(1) + "\r\n";
                TextBox1.Text = reader.GetString(0).Substring(0, 3);
                Acc = reader.GetString(0).Substring(3);
                lastname = reader.GetString(1);
                firstname = reader.GetString(2);
                middlename = reader.GetString(3);
                Addr1 = reader.GetString(4);
                Addr2 = reader.GetString(5);
                City = reader.GetString(6);
                St = reader.GetString(7);
                Zip = reader.GetString(8);
                 Dob = reader.GetDateTime(9).ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                //Dob = reader.GetString(9);
                 ssn = reader.GetString(10).Replace("-","");
                email = reader.GetString(11);
                phone1 = reader.GetString(12).Replace("-", "").Replace("(", "").Replace(")","").Replace(" ", "").Replace("/","");
                phone2 = reader.GetString(13).Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("/","");

                Country = "US";
                //Identificator 

                System.Threading.Thread.Sleep(1000);
                //  addtxt = Acc + "," + lastname + "," + firstname +","+ middlename + ",," + Addr1 + "," + Addr2 +",," + City+"," + St+"," + Country + ","+Zip+","+ Dob+ ","+ssn + ",,,,"   + email + "," + phone1 + "," + phone2 + ",,,,,,,,,0,1,,,,,,,,,,,New Card Registration";

                addtxt = Acc + "," + lastname + "," + firstname + "," + middlename + ",," + Addr1 + "," + Addr2 + ",," + City + "," + St + "," + Country + "," + Zip + "," + Dob + "," + ssn + ",,,,"+email+","+phone1+","+phone2 +" ,,,,,,,,,,0,1,,,,,,,,,,,New Card Registration";
              
                 TextBox3.Text = addtxt;
                //    for next line + "\r\n";
                //ClientNumber.Add(reader.GetString(14));
                //EEID.Add(reader.GetString(15));
                TextBoxClientId.Text = reader.GetString(14);
                TextBoxEmpID.Text = reader.GetString(15);

                EEinformation.Add(addtxt);

            }

            reader.Close();
            dbConnection.Close();
        }

        //ALL OPEN CASES COLLECTED    NOW SEND RAPID PAY 
        
        


    }

 
    public void CallRapidPRO(string protext, string transID)
    {
        //ServicePointManager.Expect100Continue = true;
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        // System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
        // ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

        //const SecurityProtocolType tls13 = (SecurityProtocolType)12288;
        //ServicePointManager.SecurityProtocol = tls13 | SecurityProtocolType.Tls12;




      //  PRORapid.FsvRemoteServiceSoapClient client = new PRORapid.FsvRemoteServiceSoapClient();
        PRORapid2.FsvRemoteServiceSoapClient client = new PRORapid2.FsvRemoteServiceSoapClient();

        if (transID == "933")
        {
            var responce = client.Transact("MBA", "5z9te86GWlBVV", "1233323912,4314,3123,1233323912,4314," + protext);
            TextBox2.Text = responce;
        }
        if (transID == "353")
        {
              var responce = client.Transact("MBA", "5z9te86GWlBVV", "2306896784,4047,3123,2306896784,4047," + protext);
          
            TextBox2.Text = responce;
        }

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        // GetEEforRapid();
        GetEEforRapid();

        // var responce = client.Transact("MBA", "5z9te86GWlBVV", "124085244,4047,3123,124085244,4047," + protext);
    }


    public void UpdateRapid()
    {
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];

        if (TextBox2.Text.Contains("Transaction successful"))
        {

            using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[PapidpayEmp]   " +
                  "SET Response = '" + TextBox2.Text.ToString() + "' , RapidStatus = 'pass', ProcessDate = GETDATE() " +
                " Where ClientID = '" + TextBoxClientId.Text.ToString() + "'  AND " +
                " EmpID = '" + TextBoxEmpID.Text.ToString() + "' ", dbConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                dbConnection.Close();
            }
        }
        else
        {
            using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[PapidpayEmp]   " +
                  "SET Response = '" + TextBox2.Text + "' , RapidStatus = 'error' , RapidExtra = 'today', ProcessDate = GETDATE() " +
                " Where ClientID = '" + TextBoxClientId.Text.ToString() + "'  AND " +
                " EmpID = '" + TextBoxEmpID.Text.ToString() + "' ", dbConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                dbConnection.Close();
            }
        }

    }

    public void DeleteErrors()
    {
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];

        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[PapidpayEmp]   " +
              "Set RapidStatus = 'pass' , RapidExtra = 'pushtopass' " +
            " WHERE CHARINDEX('85,The CertCard Company', Response) <> 0  AND RapidStatus = 'error'  AND RapidExtra = 'today'  " +
            " AND CAST(ProcessDate as time) > '14:30:00.9100000'  ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            dbConnection.Close();
        }







        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM [Bridge].[dbo].[PapidpayEmp]   " +
            " WHERE RapidStatus = 'error' ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            dbConnection.Close();

        }
    }

        protected void Button3_Click(object sender, EventArgs e)  //Update Table 
    {
        UpdateRapid();
    }

    protected void ButtonDeleteError_Click(object sender, EventArgs e)
    {
        DeleteErrors();
        UpdateRapidTable();
    }
}