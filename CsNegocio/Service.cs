using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Net;


namespace CsNegocio
{
    public class Service
    {
        public string Get(string uri)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers.Add(@"xmlns:wsdlns=""Gx""");
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = -1;
            //request.UserAgent = "Mozilla/5.0 (Taco2) Gecko/20100101";
            request.Method = "GET";
            
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }



        public DataSet HTTP_GET(string url, string Token)
        {
            DataSet oDs = new DataSet();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            //request.Headers.Add("Authorization", "Basic " + Token);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();

            request.Timeout = 600000;

            try
            {
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    oDs.ReadXml(responseStream);
                    return oDs;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                    return oDs;
                }
            }
            catch (Exception ex)
            {
                return oDs;
            }
        }

        public DataSet HTTP_POST(string url, string jsonContent, string Token)
        {
            DataSet oDs = new DataSet();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            //request.Headers.Add("Authorization", "Basic " + Token);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/xml";
            request.Timeout = 600000;


            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {

                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    oDs.ReadXml(responseStream);
                    return oDs;
                }
            }
            catch (WebException e)
            {
                return oDs;
            }
            catch (Exception ex)
            {
                return oDs;
            }

        }

        public DataSet HTTP_PUT(string url, string jsonContent, string Token)
        {
            DataSet oDs = new DataSet();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Headers.Add("Authorization", "Basic " + Token);

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/xml";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    oDs.ReadXml(responseStream);
                    return oDs;
                }
            }
            catch (Exception ex)
            {
                return oDs;
            }
        }

        public void HTTP_DELETE(string url, string Token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            DataSet oDs = new DataSet();
            try
            {
                request.Method = "DELETE";
                request.Headers.Add("Authorization", "Basic " + Token);
                //WebResponse response = request.GetResponse();

                //using (Stream responseStream = response.GetResponseStream())
                //{
                //    oDs.ReadXml(responseStream);
                //    return oDs;
                //}
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        public String Get_Token()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://up.h2o-server.dsgsolutions.io/api/oauth/v2/token?client_id=1_4p3vnsx5nqucscksoks0kkws4c4c8w84gwc4oko40s448cccow&client_secret=30rnasqpjig4k48440kgso4o8ccck40cscg0o48wsw8kw04cko&grant_type=password&username=airtechtest&password=airtechtest");
            DataSet oDs = new DataSet();
            string respuesta = "";
            string[] resp1;
            string[] resp2;
            try
            {
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    respuesta = reader.ReadToEnd();
                    respuesta = respuesta.Replace("\\", "");
                    respuesta = respuesta.Replace('"', ' ');
                    resp1 = respuesta.Split(',');
                    if (resp1.Length > 0)
                    {
                        if (resp1[0].Substring(0, 16) == "{ access_token :")
                        {
                            respuesta = resp1[0].Substring(17).Trim();
                        }
                    }

                    return respuesta;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }


    }
}
