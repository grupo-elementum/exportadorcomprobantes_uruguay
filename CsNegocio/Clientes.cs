using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsDatos;

namespace CsNegocio
{
    public class Clientes
    {
        #region Declaraciones

        private double _Nrocta;
        private string _RazonSocial;
        private string _Direccion;
        private string _Localidad;
        private string _Telefono;       
        private string _Tipo;
        private string _Iva;
        private string _CondicionPago;
        private int    _Cuota;
        private int    _Cumplimiento;
        private string _NrCuit;
        private int    _RetiraEnvase;
        private double _PorcIIBB;

        #endregion

        #region Constructores

        public Clientes(double Nrocta, string RazonSocial, string Direccion, string Localidad, string Telefono, string Tipo, string Iva, string CondicionPago, int Cuota, int Cumplimiento, string NrCuit, int RetiraEnvase, double ProcIIBB)
        {
            _Nrocta = Nrocta;
            _RazonSocial = RazonSocial;
            _Direccion = Direccion;
            _Localidad = Localidad;
            _Telefono = Telefono;
            _Tipo = Tipo;
            _Iva = Iva;
            _CondicionPago = CondicionPago;
            _Cuota = Cuota;
            _Cumplimiento = Cumplimiento;
            _NrCuit = NrCuit;
            _RetiraEnvase = RetiraEnvase;
            _PorcIIBB = PorcIIBB;
        }
        public Clientes()
        {
        }

        #endregion

        #region Propiedades

        public double Nrocta
        {
            get { return _Nrocta; }
            set { this._Nrocta = value; }
        }


        public string RazonSocial
        {
            get { return _RazonSocial; }
            set { this._RazonSocial = value; }
        }


        public string Direccion
        {
            get { return _Direccion; }
            set { this._Direccion = value; }
        }


        public string Localidad
        {
            get { return _Localidad; }
            set { this._Localidad = value; }
        }


        public string Telefono
        {
            get { return _Telefono; }
            set { this._Telefono = value; }
        }


        public string Tipo
        {
            get { return _Tipo; }
            set { this._Tipo = value; }
        }


        public string Iva
        {
            get { return _Iva; }
            set { this._Iva = value; }
        }

        
        public string CondicionPago
        {
            get { return _CondicionPago; }
            set { this._CondicionPago = value; }
        }

        
        public int Cuota
        {
            get { return _Cuota; }
            set { this._Cuota = value; }
        }


        public int Cumplimiento
        {
            get { return _Cumplimiento; }
            set { this._Cumplimiento = value; }
        }


        public string NrCuit
        {
            get { return _NrCuit; }
            set { this._NrCuit = value; }
        }

        
        public int RetiraEnvase
        {
            get { return _RetiraEnvase; }
            set { this._RetiraEnvase = value; }
        }


        public double PorcIIBB
        {
            get { return _PorcIIBB; }
            set { this._PorcIIBB = value; }
        }
        #endregion

        #region Metodos

        public DataSet Listar(double IdCliente)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Fecha", "20100101");
            sPar[1] = new DBHelper.StoredProcedureParameter("@Nrocta", IdCliente);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Sincro_H2O_Clientes", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet Listar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Fecha", DateTime.Now.ToShortDateString());

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Sincro_H2O_Clientes", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public bool ClienteExiste(double IdCliente)
        {
            bool bValida = false;
            IDataReader oDr;
            DBHelper oDb = new DBHelper();
            
            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Fecha", "20100101");
            sPar[1] = new DBHelper.StoredProcedureParameter("@Nrocta", IdCliente);

            try
            {
                oDb.Connect();
                oDr = oDb.ExecuteProcedureAsReader("Sincro_H2O_Clientes", sPar);

                if (oDr.Read())
                {
                    bValida = true;
                    //_IdCliente = double.Parse(oDr["IdCliente"].ToString());
                }

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "ClientesExiste", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return bValida;
        }
        public DataSet ClienteRecuperarEquipos(double IdCliente)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Nrocta", IdCliente);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Mensajes_H2O_EquiposFC", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "ClienteRecuperarEquipos", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ClienteRecuperarProductos(double IdCliente,string Tipo)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[4];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Idcliente", IdCliente);
            sPar[1] = new DBHelper.StoredProcedureParameter("@Activo", "S");
            sPar[2] = new DBHelper.StoredProcedureParameter("@Productos", "");
            sPar[3] = new DBHelper.StoredProcedureParameter("@Tipo", Tipo);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Clientes_Productos_Recuperar", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "ClienteRecuperarProductos", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet ClienteRecuperarPrecios(double IdCliente)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@Idcliente", IdCliente);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Mensajes_H2O_Cliente_Precios", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "ClienteRecuperarPrecios", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public void ModificarFecha(string nrocta)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDb.ExecuteSqlNonQuery("UPDATE CLIENTES SET FECALT = GETDATE()-1, FECMOD = GETDATE()-1 WHERE NROCTA = " + nrocta.ToString());
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "ModificarFecha", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void CalcularTemporada()
        {
            int Temporada = 0;
            string peract; string perdde; string perhta;
            DateTime fDesde; DateTime fHasta;
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT * FROM TEMPORADAS ORDER BY IDTEMPORADA");
                peract = DateTime.Now.Date.ToString().Substring(3, 2) + DateTime.Now.Date.ToString().Substring(0, 2);
                foreach (DataRow row in oDs.Tables[0].Rows)
                {
                    perdde = row["periodo_desde"].ToString().Substring(row["periodo_desde"].ToString().Length - 2, 2) + row["periodo_desde"].ToString().Substring(0, 2);
                    perhta = row["periodo_hasta"].ToString().Substring(row["periodo_hasta"].ToString().Length - 2, 2) + row["periodo_hasta"].ToString().Substring(0, 2);
                    fDesde = DateTime.Parse(row["periodo_desde"].ToString() + "/" + DateTime.Now.Year.ToString().Substring(DateTime.Now.Year.ToString().Length - 2, 2));
                    fHasta = DateTime.Parse(row["periodo_hasta"].ToString() + "/" + DateTime.Now.Year.ToString().Substring(DateTime.Now.Year.ToString().Length - 2, 2));
                  if (Int32.Parse(perhta) < Int32.Parse(perdde))
	                {
                        if (Int32.Parse(peract) <= Int32.Parse(perhta))
	                        {
                        		 fDesde = DateTime.Parse(row["periodo_desde"].ToString() + "/" + (DateTime.Now.Year - 1).ToString().Substring((DateTime.Now.Year - 1).ToString().Length - 2,2));
	                        }
                         else
	                        {
                                fHasta = DateTime.Parse(row["periodo_hasta"].ToString() + "/" + (DateTime.Now.Year - 1).ToString().Substring((DateTime.Now.Year + 1).ToString().Length - 2, 2));
	                        }
	                }
                  if (DateTime.Now.Date >= fDesde && DateTime.Now.Date <= fHasta)
	                {
                		 Temporada = Int32.Parse(row["idtemporada"].ToString());
                         break;
	                }
                }
                DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
                sPar[0] = new DBHelper.StoredProcedureParameter("@TEMPORADA", Temporada);
                sPar[1] = new DBHelper.StoredProcedureParameter("@BULTOSOLITROS", 1);

                oDs = oDb.ExecuteProcedureAsDataSet("Sincro_H2O_ClientesCalculaCuota", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Clientes", "CalcularTemporada", ex);
            }
            finally
            {
                oDb.Disconnect();
            }


        }


        public DataSet GetFacturasaExportar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet ("PDF_LISTAR_FACTURAS");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "PDF_LISTAR_FACTURAS", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;

        }

        public DataSet GetFacturaDetalle(string codmov , string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("PDF_LISTAR_FACTURA_DETALLE", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "PDF_LISTAR_FACTURA_DETALLE", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }


        public DataSet GetFacturaDetalleTOTALES(string codmov, string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("PDF_FACTURA_TOTAL",sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "PDF_FACTURA_TOTAL", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }
        public DataSet GetRecibosExportar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("PDF_LISTAR_RECIBOS");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "PDF_LISTAR_RECIBOS", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;

        }

        public DataSet GetReciboAplicacion(string codmov ,string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
                sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
                sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("PDF_LISTAR_RECIBO_APLICACION",sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "PDF_LISTAR_RECIBO_APLICACION", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;

        }

        public DataSet GetReciboPago(string codmov, string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            try
            {
                DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
                sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
                sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("PDF_LISTAR_RECIBO_PAGO", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "PDF_LISTAR_RECIBO_PAGO", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;

        }

        public DataSet SetFacturaExportada(string codmov, string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("PDF_FACTURA_EXPORTA", sPar);
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "PDF_FACTURA_EXPORTA", "Listar", ex);
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }






        #endregion

    }
}
