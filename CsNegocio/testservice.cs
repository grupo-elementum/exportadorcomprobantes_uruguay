using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace CsNegocio
{
    public class testservice
    {

            public static void ejecuta (string datos, string url)
            {
                //creating object of program class to access methods  
                testservice obj = new testservice();
                //Calling InvokeService method  
                obj.InvokeService(datos,url);
            }
            public void InvokeService(string data,string url)
            {
                //Calling CreateSOAPWebRequest method  
                HttpWebRequest request = CreateSOAPWebRequest(url);

                //<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-   instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">  

                XmlDocument SOAPReqBody = new XmlDocument();
                //SOAP Body Request  
                SOAPReqBody.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>  
             <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:gx=""Gx""><soapenv:Header/>
             <soapenv:Body>  
                " + data + @"
              </soapenv:Body>  
            </soapenv:Envelope>");


                using (Stream stream = request.GetRequestStream())
                {
                    SOAPReqBody.Save(stream);
                }
                //Geting response from request  
                using (WebResponse Serviceres = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(Serviceres.GetResponseStream()))
                    {
                        //reading stream  
                        var ServiceResult = rd.ReadToEnd();
                        //writting stream result on console  
                        Console.WriteLine(ServiceResult);
                        Console.ReadLine();
                    }
                }
            }

            public HttpWebRequest CreateSOAPWebRequest(string url)
            {
                //Making Web Request  
                HttpWebRequest Req = (HttpWebRequest)WebRequest.Create(url);
                //SOAPAction  
                //Req.Headers.Add(@"SOAPAction:http://tempuri.org/Addition");
                //Content_type  
                Req.ContentType = "text/xml;charset=\"utf-8\"";
                Req.Accept = "text/xml";
                //HTTP method  
                Req.Method = "POST";
                //return HttpWebRequest  
                return Req;
            }
        }  
    }

