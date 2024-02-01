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

public partial class AutoTermLog : System.Web.UI.Page
{

    protected string message1;

    protected void Page_Load(object sender, EventArgs e)
    {
        LabelUserName.Text = Request.LogonUserIdentity.Name;
       

    }

   

    /*
     //DEPRACATED NOT USE 
    public string GetsessionId()
    {
        string sBaseUrl = "";
        sBaseUrl = "https://api.prismhr.com/api-1.19/services/rest/login/createPeoSession?username=apiLIVE&password=ThisIs13UGze&peoId=545";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBaseUrl);
        request.Method = "POST";
        request.UnsafeAuthenticatedConnectionSharing = true;

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());

        string rawst = reader.ReadToEnd();
        string sess = rawst.Substring(14, 25);
        return sess;
    }

    */


    protected void Button1_Click(object sender, EventArgs e)
    {
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        UATRapid.FsvRemoteServiceSoapClient client = new UATRapid.FsvRemoteServiceSoapClient();
        var responce = client.Transact("MBADEV", "RT86*$hRDetgd", "1233323912, 4314, 3801,Verify FSV SOAP Connection Services");

        TextBox2.Text = responce;

    }






    protected void Button2_Click(object sender, EventArgs e)
    {

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        PRORapid.FsvRemoteServiceSoapClient client = new PRORapid.FsvRemoteServiceSoapClient();

        var responce = client.Transact("MBA", "5z9te86GWlBVV", "1233323912, 4314, 3801, Verify FSV SOAP Connection Services");

        TextBox2.Text = responce;
    }
    //Pro send employee and get response 
    protected void Button4_Click(object sender, EventArgs e) 
    {

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        PRORapid.FsvRemoteServiceSoapClient client = new PRORapid.FsvRemoteServiceSoapClient();

        //var responce = client.Transact("MBA", "5z9te86GWlBVV", "1233323912,4314,3123,1233323912,4314,3954991349,LAST NAME,FIRST NAME,,,ADDRESS,,,CITY,OH,US,44236,19900101,647846178,,,,,,,,,,,,,,,,0,1,,,,,,,,,,,New Card Registration");
      
        
        //MBA 
        //  var responce = client.Transact("MBA", "5z9te86GWlBVV", "1233323912,4314,3123,1233323912,4314,3678821483,Weddington,Paul,,,1906 Sharp St. Apt C,,,Chattanooga,TN,US,37404,19810624,415351054,,,,,,,,,,,,,,,,0,1,,,,,,,,,,,New Card Registration");
      
        //Gold CO 
        var responce = client.Transact("MBA", "5z9te86GWlBVV", "2306896784,4047,3123,2306896784,4047,3679172357,CyrilienDerice,Dieudonise,,,1620 NW 14th Cir,,,Pompano Beach,FL,US,33096,19850315,863928196,,,,,,,,,,,,,,,,0,1,,,,,,,,,,,New Card Registration");

        TextBox2.Text = responce;



    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        UATRapid.FsvRemoteServiceSoapClient client = new UATRapid.FsvRemoteServiceSoapClient();
        var responce = client.Transact("MBADEV", "RT86*$hRDetgd", "6889288400,4047,3123,6889288400,4047,6889288822,LAST NAME,FIRST NAME,,,ADDRESS,,,CITY,OH,US,44236,19900101,647846178,,,,,,,,,,,,,,,,0,1,,,,,,,,,,,New Card Registration");

        TextBox2.Text = responce;
    }
}