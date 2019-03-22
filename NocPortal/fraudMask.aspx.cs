using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NocPortal
{
    public partial class fraudMask : System.Web.UI.Page
    {
        static String configFilePath = "fraudMaskConfigs.json";
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static void updateConfigurationsJson(string configJson, String ivtType){
            //System.IO.File.WriteAllText("fraudMaskConfigs.json", configJson);

            if (ivtType == "GIVT")
            {
                configFilePath = "assets/fraudMask/GivtMaskConfigs.json";
            }
            else if (ivtType == "SIVT")
            {
                configFilePath = "assets/fraudMask/SivtMaskConfigs.json";
            }
            else if (ivtType == "Legacy")
            {
                configFilePath = "fraudMaskConfigs.json";
            }
            WriteLog(configJson);
            syncProjectFiles();

        }

        private static void WriteLog(string log)
        {
            try {
                string fileName = HttpContext.Current.Request.MapPath(configFilePath);
                StreamWriter sw = new StreamWriter(fileName);
                sw.WriteLine(log);
                sw.Close();
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e);
            }
        }

        private static void syncProjectFiles(){
            try{
                string fileName = HttpContext.Current.Request.MapPath(configFilePath);
                StreamReader reader = new StreamReader(fileName);
                String text = reader.ReadToEnd();
                reader.Close();
                string projectConfigFile = "C:\\Users\\lee.hellow\\Desktop\\NocPortal\\NocPortal\\" + configFilePath.Replace("/","\\");
                StreamWriter sw = new StreamWriter(projectConfigFile);
                sw.WriteLine(text);
                sw.Close();                
            }
            catch (Exception e){
                HttpContext.Current.Response.Write(e);
            }
        }
    }
}