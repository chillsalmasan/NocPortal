<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="GMTReportsReadiness.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>



    <script>
        $(function () {
            setInterval(function () {
                var seconds = new Date().getTime() / 1000;
                var now = new Date();
                var time = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds()),
                  hours = time.getHours(),
                  min = time.getMinutes(),
                  sec = time.getSeconds(),
                  millSec = time.getMilliseconds(),
                  millString = millSec.toString().slice(0, -2),
                  day = time.getDay(),
                  ampm = hours >= 12 ? 'PM' : 'AM',
                  month = time.getMonth(),
                  date = time.getDate(),
                  year = time.getFullYear(),
                  monthShortNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
                    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
                  ];;

                //convert hours from military time and add the am or pm
                if (hours > 11) $('#ampm').text(ampm);
                if (hours > 12) hours = hours % 12;
                if (hours == 0) hours = 12;

                //add leading zero for min and sec 
                if (sec <= 9) sec = "0" + sec;
                if (min <= 9) min = "0" + min;

                $('#hours').text(hours);
                $('#min').text(min);
                $('#sec').text(sec);
                //$("#test").text(day);
                // $('#millSec').text(millString);
                $('.days:nth-child(' + (day + 1) + ')').addClass('active');
                $("#month").text(monthShortNames[month]);
                $('#date').text(date);
                $('#year').text(year);

            }, 100);

        });
    </script>


    <style>
        body {
            background-image: url("blue-abstract-background.jpg");
        }
        td,th{
            text-align:center;
        }



        @import url(https://fonts.googleapis.com/css?family=Orbitron:700,900,500,400);
        #wrapper {
          /*width: 100%;*/
          width: 60%;
          height: 100%
        }

        #wrapper {
          margin: 0;
          padding: 0;
          /*margin-left: 4%;*/
          /*padding-right: 8%;*/
          font-family: 'Orbitron', sans-serif;
          font-weight: 700;
          color: paleturquoise;
          display: flex;
          align-items: center;
          justify-content: center;
        }

        #main {
          width: 100%;
          height: auto;
        }

        #time {
          width: 40%;
          margin: 0 auto;
          text-align: center;
          font-size: 2.5em;
          text-shadow: 0px 2px 25px rgba(144, 244, 253, .6);
        }

        #ampm {
          font-size: .5em;
        }


        #days, #fullDate {
          /*width: 25%;*/
          width: 50%;
          margin: 0 auto;
          display: flex;
          text-align: center;
          align-items: center;
          justify-content: center;
        }

        .days {
          flex: 1;
          color: #444;
          text-align: center;
        }

        .active {
          color: paleturquoise;
          text-shadow: 0px 2px 25px rgba(144, 244, 253, .6);
        }
        #fullDate {
          margin-top:.25em;
          text-shadow: 0px 2px 25px rgba(144, 244, 253, .6);
        }
    </style>

    <style>
        .left{
        float:left;
        height:100vh;
            }
    
        .left {
                background: #337ab7;
                display: inline-block;
                white-space: nowrap;
                width: 50px;
                transition: width 1s ;
            }

   

        .left:hover {
                width: 250px;
            }    
    
        .item:hover {
                /*background-color:#ccc;*/
                background-color:#2685d6;
                }
        
        .left .glyphicon {
                margin:15px;
                width:20px;
                color:#fff;
            }
    

        span.glyphicon.glyphicon-refresh{
            font-size:17px;
            vertical-align: middle !important;
            }
    
        .item {
                height:50px;
                overflow:hidden;
                color:#fff;
            }
    </style>


    <title>GMT Reports Readiness</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField  id="HiddenPredefinedReady" runat="server" value = "0" />
        <asp:HiddenField  id="HiddenOlapReady" runat="server" value = "0" />
        <asp:HiddenField  id="HiddenGroupMReady" runat="server" value = "0" />
        <asp:HiddenField  id="HiddenGroupMReady2" runat="server" value = "0" />
        <asp:HiddenField  id="HiddenDailyFeedReady" runat="server" value = "0" />

        <div class="row">
             <div class="col-md-2">
        <div style="display:inline-block">
            <div class="left" style="height:100%;">
            <div class="item">
            <span class="glyphicon glyphicon-th-large"></span>
                Sizmek - Xaxis Reports
            </div>
            <div class="item" onclick="window.location.href = 'http://nocportal/SizmekReports.aspx';" style="cursor:pointer">
            <span class="glyphicon glyphicon-home"></span>
                Home</div>
            <div class="item active" onclick="window.location.href = 'http://nocportal/Reports.aspx';" style="cursor:pointer">
            <span class="glyphicon glyphicon-file"></span>
                Xaxis Reports</div>
            <div class="item" onclick="window.location.href = 'http://nocportal/XaxisArchive.aspx';" style="cursor:pointer">
            <span class="glyphicon glyphicon-calendar"></span>
                Archive</div>
            <div class="item" onclick="window.location.href = 'http://nocportal/XaxisExcel.aspx';" style="cursor:pointer">
            <span class="glyphicon glyphicon-list-alt"></span>
                Xaxis Excel</div> 
            <div class="item" onclick="window.location.href = 'http://nocportal/GroupM_De.aspx';" style="cursor:pointer">
            <span class="glyphicon glyphicon-align-justify"></span>
                GroupM Feeds</div> 
            <div class="item" onclick="window.location.href = 'http://nocnotification/';" style="cursor:pointer">
            <span class="glyphicon glyphicon-time"></span>
                Delay Notification</div>
            <div class="item" style="cursor:pointer" onclick="window.location.href = 'http://nocportal/';">
            <span class="glyphicon glyphicon-th"></span>
               NOC Portal</div>    
            </div>
        </div>
      </div>
            
       
        
           


            <div class="col-md-8">
                <center>
                <h3 style="color:white">Xaxis Reports Availability</h3>
            </center>
                <center>
        <div id="wrapper">
            <div id="main">
              <div id="time">
                <span id="hours"></span>:<span id="min"></span>:<span id="sec"></span> <span id="ampm"></span>
              </div>
              <div id='days'>
                <div class="days">sun
                </div>
                <div class="days">mon
                </div>
                <div class="days">tue
                </div>
                <div class="days">wed
                </div>
                <div class="days">thu
                </div>
                <div class="days">fri
                </div>
                <div class="days">sat
                </div>
              </div>
              <div id="fullDate">
                <span id="month"></span>&nbsp;<span id="date"></span>&nbsp;<span id="year"></span>
              </div>
            </div>
        </div>
      </center>

  

        <%--<div style="margin: 15px 0px 0px; display: inline-block; text-align: center; width: 200px;"><noscript><div style="display: inline-block; padding: 2px 4px; margin: 0px 0px 5px; border: 1px solid rgb(204, 204, 204); text-align: center; background-color: rgb(255, 255, 255);"><a href="" style="text-decoration: none; font-size: 13px; color: rgb(0, 0, 0);"> </a></div></noscript><script type="text/javascript" src="http://localtimes.info/clock.php?&cp1_Hex=000000&cp2_Hex=FFFFFF&cp3_Hex=000000&fwdt=200&ham=0&hbg=0&hfg=0&sid=0&mon=0&wek=0&wkf=0&sep=0&widget_number=1025"></script></div>--%>
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>

        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="300000"></asp:Timer>
            <%--<asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="5000"></asp:Timer>--%>

        <div id="tableDiv" runat="server">
            <table id="MainTable" class="table table-hover" style="height:250px; margin-top:10%; background-color:white;" runat="server">
              <thead>
                <%--<tr style="background-color:#b69eef;">--%>
                  
                 <tr style="background-color:#337ab7;">
                    <th style="color:white">Reports</th>
                    <th style="color:white">Availability</th>
                    <th style="color:white">Status</th>
                    <th style="color:white">Delay in minutes</th>
                    <th style="color:white">SLA</th>
                    <th style="color:white">Xaxis SLA delay in minutes</th>
                    <th style="color:white">Xaxis SLA</th>
                </tr>
              </thead>
              <tbody>
                  <tr id="PredefinedRow" class="success">
                    <td>Predefined</td>
                    <td id="predefinedReports"></td>
                    <td id="predefinedStatus"></td> 
                    <td id="predefinedDiff"></td> 
                    <td id="predefinedSLA">8:00 AM GMT</td>
                    <td id="XaxisPredefinedDiff"></td>
                    <td>7:30 AM GMT</td> 
                  </tr>
                  <tr id="OlapRow" class="danger">
                    <td>OLAP</td>
                    <td id="olapReports"></td>
                    <td id="olapStatus"></td>
                    <td id="olapDiff"></td> 
                    <td id="olapSLA">9:00 AM GMT</td>
                    <td id="XaxisOlapDiff"></td>
                    <td>7:30 AM GMT</td> 
                  </tr>
                  <tr id="GroupMRow" class="warning">
                    <td>GroupM DE AllAccount GP GMT</td>
                    <td id="GroupMReports"></td>
                    <td id="GroupMStatus"></td>
                    <td id="GroupMDiff"></td>  
                    <td>8:00 AM GMT</td>
                    <td id="XaxisGroupMDiff"></td>
                    <td>7:30 AM GMT</td>
                  </tr>
                  <tr id="GroupMRow2" class="warning">
                    <td>GroupM DE AllAccount WinningEvent GMT</td>
                    <td id="GroupMReports2"></td>
                    <td id="GroupMStatus2"></td>
                    <td id="GroupMDiff2"></td>  
                    <td>8:00 AM GMT</td>
                    <td id="XaxisGroupMDiff2"></td>
                    <td>7:30 AM GMT</td>
                  </tr>
                  <tr id="XaxisDailyRow" class="info">
                    <td>Xaxis DE - Daily feed</td>
                    <td id="XaxisDailyReports"></td>
                    <td id="XaxisDailyStatus"></td>
                    <td id="XaxisDailyDiff"></td> 
                    <td>8:00 AM GMT</td>
                    <td id="XaxisDailyNewSLADiff"></td>
                    <td>7:30 AM GMT</td>
                  </tr>
                </tbody>
            </table>
        </div>
         </div>
      </div>
      
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
