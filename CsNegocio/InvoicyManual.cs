using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace CsNegocio
{
    public class InvoicyManual
    {
        public string  UrlWs,Soap,SoapRet,ErrorDesc,xmlret,status,statusdesc,errcode,errdesc;
        public XmlDocument xmlresultado;
        public int ErrorCode;
        public bool tiene_error = true;

        public void ExecutaWSEnvioCFE()
        {
            string resultado = "";
            int desde, hasta;

            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(UrlWs);
                req.Method = "POST";
                
                Stream stm = req.GetRequestStream();
                StreamWriter stmw = new StreamWriter(stm);

                stmw.Write(Soap);
                stmw.Close();
                stmw.Dispose();

                stm.Close();
                stm.Dispose();

                try
                {
                    WebResponse response = req.GetResponse();

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                        SoapRet = reader.ReadToEnd().Trim();
                        SoapRet = SoapRet.Replace("\\n", "");
                        SoapRet = SoapRet.Replace("\\t", "");

                        ErrorCode = 100;
                        reader.Close();
                        reader.Dispose();

                        resultado = SoapRet;
                        string a = "&lt;?xml version=";
                        string b = "</Xmlretorno>";
                        desde = resultado.IndexOf(a);
                        hasta = resultado.IndexOf(b);
                        resultado = resultado.Substring(desde, hasta - desde);
                        resultado = resultado.Replace("&lt;", "<");
                        resultado = resultado.Replace("&gt;", ">");
                        resultado = resultado.Replace("&quot", "\\");

                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(resultado);

                        status = doc.DocumentElement.SelectSingleNode("/EnvioCFERetorno/ListaCFE/CFE/CFEStatus").InnerText;
                        errcode = doc.DocumentElement.SelectSingleNode("/EnvioCFERetorno/ListaCFE/CFE/Erros/ErrosItem/CFEErrCod").InnerText;
                        errdesc = doc.DocumentElement.SelectSingleNode("/EnvioCFERetorno/ListaCFE/CFE/Erros/ErrosItem/CFEErrDesc").InnerText;
                        
                        switch (status)
                        {
                            case "1":
                            statusdesc="Importado";
                            break;
                            case "2":
                            statusdesc="Firmado";
                            break;
                            case "3":
                            statusdesc="Rechazado";
                            break;
                            case "4":
                            statusdesc="Enviado DGI";
                            break;
                            case "5":
                            statusdesc="Autorizado";
                            break;
                            case "6":
                            statusdesc="Anulado";
                            break;
                            case "7":
                            statusdesc="Aceptado";
                            break;
                            case "9":
                            statusdesc="En Digitacion";
                            break;
                            default:
                            statusdesc="";
                            break;
                        }

                        if (status == "3")
                        {
                            tiene_error = true;
                        }
                        else
                        {
                            tiene_error = false;
                        }
                        


                    }
                    
                    
                }
                catch (WebException exx)
                {

                    errdesc = exx.Message.ToString(); 
                    WebResponse errorResponse = exx.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        SoapRet = reader.ReadToEnd();
                        ErrorCode = 801;
                        
                        reader.Close();
                        reader.Dispose();
                        // log errorText
                    }
                }


            }
            catch (Exception ex)
            {
                errdesc = ex.Message.ToString();
            }
        }

        public void ExecutaWS()
        {
            string resultado = "";
            int desde, hasta;

            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(UrlWs);
                req.Method = "POST";

                Stream stm = req.GetRequestStream();
                StreamWriter stmw = new StreamWriter(stm);

                stmw.Write(Soap);
                stmw.Close();
                stmw.Dispose();

                stm.Close();
                stm.Dispose();

                try
                {
                    WebResponse response = req.GetResponse();

                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

                        SoapRet = reader.ReadToEnd().Trim();
                        SoapRet = SoapRet.Replace("\\n", "");
                        SoapRet = SoapRet.Replace("\\t", "");

                        ErrorCode = 100;
                        reader.Close();
                        reader.Dispose();

                        resultado = SoapRet;
                        string a = "&lt;?xml version=";
                        string b = "</Xmlretorno>";
                        desde = resultado.IndexOf(a);
                        hasta = resultado.IndexOf(b);
                        resultado = resultado.Substring(desde, hasta - desde);
                        resultado = resultado.Replace("&lt;", "<");
                        resultado = resultado.Replace("&gt;", ">");
                        resultado = resultado.Replace("&quot", "\\");

                        xmlresultado = new XmlDocument();
                        xmlresultado.LoadXml(resultado);
                        tiene_error = false;

                    }


                }
                catch (WebException exx)
                {
                    errdesc = exx.Message.ToString();
                    WebResponse errorResponse = exx.Response;
                    using (Stream responseStream = errorResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                        SoapRet = reader.ReadToEnd();
                        ErrorCode = 801;
                        reader.Close();
                        reader.Dispose();
                        // log errorText
                    }
                }


            }
            catch (Exception ex)
            {
                errdesc = ex.Message.ToString();
            }
        }


        //public void EjecutarWS()
        //{
        //    // Create a request using a URL that can receive a post.
        //    WebRequest request = WebRequest.Create(UrlWs);
        //    // Set the Method property of the request to POST.
        //    request.Method = "POST";

        //    // Create POST data and convert it to a byte array.
        //    byte[] byteArray = Encoding.UTF8.GetBytes(Soap);

        //    // Set the ContentType property of the WebRequest.
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    // Set the ContentLength property of the WebRequest.
        //    request.ContentLength = byteArray.Length;

        //    // Get the request stream.
        //    Stream dataStream = request.GetRequestStream();
        //    // Write the data to the request stream.
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    // Close the Stream object.
        //    dataStream.Close();

        //    // Get the response.
        //    WebResponse response = request.GetResponse();
        //    // Display the status.
        //    Console.WriteLine(((HttpWebResponse)response).StatusDescription);

        //    // Get the stream containing content returned by the server.
        //    // The using block ensures the stream is automatically closed.
        //    using (dataStream = response.GetResponseStream())
        //    {
        //        // Open the stream using a StreamReader for easy access.
        //        StreamReader reader = new StreamReader(dataStream);
        //        // Read the content.
        //        string responseFromServer = reader.ReadToEnd();
        //        // Display the content.
        //        Console.WriteLine(responseFromServer);
        //    }

        //    // Close the response.
        //    response.Close();
        //}


        public string WriteSOAP(string xml,string accion,string accion2)
        {
            string sBody = "";

            xml.Replace("(?ism)(?<=>)[^a-z|0-9]*(?=<)", "");

            string xmlEnvio = xml;
            xmlEnvio = xmlEnvio.Replace("<", "&lt;");
            xmlEnvio = xmlEnvio.Replace(">", "&gt;");
            xmlEnvio = xmlEnvio.Replace("\\", "&quot");

            sBody += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:gx=""Gx"">";
            sBody += "<soapenv:Header/>";
            sBody += "<soapenv:Body>";
            sBody += "<gx:"+accion+">";
            sBody += "<gx:" + accion2 + ">";
            sBody += xmlEnvio;
            sBody += "</gx:" + accion2 + ">";
            sBody += "</gx:"+accion+">";
            sBody += "</soapenv:Body>";
            sBody += "</soapenv:Envelope>";
            return sBody;
        }

        public string get_url(string tipo, bool testing)
        {
            string url = "";

            if (testing == true)
            {
                switch (tipo)
                {
                    case "EmissionCFE":
                        url = "https://appuypruebas.migrate.info/InvoiCy/aws_emissionfactura.aspx?wsdl";
                        break;
                    case "ConsultaCFE":
                        url = "https://appuypruebas.migrate.info/InvoiCy/aws_consultafactura.aspx?wsdl";
                        break;
                    case "AnulaCFE":
                        url = "https://appuypruebas.migrate.info/InvoiCy/aws_anulacion.aspx?wsdl";
                        break;


                }
            }
            else
            {
                switch (tipo)
                {
                    case "EmissionCFE":
                        url = "https://appuy.migrate.info/InvoiCy/aws_emissionfactura.aspx?wsdl";
                        break;
                    case "ConsultaCFE":
                        url = "https://appuy.migrate.info/InvoiCy/aws_consultafactura.aspx?wsdl";
                        break;
                    case "AnulaCFE":
                        url = "https://appuy.migrate.info/InvoiCy/aws_anulacion.aspx?wsdl";
                        break;

                }

            }

            return url;
        }

    }
}
