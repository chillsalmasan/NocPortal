using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace NocPortal
{
    public partial class ShiftSummary : System.Web.UI.Page
    {
        public string events;
        static string Shift_id; // static variable of shift ID
        static bool update = false;
        string userName;
        static StringBuilder Table = new StringBuilder();
        static string eventIdLast;
        static string SubeventLast;
        static string shift;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Timer1.Enabled == false)
            {
                Timer1.Enabled = true;
                selectedTimer.Enabled = false;
            }
            shift = sql_query("select top 1 Shift_title from Shift ORDER BY ID DESC");
            Shift_label.Text = "Good " + getShift_title(); // insert into shift lable the shift title
            if (!shift.Contains(getShift_title()))
            {
                      Button2_Click(sender, e);
            }
            Shift_id = GetShiftID();// insert into Shift variable
            sql_query_table("");
            userName = Page.User.Identity.Name;
            string[] pathArr = userName.Split('\\');
            userName = pathArr.Last().ToString();
            if (!Page.IsPostBack)
            {
                loadEventsForDDL();
                loadActionsForDDL();
                try {
                    string result = userName.Substring(0, userName.IndexOf('.'));
                    currentUsers.Text = result;
                }
                catch { }
               

            }

            eventIdLast = sql_query_returnLastUpdate().ToString();
            SubeventLast = GetLastSubEventId();
        }


        private void loadEventsForDDL()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=SHIFT_summary_new;" + "connection timeout=30");
            conn.Open();
            string stmt5 = "SELECT * FROM Events;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            while (myReader.Read())
            {
                eventsDDL.Items.Add(myReader["event"].ToString());
            }
            myReader.Close();
            conn.Close();
        }

        private void loadActionsForDDL()
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=SHIFT_summary_new;" + "connection timeout=30");
            conn.Open();
            string stmt5 = "SELECT * FROM Actions;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            myReader = comm4.ExecuteReader();
            while (myReader.Read())
            {
                actionsDDL.Items.Add(myReader["action"].ToString());
            }
            myReader.Close();
            conn.Close();
        }


        protected void eventsDDL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void actionsDDL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public string GetShiftID() // get the last record from Shift table (the current shift Id)
        {
            return sql_query("SELECT TOP 1 * FROM Shift ORDER BY ID DESC");
        }

        public string GetPreviousShiftID() // get the last record from Shift table (the current shift Id)
        {
            return sql_query("SELECT TOP 1 * From (select Top 2 * from [SHIFT_summary_new].[dbo].[Shift] ORDER BY Id DESC) x ORDER BY Id");
        }

        public string GetLastEventId() // get the last record from Shift table (the current shift Id)
        {
            return sql_query("SELECT TOP 1 Entery_id FROM Entery ORDER BY Entery_id DESC");
        }

        public string GetLastSubEventId() // get the last record from Shift table (the current shift Id)
        {
            return sql_query("SELECT TOP 1 Sub_Id FROM Sub_Entery ORDER BY Sub_Id DESC");
        }

        protected void copyCriticalEntries()
        {
            SQL_conectionsAndCommand("update Entery Set IsCritical='false' where IsResolved='true'");
            string shift_id = GetShiftID().ToString();
            SQL_conectionsAndCommand("INSERT INTO Entery ([Event],[IsCritical],[IsResolved],[User_name],[CreatedTime],[CreateDate],[ResolvedTime]) SELECT [Event],[IsCritical],[IsResolved],[User_name],[CreatedTime],[CreateDate],[ResolvedTime] FROM Entery WHERE [IsCritical]='true' and [Shift_Id]='"+GetPreviousShiftID()+"'");
            SQL_conectionsAndCommand("UPDATE Entery SET Shift_Id='" + GetShiftID() + "' where Shift_Id is NULL");
        }

        protected void copyFollowedEntries()
        {
            string shift_id = GetShiftID().ToString();
            SQL_conectionsAndCommand("INSERT INTO Entery ([Event],[IsCritical],[IsResolved],[IsFollowed],[User_name],[CreatedTime],[CreateDate],[ResolvedTime]) SELECT [Event],[IsCritical],[IsResolved],[Isfollowed],[User_name],[CreatedTime],[CreateDate],[ResolvedTime] FROM Entery WHERE [IsFollowed]='true' and [Shift_Id]='" + GetPreviousShiftID() + "'");
            SQL_conectionsAndCommand("UPDATE Entery SET Shift_Id='" + GetShiftID() + "' where Shift_Id is NULL");
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if(Timer1.Enabled==false)
            {
                Timer1.Enabled = true;
                selectedTimer.Enabled = false;
            }
            if (eventDetails.Value == "")
            {
                if(eventsDDL.SelectedIndex != 0)
                {
                    hiddenSelected.Value = "";
                    hiddenSelectedSubEvent.Value = "";
                    eventDetails.Value = eventsDDL.SelectedItem.Text;

                    if(actionsDDL.SelectedIndex != 0)
                    {
                        eventDetails.Value = eventDetails.Value + " - " + actionsDDL.SelectedItem.Text;
                    }
                }
                else
                {
                    hiddenSelected.Value = "";
                    hiddenSelectedSubEvent.Value = "";
                    eventDetails.Value = "";

                    eventsDDL.ClearSelection();
                    actionsDDL.ClearSelection();

                    return;
                }

                eventsDDL.ClearSelection();
                actionsDDL.ClearSelection();
            }
            if (hiddenSelected.Value != "" && hiddenSelectedSubEvent.Value == "" && update == true) // check if it update case for event
            {
                string id = hiddenSelected.Value.Split('-')[1];
                SQL_conectionsAndCommand("update [SHIFT_summary_new].[dbo].[Entery] set Event='" + eventDetails.Value + "' where Entery_id='" + id + "'");
                SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");
                hiddenSelected.Value = "";
                hiddenSelectedSubEvent.Value = "";
                eventDetails.Value = "";
                update = false;
                return;
            }
            if (hiddenSelected.Value != "" && hiddenSelectedSubEvent.Value != "" && update == true) // check if it update case for subevent
            {

                SQL_conectionsAndCommand("update [SHIFT_summary_new].[dbo].[Sub_Entery] set Title='" + eventDetails.Value + "' where Sub_Id='" + hiddenSelectedSubEvent.Value + "'");
                SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");
                hiddenSelected.Value = "";
                hiddenSelectedSubEvent.Value = "";
                eventDetails.Value = "";
                update = false;
                return;
            }
            if (hiddenSelected.Value == "") //if it's main event
            {
                if (eventDetails.Value == "")
                {
                    //Button1.BorderColor = System.Drawing.Color.Red;
                    eventDetails.Style.Add("border-color", "red");
                    return;
                }

                SQL_conectionsAndCommand("insert into Entery (Shift_Id,Event,User_name,CreatedTime,CreateDate) Values(" + "'" + Shift_id + "'" + "," + "'" + eventDetails.Value.ToString() + "'" + "," + "'" + userName + "'" + "," + "'" + DateTime.Now.ToString("HH:mm:ss tt") + "'" + "," + "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'" + ")");
                sql_query_table("aa");
                SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");
            }
            else //if it's a sub entry
            {
                if (Int32.Parse(hiddenSelected.Value) < 0)
                {
                    hiddenSelected.Value = "" + Int32.Parse(hiddenSelected.Value) * -1;
                }
                string subTitle;
                bool isCall = false;
                if (callExpertBox.Checked == true)
                {
                    subTitle = "Call " + callTo.SelectedValue.ToString() + " " + eventDetails.Value.ToString();
                    isCall = true;
                }
                else
                {
                    subTitle = eventDetails.Value.ToString();
                }
               SQL_conectionsAndCommand("insert into Sub_Entery (Entery_Id,Title,EventTime,User_name,IsCalled) Values(" + "'" + hiddenSelected.Value + "'" + "," + "'" + subTitle + "'" + "," + "'" + DateTime.Now.ToString("HH:mm:ss tt") + "'" + "," + "'" + userName + "','" + isCall + "')");
               SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");

            }

            hiddenSelected.Value = "";
            hiddenSelectedSubEvent.Value = "";
            eventDetails.Value = "";
        }

        [System.Web.Services.WebMethod]
        public static string sql_query_table(string Id)
        {
            if (Id == "")
            {
                Id = Shift_id;
            }
            System.Data.SqlClient.SqlConnection sqlConnection1 =
    new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=Delay_notification; " + "connection timeout=30;  MultipleActiveResultSets=True;");
            using (sqlConnection1)
            {
                SqlCommand command = new SqlCommand(
                  "SELECT  [User_name],[CreatedTime],[Event],[Entery_id],[IsCritical],[IsResolved],[IsPinned],[IsFollowed] FROM [SHIFT_summary_new].[dbo].[Entery] where Shift_Id=" + "'" + Id + "'",
                  sqlConnection1);
                sqlConnection1.Open();
                StringBuilder htmlTable = new StringBuilder();
                SqlDataReader reader = command.ExecuteReader();
                string EventClassStatus = null;
                string icon = null;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            if (reader.GetBoolean(4) == true)
                            {
                                EventClassStatus = "danger";
                            }
                        }
                        catch { }
                        try
                        {
                            if (reader.GetBoolean(5) == true)
                            {
                                EventClassStatus = "success";
                            }
                        }
                        catch { }

                        try
                        {
                            if (reader.GetBoolean(7) == true)
                            {
                                EventClassStatus = "warning";
                                icon = "glyphicon glyphicon-paperclip";
                            }
                        }
                        catch { }
                        try
                        {
                            if (reader.GetBoolean(6) == true)
                            {
                                icon = "glyphicon glyphicon-pushpin";
                            }
                        }
                        catch { }
                        htmlTable.AppendLine("<tr Class ='" + EventClassStatus + "' name='Event'  id='" + reader.GetInt32(3).ToString() + "' onclick='onEventSelect(this);'>");
                        htmlTable.AppendLine("<td style='" + "width: 200px;" + "><div style='display:inline-block' id='icon' class='" + icon + "'>" + reader.GetString(0) + " " + reader.GetTimeSpan(1).ToString() + "</div></td>");
                        htmlTable.AppendLine("<td type='eventText' colspan='" + "2'" + ">" + reader.GetString(2) + "</td>");
                        htmlTable.AppendLine("</tr>");
                        EventClassStatus = null;
                        icon = null;
                        System.Data.SqlClient.SqlConnection sqlConnection2 = new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=Delay_notification; " + "connection timeout=30; MultipleActiveResultSets=True;");
                        using (sqlConnection2)
                        {
                            SqlCommand command1 = new SqlCommand(
                  " SELECT[User_name],[EventTime],[Title],[Sub_Id] FROM[SHIFT_summary_new].[dbo].[Sub_Entery] where Entery_Id=" + "'" + reader.GetInt32(3).ToString() + "'", sqlConnection1);
                            sqlConnection2.Open();
                            SqlDataReader reader1 = command1.ExecuteReader();
                            while (reader1.Read())
                            {
                                htmlTable.AppendLine("<tr name='subEvent' id='" + reader1.GetInt32(3).ToString() + "'" + "parentID = '" + reader.GetInt32(3).ToString() + "' onclick='onSubEventSelect(this);' >");
                                htmlTable.AppendLine("<td style='width: 200px; border: 0;'></td>");
                                htmlTable.AppendLine("<td style='width: 200px; border: 0;'>" + reader1.GetString(0) + " " + reader1.GetTimeSpan(1).ToString() + "</td>");
                                htmlTable.AppendLine("<td style='border: 0;' type='eventText'>" + reader1.GetString(2) + "</td>");
                                htmlTable.AppendLine("</tr>");
                            }

                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }
                
                reader.Close();
                
                sqlConnection1.Close();
                Table = htmlTable;
                return htmlTable.ToString();
            }
        }

        [System.Web.Services.WebMethod]
        public static string return_lastEventId()
        {
            return eventIdLast;
        }

        [System.Web.Services.WebMethod]
        public static string return_lastSubEventId()
        {
            return SubeventLast;
        }

        public void SQL_conectionsAndCommand(string command)
        {
            //  try {
            System.Data.SqlClient.SqlConnection sqlConnection1 =
 new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database= SHIFT_summary_new; " + "connection timeout=30");
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = command;
            cmd.Connection = sqlConnection1;
            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
            //     }
            //   catch { errorbox.Items.Add("Error to add/update/delete data from DATABASE"); Delete_Data.Enabled = true; }
        }

        public string sql_query(string command1) // returning the value of the shift 
        {
            System.Data.SqlClient.SqlConnection sqlConnection1 =
    new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=SHIFT_summary_new; " + "connection timeout=30");
            using (sqlConnection1)
            {
                SqlCommand command = new SqlCommand(command1, sqlConnection1);
                sqlConnection1.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            return reader.GetInt32(0).ToString();
                        }
                        catch
                        {
                            return reader.GetString(0);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return null;
                }
                reader.Close();
            }
            return null;
        }

        public TimeSpan sql_query_returnLastUpdate() // returning the value of the shift 
        {
            System.Data.SqlClient.SqlConnection sqlConnection1 =
    new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=SHIFT_summary_new; " + "connection timeout=30");
            using (sqlConnection1)
            {
                SqlCommand command = new SqlCommand("SELECT TOP 1 Time FROM UpdateTime ORDER BY Id DESC", sqlConnection1);
                sqlConnection1.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                            return reader.GetTimeSpan(0);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                   // return null;
                }
                reader.Close();
            }

            return DateTime.Now.TimeOfDay;


        }

        public void Insert_New_shift() // Add New shift into Shift Table
        {
            TimeSpan start = new TimeSpan(07, 30, 0); //10 o'clock
            TimeSpan end = new TimeSpan(15, 30, 0); //12 o'clock
            DateTime Time = DateTime.Now;
            TimeZoneInfo CebuTime = TimeZoneInfo.FindSystemTimeZoneById("W. Australia Standard Time");
            Time = TimeZoneInfo.ConvertTime(Time, CebuTime);
            TimeSpan now = Time.TimeOfDay;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string shift_title = "";
            if (now > start && now < end)
                shift_title = "Morning";
            else
            {
                start = new TimeSpan(15, 30, 0);
                end = new TimeSpan(23, 30, 0);
                if (now > start && now < end)
                    shift_title = "Evening";
                else
                {
                        shift_title = "Night";
                }
            }
            Shift_label.Text = shift_title;
            SQL_conectionsAndCommand("insert into Shift(Date,Shift_title) Values(" + "'" + date + "'" + "," + "'" + shift_title + "'" + ")");

        }

        protected string getShift_title() // Returning a shift title according to the time of the day
        {
            TimeSpan start = new TimeSpan(07, 30, 0); //07:30 o'clock
            TimeSpan end = new TimeSpan(15, 30, 0); //15:30 o'clock
            DateTime Time = DateTime.Now;
            TimeZoneInfo CebuTime = TimeZoneInfo.FindSystemTimeZoneById("W. Australia Standard Time");
            Time = TimeZoneInfo.ConvertTime(Time, CebuTime);
            TimeSpan now = Time.TimeOfDay;
            string shift_title = "";
            if (now > start && now < end)
                return "Morning";
            else
            {
                start = new TimeSpan(15, 30, 0);
                end = new TimeSpan(23, 30, 0);
                if (now > start && now < end)
                    return "Evening";
            }
            return "Night";
        }

        protected void Button2_Click(object sender, EventArgs e) //  
        {
            Insert_New_shift();
            Shift_id = GetShiftID();
            copyCriticalEntries();
            copyFollowedEntries();
            sql_query_table("");
            SQL_conectionsAndCommand("Delete from UpdateTime");
            SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");
        }

        protected void PreviousShift_Click(object sender, EventArgs e)
        {
            string Shift_id_Previous = GetPreviousShiftID();
            table_container_prev_shift.InnerHtml = sql_query_table(Shift_id_Previous);
            string shift_name = sql_query("select [Shift_title] from Shift where Id='" + Shift_id_Previous + "'");
            Prev_shift_Modal_header.InnerText = shift_name;
           // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "jQuery.noConflict();openModal_prev_shift()", true);
            Page.ClientScript.RegisterStartupScript(GetType(), "openModal", "jQuery.noConflict(); openModal_prev_shift();stopTimer();", true);
        }

        protected void CriticalButton_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                Timer1.Enabled = true;
                selectedTimer.Enabled = false;
            }
            if (hiddenSelected.Value == "")
            {
                return;
            }
            if (Int32.Parse(hiddenSelected.Value) < 0)
            {
                hiddenSelected.Value = "" + Int32.Parse(hiddenSelected.Value) * -1;
            }

            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=SHIFT_summary_new; " + "connection timeout=30");
            using (sqlConnection1)
            {
                SqlCommand command = new SqlCommand("UPDATE Entery SET IsCritical = 1, IsResolved = 0, Isfollowed=0 WHERE Entery_id =" + Int32.Parse(hiddenSelected.Value) + ";", sqlConnection1);
                sqlConnection1.Open();

                SqlDataReader reader = command.ExecuteReader();


                reader.Close();
            }
            SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");

            hiddenSelected.Value = "";

            sqlConnection1.Close();
            return;
        }

        protected void FollowUP_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                Timer1.Enabled = true;
                selectedTimer.Enabled = false;
            }
            if (hiddenSelected.Value == "")
            {
                return;
            }
            if (Int32.Parse(hiddenSelected.Value) < 0)
            {
                hiddenSelected.Value = "" + Int32.Parse(hiddenSelected.Value) * -1;
            }

            SQL_conectionsAndCommand("UPDATE Entery SET IsCritical = 0, IsResolved = 0, IsFollowed=1 WHERE Entery_id ='" + Int32.Parse(hiddenSelected.Value) + "'");
            SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");
            hiddenSelected.Value = "";
        }

        protected void ResolvedButton_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                Timer1.Enabled = true;
                selectedTimer.Enabled = false;
            }
            if (hiddenSelected.Value == "")
            {
                return;
            }
            if (Int32.Parse(hiddenSelected.Value) < 0)
            {
                hiddenSelected.Value = "" + Int32.Parse(hiddenSelected.Value) * -1;
            }

            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=SHIFT_summary_new; " + "connection timeout=30");
            using (sqlConnection1)
            {
                SqlCommand command = new SqlCommand("UPDATE Entery SET IsCritical = 'false', IsResolved = 1, Isfollowed=0  WHERE Entery_id =" + Int32.Parse(hiddenSelected.Value) + ";", sqlConnection1);
                sqlConnection1.Open();

                SqlDataReader reader = command.ExecuteReader();


                reader.Close();
            }
            SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");

            hiddenSelected.Value = "";

            sqlConnection1.Close();
            return;
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            if (hiddenSelected.Value == "")
            {
                return;
            }
            if (Int32.Parse(hiddenSelected.Value) < 0)
            {
                hiddenSelected.Value = "" + Int32.Parse(hiddenSelected.Value) * -1;
            }

            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database=SHIFT_summary_new; " + "connection timeout=30");
            using (sqlConnection1)
            {
                SqlCommand command = new SqlCommand("UPDATE Entery SET IsCritical = 'false', IsResolved = 'false', IsFollowed=0  WHERE Entery_id =" + Int32.Parse(hiddenSelected.Value) + ";", sqlConnection1);
                sqlConnection1.Open();

                SqlDataReader reader = command.ExecuteReader();


                reader.Close();
            }
            SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");
            hiddenSelected.Value = "";
            hiddenSelectedSubEvent.Value = "";
            eventDetails.Value = "";
            update = false;
            sqlConnection1.Close();
            return;
        }

        protected void Edit_event_Click(object sender, EventArgs e)
        {
            eventDetails.Value = Hiddentext.Value;
            update = true;
        }

        protected void Remove_entry_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                Timer1.Enabled = true;
                selectedTimer.Enabled = false;
            }
            if (hiddenSelected.Value != "" && hiddenSelectedSubEvent.Value == "")
            {
                string id = hiddenSelected.Value.Split('-')[1];
                SQL_conectionsAndCommand("delete from [SHIFT_summary_new].[dbo].[Entery] where Entery_id='" + id + "'");
                SQL_conectionsAndCommand("delete from [SHIFT_summary_new].[dbo].[Sub_Entery] where Entery_id='" + id + "'");
                SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");
            }
            else
            {
                if (hiddenSelected.Value != "" && hiddenSelectedSubEvent.Value != "")
                {
                    SQL_conectionsAndCommand("delete from [SHIFT_summary_new].[dbo].[Sub_Entery] where Sub_Id='" + hiddenSelectedSubEvent.Value + "'");
                    SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");

                }
            }
            hiddenSelectedSubEvent.Value = "";
            hiddenSelected.Value = "";
        }

        public bool sql_query_check_if_pinned(string id) // return true or false 
        {
            System.Data.SqlClient.SqlConnection sqlConnection1 =
new System.Data.SqlClient.SqlConnection("user id=noc;" + "password=noc972;" + "server=ilnoc01;" + "Trusted_Connection=false;" + "database= SHIFT_summary_new; " + "connection timeout=30");
            using (sqlConnection1)
            {
                SqlCommand command = new SqlCommand("SELECT [IsPinned] FROM [SHIFT_summary_new].[dbo].[Entery] where Entery_id='" + id + "'", sqlConnection1);
                sqlConnection1.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            return reader.GetBoolean(0);
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                    return false;
                }
                reader.Close();
            }
            return false;
        }

        protected void PinButton_Click(object sender, EventArgs e)
        {
            if (Timer1.Enabled == false)
            {
                Timer1.Enabled = true;
                selectedTimer.Enabled = false;
            }
            if(hiddenSelected.Value=="")
            {
                return;
            }
            string id;
            try
            {
                id = hiddenSelected.Value.Split('-')[1];
            }
            catch
            {
                id = hiddenSelected.Value;
            }
            if (sql_query_check_if_pinned(id) == true)
            {
                SQL_conectionsAndCommand("update Entery set IsPinned='false' where Entery_id='" + id + "'");
                SQL_conectionsAndCommand("delete from Entery where pin_id='" + id + "'");
                SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");

            }
            else
            {
                SQL_conectionsAndCommand("update Entery set IsPinned='true' where Entery_id='" + id + "'");
                int shift_id = int.Parse(GetShiftID());
                shift_id++;
                SQL_conectionsAndCommand("INSERT INTO Entery ([Shift_Id],[Event],[IsCritical],[IsResolved],[User_name],[CreatedTime],[CreateDate],[ResolvedTime],[pin_id]) SELECT '" + shift_id + "',[Event],[IsCritical],[IsResolved],[User_name],[CreatedTime],[CreateDate],[ResolvedTime],'" + id + "' FROM Entery WHERE Entery_id='" + id + "'");
                int Entery_id = int.Parse(id);
                Entery_id++;
                SQL_conectionsAndCommand("INSERT INTO Sub_Entery ([Entery_Id],[Title],[IsCalled],[Department],[Expert],[EventTime],[User_name]) SELECT '" + Entery_id + "',[Title],[IsCalled],[Department],[Expert],[EventTime],[User_name]  FROM Sub_Entery WHERE Entery_id='" + id + "'");
                SQL_conectionsAndCommand("Insert Into UpdateTime (UserName, Time) Values('" + userName + "', '" + DateTime.Now.ToString("HH:mm:ss tt") + "')");

            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan lastUpdate = sql_query_returnLastUpdate();

            System.Diagnostics.Debug.WriteLine("HiddenLastEventId.Value when entering: " + HiddenLastEventId.Value);
            ScriptManager.RegisterStartupScript(this, GetType(), "Get_Ids_values", "Get_Ids_values();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "Get_SubIds_values", "Get_SubIds_values();", true);
            string shift = sql_query("select top 1 Shift_title from Shift ORDER BY ID DESC");
            if (!shift.Contains(getShift_title()))
            {
                Button2_Click(sender, e);
            }
            try
            {
               TimeSpan userTime = TimeSpan.Parse(HiddenLastEventId.Value);
                if(userTime == lastUpdate)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Set_offLine_table", "Set_offLine_table();", true);
                    return;
                }
            }
            catch { }
            sql_query_table("");
            HiddenLastEventId.Value = lastUpdate.ToString();
            eventIdLast = sql_query_returnLastUpdate().ToString();
            System.Diagnostics.Debug.WriteLine("HiddenLastEventId.Value when setting: " + HiddenLastEventId.Value);

            HiddenLastSubId.Value = GetLastSubEventId().ToString();
            ScriptManager.RegisterStartupScript(this, GetType(), "Get_Shift_data", "Get_Shift_data();", true);


        }

        protected void callExpertBox_CheckedChanged(object sender, EventArgs e)
        {
            if (callExpertBox.Checked == true)
            {
                callTo.Visible = true;
            }
            else
            {
                callTo.Visible = false;
            }
        }

        protected void selectedTimer_Tick(object sender, EventArgs e)
        {
            selectedTimer.Enabled = false;
            Timer1.Enabled = true;
            sql_query_table("");
        }


    }
}