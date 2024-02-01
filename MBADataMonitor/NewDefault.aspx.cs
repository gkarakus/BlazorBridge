using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class NewDefault : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Btn2_Click(object sender, EventArgs e)
    {
        System.Diagnostics.Process.Start("http://www.facebook.com");
    }
}