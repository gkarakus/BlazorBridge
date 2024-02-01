<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="_Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BridgeMain</title>
    <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
 <style>

  
        #c1 {
            margin: 10px;
            padding: 5px;            
            border-style: solid;
            border-width: 1px;
            position:relative;
            animation-name: kaydirc1;
            animation-duration: 70s;
            animation-iteration-count: infinite;
        }

        @keyframes kaydirc1 {

            0%   { left:0px; top:100% }
            5%  { left:0px; top: 1%;}
            15%  { left:0px; top:-100%;}
            20%  { left:0px; top:-150%;}
            30%  { left:0px; top:-200%;}
            40%  { left:0px; top:-250%;}
            50%  { left:0px; top:-300%;}
            60%  { left:0px; top:-350%;}
            70%  { left:0px; top:-400%;}
            80%  { left:0px; top:-450%;}
            90%  { left:0px; top:-500%;}
            100% { left:0px; top:-550%;}
          }

        img {
            float: left;
            margin: 5px;  
               
            }

           

            a {
                color: #95b600;
                text-decoration: none;
                margin-right: 10px;
                font-size: large;
            }
            
            a:hover {
                color:#00A0C6; 
                text-decoration:none; 
                cursor:pointer;  
            }
       
            #output1 {              
                height: 800px;
               overflow:hidden;
                border: 1px solid #ccc;
               
            }
            #output2 {              
                height: 800px;
                overflow: hidden;
                border: 1px solid #ccc;
            }

            #output3 {              
                height: 800px;
                overflow: hidden;
                border: 1px solid #ccc;
            }



        </style>
    </head>
<body>
    <script>

let url = 'https://newsapi.org/v2/top-headlines?country=us&apiKey=08f7ae6df8e94c2590ad666f5dfc2d2c';
fetch(url)
.then((res) => res.json())
.then((data) => {
let output = '<div id = "c1"><p> US </p>';
let outputend = '</div>';
data.articles.forEach(function(post){
output += `<div>
    <span > <a href =" ${post.url}" target="_blank"> ${post.title} </a>  </span>
      <p><img src="${post.urlToImage}" alt="Pineapple" style="width:110px;height:70px;margin-left:10px;">
      ${post.description}  :  ${post.content} </p>     
    </div>
`;
});
output = output + outputend;
document.getElementById('output1').innerHTML = output;    
})

let url2 = 'https://newsapi.org/v2/everything?q=business&apiKey=08f7ae6df8e94c2590ad666f5dfc2d2c';
fetch(url2)
.then((res) => res.json())
.then((data) => {
    let output = '<div id = "c2"><p> Business </p>';
    let outputend = '</div>';
    data.articles.forEach(function(post){
    output += `<div id="c2">
        <span > <a href =" ${post.url}" target="_blank"> ${post.title} </a>  </span>
          <p><img src="${post.urlToImage}" alt="Pineapple" style="width:110px;height:70px;margin-left:10px;">
          ${post.description}  :  ${post.content} </p>
         
        </div>
    `;
    });
    output = output + outputend;
document.getElementById('output2').innerHTML = output;    
})

    </script>
    <div class="container-fluid sm-3">
        <h3>MBA News  </h3>
       
        <div class="row">
          <div id= "output1" class="col p-3 bg-dark text-white">.col</div>
          <div id = "output2" class="col p-3 bg-dark text-white">.col</div>
          <div id = "output3" class="col p-3 bg-dark text-white">.col</div>
         
        </div>
      </div>

</body>
</html> 