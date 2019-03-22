<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fraudMask.aspx.cs" Inherits="NocPortal.fraudMask" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Fraud Mask Explanation</title>


    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>


    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
    <!-- Optional theme -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">
    <!-- Latest compiled and minified JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>


    <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous">

    <style>
        body {
            background-image: url("blue-poly-background.jpg");
        }
        td,th{
            text-align:center;
            width:10%;
            font-size:16px;
        }
        th{
            background-color: #337ab7;
            color: white;
        }
        td{
            background-color: rgba(255, 255, 255, .2);
            font-weight: bold;
        }

        .table-hover tbody tr:hover td {
            background: #aeeaf2;
        }

        #animeDiv{
	        visibility: hidden;
        }
        
        .table th, .table td { 
             border-top: none !important; 
         }
    </style>


    <style>
        @import url(https://fonts.googleapis.com/css?family=Open+Sans);

        body{
          font-family: 'Open Sans', sans-serif;
        }

        .search {
          width: 100%;
          position: relative
        }

        .searchTerm {
          float: left;
          width: 100%;
          border: 3px solid #00B4CC;
          padding: 5px;
          //height: 20px;
          border-radius: 5px;
          outline: none;
          color: #9DBFAF;
        }

        .searchTerm:focus{
          color: #00B4CC;
        }

        .searchButton {
          position: absolute;  
          right: -240px;
          width: 110px;
          height: 36px;
          border: 1px solid #00B4CC;
          background: #00B4CC;
          text-align: center;
          color: #fff;
          border-radius: 5px;
          cursor: pointer;
          font-size: 20px;
        }

        /*Resize the wrap to see the search bar change!*/
        .searchWrapper{
          width: 30%;
          position: absolute;
          top: 30%;
          left: 50%;
          transform: translate(-50%, -50%);
        }
    </style>


    <script>
        function explainFraud() {
            var tableRef = document.getElementById('resTable').getElementsByTagName('tbody')[0];
            tableRef.innerHTML = "";
            //var inputFraudAsInt = parseFloat(inputFraudMask.value);
            findFraudMasksInNumber();
            /*
            for (var fraudMask in configsJson) {
                if (configsJson.hasOwnProperty(fraudMask)) {
                    console.log("input: " + inputFraudAsInt + ", mask: " + fraudMask + ", " + ((inputFraudAsInt & fraudMask) == fraudMask));
                    if ((inputFraudAsInt & fraudMask) == fraudMask) {
                        buildTable(fraudMask, configsJson[fraudMask].Name, configsJson[fraudMask].Description);
                    }
                }
            }*/
        }

        function findFraudMasksInNumber() {
            var arr = [];
            var number = inputFraudMask.value;
            if (isNaN(number)) {
                alert("Please enter a valid number!");
                return;
            }
            if (number < 1) {
                alert("The number should be greater than 0!");
                return;
            }
            while (number != 0) {
                if (number % 2 == 0) {
                    arr.push(0);
                }
                else {
                    arr.push(1)
                }
                number = Math.floor(number / 2);
            }
            var count = 0; 
            for (i = 0; i < arr.length; i++) {
                if (arr[i] == 1) {
                    var fraudMask = Math.pow(2, count);
                    //console.log(fraudMask);
                    buildTable(fraudMask, configsJson[fraudMask].Name, configsJson[fraudMask].Description);
                }
                count++;               
            }

            tableDiv.style.display = "";
        }

        function buildTable(fraudMask, name, description) {
            var tableRef = document.getElementById('resTable').getElementsByTagName('tbody')[0];
            // Insert a row in the table at the last row
            var newRow = tableRef.insertRow(tableRef.rows.length);
            var newCell = newRow.insertCell(0);
            newCell.innerText = fraudMask;
            var newCell1 = newRow.insertCell(1);
            newCell1.innerText = name;
            var newCell2 = newRow.insertCell(2);
            newCell2.innerText = description;

        }


        function verifyJSONformat() {
            var jsonString = modalBody.innerText;
            try {
                var o = JSON.parse(jsonString);
                // Handle non-exception-throwing cases:
                // Neither JSON.parse(false) or JSON.parse(1234) throw errors, hence the type-checking,
                // but... JSON.parse(null) returns null, and typeof null === "object", 
                // so we must check for that, too. Thankfully, null is falsey, so this suffices:
                if (o && typeof o === "object") {
                    return;
                }
                else {
                    alert("The text is not in Json format!");
                }
            }
            catch (e) {
                alert("The text is not in Json format!");
            }
            return; 
        }

        function saveJsonConfigs() {
            //alert(modalBody.innerText);
            $.ajax({
                type: "POST",
                url: "fraudMask.aspx/updateConfigurationsJson",
                data: JSON.stringify({ configJson: modalBody.innerText, ivtType: fraudTypeDropDown.value }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    alert("Successfully updated the configurations file!");
                },
                fail: function () {
                    alert("Failed to update the configurations file!");
                },
                async: false
            });

            configsJson = "";
        }

        function reloadPage() {
            window.location.reload(true);
        }

        function changeModalTitle() {
            modalTitle.innerText = fraudTypeDropDown.value + " Configurations";
        }
    </script>

    <script>
        var configsJson;
        function loadJSON(callback) {
            var oldConfigs = 'fraudMaskConfigs.json';
            var givtConfigs = 'assets/fraudMask/GivtMaskConfigs.json';
            var sivtConfigs = 'assets/fraudMask/SivtMaskConfigs.json';
            var configsToLoad = oldConfigs;
            if (fraudTypeDropDown.value == "GIVT") {
                configsToLoad = givtConfigs;
            }
            else if (fraudTypeDropDown.value == "SIVT") {
                configsToLoad = sivtConfigs;
            }
            else if (fraudTypeDropDown.value == "Legacy") {
                configsToLoad = oldConfigs;
            }
            var xobj = new XMLHttpRequest();
            xobj.overrideMimeType("application/json");
            xobj.open('GET', configsToLoad, true);
            xobj.setRequestHeader('cache-control', 'no-cache, must-revalidate, post-check=0, pre-check=0');
            xobj.setRequestHeader('cache-control', 'max-age=0');
            xobj.setRequestHeader('expires', '0');
            xobj.setRequestHeader('expires', 'Tue, 01 Jan 1980 1:00:00 GMT');
            xobj.setRequestHeader('pragma', 'no-cache');
            xobj.onreadystatechange = function () {
                if (xobj.readyState == 4 && xobj.status == "200") {
                    // Required use of an anonymous callback as .open will NOT return a value but simply returns undefined in asynchronous mode
                    callback(xobj.responseText);
                }
            };
            xobj.send(null);
        }

        function init() {
            loadJSON(function (response) {
                configsJson = JSON.parse(response); // Parse JSON string into object
                modalBody.innerText = response;
            });            
        }
    </script>
</head>
<body onload="init()">
    <form id="form1" runat="server">
        <div class="searchWrapper">
           <div class="search">
              <input id="inputFraudMask" type="text" class="searchTerm" placeholder="Please enter a fraudMask...">
               <div class="form-group" style="position:absolute; right: -120px; color:#00B4CC; width: 110px;">
                   <!--<label for="sel1">Type:</label>-->
                  <select class="form-control" id="fraudTypeDropDown" onchange="init(); changeModalTitle();" style="color:white; background-color:#00B4CC; border:1px solid #00B4CC; text-align:center; font-size:18px; padding:1px 6px">
                    <option>GIVT</option>
                    <option>SIVT</option>
                    <option>Legacy</option>
                  </select>
                </div>
              <button type="submit" class="searchButton" onclick="explainFraud(); return false;"> Explain
                <!--<i class="fa fa-search"></i>-->
             </button>
           </div>
        </div>
        <div id="tableDiv" style="position:absolute; top:40%; left:22%; color:#239AB9; max-width:65%; display:none">
            <!--<h1 id="result"></h1>-->
            <table id="resTable" class="table table-hover">
                <thead>
                    <tr>
                        <th>FraudMask</th>
                        <th>Name</th>
                        <th>Description</th>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
        <!-- Button trigger modal -->
        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#myModal" style="margin-left:97%; margin-top:1%;">
          <i class="fa fa-cog" aria-hidden="true"></i>
        </button>

        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
          <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" onclick="changeModalTitle();" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="modalTitle">GIVT Configurations</h4>
              </div>
              <div class="modal-body">
                <div class="form-control" id="modalBody" contenteditable="true" style="height:90%"></div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary" id="saveButton" onclick="verifyJSONformat(); saveJsonConfigs(); init(); reloadPage(); return false;">Save changes</button>
              </div>
            </div>
          </div>
        </div>
    </form>
</body>
</html>
