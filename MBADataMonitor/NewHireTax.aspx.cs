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
using System.Web.Script.Serialization;
using System.Text;
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
            SqlCommand cmd = new SqlCommand("SELECT  ssn, ssn + ' ' + firstName + ' ' +  lastName + ' ' + ClientID  FROM[Bridge].[dbo].[NewHireEmployeeMaint]", dbConnection);
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
        TextBox2.Text = ListBox1.SelectedItem.Text.Substring(0, 11).ToString();

          ListBox2.Items.Clear();
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT [ErrorMsg]  FROM[Bridge].[dbo].[NewHireErrorsMaint] " +
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "'", dbConnection);
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
            "benefitsGroup, payGroup, payPeriod,mobilePhone, emailAddress, citizenStatus, ID  FROM[Bridge].[dbo].[NewHireEmployeeMaint] " +
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "' ", dbConnection);
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
   

                   

                        
                    TextBox30.Text = reader.GetInt32(29).ToString();
                }
            }
            dbConnection.Close();
        }

    }
       


    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
       
    }

    protected void Button1_Click1(object sender, EventArgs e)
    {

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [Bridge].[dbo].[NewHireEmployeeMaint]   " +
                "SET ssn = '" + TxtFirstName.Text + "' " +
                ",ClientID          = '" + TextBox2.Text + "' " +
                ",firstName         = '" + TextBox3.Text + "' " +
                ",lastName          = '" + TextBox4.Text + "' " +
                ",birthDate         = '" + TextBox5.Text + "' " +
                ",ethnicCode        = '" + TextBox6.Text + "' " +
            
             
              
             
                ",gender            = '" + TextBox14.Text + "' " +
            
             
                ",FixFlag ='Fixed'" + 
                " Where ssn = '" + ListBox1.SelectedItem.Text.Substring(0, 11).ToString() + "'", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
          
            dbConnection.Close();
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
        string uriString = "https://api.prismhr.com/api-1.20/services/rest/login/createPeoSession";
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

    protected void Button2_Click(object sender, EventArgs e)
    {

        //Check to Table for fixed sign
       

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
                             
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("SELECT ClientID, ssn, firstName, lastName, birthDate, ethnicCode, " +
            "maritalStatus, addressLine1, addressLine2, zipCode, city, stateCode, HomePhone, gender, employeeTypeCode," +
            "workLocationCode, jobCode, origHireDate, lastHireDate, peoStartDate, payMethod, payRate, fedFileStatus, " +
            "benefitsGroup, payGroup, payPeriod,mobilePhone, emailAddress, citizenStatus, ID  FROM[Bridge].[dbo].[NewHireEmployeeMaint] " +
                " Where FixFlag = 'Fixed' ", dbConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {

                    //ListItem itm = new ListItem(reader.GetString(0));
                   // if (!ListBox2.Items.Contains(itm))
                   // {
                   //     ListBox2.Items.Add(reader.GetString(0));
                   // }
                }
            }
            dbConnection.Close();
        }
        

                                 
        //int FixNewHireCnt = 0;
        //int FixNewHireErrorCnt = 0;

        //get session ID
        //  ApiModels.SessionID sessionID = await ApiCalls.CreatePEOSession(ConfigClass.strUser, ConfigClass.strPassword, ConfigClass.strPeoID);

        //  txtSessionID.Text = sessionID.sessionId;
        //  Console.WriteLine(txtSessionID.Text);
        /*
        //gather employerID from client master API for relevant client
        ApiModels.ClientMasterResults newClientMaster = await ApiCalls.GetClientMaster(sessionID.sessionId, ConfigClass.strClientID);
        ConfigClass.strEmployerID = newClientMaster.clientMaster.employerId;

        //load FIXED employees
        string strSQLQuery = $"SELECT * FROM [dbo].[NewHireEmployeeMaint] WHERE [ClientId]=\'{ConfigClass.strClientID}\' AND [FixFlag]=\'Fixed\'";
        SQLUtilities.SQLConnectionOpen(ConfigClass.SQLServer, ConfigClass.SQLDatabase);
        DataTable dtFixedEmployees = SQLUtilities.SQLGetTableData(strSQLQuery);

        if (dtFixedEmployees.Rows.Count > 0)
        {

            List<ApiModels.NewHireEmployee> loadFixedNewHires = new List<ApiModels.NewHireEmployee>();

            //map and load each data row into list for comparison to required fields
            foreach (DataRow SqlData in dtFixedEmployees.Rows)
            {
                //******************************************************************
                //NEED TO DELETE THE FIXED EMPLOYEES AND THE ASSOCIATED ERROR MESSAGES HERE
                //******************************************************************
                strSQLQuery = $"DELETE FROM NewHireEmployeeMaint WHERE [ClientId]=\'{ConfigClass.strClientID}\' AND [FixFlag]=\'Fixed\' AND [ssn] = '{SqlData["ssn"].ToString()}' AND [ID] = '{SqlData["ID"].ToString()}';";
                SQLUtilities.SQLDeleteFIXEDRecords(strSQLQuery);
                strSQLQuery = $"DELETE FROM NewHireErrorsMaint WHERE [ClientId]=\'{ConfigClass.strClientID}\' AND [ssn] = '{SqlData["ssn"].ToString()}';";
                SQLUtilities.SQLDeleteFIXEDRecords(strSQLQuery);

                ApiModels.NewHireEmployee fixedEmployee = ApiUtilities.MapFIXEDEmp(SqlData);

                loadFixedNewHires.Add(fixedEmployee);
            }

            int newEmpKeyID = 0;

            foreach (ApiModels.NewHireEmployee fixedNewHire in loadFixedNewHires)
            {
                //try to import and commit
                //get/refresh session ID
                sessionID = await ApiCalls.CreatePEOSession(ConfigClass.strUser, ConfigClass.strPassword, ConfigClass.strPeoID);

                //only here for temp - delete when routine is complete
                txtSessionID.Text = sessionID.sessionId;
                Console.WriteLine(txtSessionID.Text);

                //IMPORT THE EMPLOYEE
                var newHiresImportResult = await ApiCalls.ImportEmployees(sessionID.sessionId, ConfigClass.strClientID, fixedNewHire);
                if (newHiresImportResult.importResult.importedHire[0].validFlag == true)
                //                        if (newHiresImportResult.importResult.importError == null)
                {
                    //COMMIT THE EMPLOYEE
                    var newHiresCommitResult = await ApiCalls.CommitEmployees(sessionID.sessionId, ConfigClass.strClientID, newHiresImportResult.importResult.importBatchId);
                    if (newHiresCommitResult.commitResult.committedHire.Count > 0)
                    //                            if (newHiresCommitResult.commitResult.commitError == null)
                    {
                        //done, successful newhire added
                        lblMessages.Content = "New Hire Imported!";
                        //done, successful newhire added - update count
                        FixNewHireCnt += 1;
                        lblMessages.Content = "[  " + Convert.ToString(FixNewHireCnt) + "  ] Fixed New Hires Imported! \n" + "[  " + Convert.ToString(FixNewHireErrorCnt) + "  ] Fixed New Hires Errored Out! \n";
                    }
                    else
                    {
                        //save newHire into SQL table to be fixed
                        newEmpKeyID = SQLUtilities.SQLSaveBadNewHire(fixedNewHire);

                        //save list of COMMIT errors by newHire ssn
                        SQLUtilities.SQLSaveBadNewHireCommitErrors(newHiresCommitResult, newEmpKeyID);

                        //CANCEL THE COMMIT/IMPORT
                        var newHiresCancelResult = await ApiCalls.CancelImport(sessionID.sessionId, ConfigClass.strClientID, newHiresImportResult.importResult.importBatchId);

                        //just dummy code in case I want to do something later
                        if (newHiresCancelResult.cancelledHires == "1")
                        {
                            //cancel successful
                            //do something or nothing
                            //proceed to next record
                            Console.WriteLine("Import Cancel Successful");
                        }
                        else
                        {
                            //cancel failed
                            //do something
                            Console.WriteLine("Import Cancel Failed");
                        }
                        //done, newhire error added - update count
                        FixNewHireErrorCnt += 1;
                        lblMessages.Content = "[  " + Convert.ToString(FixNewHireCnt) + "  ] Fixed New Hires Imported! \n" + "[  " + Convert.ToString(FixNewHireErrorCnt) + "  ] Fixed New Hires Errored Out! \n";
                    }

                }
                else
                {
                    //save newHire into SQL table to be fixed
                    newEmpKeyID = SQLUtilities.SQLSaveBadNewHire(fixedNewHire);

                    //save list of IMPORT errors by newHire ssn
                    SQLUtilities.SQLSaveBadNewHireImportErrors(newHiresImportResult, newEmpKeyID);

                    //CANCEL THE IMPORT
                    var newHiresCancelResult = await ApiCalls.CancelImport(sessionID.sessionId, ConfigClass.strClientID, newHiresImportResult.importResult.importBatchId);

                    //just dummy code in case I want to do something later
                    if (newHiresCancelResult.cancelledHires == "1")
                    {
                        //cancel successful
                        //do something or nothing
                        //proceed to next record
                        Console.WriteLine("Import Cancel Successful");
                    }
                    else
                    {
                        //cancel failed
                        //do something
                        Console.WriteLine("Import Cancel Failed");
                    }
                    //done, newhire error added - update count
                    FixNewHireErrorCnt += 1;
                    lblMessages.Content = "[  " + Convert.ToString(FixNewHireCnt) + "  ] Fixed New Hires Imported! \n" + "[  " + Convert.ToString(FixNewHireErrorCnt) + "  ] Fixed New Hires Errored Out! \n";
                }
            }
        }
        else
        {
            //done, newhire error added - show count
            lblMessages.Content = "[  " + Convert.ToString(FixNewHireCnt) + "  ] Fixed New Hires Imported! \n" + "[  " + Convert.ToString(FixNewHireErrorCnt) + "  ] Fixed New Hires Errored Out! \n";
            MessageBox.Show("No FIXED records to process!  Please try again later!");
        }
        //Close SQL connection
        SQLUtilities.SQLConnectionClose();
        MessageBox.Show("Finished!");
        lblMessages.Content = "Messages:";

    */


    }
    //remove button
    protected void Button3_Click(object sender, EventArgs e)
    {

        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["MBASQLConnection"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            dbConnection.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireEmployeeMaint]   " +
                "Where ID =  " + TextBox30.Text + " " , dbConnection);           
            SqlDataReader reader = cmd.ExecuteReader();
            dbConnection.Close();

            dbConnection.Open();
            SqlCommand cmd2 = new SqlCommand("DELETE FROM [Bridge].[dbo].[NewHireErrorsMaint]   " +
                "Where EmpKeyID =  " + TextBox30.Text + " ", dbConnection);

            SqlDataReader reader2 = cmd2.ExecuteReader();
            dbConnection.Close();

        }        

        Response.Redirect(Request.Url.ToString());

    }
}