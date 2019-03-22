using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

namespace NocPortal
{
    public partial class XaxisExcel : System.Web.UI.Page
    {

        public int i = 0;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            ExportButton.Visible = false;

            if(hiddenStartDate.Value != "")
            {
                int gotData;

                createTableHeader();


                DateTime startDateTime = DateTime.Parse(hiddenStartDate.Value);
                hiddenStartDate.Value = String.Format("{0:yyyy-MM-dd}", startDateTime);

                DateTime endDateTime = DateTime.Parse(hiddenEndDate.Value);
                hiddenEndDate.Value = String.Format("{0:yyyy-MM-dd}", endDateTime);

                //System.Diagnostics.Debug.WriteLine("hiddenStartDate.Value: " + hiddenStartDate.Value);

                gotData = loadFromArchiveData(); //load the data from the database and build the table

                hiddenStartDate.Value = "";
                hiddenEndDate.Value = "";

                if(gotData == 1)
                {
                    ExportButton.Visible = true;
                }
            }
            
        }


        private int loadFromArchiveData()
        {
            int j = 0;
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "noc" + ";" + "password=" + "noc972" + ";" + "server=" + "ILNOC01" + ";" + "Trusted_Connection=false;" + "database=" + "GMTReportsReadiness" + ";" + "connection timeout=30;");
            conn.Open();
            string stmt5 = "SELECT * FROM ArchiveData WHERE ReportDate >= @startDate AND ReportDate <= @endDate;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            comm4.Parameters.AddWithValue("@startDate", hiddenStartDate.Value);
            comm4.Parameters.AddWithValue("@endDate", hiddenEndDate.Value);
            myReader = comm4.ExecuteReader();
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            Boolean predefinedDelay = false;
            Boolean olapDelay = false;
            Boolean GroupMDelay = false;
            Boolean datafeedDelay = false;
            Boolean predefinedXaxisSLADelay = false;
            Boolean olapXaxisSLADelay = false;
            Boolean GroupMXaxisSLADelay = false;
            Boolean datafeedXaxisSLADelay = false;
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {


                    if (j == 4)
                    {
                        i = i + 1;
                        predefinedDelay = false;
                        olapDelay = false;
                        GroupMDelay = false;
                        datafeedDelay = false;
                        predefinedXaxisSLADelay = false;
                        olapXaxisSLADelay = false;
                        GroupMXaxisSLADelay = false;
                        datafeedXaxisSLADelay = false;
                        j = 0;
                    }

                    if (myReader["ReportType"].ToString() == "Predefined")
                    {
                        System.Diagnostics.Debug.WriteLine("reached");

                        if (myReader["Status"].ToString() != "Ready")
                        {
                            predefinedDelay = true;
                        }

                        if(myReader["XaxisDelay"].ToString() != ""){
                            predefinedXaxisSLADelay = true;
                        }

                        row = new HtmlTableRow();
                        cell = new HtmlTableCell();
                        //row.Cells.Add(cell);
                        //tableContent.Rows.Add(row);

                        cell = new HtmlTableCell();
                        cell.ID = "predefinedReportsDATE" + i;
                        cell.InnerText = myReader["ReportDate"].ToString();
                        cell.BgColor = "#DFF0D8"; //light green
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);

                        cell = new HtmlTableCell();
                        cell.ID = "predefinedReportsAVAIL" + i;
                        cell.BgColor = "#DFF0D8"; //light green
                        cell.InnerText = myReader["Availability"].ToString();
                        if (predefinedDelay)
                        {
                            cell.BgColor = "#e74c3c"; //red
                        }
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);


                    }
                    else if (myReader["ReportType"].ToString() == "OLAP")
                    {
                        if (myReader["Status"].ToString() != "Ready")
                        {
                            olapDelay = true;
                        }
                        if (myReader["XaxisDelay"].ToString() != "")
                        {
                            olapXaxisSLADelay = true;
                        }


                        cell = new HtmlTableCell();
                        cell.ID = "olapReportsAVAIL" + i;
                        cell.InnerText = myReader["Availability"].ToString();
                        cell.BgColor = "#DFF0D8"; //light green
                        if (olapDelay)
                        {
                            cell.BgColor = "#e74c3c"; //red
                        }
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);


                    }
                    else if (myReader["ReportType"].ToString() == "GroupM DE - Xaxis DE")
                    {
                        if (myReader["Status"].ToString() != "Ready")
                        {
                            GroupMDelay = true;
                        }
                        if (myReader["XaxisDelay"].ToString() != "")
                        {
                            GroupMXaxisSLADelay = true;
                        }

                        cell = new HtmlTableCell();
                        cell.ID = "GroupMReportsAVAIL" + i;
                        cell.InnerText = myReader["Availability"].ToString();
                        cell.BgColor = "#DFF0D8"; //light green
                        if (GroupMDelay)
                        {
                            cell.BgColor = "#e74c3c"; //red
                        }
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);
                    }
                    else if (myReader["ReportType"].ToString() == "Xaxis DE - Daily feed")
                    {
                        if (myReader["Status"].ToString() != "Ready")
                        {
                            datafeedDelay = true;
                        }
                        if (myReader["XaxisDelay"].ToString() != "")
                        {
                            datafeedXaxisSLADelay = true;
                        }

                        cell = new HtmlTableCell();
                        cell.ID = "XaxisDailyReportsAVAIL" + i;
                        cell.InnerText = myReader["Availability"].ToString();
                        cell.BgColor = "#DFF0D8"; //light green
                        if (datafeedDelay)
                        {
                            cell.BgColor = "#e74c3c"; //red
                        }
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);
                    }

                    if (j == 3)
                    {
                        if (predefinedDelay || olapDelay || GroupMDelay || datafeedDelay)
                        {
                            cell = new HtmlTableCell();
                            cell.ID = "Status" + i;
                            cell.InnerText = "Delay";
                            cell.BgColor = "#DFF0D8"; //light green
                            row.Cells.Add(cell);
                            tableContent.Rows.Add(row);
                        }
                        else
                        {
                            cell = new HtmlTableCell();
                            cell.ID = "Status" + i;
                            cell.InnerText = "On Time";
                            cell.BgColor = "#DFF0D8"; //light green
                            row.Cells.Add(cell);
                            tableContent.Rows.Add(row);
                        }

                        if (predefinedXaxisSLADelay || olapXaxisSLADelay || GroupMXaxisSLADelay || datafeedXaxisSLADelay)
                        {
                            cell = new HtmlTableCell();
                            cell.ID = "XaxisStatus" + i;
                            cell.InnerText = "Delay";
                            cell.BgColor = "#DFF0D8"; //light green
                            row.Cells.Add(cell);
                            tableContent.Rows.Add(row);
                        }
                        else
                        {
                            cell = new HtmlTableCell();
                            cell.ID = "XaxisStatus" + i;
                            cell.InnerText = "On Time";
                            cell.BgColor = "#DFF0D8"; //light green
                            row.Cells.Add(cell);
                            tableContent.Rows.Add(row);
                        }
                    }

                    j++;
                }
            }
            else
            {
                //myReader doesn't have rows (which means we couldn't find any data in the database)
                myReader.Close();
                conn.Close();
                return 0;
            }

            myReader.Close();
            conn.Close();
            return 1;
        }


        

        /*
        private void loadTablesFromDB()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "noc" + ";" + "password=" + "noc972" + ";" + "server=" + "ILNOC01" + ";" + "Trusted_Connection=false;" + "database=" + "GMTReportsReadiness" + ";" + "connection timeout=30; MultipleActiveResultSets=True;");
            conn.Open();
            string stmt5 = "SELECT TOP 3 * FROM Archive;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            while (myReader.Read())
            {
                hiddenDiv.InnerHtml = "";
                hiddenDiv.InnerHtml = myReader["tableHTML"].ToString();
                //tableHTML = myReader["tableHTML"].ToString();
                
                //PlaceHolder1.Controls.Add(new LiteralControl(myReader["tableHTML"].ToString()));
                
                buildTable();

                insertDataToTable();

                i++;
            }
        }

        private void buildTable()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell();
            cell.ID = "predefinedReportsDATE" + i;
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell();
            cell.ID = "predefinedReportsAVAIL" + i;
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell();
            cell.ID = "olapReportsAVAIL" + i;
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell();
            cell.ID = "GroupMReportsAVAIL" + i;
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell();
            cell.ID = "XaxisDailyReportsAVAIL" + i;
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell();
            cell.ID = "status" + i;
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell();
            cell.ID = "xaxisStatus" + i;
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);
        }


        private void insertDataToTable()
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "MyScript" + i, "fillTable("+i+");", true);
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "MyScript" + i, "fillTable(" + i + ");", true);
        }
        */

        private void createTableHeader()
        {
            HtmlTableRow row = new HtmlTableRow();
            //HtmlTableCell cell = new HtmlTableCell("th");
            //row.Cells.Add(cell);
            //tableContent.Rows.Add(row);

            HtmlTableCell cell = new HtmlTableCell("th");
            cell.InnerText = "Date";
            cell.BgColor = "#337AB7"; //dark blue
            cell.Attributes.Add("style", "color: white;");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Predefined";
            cell.BgColor = "#337AB7"; //dark blue
            cell.Attributes.Add("style", "color: white;");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "OLAP";
            cell.BgColor = "#337AB7"; //dark blue
            cell.Attributes.Add("style", "color: white;");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "GroupM DE - Xaxis DE";
            cell.BgColor = "#337AB7"; //dark blue
            cell.Attributes.Add("style", "color: white;");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Xaxis DE - Daily Feed";
            cell.BgColor = "#337AB7"; //dark blue
            cell.Attributes.Add("style", "color: white;");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Status";
            cell.BgColor = "#337AB7"; //dark blue
            cell.Attributes.Add("style", "color: white;");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Xaxis SLA";
            cell.BgColor = "#337AB7"; //dark blue
            cell.Attributes.Add("style", "color: white;");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);
        }






        protected void ExportOnClick(object sender, EventArgs e)
        {
            
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ReceiptExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(HiddenField1.Value);
            Response.Flush();
            Response.End();
        }




        /*
        protected void ExportToExcel(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[3] {
            new DataColumn("Id"),
            new DataColumn("Name"),
            new DataColumn("Country")});
            dt.Rows.Add(1, "John Hammond", "United States");
            dt.Rows.Add(2, "Mudassar Khan", "India");
            dt.Rows.Add(3, "Suzanne Mathews", "France");
            dt.Rows.Add(4, "Robert Schidner", "Russia");

            StringBuilder sb = new StringBuilder();
            sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
            sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
            sb.Append("<tr><td colspan = '2'></td></tr>");
            sb.Append("<tr><td><b>Order No:</b> 100</td><td><b>Date: </b>" + DateTime.Now + " </td></tr>");
            sb.Append("<tr><td><b>From :</b> " + "Company Name" + " </td><td><b>To: </b>" + " Some Company " + " </td></tr>");
            sb.Append("</table>");

            sb.Append("<table border = '1'>");
            sb.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
                sb.Append(column.ColumnName);
                sb.Append("</th>");
            }
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append("<td>");
                    sb.Append(row[column]);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=ReceiptExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }*/
    }
}