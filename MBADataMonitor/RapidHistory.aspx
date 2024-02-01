<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RapidHistory.aspx.cs" Inherits="AutoTermLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bridge script test </title>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
  
     <script type="text/javascript">
        document.getElementById("myBtn").addEventListener("click", function () {
         //   alert("Hello World!");
        });
</script>
    


    </head>
<body>
        <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
    <ul class="navbar-nav">
     <li class="nav-item">
      <a class="nav-link"  href="RapidPay.aspx">RapidPay</a>
    </li>         
   
     <li class="nav-item">
      <a class="nav-link" href="Rapidsetup.aspx">Rapid Setup</a>
    </li>         
     <li class="nav-item" style="background-color:#808080">
      <a class="nav-link"style="color:white" href="RapidHistory.aspx">Rapid History</a>
    </li>  

     </ul>
        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>
    
    <form id="form1" runat="server">
 
       
       
       
       
        
 
       
       
    </form>
    
</body>
</html>
