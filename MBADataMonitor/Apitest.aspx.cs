using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Apitest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
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
        TextBox1.Text = sess;

    }




//Term Test 
    protected void Button2_Click(object sender, EventArgs e)
    {
        string sessionid = TextBox1.Text;
        string clientid = "013126";
        string employeeid  = "P45855";
        string termDate = "2021-02-23";
        string space = "";


        string apilink = "https://api.prismhr.com/api-1.25/services/rest/employee/terminateEmployee";
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(apilink);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";
        httpWebRequest.UnsafeAuthenticatedConnectionSharing = true;

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {

            string json = "{\n\"sessionId\":\"" + sessionid + "\", \n" +
              "\"clientId\": \"" + clientid + "\", " +
             "\n\"employeeId\": \"" + employeeid + "\", " +           
             "\n\"termStatusCode\": \"T\"," +
             "\n\"reasonCode\": \"TI17\"," +
             "\n\"termDate\": \"" + termDate + "\"," +
             "\n\"termReason\": \"[I] No Hours Reported\"," +
             "\n\"okayToRehire\": \"\"," +
             "\n\"turnOffACH\": \"Y\"," +            
             "\n\"forceApproval\": \"false\"  \n  } ";



            TextBox2.Text = json;

            streamWriter.Write(json);
            streamWriter.Flush();
            streamWriter.Close();
        }
               var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
       
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            var result = streamReader.ReadToEnd();

            //  Console.WriteLine(result);
            // Console.ReadLine();
            TextBox3.Text = result;
        }

       

        /*
        "\n\"createCobraRecord\": \"" + space + "\"," +
            "\n\"cobraQualifyingEvent\": \"" + space + "\"," +
            "\n\"cobraQualifyingEventDate\": \"" + space + "\"," +
        */


    }





}