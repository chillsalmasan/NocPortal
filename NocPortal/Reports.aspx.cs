using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GMTReportsReadiness
{
    public partial class HomePage : System.Web.UI.Page
    {

        DateTime GMTpredefined;
        DateTime GMTolap;
        DateTime GMTdatafeed;

        DateTime GMTpredefinedSLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "08:00:00");
        DateTime GMTolapSLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "09:00:00");
        DateTime GMTdatafeedSLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "08:00:00");
        DateTime GMTdatafeedSLACutOff = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "08:05:00");
        DateTime Xaxis730SLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "07:30:00");


        protected void Page_Load(object sender, EventArgs e)
        {
            checkBDB();
            checkDatafeed();

            if (HiddenPredefinedReady.Value == "1" && HiddenOlapReady.Value == "1" && HiddenGroupMReady.Value == "1" && HiddenGroupMReady2.Value == "1" && HiddenDailyFeedReady.Value == "1")
            {//meaning if all the reports are ready

                storeInDatabase();

                saveData();
            }
        }

        private SqlConnection connectToDB(string id, string password, string server, string database)
        {
            return new SqlConnection("user id=" + id + ";" + "password=" + password + ";" + "server=" + server + ";" + "Trusted_Connection=false;" + "database=" + database + ";" + "connection timeout=30");
        }

        private void checkBDB()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "readonlyuser" + ";" + "password=" + "rou123" + ";" + "server=" + "BDBNJ.eyedcny.local" + ";" + "Trusted_Connection=false;" + "database=" + "master" + ";" + "connection timeout=30");
            conn.Open();
            //string stmt5 = "SELECT TimeZoneID , Description , BurstingDB.dbo.DateIDToDateTime(LastAggDateID) AS PTM_API_AggDatafeed_Currently_Availabe_Data , DATEADD(dd,-1, BurstingDB.dbo.DateIDToDateTime(LastCRBSyncRepDateUpdateDateIDIncludeHour / 100 * 100)) AS CRB_VA_Currently_Availabe_Data FROM BurstingDB..TimeZoneLookup WHERE AdditionalTimeZoneSupported = 1 AND Description = 'GMT Greenwich Mean Time';";
            string stmt5 = "SELECT [TimeZoneID] ,[GMT] ,[LastAggDateID] ,[Description] ,[EST] ,[LastVAAggDateID] ,[LastVAPublishDate] ,[VaAggUpdateTime] ,[AggUpdateTime] FROM [BurstingDB].[dbo].[TimeZoneLookup] WHERE AdditionalTimeZoneSupported = 1 AND Description = 'GMT Greenwich Mean Time';";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            while (myReader.Read())
            {
                /*
                DateTime predefinedDate = DateTime.Parse(myReader["PTM_API_AggDatafeed_Currently_Availabe_Data"].ToString().Split(' ')[0]);
                predefinedReports.InnerText = predefinedDate.ToString("dd/MM/yyyy");
                DateTime olapDate = DateTime.Parse(myReader["CRB_VA_Currently_Availabe_Data"].ToString().Split(' ')[0]);
                olapReports.InnerText = olapDate.ToString("dd/MM/yyyy");
                */

                GMTpredefined = DateTime.Parse(myReader["AggUpdateTime"].ToString());
                GMTolap = DateTime.Parse(myReader["VaAggUpdateTime"].ToString());

                DateTime predefinedDate = DateTime.Parse(myReader["AggUpdateTime"].ToString().Split(' ')[0]);
                predefinedReports.InnerText = predefinedDate.ToString("dd/MM/yyyy") + " " + myReader["AggUpdateTime"].ToString().Split(' ')[1];
                DateTime olapDate = DateTime.Parse(myReader["VaAggUpdateTime"].ToString().Split(' ')[0]);
                olapReports.InnerText = olapDate.ToString("dd/MM/yyyy") + " " + myReader["VaAggUpdateTime"].ToString().Split(' ')[1];

                //time in gmt
                DateTime utc = DateTime.UtcNow;

                double GMTPredefinedSLAdiff = utc.Subtract(GMTpredefinedSLA).TotalMinutes;
                TimeSpan GMTpredefinedDiff = GMTpredefined.Subtract(GMTpredefinedSLA);


                //if the predefined reports are still not ready
                if (predefinedDate != DateTime.Today)
                {
                    predefinedDiff.InnerText = "";
                    XaxisPredefinedDiff.InnerText = "";


                    HiddenPredefinedReady.Value = "0";

                    predefinedStatus.InnerText = "Not Ready";
                    predefinedStatus.Style.Add("color", "orange");

                    if (GMTPredefinedSLAdiff >= 0 && GMTPredefinedSLAdiff < 60)
                    {
                        PredefinedRow.Attributes.Add("class", "warning");
                    }
                    else if (GMTPredefinedSLAdiff >= 60)
                    {
                        PredefinedRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        PredefinedRow.Attributes.Add("class", "info");
                    }
                }

                else // the predefined reports are ready
                {
                    HiddenPredefinedReady.Value = "1";

                    if (GMTpredefinedDiff.TotalMinutes > 0) //Check if we surpassed the internal SLA
                    {
                        predefinedDiff.InnerText = Math.Floor(GMTpredefinedDiff.TotalMinutes).ToString();

                        predefinedStatus.InnerText = "Were delivered with Delay";
                        predefinedStatus.Style.Add("color", "red");

                        if (GMTpredefinedDiff.TotalMinutes < 60)
                        {
                            PredefinedRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            PredefinedRow.Attributes.Add("class", "danger");
                        }
                    }
                    else //they were ready before the internal SLA
                    {
                        predefinedStatus.InnerText = "Ready";
                        predefinedStatus.Style.Add("color", "green");

                        PredefinedRow.Attributes.Add("class", "success");
                    }


                    TimeSpan Xaxis730SLADiff = GMTpredefined.Subtract(Xaxis730SLA);
                    if (Xaxis730SLADiff.TotalMinutes > 0)
                    {
                        XaxisPredefinedDiff.InnerText = Math.Floor(Xaxis730SLADiff.TotalMinutes).ToString();
                    }
                }



                double GMTOlapSLAdiff = utc.Subtract(GMTolapSLA).TotalMinutes;
                TimeSpan GMTolapDiff = GMTolap.Subtract(GMTolapSLA);

                //if the OLAP reports are still not ready
                if (olapDate != DateTime.Today)
                {
                    olapDiff.InnerText = "";
                    XaxisOlapDiff.InnerText = "";


                    HiddenOlapReady.Value = "0";

                    olapStatus.InnerText = "Not Ready";
                    olapStatus.Style.Add("color", "orange");


                    if (GMTOlapSLAdiff >= 0 && GMTOlapSLAdiff < 60)
                    {
                        OlapRow.Attributes.Add("class", "warning");
                    }
                    else if (GMTOlapSLAdiff >= 60)
                    {
                        OlapRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        OlapRow.Attributes.Add("class", "info");
                    }
                }
                else
                {
                    HiddenOlapReady.Value = "1";
                    if (GMTolapDiff.TotalMinutes > 0)
                    {
                        olapDiff.InnerText = Math.Floor(GMTolapDiff.TotalMinutes).ToString();
                        olapStatus.InnerText = "Were delivered with Delay";
                        olapStatus.Style.Add("color", "red");

                        if (GMTolapDiff.TotalMinutes < 60)
                        {
                            OlapRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            OlapRow.Attributes.Add("class", "danger");
                        }
                    }
                    else
                    {
                        olapStatus.InnerText = "Ready";
                        olapStatus.Style.Add("color", "green");

                        OlapRow.Attributes.Add("class", "success");
                    }

                    TimeSpan Xaxis730SLADiff = GMTolap.Subtract(Xaxis730SLA);
                    if (Xaxis730SLADiff.TotalMinutes > 0)
                    {
                        XaxisOlapDiff.InnerText = Math.Floor(Xaxis730SLADiff.TotalMinutes).ToString();
                    }
                }

            }
            myReader.Close();
            conn.Close();
        }

        private void checkDatafeed()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "readonlyuser" + ";" + "password=" + "rou123" + ";" + "server=" + "RSDWHNJ.eyedcny.local" + ";" + "Trusted_Connection=false;" + "database=" + "Reportsdwh" + ";" + "connection timeout=30");
            conn.Open();
            string stmt5 = "SELECT FeedName, MAX(AvailabeSince) AS AvailabeSince ,MAX(LastDateAvailabe) AS LastDateAvailabe FROM dbo.DataFeeds_Status_ByAccount_Detailed WHERE (FeedName LIKE '%Xaxis DE%' or FeedName  like '%AllAccount%GMT%') AND LastDateAvailabe < GetDate() GROUP BY FeedName;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            while (myReader.Read())
            {
                /*
                DateTime datafeedDate = DateTime.Parse(myReader["LastDateAvailabe"].ToString().Split(' ')[0]);
                datafeedReports.InnerText = datafeedDate.ToString("dd/MM/yyyy");
                */
                if (myReader["FeedName"].ToString() == "GroupM_DE_AllAccount_GP_GMT")
                {
                    GMTdatafeed = DateTime.Parse(myReader["AvailabeSince"].ToString());

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
                    //datafeedReports.InnerText = datafeedDate.ToString("dd/MM/yyyy") + " " + myReader["AvailabeSince"].ToString().Split(' ')[1];

                    GroupMReports.InnerText = datafeedTimeInGMT.ToString("dd/MM/yyyy HH:mm:ss");


                    double GMTSLAdiff = utc.Subtract(GMTdatafeedSLA).TotalMinutes;

                    //TimeSpan GMTdatafeedDiff = GMTdatafeed.Subtract(GMTdatafeedSLA);
                    TimeSpan GMTdatafeedDiff = datafeedTimeInGMT.Subtract(GMTdatafeedSLA);
                    if (DateTime.Parse(datafeedTimeInGMT.ToString().Split(' ')[0]) != DateTime.Today)
                    {
                        GroupMDiff.InnerText = "";
                        XaxisGroupMDiff.InnerText = "";


                        HiddenGroupMReady.Value = "0";

                        //NEW CODE
                        if (utc == GMTdatafeedSLA)
                        {
                            sendMail("GroupM_DE_AllAccount_GP_GMT Not Ready", "GroupM DE AllAccount GP GMT is not yet ready.");
                        }
                        //END
                        GroupMStatus.InnerText = "Not Ready";
                        GroupMStatus.Style.Add("color", "orange");

                        if (GMTSLAdiff >= 0 && GMTSLAdiff < 60)
                        {
                            GroupMRow.Attributes.Add("class", "warning");
                        }
                        else if (GMTSLAdiff >= 60)
                        {
                            GroupMRow.Attributes.Add("class", "danger");
                        }
                        else
                        {
                            GroupMRow.Attributes.Add("class", "info");
                        }
                    }
                    else
                    {
                        HiddenGroupMReady.Value = "1";

                        if (GMTdatafeedDiff.TotalMinutes > 0)
                        {
                            GroupMDiff.InnerText = Math.Floor(GMTdatafeedDiff.TotalMinutes).ToString();

                            //NEW CODE
                            if (utc == GMTdatafeedSLA)
                            {
                                sendMail("GroupM_DE_AllAccount_GP_GMT is now Ready", "GroupM DE AllAccount GP GMT is now ready and were delivered with delay.");
                            }
                            //END

                            GroupMStatus.InnerText = "Were delivered with Delay";
                            GroupMStatus.Style.Add("color", "red");

                            if (GMTdatafeedDiff.TotalMinutes < 60)
                            {
                                GroupMRow.Attributes.Add("class", "warning");
                            }
                            else // >= 60
                            {
                                GroupMRow.Attributes.Add("class", "danger");
                            }
                        }
                        else
                        {
                            //NEW CODE
                            if (utc == GMTdatafeedSLA)
                            {
                                sendMail("GroupM_DE_AllAccount_GP_GMT is now Ready", "GroupM DE AllAccount GP GMT is now ready.");
                            }
                            //END
                            GroupMStatus.InnerText = "Ready";
                            GroupMStatus.Style.Add("color", "green");

                            GroupMRow.Attributes.Add("class", "success");
                        }

                        TimeSpan Xaxis730SLADiff = datafeedTimeInGMT.Subtract(Xaxis730SLA);
                        if (Xaxis730SLADiff.TotalMinutes > 0)
                        {
                            XaxisGroupMDiff.InnerText = Math.Floor(Xaxis730SLADiff.TotalMinutes).ToString();
                        }
                    }
                }
                else if (myReader["FeedName"].ToString() == "GroupM_DE_AllAccount_WinningEvent_GMT")
                {
                    GMTdatafeed = DateTime.Parse(myReader["AvailabeSince"].ToString());

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
                    //datafeedReports.InnerText = datafeedDate.ToString("dd/MM/yyyy") + " " + myReader["AvailabeSince"].ToString().Split(' ')[1];

                    GroupMReports2.InnerText = datafeedTimeInGMT.ToString("dd/MM/yyyy HH:mm:ss");


                    double GMTSLAdiff2 = utc.Subtract(GMTdatafeedSLA).TotalMinutes;

                    //TimeSpan GMTdatafeedDiff = GMTdatafeed.Subtract(GMTdatafeedSLA);
                    TimeSpan GMTdatafeedDiff2 = datafeedTimeInGMT.Subtract(GMTdatafeedSLA);

                    if (DateTime.Parse(datafeedTimeInGMT.ToString().Split(' ')[0]) != DateTime.Today)
                    {
                        GroupMDiff2.InnerText = "";
                        XaxisGroupMDiff2.InnerText = "";


                        HiddenGroupMReady2.Value = "0";

                        //NEW CODE
                        if (utc == GMTdatafeedSLA)
                        {
                            sendMail("GroupM_DE_AllAccount_WinningEvent_GMT is not yet ready", "GroupM_DE_AllAccount_WinningEvent_GMT is not yet ready.");
                        }
                        //END

                        GroupMStatus2.InnerText = "Not Ready";
                        GroupMStatus2.Style.Add("color", "orange");

                        if (GMTSLAdiff2 >= 0 && GMTSLAdiff2 < 60)
                        {
                            GroupMRow2.Attributes.Add("class", "warning");
                        }
                        else if (GMTSLAdiff2 >= 60)
                        {
                            GroupMRow2.Attributes.Add("class", "danger");
                        }
                        else
                        {
                            GroupMRow2.Attributes.Add("class", "info");
                        }
                    }
                    else
                    {
                        HiddenGroupMReady2.Value = "1";

                        if (GMTdatafeedDiff2.TotalMinutes > 0)
                        {
                            GroupMDiff2.InnerText = Math.Floor(GMTdatafeedDiff2.TotalMinutes).ToString();

                            //NEW CODE
                            if (utc == GMTdatafeedSLA)
                            {
                                sendMail("GroupM_DE_AllAccount_WinningEvent_GMT is now ready", "GroupM_DE_AllAccount_WinningEvent_GMT is now ready and were delivered with delay.");

                            }
                            //END

                            GroupMStatus2.InnerText = "Were delivered with Delay";
                            GroupMStatus2.Style.Add("color", "red");

                            if (GMTdatafeedDiff2.TotalMinutes < 60)
                            {
                                GroupMRow2.Attributes.Add("class", "warning");
                            }
                            else // >= 60
                            {
                                GroupMRow2.Attributes.Add("class", "danger");
                            }
                        }
                        else
                        {
                            //NEW CODE
                            if (utc == GMTdatafeedSLA)
                            {
                                sendMail("GroupM_DE_AllAccount_WinningEvent_GMT is now ready", "GroupM_DE_AllAccount_WinningEvent_GMT is now ready.");
                            }
                            //END

                            GroupMStatus2.InnerText = "Ready";
                            GroupMStatus2.Style.Add("color", "green");

                            GroupMRow2.Attributes.Add("class", "success");
                        }

                        TimeSpan Xaxis730SLADiff2 = datafeedTimeInGMT.Subtract(Xaxis730SLA);
                        if (Xaxis730SLADiff2.TotalMinutes > 0)
                        {
                            XaxisGroupMDiff2.InnerText = Math.Floor(Xaxis730SLADiff2.TotalMinutes).ToString();
                        }
                    }
                }
                else if (myReader["FeedName"].ToString() == "1_Xaxis DE - Daily feed")
                {
                    GMTdatafeed = DateTime.Parse(myReader["AvailabeSince"].ToString());

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

                    XaxisDailyReports.InnerText = datafeedTimeInGMT.ToString("dd/MM/yyyy HH:mm:ss");

                    double GMTSLAdiff = utc.Subtract(GMTdatafeedSLA).TotalMinutes;

                    TimeSpan GMTdatafeedDiff = datafeedTimeInGMT.Subtract(GMTdatafeedSLA);
                    if (DateTime.Parse(datafeedTimeInGMT.ToString().Split(' ')[0]) != DateTime.Today)
                    {
                        XaxisDailyDiff.InnerText = "";
                        XaxisDailyNewSLADiff.InnerText = "";


                        HiddenDailyFeedReady.Value = "0";

                        //NEW CODE
                        if (utc == GMTdatafeedSLA)
                        {
                            sendMail("Xaxis DE - Daily feed is not yet ready", "Xaxis DE - Daily feed is not yet ready.");
                        }
                        //END

                        XaxisDailyStatus.InnerText = "Not Ready";
                        XaxisDailyStatus.Style.Add("color", "orange");

                        if (GMTSLAdiff >= 0 && GMTSLAdiff < 60)
                        {
                            XaxisDailyRow.Attributes.Add("class", "warning");
                        }
                        else if (GMTSLAdiff >= 60)
                        {
                            XaxisDailyRow.Attributes.Add("class", "danger");
                        }
                        else
                        {
                            XaxisDailyRow.Attributes.Add("class", "info");
                        }
                    }
                    else
                    {
                        HiddenDailyFeedReady.Value = "1";

                        if (GMTdatafeedDiff.TotalMinutes > 0)
                        {
                            XaxisDailyDiff.InnerText = Math.Floor(GMTdatafeedDiff.TotalMinutes).ToString();

                            //NEW CODE
                            if (utc == GMTdatafeedSLA)
                            {
                                sendMail("Xaxis DE - Daily feed is now ready", "Xaxis DE - Daily feed is now ready and were delivered with delay.");
                            }
                            //END

                            XaxisDailyStatus.InnerText = "Were delivered with Delay";
                            XaxisDailyStatus.Style.Add("color", "red");

                            if (GMTdatafeedDiff.TotalMinutes < 60)
                            {
                                XaxisDailyRow.Attributes.Add("class", "warning");
                            }
                            else // >= 60
                            {
                                XaxisDailyRow.Attributes.Add("class", "danger");
                            }
                        }
                        else
                        {
                            //NEW CODE
                            if (utc == GMTdatafeedSLA)
                            {
                                sendMail("Xaxis DE - Daily feed is now ready", "Xaxis DE - Daily feed is now ready.");
                            }
                            //END

                            XaxisDailyStatus.InnerText = "Ready";
                            XaxisDailyStatus.Style.Add("color", "green");

                            XaxisDailyRow.Attributes.Add("class", "success");
                        }


                        TimeSpan Xaxis730SLADiff = datafeedTimeInGMT.Subtract(Xaxis730SLA);
                        if (Xaxis730SLADiff.TotalMinutes > 0)
                        {
                            XaxisDailyNewSLADiff.InnerText = Math.Floor(Xaxis730SLADiff.TotalMinutes).ToString();
                        }
                    }
                }
            }
            myReader.Close();
            conn.Close();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            checkBDB();
            checkDatafeed();

            if (HiddenPredefinedReady.Value == "1" && HiddenOlapReady.Value == "1" && HiddenGroupMReady.Value == "1" && HiddenGroupMReady2.Value == "1" && HiddenDailyFeedReady.Value == "1")
            {//meaning if all the reports are ready

                storeInDatabase();

                saveData();
            }
        }
        //Additional Code
        public static void sendMail(string emailSubject, string emailBody)
        {
            MailMessage mailMessage = new MailMessage("xaxisreports@sizmek.com", "nocsupport@sizmek.com");
            mailMessage.Subject = emailSubject;
            mailMessage.Body = emailBody;

            SmtpClient smtpClient = new SmtpClient("10.10.2.15", 25);
            smtpClient.EnableSsl = false;
            smtpClient.Send(mailMessage);
        }
        //End of additional code

        private void saveData()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "noc" + ";" + "password=" + "noc972" + ";" + "server=" + "ILNOC01" + ";" + "Trusted_Connection=false;" + "database=" + "GMTReportsReadiness" + ";" + "connection timeout=30; MultipleActiveResultSets=True;");
            DateTime GMTdate = DateTime.UtcNow.Date;
            conn.Open();
            string stmt5 = "SELECT TOP 1 * FROM ArchiveData ORDER BY id DESC;";
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
                        //System.Diagnostics.Debug.WriteLine("break!");
                        break;
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            stmt5 = "INSERT INTO ArchiveData (ReportDate, ReportType, Availability, Status, Delay, SLA, XaxisDelay, XaxisSLA)" +
                                "VALUES (@ReportDate, @ReportType, @Availability, @Status, @Delay, @SLA, @XaxisDelay, @XaxisSLA);";
                            comm4 = new SqlCommand(stmt5, conn);
                            if (i == 0)
                            {
                                comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                                comm4.Parameters.AddWithValue("@ReportType", "Predefined");
                                comm4.Parameters.AddWithValue("@Availability", predefinedReports.InnerText);
                                comm4.Parameters.AddWithValue("@Status", predefinedStatus.InnerText);
                                comm4.Parameters.AddWithValue("@Delay", predefinedDiff.InnerText);
                                comm4.Parameters.AddWithValue("@SLA", predefinedSLA.InnerText);
                                comm4.Parameters.AddWithValue("@XaxisDelay", XaxisPredefinedDiff.InnerText);
                                comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                            }
                            else if (i == 1)
                            {
                                comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                                comm4.Parameters.AddWithValue("@ReportType", "OLAP");
                                comm4.Parameters.AddWithValue("@Availability", olapReports.InnerText);
                                comm4.Parameters.AddWithValue("@Status", olapStatus.InnerText);
                                comm4.Parameters.AddWithValue("@Delay", olapDiff.InnerText);
                                comm4.Parameters.AddWithValue("@SLA", olapSLA.InnerText);
                                comm4.Parameters.AddWithValue("@XaxisDelay", XaxisOlapDiff.InnerText);
                                comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                            }
                            else if (i == 2)
                            {
                                comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                                comm4.Parameters.AddWithValue("@ReportType", "GroupM DE - Xaxis DE");
                                comm4.Parameters.AddWithValue("@Availability", GroupMReports.InnerText);
                                comm4.Parameters.AddWithValue("@Status", GroupMStatus.InnerText);
                                comm4.Parameters.AddWithValue("@Delay", GroupMDiff.InnerText);
                                comm4.Parameters.AddWithValue("@SLA", "9:30 AM GMT");
                                comm4.Parameters.AddWithValue("@XaxisDelay", XaxisGroupMDiff.InnerText);
                                comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                            }
                            else if (i == 3)
                            {
                                comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                                comm4.Parameters.AddWithValue("@ReportType", "Xaxis DE - Daily feed");
                                comm4.Parameters.AddWithValue("@Availability", XaxisDailyReports.InnerText);
                                comm4.Parameters.AddWithValue("@Status", XaxisDailyStatus.InnerText);
                                comm4.Parameters.AddWithValue("@Delay", XaxisDailyDiff.InnerText);
                                comm4.Parameters.AddWithValue("@SLA", "9:30 AM GMT");
                                comm4.Parameters.AddWithValue("@XaxisDelay", XaxisDailyNewSLADiff.InnerText);
                                comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                            }
                            comm4.ExecuteReader();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    stmt5 = "INSERT INTO ArchiveData (ReportDate, ReportType, Availability, Status, Delay, SLA, XaxisDelay, XaxisSLA)" +
                        "VALUES (@ReportDate, @ReportType, @Availability, @Status, @Delay, @SLA, @XaxisDelay, @XaxisSLA);";
                    comm4 = new SqlCommand(stmt5, conn);
                    if (i == 0)
                    {
                        comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                        comm4.Parameters.AddWithValue("@ReportType", "Predefined");
                        comm4.Parameters.AddWithValue("@Availability", predefinedReports.InnerText);
                        comm4.Parameters.AddWithValue("@Status", predefinedStatus.InnerText);
                        comm4.Parameters.AddWithValue("@Delay", predefinedDiff.InnerText);
                        comm4.Parameters.AddWithValue("@SLA", predefinedSLA.InnerText);
                        comm4.Parameters.AddWithValue("@XaxisDelay", XaxisPredefinedDiff.InnerText);
                        comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                    }
                    else if (i == 1)
                    {
                        comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                        comm4.Parameters.AddWithValue("@ReportType", "OLAP");
                        comm4.Parameters.AddWithValue("@Availability", olapReports.InnerText);
                        comm4.Parameters.AddWithValue("@Status", olapStatus.InnerText);
                        comm4.Parameters.AddWithValue("@Delay", olapDiff.InnerText);
                        comm4.Parameters.AddWithValue("@SLA", olapSLA.InnerText);
                        comm4.Parameters.AddWithValue("@XaxisDelay", XaxisOlapDiff.InnerText);
                        comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                    }
                    else if (i == 2)
                    {
                        comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                        comm4.Parameters.AddWithValue("@ReportType", "GroupM DE - Xaxis DE");
                        comm4.Parameters.AddWithValue("@Availability", GroupMReports.InnerText);
                        comm4.Parameters.AddWithValue("@Status", GroupMStatus.InnerText);
                        comm4.Parameters.AddWithValue("@Delay", GroupMDiff.InnerText);
                        comm4.Parameters.AddWithValue("@SLA", "8:00 AM GMT");
                        comm4.Parameters.AddWithValue("@XaxisDelay", XaxisGroupMDiff.InnerText);
                        comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                    }
                    else if (i == 3)
                    {
                        comm4.Parameters.AddWithValue("@ReportDate", GMTdate.ToString("yyyy-MM-dd"));
                        comm4.Parameters.AddWithValue("@ReportType", "Xaxis DE - Daily feed");
                        comm4.Parameters.AddWithValue("@Availability", XaxisDailyReports.InnerText);
                        comm4.Parameters.AddWithValue("@Status", XaxisDailyStatus.InnerText);
                        comm4.Parameters.AddWithValue("@Delay", XaxisDailyDiff.InnerText);
                        comm4.Parameters.AddWithValue("@SLA", "8:00 AM GMT");
                        comm4.Parameters.AddWithValue("@XaxisDelay", XaxisDailyNewSLADiff.InnerText);
                        comm4.Parameters.AddWithValue("@XaxisSLA", "7:30 AM GMT");
                    }
                    comm4.ExecuteReader();
                }
            }


            myReader.Close();
            conn.Close();
        }


        private void storeInDatabase()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "noc" + ";" + "password=" + "noc972" + ";" + "server=" + "ILNOC01" + ";" + "Trusted_Connection=false;" + "database=" + "GMTReportsReadiness" + ";" + "connection timeout=30; MultipleActiveResultSets=True;");
            DateTime GMTdate = DateTime.UtcNow.Date;
            conn.Open();
            string stmt5 = "SELECT TOP 1 * FROM Archive ORDER BY id DESC;";
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
                        //System.Diagnostics.Debug.WriteLine("break!");
                        break;
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("writing to db");
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter w = new HtmlTextWriter(sw);
                        tableDiv.RenderControl(w);
                        string tableDivContent = sw.GetStringBuilder().ToString();

                        stmt5 = "INSERT INTO Archive (tableHTML, ReportDate) VALUES (@tableHTML, @currentGMTdate);";
                        comm4 = new SqlCommand(stmt5, conn);
                        comm4.Parameters.AddWithValue("@tableHTML", tableDivContent);
                        comm4.Parameters.AddWithValue("@currentGMTdate", DateTime.UtcNow.Date);
                        comm4.ExecuteReader();
                    }


                }
            }
            else
            {
                StringWriter sw = new StringWriter();
                HtmlTextWriter w = new HtmlTextWriter(sw);
                tableDiv.RenderControl(w);
                string tableDivContent = sw.GetStringBuilder().ToString();

                stmt5 = "INSERT INTO Archive (tableHTML, ReportDate) VALUES (@tableHTML, @currentGMTdate);";
                comm4 = new SqlCommand(stmt5, conn);
                comm4.Parameters.AddWithValue("@tableHTML", tableDivContent);
                comm4.Parameters.AddWithValue("@currentGMTdate", DateTime.UtcNow.Date);
                comm4.ExecuteReader();

            }
            myReader.Close();
            conn.Close();
        }
    }
}