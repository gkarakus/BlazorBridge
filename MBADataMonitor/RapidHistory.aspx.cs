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

   


   



   
   

   
  
}