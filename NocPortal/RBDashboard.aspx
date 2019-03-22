<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RBDashboard.aspx.cs" Inherits="NocPortal.RBDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Builder Dashboard</title>


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
        th{
            background-color: #337ab7;
            color: white;
        }

        #animeDiv{
	        visibility: hidden;
        }
    </style>

    <style>
        /*
        body {
            background: linear-gradient(270deg, #75d2ba, #88d0e1);
            background-size: 400% 400%;
            -webkit-animation: AnimationName 30s ease infinite;
            -moz-animation: AnimationName 30s ease infinite;
            animation: AnimationName 30s ease infinite;
        }

        @-webkit-keyframes AnimationName {
            0%{background-position:0% 50%}
            50%{background-position:100% 50%}
            100%{background-position:0% 50%}
        }
        @-moz-keyframes AnimationName {
            0%{background-position:0% 50%}
            50%{background-position:100% 50%}
            100%{background-position:0% 50%}
        }
        @keyframes AnimationName { 
            0%{background-position:0% 50%}
            50%{background-position:100% 50%}
            100%{background-position:0% 50%}
        }
        */
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


    <link rel="stylesheet" href="css/animations.css">



</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <div class="row">

            <div class="col-md-1" style="display:inline-block; z-index:3;">
                <div class="left" style="height:100%;">
                    <div class="item">
                    <span class="glyphicon glyphicon-th-large"></span>
                        Report Builder Dashboard
                    </div>
                    <div class="item active" onclick="window.location.href = 'http://nocportal/';" style="cursor:pointer">
                    <span class="glyphicon glyphicon-home"></span>
                        Home</div>
                    <div class="item" onclick="window.location.href = 'https://sizmek.atlassian.net/wiki/display/RRB/General+RB+KB+-+Report+Builder+Dashboard';" style="cursor:pointer">
                    <span class="glyphicon glyphicon-list-alt"></span>
                        KB</div>
                    <div class="item" onclick="window.location.href = 'http://oncall/todays_new.php';" style="cursor:pointer">
                    <span class="glyphicon glyphicon-earphone"></span>
                        On Call</div>    
                </div>
            </div>
        
            <div><h3 style="color:white; margin-top:3%;"><center>Report Builder Dashboard</center></h3></div>

            <div class="col-md-10">
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                    <ContentTemplate>
                        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="600000"></asp:Timer>
                        <div style="margin-top:5%;">
                            <div>
                                <div id="animeDiv" class="slideUp" runat="server">
                                    <asp:GridView ID="GridView1" runat="server" GridLines="None" CssClass="table table-hover"></asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </form>
</body>
</html>
