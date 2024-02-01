<%@ Page Language="C#"  Debug="true"  AutoEventWireup="true" CodeFile="Everify.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BridgeMain</title>
    <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css"> 
   <script src="https://kit.fontawesome.com/447ed2f583.js" crossorigin="anonymous"></script>
   <link rel="stylesheet" href="Content/style.css">
     <!--  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"> -->
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@600&display=swap" rel="stylesheet">
    <style>
       body {
            background-color: #0A171C;
            color:#F2FDFF;
        }     

.fa-2x {
    margin: 0 auto;
    float: none;
    display: table;
    color: #4ad1e5;
}

        .auto-style1 {
            position: relative;
            width: 100%;
            -ms-flex-preferred-size: 0;
            flex-basis: 0;
            -ms-flex-positive: 1;
            flex-grow: 1;
            max-width: 100%;
            left: 6px;
            top: 1px;
            padding-left: 15px;
            padding-right: 15px;
        }
        .auto-style2 {
            position: relative;
            width: 446px;
            -ms-flex-preferred-size: 0;
            flex-basis: 0;
            -ms-flex-positive: 1;
            flex-grow: 1;
            max-width: 100%;
            left: 790px;
            top: 280px;
            z-index: 1;
            padding-left: 15px;
            padding-right: 15px;
            height: 551px;
            margin-bottom: 0px;
        }

        .auto-style3 {
            width: 1349px;
            height: 1176px;
            position: absolute;
            top: 235px;
            left: 0px;
        }
        .auto-style4 {
            width: 708px;
            height: 163px;
            position: absolute;
            top: 562px;
            left: 209px;
            z-index: 1;
        }

        </style>


</head>
<body>
   <!-- partial:index.partial.html -->
   
    -->
   
    <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->


  <a class="navbar-brand" href="default.aspx"><img src="Mbabridgelogo.JPG" class="img-thumbnail" alt="Cinque Terre" width="75" height="50"> </a>

  <!-- Links -->
    <ul class="navbar-nav">
  

  <ul class="navbar-nav">
    <li class="nav-item">
      <a class="nav-link" href="autoterm.aspx"> <i class="fas fa-user-slash"></i> AutoTerm </a>
    </li>
   
        <li class="nav-item">
      <a class="nav-link" href="NewHireFix.aspx"> <i class="far fa-address-card"></i> NewHire </a>
    </li>

       <li class="nav-item">
      <a class="nav-link" href="RapidPay.aspx"> <i class="far fa-address-card"></i> RapidPay </a>
    </li>


 <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report"> <i class="far fa-file-alt"></i> Reports   </a>
     
    </li>
      <li class="nav-item">
      <a class="nav-link" href="TDI.aspx">TDI</a>
    </li>

        <li class="nav-item">
      <a class="nav-link" href="https://www.mbahro.com/">MBA</a>
    </li>

    <!-- Dropdown -->
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle" href="#" id="navbardrop" data-toggle="dropdown">
        Other
      </a>
      <div class="dropdown-menu">
        <a class="dropdown-item" href="Bridgetest.aspx">BridgeTest</a>
        <a class="dropdown-item" href="#">Adjust</a>
        <a class="dropdown-item" href="#">Mike Test Page</a>
      </div>
    </li>
  </ul>
        </ul>
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white "><asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>
</nav>  
     <form id="form1" runat="server" class="auto-style3" style="z-index: 1">
       <div class="container">
	<div class="row">
	    <br/>
	  
		
	</div>


       

       
 





    </form>
   
    
</body>
</html>
