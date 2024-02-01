using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AutoTermLog : System.Web.UI.Page
{

    protected string message1;
    protected void Page_Load(object sender, EventArgs e)
    {
        LabelUserName.Text = Request.LogonUserIdentity.Name;
        if (Request.LogonUserIdentity.Name == @"MBASTPETE\gokhank")
        {
            message1 = "Merhaba Gokhan";

        }
    }








}