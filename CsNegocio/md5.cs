using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsNegocio
{
    public class md5
    {
        public static string getmd5(string input)
        {
            //input = "clave_de_acceso" + "<CFE>...</CFE>";
            using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                System.Text.StringBuilder sBuilder = new System.Text.StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
    }
}
