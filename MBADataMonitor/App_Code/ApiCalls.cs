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

class PrismApi
    {
    /*
                //Depracated  2021 - GK - 
        public string OLDGetsessionId()
        {
            string sBaseUrl = "";
            sBaseUrl = "https://api.hrpyramid.net/api-1.19/services/rest/login/createPeoSession?username=apiLIVE&password=ThisIs13UGze&peoId=545";

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



}




