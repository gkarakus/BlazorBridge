<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewHireTax.aspx.cs" Inherits="deneme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NEWHIRETax</title>
      <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="Content/bootstrap.min.css">
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
            <script src="Scripts/bootstrap.min.js"></script>
    <style>
        .auto-style2 {
            position: absolute;
            top: 131px;
            left: 16px;
            z-index: 1;
            width: 333px;
            height: 157px;
        }
        .btn1 {
             position: absolute;
            top: 90px;
            left: 21px;
        }
         .btnfname {
             position: absolute;
            top: 125px;
            left: 499px;           
            bottom: 515px;
        }

        .auto-style3 {
            position: absolute;
            top: 120px;
            left: 454px;
            width: 125px;
        }
        .auto-style4 {
            position: absolute;
            top: 122px;
            left: 706px;
            z-index: 1;
            height: 20px;
            width: 114px;
        }

        .auto-style5 {
            position: absolute;
            top: 157px;
            left: 706px;
            z-index: 1;
            width: 140px;
        }
        .auto-style6 {
            position: absolute;
            top: 619px;
            left: 1059px;
            z-index: 1;
            width: 98px;
            height: 31px;
        }
        .auto-style7 {
            left: 376px;
            top: 216px;
            height: 68px;
        }
        .auto-style8 {
            position: absolute;
            top: 122px;
            left: 985px;
            z-index: 1;
            width: 113px;
        }
        .auto-style9 {
            position: absolute;
            top: 158px;
            left: 1132px;
            z-index: 1;
            width: 44px;
        }
        
        .auto-style17 {
            position: absolute;
            top: 157px;
            left: 986px;
            z-index: 1;
            width: 45px;
        }
        .auto-style29 {
            position: absolute;
            top: 125px;
            left: 409px;
            z-index: 1;
        }
        .auto-style30 {
            position: absolute;
            top: 159px;
            left: 384px;
            z-index: 1;
        }
        .auto-style32 {
            position: absolute;
            top: 124px;
            left: 916px;
            z-index: 1;
            width: 62px;
            bottom: 538px;
        }
        .auto-style38 {
            position: absolute;
            top: 160px;
            left: 1072px;
            z-index: 1;
            width: 50px;
        }
        .auto-style40 {
            position: absolute;
            top: 160px;
            left: 920px;
            z-index: 1;
            width: 54px;
        }
        
        .auto-style61 {
            position: absolute;
            top: 60px;
            left: 555px;
            z-index: 1;
            height: 23px;
            width: 288px;
        }

        .auto-style62 {
            position: absolute;
            top: 619px;
            left: 1177px;
            z-index: 1;
            width: 105px;
            height: 31px;
        }

        .auto-style64 {
            position: absolute;
            top: 305px;
            left: 20px;
            z-index: 1;
            width: 57px;
        }
        .auto-style65 {
            position: absolute;
            top: 90px;
            left: 21px;
            width: 315px;
        }

        .auto-style66 {
            position: absolute;
            top: 190px;
            left: 382px;
            z-index: 1;
        }

    </style>


    </head>
    <body style="height: 664px">
     <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
    <ul class="navbar-nav">

          <li class="nav-item" >
      <a class="nav-link" href="NewHireFix.aspx">NewHire Employee</a>
    </li>      
        
   <li class="nav-item" >
      <a class="nav-link" href="NewHireDD.aspx"> DirectDeposit</a>
    </li>      
   
         <li class="nav-item" style="background-color:#808080">
      <a class="nav-link" style="color:white" href="NewHireFix.aspx"> Tax</a>
    </li>     
        

        </ul>
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white "><asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>


    <form id="form1" runat="server">
        <div class="text-left">
        <div>
           <div>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style65"  OnTextChanged="TextBox1_TextChanged"></asp:TextBox>      
           <asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
               AutoPostBack="True" CssClass="auto-style2" Rows="7"></asp:ListBox>                         
           
             </div>
           
        </div>
         <asp:Label ID="Label1" runat="server" BackColor="White" CssClass="Label1" style="z-index: 1; left: 621px; top: 124px; position: absolute; height: 24px; width: 77px; " Text="Firstname"></asp:Label>
            <asp:TextBox ID ="TxtFirstName" runat="server" CssClass="auto-style3" BackColor="White"></asp:TextBox> 
 
       <asp:Label ID="Label2" runat="server" BackColor="White" CssClass="Label2" style="z-index: 1; left: 620px; top: 161px; position: absolute; height: 24px; width: 77px; " Text="LastName"></asp:Label>
       <asp:TextBox ID ="TextBox2" runat="server" CssClass="Label2" style="z-index: 1; left: 456px; top: 155px; position: absolute; width: 120px; "></asp:TextBox> 

         <script type="text/javascript">


        function DoListBoxFilter(listBoxSelector, filter, keys, values) {
            var list = $(listBoxSelector);
            var selectBase = '<option value="{0}">{1}</option>';


            list.empty();
            for (i = 0; i < values.length; ++i) {

                var value = values[i];
                if (value == "" || value.toLowerCase().indexOf(filter.toLowerCase()) >= 0) {
                    var temp = '<option value="'+keys[i]+'">'+value+'</option>' ;
                    list.append(temp);
                }
            }
        }
        var keys=[];
        var values=[];


        var options=$('#<% = ListBox1.ClientID %> option');
        $.each(options, function (index, item) {
            keys.push(item.value);
            values.push(item.innerHTML);
        });


        $('#<% = TextBox1.ClientID %>').keyup(function() {


        var filter = $(this).val();


        DoListBoxFilter('#<% = ListBox1.ClientID %>', filter, keys, values);
   });
    </script>      

           
        <asp:ListBox ID="ListBox2" runat="server" CssClass="auto-style7" OnSelectedIndexChanged="ListBox2_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>           
        <asp:TextBox ID="TextBox3" runat="server" CssClass="auto-style4"></asp:TextBox> 
           
        <asp:TextBox ID="TextBox4" runat="server" CssClass="auto-style5"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" CssClass="auto-style6" Text="Save" OnClick="Button1_Click1" />
        <asp:TextBox ID="TextBox5" runat="server" CssClass="auto-style8"></asp:TextBox>
        <asp:TextBox ID="TextBox6" runat="server" CssClass="auto-style9"></asp:TextBox>

           
            <asp:TextBox ID="TextBox14" runat="server" CssClass="auto-style17"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" CssClass="auto-style29" Text="SSN"></asp:Label>
            <asp:Label ID="Label4" runat="server" CssClass="auto-style30" Text="ClientID"></asp:Label>
            <asp:Label ID="Label5" runat="server" CssClass="auto-style32" Text="Birthday"></asp:Label>
            <asp:Label ID="Label11" runat="server" CssClass="auto-style38" Text="Ethnic"></asp:Label>
            <asp:Label ID="Label13" runat="server" CssClass="auto-style40" Text="Gender"></asp:Label>
        </div>

           
        <strong>
        <asp:Label ID="Label30" runat="server" CssClass="auto-style61" Text="Newhire Employee Tax"></asp:Label>
        </strong>     

           
        <p>
            &nbsp;</p>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style62" OnClick="Button2_Click" Text="Submit" />
           
        <asp:TextBox ID="TextBox30" runat="server" CssClass="auto-style64"></asp:TextBox>

           
        <asp:Label ID="Label31" runat="server" CssClass="auto-style66" Text="Errors / Problems"></asp:Label>

           
    </form>  

</body>
</html>
