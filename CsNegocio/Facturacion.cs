using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CsDatos;

namespace CsNegocio
{
    public class Facturacion
    {
        double tasaiibb = 0, tasaiibb2 = 0, jurisdiccioniibb = -1, jurisdiccioniibb2 = -1,
            fac_percepcioniibb = 0, fac_percepcioniibb2 = 0;
        string concepto_iibb = "", concepto_iibb2 = "", concepto_iibb_desc = "", concepto_iibb2_desc ="";

        #region METODOS_BASEDATOS

        public DataSet Listar_comprobantes_a_generar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            //DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            //sPar[0] = new DBHelper.StoredProcedureParameter("@Fecha", "20100101");
            //sPar[1] = new DBHelper.StoredProcedureParameter("@Nrocta", IdCliente);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("Elect_comprobantes_a_generar");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Facturacion", "Listar_comprobantes_a_generar", ex, "0");
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }


        public DataSet Listar_pedidos_a_facturar()
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            //DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            //sPar[0] = new DBHelper.StoredProcedureParameter("@Fecha", "20100101");
            //sPar[1] = new DBHelper.StoredProcedureParameter("@Nrocta", IdCliente);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_GET_PEDIDOS_A_FACTURAR");
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Facturacion", "Listar_pedidos_a_facturar", ex, "0");
            }
            finally
            {
                oDb.Disconnect();
            }
            return oDs;
        }

        public DataSet get_datos_pedido(double idpedido)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@idpedido", idpedido);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_GET_DATOS_PEDIDO",sPar);
                oDb.Disconnect();
            return oDs;
        }
        public DataSet get_datos_comprobante(string codmov, string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("Elect_comprobantes_get_comp", sPar);
            oDb.Disconnect();
            return oDs;
        }

        public DataSet set_status_facturado_pedido(double idpedido,string codmov, string nromov, string sucursal,string cteori, string idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[6];
            sPar[0] = new DBHelper.StoredProcedureParameter("@idpedido", idpedido);
            sPar[1] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@nromov", nromov);
            sPar[3] = new DBHelper.StoredProcedureParameter("@sucursal", sucursal);
            sPar[4] = new DBHelper.StoredProcedureParameter("@cteori", cteori);
            sPar[5] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_SET_STATUS_PEDIDO", sPar);
            oDb.Disconnect();

            return oDs;
        }
        public DataSet set_status_comprobante(string ant_codmov, string ant_nromov, string codmov, string nromov, string sucursal, string cteori, string idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[7];
            sPar[0] = new DBHelper.StoredProcedureParameter("@ant_codmov", ant_codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@ant_nromov", ant_nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@sucursal", sucursal);
            sPar[3] = new DBHelper.StoredProcedureParameter("@cteori", cteori);
            sPar[4] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);
            sPar[5] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[6] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("Elect_comprobantes_SET_STATUS", sPar);
            oDb.Disconnect();

            return oDs;
        }


        public DataSet get_IIBB(double nrocta)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@nrocta", nrocta);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_GET_IIBB", sPar);
            oDb.Disconnect();

            return oDs;
        }

        public void calcular_IIBB(double nrocta,double total, double neto)
        {
            DataSet oIibb = new DataSet();
            oIibb = get_IIBB(nrocta);
            fac_percepcioniibb = 0;
            fac_percepcioniibb2 = 0;
            tasaiibb = 0;
            tasaiibb2 = 0;
            concepto_iibb = "";
            concepto_iibb_desc = "";
            concepto_iibb2 = "";
            concepto_iibb2_desc = "";
            foreach (DataRow item in oIibb.Tables[0].Rows )
            {
                if (item["tipo"].ToString() == "IB")
                {
                    tasaiibb = double.Parse (item["porc_iibb"].ToString());
                    jurisdiccioniibb = double.Parse(item["pcia_iibb"].ToString());
                    concepto_iibb = item["concepto_iibb"].ToString();
                    concepto_iibb_desc = item["descrp"].ToString();
                    if (item["tipo_base_calculo"].ToString() == "I" && neto > double.Parse(item["monto_base_calculo"].ToString()))
                    {
                        fac_percepcioniibb = neto * tasaiibb / 100;
                    }
                    if (item["tipo_base_calculo"].ToString() == "T" && total > double.Parse(item["monto_base_calculo"].ToString()))
                    {
                        fac_percepcioniibb = total * tasaiibb / 100;
                    }
                    
                }

                if (item["tipo"].ToString() == "IB2")
                {
                    tasaiibb2 = double.Parse(item["porc_iibb"].ToString());
                    jurisdiccioniibb2 = double.Parse(item["pcia_iibb"].ToString());
                    concepto_iibb2 = item["concepto_iibb"].ToString();
                    concepto_iibb2_desc = item["descrp"].ToString();
                    if (item["tipo_base_calculo"].ToString() == "I" && neto > double.Parse(item["monto_base_calculo"].ToString()))
                    {
                        fac_percepcioniibb2 = neto * tasaiibb2 / 100;
                    }
                    if (item["tipo_base_calculo"].ToString() == "T" && total > double.Parse(item["monto_base_calculo"].ToString()))
                    {
                        fac_percepcioniibb2 = total * tasaiibb2 / 100;
                    }

                }

            }
        }

        private DataSet  get_formulario_numero(string sucursal,string cndiva,string factura_nc)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[3];
            sPar[0] = new DBHelper.StoredProcedureParameter("@sucursal", sucursal);
            sPar[1] = new DBHelper.StoredProcedureParameter("@cndiva", cndiva);
            sPar[2] = new DBHelper.StoredProcedureParameter("@factura_notacredito", factura_nc);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_GET_FORMULARIO", sPar);
                oDb.Disconnect();
            return oDs;           
    
        }

        private DataSet agregar_concepto(DataSet asientos,string concepto,double importe, string deb_hab)
        {
            DataSet resultado = new DataSet();
            DataTable dt = new DataTable("Table");
            DataRow dr;

            dt.Columns.Add(new DataColumn("tipo", typeof(string)));
            dt.Columns.Add(new DataColumn("concepto", typeof(string)));
            dt.Columns.Add(new DataColumn("debhab", typeof(string)));
            dt.Columns.Add(new DataColumn("descripcion", typeof(string)));
            dt.Columns.Add(new DataColumn("importe", typeof(double)));

            //dt = asientos.Tables[0];
            foreach (DataRow item in asientos.Tables[0].Rows)
            {
                dr = dt.NewRow();
                dr["tipo"] = item["tipo"];
                dr["concepto"] = item["concepto"];
                dr["debhab"] = item["debhab"];
                dr["descripcion"] = item["descripcion"];
                dr["importe"] = item["importe"];
                dt.Rows.Add(dr);
            }


            bool encontro = false;
            foreach (DataRow item in asientos.Tables[0].Rows)
            {
                if (item["concepto"].ToString() == concepto)
                {
                    item["importe"] = double.Parse(item["importe"].ToString() )+ importe;
                    encontro = true;
                }
            }

            if (!encontro)
            {
                dr = dt.NewRow();
                dr["tipo"] = "VENTA";
                dr["concepto"] = concepto;
                dr["debhab"] = deb_hab;
                dr["descripcion"] = concepto;
                dr["importe"] = importe;
                dt.Rows.Add(dr);

                resultado.Tables.Add(dt);
            }
            else
            {
                resultado = asientos;
            }

            

            return resultado;
        }

        public void insert_fe_calculofactura( string idproducto, string descripcion, double cantidad,double precio,double subtotal,string tipo,
            double tasaiva, double tasaimpuestointerno, int facturasiniva, double importeiva, double importeimpinterno, double netogravado,
            string cptvta, double tasaiibb, double tasaiibb2, double tasaimpuestomunicipal, int jurisdiccioniibb, int jurisdiccioniibb2, double _idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            double idfactura = 1;

            if (_idfactura == 0)
            {
                    oDb.Connect();
                    oDs = oDb.ExecuteSqlAsDataSet("SELECT id=ISNULL((SELECT MAX(IDFACTURA) FROM FE_CALCULOFACTURA),0) + 1");
                    if (oDs.IsInitialized)
                    {
                        idfactura = double.Parse(oDs.Tables[0].Rows[0]["id"].ToString());
                    }
                    oDb.Disconnect();
            }
            else
            {
                idfactura = _idfactura;
            }

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[19];
            sPar[0] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);
            sPar[1] = new DBHelper.StoredProcedureParameter("@idproducto", idproducto);
            sPar[2] = new DBHelper.StoredProcedureParameter("@descripcion", descripcion);
            sPar[3] = new DBHelper.StoredProcedureParameter("@cantidad", cantidad);
            sPar[4] = new DBHelper.StoredProcedureParameter("@precio", precio);
            sPar[5] = new DBHelper.StoredProcedureParameter("@subtotal", subtotal);
            sPar[6] = new DBHelper.StoredProcedureParameter("@tipo", tipo);
            sPar[7] = new DBHelper.StoredProcedureParameter("@tasaiva", tasaiva);
            sPar[8] = new DBHelper.StoredProcedureParameter("@tasaimpuestointerno", tasaimpuestointerno);
            sPar[9] = new DBHelper.StoredProcedureParameter("@facturasiniva", facturasiniva);
            sPar[10] = new DBHelper.StoredProcedureParameter("@importeiva", importeiva);
            sPar[11] = new DBHelper.StoredProcedureParameter("@importeimpinterno", importeimpinterno);
            sPar[12] = new DBHelper.StoredProcedureParameter("@netogravado", netogravado);
            sPar[13] = new DBHelper.StoredProcedureParameter("@cptvta", cptvta);
            sPar[14] = new DBHelper.StoredProcedureParameter("@tasaiibb", tasaiibb);
            sPar[15] = new DBHelper.StoredProcedureParameter("@tasaiibb2", tasaiibb2);
            sPar[16] = new DBHelper.StoredProcedureParameter("@tasaimpuestomunicipal", tasaimpuestomunicipal);
            sPar[17] = new DBHelper.StoredProcedureParameter("@jurisdiccioniibb", jurisdiccioniibb);
            sPar[18] = new DBHelper.StoredProcedureParameter("@jurisdiccioniibb2", jurisdiccioniibb2);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("[fe_calculofactura_insertar]", sPar);
            oDb.Disconnect();


        }
        
        public string insert_pedido(string idpedido,string idvendedor,string idcliente,string idclientenuevo ,string fecha, string factura,
            string idproducto, double cantidad, double precio, string tipo, Boolean encabezado, string cobrado, string nro_recibo )
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            string resultado = "0";
            string enc = "0";
            if (encabezado)
            {
                enc = "1";
            }
                    DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[13];
                    sPar[0] = new DBHelper.StoredProcedureParameter("@idpedido", idpedido);
                    sPar[1] = new DBHelper.StoredProcedureParameter("@idcliente", idcliente);
                    sPar[2] = new DBHelper.StoredProcedureParameter("@idclientenuevo", idclientenuevo);
                    sPar[3] = new DBHelper.StoredProcedureParameter("@fecha", fecha);
                    sPar[4] = new DBHelper.StoredProcedureParameter("@factura", factura);
                    sPar[5] = new DBHelper.StoredProcedureParameter("@idproducto", idproducto);
                    sPar[6] = new DBHelper.StoredProcedureParameter("@cantidad", cantidad);
                    sPar[7] = new DBHelper.StoredProcedureParameter("@precio", precio);
                    sPar[8] = new DBHelper.StoredProcedureParameter("@tipo", tipo);
                    sPar[9] = new DBHelper.StoredProcedureParameter("@encabezado", enc);
                    sPar[10] = new DBHelper.StoredProcedureParameter("@idvendedor", idvendedor);
                    sPar[11] = new DBHelper.StoredProcedureParameter("@cobrado", cobrado);
                    sPar[12] = new DBHelper.StoredProcedureParameter("@nrorecibo", nro_recibo);
                    

                    oDb.Connect();
                    oDs = oDb.ExecuteProcedureAsDataSet ("MENSAJERO_FAC_INS_PEDIDO",sPar);
                    if (oDs.IsInitialized)
                    {
                        resultado = oDs.Tables[0].Rows[0]["resultado"].ToString();
                    }
                    oDb.Disconnect();


                return resultado;
        }

        public DataSet insertar_encabezado(string codmov ,double nromov, string cteori,string fchmov,double nrocta,double nrosub, string nombre,string direccion, string localidad, string provincia,
            string cndiva, string cndpag, string idvendedor, string usuario, string nrcuit,string idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[16];
            sPar[0] = new DBHelper.StoredProcedureParameter("@CODMOV", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@NROMOV", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@CTEORI", cteori);
            sPar[3] = new DBHelper.StoredProcedureParameter("@FCHMOV", fchmov);
            sPar[4] = new DBHelper.StoredProcedureParameter("@NROCTA", nrocta);
            sPar[5] = new DBHelper.StoredProcedureParameter("@NROSUB", nrosub);
            sPar[6] = new DBHelper.StoredProcedureParameter("@NOMBRE", nombre);
            sPar[7] = new DBHelper.StoredProcedureParameter("@DIRECCION", direccion);
            sPar[8] = new DBHelper.StoredProcedureParameter("@LOCALIDAD", localidad);
            sPar[9] = new DBHelper.StoredProcedureParameter("@PROVINCIA", provincia);
            sPar[10] = new DBHelper.StoredProcedureParameter("@CNDIVA", cndiva);
            sPar[11] = new DBHelper.StoredProcedureParameter("@CNDPAG", cndpag);
            sPar[12] = new DBHelper.StoredProcedureParameter("@IDVENDEDOR", idvendedor);
            sPar[13] = new DBHelper.StoredProcedureParameter("@USUARIO", usuario);
            sPar[14] = new DBHelper.StoredProcedureParameter("@nrcuit", nrcuit);
            sPar[15] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);



                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_INSERT_ENCABEZADO",sPar);
                oDb.Disconnect();
            return oDs;
        }
        public DataSet insertar_cuenta_corriente(string codmov, double nromov, string cteori, string fchmov, double nrocta,
            double nrosub,string codapl, string nroapl, double importe, double nroitem,string idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[11];
            sPar[0] = new DBHelper.StoredProcedureParameter("@CODMOV", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@NROMOV", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@CTEORI", cteori);
            sPar[3] = new DBHelper.StoredProcedureParameter("@FCHMOV", fchmov);
            sPar[4] = new DBHelper.StoredProcedureParameter("@NROCTA", nrocta);
            sPar[5] = new DBHelper.StoredProcedureParameter("@NROSUB", nrosub);
            sPar[6] = new DBHelper.StoredProcedureParameter("@codapl", codapl);
            sPar[7] = new DBHelper.StoredProcedureParameter("@nroapl", nroapl);
            sPar[8] = new DBHelper.StoredProcedureParameter("@importe", importe);
            sPar[9] = new DBHelper.StoredProcedureParameter("@nritem", nroitem);
            sPar[10] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);



                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_INSERT_CTACTEVT", sPar);
                oDb.Disconnect();
            return oDs;
        }
        public DataSet insertar_movimvtas(string codmov, double nromov, string cteori, string fchmov, double nrocta, string nombre, string direccion, 
            string localidad, string provincia,string cndiva, string cndpag, string idvendedor,string nrocuit,string idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[14];
            sPar[0] = new DBHelper.StoredProcedureParameter("@CODMOV", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@NROMOV", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@CTEORI", cteori);
            sPar[3] = new DBHelper.StoredProcedureParameter("@FCHMOV", fchmov);
            sPar[4] = new DBHelper.StoredProcedureParameter("@NROCTA", nrocta);
            sPar[5] = new DBHelper.StoredProcedureParameter("@NOMBRE", nombre);
            sPar[6] = new DBHelper.StoredProcedureParameter("@DIRECCION", direccion);
            sPar[7] = new DBHelper.StoredProcedureParameter("@LOCALIDAD", localidad);
            sPar[8] = new DBHelper.StoredProcedureParameter("@PROVINCIA", provincia);
            sPar[9] = new DBHelper.StoredProcedureParameter("@CNDIVA", cndiva);
            sPar[10] = new DBHelper.StoredProcedureParameter("@CNDPAG", cndpag);
            sPar[11] = new DBHelper.StoredProcedureParameter("@IDVENDEDOR", idvendedor);
            sPar[12] = new DBHelper.StoredProcedureParameter("@NROCUIT", nrocuit);
            sPar[13] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);



                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_INSERT_MOVIMVTAS", sPar);
                oDb.Disconnect();
            return oDs;
        }

        public DataSet insertar_facturacioncierres(string codmov, double nromov, string fchmov, double nrocta, string idvendedor, string idpedido)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[6];
            sPar[0] = new DBHelper.StoredProcedureParameter("@CODMOV", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@NROMOV", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@FCHMOV", fchmov);
            sPar[3] = new DBHelper.StoredProcedureParameter("@NROCTA", nrocta);
            sPar[4] = new DBHelper.StoredProcedureParameter("@idpedido", idpedido);
            sPar[5] = new DBHelper.StoredProcedureParameter("@IDVENDEDOR", idvendedor);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_INSERT_RECIBO_FACTURACIONCIERRES", sPar);
                oDb.Disconnect();
            return oDs;
        }


        public DataSet insertar_item(string codmov, double nromov, double item, string tipcod, string idproducto, string descripcion,
                                            double cantidad, double precio, double subtotal, string debhab, double impiva,double impinterno,string idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[13];
            sPar[0] = new DBHelper.StoredProcedureParameter("@CODMOV", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@NROMOV", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@item", item);
            sPar[3] = new DBHelper.StoredProcedureParameter("@tipcod", tipcod);
            sPar[4] = new DBHelper.StoredProcedureParameter("@idproducto", idproducto);
            sPar[5] = new DBHelper.StoredProcedureParameter("@descripcion", descripcion);
            sPar[6] = new DBHelper.StoredProcedureParameter("@cantidad", cantidad);
            sPar[7] = new DBHelper.StoredProcedureParameter("@precio", precio);
            sPar[8] = new DBHelper.StoredProcedureParameter("@subtotal", subtotal);
            sPar[9] = new DBHelper.StoredProcedureParameter("@debhab", debhab);
            sPar[10] = new DBHelper.StoredProcedureParameter("@impiva", impiva);
            sPar[11] = new DBHelper.StoredProcedureParameter("@impinterno", impinterno);
            sPar[12] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);


                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_INSERT_ITEM", sPar);
                oDb.Disconnect();
            return oDs;
        }

        public DataSet insertar_subdiavt(string codmov, double nromov,double item,string concepto,string debhab, double importe,string idfactura)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[7];
            sPar[0] = new DBHelper.StoredProcedureParameter("@CODMOV", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@NROMOV", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@item", item);
            sPar[3] = new DBHelper.StoredProcedureParameter("@concepto", concepto);
            sPar[4] = new DBHelper.StoredProcedureParameter("@debhab", debhab);
            sPar[5] = new DBHelper.StoredProcedureParameter("@importe", importe);
            sPar[6] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);

                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_INSERT_SUBDIAVT", sPar);
                oDb.Disconnect();
            return oDs;
        }

        public double get_ultimonro_recibo(string codmov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            double ultnumero = 0;
                oDb.Connect();
                oDs = oDb.ExecuteSqlAsDataSet("SELECT ultnum = ISNULL((SELECT ultnum = max(nromov) FROM movimvtas WHERE CODMOV = '" + codmov + "'),0)");
                if (oDs.IsInitialized)
                {
                    ultnumero = double.Parse(oDs.Tables[0].Rows[0]["ultnum"].ToString());
                }
                oDb.Disconnect();
            return ultnumero;
        }
        public double comprobante_numeracion_actualizar(string usuario)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            double idcomprobante = 0;
            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@usuario", usuario);


            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("facturasnumeracion_actualizar", sPar);
            oDb.Disconnect();
            if (oDs.IsInitialized)
            {
                idcomprobante = double.Parse(oDs.Tables[0].Rows[0]["idfactura"].ToString());
            }
            oDb.Disconnect();
            return idcomprobante;
        }
        public double comprobante_numeracion_actualizar(string usuario,string idfactura, string cteori,string codmov,string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            double idcomprobante = 0;

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[5];
            sPar[0] = new DBHelper.StoredProcedureParameter("@usuario", usuario);
            sPar[1] = new DBHelper.StoredProcedureParameter("@idfactura", idfactura);
            sPar[2] = new DBHelper.StoredProcedureParameter("@cterori", cteori);
            sPar[3] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[4] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("facturasnumeracion_actualizar", sPar);
            oDb.Disconnect();
            if (oDs.IsInitialized)
            {
                idcomprobante = double.Parse(oDs.Tables[0].Rows[0]["idfactura"].ToString());
            }
            oDb.Disconnect();
            return idcomprobante;
        }

        public void set_valor_cuenta(string cuenta,string importe)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            double ultnumero = 0;
                oDb.Connect();
                oDb.ExecuteSqlNonQuery("UPDATE CTASTES SET SALDOC = SALDOC +" + importe + " WHERE CODIGO = '" + cuenta + "'");
                oDb.Disconnect();
        }

        public DataSet insertar_movtes(string codmov, double nromov, string fchmov, double nrocta,
            string tipocuenta, string referencia,string concepto,string nrobanco,string sucursal,
            string cpbanco,string nrocheque, string cuenta, string titular, string fchvencimiento,string ctacte,
            double importe, double nroitem,string codasi,string nroasi, string debhab)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[20];
            sPar[0] = new DBHelper.StoredProcedureParameter("@CODMOV", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@NROMOV", nromov);
            sPar[2] = new DBHelper.StoredProcedureParameter("@nritem", nroitem);
            sPar[3] = new DBHelper.StoredProcedureParameter("@FCHMOV", fchmov);
            sPar[4] = new DBHelper.StoredProcedureParameter("@NROCTA", nrocta);
            sPar[5] = new DBHelper.StoredProcedureParameter("@refern", referencia);
            sPar[6] = new DBHelper.StoredProcedureParameter("@CodCpt", concepto);
            sPar[7] = new DBHelper.StoredProcedureParameter("@NroBco", nrobanco);
            sPar[8] = new DBHelper.StoredProcedureParameter("@Sucurs", sucursal);
            sPar[9] = new DBHelper.StoredProcedureParameter("@CPBcos", cpbanco);
            sPar[10] = new DBHelper.StoredProcedureParameter("@Cheque", nrocheque);
            sPar[11] = new DBHelper.StoredProcedureParameter("@Cuenta", cuenta);
            sPar[12] = new DBHelper.StoredProcedureParameter("@Titular", titular);
            sPar[13] = new DBHelper.StoredProcedureParameter("@FchVnc", fchvencimiento);
            sPar[14] = new DBHelper.StoredProcedureParameter("@CtaCte", ctacte);
            sPar[15] = new DBHelper.StoredProcedureParameter("@Import", importe);
            sPar[16] = new DBHelper.StoredProcedureParameter("@DebHab",debhab );
            sPar[17] = new DBHelper.StoredProcedureParameter("@CodAsi", codasi);
            sPar[18] = new DBHelper.StoredProcedureParameter("@NroAsi", nroasi);
            sPar[19] = new DBHelper.StoredProcedureParameter("@TipCta", tipocuenta);


                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("MENSAJERO_FAC_INSERT_MOVTES", sPar);
                oDb.Disconnect();
            return oDs;
        }

        public void Rollback_FACTURA(string idpedido, string codmov_fac, string nromov_fac)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            string sSql = "";

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov_fac);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov_fac);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("SP_DELETE_FACTURA", sPar);

                sSql = "DELETE FROM MOVIMIENTOS_CAJA_AJUSTES WHERE CODMOV = '" + codmov_fac + "' AND NROMOV = " + nromov_fac;
                oDb.ExecuteSqlNonQuery(sSql);

                sSql = "DELETE FROM NOVEDADES WHERE CODMOV = '" + codmov_fac + "' AND NROMOV = " + nromov_fac;
                oDb.ExecuteSqlNonQuery(sSql);

                sSql = "DELETE FROM FACTURAELECTRONICA_MOVIL WHERE CODMOV = '" + codmov_fac + "' AND NROMOV = " + nromov_fac;
                oDb.ExecuteSqlNonQuery(sSql);

                sSql = "UPDATE FRMVTAS SET ULTNUM="+ ( long.Parse(nromov_fac)-1).ToString ()+" WHERE CODIGO = '" + codmov_fac + "'" ;
                oDb.ExecuteSqlNonQuery(sSql);


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Facturacion", "Rollback_FACTURA", ex, "0");
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void Rollback_Comprobante(string codmov, string nromov)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            string sSql = "";

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[2];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov);

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("SP_DELETE_FACTURA", sPar);

                sSql = "DELETE FROM MOVIMIENTOS_CAJA_AJUSTES WHERE CODMOV = '" + codmov + "' AND NROMOV = " + nromov;
                oDb.ExecuteSqlNonQuery(sSql);

                sSql = "DELETE FROM NOVEDADES WHERE CODMOV = '" + codmov + "' AND NROMOV = " + nromov;
                oDb.ExecuteSqlNonQuery(sSql);

                sSql = "DELETE FROM COMPROBANTES_ELECTRONICOS WHERE CODMOV = '" + codmov + "' AND NROMOV = " + nromov;
                oDb.ExecuteSqlNonQuery(sSql);

                sSql = "UPDATE FRMVTAS SET ULTNUM=" + (long.Parse(nromov) - 1).ToString() + " WHERE CODIGO = '" + codmov + "'";
                oDb.ExecuteSqlNonQuery(sSql);


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Facturacion", "Rollback_COMPROBATE", ex, "0");
            }
            finally
            {
                oDb.Disconnect();
            }
        }
        public void Rollback_RECIBO(string idpedido, string codmov_rec, string nromov_rec)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();
            string sSql = "";

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[3];
            sPar[0] = new DBHelper.StoredProcedureParameter("@codmov", codmov_rec);
            sPar[1] = new DBHelper.StoredProcedureParameter("@nromov", nromov_rec);
            sPar[2] = new DBHelper.StoredProcedureParameter("@usuario", "EXPORTADOR_AFIP");

            try
            {
                oDb.Connect();
                oDs = oDb.ExecuteProcedureAsDataSet("SP_DELETE_RECIBO", sPar);

                sSql = "DELETE FROM FACTURAELECTRONICA_MOVIL WHERE CODMOV = '" + codmov_rec + "' AND NROMOV = " + nromov_rec;
                oDb.ExecuteSqlNonQuery(sSql);

                sSql = "UPDATE FRMVTAS SET ULTNUM=" + (long.Parse(nromov_rec) - 1).ToString() + " WHERE CODIGO = '" + codmov_rec + "'";
                oDb.ExecuteSqlNonQuery(sSql);


            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Facturacion", "Rollback_RECIBO", ex, "0");
            }
            finally
            {
                oDb.Disconnect();
            }
        }

        public DataSet generar_remito(string idpedido)
        {
            DataSet oDs = new DataSet();
            DBHelper oDb = new DBHelper();

            DBHelper.StoredProcedureParameter[] sPar = new DBHelper.StoredProcedureParameter[1];
            sPar[0] = new DBHelper.StoredProcedureParameter("@idpedido", idpedido);

            oDb.Connect();
            oDs = oDb.ExecuteProcedureAsDataSet("Elect_Comprobantes_GENERAR_REMITO", sPar);
            oDb.Disconnect();
            return oDs;
        }
#endregion

        #region METODOS_CLASE

        public string Generar_recibo(double nrocta, double nrosub, string cte_recibo, double nro_recibo, string codfac, double nrofac, double importe, DataRow item, string idpedido, Boolean cuentacorriente)
        {
            Boolean genero = false;
            string recibo_generado = "";
            string fecha = "", nombre= "",direccion ="",localidad = "",cndiva="",nrcuit="",vendedor="",cndpag="",referencia="";


            if (nro_recibo == 0)
            {
                cte_recibo = "RT";
                nro_recibo = get_ultimonro_recibo(cte_recibo);
                nro_recibo += 1;
                fecha = item["fecha"].ToString();
                nombre = item["nombre"].ToString();
                direccion = item["direcc"].ToString();
                localidad = item["localidad"].ToString();
                cndiva = item["cndiva"].ToString();
                nrcuit = item["nrcuit"].ToString();
                vendedor = item["idvendedor"].ToString();
                cndpag = item["cndpag"].ToString();
                referencia = "Cobrado x Celular. Rep." + item["idvendedor"].ToString();

                //inserta cuenta corriente
                if (cuentacorriente)
                {
                    insertar_cuenta_corriente(cte_recibo, nro_recibo, cte_recibo, fecha, nrocta, 0, codfac, nrofac.ToString(), importe, 1, "0");
                }
                //inserta movimvtas
                insertar_movimvtas(cte_recibo, nro_recibo, cte_recibo, fecha, nrocta, nombre, direccion, localidad, "0", cndiva, cndpag, vendedor, nrcuit, "0");
                //inserta subdiavt
                insertar_subdiavt(cte_recibo, nro_recibo, 1, "T01", "D", importe * -1, "0");
                insertar_subdiavt(cte_recibo, nro_recibo, 2, "CCB", "D", importe * -1, "0");

                //insertar movtes
                insertar_movtes(cte_recibo, nro_recibo, fecha, nrocta, "C", referencia, "EFE", "", "", "", "", "", "", fecha, "EF", importe * -1, 1, "", "0", "D");
                insertar_movtes(cte_recibo, nro_recibo, fecha, nrocta, "C", referencia, "CCB", "", "", "", "", "", "", fecha, "EF", importe * -1, 2, "", "0", "H");

                //facturacioncierres
                insertar_facturacioncierres(cte_recibo, nro_recibo, fecha, nrocta, vendedor, idpedido); 

                //suma el efectivo a la cuenta de tesoreria
                set_valor_cuenta("EF", (importe * -1).ToString());

                genero = true;
            }
            recibo_generado = cte_recibo + ";" + nro_recibo.ToString();
 
            return recibo_generado;
        }

        public string Facturar_pedido(double idpedido)
        {
            DataSet oItems = new DataSet();
            DataSet formulario = new DataSet();
            DataSet resultado = new DataSet();
            string pedido_facturado = "";
            pedido_facturado = idpedido.ToString() + ';';
            oItems = get_datos_pedido(idpedido);
            bool iva_explicito = false, encabezado = true,cuentacorriente = false;
            double nroitem = 0, total_factura =0, nro_recibo = 0, cobrado = 0,nroitem_subdia=0;
            
            double total_productos =0, total_iva =0, total_impuesto_interno=0, total_neto=0, nrocta =0,nromov =0,IDFACTURA=0;
            string sucursal = "",codmov = "", comprobante ="", fecha = "", idreparto = "0",recibo_codmov="",recibo_nromov="";
            string[] recibo;

            try
            {

                foreach (DataRow item in oItems.Tables[0].Rows)
                {
                    total_productos += double.Parse(item["subtotal"].ToString());
                    total_neto += double.Parse(item["neto"].ToString());
                    total_iva += double.Parse(item["iva"].ToString());
                    total_impuesto_interno += double.Parse(item["ii"].ToString());
                    fecha = item["fecha"].ToString();
                    nro_recibo = double.Parse(item["nro_recibo"].ToString());
                    cobrado = double.Parse(item["cobrado"].ToString());
                    idreparto = item["idvendedor"].ToString();

                    if (item["cndpag"].ToString() == "2")
                    {
                        cuentacorriente = true;
                    }

                    if (item["nrosub"].ToString() != "0")
                    {
                        nrocta = double.Parse(item["nrosub"].ToString());
                    }
                    else
                    {
                        nrocta = double.Parse(item["nrocta"].ToString());
                    }
                    sucursal = item["sucursal_comprobante"].ToString();
                    if (item["IVATIPO"].ToString() == "E")
                    {
                        iva_explicito = true;
                    }
                }
                if (total_productos > 0)
                {
                    calcular_IIBB(nrocta, total_productos, total_neto);


                    DataSet asiento = new DataSet();
                    DataTable dt = new DataTable("MyTable");

                    dt.Columns.Add(new DataColumn("tipo", typeof(string)));
                    dt.Columns.Add(new DataColumn("concepto", typeof(string)));
                    dt.Columns.Add(new DataColumn("debhab", typeof(string)));
                    dt.Columns.Add(new DataColumn("descripcion", typeof(string)));
                    dt.Columns.Add(new DataColumn("importe", typeof(double)));

                    DataRow dr = dt.NewRow();

                    //IVA
                    dr["tipo"] = "IVA";
                    dr["concepto"] = "I01";
                    dr["debhab"] = "H";
                    dr["descripcion"] = "";
                    dr["importe"] = total_iva;
                    dt.Rows.Add(dr);


                    //IIBB
                    if (concepto_iibb != "")
                    {
                        dr = dt.NewRow();
                        dr["tipo"] = "IB";
                        dr["concepto"] = concepto_iibb;
                        dr["debhab"] = "H";
                        dr["descripcion"] = concepto_iibb_desc;
                        dr["importe"] = fac_percepcioniibb;
                        dt.Rows.Add(dr);
                    }

                    //IIBB2
                    if (concepto_iibb2 != "")
                    {
                        dr = dt.NewRow();
                        dr["tipo"] = "IB2";
                        dr["concepto"] = concepto_iibb2;
                        dr["debhab"] = "H";
                        dr["descripcion"] = concepto_iibb2_desc;
                        dr["importe"] = fac_percepcioniibb2;
                        dt.Rows.Add(dr);
                    }

                    asiento.Tables.Add(dt);


                    formulario = get_formulario_numero(sucursal, oItems.Tables[0].Rows[0]["cndiva"].ToString(),"FC");

                    codmov = formulario.Tables[0].Rows[0]["form"].ToString();
                    comprobante = formulario.Tables[0].Rows[0]["comprobante"].ToString();
                    nromov = double.Parse(formulario.Tables[0].Rows[0]["ultnum"].ToString());
                    nromov += 1;


                    IDFACTURA = comprobante_numeracion_actualizar("EXPORTADORAFIP");

                    foreach (DataRow item in oItems.Tables[0].Rows)
                    {
                        nroitem +=1;
                        asiento = agregar_concepto(asiento, item["conceptoventa"].ToString(), double.Parse(item["neto"].ToString()),"H");

                        insert_fe_calculofactura(item["idproducto"].ToString(), item["descripcion"].ToString(), double.Parse(item["cantidad"].ToString()), double.Parse(item["precio"].ToString()),
                            double.Parse(item["subtotal"].ToString()), "P", double.Parse(item["TASAIVA"].ToString()), double.Parse(item["impuesto_interno"].ToString()), 0, double.Parse(item["iva"].ToString()),
                            double.Parse(item["ii"].ToString()), double.Parse(item["neto"].ToString()), item["conceptoventa"].ToString(), tasaiibb, tasaiibb2, 0, int.Parse(jurisdiccioniibb.ToString()), int.Parse(jurisdiccioniibb2.ToString()), IDFACTURA);

                        if (encabezado)
                        {
                            insertar_encabezado(codmov, nromov, comprobante, item["fecha"].ToString(), 
                                                double.Parse(item["nrocta"].ToString()), double.Parse(item["nrosub"].ToString()), 
                                                item["nombre"].ToString(), item["direcc"].ToString(),item["localidad"].ToString(), 
                                                item["provincia"].ToString(), item["cndiva"].ToString(),item["cndpag"].ToString(),
                                                item["idvendedor"].ToString(), "MENSAJERO", item["nrcuit"].ToString(),IDFACTURA.ToString());

                            insertar_movimvtas(codmov, nromov, comprobante, item["fecha"].ToString(),
                                                double.Parse(item["nrocta"].ToString()), item["nombre"].ToString(), item["direcc"].ToString(), item["localidad"].ToString(),
                                                item["provincia"].ToString(), item["cndiva"].ToString(), item["cndpag"].ToString(),
                                                item["idvendedor"].ToString(), item["nrcuit"].ToString(), IDFACTURA.ToString());

                            encabezado = false;
                        }

                        insertar_item(codmov, nromov, nroitem, "P", item["idproducto"].ToString(), item["descripcion"].ToString(), double.Parse(item["cantidad"].ToString()),
                            double.Parse(item["precio"].ToString()), double.Parse(item["subtotal"].ToString()), "H", double.Parse(item["iva"].ToString()), double.Parse(item["ii"].ToString()), IDFACTURA.ToString());
                    }

                    
                    foreach (DataRow item in asiento.Tables[0].Rows)
                    {
                        if ((item["tipo"].ToString() != "VENTA")  )
                        {
                            if ((item["tipo"].ToString() != "IVA") || (item["tipo"].ToString() == "IVA" && iva_explicito))
                            {
                                insert_fe_calculofactura(item["tipo"].ToString(), item["descripcion"].ToString(), 0, 0,
                                    double.Parse(item["importe"].ToString()), "C", 0, 0, 0, 0,
                                    0, 0, "", tasaiibb, tasaiibb2, 0, int.Parse(jurisdiccioniibb.ToString()), int.Parse(jurisdiccioniibb2.ToString()), IDFACTURA);
                                nroitem += 1;
                                insertar_item(codmov, nromov, nroitem, "C", item["tipo"].ToString(), item["descripcion"].ToString(), 0,0,
                                    double.Parse(item["importe"].ToString()), "H", 0, 0, IDFACTURA.ToString());

                            }
                        }
                    }


                    //SUBIDAVT
                    nroitem_subdia = 0;
                    foreach (DataRow item in asiento.Tables[0].Rows)
                    {
                        nroitem_subdia += 1;
                        insertar_subdiavt(codmov, nromov, nroitem_subdia, item["concepto"].ToString(), item["debhab"].ToString(),
                            double.Parse(item["importe"].ToString()), IDFACTURA.ToString());
                        
                    }


                    total_factura =  total_productos + fac_percepcioniibb + fac_percepcioniibb2 ;
                    if (iva_explicito)
                    {
                        total_factura+= total_iva;
                    }

                    //total factura 
                    insert_fe_calculofactura("TF", "TOTAL FACTURA", 0, 0, total_factura, "C", 0, 0, 0, 0, 0, 0, "", 0, 0, 0, int.Parse(jurisdiccioniibb.ToString()), int.Parse(jurisdiccioniibb2.ToString()), IDFACTURA);
                    //total factura en facturasitem
                    nroitem += 1;
                    insertar_item(codmov, nromov, nroitem, "C", "TF", "Total Comprobante", 0, 0, total_factura, "D", 0, 0, IDFACTURA.ToString());
                    //total en subdiavt
                    nroitem_subdia += 1;
                    insertar_subdiavt(codmov, nromov, nroitem_subdia, "T01", "D", total_factura, IDFACTURA.ToString());

                    if (cuentacorriente)
                    {
                        insertar_cuenta_corriente(codmov, nromov, comprobante, fecha, nrocta, 0, codmov, nromov.ToString(), total_factura, 1,IDFACTURA.ToString());
                    }

                    //setea como facturado el pedido
                    set_status_facturado_pedido(idpedido, codmov, nromov.ToString(), sucursal, comprobante, IDFACTURA.ToString());

                    if (cobrado != 0 && Math.Round(total_factura, 2) == cobrado * -1)
                    {
                        string resultado_recibo = Generar_recibo(nrocta, nrocta, "RC", nro_recibo, codmov, nromov, cobrado, oItems.Tables[0].Rows[0], idpedido.ToString(), cuentacorriente);
                        
                        if (resultado_recibo != "")
                        {
                            recibo = resultado_recibo.Split(';');
                            recibo_codmov =recibo[0];
                            recibo_nromov =recibo[1];
                        }
                        else
                        {
                            //no genero recibo
                        }
                    }
                    
                    pedido_facturado += codmov + ';' + nromov.ToString();
                    

                }
            }
            catch (Exception ex)
            {
                //eliminar comprobantes
                pedido_facturado = "ERROR;idpedido" + idpedido.ToString()+";" + ex.Message ;
                Rollback(idpedido.ToString(), codmov, nromov.ToString(), recibo_codmov, recibo_nromov);
            }
            return pedido_facturado;
        }

        public string generar_comprobante(string ant_codmov, string ant_nromov)
        {



            DataSet oItems = new DataSet();
            DataSet formulario = new DataSet();
            DataSet resultado = new DataSet();



            string pedido_facturado = "";
            //pedido_facturado = idpedido.ToString() + ';';
            oItems = get_datos_comprobante(ant_codmov, ant_nromov);
            bool iva_explicito = false, encabezado = true, cuentacorriente = false, es_factura = true;
            double nroitem = 0, total_factura = 0, nro_recibo = 0, cobrado = 0, nroitem_subdia = 0;

            double total_productos = 0, total_iva = 0, total_impuesto_interno = 0, total_neto = 0, nrocta = 0, nromov = 0, IDFACTURA = 0;
            string sucursal = "", codmov = "", comprobante = "", fecha = "", idreparto = "0", recibo_codmov = "", recibo_nromov = "",deb_hab ="", fac_nc;
            string[] recibo;

            if (ant_codmov == "DI")
            {
                es_factura = true;
                fac_nc = "FC";
            }
            else
            {
                es_factura = false;
                fac_nc = "NC";
            }

            try
            {

                foreach (DataRow item in oItems.Tables[0].Rows)
                {
                    total_productos += double.Parse(item["subtotal"].ToString());
                    total_neto += double.Parse(item["neto"].ToString());
                    total_iva += double.Parse(item["iva"].ToString());
                    total_impuesto_interno += double.Parse(item["ii"].ToString());
                    fecha = item["fecha"].ToString();
                    nro_recibo = double.Parse(item["nro_recibo"].ToString());
                    cobrado = double.Parse(item["cobrado"].ToString());
                    idreparto = item["idvendedor"].ToString();

                    if (item["cndpag"].ToString() == "2")
                    {
                        cuentacorriente = true;
                    }

                    if (item["nrosub"].ToString() != "0")
                    {
                        nrocta = double.Parse(item["nrosub"].ToString());
                    }
                    else
                    {
                        nrocta = double.Parse(item["nrocta"].ToString());
                    }
                    sucursal = item["sucursal_comprobante"].ToString();
                    if (item["IVATIPO"].ToString() == "E")
                    {
                        iva_explicito = true;
                    }
                }
                if (total_productos > 0)
                {
                    //calcular_IIBB(nrocta, total_productos, total_neto);


                    DataSet asiento = new DataSet();
                    DataTable dt = new DataTable("MyTable");

                    dt.Columns.Add(new DataColumn("tipo", typeof(string)));
                    dt.Columns.Add(new DataColumn("concepto", typeof(string)));
                    dt.Columns.Add(new DataColumn("debhab", typeof(string)));
                    dt.Columns.Add(new DataColumn("descripcion", typeof(string)));
                    dt.Columns.Add(new DataColumn("importe", typeof(double)));

                    DataRow dr = dt.NewRow();

                    if (es_factura)
                    {
                        deb_hab = "H";
                    }
                    else
                    {
                        deb_hab = "D";
                    }

                    //IVA
                    dr["tipo"] = "IVA";
                    dr["concepto"] = "I01";
                    dr["debhab"] = deb_hab;
                    dr["descripcion"] = "";
                    dr["importe"] = total_iva;
                    dt.Rows.Add(dr);

                    asiento.Tables.Add(dt);



                    formulario = get_formulario_numero(sucursal, oItems.Tables[0].Rows[0]["cndiva"].ToString(), fac_nc);

                    codmov = formulario.Tables[0].Rows[0]["form"].ToString();
                    comprobante = formulario.Tables[0].Rows[0]["comprobante"].ToString();
                    nromov = double.Parse(formulario.Tables[0].Rows[0]["ultnum"].ToString());
                    nromov += 1;


                    IDFACTURA = comprobante_numeracion_actualizar("EXPORTADORAFIP");

                    foreach (DataRow item in oItems.Tables[0].Rows)
                    {
                        nroitem += 1;
                        asiento = agregar_concepto(asiento, item["conceptoventa"].ToString(), double.Parse(item["neto"].ToString()), deb_hab);

                        insert_fe_calculofactura(item["idproducto"].ToString(), item["descripcion"].ToString(), double.Parse(item["cantidad"].ToString()), double.Parse(item["precio"].ToString()),
                            double.Parse(item["subtotal"].ToString()), "P", double.Parse(item["TASAIVA"].ToString()), double.Parse(item["impuesto_interno"].ToString()), 0, double.Parse(item["iva"].ToString()),
                            double.Parse(item["ii"].ToString()), double.Parse(item["neto"].ToString()), item["conceptoventa"].ToString(), tasaiibb, tasaiibb2, 0, int.Parse(jurisdiccioniibb.ToString()), int.Parse(jurisdiccioniibb2.ToString()), IDFACTURA);

                        if (encabezado)
                        {
                            insertar_encabezado(codmov, nromov, comprobante, item["fecha"].ToString(),
                                                double.Parse(item["nrocta"].ToString()), double.Parse(item["nrosub"].ToString()),
                                                item["nombre"].ToString(), item["direcc"].ToString(), item["localidad"].ToString(),
                                                item["provincia"].ToString(), item["cndiva"].ToString(), item["cndpag"].ToString(),
                                                item["idvendedor"].ToString(), "EXPORTADOR", item["nrcuit"].ToString(), IDFACTURA.ToString());

                            insertar_movimvtas(codmov, nromov, comprobante, item["fecha"].ToString(),
                                                double.Parse(item["nrocta"].ToString()), item["nombre"].ToString(), item["direcc"].ToString(), item["localidad"].ToString(),
                                                item["provincia"].ToString(), item["cndiva"].ToString(), item["cndpag"].ToString(),
                                                item["idvendedor"].ToString(), item["nrcuit"].ToString(), IDFACTURA.ToString());

                            encabezado = false;
                        }

                        insertar_item(codmov, nromov, nroitem, "P", item["idproducto"].ToString(), item["descripcion"].ToString(), double.Parse(item["cantidad"].ToString()),
                            double.Parse(item["precio"].ToString()), double.Parse(item["subtotal"].ToString()), deb_hab, double.Parse(item["iva"].ToString()), double.Parse(item["ii"].ToString()), IDFACTURA.ToString());
                    }


                    foreach (DataRow item in asiento.Tables[0].Rows)
                    {
                        if ((item["tipo"].ToString() != "VENTA"))
                        {
                            if ((item["tipo"].ToString() != "IVA") || (item["tipo"].ToString() == "IVA" && iva_explicito))
                            {
                                insert_fe_calculofactura(item["tipo"].ToString(), item["descripcion"].ToString(), 0, 0,
                                    double.Parse(item["importe"].ToString()), "C", 0, 0, 0, 0,
                                    0, 0, "", tasaiibb, tasaiibb2, 0, int.Parse(jurisdiccioniibb.ToString()), int.Parse(jurisdiccioniibb2.ToString()), IDFACTURA);
                                nroitem += 1;
                                insertar_item(codmov, nromov, nroitem, "C", item["tipo"].ToString(), item["descripcion"].ToString(), 0, 0,
                                    double.Parse(item["importe"].ToString()), deb_hab, 0, 0, IDFACTURA.ToString());

                            }
                        }
                    }


                    //SUBIDAVT
                    nroitem_subdia = 0;
                    foreach (DataRow item in asiento.Tables[0].Rows)
                    {
                        nroitem_subdia += 1;
                        insertar_subdiavt(codmov, nromov, nroitem_subdia, item["concepto"].ToString(), item["debhab"].ToString(),
                            double.Parse(item["importe"].ToString()), IDFACTURA.ToString());

                    }


                    total_factura = total_productos + fac_percepcioniibb + fac_percepcioniibb2;
                    if (iva_explicito)
                    {
                        total_factura += total_iva;
                    }

                    if (es_factura)
                    {
                        deb_hab = "D";
                    }
                    else
                    {
                        deb_hab = "H";
                    }

                    //total factura 
                    insert_fe_calculofactura("TF", "TOTAL FACTURA", 0, 0, total_factura, "C", 0, 0, 0, 0, 0, 0, "", 0, 0, 0, int.Parse(jurisdiccioniibb.ToString()), int.Parse(jurisdiccioniibb2.ToString()), IDFACTURA);
                    //total factura en facturasitem
                    nroitem += 1;
                    insertar_item(codmov, nromov, nroitem, "C", "TF", "Total Comprobante", 0, 0, total_factura, deb_hab, 0, 0, IDFACTURA.ToString());
                    //total en subdiavt
                    nroitem_subdia += 1;
                    insertar_subdiavt(codmov, nromov, nroitem_subdia, "T01", deb_hab, total_factura, IDFACTURA.ToString());

                    //if (cuentacorriente)
                    //{
                    //    insertar_cuenta_corriente(codmov, nromov, comprobante, fecha, nrocta, 0, codmov, nromov.ToString(), total_factura, 1, IDFACTURA.ToString());
                    //}

                    //setea como facturado el pedido
                    set_status_comprobante(ant_codmov, ant_nromov, codmov, nromov.ToString(), sucursal, comprobante, IDFACTURA.ToString());

                    //if (cobrado != 0 && Math.Round(total_factura, 2) == cobrado * -1)
                    //{
                    //    string resultado_recibo = Generar_recibo(nrocta, nrocta, "RC", nro_recibo, codmov, nromov, cobrado, oItems.Tables[0].Rows[0], idpedido.ToString(), cuentacorriente);

                    //    if (resultado_recibo != "")
                    //    {
                    //        recibo = resultado_recibo.Split(';');
                    //        recibo_codmov = recibo[0];
                    //        recibo_nromov = recibo[1];
                    //    }
                    //    else
                    //    {
                    //        //no genero recibo
                    //    }
                    //}

                    pedido_facturado += codmov + ';' + nromov.ToString();


                }
            }
            catch (Exception ex)
            {
                //eliminar comprobantes
                //pedido_facturado = "ERROR;idpedido" + idpedido.ToString() + ";" + ex.Message;
                Rollback("0", codmov, nromov.ToString(), "", "");
            }
            return pedido_facturado;
        }

        public Boolean Rollback(string idpedido, string codmov_fac, string nromov_fac, string codmov_rec, string nromov_rec)
        {
            Boolean exito = false;
            try
            {
                if (codmov_fac != "")
                {
                    Rollback_Comprobante(codmov_fac, nromov_fac);
                }
                if (codmov_rec != "")
                {

                    Rollback_RECIBO(idpedido, codmov_rec, nromov_rec);
                }

            }
            catch (Exception) { }
            return exito;
        }

        #endregion


    }
}
