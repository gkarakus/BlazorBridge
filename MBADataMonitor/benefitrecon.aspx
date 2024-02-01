<%@ Page Language="C#" AutoEventWireup="true" CodeFile="benefitrecon.aspx.cs" Inherits="benefitrecon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Benefit Recon</title>
      <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/normalize/5.0.0/normalize.min.css">
   <link rel="stylesheet" href="Content/style.css">
</head>
<body>

     <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
    <ul class="navbar-nav">
    <li class="nav-item">
      <a class="nav-link" href="https://www.mbahro.com/">MBA</a>
    </li>

  <ul class="navbar-nav">
    <li class="nav-item">
      <a class="nav-link" href="#">BenefitRecon</a>
    </li>

    <li class="nav-item">
      <a class="nav-link" href="#">Client</a>
    </li>
        <li class="nav-item">
      <a class="nav-link" href="#">Carrier</a>
    </li>
 <li class="nav-item">
      <a class="nav-link" href="http://tpaa-pdb-sql01/reports_mbasql/report">Employee</a>
    </li>

    <!-- Dropdown -->
    <li class="nav-item dropdown">
      <a class="nav-link dropdown-toggle" href="#" id="navbardrop" data-toggle="dropdown">
        Benefit detail
      </a>
      <div class="dropdown-menu">
        <a class="dropdown-item" href="#">Carrier Note</a>
        <a class="dropdown-item" href="#">Adjust</a>
        <a class="dropdown-item" href="#">Employee</a>
      </div>
    </li>
  </ul>
</nav>


    <form id="form1" runat="server">
        <div>
            <button type="button" class="btn btn-primary">Primary</button>

        </div>
    </form>
</body>
</html>
