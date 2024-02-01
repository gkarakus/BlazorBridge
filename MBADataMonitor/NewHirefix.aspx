<%@ Page Language="C#"  Debug="true" CodeFile="NewHirefix.aspx.cs" Inherits="deneme" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NEWHIRE </title>
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
            height: 145px;
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
            width: 137px;
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
            top: 641px;
            left: 1059px;
            z-index: 1;
            width: 98px;
            height: 31px;
        }
        .auto-style7 {
            left: 369px;
            top: 205px;
            height: 68px;
            width: 832px;
            position: absolute;
            z-index: 1;
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
        .auto-style10 {
            position: absolute;
            top: 552px;
            left: 531px;
            z-index: 1;
            width: 50px;
        }
        .auto-style11 {
            position: absolute;
            top: 293px;
            left: 531px;
            z-index: 1;
        }
        .auto-style12 {
            position: absolute;
            top: 336px;
            left: 531px;
            z-index: 1;
        }
        .auto-style13 {
            position: absolute;
            top: 419px;
            left: 531px;
            z-index: 1;
            width: 84px;
        }
        .auto-style14 {
            position: absolute;
            top: 376px;
            left: 531px;
            z-index: 1;
            width: 205px;
        }
        .auto-style15 {
            position: absolute;
            top: 419px;
            left: 731px;
            z-index: 1;
            width: 34px;
        }
        .auto-style16 {
            position: absolute;
            top: 293px;
            left: 136px;
            z-index: 1;
            width: 175px;
            right: 898px;
        }

        .auto-style17 {
            position: absolute;
            top: 157px;
            left: 986px;
            z-index: 1;
            width: 45px;
            right: 509px;
        }
        .auto-style18 {
            position: absolute;
            top: 507px;
            left: 136px;
            z-index: 1;
            bottom: 233px;
            width: 75px;
            height: 22px;
        }
        .auto-style19 {
            position: absolute;
            top: 463px;
            left: 531px;
            z-index: 1;
            width: 179px;
        }
        .auto-style20 {
            position: absolute;
            top: 463px;
            left: 136px;
            z-index: 1;
            width: 119px;
        }
        .auto-style21 {
            position: absolute;
            top: 293px;
            left: 1000px;
            z-index: 1;
            width: 182px;
        }
        .auto-style22 {
            position: absolute;
            top: 336px;
            left: 1000px;
            z-index: 1;
            width: 178px;
        }
        .auto-style23 {
            position: absolute;
            top: 376px;
            left: 1000px;
            z-index: 1;
            width: 172px;
        }
        .auto-style24 {
            position: absolute;
            top: 419px;
            left: 1000px;
            z-index: 1;
            width: 119px;
        }
        .auto-style25 {
            position: absolute;
            top: 463px;
            left: 1000px;
            z-index: 1;
            width: 177px;
        }
        .auto-style26 {
            position: absolute;
            top: 552px;
            left: 136px;
            z-index: 1;
            width: 114px;
        }
        .auto-style27 {
            position: absolute;
            top: 507px;
            left: 531px;
            z-index: 1;
            width: 114px;
        }
        .auto-style28 {
            position: absolute;
            top: 507px;
            left: 1000px;
            z-index: 1;
            width: 180px;
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
        .auto-style33 {
            position: absolute;
            top: 294px;
            left: 451px;
            z-index: 1;
            width: 57px;
        }
        .auto-style34 {
            position: absolute;
            top: 339px;
            left: 449px;
            z-index: 1;
            width: 68px;
        }
        .auto-style35 {
            position: absolute;
            top: 424px;
            left: 492px;
            z-index: 1;
            width: 26px;
        }
        .auto-style36 {
            position: absolute;
            top: 380px;
            left: 483px;
            z-index: 1;
            width: 32px;
            bottom: 282px;
        }
        .auto-style37 {
            position: absolute;
            top: 423px;
            left: 685px;
            z-index: 1;
            width: 41px;
        }
        .auto-style38 {
            position: absolute;
            top: 160px;
            left: 1072px;
            z-index: 1;
            width: 50px;
        }
        .auto-style39 {
            position: absolute;
            top: 557px;
            left: 420px;
            z-index: 1;
            width: 96px;
        }
        .auto-style40 {
            position: absolute;
            top: 160px;
            left: 920px;
            z-index: 1;
            width: 54px;
        }
        .auto-style41 {
            position: absolute;
            top: 511px;
            left: 57px;
            z-index: 1;
            width: 69px;
        }
        .auto-style42 {
            position: absolute;
            top: 467px;
            left: 66px;
            z-index: 1;
            width: 61px;
        }
        .auto-style43 {
            position: absolute;
            top: 380px;
            left: 900px;
            z-index: 1;
            width: 98px;
            right: 353px;
        }
        .auto-style44 {
            position: absolute;
            top: 557px;
            left: 29px;
            z-index: 1;
            width: 95px;
        }
        .auto-style45 {
            position: absolute;
            top: 294px;
            left: 908px;
            z-index: 1;
            width: 77px;
        }
        .auto-style46 {
            position: absolute;
            top: 426px;
            left: 918px;
            z-index: 1;
            width: 75px;
        }
        .auto-style47 {
            position: absolute;
            top: 512px;
            left: 419px;
            z-index: 1;
            width: 89px;
        }
        .auto-style48 {
            position: absolute;
            top: 294px;
            left: 32px;
            z-index: 1;
            width: 98px;
            right: 1090px;
        }
        .auto-style49 {
            position: absolute;
            top: 468px;
            left: 410px;
            z-index: 1;
            width: 98px;
        }
        .auto-style50 {
            position: absolute;
            top: 339px;
            left: 904px;
            z-index: 1;
            width: 90px;
        }
        .auto-style51 {
            position: absolute;
            top: 468px;
            left: 934px;
            z-index: 1;
            width: 59px;
        }
        .auto-style52 {
            position: absolute;
            top: 511px;
            left: 924px;
            z-index: 1;
            width: 72px;
        }

        .auto-style53 {
            position: absolute;
            top: 552px;
            left: 1000px;
            z-index: 1;
            width: 80px;
        }
        .auto-style54 {
            position: absolute;
            top: 552px;
            left: 919px;
            z-index: 1;
            width: 76px;
            right: 356px;
        }
        .auto-style56 {
            position: absolute;
            top: 339px;
            left: 35px;
            z-index: 1;
            width: 92px;
            right: 1093px;
        }
        .auto-style57 {
            position: absolute;
            top: 376px;
            left: 136px;
            z-index: 1;
            width: 231px;
        }
        .auto-style58 {
            position: absolute;
            top: 419px;
            left: 136px;
            z-index: 1;
            width: 141px;
        }
        .auto-style59 {
            position: absolute;
            top: 382px;
            left: 78px;
            z-index: 1;
            width: 45px;
            }
        .auto-style60 {
            position: absolute;
            top: 423px;
            left: 33px;
            z-index: 1;
            width: 98px;
        }

        .auto-style61 {
            position: absolute;
            top: 59px;
            left: 475px;
            z-index: 1;
            height: 23px;
            width: 366px;
            font-size: medium;
            background-color: #0033CC;
        }

        .auto-style62 {
            position: absolute;
            top: 643px;
            left: 1177px;
            z-index: 1;
            width: 105px;
            height: 31px;
        }

        .auto-style63 {
            position: absolute;
            top: 645px;
            z-index: 1;
            width: 92px;
            height: 27px;
            left: 137px;
        }
        .auto-style64 {
            position: absolute;
            top: 741px;
            left: 35px;
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
            top: 759px;
            left: 327px;
            z-index: 1;
            width: 670px;
            height: 79px;
        }

        .auto-style67 {
            position: absolute;
            top: 530px;
            left: 716px;
            z-index: 1;
            width: 106px;
            height: 24px;
        }

        .auto-style68 {
            position: absolute;
            top: 720px;
            left: 325px;
            z-index: 1;
            width: 665px;
            right: 277px;
        }
        .auto-style69 {
            position: absolute;
            top: 86px;
            left: 1183px;
            z-index: 1;
            width: 107px;
        }

        .auto-style70 {
            position: absolute;
            top: 184px;
            left: 383px;
            z-index: 1;
            width: 66px;
        }

        .auto-style71 {
            position: absolute;
            top: 595px;
            left: 136px;
            z-index: 1;
            width: 155px;
        }
        .auto-style72 {
            position: absolute;
            top: 599px;
            left: 5px;
            z-index: 1;
        }
        .auto-style73 {
            position: absolute;
            top: 595px;
            left: 531px;
            z-index: 1;
            width: 160px;
        }
        .auto-style74 {
            position: absolute;
            top: 600px;
            left: 413px;
            z-index: 1;
        }

        .auto-style75 {
            position: absolute;
            top: 675px;
            left: 410px;
            z-index: 1;
            width: 106px;
            height: 26px;
            right: 759px;
        }

        .auto-style76 {
            position: absolute;
            top: 332px;
            left: 136px;
            z-index: 1;
            width: 200px;
        }

    </style>


    </head>
 <body style ="background-color: #E9EBEE">
     
      <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
     <ul class="navbar-nav">
    <li class="nav-item" style="background-color:#808080">
      <a class="nav-link" style="color:white" href="NewHireFix.aspx">NewHire Employee </a>
    </li>         
   
     <li class="nav-item">
      <a class="nav-link" href="NewHireDD.aspx">Direct Deposit</a>
    </li>         
     <li class="nav-item">
      <a class="nav-link" href="NewHireTax.aspx">Tax</a>
    </li>  

     </ul>
        <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>
      

</nav>




    <form id="form1" runat="server">
        <div class="text-center">
        <div class="text-left">
        <div>
           <div>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style65"  OnTextChanged="TextBox1_TextChanged"></asp:TextBox>      
           <asp:ListBox ID="ListBox1" runat="server" OnSelectedIndexChanged="ListBox1_SelectedIndexChanged" 
               AutoPostBack="True" CssClass="auto-style2" Rows="7"></asp:ListBox>                         
           
             </div>
           
        </div>
         <asp:Label ID="Label1" runat="server" BackColor="#E9EBEE" CssClass="Label1" style="z-index: 1; left: 621px; top: 124px; position: absolute; height: 24px; width: 77px; " Text="Firstname"></asp:Label>
            <asp:TextBox ID ="TxtFirstName" runat="server" CssClass="auto-style3" BackColor="White"></asp:TextBox> 
 
       <asp:Label ID="Label2" runat="server" CssClass="Label2" style="z-index: 1; left: 620px; top: 161px; position: absolute; height: 24px; width: 77px; " Text="LastName"></asp:Label>
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
        <asp:Button ID="Button1" runat="server" CssClass="auto-style6" BackColor="YellowGreen" Text="Save" OnClick="Button1_Click1" />


        <asp:TextBox ID="TextBox5" runat="server" CssClass="auto-style8"></asp:TextBox>
        <asp:TextBox ID="TextBox6" runat="server" CssClass="auto-style9"></asp:TextBox>
        <asp:TextBox ID="TextBox7" runat="server" CssClass="auto-style10"></asp:TextBox>
        <asp:TextBox ID="TextBox8" runat="server" CssClass="auto-style11"></asp:TextBox>
        <asp:TextBox ID="TextBox9" runat="server" CssClass="auto-style12"></asp:TextBox>
        <asp:TextBox ID="TextBox10" runat="server" CssClass="auto-style13"></asp:TextBox>
        <asp:TextBox ID="TextBox11" runat="server" CssClass="auto-style14"></asp:TextBox>
        <asp:TextBox ID="TextBox12" runat="server" CssClass="auto-style15"></asp:TextBox>
        <asp:TextBox ID="TextBox13" runat="server" CssClass="auto-style16"></asp:TextBox>      

           
            <asp:TextBox ID="TextBox14" runat="server" CssClass="auto-style17"></asp:TextBox>
            <asp:TextBox ID="TextBox15" runat="server" CssClass="auto-style18"></asp:TextBox>
            <asp:TextBox ID="TextBox16" runat="server" CssClass="auto-style19"></asp:TextBox>
            <asp:TextBox ID="TextBox17" runat="server" CssClass="auto-style20"></asp:TextBox>
            <asp:TextBox ID="TextBox18" runat="server" CssClass="auto-style21"></asp:TextBox>
            <asp:TextBox ID="TextBox19" runat="server" CssClass="auto-style22"></asp:TextBox>
            <asp:TextBox ID="TextBox20" runat="server" CssClass="auto-style23"></asp:TextBox>
            <asp:TextBox ID="TextBox21" runat="server" CssClass="auto-style24"></asp:TextBox>
            <asp:TextBox ID="TextBox22" runat="server" CssClass="auto-style25"></asp:TextBox>
            <asp:TextBox ID="TextBox23" runat="server" CssClass="auto-style26"></asp:TextBox>
            <asp:TextBox ID="TextBox24" runat="server" CssClass="auto-style27"></asp:TextBox>
            <asp:TextBox ID="TextBox25" runat="server" CssClass="auto-style28"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" CssClass="auto-style29" Text="SSN"></asp:Label>
            <asp:Label ID="Label4" runat="server" CssClass="auto-style30" Text="ClientID"></asp:Label>
            <asp:Label ID="Label5" runat="server" CssClass="auto-style32" Text="Birthday"></asp:Label>
            <asp:Label ID="Label6" runat="server" CssClass="auto-style33" Text="Address1"></asp:Label>
            <asp:Label ID="Label7" runat="server" CssClass="auto-style34" Text="Address2"></asp:Label>
            <asp:Label ID="Label8" runat="server" CssClass="auto-style35" Text="Zip"></asp:Label>
            <asp:Label ID="Label9" runat="server" CssClass="auto-style36" Text="City"></asp:Label>
            <asp:Label ID="Label10" runat="server" CssClass="auto-style37" Text="State"></asp:Label>
            <asp:Label ID="Label11" runat="server" CssClass="auto-style38" Text="Ethnic"></asp:Label>
            <asp:Label ID="Label12" runat="server" CssClass="auto-style39" Text="MaritalStatus"></asp:Label>
            <asp:Label ID="Label13" runat="server" CssClass="auto-style40" Text="Gender"></asp:Label>
            <asp:Label ID="Label14" runat="server" CssClass="auto-style41" Text="EmpType"></asp:Label>
        </div>
        <asp:Label ID="Label15" runat="server" CssClass="auto-style42" Text="JobCode"></asp:Label>
        <asp:Label ID="Label16" runat="server" CssClass="auto-style43" Text="PEOstartDate"></asp:Label>
        <asp:Label ID="Label17" runat="server" CssClass="auto-style44" Text="FedFileStatus"></asp:Label>
        <asp:Label ID="Label18" runat="server" CssClass="auto-style45" Text="OriginalHire"></asp:Label>
        <asp:Label ID="Label19" runat="server" CssClass="auto-style46" Text="PayMethod"></asp:Label>
        <asp:Label ID="Label20" runat="server" CssClass="auto-style47" Text="BenefitsGroup"></asp:Label>
        <asp:Label ID="Label21" runat="server" CssClass="auto-style48" Text="HomePhone"></asp:Label>
        <asp:Label ID="Label22" runat="server" CssClass="auto-style49" Text="WorkLocation"></asp:Label>
        <asp:Label ID="Label23" runat="server" CssClass="auto-style50" Text="LastHireDate"></asp:Label>
        <asp:Label ID="Label24" runat="server" CssClass="auto-style51" Text="PayRate"></asp:Label>
        <asp:Label ID="Label25" runat="server" CssClass="auto-style52" Text="PayGroup"></asp:Label>
           
        <asp:TextBox ID="TextBox26" runat="server" CssClass="auto-style53"></asp:TextBox>
        <asp:Label ID="Label26" runat="server" CssClass="auto-style54" Text="PayPeriod"></asp:Label>
      
        <asp:Label ID="Label27" runat="server" CssClass="auto-style56" Text="MobilePhone"></asp:Label>
        <asp:TextBox ID="TextBox28" runat="server" CssClass="auto-style57"></asp:TextBox>
        <asp:TextBox ID="TextBox29" runat="server" CssClass="auto-style58"></asp:TextBox>
        <asp:Label ID="Label28" runat="server" CssClass="auto-style59" Text="Email"></asp:Label>
        <asp:Label ID="Label29" runat="server" CssClass="auto-style60" Text="CitizenStatus"></asp:Label>       

           
        <strong>
        <asp:Label ID="Label30" runat="server" CssClass="auto-style61" Text="NewHire / ReHire / CrossHire " Font-Size="Medium" BackColor="#0000CC" Font-Names="Arial" ForeColor="White"></asp:Label>
        </strong>     

           
        <p>
            &nbsp;</p>
        <asp:Button ID="Button2" runat="server" CssClass="auto-style62" OnClick="Button2_Click" Text="Submit" />
           
        <asp:TextBox ID="TextBox30" runat="server" CssClass="auto-style64" Visible="False"></asp:TextBox>

           
        <asp:TextBox ID="Textdene" runat="server" CssClass="auto-style66" TextMode="MultiLine"></asp:TextBox>

           
        <asp:Button ID="Button4" runat="server" CssClass="auto-style67" OnClick="Button4_Click" Text="Test" Visible="False" />

           
        <asp:TextBox ID="TextBoxMessage" runat="server" CssClass="auto-style68"></asp:TextBox>
        <asp:Label ID="Label31" runat="server" BorderStyle="Groove" CssClass="auto-style69" Text="Status"></asp:Label>

           
        <asp:Label ID="Label32" runat="server" CssClass="auto-style70" Text="Errors"></asp:Label>

           
        <asp:TextBox ID="TextBox31" runat="server" CssClass="auto-style71"></asp:TextBox>
        <asp:Label ID="Label33" runat="server" CssClass="auto-style72" Text="DefaultDeptCode"></asp:Label>
        <asp:TextBox ID="TextBox32" runat="server" CssClass="auto-style73"></asp:TextBox>
        <asp:Label ID="Label34" runat="server" CssClass="auto-style74" Text="StandartHours"></asp:Label>

           
        </div>
        <p>
           
        <asp:Button ID="Button3" runat="server" CssClass="auto-style63" OnClick="Button3_Click" Text="Remove" />
        </p>

           
        <asp:Button ID="Button5" runat="server" CssClass="auto-style75" OnClick="Button5_Click" Text="Session" />

           
        <asp:ListBox ID="ListBox2" runat="server" CssClass="auto-style7" OnSelectedIndexChanged="ListBox2_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>           

           
        <asp:TextBox ID="TextBoxMobilePhone" runat="server" CssClass="auto-style76"></asp:TextBox>

           
    </form>  

</body>
</html>
