using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsDatos
{
    public class Conexion
    {

        #region Declaraciones

        private string _bdweb;
        private string _datasource;
        private string _bdentrada;
        private string _usuariosql;
        private string _passsql;
        private string _bdoperativa;

        public string Bdweb
        {
            get { return _bdweb; }
            set { _bdweb = value; }
        }


        public string Datasource
        {
            get { return _datasource; }
            set { _datasource = value; }
        }

        public string Bdentrada
        {
            get { return _bdentrada; }
            set { _bdentrada = value; }
        }

        public string Usuariosql
        {
            get { return _usuariosql; }
            set { _usuariosql = value; }
        }

        public string Passsql
        {
            get { return _passsql; }
            set { _passsql = value; }
        }

        public string Bdoperativa
        {
            get { return _bdoperativa; }
            set { _bdoperativa = value; }
        }

        #endregion

        private static Conexion instance;

        private Conexion() { }

        public static Conexion Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Conexion();
                }
                return instance;
            }
        }

        public string get_connectionstring_entrada()
        {
            string resul = "Password=" + this._passsql + ";Persist Security Info=True;User ID=" + this._usuariosql + ";Initial Catalog=" + this._bdentrada + ";Data Source=" + this._datasource + ";Connect Timeout=30;Application Name=H2O EXPORTADOR FACTRURAS";

            return resul;
        }

        public string get_connectionstring()
        {
            string resul = "Password=" + this._passsql + ";Persist Security Info=True;User ID=" + this._usuariosql + ";Initial Catalog=" + this._bdoperativa + ";Data Source=" + this._datasource + ";Connect Timeout=30;Application Name=H2O EXPORTADOR FACTRURAS";

            return resul;
        }

    }
}
