<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="NocPortal.Homepage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> NOC PORTAL</title>
    <style>

        header {
            color: #013471;
            font-size:xx-large;
        }

        .small {
            line-height: 25%;
        }

        p {
        color: #013471;
        
        }

        #mainTable {
            margin-left: 90px
        }

        a:link {
            text-decoration: none;
        }

        a:visited {
            text-decoration: none;
            color: #013471;
        }

        a:hover {
            text-decoration: none;
        }

        a:active {
            text-decoration: none;
        }

         /*#content {
            width: 1200px;
            position: relative;
        }

        #secondary {
            width: 240px;
        }*/

    </style>


</head>
<body background="img/bkg-blu.jpg" id="top">
    <form id="form1" runat="server">
        <div style="width:100%;">
        <header style="font-size:50px"><center>NOC PORTAL</center></header>
            </div>
        <br />
        <br />
        <br />
   
   <div id="content" style="float:left; width:80%;"> 
        <table id="mainTable" style="width:70%; margin-left:35%;">
            <tr>
                <td>
                <div class="container">
                 
                    <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://nocdashboard/(S(mqfbunhzeeaemfde0z4fkrgi))/Display.aspx" target="_blank"> <img src="icons/dashboard.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp NOC DASHBOARD</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp Live status check</p>
                   </div>               
                      <br />
                    <br />   
                </div>
                   
                </td>

                <td>
                    <div class="container">

                    <div class="small">
                    <p id="colors" style="font-size:x-large"> <a href="http://nocportal/ShiftSummary.aspx" target="_blank"> <img src="icons/shift summary.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp SHIFT SUMMARY</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp What happened step by step</p>
                   </div>           
                     <br />
                    <br />
                     </div>

                </td>
            </tr>

            <tr>
                <td>
                      <div class="container">
                 
                              <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://oncall.eyeblaster.com/todays_new.php" target="_blank"> <img src="icons/on call.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp ON CALL</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp Who should I call?</p>
                   </div> 
                     <br />
                    <br />
                </div>

                </td>
                <td>
                     <div class="container">
                 
                         <div class="small">
                    <p id="colors" style="font-size:x-large"> <a href="http://nocnotification.eyeblaster.com" target="_blank"> <img src="icons/NOTIFICATIONs.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp NOTIFICATION SYSTEM</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp Send an email notification</p>
                   </div> 
                      
                    <br />
                    <br />  
                </div>

                </td>

            </tr>

            <tr>
                <td>
                    <div class="container">
                 
                        <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://plannedmaintenance/dashboard.aspx" target="_blank"> <img src="icons/critical issues.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp CRITICAL ISSUES</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp Latest and past critical issues</p>
                   </div> 

                         <br />
                    <br />
                </div>

                </td>

                <td>
                     <div class="container">
                    
                          <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://openticket/" target="_blank"> <img src="icons/open ticket.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp OPEN TICKET</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp Open a ticket for data center ISP</p>
                   </div> 
                    <br />
                    <br />
                        
                </div>

                </td>

            </tr>
            
            <tr>
            <td>

        <div class="container">

               <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://plannedmaintenance.eyeblaster.com" target="_blank"> <img src="icons/planned maintenence.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp PLANNED MAINTENANCE</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp What's down and when</p>
                   </div>  
                      <br />
                    <br />
               
                </div>


            </td>


                <td>
         
                     <div class="container">

               <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://schedulesystem.eyeblaster.com" target="_blank"> <img src="icons/latencey.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp SHIFTS SCHEDULING</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp Schedule the NOC shifts in a click!</p>
                   </div>  
                      <br />
                    <br />
               
                </div>

                </td>

            </tr>

            <tr>

                <td>
                <div class="container">

               <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://sqlmanager/" target="_blank"> <img src="icons/sql.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp SQL MANAGER</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp An easy way to check job's history</p>
                   </div> 

                </td>



                <td>
                <div class="container">

               <div class="small">
                    <p id="colors" style="font-size:x-large"><a href="http://nocportal/SizmekReports.aspx" target="_blank"> <img src="icons/report-3-xxl.png" alt="NocDashboard" width="50" height="44" align="left">&nbsp&nbsp SIZMEK REPORTS</p>
                    <p style="font-size:medium"> &nbsp&nbsp&nbsp&nbsp View the status of the reports</p>
                   </div> 

                </td>


            </tr>


            </table>


    



       </div>
        <br />
 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick"></asp:Timer>

         <div style="float:right; width:20%; margin-right:0px;">
           <marquee direction="UP" onmouseover="this.stop();" onmouseout="this.start();" scrollamount="2" width="100%" height="450" style="margin-right:10px;">

               <asp:Label ID="todayy" runat="server" Text="Label"></asp:Label>
           </marquee>
             <asp:Button ID="Button1" runat="server" Text="Button" style="display:none" />
           </div>
            </ContentTemplate>
            <Triggers> 
            <asp:AsyncPostBackTrigger ControlID="Button1" ></asp:AsyncPostBackTrigger>
        </Triggers>
            </asp:UpdatePanel>

  
    </form>
</body>
</html>
