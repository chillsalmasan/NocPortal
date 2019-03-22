<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XaxisExcel.aspx.cs" Inherits="NocPortal.XaxisExcel" validateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Xaxis - Excel Reports</title>


    <script src="https://code.jquery.com/jquery-1.9.1.min.js"></script>

    <link href="https://netdna.bootstrapcdn.com/bootstrap/3.0.3/css/bootstrap.min.css" rel="stylesheet">
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker3.standalone.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker3.standalone.min.css.map" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.min.js"></script>




    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>



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


    <script>
        function passTable() {
            HiddenField1.value = tableDiv.innerHTML;
        }
    </script>

    <script>
        $(function () {
            $('#sandbox-container .input-daterange').datepicker({
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            })
        });
    </script>

    <script>
        function show_onClick() {

            if ($("#startDate").val() == "" || $("#endDate").val() == "") {
                alert("You must enter a valid date");
                return;
            }

            hiddenStartDate.value = $("#startDate").val();

            hiddenEndDate.value = $("#endDate").val();

            __doPostBack("", "")
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptMgr" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <asp:HiddenField ID ="HiddenField1" runat="server" Value="" />
        <asp:HiddenField ID ="hiddenStartDate" runat="server" Value="" />
        <asp:HiddenField ID ="hiddenEndDate" runat="server" Value="" />

        <div class="row">
           <div class="col-md-2" style="display:inline-block">
                <div class="left" style="height:100%;">
                <div class="item">
                <span class="glyphicon glyphicon-th-large"></span>
                    Sizmek - Xaxis Excel
                </div>
                <div class="item" onclick="window.location.href = 'http://nocportal/SizmekReports.aspx';" style="cursor:pointer">
                <span class="glyphicon glyphicon-home"></span>
                    Home</div>
                <div class="item" onclick="window.location.href = 'http://nocportal/Reports.aspx';" style="cursor:pointer">
                <span class="glyphicon glyphicon-file"></span>
                    Xaxis Reports</div>
                <div class="item" onclick="window.location.href = 'http://nocportal/XaxisArchive.aspx';" style="cursor:pointer">
                <span class="glyphicon glyphicon-calendar"></span>
                    Archive</div>
                <div class="item active" onclick="window.location.href = 'http://nocportal/XaxisExcel.aspx';" style="cursor:pointer">
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
            <div class="col-md-8">
                <h3 style="color:white; margin-left:35%">Xaxis Reports - Excel</h3>
                <div style="height:50px; margin-top:3%; margin-left:20%">
                    <span style="margin-left:1%; color:white;">Data From:</span>
            
                    <div id="sandbox-container" style="display:inline-block; height:20px;">
                        <div class="input-daterange input-group" id="datepicker" style="width:400px">
                            <input type="text"  class="input-sm form-control" id="startDate" name="start" />
                            <span class="input-group-addon">to</span>
                            <input type="text" class="input-sm form-control" id="endDate" name="end" />
                        </div>
                    </div>
                    <button id="showButton" class="btn btn-primary" onclick="show_onClick(); return false;"> Show </button>
                    <div style="margin-left:1%; display:inline-block;">
                        <asp:Button class="btn btn-success" ID="ExportButton" Text="Export" OnClick="ExportOnClick" OnClientClick="passTable();" runat="server" />
                    </div>
                </div>


                <div id="tableDiv">
                    <table class="table" id="tableContent" style="text-align:center; margin-top:15%" runat="server">

                    </table>
                </div>


                
            </div>
        </div>


        <%--<div id="hiddenDiv" style="display:none;" runat="server">
            <asp:PlaceHolder runat="server" ID="PlaceHolder1"></asp:PlaceHolder>
        </div>--%>

    </form>
</body>
</html>
