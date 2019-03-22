<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XaxisArchive.aspx.cs" Inherits="NocPortal.XaxisArchive" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Xaxis Reports Archive</title>

    <script src="https://code.jquery.com/jquery-1.9.1.min.js"></script>

    <link href="https://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet">
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker3.standalone.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker3.standalone.min.css.map" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.min.js"></script>

    <script>
        $(function () {
            $('#sandbox-container div').datepicker({
                todayBtn: "linked",
                todayHighlight: true,
            }).on("changeDate", function (e) {
                HiddenChosenDate.value = e.format('yyyy-mm-dd');

                //on click, make an ajax call to a service that returns the archived table of the chosen date
                $.ajax({
                    type: "POST",
                    url: "XaxisArchive.aspx/GetTableFromArchive",
                    data: JSON.stringify({ date: HiddenChosenDate.value }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        //alert(JSON.stringify(data));
                        //alert(data.d);
                        if (data.d != "") {
                            TableDiv.innerHTML = data.d;
                        }
                        else {
                            TableDiv.innerHTML = "<h3 style='margin-left: 35%; margin-top:15%; color:white'>Nothing Was Found</h3>"
                        }
                        
                    },
                    fail: function () {
                        alert("Failed to get data from the server!");
                    },
                });
                

            });

            /*
            $('#sandbox-container div').on("changeDate", function () {
                alert($('#sandbox-container').datepicker('getFormattedDate'));
            });*/
        });
    </script>

    <style>
        body {
            background-image: url("blue-abstract-background.jpg");
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
</head>
<body>
    <div style="display:inline-block; width: 40%;">
        <div class="left" style="height:100%;">
        <div class="item">
        <span class="glyphicon glyphicon-th-large"></span>
            Sizmek - Xaxis Archive
        </div>
        <div class="item" onclick="window.location.href = 'http://nocportal/SizmekReports.aspx';" style="cursor:pointer">
        <span class="glyphicon glyphicon-home"></span>
            Home</div>
        <div class="item" onclick="window.location.href = 'http://nocportal/Reports.aspx';" style="cursor:pointer">
        <span class="glyphicon glyphicon-file"></span>
            Xaxis Reports</div>
        <div class="item active" onclick="window.location.href = 'http://nocportal/XaxisArchive.aspx';" style="cursor:pointer">
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

    <div id="sandbox-container" style="background-color:white; display:inline-block;">
        <div></div>
    </div>

    <div class="row">
        <div class="col-md-2"></div>
        <div id="TableDiv" class="col-md-8">

        </div>
        <div class="col-md-2"></div>
    </div>
    
    <form id="form1" runat="server">
        <asp:HiddenField  id="HiddenChosenDate" runat="server" value = "" />

        <asp:ScriptManager ID="ScriptMgr" runat="server" EnablePageMethods="true"></asp:ScriptManager>

    </form>
 
</body>
</html>
