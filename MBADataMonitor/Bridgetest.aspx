<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bridgetest.aspx.cs" Inherits="AutoTermLog" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bridge script test </title>
     <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js"></script>
    <script src="http://dc-js.github.io/dc.js/js/d3.js"></script>
    <script src="http://dc-js.github.io/dc.js/js/crossfilter.js"></script>
    <script src="http://dc-js.github.io/dc.js/js/dc.js"></script>
     <script type="text/javascript">
        document.getElementById("myBtn").addEventListener("click", function () {
            alert("Hello World!");
        });
</script>
    


    </head>
<body>
      <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
  <!-- Brand -->
  <a class="navbar-brand" href="default.aspx">Bridge</a>

  <!-- Links -->
     <ul class="navbar-nav">
     

  </ul>        
       <ul class="navbar-nav ml-auto">
             <li class="nav-item" style="right:auto; color:white ">
                 <asp:Label ID="LabelUserName" runat="server" Text="Label"></asp:Label></li>
        </ul>
</nav>
   
    <h4 onclick="this.innerHTML='Ooops!'">Click on this text!</h4>

      <button id="myBtn">Try it</button>
    
    <div class="container">
<script type="text/javascript" src="header.js"></script>
  <div id="chart-ring-year" style="width:200px; height:230px">
    <div class="reset" style="visibility: hidden;">selected: <span class="filter"></span>
      <a href="javascript:yearRingChart.filterAll();dc.redrawAll();">reset</a>
    </div>
  </div>
  <div id="chart-hist-spend" style="width:300px; height:330px">
    <div class="reset" style="visibility: hidden;">range: <span class="filter"></span>
      <a href="javascript:spendHistChart.filterAll();dc.redrawAll();">reset</a>
    </div>
  </div>
  <div id="chart-row-spenders">
    <div class="reset" style="visibility: hidden;">selected: <span class="filter"></span>
      <a href="javascript:spenderRowChart.filterAll();dc.redrawAll();">reset</a>
    </div>
  </div>
  <!-- not sure why all these styles necessary, not the point of this -->
  <div style="clear: both; margin: 30px; float: left">
    <div id="table"></div>
    <div id="download-type" style="clear: both; float: left">
      <div><label><input type=radio name="operation" value="raw" checked="true">&nbsp;all data</label></div>
      <div><label><input type=radio name="operation" value="table">&nbsp;table data</label></div>
    </div>
    <div style="float: left">
      <button class="btn" id="download">download</button>
    </div>
  </div>








    <form id="form1" runat="server">      
        <script type="text/javascript">
            var text = '{"employees":[' +
                '{"firstName":"John","lastName":"Doe" },' +
                '{"firstName":"Anna","lastName":"Smith" },' +
                '{"firstName":"Peter","lastName":"Jones" }]}';

            obj = JSON.parse(text);
            document.getElementById("demo").innerHTML =
                obj.employees[0].firstName + " " + obj.employees[0].lastName;  


           
</script>

        <script type="text/javascript">
            document.getElementById("myBtn").addEventListener("click", function () {
              
                alert("<%=message1%>" );
            });




            var yearRingChart = new dc.PieChart("#chart-ring-year"),
                spendHistChart = new dc.BarChart("#chart-hist-spend"),
                spenderRowChart = new dc.RowChart("#chart-row-spenders");

            var table = new dc.DataTable('#table');

            // use static or load via d3.csv("spendData.csv").then(function(spendData) {/* do stuff */});
            var spendData = [
                { Name: 'Company A', Spent: '$40', Year: 2022, Tdate: '2020-01-20'},
                { Name: 'Company B', Spent: '$10', Year: 2011, Tdate: '2020-03-20'},
                { Name: 'Company A', Spent: '$40', Year: 2011, Tdate: '2020-02-20'},
                { Name: 'Company B', Spent: '$70', Year: 2012, Tdate: '2020-01-11'},
                { Name: 'Company B', Spent: '$20', Year: 2012, Tdate: '2020-01-15'},
                { Name: 'Company A', Spent: '$50', Year: 2013, Tdate: '2020-02-25'},
                { Name: 'Company C', Spent: '$30', Year: 2013, Tdate: '2020-03-26'}
            ];

            // normalize/parse data
            spendData.forEach(function (d) {
                d.Spent = d.Spent.match(/\d+/)[0];
            });

            // set crossfilter
            var ndx = crossfilter(spendData),
                yearDim = ndx.dimension(function (d) { return +d.Year; }),
                spendPerYear = yearDim.group().reduceSum(function (d) { return +d.Spent; }),

                spendDim = ndx.dimension(function (d) { return Math.floor(d.Spent / 10); }),
                spendHist = spendDim.group().reduceCount();



                nameDim = ndx.dimension(function (d) { return d.Name; }),               
                spendPerName = nameDim.group().reduceSum(function (d) { return +d.Spent; }),
              

            yearRingChart
                .width(200)
                .height(200)
                .dimension(yearDim)
                .group(spendPerYear)
                .innerRadius(25)
                .controlsUseVisibility(true);

            spendHistChart
                .dimension(spendDim)
                .group(spendHist)
                .x(d3.scaleLinear().domain([0, 10]))
                .elasticY(true)
                .controlsUseVisibility(true);

            spendHistChart.xAxis().tickFormat(function (d) { return d * 10 }); // convert back to base unit
            spendHistChart.yAxis().ticks(2);

            spenderRowChart
                .dimension(nameDim)
                .group(spendPerName)
                .elasticX(true)
                .controlsUseVisibility(true);

            var allDollars = ndx.groupAll().reduceSum(function (d) { return +d.Spent; });

            table
                .dimension(spendDim)
                .sortBy(function (d) { return +d.Spent; })
                .showSections(false)
                .columns(['Name',
                    {
                        label: 'Spent',
                        format: function (d) {
                            return '$' + d.Spent;
                        }
                    },
                    'Year',
                    {
                        label: 'Percent of Total',
                        format: function (d) {
                            return Math.floor((d.Spent / allDollars.value()) * 100) + '%';
                        }
                    }]);

            d3.select('#download')
                .on('click', function () {
                    var data = nameDim.top(Infinity);
                    if (d3.select('#download-type input:checked').node().value === 'table') {
                        data = data.sort(function (a, b) {
                            return table.order()(table.sortBy()(a), table.sortBy()(b));
                        });
                        data = data.map(function (d) {
                            var row = {};
                            table.columns().forEach(function (c) {
                                row[table._doColumnHeaderFormat(c)] = table._doColumnValueFormat(c, d);
                            });
                            return row;
                        });
                    }
                    var blob = new Blob([d3.csvFormat(data)], { type: "text/csv;charset=utf-8" });
                    saveAs(blob, 'data.csv');
                });

            dc.renderAll();

</script>



        <div class="text-left">
        <div>
        </div>
        </div>
    </form>

    



</body>
</html>
