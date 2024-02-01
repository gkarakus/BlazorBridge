<%@ Page Language="C#" Debug="true" AutoEventWireup="true" CodeFile="NewHireDD.aspx.cs" Inherits="deneme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NEWHIREDirectDeposit</title>
      <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="Content/bootstrap.min.css">
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
            <script src="Scripts/bootstrap.min.js"></script>
    <style>
        .auto-style2 {
            position: absolute;
            top: 134px;
            left: 12px;
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
            width: 152px;
        }

        .auto-style5 {
            position: absolute;
            top: 157px;
            left: 706px;
            z-index: 1;
            width: 149px;
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
            left: 375px;
            top: 223px;
            height: 59px;
            position: absolute;
            z-index: 1;
            width: 696px;
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
            top: 122px;
            left: 1039px;
            z-index: 1;
            width: 115px;
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

        .auto-style67 {
            position: absolute;
            top: 496px;
            left: 297px;
            z-index: 1;
        }
        .auto-style68 {
            position: absolute;
            top: 326px;
            left: 489px;
            z-index: 1;
            width: 199px;
        }
        .auto-style70 {
            position: absolute;
            top: 330px;
            left: 754px;
            z-index: 1;
            width: 122px;
        }
        .auto-style71 {
            position: absolute;
            top: 382px;
            left: 349px;
            z-index: 1;
            width: 104px;
            bottom: 286px;
            right: 674px;
        }
        .auto-style72 {
            position: absolute;
            top: 382px;
            left: 645px;
            z-index: 1;
            width: 62px;
        }
        .auto-style73 {
            position: absolute;
            top: 442px;
            left: 356px;
            z-index: 1;
            width: 55px;
        }
        .auto-style74 {
            position: absolute;
            top: 444px;
            left: 774px;
            z-index: 1;
            width: 49px;
        }
        .auto-style75 {
            position: absolute;
            top: 384px;
            left: 887px;
            z-index: 1;
            width: 50px;
            margin-top: 0px;
        }
        .auto-style76 {
            position: absolute;
            top: 327px;
            left: 920px;
            z-index: 1;
        }
        .auto-style77 {
            position: absolute;
            top: 380px;
            z-index: 1;
            width: 48px;
            right: 588px;
        }
        .auto-style78 {
            position: absolute;
            top: 380px;
            left: 709px;
            z-index: 1;
            width: 56px;
        }
        .auto-style79 {
            position: absolute;
            top: 439px;
            left: 474px;
            z-index: 1;
            width: 173px;
        }
        .auto-style80 {
            position: absolute;
            top: 441px;
            left: 834px;
            z-index: 1;
            width: 143px;
        }
        .auto-style81 {
            position: absolute;
            top: 381px;
            left: 957px;
            z-index: 1;
            width: 93px;
        }
        .auto-style82 {
            position: absolute;
            top: 329px;
            left: 350px;
            z-index: 1;
            width: 124px;
        }

        .auto-style83 {
            position: absolute;
            top: 126px;
            left: 960px;
            z-index: 1;
            width: 66px;
        }
        .auto-style84 {
            position: absolute;
            top: 157px;
            left: 1038px;
            z-index: 1;
            width: 94px;
        }
        .auto-style85 {
            position: absolute;
            top: 165px;
            left: 933px;
            z-index: 1;
            width: 91px;
        }

        .auto-style86 {
            position: absolute;
            top: 555px;
            left: 989px;
            z-index: 1;
            height: 27px;
            width: 249px;
        }
        .auto-style87 {
            position: absolute;
            top: 549px;
            left: 290px;
            z-index: 1;
            width: 621px;
            height: 61px;
        }

        .auto-style88 {
            position: absolute;
            top: 318px;
            left: 21px;
            z-index: 1;
            width: 131px;
            height: 34px;
        }

    </style>


    </head>
  <body style ="background-color: #E9EBEE">


    <form id="form1" runat="server">
        <div class="text-right">
     <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
         <asp:TextBox ID="TextBox31" runat="server" CssClass="auto-style84"></asp:TextBox>
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
    <ul class="navbar-nav">

          <li class="nav-item" >
      <a class="nav-link" href="NewHireFix.aspx">NewHire Employee</a>
    </li>     

   <li class="nav-item" style="background-color:#808080">
      <a class="nav-link" style="color:white" href="NewHireFix.aspx"> DirectDeposit</a>
    </li>        
 
        <li class="nav-item">
      <a class="nav-link" href="NewHireTax.aspx"> Tax</a>
    </li>





        </ul>
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white "><asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>

</nav>


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

           
        <asp:TextBox ID="TextBox3" runat="server" CssClass="auto-style4"></asp:TextBox> 
           
        <asp:TextBox ID="TextBox4" runat="server" CssClass="auto-style5"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" CssClass="auto-style6" Text="Save" OnClick="Button1_Click1" />
            <asp:Label ID="Label3" runat="server" CssClass="auto-style29" Text="SSN"></asp:Label>
            <asp:Label ID="Label4" runat="server" CssClass="auto-style30" Text="ClientID"></asp:Label>
           
        </div>

           
        <strong>
        <asp:Label ID="Label30" runat="server" CssClass="auto-style61" Text="Newhire Employee Direct Deposit"></asp:Label>
        </strong>     

           
        <p>
            &nbsp;</p>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style62" OnClick="Button2_Click" Text="Submit" />
           
           
        <asp:Label ID="Label31" runat="server" CssClass="auto-style66" Text="Errors / Problems"></asp:Label>

           
            <asp:TextBox ID="TextBox32" runat="server" CssClass="auto-style68"></asp:TextBox>
            <asp:Label ID="Label33" runat="server" CssClass="auto-style70" Text="Account Number"></asp:Label>
            <asp:Label ID="Label34" runat="server" CssClass="auto-style71" Text="Account Type"></asp:Label>
            <asp:Label ID="Label35" runat="server" CssClass="auto-style72" Text="Method"></asp:Label>
            <asp:Label ID="Label36" runat="server" CssClass="auto-style73" Text="Amount"></asp:Label>
            <asp:Label ID="Label37" runat="server" CssClass="auto-style74" Text="Limit"></asp:Label>
            <asp:Label ID="Label38" runat="server" CssClass="auto-style75" Text="Status"></asp:Label>
            <asp:TextBox ID="TextBox33" runat="server" CssClass="auto-style76"></asp:TextBox>
            <asp:TextBox ID="TextBox34" runat="server" CssClass="auto-style77" MaxLength="1"></asp:TextBox>
            <asp:TextBox ID="TextBox35" runat="server" CssClass="auto-style78" MaxLength="1"></asp:TextBox>
        </div>
        <asp:TextBox ID="TextBox36" runat="server" CssClass="auto-style79"></asp:TextBox>
        <asp:TextBox ID="TextBox37" runat="server" CssClass="auto-style80"></asp:TextBox>
        <asp:TextBox ID="TextBox38" runat="server" CssClass="auto-style81"></asp:TextBox>
        <asp:Label ID="Label39" runat="server" CssClass="auto-style82" Text="Routing Number"></asp:Label>

           
        <p>
           
        <asp:TextBox ID="TextBox30" runat="server" CssClass="auto-style64"></asp:TextBox>

           
         <asp:Label ID="Label40" runat="server" BackColor="White" CssClass="Label1" style="z-index: 1; left: 621px; top: 124px; position: absolute; height: 24px; width: 77px; " Text="Firstname"></asp:Label>

           
        <asp:ListBox ID="ListBox2" runat="server" CssClass="auto-style7" OnSelectedIndexChanged="ListBox2_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>           
            </p>
        <asp:Label ID="Label41" runat="server" CssClass="auto-style83" Text="PayType"></asp:Label>
        <asp:TextBox ID="TextBox39" runat="server" CssClass="auto-style67"></asp:TextBox>
        <asp:Label ID="Label42" runat="server" CssClass="auto-style85" Text="EmployeeID"></asp:Label>

           
        <p>
            &nbsp;</p>
        <asp:Button ID="Button3" runat="server" CssClass="auto-style86" OnClick="Button3_Click1" Text="Button" Visible="False" />
        <asp:TextBox ID="testmsg" runat="server" CssClass="auto-style87" TextMode="MultiLine" Visible="True"></asp:TextBox>

           
        <asp:Button ID="BtnRemove" runat="server" CssClass="auto-style88" OnClick="BtnRemove_Click" Text="Remove" />

           
    </form>  

</body>
</html>
