using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Upload.App.Utils
{
    public static class ServiceApiUtil
    {
        public static string ApiResponse(string method, string requestType, Object request = null)
        {
            string ApiBaseUrl = "http://localhost/UploadApi/";
            string response = "";
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(ApiBaseUrl + method);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = requestType;
                httpWebRequest.UseDefaultCredentials = true;
                if (request != null)
                {
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = new JavaScriptSerializer().Serialize(request);

                        streamWriter.Write(json);
                    }
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                using (var stream = e.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    throw new Exception(reader.ReadToEnd());
                }
            }
            return response;
        }
    }
}