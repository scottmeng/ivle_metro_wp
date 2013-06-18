using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace mockup
{
    public class LAPI
    {
        private static String key = "lAY3TAAcAGYcokEEqKNCt";
        private static String domain = "https://ivle.nus.edu.sg/";
        //public static String token {get; set;}
        public static String token;

        public static string validateTokenURL()
        {
            string url = domain + "api/Lapi.svc/Validate?APIKey=" + key + "&Token=" + token;

            return url;
        }

        // get the request url with API item and parameters
        public static string requestURL(string item, string[] parameters, string[] paramsVal)
        {
            string url = domain + "api/Lapi.svc/" + item + "?APIKey=" + key + "&AuthToken=" + token;

            if (parameters.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    url += "&" + parameters[i] + "=" + paramsVal[i];
                }
            }
            // default data format is json
            //return url + "&output=json";

            return url;
        }

        // return the url for token achieve
        public static string GeneratePostString(string userid, string password, string domain)
        {
            string url = "APIKey=" + key + "&UserID=" + userid + "&Password=" + password + "&Domain=" + domain;
            return url;
        }

        public static string GenerateDownloadURL(string id)
        {
            string url = domain + "api/downloadfile.ashx?APIKey=" + key + "&AuthToken=" + token + "&ID=" + id + "&target=workbin";

            return url;
        }
    };
}
