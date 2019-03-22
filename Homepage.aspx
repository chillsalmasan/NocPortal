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


    </style>


</head>
<body background="img/bkg-blu.jpg" id="top">
    <form id="form1" runat="server">
        
        <header><center>NOC PORTAL</center></header>

        <br />
        <br />
    <div>
    
        <table id="mainTable" style="width:50%">
            <tr>
                <td>
                <div class="container">
                 
                    <div class="small">
                    <p id="colors" style="font-size:large"><a href=#> <img src="icons/dashboard.png" alt="NocDashboard" width="35" height="29" align="left">&nbsp&nbsp NOC DASHBOARD</p>
                    <p style="font-size:small"> &nbsp&nbsp&nbsp Live status check</p>
                   </div>               
                      <br />
                    <br />   
                </div>
                   
                </td>

                <td>
                    <div class="container">

                    <div class="small">
                    <p id="colors" style="font-size:large"> <img src="icons/shift summary.png" alt="NocDashboard" width="35" height="29" align="left">&nbsp&nbsp SHIFT SUMMARY</p>
                    <p style="font-size:small"> &nbsp&nbsp&nbsp What happened step by step</p>
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
                    <p id="colors" style="font-size:large"> <img src="icons/on call.png" alt="NocDashboard" width="35" height="29" align="left">&nbsp&nbsp ON CALL</p>
                    <p style="font-size:small"> &nbsp&nbsp&nbsp Who should I call?</p>
                   </div> 
                     <br />
                    <br />
                </div>

                </td>
                <td>
                     <div class="container">
                 
                         <div class="small">
                    <p id="colors" style="font-size:large"> <img src="icons/NOTIFICATIONs.png" alt="NocDashboard" width="35" height="29" align="left">&nbsp&nbsp NOTIFICATION SYSTEM</p>
                    <p style="font-size:small"> &nbsp&nbsp&nbsp Send an email notification</p>
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
                    <p id="colors" style="font-size:large"> <img src="icons/critical issues.png" alt="NocDashboard" width="35" height="29" align="left">&nbsp&nbsp CRITICAL ISSUES</p>
                    <p style="font-size:small"> &nbsp&nbsp&nbsp Latest and past critical issues</p>
                   </div> 

                         <br />
                    <br />
                </div>

                </td>

                <td>
                     <div class="container">
                    
                          <div class="small">
                    <p id="colors" style="font-size:large"> <img src="icons/open ticket.png" alt="NocDashboard" width="33" height="29" align="left">&nbsp&nbsp OPEN TICKET</p>
                    <p style="font-size:small"> &nbsp&nbsp&nbsp Open a ticket for data center ISP</p>
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
                    <p id="colors" style="font-size:large"> <img src="icons/planned maintenence.png" alt="NocDashboard" width="33" height="33" align="left">&nbsp&nbsp PLANNED MAINTENANCE</p>
                    <p style="font-size:small"> &nbsp&nbsp&nbsp What's down and when</p>
                   </div>  
                      <br />
                    <br />
               
                </div>


            </td>


                <td>
         

                </td>

            </tr>


        <br />

    </div>
    </form>
</body>
</html>
