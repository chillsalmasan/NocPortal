<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShiftSummary.aspx.cs" Inherits="NocPortal.ShiftSummary" EnableEventValidation="false" validateRequest="false" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>

    <title>Shift Summary</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous" />
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script> 
      <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
        option.EnlargeOnHover:hover{font-size:16px}
        
        tr.highlighted td {
        background:#8BB9FF;
        color:white;
        font-weight:bold;
          }
    </style>
    <script>
        function AddEntry_Click() {
            if (eventDetails.value == "") {
                alert("Please enter an event first!");
                return;
            }
            else {

            }
        }
    </script>

      <script>
          function Get_Shift_data() {
              //alert("hi");
              var a = '';
             
              var result = PageMethods.sql_query_table(a, function (response)

            {
               // alert('working');
                if(response!= null)
                    document.getElementById('table_container').innerHTML = response;
                    HiddenTable.value = response;
            });
        }
    </script>

     <script>
          function Get_Ids_values() {
              //alert("hi");
              var a = '';
             
              var result = PageMethods.return_lastEventId( function (response)

            {
               // alert('working');
                  if (response != null)
                      HiddenLastEventId.value = response;
                    //alert(HiddenLastEventId.value);
            });
        }
    </script>

         <script>
          function Get_SubIds_values() {
              var a = '';
             
              var result = PageMethods.return_lastSubEventId(function (response)

            {
                  if (response != null)
                      HiddenLastSubId.Value = response;
            });
        }
    </script>

    <script>
        function Set_offLine_table()
        {
            //$('tr').removeClass('highlighted');
            document.getElementById('table_container').innerHTML = HiddenTable.value;
        }
    </script>
    <script>

        var lastSelection = "";
        var lastSelectionClass = "";
        
        $(document).click(function () {
            //alert('clicked outside');
            if (hiddenSelected.value != "") {
                resetBorders(lastSelection);
                hiddenSelected.value = "";
            }
        });

        function resetBorders(item) {
            var nodes = item.childNodes;
            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].nodeName.toLowerCase() == 'td') {
                    nodes[i].style.borderColor = "";
                }
            }
        }



        function onEventSelect(item) {
            if (item.id != SelectedItem.id)
            {
                if(IsDangerSuccess !="")
                {
                    $(SelectedItem).addClass(IsDangerSuccess);
                    IsDangerSuccess = "";
                }
            }

            SelectedItem = item;
            if ($(item).hasClass("danger"))
            {
                IsDangerSuccess = "danger";
                $(item).removeClass('danger');
            }
            if ($(item).hasClass("success")) {
                IsDangerSuccess = "success";
                $(item).removeClass('success');
            }
            if ($(item).hasClass("warning")) {
                IsDangerSuccess = "warning";
                $(item).removeClass('warning');
            }
            // item.style.backgroundColor = "#F5F5F5";
            if ($(item).hasClass("highlighted")) {
                $('tr').removeClass('highlighted');
                if (IsDangerSuccess != "")
                {
                    $(item).addClass(IsDangerSuccess);
                }
                IsDangerSuccess = "";
                hiddenSelected.value = "";
                hiddenSelectedSubEvent.value = "";
                stopEventSelectedTimer();
                HiddenTable.value = document.getElementById('table_container').innerHTML;
                Set_offLine_table();
                resumeTimer();
                return;
            } else
            {
                stopTimer();
                startEventSelectedTimer();
            }
            $('tr').removeClass('highlighted');
            $(item).addClass('highlighted');
            HiddenTable.value = document.getElementById('table_container').innerHTML;
            if (item.id == "eventDetails") {
                //alert('clicked inside');
                event.stopPropagation();
                if (hiddenSelected.value == "") {
                    hiddenSelected.value = "";
                }
                
                return;
            }
            event.stopPropagation();

            if (hiddenSelected.value != "") {
                resetBorders(lastSelection);
            }

            hiddenSelected.value = "-"+item.id;
            var child = item.children;
            for (var i = 0; i < child.length; i++) {
                if (child[i].getAttribute("type") == "eventText") {
                    Hiddentext.value = child[i].innerHTML;
                }
            }

        }


        function onSubEventSelect(item) {
            if (item.id != SelectedItem.id) {
                if (IsDangerSuccess != "") {
                    $(SelectedItem).addClass(IsDangerSuccess);
                    IsDangerSuccess = "";
                }
            }

            SelectedItem = item;

            if ($(item).hasClass("highlighted"))
            {
                $('tr').removeClass('highlighted');
                hiddenSelected.value = "";
                hiddenSelectedSubEvent.value = "";
                resumeTimer();
                stopEventSelectedTimer();
                HiddenTable.value = document.getElementById('table_container').innerHTML;
                return;
            }
            else {
                stopTimer();
                startEventSelectedTimer();
            }

            $('tr').removeClass('highlighted');
            $(item).addClass('highlighted');
            HiddenTable.value = document.getElementById('table_container').innerHTML;
            event.stopPropagation();


            hiddenSelected.value = item.getAttribute("parentID");
            hiddenSelectedSubEvent.value = item.id;

            var child = item.children;
            for (var i = 0; i < child.length; i++) {
                if (child[i].getAttribute("type") == "eventText") {
                    Hiddentext.value = child[i].innerHTML;
                }
            }

        }
        

    </script>
     <script>
        function stopTimer() {
          // alert("stopped");
            var timer = $find("<%=Timer1.ClientID%>");
            timer.set_enabled(true);
            timer._stopTimer();
        }
    </script>

    <script>
        function resumeTimer() {
          //  alert("started");
            var timer = $find("<%=Timer1.ClientID%>");
            timer.set_enabled(false);
            timer._startTimer();
        } 

    </script>
    <script>
         function startEventSelectedTimer() {
           //alert("started selected timer");
            var timer = $find("<%=selectedTimer.ClientID%>");
             timer.set_enabled(false);
             timer._startTimer();

        } 
    </script>

        <script>
         function stopEventSelectedTimer() {
           // alert("stopped selected timer");
            var timer = $find("<%=selectedTimer.ClientID%>");
             timer.set_enabled(true);
             timer._stopTimer();
        } 
    </script>

    <!-- For autocomplete -->
    <link rel="stylesheet" type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1/themes/smoothness/jquery-ui.css">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1/jquery-ui.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.3/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.3/js/select2.min.js"></script>
<%--        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>--%>


    <script>
        $(document).ready(function () { $("#eventsDDL").select2(); }); 
        $(document).ready(function () { $("#actionsDDL").select2(); });
    </script>

       
</head>
<body background="background.jpg" onload="Get_Shift_data()">
    <form runat="server">
        <asp:HiddenField ID="HiddenTable" runat="server" />
        <asp:HiddenField ID="HiddenLastEventId" runat="server" Value="" />
           <asp:HiddenField ID="HiddenLastSubId" runat="server" Value="" />
        <asp:HiddenField runat="server" ID="hiddenSelected" Value="" />
     <asp:HiddenField ID="hiddenSelectedSubEvent" runat="server" Value="" />
        <asp:HiddenField ID="Hiddentext" runat="server" Value=""/>
        <asp:HiddenField ID="IsDangerSuccess" runat="server" Value=""/>
        <asp:HiddenField ID="SelectedItem" runat="server" Value="" />
            <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server"></asp:ScriptManager>

        <div class="row">
            <div id="greetings" class="col-md-4">
                <asp:Label ID="Shift_label" runat="server" Text="" ForeColor="#428bca"></asp:Label>
                <asp:Label ID="currentUsers" runat="server" Text="" ForeColor="#428bca"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4"></div>
            <div class="col-md-4">
                <center>
                <h1 style="font-weight: bold; color: #428BCA;">Shift Summary</h1>
                </center>
            </div>
            <div class="col-md-4">

                </div>

        </div>

        <div class="row">
            <div class="col-md-4"></div>
            <div class="col-md-4">
                <center>
                <p style="font-size: larger; font-weight: bolder; color: #85C0FF">Sizmek | NOC Team</p>
                </center>
            </div>
            <div class="col-md-3">
                <div class="col-md-4" style=" ">
                    <asp:Button ID="PreviousShift" runat="server" Text="Previous Shift" CssClass="btn btn-primary btn-sm" OnClick="PreviousShift_Click"/>
                     </div>
                <div class="col-md-3" style=" padding-right: 50px;">
                    <button type="button" class="btn btn-primary btn-sm">ARCHIVE</button>
                </div>
                <div class="col-md-1"></div>
                </div>
        </div>

        <div class="row" style="padding-top: 30px;">
            <div class="col-md-1"></div>
            <div class="col-md-10">
                <form style="background-color: #E1ECF4; width: 100%">
                    

                    <div class="row">
                        <div class="col-md-1"></div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="eventsDDL" runat="server" OnSelectedIndexChanged="eventsDDL_SelectedIndexChanged" style="width:300px">
                                <asp:ListItem>Choose an Event</asp:ListItem>
                                <%--<asp:ListItem>Greenplum is down</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-1"></div>
                        <div class="col-md-2">
                            <asp:DropDownList ID="actionsDDL" runat="server" OnSelectedIndexChanged="actionsDDL_SelectedIndexChanged" style="width:300px">
                                <asp:ListItem>Choose an Action</asp:ListItem>
                                <asp:ListItem>Called Zohar</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row" style="margin-top:2%;">
                        <div class="col-md-1"></div>
                    <div class="form-group" style="width: 70%; display: inline-block;">
                        <input runat="server" class="form-control" id="eventDetails" placeholder="Enter an event here" >
                    </div>
                    <div style="display: inline-block">
                        <asp:DropDownList ID="callTo" runat="server" Font-Underline="False" Visible="False">
                            <asp:ListItem>IT</asp:ListItem>
                            <asp:ListItem>R&D</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="checkbox" style="display: inline-block; margin-left: 2%;">
                        <label style="font-weight: bold">
                            <asp:CheckBox Checked="false" ID="callExpertBox" runat="server" OnCheckedChanged="callExpertBox_CheckedChanged" AutoPostBack="true" />Call Expert
                        </label>
                    </div>
                   </div>
                   <div class="row">
                       <div class="col-md-1"></div>
                    <div id="Entery_Buttons" style="display: inline-block;">
<%--                    <button type="submit" class="btn btn-default" onclick="AddEntry_Click(); return false;">Add Entry</button>--%>
                        <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Add Entry" OnClick="Button1_Click" />
<%--                        <button type="submit" class="btn btn-primary" runat="server" onclick="Edit_event">Edit Entry</button>--%>
                        <asp:Button ID="Edit_event" runat="server" Text="Edit Entry" CssClass="btn btn-primary" OnClick="Edit_event_Click"/>
<%--                         <button runat="server" type="submit" class="btn btn-primary">Remove Entry</button>--%>
                        <asp:Button ID="Remove_entry" runat="server" Text="Remove Entry" class="btn btn-primary" OnClick="Remove_entry_Click"/>
                    </div>
                    <div style="display: inline-block; margin-left: 33%;">
                        <asp:Button ID="FollowUP" runat="server" OnClick="FollowUP_Click" Text="Follow UP" CssClass="btn btn-warning" />
                        <asp:Button ID="CriticalButton" CssClass="btn btn-danger" runat="server" OnClick="CriticalButton_Click" Text="Critical"/>
                        <asp:Button ID="ResolvedButton" CssClass="btn btn-success" runat="server" OnClick="ResolvedButton_Click" Text="Resolved"/>
                        <asp:Button ID="ResetButton" CssClass="btn btn-default" runat="server" OnClick="ResetButton_Click"  Text="Reset"/>
<%--                        <button type="submit" class="btn btn-default">PIN</button>--%>
                        <asp:Button ID="PinButton" runat="server" OnClick="PinButton_Click" Text="PIN" CssClass="btn btn-default" />
                        
                         </div>
                       </div>
                </form>
                <br>
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <div class="row" style="height:650px; padding-top:20px; ">
                        <div class="col-md-1"></div>
                    
                        <div class="col-md-10" style="height:100%; background-color:white;overflow:auto">
                       
                                <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick">
                                </asp:Timer>
                            <table class="table table-hover" style="border:none;" id="Main_table" >
                                    <tbody id="table_container"> </tbody>
                                
                                    </table>
                               
                                </div>
                    
                    </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            <asp:Timer ID="selectedTimer" runat="server" Enabled="false" OnTick="selectedTimer_Tick" Interval="60000"></asp:Timer>
               <%-- <form style="background-color: #E1ECF4; width: 100%; height: 90%; margin-top:3%;">
                    <select multiple class="form-control" style="height: 650px;">
                        <option class="EnlargeOnHover" style="border-color:brown;">MAKE THE NOC GREAT AGAIN</option>
                    </select>
                </form>--%>
            </div>

        </div>


 
  <!-- Modal -->
  <div class="modal fade" id="PrevShiftModal" role="dialog">
    <div class="modal-dialog" style="width:65%;">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title" id="Prev_shift_Modal_header" runat="server"></h4>
        </div>
        <div class="modal-body" >
             <table class="table table-bordered table-hover">
           <tbody id="table_container_prev_shift" runat="server"> </tbody>
        </table>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-default" data-dismiss="modal" onclick="resumeTimer()">Close</button>
        </div>
      </div>
      
    </div>
  </div>
     <script>
         function openModal_prev_shift() {
           //  alert('hihi');
             $('#PrevShiftModal').modal({ show: true });
         }
        
     </script>


        
    </form>

</body>
</html>


