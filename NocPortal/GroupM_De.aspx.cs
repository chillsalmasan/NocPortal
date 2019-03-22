using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NocPortal
{
    public partial class XaxisFeeds : System.Web.UI.Page
    {
        DateTime GMTdatafeedSLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "09:30:00");
        DateTime Xaxis730SLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "07:30:00");

        protected void Page_Load(object sender, EventArgs e)
        {
            createTableLayout();
            checkDatafeed();

            bool ready = checkIfAllReportsReady();

            if (ready)
            {
                saveData();
            }

            //System.Diagnostics.Debug.WriteLine("ready: " + ready);
        }


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private bool checkIfAllReportsReady()
        {
            bool foundReady = false;

            for(int i=1; i<tableContent.Rows.Count; i++)
            {
                foundReady = false;

                for (int j=0; j<tableContent.Rows[i].Cells.Count; j++)
                {
                    if(tableContent.Rows[i].Cells[j].InnerText == "Ready" || tableContent.Rows[i].Cells[j].InnerText == "Were delivered with Delay")
                    {
                        foundReady = true;
                        break;
                    }
                }

                if (foundReady == false)
                {
                    return false;
                }
            }

            return true;
        }

        private void createTableLayout()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell("th");
            cell.InnerText = "Report Name";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Report TZ";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Availability";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Status";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "Delay in minutes";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "SLA";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "GroupM SLA delay in minutes";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            cell = new HtmlTableCell("th");
            cell.InnerText = "GroupM SLA";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            //row.BgColor = "#337ab7";
            row.Attributes.Add("class", "");
            row.Style.Add("background-color", "#337ab7");
        }

        private void checkDatafeed()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "readonlyuser" + ";" + "password=" + "rou123" + ";" + "server=" + "RSDWHNJ" + ";" + "Trusted_Connection=false;" + "database=" + "Reportsdwh" + ";" + "connection timeout=30");
            conn.Open();
            //string stmt5 = "SELECT FeedName, MAX(AvailabeSince) AS AvailabeSince ,MAX(LastDateAvailabe) AS LastDateAvailabe FROM dbo.DataFeeds_Status_ByAccount_Detailed WHERE (FeedName LIKE '%Groupm DE%' or FeedName LIKE '%Xaxis DE%') AND LastDateAvailabe < GetDate() GROUP BY FeedName;";
            string stmt5 = "SELECT FeedName, MAX(AvailabeSince) AS AvailabeSince ,MAX(LastDateAvailabe) AS LastDateAvailabe, DefaultTimeZone FROM dbo.DataFeeds_Status_ByAccount_Detailed inner join dbo.Accounts on dbo.DataFeeds_Status_ByAccount_Detailed.AccountID = dbo.Accounts.AccountID WHERE (FeedName LIKE '%Groupm DE%' or FeedName LIKE '%Xaxis DE%') AND LastDateAvailabe < GetDate() Group by FeedName,DefaultTimeZone;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            while (myReader.Read())
            {
                HtmlTableRow row = new HtmlTableRow();
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = myReader["FeedName"].ToString();
                row.Cells.Add(cell);
                tableContent.Rows.Add(row);

                cell = new HtmlTableCell();
                string timezoneID = myReader["DefaultTimeZone"].ToString();
                if(timezoneID == "8")
                {
                    cell.InnerText = "EST";
                }
                else if(timezoneID == "13")
                {
                    cell.InnerText = "GMT";
                }
                else if (timezoneID == "22")
                {
                    cell.InnerText = "JST";
                }
                else if (timezoneID == "23")
                {
                    cell.InnerText = "AEST";
                }
                else
                {
                    cell.InnerText = timezoneID;
                }

                row.Cells.Add(cell);
                tableContent.Rows.Add(row);

                DateTime GMTdatafeed = DateTime.Parse(myReader["AvailabeSince"].ToString());

                DateTime datafeedDate = DateTime.Parse(myReader["AvailabeSince"].ToString().Split(' ')[0]);

                //find out what time it is in EST
                var timeUtc = DateTime.UtcNow;
                TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

                //time in gmt
                DateTime utc = DateTime.UtcNow;

                //find the difference between them
                TimeSpan GMTandESTdiff = utc.Subtract(easternTime);


                DateTime datafeedTimeInGMT = DateTime.Parse(myReader["AvailabeSince"].ToString()).AddHours(Math.Abs(GMTandESTdiff.TotalHours));


                cell = new HtmlTableCell();
                cell.InnerText = datafeedTimeInGMT.ToString("dd/MM/yyyy HH:mm:ss");
                row.Cells.Add(cell);
                tableContent.Rows.Add(row);

                row.Attributes.Add("class", "info");


                double GMTSLAdiff = utc.Subtract(GMTdatafeedSLA).TotalMinutes;

                TimeSpan GMTdatafeedDiff = datafeedTimeInGMT.Subtract(GMTdatafeedSLA);

                if (DateTime.Parse(datafeedTimeInGMT.ToString().Split(' ')[0]) != DateTime.Today) //if the reports are not ready yet
                {
                    cell = new HtmlTableCell();
                    cell.InnerText = "Not Ready";
                    cell.Style.Add("color", "orange");
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);

                    if (GMTSLAdiff >= 0 && GMTSLAdiff < 60)
                    {
                        row.Attributes.Add("class", "warning");
                    }
                    else if (GMTSLAdiff >= 60)
                    {
                        row.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        row.Attributes.Add("class", "info");
                    }

                    cell = new HtmlTableCell();
                    cell.InnerText = ""; //delay in minutes is nullified
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);


                    cell = new HtmlTableCell();
                    cell.InnerText = "9:30 AM GMT";
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);

                    cell = new HtmlTableCell();
                    cell.InnerText = ""; //xaxis delay in minutes is nullified
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);

                    cell = new HtmlTableCell();
                    cell.InnerText = "7:30 AM GMT";
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);
                }

                else //the reports are ready
                {
                    if (GMTdatafeedDiff.TotalMinutes > 0)
                    {
                        cell = new HtmlTableCell();
                        cell.InnerText = "Were delivered with Delay";
                        cell.Style.Add("color", "red");
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);


                        cell = new HtmlTableCell();
                        cell.InnerText = Math.Floor(GMTdatafeedDiff.TotalMinutes).ToString();
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);

                        if (GMTdatafeedDiff.TotalMinutes < 60)
                        {
                            row.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            row.Attributes.Add("class", "danger");
                        }
                    }
                    else
                    {
                        cell = new HtmlTableCell();
                        cell.InnerText = "Ready";
                        cell.Style.Add("color", "green");
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);

                        row.Attributes.Add("class", "success");

                        cell = new HtmlTableCell();
                        cell.InnerText = ""; //no delay diff
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);
                    }

                    cell = new HtmlTableCell();
                    cell.InnerText = "9:30 AM GMT";
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);

                    TimeSpan Xaxis730SLADiff = datafeedTimeInGMT.Subtract(Xaxis730SLA);
                    if (Xaxis730SLADiff.TotalMinutes > 0)
                    {
                        cell = new HtmlTableCell();
                        cell.InnerText = Math.Floor(Xaxis730SLADiff.TotalMinutes).ToString();
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);
                        
                    }
                    else
                    {
                        cell = new HtmlTableCell();
                        cell.InnerText = ""; //no xaxis sla diff
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);
                    }

                    cell = new HtmlTableCell();
                    cell.InnerText = "7:30 AM GMT";
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);
                }

            }

            myReader.Close();
            conn.Close();
        }


        private void saveData()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "noc" + ";" + "password=" + "noc972" + ";" + "server=" + "ILNOC01" + ";" + "Trusted_Connection=false;" + "database=" + "GroupMDB" + ";" + "connection timeout=30; MultipleActiveResultSets=True;");
            DateTime GMTdate = DateTime.UtcNow.Date;
            conn.Open();
            string stmt5 = "SELECT TOP 1 * FROM GroupMArchiveData ORDER BY id DESC;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    DateTime reportDate = DateTime.Parse(myReader["ReportDate"].ToString());
                    //System.Diagnostics.Debug.WriteLine("ReportDate: " + reportDate.ToString("MM/dd/yyyy"));
                    //System.Diagnostics.Debug.WriteLine("GMTDate: " + GMTdate.ToString("MM/dd/yyyy"));
                    //check if we already added the table today
                    if (reportDate.ToString("MM/dd/yyyy") == GMTdate.ToString("MM/dd/yyyy"))
                    {
                        break;
                    }
                    else
                    {
                        for (int i = 1; i < tableContent.Rows.Count; i++)
                        {
                            stmt5 = "INSERT INTO GroupMArchiveData (ReportDate, ReportName, Timezone, Availability, Status, Delay, SLA, GroupMDelay, GroupMSLA)" +
                                "VALUES (@ReportDate, @ReportName, @Timezone, @Availability, @Status, @Delay, @SLA, @GroupMDelay, @GroupMSLA);";
                            comm4 = new SqlCommand(stmt5, conn);
                            comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                            comm4.Parameters.AddWithValue("@ReportName", tableContent.Rows[i].Cells[0].InnerText);
                            comm4.Parameters.AddWithValue("@Timezone", tableContent.Rows[i].Cells[1].InnerText);
                            comm4.Parameters.AddWithValue("@Availability", tableContent.Rows[i].Cells[2].InnerText);
                            comm4.Parameters.AddWithValue("@Status", tableContent.Rows[i].Cells[3].InnerText);
                            comm4.Parameters.AddWithValue("@Delay", tableContent.Rows[i].Cells[4].InnerText);
                            comm4.Parameters.AddWithValue("@SLA", "9:30 AM GMT");
                            comm4.Parameters.AddWithValue("@GroupMDelay", tableContent.Rows[i].Cells[6].InnerText);
                            comm4.Parameters.AddWithValue("@GroupMSLA", "7:30 AM GMT");
                            comm4.ExecuteReader();
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i < tableContent.Rows.Count; i++)
                {
                    stmt5 = "INSERT INTO GroupMArchiveData (ReportDate, ReportName, Timezone, Availability, Status, Delay, SLA, GroupMDelay, GroupMSLA)" +
                        "VALUES (@ReportDate, @ReportName, @Timezone, @Availability, @Status, @Delay, @SLA, @GroupMDelay, @GroupMSLA);";
                    comm4 = new SqlCommand(stmt5, conn);
                    comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                    comm4.Parameters.AddWithValue("@ReportName", tableContent.Rows[i].Cells[0].InnerText);
                    comm4.Parameters.AddWithValue("@Timezone", tableContent.Rows[i].Cells[1].InnerText);
                    comm4.Parameters.AddWithValue("@Availability", tableContent.Rows[i].Cells[2].InnerText);
                    comm4.Parameters.AddWithValue("@Status", tableContent.Rows[i].Cells[3].InnerText);
                    comm4.Parameters.AddWithValue("@Delay", tableContent.Rows[i].Cells[4].InnerText);
                    comm4.Parameters.AddWithValue("@SLA", "9:30 AM GMT");
                    comm4.Parameters.AddWithValue("@GroupMDelay", tableContent.Rows[i].Cells[6].InnerText);
                    comm4.Parameters.AddWithValue("@GroupMSLA", "7:30 AM GMT");
                    comm4.ExecuteReader();
                }
            }
        }
    }
}