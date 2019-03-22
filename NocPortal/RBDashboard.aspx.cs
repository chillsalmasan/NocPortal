using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Npgsql;
using System.Data;
using System.Drawing;
using System.Net.Mail;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;

namespace NocPortal
{
    public partial class RBDashboard : System.Web.UI.Page
    {
        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();

        //static DateTime lastAlertDate = DateTime.Today.AddDays(-1);
        static DateTime lastEmailAlert = DateTime.Today.AddDays(-1);

        static Boolean hasUnsentErrors = false;

        static int reset = 1;

        //static string day0 = "";
        //static string day1 = "";
        static int failedAPI_0 = 0;
        static int failedAPI_1 = 0;
        static int failedOrch_0 = 0;
        static int failedOrch_1 = 0;
        static int failedGeneration_0 = 0;
        static int failedGeneration_1 = 0;
        static int failedFinal_0 = 0;
        static int failedFinal_1 = 0;
        static int failedExport_0 = 0;
        static int failedExport_1 = 0;

        static int queueError = 0; //if there's already an error in the queue, there's no need to send another email about a new error. one mail for both is enough.

        //static string test = "1";

        protected void Page_Load(object sender, EventArgs e)
        {
            getData();

            styleTable();

            /*
            if (test == "1")
            {
                test = "2";
                GridView1.Rows[1].Cells[9].Text = "1";
            }
            else
            {
                GridView1.Rows[1].Cells[9].Text = "2";
            } */
            
            performChecks();  //changed to only color the cells now (doesn't send an email)

            newChecks(); //these are the new checks that I wrote, they are responsible for sending an email once a failure is detected

            unsentErrors();
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
        }

        private void getData()
        {
            try
            {
                // PostgeSQL-style connection string
                string connstring = String.Format("Server={0};Port={1};" +
                    "User Id={2};Password={3};Database={4};",
                    "prod-rptbld-rdspgsql01.cfuzavfta9ct.us-east-1.rds.amazonaws.com", "5432", "readonlyuser",
                    "rou123", "ReportServiceDB");
                // Making connection with Npgsql provider
                NpgsqlConnection conn = new NpgsqlConnection(connstring);
                conn.Open();
                // quite complex sql statement

                //string sql = "SELECT creation_timestamp::timestamp::date as \"day\", execution_type, count(CASE WHEN execution_type = 'Ad_Hoc' THEN 1 END) as ad_hoc, count(CASE WHEN execution_type = 'Scheduled' THEN 1 END) as scheduled, count(CASE WHEN execution_status = 'In Progress - API' THEN 1 END) as in_progress_api, count(CASE WHEN execution_status = 'In Progress - Orchestration' THEN 1 END) as in_progress_orchestration, count(CASE WHEN execution_status = 'Waiting - Generation' THEN 1 END) as waiting_generation, count(CASE WHEN execution_status = 'In Progress - Generation' THEN 1 END) as in_progress_generation, count(CASE WHEN execution_status = 'In Progress - Finalization' THEN 1 END) as in_progress_finalization, count(CASE WHEN execution_status = 'In Progress - Export' THEN 1 END) as in_progress_export, count(CASE WHEN execution_status = 'Failed - API' THEN 1 END) as failed_api, count(CASE WHEN execution_status = 'Failed - Orchestration' THEN 1 END) as failed_orchestration, count(CASE WHEN execution_status = 'Failed - Generation' THEN 1 END) as failed_generation, count(CASE WHEN execution_status = 'Failed - Finalization' THEN 1 END) as failed_finalization, count(CASE WHEN execution_status = 'Failed - Export' THEN 1 END) as failed_export, count(CASE WHEN execution_status = 'Completed' THEN 1 END) as completed FROM reportservice.v_rb_executions where creation_timestamp > CAST (now() AS DATE) - 8 group by creation_timestamp::timestamp::date, execution_type order by creation_timestamp::timestamp::date DESC, execution_type;";
                string sql = "SELECT creation_timestamp::timestamp::date as day, CASE WHEN (template_type = 'AnalyticsReport' OR template_type ='DeliveryAnalysisReport' OR template_type ='OverUnderDeliveryReport') THEN 'Agg' WHEN (template_type = 'RdfReport') THEN 'RDF' WHEN (template_type = 'ReportP2C') THEN 'P2C' ELSE template_type END as report_type, count(CASE WHEN execution_type = 'Ad_Hoc' THEN 1 END) as ad_hoc, count(CASE WHEN execution_type = 'Scheduled' OR execution_type ISNULL THEN 1 END) as scheduled, count(CASE WHEN execution_status = 'In Progress - API' THEN 1 END) as in_progress_api, count(CASE WHEN execution_status = 'In Progress - Orchestration' THEN 1 END) as in_progress_orchestration, count(CASE WHEN execution_status = 'In Progress - Generation' THEN 1 END) as in_progress_generation, count(CASE WHEN execution_status = 'In Progress - Finalization' THEN 1 END) as in_progress_finalization, count(CASE WHEN execution_status = 'In Progress - Export' THEN 1 END) as in_progress_export, count(CASE WHEN execution_status = 'Failed - API' THEN 1 END) as failed_api, count(CASE WHEN execution_status = 'Failed - Orchestration' THEN 1 END) as failed_orchestration, count(CASE WHEN execution_status = 'Failed - Generation' THEN 1 END) as failed_generation, count(CASE WHEN execution_status = 'Failed - Finalization' THEN 1 END) as failed_finalization, count(CASE WHEN execution_status = 'Failed - Export' THEN 1 END) as failed_export, count(CASE WHEN execution_status = 'Completed' THEN 1 END) as completed FROM reportservice.v_rb_executions_with_type where creation_timestamp > CAST (now() AS DATE) - 8 AND parent_id ISNULL group by creation_timestamp::timestamp::date, execution_type,report_type order by creation_timestamp::timestamp::date DESC, report_type;";

                // data adapter making request from our connection
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.SelectCommand.CommandTimeout = 90;
                // i always reset DataSet before i do
                // something with it.... i don't know why :-)
                ds.Reset();
                // filling DataSet with result from NpgsqlDataAdapter
                da.Fill(ds);
                // since it C# DataSet can handle multiple tables, we will select first
                dt = ds.Tables[0];
                // connect grid to DataTable
                GridView1.DataSource = dt;
                GridView1.DataBind();
                // since we only showing the result we don't need connection anymore
                conn.Close();
            }
            catch (Exception msg)
            {
                //MessageBox.Show(msg.ToString());
                System.Diagnostics.Debug.WriteLine(msg.ToString());
                throw;
            }
        }


        private void styleTable()
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Attributes.Add("class", "info");
            }

            GridView1.Style.Add("Height", "400px");
            
            
            GridView1.HeaderRow.Cells[0].Text = "Day";
            GridView1.HeaderRow.Cells[1].Text = "Report Type";
            GridView1.HeaderRow.Cells[2].Text = "Ad Hoc";
            GridView1.HeaderRow.Cells[3].Text = "Scheduled";
            GridView1.HeaderRow.Cells[4].Text = "In Progress API";
            GridView1.HeaderRow.Cells[5].Text = "In Progress Orchestration";
            //GridView1.HeaderRow.Cells[6].Text = "Waiting Generation";
            GridView1.HeaderRow.Cells[6].Text = "In Progress Generation";
            GridView1.HeaderRow.Cells[7].Text = "In Progress Finalization";
            GridView1.HeaderRow.Cells[8].Text = "In Progress Export";
            GridView1.HeaderRow.Cells[9].Text = "Failed API";
            GridView1.HeaderRow.Cells[10].Text = "Failed Orchestration";
            GridView1.HeaderRow.Cells[11].Text = "Failed Generation";
            GridView1.HeaderRow.Cells[12].Text = "Failed Finalization";
            GridView1.HeaderRow.Cells[13].Text = "Failed Export";
            GridView1.HeaderRow.Cells[14].Text = "Completed";
        }


        private void performChecks()
        {

            //Boolean error = false;

            /*
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (Convert.ToInt32(GridView1.Rows[i].Cells[6].Text) > 50) //waiting > 50
                {
                    //Color red = Color.FromName("Red");
                    //GridView1.Rows[i].Cells[4].BackColor = red;
                    
                    //if (i == GridView1.Rows.Count - 1 || i == GridView1.Rows.Count - 2) //if the error occurred today
                    if (i == 0 || i == 1)
                    {
                        if(DateTime.Today.DayOfWeek == DayOfWeek.Monday && DateTime.Now < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0)) //ignore the Q before 19:00 on monday
                        {
                            error = false; //ignore the Q before 19:00 on monday
                        }
                        else
                        {
                            GridView1.Rows[i].Cells[4].Attributes.Add("class", "danger");
                            GridView1.Rows[i].Cells[4].Style.Add("color", "red");
                            error = true;
                        }
                        
                    }
                }
            } */

            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                if (Convert.ToInt32(GridView1.Rows[i].Cells[9].Text) > 0) //Failed API > 0 
                {
                    GridView1.Rows[i].Cells[9].Attributes.Add("class", "danger");
                    GridView1.Rows[i].Cells[9].Style.Add("color", "red");
                    GridView1.Rows[i].Cells[9].Style.Add("font-weight", "bold");
                    /*
                    if (i == 0 || i == 1) //if the error occured today
                    {
                        error = true;
                    }*/
                }

                if (Convert.ToInt32(GridView1.Rows[i].Cells[10].Text) > 0) //Failed Orchestration > 0 
                {
                    GridView1.Rows[i].Cells[10].Attributes.Add("class", "danger");
                    GridView1.Rows[i].Cells[10].Style.Add("color", "red");
                    GridView1.Rows[i].Cells[10].Style.Add("font-weight", "bold");
                    /*
                    if (i == 0 || i == 1)
                    {
                        error = true;
                    }*/
                }

                if (Convert.ToInt32(GridView1.Rows[i].Cells[11].Text) > 0) //Failed Generation > 0 
                {
                    GridView1.Rows[i].Cells[11].Attributes.Add("class", "danger");
                    GridView1.Rows[i].Cells[11].Style.Add("color", "red");
                    GridView1.Rows[i].Cells[11].Style.Add("font-weight", "bold");
                    /*
                    if (i == 0 || i == 1)
                    {
                        error = true;
                    }*/
                }

                if (Convert.ToInt32(GridView1.Rows[i].Cells[12].Text) > 0) //Failed Finalization > 0 
                {
                    GridView1.Rows[i].Cells[12].Attributes.Add("class", "danger");
                    GridView1.Rows[i].Cells[12].Style.Add("color", "red");
                    GridView1.Rows[i].Cells[12].Style.Add("font-weight", "bold");
                    /*
                    if (i == 0 || i == 1)
                    {
                        error = true;
                    }*/
                }

                if (Convert.ToInt32(GridView1.Rows[i].Cells[13].Text) > 0) //Failed Export > 0 
                {
                    GridView1.Rows[i].Cells[13].Attributes.Add("class", "danger");
                    GridView1.Rows[i].Cells[13].Style.Add("color", "red");
                    GridView1.Rows[i].Cells[13].Style.Add("font-weight", "bold");
                    /*
                    if (i == 0 || i == 1)
                    {
                        error = true;
                    }*/
                }
            }

            /*
            if (error)
            {
                System.Diagnostics.Debug.WriteLine("error detected!");
                //if(DateTime.Today != lastAlertDate && DateTime.Now > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,8,0,0) && DateTime.Now < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 0, 0))
                if (DateTime.Today != lastAlertDate && GridView1.Rows[0].Cells[0].Text == DateTime.Today.ToString() && GridView1.Rows[1].Cells[0].Text == DateTime.Today.ToString())
                {
                    sendAlertEmail();
                    lastAlertDate = DateTime.Today;
                }
            }*/
        }



        private void newChecks()
        {
            Boolean error = false;
   
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                /*
                if(i==0 || i==1) //check only today's reports
                {
                    error = checkInProgressReports(i);
                }*/

                if (Convert.ToInt32(GridView1.Rows[i].Cells[6].Text) > 1000) //waiting > 1000
                {
                    GridView1.Rows[i].Cells[6].Attributes.Add("class", "danger");
                    GridView1.Rows[i].Cells[6].Style.Add("color", "red");
                    GridView1.Rows[i].Cells[6].Style.Add("font-weight", "bold");

                    //if the the threshold was surpassed today
                    if (i == 0 || i == 1)
                    {
                        /*
                        if (DateTime.Today.DayOfWeek == DayOfWeek.Monday && DateTime.Now < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 0, 0)) //ignore the Q before 19:00 on monday
                        {
                            error = false; //ignore the Q before 19:00 on monday
                        } */

                        if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                        {
                            if (Convert.ToInt32(GridView1.Rows[i].Cells[6].Text) > 1500)
                            {
                                if (queueError == 0)
                                {
                                    queueError = 1;
                                    error = true;
                                }

                            }
                            else
                            {
                                error = false;
                            }
                        }
                        else
                        {
                            if (queueError == 0)
                            {
                                queueError = 1;
                                error = true;
                            }
                        }
                    }
                }                
            }


            if (GridView1.Rows[0].Cells[0].Text == DateTime.Today.ToString() && GridView1.Rows[1].Cells[0].Text == DateTime.Today.ToString())
            {
                if (reset == 1)
                {
                    reset = 0;
                    failedAPI_0 = 0;
                    failedAPI_1 = 0;
                    failedOrch_0 = 0;
                    failedOrch_1 = 0;
                    failedGeneration_0 = 0;
                    failedGeneration_1 = 0;
                    failedFinal_0 = 0;
                    failedFinal_1 = 0;
                    failedExport_0 = 0;
                    failedExport_1 = 0;

                    queueError = 0;
                }
                
            }
            else
            {
                reset = 1;
            }

            if (Convert.ToInt32(GridView1.Rows[0].Cells[9].Text) > failedAPI_0 || Convert.ToInt32(GridView1.Rows[1].Cells[9].Text) > failedAPI_1) //detected failed api
            {
                error = true;
            }
            else if (Convert.ToInt32(GridView1.Rows[0].Cells[10].Text) > failedOrch_0 || Convert.ToInt32(GridView1.Rows[1].Cells[10].Text) > failedOrch_1) //detected failed orchestration
            {
                error = true;
            }
            else if (Convert.ToInt32(GridView1.Rows[0].Cells[11].Text) > failedGeneration_0 || Convert.ToInt32(GridView1.Rows[1].Cells[11].Text) > failedGeneration_1) //detected failed generation
            {
                error = true;
            }
            else if (Convert.ToInt32(GridView1.Rows[0].Cells[12].Text) > failedFinal_0 || Convert.ToInt32(GridView1.Rows[1].Cells[12].Text) > failedFinal_1) //detected failed finalization
            {  
                error = true;
            }
            else if (Convert.ToInt32(GridView1.Rows[0].Cells[13].Text) > failedExport_0 || Convert.ToInt32(GridView1.Rows[1].Cells[13].Text) > failedExport_1) //detected failed export
            {
                error = true;
            }

            failedAPI_0 = Convert.ToInt32(GridView1.Rows[0].Cells[9].Text);
            failedAPI_1 = Convert.ToInt32(GridView1.Rows[1].Cells[9].Text);
            failedOrch_0 = Convert.ToInt32(GridView1.Rows[0].Cells[10].Text);
            failedOrch_1 = Convert.ToInt32(GridView1.Rows[1].Cells[10].Text);
            failedGeneration_0 = Convert.ToInt32(GridView1.Rows[0].Cells[11].Text);
            failedGeneration_1 = Convert.ToInt32(GridView1.Rows[1].Cells[11].Text);
            failedFinal_0 = Convert.ToInt32(GridView1.Rows[0].Cells[12].Text);
            failedFinal_1 = Convert.ToInt32(GridView1.Rows[1].Cells[12].Text);
            failedExport_0 = Convert.ToInt32(GridView1.Rows[0].Cells[13].Text);
            failedExport_1 = Convert.ToInt32(GridView1.Rows[1].Cells[13].Text);


            if (error == true)
            {

                //make sure to not send an email while the days are changing. 
                //Removing this check will force alerts that already were sent to be sent again, because when the days are changing,
                //which usually happens in the middle of the night, the top row will become the second row, and therefore any
                //any error it holds will be sent again
                if (GridView1.Rows[0].Cells[0].Text == GridView1.Rows[1].Cells[0].Text) 
                {
                    TimeSpan span = DateTime.Now.Subtract(lastEmailAlert);
                    if (span.TotalMinutes > 29)
                    {
                        hasUnsentErrors = false;
                        sendAlertEmail();
                    }
                    else
                    {
                        hasUnsentErrors = true;
                    }
                }
                else
                {
                    return; 
                }
            }
        }

        private Boolean checkInProgressReports(int row)
        {
            int inProgressAlertThreshold = 100;
            Boolean isError = false;
            for (int column = 4; column < 10; column++)
            {
                if(column == 6) //ignore waiting generation
                {
                    continue;
                }
                if (Convert.ToInt32(GridView1.Rows[row].Cells[column].Text) > inProgressAlertThreshold)
                {
                    colorCellInRed(row, column);
                    if (queueError == 0)
                    {
                        queueError = 1;
                        isError = true;
                    }
                    else
                    {
                        isError = false;
                    }
                }
            }
            return isError;
        }


        private void unsentErrors()
        {
            TimeSpan span = DateTime.Now.Subtract(lastEmailAlert);
            if (hasUnsentErrors && span.TotalMinutes > 29)
            {
                hasUnsentErrors = false;
                sendAlertEmail();
            }
        }



        private void sendAlertEmail()
        {
            string smtpAddress = "10.10.2.15";
            int portNumber = 25;
            bool enableSSL = false;
            string emailFrom = "RBDashboard@sizmek.com";
            string emailTo = "noc.support@sizmek.com,RB-Team@sizmek.com,itay.schiff@sizmek.com";
            //string emailTo = "muhamad.kial@sizmek.com";

            string subject = "Report Builder Alert";

            using (MailMessage mail = new MailMessage())
            {

                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.CC.Add("muhamad.kial@sizmek.com");
                mail.Subject = subject;
                mail.IsBodyHtml = true;


                mail.Body = "There are errors in the Report Builder Dashboard:<br><br><br>";

                try {
                    GridView1.CellSpacing = 15;
                    foreach (GridViewRow row in GridView1.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            cell.Attributes.CssStyle["text-align"] = "center";
                        }
                    }
                    StringBuilder strBuilder = new StringBuilder();
                    StringWriter strWriter = new StringWriter(strBuilder);
                    HtmlTextWriter htw = new HtmlTextWriter(strWriter);
                    GridView1.RenderControl(htw);
                    mail.Body = mail.Body + strBuilder.ToString();
                }
                catch(Exception e)
                {
                    throw e;
                }

                mail.Body = mail.Body + "<br><br><br>";

                mail.Body = mail.Body + "Please contact the expert based on this KB: https://sizmek.atlassian.net/wiki/display/RRB/General+RB+KB+-+Report+Builder+Dashboard";
                mail.Body = mail.Body + "<br><br>http://nocportal/RBDashboard.aspx";



                using (SmtpClient smt1p = new SmtpClient(smtpAddress, portNumber))
                {
                    //    smt1p.Credentials = new NetworkCredential(emailFrom, password);
                    smt1p.EnableSsl = enableSSL;

                    try
                    {
                        smt1p.Send(mail);
                        lastEmailAlert = DateTime.Now;
                    }
                    catch (Exception e4)
                    {
                        Response.Write(e4.ToString());
                        //errorbox.Items.Add("error1");
                    }
                }
            }
        }


        private void colorCellInRed(int row, int column)
        {
            GridView1.Rows[row].Cells[column].Attributes.Add("class", "danger");
            GridView1.Rows[row].Cells[column].Style.Add("color", "red");
            GridView1.Rows[row].Cells[column].Style.Add("font-weight", "bold");
        }

        


        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        /*
        public void CreateHtmlTable(DataTable dt)
        {
            //Do your HTML work here, like the following:
            string tab = "\t";

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<html>");
            sb.AppendLine(tab + "<body>");
            sb.AppendLine(tab + tab + "<table>");

            // headers.
            sb.Append(tab + tab + tab + "<tr>");

            foreach (DataColumn dc in dt.Columns)
            {
                sb.AppendFormat("<td>{0}</td>", dc.ColumnName);
            }

            sb.AppendLine("</tr>");

            // data rows
            foreach (DataRow dr in dt.Rows)
            {
                sb.Append(tab + tab + tab + "<tr>");

                foreach (DataColumn dc in dt.Columns)
                {
                    string cellValue = dr[dc] != null ? dr[dc].ToString() : "";
                    sb.AppendFormat("<td>{0}</td>", cellValue);
                }

                sb.AppendLine("</tr>");
            }

            sb.AppendLine(tab + tab + "</table>");
            sb.AppendLine(tab + "</body>");
            sb.AppendLine("</html>");


        }*/
    }
}