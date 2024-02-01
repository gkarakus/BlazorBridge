<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="TDI.aspx.cs" Inherits="crud" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Time Data import Process</title>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
 <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.0/css/all.css" integrity="sha384-lZN37f5QGtY3VHgisS14W3ExzMWZxybE1SJSEsQp9S+oqd12jhcu+A56Ebc1zFSJ" crossorigin="anonymous">


    <style type="text/css">
       
                     
        .auto-style9 {
            position: absolute;
            top: 277px;
            left: 20px;
            z-index: 1;
            width: 449px;
            right: 1009px;
            height: 28px;
        }
        .auto-style10 {
            position: absolute;
            top: 278px;
            left: 495px;
            z-index: 1;
            width: 110px;
        }
        .auto-style11 {
            position: absolute;
            top: 312px;
            left: 47px;
            z-index: 1;
            width: 386px;
        }
        .auto-style12 {
            position: absolute;
            top: 169px;
            left: 19px;
            z-index: 1;
            width: 231px;
            height: 24px;
        }
        .auto-style13 {
            position: absolute;
            top: 168px;
            left: 687px;
            z-index: 1;
            width: 232px;
            margin-top: 0;
        }
        .auto-style14 {
            position: absolute;
            top: 143px;
            left: 14px;
            z-index: 1;
            width: 234px;
        }
        .auto-style15 {
            position: absolute;
            top: 140px;
            left: 679px;
            z-index: 1;
            width: 145px;
        }     
     
        .auto-style16 {
            position: absolute;
            top: 458px;
            left: 19px;
            z-index: 1;
            width: 815px;
            height: 130px;
            right: 476px;
        }     
     
        .auto-style17 {
            position: absolute;
            top: 168px;
            left: 326px;
            z-index: 1;
            width: 177px;
            margin-bottom: 8;
        }
        .auto-style18 {
            position: absolute;
            top: 144px;
            left: 331px;
            z-index: 1;
            width: 158px;
        }
     
        .auto-style19 {
            position: absolute;
            top: 604px;
            left: 487px;
            z-index: 1;
            width: 168px;
            height: 30px;
        }     
     
        .auto-style20 {
            position: absolute;
            top: 604px;
            left: 1092px;
            z-index: 1;
            width: 163px;
            height: 29px;
           
        }     
     
        .auto-style22 {
            position: absolute;
            top: 227px;
            left: 679px;
            z-index: 1;
            width: 252px;
            height: 130px;
        }
        .auto-style23 {
            position: absolute;
            top: 366px;
            left: 709px;
            z-index: 1;
            width: 193px;
            height: 30px;
        }     
     
        .auto-style24 {
            position: absolute;
            top: 245px;
            left: 23px;
            z-index: 1;
            width: 255px;
        }     
     
        .auto-style25 {
            position: absolute;
            top: 180px;
            left: 1190px;
            z-index: 1;
            width: 124px;
            height: 31px;
        }     
     
        .auto-style26 {
            position: absolute;
            top: 453px;
            left: 853px;
            z-index: 1;
            width: 449px;
            height: 138px;
        }     
     
        .auto-style27 {
            position: absolute;
            top: 707px;
            left: 562px;
            z-index: 1;
            width: 244px;
        }     
     
        .auto-style28 {
            position: absolute;
            top: 426px;
            left: 216px;
            z-index: 1;
            width: 208px;
        }
        .auto-style29 {
            position: absolute;
            top: 425px;
            left: 921px;
            z-index: 1;
            width: 199px;
        }     
     
        .auto-style30 {
            position: absolute;
            top: 198px;
            left: 683px;
            z-index: 1;
            width: 255px;
        }
     
     
        .auto-style31 {
            position: absolute;
            top: 649px;
            left: 24px;
            z-index: 1;
            width: 872px;
            height: 22px;
        }
     
     
    </style>
</head>
<body>
      <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
     <ul class="navbar-nav">
    <li class="nav-item" style="background-color:#808080">
      <a class="nav-link" style="color:white" href="TDI.aspx">TDI-Process</a>
    </li>         
   
     <li class="nav-item">
      <a class="nav-link" href="TDI.aspx">TDI Files</a>
    </li>         
     <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report/LIVE/TDI2" target="_blank" >SummaryReport</a>
    </li>  
    <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report/LIVE/Exception3" target="_blank" >ExceptionReport</a>
    </li>  
     
 <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report/LIVE/Summary1" target="_blank" >NEW-SummaryReport</a>
    </li>  
    <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report/LIVE/Exception4" target="_blank" >NEW-ExceptionReport</a>
    </li>  


     </ul>
        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>
         


   
  <form id="form1" runat="server">

        <p>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </p>
        <asp:FileUpload ID="FileUpload1" runat="server" CssClass="auto-style9" ForeColor="Black" AllowMultiple="true"/>
        <asp:Button ID="Button1" runat="server" CssClass="auto-style10" OnClick="Button1_Click" Text="Upload" />
        <asp:Label ID="StatusLabel" runat="server" CssClass="auto-style11"></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" CssClass="auto-style12" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
            <asp:ListItem>Select one</asp:ListItem>
            <asp:ListItem Value="011820">011820 Hometown</asp:ListItem>
            <asp:ListItem Value="012803">012803 McMullen</asp:ListItem>
            <asp:ListItem Value="012804">012804 Bay Area</asp:ListItem>
             <asp:ListItem Value="012805">012805 Handyman</asp:ListItem>
            <asp:ListItem Value="012995">012995 J.V. Restaurant</asp:ListItem>
            <asp:ListItem Value="013066">013066 24/7 Restaurant</asp:ListItem>
            <asp:ListItem Value="013126">013126 BDE Florida</asp:ListItem>
            <asp:ListItem Value="014237">014237 BDE Orlando</asp:ListItem>
          
            <asp:ListItem>013161_013248_013494</asp:ListItem>
            <asp:ListItem Value="013164">013164 Commercial</asp:ListItem>
            <asp:ListItem Value="013345">013345 Electric Co</asp:ListItem>
            <asp:ListItem Value="013350">013350 Topeka</asp:ListItem>
            <asp:ListItem Value="013397">013397 West Houston</asp:ListItem>
            <asp:ListItem Value="013429">013429 Arbor Capital</asp:ListItem>
            <asp:ListItem Value="013780">013780 WenLake</asp:ListItem>
            <asp:ListItem Value="013455">013455 Caregiver</asp:ListItem>
            <asp:ListItem Value="013487">013487 Fourke</asp:ListItem>
            <asp:ListItem Value="013603">013603 We re Back</asp:ListItem>
            <asp:ListItem Value="013623">013623 CSI Eldery</asp:ListItem>
            <asp:ListItem Value="013738">013738 Codys</asp:ListItem>
            <asp:ListItem Value="013743">013743 Hilsman</asp:ListItem>
            <asp:ListItem Value="013143">013143 Howl Philadelphia</asp:ListItem>
            <asp:ListItem Value="013736">013736 Howl Kansas City</asp:ListItem>
            <asp:ListItem Value="013745">013745 Howl Louisville</asp:ListItem>
            <asp:ListItem Value="014269">014269 Howl Milwaukee</asp:ListItem>
            <asp:ListItem Value="013749">013749  SA Howl</asp:ListItem>
            <asp:ListItem Value="013750">013750 Howl Chicago</asp:ListItem>
            <asp:ListItem Value="013751">013751 Howl Indianapolis</asp:ListItem>
            <asp:ListItem Value="013754">013754 Howl Orlando</asp:ListItem>
            <asp:ListItem Value="014217">014217 Howl Moon</asp:ListItem>
            <asp:ListItem Value="218199">218199 Howl Pittsburg</asp:ListItem>
            <asp:ListItem Value="013843">013843 HM Holdings</asp:ListItem>
            <asp:ListItem Value="013874">013874 Harbor</asp:ListItem>
            <asp:ListItem Value="013925">013925 REHAB</asp:ListItem>
            <asp:ListItem Value="014137">014137 HOME</asp:ListItem>
            <asp:ListItem Value="218064">218064 High Street</asp:ListItem>
            <asp:ListItem Value="218113">218113 HUBB</asp:ListItem>
            <asp:ListItem Value="218197">218197 Seven rest</asp:ListItem>
            <asp:ListItem Value="218230">218230 Pride Restaurant</asp:ListItem>
            <asp:ListItem Value="218260">218260 Jordan </asp:ListItem>
            <asp:ListItem></asp:ListItem>
            <asp:ListItem Value="DOCFOR">DOCFORD All</asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style13" AutoPostBack="True"></asp:TextBox>
        <asp:Label ID="Label1" runat="server" CssClass="auto-style14" Text="1. Please Select Client"></asp:Label>
        <asp:Label ID="Label2" runat="server" CssClass="auto-style15" Text="Selected Client"></asp:Label>
        <asp:TextBox ID="MessageBox" runat="server" CssClass="auto-style16" TextMode="MultiLine" AutoPostBack="True"></asp:TextBox>
        <asp:TextBox ID="TextBox2" runat="server" CssClass="auto-style17" TextMode="Date" OnTextChanged="TextBox2_TextChanged"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" CssClass="auto-style18" Text="2. Select Pay Date"></asp:Label>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style19" OnClick="Button2_Click"  OnClientClick="target='_blank'"  Text="Summary Report" />
       
        <asp:Label ID="Label8" runat="server" CssClass="auto-style24" Text="3. Select Upload File"></asp:Label>
       
        <p>
            <asp:Button ID="Button3" runat="server"  CssClass="auto-style20" OnClick="Button3_Click" Text="Exception Report" />
        </p>
       
        <asp:Label ID="Label4" runat="server" CssClass="auto-style30" Text="4. Select Prism Import file"></asp:Label>
       
        <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" CssClass="auto-style22" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged"></asp:ListBox>
        <asp:Button ID="Button4"  runat="server" CssClass="auto-style23" OnClick="Button4_Click" Text="Download" />
       
        <asp:Button ID="Button5" runat="server" CssClass="auto-style25" OnClick="Button5_Click" Text="Test" Visible="False" />
       
        <asp:TextBox ID="TextBox3" runat="server" CssClass="auto-style26" TextMode="MultiLine" AutoPostBack="True"></asp:TextBox>
       
        <asp:Label ID="Label6" runat="server" CssClass="auto-style28" Text=" TDI Import File"></asp:Label>
        <asp:Label ID="Label7" runat="server" CssClass="auto-style29" Text=" Exceptions"></asp:Label>
       
        <p>
            &nbsp;</p>
        <p>
        <asp:Label ID="Label5" runat="server" CssClass="auto-style27" Text="Label"></asp:Label>
       
        </p>
       
        <asp:TextBox ID="TextBox4" runat="server" CssClass="auto-style31"></asp:TextBox>
       
    </form>

    

       </body>
</html>
