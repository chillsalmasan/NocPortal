using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NocPortal
{
    public partial class Homepage : System.Web.UI.Page
    {
        public string delays;
        public string timezoneClicked;
        public string reportClicked;
        public string hoursDurationClicked;
        public string minutesDurationClicked;
        public string timeCreatedClicked;
        public string notificationType;
        public string closedTime;
        public string reason;
        public string resolution;
        public string dateflag;
        public string actualDelay;


        protected void Page_Load(object sender, EventArgs e)
        {

            string todayDate1 = DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            SqlDataReader myReader2 = null;


            SqlConnection conn2 = new SqlConnection("user id=noc;" +
                                                    "password=noc972;" +
                                                    "server=ilnoc01;" +
                                                    "Trusted_Connection=false;" +
                                                    "database=noc; " +
                                                    "connection timeout=30");
            conn2.Open();
            string stmt2 = "SELECT * FROM Maintenance";
            SqlCommand comm2 = new SqlCommand(stmt2, conn2);
            myReader2 = comm2.ExecuteReader();

            DateTime todayDate = DateTime.Parse(todayDate1);

            string todays = "&nbsp&nbsp <b>TODAY'S MAINTENANCES</b><br><br>";
            string s11 = "";
            string f11 = "";
            string sdate = "";
            string fdate = "";
            string stime = "";
            string ftime = "";
            string s44 = "";

            List<string> list = new List<string>();
            Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();

            while (myReader2.Read())
            {
                s11 = myReader2["S_Date"].ToString().Trim();
                sdate = s11.Substring(0, 10);
                stime = s11.Substring(11, 5);
                DateTime dt1 = DateTime.Parse(sdate);

                f11 = myReader2["F_Date"].ToString().Trim();
                fdate = f11.Substring(0, 10);
                ftime = f11.Substring(11, 5);
                DateTime dt2 = DateTime.Parse(fdate);

             

                string module = myReader2["Module"].ToString().Trim();

                if (dt1 == todayDate && dt2 == todayDate)
                {
                    string evnt = "<b><div>" + module + "</b></div>" + myReader2["title"].ToString().Trim() + "<br><div style='color: #e74c3c;'><b>" + stime + "&nbsp-&nbsp" + ftime + " IST</b></div><br><br>";
                    //todays = todays + evnt;
                    if (!dict.ContainsKey(evnt))
                    {
                        dict.Add(evnt, DateTime.Parse(s11));
                    }
                }
                else if (dt1 <= todayDate && dt2 >= todayDate)
                {
                    string evnt = "<b><div>" + module + "</b></div>" + myReader2["title"].ToString().Trim() + "<br><div style='color: #e74c3c;'><b>" + sdate + " " + stime + "&nbsp-&nbsp" + fdate + " " + ftime + " IST</b></div><br><br>";
                    //todays = todays + evnt;

                    dict.Add(evnt, DateTime.Parse(s11));
                }

                //else if (sdate == todayDate && fdate != todayDate) {

                //    todays = todays + myReader2["title"].ToString().Trim() + "<br>" + stime + "&nbsp-&nbsp" + fdate + " at " + ftime + "<br><br>";

                //}
            }


            if (dict.Count > 0)
            {
                int i = 0;
                foreach (KeyValuePair<string, DateTime> entry in dict.OrderBy(x => x.Value).ThenByDescending(x => x.Key))
                {
                    //System.Diagnostics.Debug.WriteLine("{0}. Key={1}, Value={2}", ++i, entry.Key, entry.Value);
                    todays = todays + entry.Key;
                }
                //System.Diagnostics.Debug.WriteLine("The dictionary contained a total of {0} entries.", i);
            }

            if (todays == "&nbsp&nbsp <b>TODAY'S MAINTENANCES</b><br><br>")
            {
                todays = "&nbsp&nbspThere are no events today.<br><br>";
            }

            todayy.Text = todays;

            conn2.Close();
       


        string todayDate11 = DateTime.Now.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            System.Diagnostics.Debug.WriteLine("todayDate =" + todayDate1);
            string timeCreatedClicked2;

            List<string> list2 = new List<string>();
            Dictionary<string, DateTime> dict2 = new Dictionary<string, DateTime>();


            SqlDataReader myReader22 = null;
            SqlDataReader myReader3 = null;
            SqlDataReader myReader55 = null;


            SqlConnection conn22 = new SqlConnection("user id=noc;" +
                                                    "password=noc972;" +
                                                    "server=ilnoc01;" +
                                                    "Trusted_Connection=false;" +
                                                    "database=Delay_notification; " +
                                                    "connection timeout=30");
            conn22.Open();
            string stmt22 = "SELECT * FROM Delays1";
            string stmt33 = "SELECT * FROM notification";
            string stmt55 = "SELECT * FROM notification";
            SqlCommand comm22 = new SqlCommand(stmt22, conn22);
            SqlCommand comm33 = new SqlCommand(stmt33, conn22);
            SqlCommand comm55 = new SqlCommand(stmt55, conn22);


            myReader55 = comm55.ExecuteReader();
            string[] Reason = new string[10000];
            string[] Resolution = new string[10000];
            string[] Resolved = new string[10000];
            string[] Main = new string[10000];

            int k = 0;

            while (myReader55.Read())
            {
                string notificationType = myReader55["Notification_Type"].ToString().Trim();
                if (notificationType.Length > 8) { 
                if (notificationType.Substring(0, 9) == "Analytics")
                {
                    Reason[k] = myReader55["Reason"].ToString().Trim();
                    Resolution[k] = myReader55["Resolution"].ToString().Trim();
                    Resolved[k] = myReader55["Closed_Time"].ToString().Trim();
                    Main[k] = myReader55["Delay_main_id"].ToString().Trim();

                    k++;

                }
                }
            }

            myReader55.Close();


            myReader22 = comm22.ExecuteReader();

            DateTime todayDate3 = DateTime.Parse(todayDate11);

            string todays2 = "&nbsp&nbsp<b>TODAY'S CRITICAL EVENTS</b><br><br>";
            string createdDate = "";
            string res = "";
            string timezoneClicked = "";
            string stime2 = "";
            string timeCreatedClicked = "";
           

            //List<string> list = new List<string>();
            //Dictionary<string, DateTime> dict = new Dictionary<string, DateTime>();

            while (myReader22.Read())
            {
                bool flag = false;

                int index = 0;
                string mainId = myReader22["Main"].ToString().Trim();
                timezoneClicked = myReader22["TZ"].ToString().Trim();
                reportClicked = myReader22["Report"].ToString().Trim();
                minutesDurationClicked = myReader22["DelayInMinuts"].ToString().Trim();
                hoursDurationClicked = myReader22["DelayInHours"].ToString().Trim();
                actualDelay = myReader22["Delay_duration"].ToString().Trim();

                if (actualDelay.Length > 0)
                {
                    actualDelay = actualDelay.Substring(0, 5);
                }

                if (timezoneClicked == "AEST")
                {
                    timezoneClicked = "APAC";
                }
                if (timezoneClicked == "JST")
                {
                    continue;
                }


                for (int j = 0; j < 100; j++)
                {
                    if (Main[j] == mainId)
                    {
                        index = j;
                    }
                }

                timeCreatedClicked = myReader22["Created_time"].ToString().Trim();

                System.Diagnostics.Debug.WriteLine("timeCreatedClicked = " + timeCreatedClicked);

                if (timeCreatedClicked.Length == 0)
                {
                    timeCreatedClicked2 = "00:00:00";
                    timeCreatedClicked = timeCreatedClicked2;
                    continue;
                }
                else
                {
                    timeCreatedClicked = timeCreatedClicked.Substring(0, 5);
                }
                
                

                createdDate = String.Format("{0:s}", myReader22["Created_date"]).Substring(0, 10).Trim();
                string[] strArr = myReader22["Created_date"].ToString().Split(' ');
                string str = strArr[0] + " " + timeCreatedClicked;
                DateTime createdDateAsDateTime = DateTime.Parse(str);
                TimeSpan diff = DateTime.Now.Subtract(createdDateAsDateTime);
                int diffAsInt = (int)diff.TotalHours;
                //System.Diagnostics.Debug.WriteLine("STRRRRRRRRRRRR =" + str);
                //System.Diagnostics.Debug.WriteLine("diffAsInt =" + diffAsInt);
                //System.Diagnostics.Debug.WriteLine("date timee: " + createdDate + " " + timeCreatedClicked);

                DateTime diff1 = DateTime.Now.AddDays(-1);
                string yesterdayDate = diff1.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                string time = diff1.ToString("HH:mm");
                string hoursNow = time.Substring(0, 2);
                string minutesNow = time.Substring(3, 2);
                string CreatedHour = timeCreatedClicked.Substring(0, 2);
                string CreatedMinute = timeCreatedClicked.Substring(3, 2);

                if (createdDate == yesterdayDate)
                {
                    if (Convert.ToInt32(CreatedHour) > Convert.ToInt32(hoursNow))
                    {
                        flag = true;
                    }
                    else
                    {
                        if (Convert.ToInt32(CreatedHour) == Convert.ToInt32(hoursNow))
                        {
                            if (Convert.ToInt32(CreatedMinute) >= Convert.ToInt32(minutesNow))
                            {
                                flag = true;
                            }
                        }
                    }

                }

                if (flag == true)
                {
                    dateflag = "<font color='red'><b>" + yesterdayDate + "</b></font>" + " <br>";
                }
                else {
                    dateflag = "";
                }




                if (diffAsInt <= 24 && diffAsInt >= 0)

                //if (createdDate == todayDate11)
                {

                    int intHoursDurationClicked = Convert.ToInt32(hoursDurationClicked);
                    int intMinutesDurationClicked = Convert.ToInt32(minutesDurationClicked);

                    if (intHoursDurationClicked == 0)
                    {
                        if (intMinutesDurationClicked == 0)
                        {
                            if (Resolved[index] == "")
                            {
                                string evnt = "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - Delay of TBD in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - Delay of " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                            else {
                                string evnt = "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - Delay of TBD in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><b>Actual delay duration:</b> " + actualDelay + "<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - Delay of " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                        }

                        else { 

                        if (Resolved[index] == "")
                        {
                            string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                            //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                            DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                            dict2.Add(evnt, created);
                        }
                        else {
                            string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><b>Actual delay duration:</b> " + actualDelay + "<br><br>";
                            //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><br>";
                            DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                            dict2.Add(evnt, created);
                        }
                    }
                }

                else {
                    if (intHoursDurationClicked > 1)
                    {
                        if (intMinutesDurationClicked > 0)
                        {
                            if (Resolved[index] == "")
                            {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ". <br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ". <br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);

                            }
                            else {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ". <div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><b>Actual delay duration:</b> " + actualDelay + "<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ". <div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                        }
                        else {
                            if (Resolved[index] == "")
                            {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);

                            }
                            else {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><b>Actual delay duration:</b> " + actualDelay + "<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hours in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                        }


                    }
                    //print hour instead of hours.
                    else {
                        if (intMinutesDurationClicked > 0)
                        {
                            if (Resolved[index] == "")
                            {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                            else {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><b>Actual delay duration:</b> " + actualDelay + "<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour and " + minutesDurationClicked + " minutes in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                        }

                        else {
                            if (Resolved[index] == "")
                            {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                            else {
                                string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><b>Actual delay duration:</b> " + actualDelay + "<br><br>";
                                // res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked.Substring(0, 5) + "</b></div> - Delay of " + hoursDurationClicked + " hour in " + reportClicked + " reports for " + timezoneClicked + " Timezone.<br><b>Reason:</b> " + Reason[index] + ".<br><b>Resolution:</b> " + Resolution[index] + ".<div style='color:green; display:inline-block'> (Resolved " + Resolved[index].Substring(0, 5) + ").</div><br><br>";
                                DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                                dict2.Add(evnt, created);
                            }
                        }
                    }
                }


                }

            }



            myReader22.Close();

            myReader3 = comm33.ExecuteReader();
            while (myReader3.Read())
            {
                bool flag = false;

                notificationType = myReader3["Notification_Type"].ToString().Trim();
                if(notificationType.Length > 8) { 
                if (notificationType.Substring(0, 9) != "Analytics")
                {
                    timeCreatedClicked = myReader3["Created_time"].ToString().Trim();
                    closedTime = myReader3["Closed_Time"].ToString().Trim();
                    reason = myReader3["Reason"].ToString().Trim();
                    resolution = myReader3["Resolution"].ToString().Trim();


                    createdDate = String.Format("{0:s}", myReader3["Created_date"]).Substring(0, 10).Trim();

                    string[] strArr = myReader3["Created_date"].ToString().Split(' ');
                    string str = strArr[0] + " " + timeCreatedClicked;
                    DateTime createdDateAsDateTime = DateTime.Parse(str);
                    TimeSpan diff = DateTime.Now.Subtract(createdDateAsDateTime);


                    DateTime diff1 = DateTime.Now.AddDays(-1);
                    string yesterdayDate = diff1.ToString("yyyy-MM-dd", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                    string time = diff1.ToString("HH:mm");
                    string hoursNow = time.Substring(0, 2);
                    string minutesNow = time.Substring(3, 2);
                    string CreatedHour = timeCreatedClicked.Substring(0, 2);
                    string CreatedMinute = timeCreatedClicked.Substring(3, 2);

                    //System.Diagnostics.Debug.WriteLine("yesterdaaaay " + yesterdayDate);


                    if (createdDate == yesterdayDate)
                    {
                        if (Convert.ToInt32(CreatedHour) > Convert.ToInt32(hoursNow))
                        {
                            flag = true;
                        }
                        else
                        {
                            if (Convert.ToInt32(CreatedHour) == Convert.ToInt32(hoursNow))
                            {
                                if (Convert.ToInt32(CreatedMinute) >= Convert.ToInt32(minutesNow))
                                {
                                    flag = true;
                                }
                            }
                        }

                    }

                    if (flag == true)
                    {
                        dateflag = "<font color='red'><b>" + yesterdayDate + "</b></font>" + " <br>";
                    }
                    else {
                        dateflag = "";
                    }


                    int diffAsInt = (int)diff.TotalHours;
                    System.Diagnostics.Debug.WriteLine("diff = " + diffAsInt);
                    if (diffAsInt <= 24 && diffAsInt >= 0)

                    //if (createdDate == todayDate1)
                    {

                        if (closedTime.Length > 0)
                        {
                            closedTime = closedTime.Substring(0, 5);
                        }
                        if (timeCreatedClicked.Length > 1)
                        {
                            timeCreatedClicked = timeCreatedClicked.Substring(0, 5);
                        }
                        else { }

                        if (closedTime == "")
                        {
                            string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - " + notificationType + ".<br> Reason: " + reason + ".<br> Resolution: " + resolution + ". <br><br>";
                            //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - " + notificationType + ".<br> Reason: " + reason + ".<br> Resolution: " + resolution + ". <br><br>";
                            DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                            dict2.Add(evnt, created);
                        }
                        else {

                            //System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!! " + createdDate + " " + timeCreatedClicked);
                            string evnt = dateflag + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - " + notificationType + ".<br> Reason: " + reason + ".<br> Resolution: " + resolution + ".<div style='color:green; display:inline-block'>(Resolved " + closedTime + ").</div><br><br>";
                            //res = res + "<div style='color:red; display:inline-block'><b>" + timeCreatedClicked + "</b></div> - " + notificationType + ".<br> Reason: " + reason + ".<br> Resolution: " + resolution + ".<div style='color:green; display:inline-block'>(Resolved " + closedTime + ").</div><br><br>";
                            DateTime created = DateTime.Parse(createdDate + " " + timeCreatedClicked);
                            dict2.Add(evnt, created);
                        }
                    }

                }
            }

            }


            if (dict2.Count > 0)
            {
                int i = 0;
                foreach (KeyValuePair<string, DateTime> entry in dict2.OrderBy(x => x.Value).ThenByDescending(x => x.Key))
                {
                    //System.Diagnostics.Debug.WriteLine("{0}. Key={1}, Value={2}", ++i, entry.Key, entry.Value);
                    todays2 = todays2 + entry.Key;
                }
                //System.Diagnostics.Debug.WriteLine("The dictionary contained a total of {0} entries.", i);
            }

            //todays2 = todays2 + res;


            if (todays2 == "&nbsp&nbsp<b>TODAY'S CRITICAL EVENTS</b><br><br>")
            {
                todays2 = "<br>&nbsp&nbsp&nbspThere are no critical events today.";
            }

            todayy.Text = todays + "-------------------------------------------<br><br>" + todays2;

            myReader3.Close();

            conn22.Close();



        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Button1.Click += new EventHandler(this.GreetingBtn_Click);
        }

        protected void GreetingBtn_Click(object sender, EventArgs e)
        {

        }
    }
}