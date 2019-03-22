using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NocPortal
{
    public partial class GroupMArchive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (hiddenStartDate.Value != "") //it means that we clicked the show button
            {
                DateTime startDateTime = DateTime.Parse(hiddenStartDate.Value);
                hiddenStartDate.Value = String.Format("{0:yyyy-MM-dd}", startDateTime);

                DateTime endDateTime = DateTime.Parse(hiddenEndDate.Value);
                hiddenEndDate.Value = String.Format("{0:yyyy-MM-dd}", endDateTime);

                //System.Diagnostics.Debug.WriteLine("hiddenStartDate.Value: " + hiddenStartDate.Value);

                buildTable(); //load the data from the database and build the table

                hiddenStartDate.Value = "";
                hiddenEndDate.Value = "";
            }
        }

        private void buildTable()
        {
            List<List<string>> lst = loadFromArchiveData();
            List<string> datesLst = new List<string>();
            List<string> reportsLst = new List<string>();

            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();
            row.Style.Add("background-color", "#337ab7"); //blue
            cell.InnerText = "Report Name";
            cell.Style.Add("color", "white");
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            for(int i=0; i<lst.Count; i++)
            {
                if (!datesLst.Contains(lst[i][0])) //don't duplicate the same date
                {
                    cell = new HtmlTableCell();
                    cell.InnerText = lst[i][0];
                    cell.Style.Add("color", "white");
                    row.Cells.Add(cell);
                    tableContent.Rows.Add(row);

                    datesLst.Add(lst[i][0]);
                }
            }


            for(int i=0; i<datesLst.Count; i++)
            {
                for(int j=0; j< lst.Count; j++)
                {
                    if (lst[j][0] == datesLst[i])
                    {
                        if (!reportsLst.Contains(lst[j][1])) //don't duplicate the same report
                        {
                            reportsLst.Add(lst[j][1]);
                        }
                    }
                }
            }

            //System.Diagnostics.Debug.WriteLine("count: " + reportsLst.Count);

            for(int i=0; i<reportsLst.Count; i++)
            {
                row = new HtmlTableRow();
                cell = new HtmlTableCell();
                cell.InnerText = reportsLst[i];
                //cell.Style.Add("color", "white");
                row.Cells.Add(cell);
                //row.Attributes.Add("class", "info");
                row.Style.Add("background-color", "#C4E3F3");
                tableContent.Rows.Add(row);

                for (int j = 0; j < datesLst.Count; j++)
                {
                    int indx = -1;
                    for (int k = 0; k < lst.Count; k++)
                    {
                        if(lst[k][0] == datesLst[j] && lst[k][1]== reportsLst[i])
                        {
                            indx = k;
                        }
                    }
                        
                    if (indx != -1)
                    {
                        
                        cell = new HtmlTableCell();
                        cell.InnerText = lst[indx][2];
                        //cell.Style.Add("color", "white");
                        if (lst[indx][3] != "")
                        {
                            cell.Attributes.Add("class", "danger");
                        }
                        else if(lst[indx][4] != "")
                        {
                            cell.Attributes.Add("class", "warning");
                        }
                        else
                        {
                            cell.Attributes.Add("class", "success");
                        }        
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);
                        
                    }
                    else
                    {
                        cell = new HtmlTableCell();
                        cell.InnerText = "N/A";
                        //cell.Style.Add("color", "white");
                        row.Cells.Add(cell);
                        tableContent.Rows.Add(row);
                    }


                }
            }
        }

        private List<List<string>> loadFromArchiveData()
        {
            List<List<string>> lst = new List<List<string>>();
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "noc" + ";" + "password=" + "noc972" + ";" + "server=" + "ILNOC01" + ";" + "Trusted_Connection=false;" + "database=" + "GroupMDB" + ";" + "connection timeout=30;");
            conn.Open();
            string stmt5 = "SELECT * FROM GroupMArchiveData WHERE ReportDate >= @startDate AND ReportDate <= @endDate order by ReportDate desc;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            comm4.Parameters.AddWithValue("@startDate", hiddenStartDate.Value);
            comm4.Parameters.AddWithValue("@endDate", hiddenEndDate.Value);
            myReader = comm4.ExecuteReader();

            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    List<string> innerLst = new List<string>();
                    innerLst.Add(myReader["ReportDate"].ToString());
                    innerLst.Add(myReader["ReportName"].ToString());
                    innerLst.Add(myReader["Availability"].ToString());
                    innerLst.Add(myReader["Delay"].ToString());
                    innerLst.Add(myReader["GroupMDelay"].ToString());

                    lst.Add(innerLst);
                }
            }

            return lst;
        }
    }
}