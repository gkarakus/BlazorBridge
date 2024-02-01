<%@ Page Language="C#"  Debug="true"  AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
    
t>
 <style type="text/css">


     body {
         background-color: #001a1a;
         background: #111;
            }

        .auto-style3 {
            position: absolute;
            top: 103px;
            left: 1025px;
            z-index: 1;
            width: 239px;
            height: 18px;
        }
        .gridview-style4 {
            width: 1400px;
            height: 600px;
            position: absolute;
            overflow:scroll;
            top: 152px;
            left: 24px;
            z-index: 1;
            right: 32px;
                    }

        .griddiv {
               overflow: auto;
                height: 500px;
                 border: solid 1px orange;
                height: 480px;
                width: 1450px;
        }

             



    </style>
    
  <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.5.0/Chart.min.js"></script>

</head>
<body>
      <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
     <ul class="navbar-nav">
    <li class="nav-item" >
      <a class="nav-link" style="color:white" href="autoterm.aspx">AutoTerm</a>
    </li>         
   
     <li class="nav-item">
      <a class="nav-link" href="NewHireFix.aspx">NewHire</a>
    </li>
         
     <li class="nav-item">
      <a class="nav-link" href="RapidPay.aspx"> RapidPay</a>
    </li>  
   <li class="nav-item">
      <a class="nav-link" href="TDI.aspx"> TDI </a>
    </li>  
  <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/browse/"> Reports </a>
    </li>  

    <li class="nav-item">
      <a class="nav-link" href="http://bridge.mbahro.com/"> BridgePHP </a>
    </li>  
     <li class="nav-item">
      <a class="nav-link" href="Courier.aspx"> Courier Manifest </a>
    </li>  
     </ul>
        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>
    <form id="form1" runat="server" class="auto-style3" style="z-index: 1">


    </form>
 
 <canvas id="myChart" style="width:100%;max-width:600px"></canvas>

    <script>
     
        var xValues = ["test1", "Employee", "Deduction", "Gross", "Tax"];
        var yValues = [120, 49, 44, 124, 15];
var barColors = ["red", "green","blue","orange","brown"];

new Chart("myChart", {
  type: "bar",
  data: {
    labels: xValues,
    datasets: [{
      backgroundColor: barColors,
      data: yValues
    }]
  },
  options: {
    legend: {display: false},
    title: {
      display: true,
      text: "Active Client "
    }
  }
});
    </script>


    <div class="container"> 

  <p id="days"> </p>

</div>




    
</body>
</html>
