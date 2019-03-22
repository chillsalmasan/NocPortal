<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SizmekReports.aspx.cs" Inherits="NocPortal.SizmekReports" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      
    <title>Sizmek Reports</title>


    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>


    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>


    <style>
        body {
            background-image: url("blue-poly-background.jpg");
        }
        td,th{
            text-align:center;
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



    <style>
        
        @import url(https://fonts.googleapis.com/css?family=Orbitron:700,900,500,400);
        #wrapper,#JSTwrapper, #ESTwrapper {
          /*width: 100%;*/
          width: 60%;
          height: 100%
        }

        #wrapper, #JSTwrapper, #ESTwrapper {
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

        #main,#JSTmain,#ESTmain {
          width: 100%;
          height: auto;
        }

        #time, #JSTtime, #ESTtime {
          width: 40%;
          margin: 0 auto;
          text-align: center;
          font-size: 2.5em;
          text-shadow: 0px 2px 25px rgba(144, 244, 253, .6);
        }

        #ampm, #JSTampm, #ESTampm  {
          font-size: .5em;
        }


        #days, #fullDate, #JSTdays, #JSTfullDate, #ESTdays, #ESTfullDate {
          /*width: 25%;*/
          width: 50%;
          margin: 0 auto;
          display: flex;
          text-align: center;
          align-items: center;
          justify-content: center;
        }

        .days, .JSTdays, .ESTdays {
          flex: 1;
          color: #444;
          text-align: center;
        }

        .active {
          color: paleturquoise;
          text-shadow: 0px 2px 25px rgba(144, 244, 253, .6);
        }
        #fullDate, #JSTfullDate, #ESTfullDate {
          margin-top:.25em;
          text-shadow: 0px 2px 25px rgba(144, 244, 253, .6);
        }
    </style>


    <script>
        //for JST
        $(function () {
            setInterval(function () {
                var seconds = new Date().getTime() / 1000;
                var now = new Date();
                var time = new Date(now.toLocaleString('en-US',{timeZone: 'Asia/Tokyo'})),
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
                if (hours > 11) $('#JSTampm').text(ampm);
                if (hours > 12) hours = hours % 12;
                if (hours == 0) hours = 12;

                //add leading zero for min and sec 
                if (sec <= 9) sec = "0" + sec;
                if (min <= 9) min = "0" + min;

                $('#JSThours').text(hours);
                $('#JSTmin').text(min);
                $('#JSTsec').text(sec);
                //$("#test").text(day);
                // $('#millSec').text(millString);
                $('.JSTdays:nth-child(' + (day + 1) + ')').addClass('active');
                $("#JSTmonth").text(monthShortNames[month]);
                $('#JSTdate').text(date);
                $('#JSTyear').text(year);

            }, 100);

        });
    </script>


    <script>
        //for GMT
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


    <script>
        //for JST
        $(function () {
            setInterval(function () {
                var seconds = new Date().getTime() / 1000;
                var now = new Date();
                var time = new Date(now.toLocaleString('en-US',{timeZone: 'America/New_York'})),
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
                if (hours > 11) $('#ESTampm').text(ampm);
                if (hours > 12) hours = hours % 12;
                if (hours == 0) hours = 12;

                //add leading zero for min and sec 
                if (sec <= 9) sec = "0" + sec;
                if (min <= 9) min = "0" + min;

                $('#ESThours').text(hours);
                $('#ESTmin').text(min);
                $('#ESTsec').text(sec);
                //$("#test").text(day);
                // $('#millSec').text(millString);
                $('.ESTdays:nth-child(' + (day + 1) + ')').addClass('active');
                $("#ESTmonth").text(monthShortNames[month]);
                $('#ESTdate').text(date);
                $('#ESTyear').text(year);

            }, 100);

        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <div class="row">
            <div class="col-md-2" style="display:inline-block; z-index:3;">
                <div class="left" style="height:100%;">
                    <div class="item">
                    <span class="glyphicon glyphicon-th-large"></span>
                        Sizmek Reports
                    </div>
                    <div class="item active" onclick="window.location.href = 'http://nocportal/SizmekReports.aspx';" style="cursor:pointer">
                    <span class="glyphicon glyphicon-home"></span>
                        Home</div>
                    <div class="item" onclick="window.location.href = 'http://nocportal/Reports.aspx';" style="cursor:pointer">
                    <span class="glyphicon glyphicon-list-alt"></span>
                        Xaxis Reports</div>
                    <div class="item" onclick="window.location.href = 'http://nocnotification/';" style="cursor:pointer">
                    <span class="glyphicon glyphicon-time"></span>
                        Delay Notification</div>
                    <div class="item" style="cursor:pointer" onclick="window.location.href = 'http://nocportal/';">
                    <span class="glyphicon glyphicon-th"></span>
                       NOC Portal</div>    
                </div>
            </div>

            <div class="col-md-10">
                <h3 style="color:white; margin-left:30%">Reports Live Status</h3>

                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:Timer ID="Timer2" runat="server" OnTick="Timer1_Tick" Interval="300000"></asp:Timer>
                <div id="JSTwrapper" style="display:inline-block; margin-left:-12%; width:45%">
                    <div id="JSTmain">
                      <div class="days active">JST</div>
                      <div id="JSTtime">
                        <span id="JSThours"></span>:<span id="JSTmin"></span>:<span id="JSTsec"></span> <span id="JSTampm"></span>
                      </div>
                      <div id='JSTdays'>
                        <div class="JSTdays">sun
                        </div>
                        <div class="JSTdays">mon
                        </div>
                        <div class="JSTdays">tue
                        </div>
                        <div class="JSTdays">wed
                        </div>
                        <div class="JSTdays">thu
                        </div>
                        <div class="JSTdays">fri
                        </div>
                        <div class="JSTdays">sat
                        </div>
                      </div>
                      <div id="JSTfullDate">
                        <span id="JSTmonth"></span>&nbsp;<span id="JSTdate"></span>&nbsp;<span id="JSTyear"></span>
                      </div>
                    </div>
                </div>
                <div id="wrapper" style="display:inline-block; margin-left:-18%; width:45%">
                    <div id="main">
                      <div class="days active">GMT</div>
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
                <div id="ESTwrapper" style="display:inline-block; margin-left:-18%; width:45%">
                    <div id="ESTmain">
                        <div class="days active">EST</div>
                        <div id="ESTtime">
                        <span id="ESThours"></span>:<span id="ESTmin"></span>:<span id="ESTsec"></span> <span id="ESTampm"></span>
                        </div>
                        <div id='ESTdays'>
                        <div class="ESTdays">sun
                        </div>
                        <div class="ESTdays">mon
                        </div>
                        <div class="ESTdays">tue
                        </div>
                        <div class="ESTdays">wed
                        </div>
                        <div class="ESTdays">thu
                        </div>
                        <div class="ESTdays">fri
                        </div>
                        <div class="ESTdays">sat
                        </div>
                        </div>
                        <div id="ESTfullDate">
                        <span id="ESTmonth"></span>&nbsp;<span id="ESTdate"></span>&nbsp;<span id="ESTyear"></span>
                        </div>
                    </div>
                </div>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            

            <div class="col-md-8">
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="300000"></asp:Timer>
                
                <div id="Div2" runat="server">
                    <table id="Table2" class="table table-hover" style="height:250px; margin-top:5%; background-color:white;" runat="server">
                      <thead>
                         <tr style="background-color:#337ab7;">
                            <th style="color:white">Timezone</th>
                            <th style="color:white">Reports</th>
                            <th style="color:white">Availability</th>
                            <th style="color:white">Status</th>
                            <th style="color:white">Delay in minutes</th>
                            <th style="color:white">SLA</th>
                        </tr>
                      </thead>
                      <tbody>
                          <tr id="JSTPredefinedRow" class="success">
                            <td>JST</td>
                            <td>Predefined</td>
                            <td id="JSTpredefinedReadiness"></td>
                            <td id="JSTpredefinedStatus"></td> 
                            <td id="JSTpredefinedDiff"></td> 
                            <td id="JSTpredefinedSLAcell">8:00 AM JST</td>
                          </tr>
                          <tr id="JSTOlapRow" class="danger">
                            <td>JST</td>
                            <td>OLAP</td>
                            <td id="JSTolapReadiness"></td>
                            <td id="JSTolapStatus"></td>
                            <td id="JSTolapDiff"></td> 
                            <td id="JSTolapSLAcell">9:30 AM JST</td>
                          </tr>
                          <tr id="JSTCLDRow" class="info">
                            <td>JST</td>
                            <td>CLD</td>
                            <td id="JSTCLDReadiness"></td>
                            <td id="JSTCLDStatus"></td>
                            <td id="JSTCLDDiff"></td> 
                            <td id="JSTCLDSLAcell">9:00 AM JST</td>
                          </tr>
                          <tr id="JSTARBRow" class="info">
                            <td>JST</td>
                            <td>ARB/SEM</td>
                            <td id="JSTARBReadiness"></td>
                            <td id="JSTARBStatus"></td>
                            <td id="JSTARBDiff"></td> 
                            <td id="JSTARBSLAcell">9:00 AM JST</td>
                          </tr>
                        </tbody>
                    </table>
                </div>
                <div id="Div3" runat="server">
                    <table id="Table3" class="table table-hover" style="height:250px; margin-top:4%; background-color:white;" runat="server">
                      <thead>
                         <tr style="background-color:#337ab7;">
                            <th style="color:white">Timezone</th>
                            <th style="color:white">Reports</th>
                            <th style="color:white">Availability</th>
                            <th style="color:white">Status</th>
                            <th style="color:white">Delay in minutes</th>
                            <th style="color:white">SLA</th>
                        </tr>
                      </thead>
                      <tbody>
                          <tr id="AESTPredefinedRow" class="success">
                            <td>AEST</td>
                            <td>Predefined</td>
                            <td id="AESTpredefinedReadiness"></td>
                            <td id="AESTpredefinedStatus"></td> 
                            <td id="AESTpredefinedDiff"></td> 
                            <td id="AESTpredefinedSLAcell">9:00 AM AEST</td>
                          </tr>
                          <tr id="AESTOlapRow" class="danger">
                            <td>AEST</td>
                            <td>OLAP</td>
                            <td id="AESTolapReadiness"></td>
                            <td id="AESTolapStatus"></td>
                            <td id="AESTolapDiff"></td> 
                            <td id="AESTolapSLAcell">10:30 AM AEST</td>
                          </tr>
                          <tr id="AESTCLDRow" class="info">
                            <td>AEST</td>
                            <td>CLD</td>
                            <td id="AESTCLDReadiness"></td>
                            <td id="AESTCLDStatus"></td>
                            <td id="AESTCLDDiff"></td> 
                            <td id="AESTCLDSLAcell">9:00 AM AEST</td>
                          </tr>
                          <tr id="AESTARBRow" class="info">
                            <td>AEST</td>
                            <td>ARB/SEM</td>
                            <td id="AESTARBReadiness"></td>
                            <td id="AESTARBStatus"></td>
                            <td id="AESTARBDiff"></td> 
                            <td id="AESTARBSLAcell">9:00 AM AEST</td>
                          </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tableDiv" runat="server">
                    <table id="MainTable" class="table table-hover" style="height:250px; margin-top:4%; background-color:white;" runat="server">
                      <thead>
                         <tr style="background-color:#337ab7;">
                            <th style="color:white">Timezone</th>
                            <th style="color:white">Reports</th>
                            <th style="color:white">Availability</th>
                            <th style="color:white">Status</th>
                            <th style="color:white">Delay in minutes</th>
                            <th style="color:white">SLA</th>
                        </tr>
                      </thead>
                      <tbody>
                          <tr id="GMTPredefinedRow" class="success">
                            <td>GMT</td>
                            <td>Predefined</td>
                            <td id="GMTpredefinedReadiness"></td>
                            <td id="GMTpredefinedStatus"></td> 
                            <td id="GMTpredefinedDiff"></td> 
                            <td id="GMTpredefinedSLAcell">8:00 AM GMT</td>
                          </tr>
                          <tr id="GMTOlapRow" class="danger">
                            <td>GMT</td>
                            <td>OLAP</td>
                            <td id="GMTolapReadiness"></td>
                            <td id="GMTolapStatus"></td>
                            <td id="GMTolapDiff"></td> 
                            <td id="GMTolapSLAcell">9:00 AM GMT</td>
                          </tr>
                          <tr id="GMTCLDRow" class="info">
                            <td>GMT</td>
                            <td>CLD</td>
                            <td id="GMTCLDReadiness"></td>
                            <td id="GMTCLDStatus"></td>
                            <td id="GMTCLDDiff"></td> 
                            <td id="GMTCLDSLAcell">9:00 AM GMT</td>
                          </tr>
                          <tr id="GMTARBRow" class="info">
                            <td>GMT</td>
                            <td>ARB/SEM</td>
                            <td id="GMTXaxisDailyReadiness"></td>
                            <td id="GMTXaxisDailyStatus"></td>
                            <td id="GMTARBDiff"></td> 
                            <td id="GMTARBSLAcell">9:00 AM GMT</td>
                          </tr>
                        </tbody>
                    </table>
                </div>
                <div id="Div1" runat="server">
                    <table id="Table1" class="table table-hover" style="height:250px; margin-top:4%; background-color:white;" runat="server">
                      <thead>
                         <tr style="background-color:#337ab7;">
                            <th style="color:white">Timezone</th>
                            <th style="color:white">Reports</th>
                            <th style="color:white">Availability</th>
                            <th style="color:white">Status</th>
                            <th style="color:white">Delay in minutes</th>
                            <th style="color:white">SLA</th>
                        </tr>
                      </thead>
                      <tbody>
                          <tr id="ESTPredefinedRow" class="success">
                            <td>EST</td>
                            <td>Predefined</td>
                            <td id="ESTpredefinedReadiness"></td>
                            <td id="ESTpredefinedStatus"></td> 
                            <td id="ESTpredefinedDiff"></td> 
                            <td id="ESTpredefinedSLAcell">8:00 AM EST</td>
                          </tr>
                          <tr id="ESTOlapRow" class="danger">
                            <td>EST</td>
                            <td>OLAP</td>
                            <td id="ESTolapReadiness"></td>
                            <td id="ESTolapStatus"></td>
                            <td id="ESTolapDiff"></td> 
                            <td id="ESTolapSLAcell">9:00 AM EST</td>
                          </tr>
                          <tr id="ESTCLDRow" class="info">
                            <td>EST</td>
                            <td>CLD</td>
                            <td id="ESTCLDReadiness"></td>
                            <td id="ESTCLDStatus"></td>
                            <td id="ESTCLDDiff"></td> 
                            <td id="ESTCLDSLAcell">9:00 AM EST</td>
                          </tr>
                          <tr id="ESTARBRow" class="info">
                            <td>EST</td>
                            <td>ARB/SEM</td>
                            <td id="ESTARBReadiness"></td>
                            <td id="ESTARBStatus"></td>
                            <td id="ESTARBDiff"></td> 
                            <td id="ESTARBSLAcell">9:00 AM EST</td>
                          </tr>
                        </tbody>
                    </table>
                </div>
                
                <div style="color:white; margin-left:80%;">by Muhammad Kaiyal - NOC Team</div>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
