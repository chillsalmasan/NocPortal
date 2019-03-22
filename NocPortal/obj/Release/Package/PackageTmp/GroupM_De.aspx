<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupM_De.aspx.cs" Inherits="NocPortal.XaxisFeeds" %>

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

    <link rel="stylesheet/less" type="text/css" href="styles.less" />
    
    <script src="//cdnjs.cloudflare.com/ajax/libs/less.js/2.7.1/less.min.js"></script>


    <title>Delivery status - GroupM Feeds</title>

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
        body {
            background-image: url("blue-abstract-background.jpg");
        }
        td,th{
            text-align:center;
        }
    </style>


    

    

    <script>
        function filterReady() {
            for (var i = 0; i < tableContent.rows.length; i++) {
                for (var j = 0; j < tableContent.rows[i].cells.length; j++) {
                    if (tableContent.rows[i].cells[j].innerText == "Ready") {
                        if (hiddenFilterReady.value == 0) {
                            tableContent.rows[i].style.display = "none";
                            break;
                        }
                        else {
                            tableContent.rows[i].style.display = "";
                            break;
                        }
                    }
                }
            }
            if (hiddenFilterReady.value == 0) {
                hiddenFilterReady.value = 1;
            }
            else {
                hiddenFilterReady.value = 0;
            }

        }

        function filterDelayed() {
            for (var i = 0; i < tableContent.rows.length; i++) {
                for (var j = 0; j < tableContent.rows[i].cells.length; j++) {
                    if (tableContent.rows[i].cells[j].innerText == "Were delivered with Delay") {
                        if (hiddenFilterDelayed.value == 0) {
                            tableContent.rows[i].style.display = "none";
                            break;
                        }
                        else {
                            tableContent.rows[i].style.display = "";
                            break;
                        }
                    }
                }
            }
            if (hiddenFilterDelayed.value == 0) {
                hiddenFilterDelayed.value = 1;
            }
            else {
                hiddenFilterDelayed.value = 0;
            }

        }

        function filterNotReady() {
            for (var i = 0; i < tableContent.rows.length; i++) {
                for (var j = 0; j < tableContent.rows[i].cells.length; j++) {
                    if (tableContent.rows[i].cells[j].innerText == "Not Ready") {
                        if (hiddenFilterNotReady.value == 0) {
                            tableContent.rows[i].style.display = "none";
                            break;
                        }
                        else {
                            tableContent.rows[i].style.display = "";
                            break;
                        }
                    }
                }
            }
            if (hiddenFilterNotReady.value == 0) {
                hiddenFilterNotReady.value = 1;
            }
            else {
                hiddenFilterNotReady.value = 0;
            }

        }
    </script>
</head>



<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hiddenFilterReady" Value="0" />
        <asp:HiddenField runat="server" ID="hiddenFilterDelayed" Value="0" />
        <asp:HiddenField runat="server" ID="hiddenFilterNotReady" Value="0" />
        <div class="row">
             <div class="col-md-2">
                <div style="display:inline-block">
                    <div class="left" style="height:100%;">
                    <div class="item">
                    <span class="glyphicon glyphicon-th-large"></span>
                        Sizmek - GroupM Feeds
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
                    <div class="item" onclick="window.location.href = 'http://nocportal/XaxisExcel.aspx';" style="cursor:pointer">
                    <span class="glyphicon glyphicon-list-alt"></span>
                        Xaxis Excel</div> 
                    <div class="item active" onclick="window.location.href = 'http://nocportal/GroupM_De.aspx';" style="cursor:pointer">
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
            <div class="col-md-9">
                <div>
                    <h3 style="text-align:center; color:white;">Delivery status - GroupM Feeds</h3>
                </div>

                <div style="display:inline-block;">
                    <label style="color:white;" for="chk">Filter Ready</label>
                    <input type="checkbox" id="chk" name="chk" onchange="filterReady();">
                </div>
                <div style="margin-left:25%; display:inline-block;">
                    <label style="color:white;" for="chk2">Filter Delayed</label>
                    <input type="checkbox" id="chk2" name="chk" onchange="filterDelayed();">
                </div>
                <div style="margin-left:23%; display:inline-block;">
                    <label style="color:white;" for="chk3">Filter Not Ready</label>
                    <input type="checkbox" id="chk3" name="chk" onchange="filterNotReady();">
                </div>

                <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>

                    <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="300000"></asp:Timer>
                    <table id="tableContent" class="table table-hover" style="margin-top:5%;" runat="server"></table>

                </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>


    <script>

        /* jQuery helper funciton to apply 
        /* checkboxes.
    
        /* there's probably something 
        /* that will not work in some 
        /* occasion so it's simple and 
        /* flexible.
    
        /* aim to make it completely 
        /* accessible in future */
        $.fn.chkbox = function () {

            return $(this).each(function (k, v) {

                var $this = $(v);
                if ($this.is(':checkbox') && !$this.data('checkbox-replaced')) {

                    // add some data to this checkbox so we can avoid re-replacing it.
                    $this.data('checkbox-replaced', true);

                    // create HTML for the new checkbox.
                    var $l = $('<label for="' + $this.attr('id') + '" class="chkbox"></label>');
                    var $y = $('<span class="yes">checked</span>');
                    var $n = $('<span class="no">unchecked</span>');
                    var $t = $('<span class="toggle"></span>');

                    // insert the HTML in before the checkbox.
                    $l.append($y, $n, $t).insertBefore($this);
                    $this.addClass('replaced');

                    // check if the checkbox is checked, apply styling. trigger focus.
                    $this.on('change', function () {

                        if ($this.is(':checked')) { $l.addClass('on'); }
                        else { $l.removeClass('on'); }

                        $this.trigger('focus');

                    });

                    $this.on('focus', function () { $l.addClass('focus') });
                    $this.on('blur', function () { $l.removeClass('focus') });

                    // check if the checkbox is checked on init.
                    if ($this.is(':checked')) { $l.addClass('on'); }
                    else { $l.removeClass('on'); }

                }
            });
        };
        $(':checkbox').chkbox();

    </script>
</body>


</html>
