using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NocPortal
{
    public partial class XaxisArchive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string GetTableFromArchive(string date)
        {
            SqlDataReader myReader = null;
            SqlConnection conn = new SqlConnection("user id=" + "noc" + ";" + "password=" + "noc972" + ";" + "server=" + "ILNOC01" + ";" + "Trusted_Connection=false;" + "database=" + "GMTReportsReadiness" + ";" + "connection timeout=30; MultipleActiveResultSets=True;");
            conn.Open();
            string stmt5 = "SELECT * FROM Archive WHERE ReportDate=@reportDate;";
            SqlCommand comm4 = new SqlCommand(stmt5, conn);
            comm4.Parameters.AddWithValue("@reportDate", date);
            myReader = comm4.ExecuteReader();
            if (myReader.HasRows)
            {
                string result = "";

                while (myReader.Read())
                {
                    result = myReader["tableHTML"].ToString();
                }

                myReader.Close();
                conn.Close();
                return result;
            }
            else
            {
                myReader.Close();
                conn.Close();
                return "";
            }
        }
    }
}