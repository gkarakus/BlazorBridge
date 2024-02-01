using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Collections.Specialized;
using System.Text;


public partial class autoterm : System.Web.UI.Page
{

  
               

        public string GetsessionId()
        {
            string uriString = "https://api.prismhr.com/api-1.25/services/rest/login/createPeoSession";
            WebClient myWebClient = new WebClient();
            NameValueCollection myNameValueCollection = new NameValueCollection();
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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



    public string KeepAliveSession(string activeSessionId)
    {
        string uriString = "https://api.prismhr.com/api-1.25/services/rest/login/keepAlive";
        WebClient myWebClient = new WebClient();
        NameValueCollection myNameValueCollection = new NameValueCollection();
        myNameValueCollection.Add("sessionId", activeSessionId);      
        byte[] responseArray = myWebClient.UploadValues(uriString, myNameValueCollection);
        string rawst = Encoding.ASCII.GetString(responseArray);
        // string sess = rawst.Substring(14, 25);

        //return sess;

        return rawst;

    }







    // SqlConnection con = new SqlConnection(@"Data Source=MBASQL;Integrated Security=false;User ID=Internaluser;Password=Mba123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;MultiSubnetFailover=False ");
    protected void Page_Load(object sender, EventArgs e)
    {
       
        LabelUserName.Text = Request.LogonUserIdentity.Name;
      //   PrismApi pr = new PrismApi();
      //   TextBox1.Text = pr.GetsessionId();
       
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            try
            {
                dbConnection.Open();
                Label1.Text = "SQL Connection successful";

                SqlCommand cmd = new SqlCommand("AutoTermAuto", dbConnection);
                cmd.CommandTimeout = 1100;
                   cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                 
                
             }
            catch (SqlException ex)
            {
                Label1.Text = "Connection FAIL" + ex.Message;
            }

            finally
            {
                dbConnection.Close();
                Label1.Text = GridView1.Rows.Count.ToString();
            }
        }
    }

  private static int i;

    protected void Button1_Click(object sender, EventArgs e)
    {
        string SessionID = "";
        String CID = "";
        String EEID= "";
        String EEName ="";
        String LastPaydate = "";
        String TermDatest ="";
        String LHireDate = "";
        String Payrepst = "";
        





        DateTime? LastPay = null;
        int DaysNoPaid = 0;
        IFormatProvider culture = new CultureInfo("en-US", true);       

        int satir = GridView1.Rows.Count - 1;
        SessionID = GetsessionId();
        TextBox1.Text = SessionID;




        for (int t = 0; t <= GridView1.Rows.Count-1; t++) 
       
         {
            GridView1.SelectRow(t);
            //TextBox1.Text = GridView1.Rows[t].Cells["EE_Sort_Name"].Value.ToString();          
            GridViewRow row = GridView1.SelectedRow;         
            if (row.Cells[10].Text == "&nbsp;" && row.Cells[1].Text == "A" && int.Parse(row.Cells[9].Text) < -2 )
              //  if (row.Cells[2].Text == "J70666")
                {
               // SessionID = GetsessionId();
               // TextBox1.Text = SessionID;
                TextBox1.Text = KeepAliveSession(SessionID);

                row.Cells[10].Text = "TERM";
                CID = row.Cells[0].Text;
                EEID = row.Cells[2].Text;
                EEName = row.Cells[3].Text;
                LHireDate = row.Cells[4].Text;
                Payrepst = row.Cells[11].Text;
                //DateTime dateVal = DateTime.ParseExact(row.Cells[5].Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime dateVal = Convert.ToDateTime(row.Cells[5].Text, culture);
                TermDatest = dateVal.ToString("yyyy-MM-dd");
                LastPaydate = row.Cells[6].Text;
                DaysNoPaid = int.Parse(row.Cells[7].Text);
                //----API TERMINATION START
                string sBaseUrl = "";
                string ErrString = "";
                string ErrType = "";
                string ErrCode = "";    


                    sBaseUrl = "https://api.prismhr.com/api-1.25/services/rest/employee/terminateEmployee";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sBaseUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\n\"sessionId\":\"" + SessionID + "\", " +
                        "\n \"clientId\": \"" + CID + "\", " +
                        "\n \"employeeId\": \"" + EEID + "\", " +
                        "\n \"termStatusCode\": \"T\"," +
                        "\n \"reasonCode\": \"TI17\"," +
                        "\n \"termDate\": \"" + TermDatest + "\"," +
                        "\n \"termReason\": \"[I] No Hours Reported\"," +
                        "\n \"okayToRehire\": \"\"," +
                        "\n \"turnOffACH\": \"Y\"," +
                        "\n \"createCobraRecord\": \"N\"," +
                        "\n \"forceApproval\": \"false\"  \n  }";
             
                        streamWriter.Write(json);
                   // File.WriteAllText(@"C:\TDI\dd.json", json);
                    streamWriter.Flush();
                        streamWriter.Close();
                     }
               // string json = JsonSerializer.Serialize(_data);
               

                //HttpWebResponse httpResponse =(HttpWebResponse)httpWebRequest.GetResponse();
             //   try
              //  {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
              //  }

              //  catch (IndexOutOfRangeException e) when (index < 0)
              //  {
              //      throw new ArgumentOutOfRangeException(
              //          "Parameter index cannot be negative.", e);
              //  }

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                        var result = streamReader.ReadToEnd();
                        string[] content = result.Replace("\"", "").Split(',');
                    foreach (string word in content)
                    {
                        word.Replace("{", "").Replace("}", "");
                        if (word.Contains("errorType"))
                        {
                            ErrType = word.Replace("terminateMessages:", "").Replace("[","");
                        }
                        if (word.Contains("errorString"))
                        {
                            ErrString = word.Replace("errorString:", "");
                        }
                        if (word.Contains("errorCode"))
                        {
                            ErrCode = word;
                        }
                    }
                     //  Console.WriteLine(ErrCode + "--" + ErrType + "==" + ErrString);
                    //  Console.ReadLine();               

                }
                LastPaydate = LastPaydate.Trim();
                LHireDate = LHireDate.Trim();
                ErrString = ErrString.Replace("Please note these State Rules:", "");
                ErrType = ErrType.Replace("{errorType:","");
                if (ErrString.Length > 200)
                {
                    ErrString = ErrString.Substring(0, 75);
                }
                HistoryInsert(CID, EEID, EEName,LastPaydate, LHireDate, TermDatest, DaysNoPaid, ErrCode, ErrType, ErrString,  Payrepst);
                // HistoryInsert("123321", "G12345", "karafffkus", "2019-01-01", "2018-01-01", "2020-01-01", 555, "ecode", "ertype", "bla bla");


                CID = "";
                EEID = "";
                EEName = "";
                LHireDate = "";
                Payrepst = "";
                //DateTime dateVal = DateTime.ParseExact(row.Cells[5].Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
              //  DateTime dateVal = Convert.ToDateTime(row.Cells[5].Text, culture);
                TermDatest = dateVal.ToString("yyyy-MM-dd");
                LastPaydate = "";
                DaysNoPaid = 0;




            }

              //    System.Threading.Thread.Sleep(500);
            //Label1.Text = (int.Parse(Label1.Text) + 1).ToString();
            //Response.Redirect("~/autoterm.aspx");
            TextBox1.Text = DaysNoPaid.ToString();
           
        }
        
    }

    public static void HistoryInsert(String Cid, string EE, string EEname, string Lpdate, string LastHireDate, string Termdate, int Npay, string Ecode, string Etype, string Emsg, string Payrep)
    {
        var connfromWebconfig = WebConfigurationManager.ConnectionStrings["BridgeConnectionString"];
        using (SqlConnection dbConnection = new SqlConnection(connfromWebconfig.ConnectionString))
        {
            try
            {
                dbConnection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO AutoTermHistory (ClientID, EE_ID, EE_Sort_Name, LastHireDate, LastPaidInvoiceDate, DaysWithoutPay,TermDate, ErrorCode, ErrorType, ErrorMsg,PayRep, ProcessDate)" +
                    " Values (@CID, @EE, @Name, @LasthireDate,  @LastPay, @Days, @Termdate, @ErCode, @ErType, @ErMsg, @PayRep, @ExecDate) ", dbConnection);
               
                cmd.Parameters.AddWithValue("@CID", Cid);
                cmd.Parameters.AddWithValue("@EE", EE);
                cmd.Parameters.AddWithValue("@Name", EEname);
                cmd.Parameters.AddWithValue("@lasthireDate", LastHireDate);
                cmd.Parameters.AddWithValue("@LastPay", Lpdate);
                cmd.Parameters.AddWithValue("@Days", Npay);                
                cmd.Parameters.AddWithValue("@Termdate", Termdate);
                cmd.Parameters.AddWithValue("@ErCode", Ecode);
                cmd.Parameters.AddWithValue("@ErType", Etype);
                cmd.Parameters.AddWithValue("@ErMsg", Emsg);
                cmd.Parameters.AddWithValue("@PayRep", Payrep);
                cmd.Parameters.AddWithValue("@ExecDate", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                //  Label1.Text = "Connection FAIL" + ex.Message;
            }
            finally
            {
                dbConnection.Close();

            }

        }

    }




    protected void Button2_Click(object sender, EventArgs e)
    {
      
        String SessionID;
        //PrismApi pr = new PrismApi();
        SessionID = GetsessionId();

        TextBox1.Text = SessionID;
        System.Threading.Thread.Sleep(3000);

        TextBox1.Text = KeepAliveSession(SessionID);

        System.Threading.Thread.Sleep(9000);
        /*
        string Termdate = "";
        IFormatProvider culture = new CultureInfo("en-US", true);
        GridView1.SelectRow(1);
        GridViewRow row = GridView1.SelectedRow;

       // DateTime dateVal = DateTime.ParseExact(row.Cells[5].Text, "MM/dd/yyyy", culture);
       // Termdate = dateVal.ToString("yyyy-MM-dd");
        DateTime tempDate = Convert.ToDateTime(row.Cells[5].Text, culture);

        TextBox1.Text = tempDate.ToString("yyyy-MM-dd");
        //TextBox1.Text = "buda iki";

    */
    }



    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}