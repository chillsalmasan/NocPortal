using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NocPortal
{
    public partial class SizmekReports : System.Web.UI.Page
    {

        DateTime GMTpredefinedSLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "08:00:00");
        DateTime GMTolapSLA = DateTime.Parse(DateTime.Today.ToString("MM/dd/yyyy") + " " + "09:00:00");
        /*
        static DateTime AESTtimeNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time"));
        DateTime AESTolapSLA = DateTime.Parse(AESTtimeNow.ToString("MM/dd/yyyy") + " " + "10:30:00");
        DateTime AESTpredefinedSLA = DateTime.Parse(AESTtimeNow.ToString("MM/dd/yyyy") + " " + "09:00:00");
    
        static DateTime JSTtimeNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
        DateTime JSTolapSLA = DateTime.Parse(JSTtimeNow.ToString("MM/dd/yyyy") + " " + "09:30:00");
        DateTime JSTpredefinedSLA = DateTime.Parse(JSTtimeNow.ToString("MM/dd/yyyy") + " " + "08:00:00");

        static DateTime ESTtimeNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
        DateTime ESTolapSLA = DateTime.Parse(ESTtimeNow.ToString("MM/dd/yyyy") + " " + "09:00:00");
        DateTime ESTpredefinedSLA = DateTime.Parse(ESTtimeNow.ToString("MM/dd/yyyy") + " " + "08:00:00");
        */


        
        DateTime AESTolapSLA = DateTime.Parse(TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time")).ToString("MM/dd/yyyy") + " " + "10:30:00");
        DateTime AESTpredefinedSLA = DateTime.Parse(TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time")).ToString("MM/dd/yyyy") + " " + "09:00:00");

        
        DateTime JSTolapSLA = DateTime.Parse(TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time")).ToString("MM/dd/yyyy") + " " + "09:30:00");
        DateTime JSTpredefinedSLA = DateTime.Parse(TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time")).ToString("MM/dd/yyyy") + " " + "08:00:00");

        
        DateTime ESTolapSLA = DateTime.Parse(TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToString("MM/dd/yyyy") + " " + "09:00:00");
        DateTime ESTpredefinedSLA = DateTime.Parse(TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToString("MM/dd/yyyy") + " " + "08:00:00");



        static Boolean JSTpredefinedReadyEmail = false;
        static Boolean JSTOLAPReadyEmail = false;
        static Boolean AESTpredefinedReadyEmail = false;
        static Boolean AESTOLAPReadyEmail = false;
        static Boolean GMTpredefinedReadyEmail = false;
        static Boolean GMTOLAPReadyEmail = false;
        static Boolean ESTpredefinedReadyEmail = false;
        static Boolean ESTOLAPReadyEmail = false;



        protected void Page_Load(object sender, EventArgs e)
        {
            checkBDB();

            //System.Diagnostics.Debug.WriteLine("AESTtimeNow: " + AESTtimeNow);
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            checkBDB();
        }

        private void checkBDB()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "readonlyuser" + ";" + "password=" + "rou123" + ";" + "server=" + "BDBNJ" + ";" + "Trusted_Connection=false;" + "database=" + "master" + ";" + "connection timeout=30");
            conn.Open();
            //string stmt5 = "SELECT TimeZoneID , Description , BurstingDB.dbo.DateIDToDateTime(LastAggDateID) AS PTM_API_AggDatafeed_Currently_Availabe_Data , DATEADD(dd,-1, BurstingDB.dbo.DateIDToDateTime(LastCRBSyncRepDateUpdateDateIDIncludeHour / 100 * 100)) AS CRB_VA_Currently_Availabe_Data FROM BurstingDB..TimeZoneLookup WHERE AdditionalTimeZoneSupported = 1 AND Description = 'GMT Greenwich Mean Time';";
            string stmt5 = "SELECT [TimeZoneID] ,[GMT] ,[LastAggDateID] ,[Description] ,[EST] ,[LastVAAggDateID] ,[LastVAPublishDate] ,[VaAggUpdateTime] ,[AggUpdateTime] FROM [BurstingDB].[dbo].[TimeZoneLookup] WHERE AdditionalTimeZoneSupported = 1;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            while (myReader.Read())
            {
                if(myReader["Description"].ToString() == "GMT Greenwich Mean Time")
                {
                    string predefinedAvail = myReader["AggUpdateTime"].ToString();
                    string olapAvail = myReader["VaAggUpdateTime"].ToString();

                    handleBDBreportsGMT(predefinedAvail, olapAvail);
                }
                else if(myReader["Description"].ToString() == "GMT -5 Eastern Standard Time")
                {
                    string predefinedAvail = myReader["AggUpdateTime"].ToString();
                    string olapAvail = myReader["VaAggUpdateTime"].ToString();

                    handleBDBreportsEST(predefinedAvail, olapAvail);
                }
                else if (myReader["Description"].ToString() == "GMT +9 Osaka, Sapporo, Tokyo")
                {
                    string predefinedAvail = myReader["AggUpdateTime"].ToString();
                    string olapAvail = myReader["VaAggUpdateTime"].ToString();

                    handleBDBreportsJST(predefinedAvail, olapAvail);
                }
                else if (myReader["Description"].ToString() == "GMT +10 Brisbane, Guam, Sydney, Vladivostok")
                {
                    string predefinedAvail = myReader["AggUpdateTime"].ToString();
                    string olapAvail = myReader["VaAggUpdateTime"].ToString();

                    handleBDBreportsAEST(predefinedAvail, olapAvail);
                }
            }

            myReader.Close();
            conn.Close();
        }

        private void handleBDBreportsGMT(string predefinedAvail, string olapAvail)
        {
            DateTime GMTpredefined = DateTime.Parse(predefinedAvail);
            DateTime GMTolap = DateTime.Parse(olapAvail);

            DateTime predefinedDate = DateTime.Parse(predefinedAvail.Split(' ')[0]);
            GMTpredefinedReadiness.InnerText = predefinedDate.ToString("dd/MM/yyyy") + " " + predefinedAvail.Split(' ')[1];
            DateTime olapDate = DateTime.Parse(olapAvail.Split(' ')[0]);
            GMTolapReadiness.InnerText = olapDate.ToString("dd/MM/yyyy") + " " + olapAvail.Split(' ')[1];

            //time in gmt
            DateTime utc = DateTime.UtcNow;

            double GMTPredefinedSLAdiff = utc.Subtract(GMTpredefinedSLA).TotalMinutes;
            TimeSpan GMTpredefinedDifference = GMTpredefined.Subtract(GMTpredefinedSLA);

            //System.Diagnostics.Debug.WriteLine("predefinedDate: " + predefinedDate);
            //System.Diagnostics.Debug.WriteLine("DateTime.Today: " + DateTime.Today);


            //if the predefined reports are still not ready
            if (predefinedDate != DateTime.Today)
            {
                GMTpredefinedReadyEmail = false;

                GMTpredefinedDiff.InnerText = "";
                //XaxisPredefinedDiff.InnerText = "";


                //HiddenPredefinedReady.Value = "0";

                GMTpredefinedStatus.InnerText = "Not Ready";
                GMTpredefinedStatus.Style.Add("color", "orange");

                if (GMTPredefinedSLAdiff >= 0 && GMTPredefinedSLAdiff < 60)
                {
                    GMTPredefinedRow.Attributes.Add("class", "warning");
                }
                else if (GMTPredefinedSLAdiff >= 60)
                {
                    GMTPredefinedRow.Attributes.Add("class", "danger");
                }
                else
                {
                    GMTPredefinedRow.Attributes.Add("class", "info");
                }
            }

            else // the predefined reports are ready
            {
                //HiddenPredefinedReady.Value = "1";

                if (GMTpredefinedDifference.TotalMinutes > 0) //Check if we surpassed the internal SLA
                {

                    if (GMTpredefinedReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                    {
                        GMTpredefinedReadyEmail = true;
                        //sendReadyEmail("GMT", "Predefined");
                    }

                    GMTpredefinedDiff.InnerText = Math.Floor(GMTpredefinedDifference.TotalMinutes).ToString();

                    GMTpredefinedStatus.InnerText = "Were delivered with Delay";
                    GMTpredefinedStatus.Style.Add("color", "red");

                    if (GMTpredefinedDifference.TotalMinutes < 60)
                    {
                        GMTPredefinedRow.Attributes.Add("class", "warning");
                    }
                    else // >= 60
                    {
                        GMTPredefinedRow.Attributes.Add("class", "danger");
                    }
                }
                else //they were ready before the internal SLA
                {
                    GMTpredefinedStatus.InnerText = "Ready";
                    GMTpredefinedStatus.Style.Add("color", "green");

                    GMTPredefinedRow.Attributes.Add("class", "success");
                }
            }



            double GMTOlapSLAdiff = utc.Subtract(GMTolapSLA).TotalMinutes;
            TimeSpan GMTolapDifference = GMTolap.Subtract(GMTolapSLA);

            //if the OLAP reports are still not ready
            if (olapDate != DateTime.Today)
            {

                GMTOLAPReadyEmail = false;

                GMTolapDiff.InnerText = "";


                //HiddenOlapReady.Value = "0";

                GMTolapStatus.InnerText = "Not Ready";
                GMTolapStatus.Style.Add("color", "orange");


                if (GMTOlapSLAdiff >= 0 && GMTOlapSLAdiff < 60)
                {
                    GMTOlapRow.Attributes.Add("class", "warning");
                }
                else if (GMTOlapSLAdiff >= 60)
                {
                    GMTOlapRow.Attributes.Add("class", "danger");
                }
                else
                {
                    GMTOlapRow.Attributes.Add("class", "info");
                }
            }
            else //the olap reports of gmt are ready
            {
                //HiddenOlapReady.Value = "1";
                if (GMTolapDifference.TotalMinutes > 0)
                {

                    if (GMTOLAPReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                    {
                        GMTOLAPReadyEmail = true;
                        //sendReadyEmail("GMT", "OLAP");
                    }

                    GMTolapDiff.InnerText = Math.Floor(GMTolapDifference.TotalMinutes).ToString();
                    GMTolapStatus.InnerText = "Were delivered with Delay";
                    GMTolapStatus.Style.Add("color", "red");

                    if (GMTolapDifference.TotalMinutes < 60)
                    {
                        GMTOlapRow.Attributes.Add("class", "warning");
                    }
                    else // >= 60
                    {
                        GMTOlapRow.Attributes.Add("class", "danger");
                    }
                }
                else
                {
                    GMTolapStatus.InnerText = "Ready";
                    GMTolapStatus.Style.Add("color", "green");

                    GMTOlapRow.Attributes.Add("class", "success");
                }
            }

        
        }

        private void handleBDBreportsEST(string predefinedAvail, string olapAvail)
        {
            DateTime predefinedAvailGMTtime = DateTime.Parse(predefinedAvail);
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime predefinedAvailESTtime = TimeZoneInfo.ConvertTimeFromUtc(predefinedAvailGMTtime, easternZone);

            ESTpredefinedReadiness.InnerText = predefinedAvailESTtime.ToString("dd/MM/yyyy HH:mm:ss");


            DateTime olapAvailGMTtime = DateTime.Parse(olapAvail);
            easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime olapAvailESTtime = TimeZoneInfo.ConvertTimeFromUtc(olapAvailGMTtime, easternZone);

            ESTolapReadiness.InnerText = olapAvailESTtime.ToString("dd/MM/yyyy HH:mm:ss");


            var timeUtc = DateTime.UtcNow;
            DateTime easternTimeNow = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            //System.Diagnostics.Debug.WriteLine("easternTimeNow: " + easternTimeNow.ToString());
            colorsAndStatusLogic("EST", predefinedAvailESTtime, olapAvailESTtime, easternTimeNow, easternTimeNow.ToString().Split(' ')[0]);
        }


        private void handleBDBreportsJST(string predefinedAvail, string olapAvail)
        {
            DateTime predefinedAvailGMTtime = DateTime.Parse(predefinedAvail);
            TimeZoneInfo GMT = TimeZoneInfo.Utc;
            TimeZoneInfo JST = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            DateTime predefinedAvailJSTtime = TimeZoneInfo.ConvertTime(predefinedAvailGMTtime, GMT, JST);

            JSTpredefinedReadiness.InnerText = predefinedAvailJSTtime.ToString("dd/MM/yyyy HH:mm:ss");

            DateTime olapAvailGMTtime = DateTime.Parse(olapAvail);
            DateTime olapAvailJSTtime = TimeZoneInfo.ConvertTime(olapAvailGMTtime, GMT, JST);

            JSTolapReadiness.InnerText = olapAvailJSTtime.ToString("dd/MM/yyyy HH:mm:ss");


            DateTime predefinedAvailJSTtimeNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, GMT, JST);
            colorsAndStatusLogic("JST", predefinedAvailJSTtime, olapAvailJSTtime, predefinedAvailJSTtimeNow, predefinedAvailJSTtimeNow.ToString().Split(' ')[0]);
        }

        private void handleBDBreportsAEST(string predefinedAvail, string olapAvail)
        {
            DateTime predefinedAvailGMTtime = DateTime.Parse(predefinedAvail);
            TimeZoneInfo GMT = TimeZoneInfo.Utc;
            TimeZoneInfo AEST = TimeZoneInfo.FindSystemTimeZoneById("E. Australia Standard Time");
            DateTime predefinedAvailAESTtime = TimeZoneInfo.ConvertTime(predefinedAvailGMTtime, GMT, AEST);

            AESTpredefinedReadiness.InnerText = predefinedAvailAESTtime.ToString("dd/MM/yyyy HH:mm:ss");

            DateTime olapAvailGMTtime = DateTime.Parse(olapAvail);
            DateTime olapAvailAESTtime = TimeZoneInfo.ConvertTime(olapAvailGMTtime, GMT, AEST);

            AESTolapReadiness.InnerText = olapAvailAESTtime.ToString("dd/MM/yyyy HH:mm:ss");

            DateTime predefinedAvailAESTtimeNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, GMT, AEST);
            colorsAndStatusLogic("AEST", predefinedAvailAESTtime, olapAvailAESTtime, predefinedAvailAESTtimeNow, predefinedAvailAESTtimeNow.ToString().Split(' ')[0]);
        }


        private void colorsAndStatusLogic(string zone, DateTime predefinedAvail, DateTime olapAvail, DateTime timeNow, string currentDay)
        {
            if (zone == "EST")
            {
                System.Diagnostics.Debug.WriteLine("predefinedAvail: " + predefinedAvail.ToString());
                System.Diagnostics.Debug.WriteLine("olapAvailtime: " + olapAvail.ToString());
                System.Diagnostics.Debug.WriteLine("timeNow: " + timeNow.ToString());
                System.Diagnostics.Debug.WriteLine("currentDay: " + currentDay);
            }
            //if the predefined reports are still not ready
            if (predefinedAvail.ToString().Split(' ')[0] != currentDay)
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "MyScript1", "notCurrentDay('"+ zone + "');", true);

                if(zone == "EST")
                {
                    ESTpredefinedReadyEmail = false;

                    TimeSpan ESTPredefinedSLAdifference = timeNow.Subtract(ESTpredefinedSLA);
                    double ESTPredefinedSLAdiff = ESTPredefinedSLAdifference.TotalMinutes;


                    ESTpredefinedDiff.InnerText = "";

                    ESTpredefinedStatus.InnerText = "Not Ready";
                    ESTpredefinedStatus.Style.Add("color", "orange");


                    if (ESTPredefinedSLAdiff >= 0 && ESTPredefinedSLAdiff < 60)
                    {
                        ESTPredefinedRow.Attributes.Add("class", "warning");
                    }
                    else if (ESTPredefinedSLAdiff >= 60)
                    {
                        ESTPredefinedRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        ESTPredefinedRow.Attributes.Add("class", "info");
                    }
                }

                else if (zone == "JST")
                {
                    JSTpredefinedReadyEmail = false;

                    TimeSpan JSTPredefinedSLAdifference = timeNow.Subtract(JSTpredefinedSLA);
                    double JSTPredefinedSLAdiff = JSTPredefinedSLAdifference.TotalMinutes;

                    System.Diagnostics.Debug.WriteLine("JSTPredefinedSLAdiff: " + JSTPredefinedSLAdiff);

                    JSTpredefinedDiff.InnerText = "";

                    JSTpredefinedStatus.InnerText = "Not Ready";
                    JSTpredefinedStatus.Style.Add("color", "orange");

                    if (JSTPredefinedSLAdiff >= 0 && JSTPredefinedSLAdiff < 60)
                    {
                        JSTPredefinedRow.Attributes.Add("class", "warning");
                    }
                    else if (JSTPredefinedSLAdiff >= 60)
                    {
                        JSTPredefinedRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        JSTPredefinedRow.Attributes.Add("class", "info");
                    }
                }
                else if (zone == "AEST")
                {
                    AESTpredefinedReadyEmail = false;

                    TimeSpan AESTPredefinedSLAdifference = timeNow.Subtract(AESTpredefinedSLA);
                    double AESTPredefinedSLAdiff = AESTPredefinedSLAdifference.TotalMinutes;

                    AESTpredefinedDiff.InnerText = "";

                    AESTpredefinedStatus.InnerText = "Not Ready";
                    AESTpredefinedStatus.Style.Add("color", "orange");

                    if (AESTPredefinedSLAdiff >= 0 && AESTPredefinedSLAdiff < 60)
                    {
                        AESTPredefinedRow.Attributes.Add("class", "warning");
                    }
                    else if (AESTPredefinedSLAdiff >= 60)
                    {
                        AESTPredefinedRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        AESTPredefinedRow.Attributes.Add("class", "info");
                    }
                }
            }
            else
            {
                //the predefined reports are ready
                if (zone == "EST")
                {
                    TimeSpan ESTPredefinedSLAdifference = predefinedAvail.Subtract(ESTpredefinedSLA);
                    double ESTPredefinedSLAdiff = ESTPredefinedSLAdifference.TotalMinutes;


                    if (ESTPredefinedSLAdifference.TotalMinutes > 0) //Check if we surpassed the internal SLA
                    {

                        if(ESTpredefinedReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                        {
                            ESTpredefinedReadyEmail = true;
                            //sendReadyEmail("EST", "Predefined");
                        }

                        ESTpredefinedDiff.InnerText = Math.Floor(ESTPredefinedSLAdifference.TotalMinutes).ToString();

                        ESTpredefinedStatus.InnerText = "Were delivered with Delay";
                        ESTpredefinedStatus.Style.Add("color", "red");

                        if (ESTPredefinedSLAdifference.TotalMinutes < 60)
                        {
                            ESTPredefinedRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            ESTPredefinedRow.Attributes.Add("class", "danger");
                        }
                    }
                    else //they were ready before the internal SLA
                    {
                        ESTpredefinedStatus.InnerText = "Ready";
                        ESTpredefinedStatus.Style.Add("color", "green");

                        ESTPredefinedRow.Attributes.Add("class", "success");
                    }
                }
                else if (zone == "JST")
                {
                    TimeSpan JSTPredefinedSLAdifference = predefinedAvail.Subtract(JSTpredefinedSLA);
                    double JSTPredefinedSLAdiff = JSTPredefinedSLAdifference.TotalMinutes;


                    if (JSTPredefinedSLAdifference.TotalMinutes > 0) //Check if we surpassed the internal SLA
                    {

                        if (JSTpredefinedReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                        {
                            JSTpredefinedReadyEmail = true;
                            //sendReadyEmail("JST", "Predefined");
                        }

                        JSTpredefinedDiff.InnerText = Math.Floor(JSTPredefinedSLAdifference.TotalMinutes).ToString();

                        JSTpredefinedStatus.InnerText = "Were delivered with Delay";
                        JSTpredefinedStatus.Style.Add("color", "red");

                        if (JSTPredefinedSLAdifference.TotalMinutes < 60)
                        {
                            JSTPredefinedRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            JSTPredefinedRow.Attributes.Add("class", "danger");
                        }
                    }
                    else //they were ready before the internal SLA
                    {
                        JSTpredefinedStatus.InnerText = "Ready";
                        JSTpredefinedStatus.Style.Add("color", "green");

                        JSTPredefinedRow.Attributes.Add("class", "success");
                    }
                }
                else if (zone == "AEST")
                {
                    TimeSpan AESTPredefinedSLAdifference = predefinedAvail.Subtract(AESTpredefinedSLA);
                    double AESTPredefinedSLAdiff = AESTPredefinedSLAdifference.TotalMinutes;


                    if (AESTPredefinedSLAdifference.TotalMinutes > 0) //Check if we surpassed the internal SLA
                    {

                        if (AESTpredefinedReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                        {
                            AESTpredefinedReadyEmail = true;
                            //sendReadyEmail("AEST", "Predefined");
                        }

                        AESTpredefinedDiff.InnerText = Math.Floor(AESTPredefinedSLAdifference.TotalMinutes).ToString();

                        AESTpredefinedStatus.InnerText = "Were delivered with Delay";
                        AESTpredefinedStatus.Style.Add("color", "red");

                        if (AESTPredefinedSLAdifference.TotalMinutes < 60)
                        {
                            AESTPredefinedRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            AESTPredefinedRow.Attributes.Add("class", "danger");
                        }
                    }
                    else //they were ready before the internal SLA
                    {
                        AESTpredefinedStatus.InnerText = "Ready";
                        AESTpredefinedStatus.Style.Add("color", "green");

                        AESTPredefinedRow.Attributes.Add("class", "success");
                    }
                }
            }


            //if the olap reports are still not ready
            if (olapAvail.ToString().Split(' ')[0] != currentDay)
            {
                if (zone == "EST")
                {
                    ESTOLAPReadyEmail = false;

                    TimeSpan ESTOlapSLAdifference = timeNow.Subtract(ESTolapSLA);
                    double ESTOlapSLAdiff = ESTOlapSLAdifference.TotalMinutes;

                    System.Diagnostics.Debug.WriteLine("ESTOlapSLAdiff: " + ESTOlapSLAdiff);

                    ESTolapDiff.InnerText = "";

                    ESTolapStatus.InnerText = "Not Ready";
                    ESTolapStatus.Style.Add("color", "orange");


                    if (ESTOlapSLAdiff >= 0 && ESTOlapSLAdiff < 60)
                    {
                        ESTOlapRow.Attributes.Add("class", "warning");
                    }
                    else if (ESTOlapSLAdiff >= 60)
                    {
                        ESTOlapRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        ESTOlapRow.Attributes.Add("class", "info");
                    }
                }

                else if (zone == "JST")
                {
                    JSTOLAPReadyEmail = false;

                    TimeSpan JSTOlapSLAdifference = timeNow.Subtract(JSTolapSLA);
                    double JSTOlapSLAdiff = JSTOlapSLAdifference.TotalMinutes;

                    JSTolapDiff.InnerText = "";

                    JSTolapStatus.InnerText = "Not Ready";
                    JSTolapStatus.Style.Add("color", "orange");


                    if (JSTOlapSLAdiff >= 0 && JSTOlapSLAdiff < 60)
                    {
                        JSTOlapRow.Attributes.Add("class", "warning");
                    }
                    else if (JSTOlapSLAdiff >= 60)
                    {
                        JSTOlapRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        JSTOlapRow.Attributes.Add("class", "info");
                    }
                }
                else if (zone == "AEST")
                {
                    AESTOLAPReadyEmail = false;

                    TimeSpan AESTOlapSLAdifference = timeNow.Subtract(AESTolapSLA);
                    double AESTOlapSLAdiff = AESTOlapSLAdifference.TotalMinutes;

                    AESTolapDiff.InnerText = "";

                    AESTolapStatus.InnerText = "Not Ready";
                    AESTolapStatus.Style.Add("color", "orange");


                    if (AESTOlapSLAdiff >= 0 && AESTOlapSLAdiff < 60)
                    {
                        AESTOlapRow.Attributes.Add("class", "warning");
                    }
                    else if (AESTOlapSLAdiff >= 60)
                    {
                        AESTOlapRow.Attributes.Add("class", "danger");
                    }
                    else
                    {
                        AESTOlapRow.Attributes.Add("class", "info");
                    }
                }
            }
            else
            {
                //the olap reports are ready
                if (zone == "EST")
                {
                    TimeSpan ESTOlapSLAdifference = olapAvail.Subtract(ESTolapSLA);
                    double ESTOlapSLAdiff = ESTOlapSLAdifference.TotalMinutes;

                    //System.Diagnostics.Debug.WriteLine("ESTOlapSLAdiff: " + ESTOlapSLAdiff);

                    if (ESTOlapSLAdifference.TotalMinutes > 0) //Check if we surpassed the internal SLA
                    {

                        if (ESTOLAPReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                        {
                            ESTOLAPReadyEmail = true;
                            //sendReadyEmail("EST", "OLAP");
                        }

                        ESTolapDiff.InnerText = Math.Floor(ESTOlapSLAdifference.TotalMinutes).ToString();

                        ESTolapStatus.InnerText = "Were delivered with Delay";
                        ESTolapStatus.Style.Add("color", "red");

                        if (ESTOlapSLAdifference.TotalMinutes < 60)
                        {
                            ESTOlapRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            ESTOlapRow.Attributes.Add("class", "danger");
                        }
                    }
                    else //they were ready before the internal SLA
                    {
                        ESTolapStatus.InnerText = "Ready";
                        ESTolapStatus.Style.Add("color", "green");

                        ESTOlapRow.Attributes.Add("class", "success");
                    }
                }

                else if (zone == "JST")
                {
                    TimeSpan JSTOlapSLAdifference = olapAvail.Subtract(JSTolapSLA);
                    double JSTOlapSLAdiff = JSTOlapSLAdifference.TotalMinutes;


                    if (JSTOlapSLAdifference.TotalMinutes > 0) //Check if we surpassed the internal SLA
                    {

                        if (JSTOLAPReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                        {
                            JSTOLAPReadyEmail = true;
                            //sendReadyEmail("JST", "OLAP");
                        }

                        JSTolapDiff.InnerText = Math.Floor(JSTOlapSLAdifference.TotalMinutes).ToString();

                        JSTolapStatus.InnerText = "Were delivered with Delay";
                        JSTolapStatus.Style.Add("color", "red");

                        if (JSTOlapSLAdifference.TotalMinutes < 60)
                        {
                            JSTOlapRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            JSTOlapRow.Attributes.Add("class", "danger");
                        }
                    }
                    else //they were ready before the internal SLA
                    {
                        JSTolapStatus.InnerText = "Ready";
                        JSTolapStatus.Style.Add("color", "green");

                        JSTOlapRow.Attributes.Add("class", "success");
                    }
                }

                else if (zone == "AEST")
                {
                    TimeSpan AESTOlapSLAdifference = olapAvail.Subtract(AESTolapSLA);
                    double AESTOlapSLAdiff = AESTOlapSLAdifference.TotalMinutes;


                    if (AESTOlapSLAdifference.TotalMinutes > 0) //Check if we surpassed the internal SLA
                    {

                        if (AESTOLAPReadyEmail == false) //check if we haven't sent a "reports ready" email to noc.support
                        {
                            AESTOLAPReadyEmail = true;
                            //sendReadyEmail("AEST", "OLAP");
                        }

                        AESTolapDiff.InnerText = Math.Floor(AESTOlapSLAdifference.TotalMinutes).ToString();

                        AESTolapStatus.InnerText = "Were delivered with Delay";
                        AESTolapStatus.Style.Add("color", "red");

                        if (AESTOlapSLAdifference.TotalMinutes < 60)
                        {
                            AESTOlapRow.Attributes.Add("class", "warning");
                        }
                        else // >= 60
                        {
                            AESTOlapRow.Attributes.Add("class", "danger");
                        }
                    }
                    else //they were ready before the internal SLA
                    {
                        AESTolapStatus.InnerText = "Ready";
                        AESTolapStatus.Style.Add("color", "green");

                        AESTOlapRow.Attributes.Add("class", "success");
                    }
                }
            }
        }


        private void sendReadyEmail(string timezone, string reportType)
        {
            string smtpAddress = "10.10.2.15";
            int portNumber = 25;
            bool enableSSL = false;
            string emailFrom = "SizmekReports@sizmek.com";
            string emailTo = "noc.support@sizmek.com";
            //string emailTo = "muhamad.kial@sizmek.com";

            string subject = "Report Readiness Notification";

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.CC.Add("muhamad.kial@sizmek.com");
                mail.Subject = subject;
                mail.IsBodyHtml = true;


                mail.Body = "The " + reportType + " reports of " + timezone + " are ready. Please verify & send an update.";

                using (SmtpClient smt1p = new SmtpClient(smtpAddress, portNumber))
                {
                    //    smt1p.Credentials = new NetworkCredential(emailFrom, password);
                    smt1p.EnableSsl = enableSSL;

                    try
                    {
                        smt1p.Send(mail);
                    }
                    catch (Exception e4)
                    {
                        Response.Write(e4.ToString());
                        //errorbox.Items.Add("error1");
                    }
                }
            }
        }

    }
}