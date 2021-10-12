using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CsNegocio
{
    public class log
    {
        private const string FILE_NAME = "log.txt";
        public static void escribirArchivo(string fecha,string accion, string proceso, Exception ex) 
        {
            //if (File.Exists(FILE_NAME)) 
            //{
            //    Console.WriteLine("{0} already exists.", FILE_NAME);
            //    return;
            //}
            using (StreamWriter sw = File.CreateText(FILE_NAME))
            {
                sw.WriteLine ("Error {0} - {1} {2} mensaje:{3} source:{4} trace:{5} targetsite:{6}",fecha.ToString(),accion.ToString(),proceso.ToString(),
                                ex.Message,ex.Source,ex.StackTrace,ex.TargetSite);
                sw.Close();
            }
        }
        public static void escribirArchivo(string fecha, string accion, string proceso, Exception ex, string idcliente)
        {
            if (File.Exists(FILE_NAME))
            {
                using (StreamWriter sw = File.AppendText(FILE_NAME))
                {
                    sw.WriteLine("Error {0} - {1} {2} mensaje:{3} source:{4} trace:{5} targetsite:{6} {7}", fecha.ToString(), accion.ToString(), proceso.ToString(),
                                    ex.Message, ex.Source, ex.StackTrace, ex.TargetSite, idcliente);
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    sw.WriteLine("Error {0} - {1} {2} mensaje:{3} source:{4} trace:{5} targetsite:{6} {7}", fecha.ToString(), accion.ToString(), proceso.ToString(),
                                    ex.Message, ex.Source, ex.StackTrace, ex.TargetSite, idcliente);
                    sw.Close();
                }
            }            
        }
    }
}