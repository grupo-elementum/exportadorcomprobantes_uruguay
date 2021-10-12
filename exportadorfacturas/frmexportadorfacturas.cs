using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using CsNegocio;
using CsDatos;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Net.Mail;
using System.Web;
using System.Net;
using System.Xml;



namespace exportadorfacturas
{
    public partial class frmexportadorfacturas : Form
    {
        #region Variables Globales
        int chequeos = 0; int cantLlamadas = 0; int cantRecargas = 0; int cantClientes = 0, cantEjecucionesManuales =0;
        string bdCliente = ConfigurationSettings.AppSettings["BdWeb"];
        string ipdebug = ConfigurationSettings.AppSettings["IPDEBUG"];
        string debugueando = ConfigurationSettings.AppSettings["DEBUGUEANDO"];
        string resul = ""; DateTime FechaAct; bool config = false; bool CerFor = false;
        //string Mensaje = "";


        wsfe.ServicioFacturaElectronicaWebServiceService oWsfe;
        WsAirtech.Service1SoapClient oWs;

        string bdweb, datasource, entrada, usuario, pass, operativa;

        Configuracion oConfFE = new Configuracion("FacturaElectronica");
        Configuracion oConCompOnline = new Configuracion("CompOnline");

        #endregion

        #region Inicializacion
        public frmexportadorfacturas()
        {
            InitializeComponent();
        }

        private void inicializar_servicios()
        {
            try
            {
                oWs = new WsAirtech.Service1SoapClient();
                oWsfe = new wsfe.ServicioFacturaElectronicaWebServiceService();
            }
            catch (Exception ex)
            {
            }

        }

        private void frmexportadorfacturas_Load(object sender, EventArgs e)
        {

            
            inicializar_servicios();
           // string resultadowss = oWsfe.consultarMontoObligadoRecepcion("30710681771", "41");

            Clientes oCli = new Clientes();
            DataSet oDs = new DataSet();
            Mensajes oMen = new Mensajes();
            Equipos oEq = new Equipos();            
            Size tempSize = panel1.Size;
            string iniweb;

            tempSize.Height = 0;
            tempSize.Width = 0;
            panel1.Size = tempSize;
            FechaAct = DateTime.Now;

            string[] datosini;
            iniweb = oWs.getConfiguracionIni(bdCliente);
            if (iniweb != "")
            {
                datosini = iniweb.Split(';');
                bdweb = datosini[1];
                if (debugueando == "1")
                {
                    datasource = ipdebug;
                }
                else
                {
                    datasource = datosini[8].ToString().Split('=')[1];
                }                
                entrada = datosini[9].ToString().Split('=')[1];
                usuario = datosini[10].ToString().Split('=')[1];
                pass = datosini[11].ToString().Split('=')[1];
                operativa = datosini[16];
                if (operativa == "")
                {
                    string[] linStr;
                    foreach (string linea in ConfigurationSettings.AppSettings["ConnectionString"].Split(';'))
                    {
                        linStr = linea.Split('=');
                        if (linStr[0].ToString() == "Initial Catalog")
                        {
                            operativa = linStr[1].ToString().Trim();
                            break;
                        }
                    }
                    string result = "";
                    result = oWs.setConfiguracionIni(bdweb,datosini[2],datosini[3],datosini[4],datosini[5],datosini[6],datosini[7],
                        datosini[8],datosini[9],datosini[10],datosini[11],datosini[12],datosini[13],datosini[14],datosini[15],operativa);
                }
            }
            Conexion oConn = Conexion.Instance;

            oConn.Bdweb = bdweb;
            oConn.Bdentrada = entrada;
            oConn.Datasource = datasource;
            oConn.Usuariosql = usuario;
            oConn.Passsql = pass;
            oConn.Bdoperativa = operativa;
            

            string stringconn = oConn.get_connectionstring();
            string stringconnent = oConn.get_connectionstring_entrada();



            if (!oConfFE.Abierto())
            {
                oConfFE.InicializarObjeto();
                oConCompOnline.InicializarObjeto();
            }
            else
            {
                timer1.Enabled = false;
                if (MessageBox.Show("Error, No se puede ejecutar el Exportador, este se esta ejecutandose en otra PC, desea continuar?","Error",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    oConfFE.InicializarObjeto();
                    oConCompOnline.InicializarObjeto();
                    oConfFE.NoCorrio();
                    timer1.Enabled = false;
                    timer2.Enabled = true;
                }
                else
                {
                    oConfFE.InicializarObjeto();
                    CerFor = true;
                    Application.Exit();
                }
            }
        }

        private void frmexportadorfacturas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CerFor == false)
            {
                oConfFE.Desactivar();
                oConCompOnline.Desactivar();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Interval = 65535;
            lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Esperando respuesta...";
            lblMensajes.Refresh();
            chequeos = chequeos + 1;
            if (chequeos < 3)
            {
                if (oConfFE.Corrio())
                {
                    CerFor = true;
                    MessageBox.Show("El Exportador de Facturas se encuentra operativo en otra PC", "Error");
                    Application.Exit();
                }
            }
            else
            {
                lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Inicializando Componentes de Exportador...";
                lblMensajes.Refresh();
                chequeos = 0;
                timer2.Enabled = false;
                timer1.Enabled = true;
            }
        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            timer1.Interval = 65535;
            try
            {
                generar_comprobantes();

                enviar_comprobantes_electronicos();
            }
            catch (Exception ex)
            { 
            }
        }

        private void enviar_comprobantes_electronicos()
        {
            //Ejecuta la tarea programada de factura electronica
            if (oConfFE.Activo())
            {
                TimeSpan ts = (DateTime.Now) - (oConfFE.UltimaEjecucion());
                if (ts.Minutes >= Convert.ToInt32(oConfFE.Tarea4) || ts.Days > 0)
                {
                    oConfFE.SiCorrio();
                    //consulta por parametro para exportar a archivo o enviar directamente
                    //si el parametro es 1 exporta a archivo sino envia directamente
                    Uruguay_ExportarFacturas();

                    //Uruguay_ExportarRemitos();

                    Uruguay_ActualizarComprobantes();

                    Uruguay_EliminarComprobantes();
                }
            }
        }

        private void generar_comprobantes()
        {
            //Ejecuta la tarea programada de comprobantes online
            if (oConCompOnline.Activo())
            {
                TimeSpan ts = (DateTime.Now) - (oConCompOnline.UltimaEjecucion());
                if (oConCompOnline.Tarea4 != "")
                {
                    if (ts.Minutes >= Convert.ToInt32(oConCompOnline.Tarea4) || ts.Days > 0)
                    {
                        oConCompOnline.SiCorrio();
                        Proceso_generacion_comprobantes_electronicos();

                    }
                }
            }
        }


        #region Configuracion
        private void Expandir()
        {
            Size tempSize = panel1.Size;

            tempSize.Height = 0;
            tempSize.Width = 0;
            while (panel1.Size.Height <= 98 || panel1.Size.Width <= tabControl1.Size.Width + 25)
            {
                tempSize.Width += 1;
                if (panel1.Size.Height <= 98)
                {
                    tempSize.Height += 1;    
                }
                panel1.Size = tempSize;
            }
        }
        private void Reducir()
        {
            Size tempSize = panel1.Size;

            while (panel1.Size.Height > 0 || panel1.Size.Width > 0)
            {
                tempSize.Width -= 1;
                if (panel1.Size.Height > 0)
                {
                    tempSize.Height -= 1;    
                }
                panel1.Size = tempSize;
            }
        }
        private void inicializarConfig()
        {
            try
            {
                chkFacturaElectronica.Checked = false;
                chkExportarTxt.Checked = false;
                if (oConfFE.Activo())
                {
                    chkFacturaElectronica.Checked = true;                    
                    if (oConfFE.Tarea2 == "1")
                    {
                        chkExportarTxt.Checked = true;
                    }
                    if (oConfFE.Tarea4 != "")
                    {
                        txtMin.Text = oConfFE.Tarea4.Trim();
                    }
                }

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "InicializarConfig", "Principal", ex);
                lblError.Text = DateTime.Now.ToShortTimeString() + " Error Inicializando Configuracion" + ex.Message.ToString();
                lblError.Refresh();
            }
        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            //string prog;

            try
            {
                if (!config)
                {
                    timer1.Stop();
                    inicializarConfig();
                    Expandir();
                    config = true;
                    timer3.Start();
                }
                else
                {
                    timer3.Stop();
                    Reducir();
                    config = false;

                    oConfFE.Tomado = 0;
                    oConfFE.Tarea2 = "0";
                    oConfFE.Tarea3 = "";
                    oConfFE.Tarea4 = "";
                    if (chkFacturaElectronica.Checked == true)
                    {
                        oConfFE.Programacion = DateTime.Now;
                        oConfFE.Tarea2 = "0";
                        oConfFE.Tarea3 = "";
                        oConfFE.Tarea4 = "5";
                        oConfFE.Tomado = 1;
                        if (chkExportarTxt.Checked == true)
                        {
                            oConfFE.Tarea2 = "1";
                        }
                        if (txtMin.Text != "")
                        {
                            oConfFE.Tarea4 = txtMin.Text;
                        }
                    }

                    oConfFE.Guardar();
                    timer1.Start();
                }
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "btnConfig", "Principal", ex);
                lblError.Text = DateTime.Now.ToShortTimeString() + " Error Grabando Configuracion" + ex.Message.ToString();
                lblError.Refresh();
                timer1.Start();
            }
        }
        private void escribirFichero(string texto)
        {
            //obtenemos la carpeta y ejecutable de nuestra aplicación 
            //string rutaFichero = Environment.GetCommandLineArgs()[0];
            //obtenemos sólo la carpeta (quitamos el ejecutable) 
            string carpeta = "C:/H2O/BACKUP/";
            //Montamos la carpeta y el fichero temporal con el 
            //primer parámetro que es el código de solicitud 
            //rutaFichero = Path.Combine(carpeta, "factura_" +
            //    Environment.GetCommandLineArgs()[1] + ".inc");
            try
            {
                //si no existe la carpeta temporal la creamos 
                if (!(Directory.Exists(carpeta)))
                {
                    Directory.CreateDirectory(carpeta);
                }
            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "EscribirFichero", "Principal", ex);
                lblError.Text = DateTime.Now.ToShortTimeString() + " Ha habido un error al intentar crear la carpeta backup";
                lblError.Refresh();
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            btnConfig_Click(sender, e);
        }
        #endregion

        #region FacturaElectronica

        private string ObjectToXml<T>(T objectToSerialise)
        {
            StringWriter Output = new StringWriter(new StringBuilder());
            XmlSerializer xs = new XmlSerializer(objectToSerialise.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("fac", "http://www.facturante.com.API");
            xs.Serialize(Output, objectToSerialise, ns);
            return Output.ToString();
        }

        private void ExportarFacturasFE ()
        {
            DataSet oEmp = new DataSet();
            DataSet oDs = new DataSet();
            DataSet oTxt = new DataSet();
            
            DataSet oEncabezado = new DataSet ();
            DataSet oTributos = new DataSet();
            DataSet oIva = new DataSet();
            DataSet oDetalle = new DataSet();
            DataSet oPie = new DataSet();
            DataSet oNotas = new DataSet();

            String[] datos;
            List<String> errCodmovList = new List<String>();
            object cae;
            String facturaID, errores, ResulErrores,EmpresaID = "";
            int ErrorCorrelatividad = 0, cantcomprobantes = 0, cantcomprobantesok = 0, cantcomprobanteserror = 0;
            String errorMail = "",tipo = "",codmov = "",nromov = "",origen_error = "";
            Int32 error;

 
            oEmp = oConfFE.ObtenerEmpresasParametrosFE();
            try
            {
                foreach (DataRow emp in oEmp.Tables[0].Rows)
                {
                        EmpresaID = emp["clienteid"].ToString();
                        oDs = oConfFE.ObtenerFacturasExportar(long.Parse(emp["id"].ToString()));
                        foreach (DataRow row in oDs.Tables[0].Rows)
                        {
                            ResulErrores = "";
                            facturaID = "0";

                            //actualizar visual con cantidades
                            FechaAct = DateTime.Now;
                            cantcomprobantes = oDs.Tables[0].Rows.Count -1  ; 
                            resul = "";

                            codmov = row["codmov"].ToString();
                            nromov = row["nromov"].ToString();

                            lblCantLlamadas.Text = cantcomprobantes.ToString();
                            lblCantRecargas.Text = cantcomprobantesok.ToString();
                            lblCantClientes.Text = cantcomprobanteserror.ToString();

                            lblCantLlamadas.Refresh();
                            lblCantRecargas.Refresh();
                            lblCantClientes.Refresh();

                            foreach (string item in errCodmovList)
                            {
                                if (item == row["codmov"].ToString())
                                {
                                    ErrorCorrelatividad = 1;
                                    break;
                                }
                            }
                            if (ErrorCorrelatividad == 1)
                            {
                                ErrorCorrelatividad = 0;
                                //break;
                            }
                            else
                            {
                                    origen_error = "0";
                                    tipo = " Error obteniendo CR comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                    oEncabezado = oConfFE.getResultadoFE("CR", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));
                                    
                                    tipo = " Error obteniendo TR comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                    oTributos = oConfFE.getResultadoFE("TR", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                    tipo = " Error obteniendo IV comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                    oIva = oConfFE.getResultadoFE("IV", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                    tipo = " Error obteniendo DE comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                    oDetalle = oConfFE.getResultadoFE("DE", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                    tipo = " Error obteniendo PI comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                    oPie = oConfFE.getResultadoFE("PI", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                    tipo = " Error obteniendo OB comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                    oNotas = oConfFE.getResultadoFE("OB", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                //Pregunta si exporta a archivo (1) o envia directamente (0)
                                if (oConfFE.Tarea2 == "0")
                                {
                                    origen_error = "1";
                                    if (oEncabezado.IsInitialized  )
                                    {
                                        
                                        tipo = "Error insertando WS Encabezado" + " comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                        
                                        facturaID = oWsfe.insertaFacturaEncabezadoNuevo(oEncabezado.Tables[0].Rows[0]["asociado_comprobante"].ToString()//asociado comprobante
                                                            , oEncabezado.Tables[0].Rows[0]["letra"].ToString()           // letra
                                                            , oEncabezado.Tables[0].Rows[0]["cuit"].ToString()            // cuit
                                                            , oEncabezado.Tables[0].Rows[0]["cbu"].ToString()            // cbu
                                                            , EmpresaID    // empresaid
                                                            , oEncabezado.Tables[0].Rows[0]["asociado_cuit"].ToString()//asociado cuit
                                                            , oEncabezado.Tables[0].Rows[0]["asociado_nro"].ToString()//asociado nro
                                                            , oEncabezado.Tables[0].Rows[0]["asociado_fechacomp"].ToString()//asociado fecha
                                                            , oEncabezado.Tables[0].Rows[0]["direcc"].ToString()        // direccion
                                                            , oEncabezado.Tables[0].Rows[0]["fchmov"].ToString()           // fechafactura
                                                            , oEncabezado.Tables[0].Rows[0]["sucursal"].ToString()           // puntoventa
                                                            , oEncabezado.Tables[0].Rows[0]["nrocta"].ToString()            // cienteid
                                                            , oEncabezado.Tables[0].Rows[0]["nombre"].ToString()           // nombre
                                                            , oEncabezado.Tables[0].Rows[0]["telefono"].ToString()           // telefono
                                                            , oEncabezado.Tables[0].Rows[0]["mail"].ToString()           // email
                                                            , oEncabezado.Tables[0].Rows[0]["frecuencia"].ToString()           // frecuencia
                                                            , oEncabezado.Tables[0].Rows[0]["codigobarra"].ToString()           // barcode
                                                            , oEncabezado.Tables[0].Rows[0]["tipo"].ToString()            // documentoid
                                                            , oEncabezado.Tables[0].Rows[0]["condicion_pago"].ToString()           // condicion_pago
                                                            , oEncabezado.Tables[0].Rows[0]["provincia"].ToString()           // provinciaid
                                                            , ""                            // url
                                                            , oEncabezado.Tables[0].Rows[0]["codalt"].ToString()            // responsableid
                                                            , oEncabezado.Tables[0].Rows[0]["bienes"].ToString()           // conceptoid
                                                            , oEncabezado.Tables[0].Rows[0]["reparto"].ToString()           // reparto
                                                            , oEncabezado.Tables[0].Rows[0]["nromov"].ToString()           // comprobantenro
                                                            , oEncabezado.Tables[0].Rows[0]["locali"].ToString()           // localidad
                                                            , oEncabezado.Tables[0].Rows[0]["comprobantefe"].ToString()           // comprobanteid
                                                            , oEncabezado.Tables[0].Rows[0]["asociado_puntoventa"].ToString()//asociado puntoventa
                                        );
                                                                                        

                                        if (Int32.TryParse(facturaID.Substring(0, 2), out error))
                                        {
                                            if (error == -1)
                                            {
                                                ResulErrores += facturaID;
                                            }
                                        }
                                        
                                        if (oTributos.IsInitialized)
                                        {
                                            tipo = "Error insertando WS Tributo" + " comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                            foreach (DataRow oTrib in oTributos.Tables[0].Rows)
                                            {

                                                errores = oWsfe.insertaFacturaTributo(oTrib["tipotributo"].ToString(), //tributoid
                                                                                    oTrib["porcentaje"].ToString(), //alicuota
                                                                                    oTrib["neto"].ToString(), //baseimponible
                                                                                    oTrib["tributodescripcion"].ToString(),//descripcion
                                                                                    oTrib["tributoimporte"].ToString(),//importe
                                                                                    facturaID, EmpresaID);
                                                if (Int32.TryParse(errores.Substring(0, 2), out error))
                                                {
                                                    if (error == -1)
                                                    {
                                                        ResulErrores += errores;
                                                    }
                                                }
                                            }
                                        }
                                        if (oIva.IsInitialized)
                                        {
                                            tipo = "Error insertando WS Iva" + " comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                            foreach (DataRow item in oIva.Tables[0].Rows)
                                            {
                                                if (item["importeiva"].ToString().Length > 0)
                                                {
                                                    errores = oWsfe.insertaFacturaIva(item["tasaiva"].ToString(), //ivaid
                                                                                        item["neto"].ToString(),//base imponible
                                                                                        item["importeiva"].ToString(), //importe
                                                                                        facturaID, EmpresaID);

                                                    if (Int32.TryParse(errores.Substring(0, 2), out error))
                                                    {
                                                        if (error == -1)
                                                        {
                                                            ResulErrores += errores;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if(oDetalle.IsInitialized)
                                        {
                                            tipo = "Error insertando WS Detalle" + " comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                            foreach (DataRow  item in oDetalle.Tables[0].Rows)
	                                        {

                                                errores = oWsfe.insertaFacturaDetalle(item["total"].ToString(),//subtotal 
                                                item["cantidad"].ToString(),//cantidad
                                                item["tipoiva"].ToString(),//ivaid
                                                item["iva"].ToString(),//importeiva
                                                item["precio"].ToString(),//preciounitario
                                                item["descripcion"].ToString(),//descripcion
                                                "",//importebonificacion
                                                item["tipoiva"].ToString(),//alicuotaiva
                                                item["neto"].ToString(),//subtotal
                                                emp["clienteid"].ToString(),
                                                item["unidad"].ToString(),//unidadmedida
                                                item["gln"].ToString(),//gln
                                                "",//importebonificacion
                                                item["idproducto"].ToString(),//codigo
                                                facturaID);
                                                if (Int32.TryParse(errores.Substring(0, 2), out error))
                                                {
                                                    if (error == -1)
                                                    {
                                                        ResulErrores += errores;
                                                    }
                                                }

	         
	                                        }
                                        }
                                        if (oPie.IsInitialized)
                                        {
                                            tipo = "Error insertando WS Pie" + " comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                            errores = oWsfe.insertaFacturaPie(
                                                oPie.Tables[0].Rows[0]["tributo"].ToString(),//importe tributo
                                                "",//importepercecionesmunicipales
                                                oPie.Tables[0].Rows[0]["fecha_desde"].ToString(),//fechadesde
                                                oPie.Tables[0].Rows[0]["fecha_vencimiento"].ToString(),//fechavencimiento
                                                "1",//tipocambio
                                                oPie.Tables[0].Rows[0]["total"].ToString(),//importetotal
                                                oPie.Tables[0].Rows[0]["nogravado"].ToString(),//importenetogravado
                                                "",//importeimpuestosinternos
                                                oPie.Tables[0].Rows[0]["moneda"].ToString(),//codigomonedaid
                                                "",//importeiibb
                                                oPie.Tables[0].Rows[0]["gravado"].ToString(),//importegravado
                                                oPie.Tables[0].Rows[0]["iva"].ToString(),//importeiva
                                                "0",//importeoperacionesexcentas
                                                oPie.Tables[0].Rows[0]["fecha_hasta"].ToString(),//fechahasta 
                                                "",//importebonificaciones
                                                "",//formapagoid
                                                "",//importeopreracionesnacionales
                                                facturaID, EmpresaID);
                                            if (Int32.TryParse(errores.Substring(0, 2), out error))
                                            {
                                                if (error == -1)
                                                {
                                                    ResulErrores += errores;
                                                }
                                            }
                                        }
                                        if (oNotas.IsInitialized)
                                        {
                                            tipo = "Error insertando WS Notas" + " comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                            foreach (DataRow item in oNotas.Tables[0].Rows)
                                            {
                                                errores = oWsfe.insertaFacturaNota(
                                                    item["observ"].ToString(),//descripcion
                                                    item["nritem"].ToString(),//orden 
                                                    facturaID, EmpresaID);
                                                if (Int32.TryParse(errores.Substring(0, 2), out error))
                                                {
                                                    if (error == -1)
                                                    {
                                                        ResulErrores += errores;
                                                    }
                                                }
                                            }
                                        }
                                        

                                        if (ResulErrores != "")
                                        {
                                            errCodmovList.Add(row["codmov"].ToString());
                                            errorMail += Environment.NewLine + "Factura: " + row["codmov"].ToString() + "-" + row["nromov"].ToString() +
                                                Environment.NewLine + "-------------------------------" + Environment.NewLine +
                                                ResulErrores +
                                                Environment.NewLine + "-------------------------------" + Environment.NewLine + Environment.NewLine;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (ResulErrores == "" && facturaID != "0")
                            {
                                cae = oWsfe.solicitarCae(facturaID, EmpresaID);
                                if (((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).cae[0] == "-1")
                                {
                                    //Insertar Error
                                    errCodmovList.Add(row["codmov"].ToString());
                                    oConfFE.InsertarFacturaError(row["codmov"].ToString(), long.Parse(row["nromov"].ToString()), ((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).obs[0], ((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).error[0]);
                                    errorMail += Environment.NewLine + "Factura: " + row["codmov"].ToString() + "-" + row["nromov"].ToString() +
                                        Environment.NewLine + "-------------------------------" + Environment.NewLine +
                                        ((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).obs[0] + ((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).error[0] +
                                        Environment.NewLine + "-------------------------------" + Environment.NewLine + Environment.NewLine;

                                    cantcomprobanteserror += 1;
                                }
                                else
                                {
                                    //Insertar Cae
                                    oConfFE.InsertarFacturaCae(row["codmov"].ToString(), long.Parse(row["nromov"].ToString()), long.Parse(((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).cae[0]), ((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).fecha[0], ((exportadorfacturas.wsfe.ArrayOfsolicitarCaeResponsecaeresul)cae).vencimiento[0]);
                                    cantcomprobantesok += 1;
                                }
                                //Insertar cae o error en bd operativo 
                            }
                        }
                        if (errorMail != "")
                        {
                            oWsfe.enviarEmailError(errorMail, EmpresaID);
                            errorMail = "";
                        }
                }
            }

            catch (System.Net.WebException exx)
            {
                lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Exportando FE Servicio inaccesible";
                lblMensajes.Refresh();
            }
            catch (Exception exc)
            {

                lblError.Text = DateTime.Now.ToShortTimeString() + tipo;
                lblError.Refresh();
                string result = oConfFE.InsertarExportadorError (codmov, nromov,tipo, origen_error,0);
                cantcomprobanteserror += 1;

                if (oConfFE.Tarea3 == "MAIL_ERROR" && result.ToUpper() == "NOTIFICAR")
                {
                    SendMail("soporte@airtech-it.com.ar", tipo +" "+ exc.Message  , "Error Proceso CAE " + bdCliente, bdCliente);
                    oConfFE.NotificoExportadorError(codmov, nromov, origen_error); 
                }
            }
            if (oConfFE.Tarea2 == "1")
            {
                string bdEnt = "C:/H2O";
                string[] linStr;

                foreach (string linea in ConfigurationSettings.AppSettings["ConnectionString"].Split(';'))
                {
                    linStr = linea.Split('=');
                    if (linStr[0].ToString() == "Initial Catalog")
                    {
                        bdEnt = linStr[1].ToString().Trim();
                        break;
                    }
                }
                if (oDs.Tables[0].Rows.Count > 0)
                {
                    oConfFE.EscribirTxt(bdEnt);
                }
            }         
        }
        private void ImportarCaeFacturas()
        {
            oConfFE.ProcesarArchivosCae();
            oConfFE.ProcesarCaeFacturas();
        }
        private void ImportarErrorFacturas()
        {
            oConfFE.ProcesarArchivosError();
            oConfFE.ProcesarErrorFacturas();
        }
        private void btnEjecutar_Click(object sender, EventArgs e)
        {

            cantEjecucionesManuales += 1;
            btnEjecutar.Enabled = false;
            log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Exportar FE", "Principal", new Exception("Ejecucion manualNro " + cantEjecucionesManuales.ToString()), "0");            
            
            if (oConfFE.Tarea2 == "1")
            {
                lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Importando Cae Facturas..";
                lblMensajes.Refresh();
                ImportarCaeFacturas();

                lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Importando Errores..";
                lblMensajes.Refresh();
                ImportarErrorFacturas();                
            }

            lblMensajes.Text = DateTime.Now.ToShortTimeString() + "(Ejecucion Manual Nro "+cantEjecucionesManuales.ToString()+ ") Exportando Facturas..";
            lblMensajes.Refresh();
            inicializar_servicios();
            ExportarFacturasFE();
            btnEjecutar.Enabled = true;
        }
        private void SendMail(string MailTo, string Mensaje, string asunto, string cliente)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress("sguidi@airtech-it.com.ar");

                // The important part -- configuring the SMTP client
                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;   // [1] You can try with 465 also, I always used 587 and got success
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
                smtp.UseDefaultCredentials = false; // [3] Changed this
                System.Security.SecureString pass;
                pass = new System.Security.SecureString();

                pass.AppendChar('s');
                pass.AppendChar('e');
                pass.AppendChar('r');
                pass.AppendChar('g');
                pass.AppendChar('i');
                pass.AppendChar('o');
                pass.AppendChar('j');
                pass.AppendChar('g');
                pass.AppendChar('u');
                pass.AppendChar('i');
                pass.AppendChar('d');
                pass.AppendChar('i');
                pass.AppendChar('6');
                pass.AppendChar('6');

                //smtp.Credentials = new NetworkCredential("sguidi@airtech-it.com.ar", "sergiojguidi66");  // [4] Added this. Note, first parameter is NOT string.                smtp.Host = "smtp.gmail.com";

                //recipient address
                mail.To.Add(new MailAddress(MailTo));

                //Formatted mail body
                mail.IsBodyHtml = true;
                mail.Subject = asunto;
                mail.Body = Mensaje;
                smtp.Send(mail);

            }
            catch (Exception exx)
            {
                Console.WriteLine(exx.Message);
            }
        }

        private void Proceso_generacion_comprobantes_electronicos()
        {
            DataSet oDs = new DataSet();
            DataSet oPedidos = new DataSet();
            Facturacion oFac = new Facturacion();
            string idpedido_ant = "", resultado = "", comp_generado = "";
            Boolean encabezado = true;
            double cant = 0;
            string comprobante = "";


            try
            {

                oDs = oFac.Listar_comprobantes_a_generar();
                cant = 0;

                foreach (DataRow comp in oDs.Tables[0].Rows)
                {
                    try
                    {
                        comprobante = comp["codmov"].ToString() + " " + comp["nromov"].ToString();
                        if (comp["codmov"].ToString() == "DI" || comp["codmov"].ToString() == "CI")
                        {
                            comp_generado = oFac.generar_comprobante(comp["codmov"].ToString(), comp["nromov"].ToString());
                        }
                        else
                        {
                            oFac.generar_remito(comp["nromov"].ToString());

                        }
                        cant += 1;
                        lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Generando " + cant.ToString() + " de " + oDs.Tables[0].Rows.Count.ToString() + " comp electronicos ...";
                        lblMensajes.Refresh();
                    }
                    catch (Exception exx)
                    {
                        log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Error Generando Comprobantes elect - " + comprobante, "Principal", exx, "0");
                        lblError.Text = DateTime.Now.ToShortTimeString() + "Error  Generando Comprobantes elect - " + comprobante + " " + exx.Message.ToString();
                        lblError.Refresh();

                    }

                }

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Error Generando Comprobantes elect - " + comprobante, "Principal", ex, "0");
                lblError.Text = DateTime.Now.ToShortTimeString() + "Error Generando Comprobantes elect - " + comprobante + " " + ex.Message.ToString();
                lblError.Refresh();
            }
        }

        private void Proceso_facturaen_calle_electronicos()
        {
            DataSet oDs = new DataSet();
            DataSet oPedidos = new DataSet();
            Facturacion oFac = new Facturacion();
            string idpedido_ant = "", resultado = "", pedido_facturado = "";
            Boolean encabezado = true;
            double cant = 0;


            try
            {
                cant  = 0;
                oPedidos = oWs.FACTURACION_getpedidosafacturar(bdCliente);
                foreach (DataRow pedido in oPedidos.Tables[0].Rows)
                {
                    cant += 1;
                    lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Descargando pedidos " + cant.ToString() + " de " + oPedidos.Tables[0].Rows.Count.ToString();
                    lblMensajes.Refresh();

                    if (idpedido_ant != pedido["idpedido"].ToString())
                    {
                        idpedido_ant = pedido["idpedido"].ToString();
                        encabezado = true;
                        resultado = "";
                    }
                    else
                    {
                        encabezado = false;
                    }
                    if (resultado != "ya existe")
                    {
                        resultado = oFac.insert_pedido(pedido["idpedido"].ToString(), pedido["idvendedor"].ToString(), pedido["idcliente"].ToString(), pedido["idclientenuevo"].ToString(),
                            pedido["fecha_pedido"].ToString(), pedido["factura"].ToString(), pedido["idproducto"].ToString(), double.Parse(pedido["cantidad"].ToString()), double.Parse(pedido["precio"].ToString()),
                            pedido["tipo"].ToString(), encabezado, pedido["cobrado"].ToString(), pedido["nro_recibo"].ToString());
                    }
                }

                oDs = oFac.Listar_pedidos_a_facturar();
                cant = 0;

                foreach (DataRow pedido in oDs.Tables[0].Rows)
                {
                    pedido_facturado = oFac.Facturar_pedido(double.Parse(pedido["idpedido"].ToString()));
                    cant += 1;
                    lblMensajes.Text = DateTime.Now.ToShortTimeString() + " Generando " + cant.ToString() + " de " + oDs.Tables[0].Rows.Count.ToString() + " facturas en calle ...";
                    lblMensajes.Refresh();


                    if (pedido_facturado != "")
                    {
                        string[] result = pedido_facturado.Split(';');
                        if (result[0] != "ERROR")
                        {
                            try
                            {
                                oWs.FACTURACION_setpedidofacturado(bdCliente, result[0].ToString(), result[1].ToString(), result[2].ToString());
                            }
                            catch (Exception ex)
                            { 
                            }
                        }
                        else
                        {
                            if (oConfFE.Tarea3 == "MAIL_ERROR")
                            {
                                string mensaje = "";
                                mensaje = "Error generando comprobantes Online " + result[1] + "\r\n" + "\r\n";
                                mensaje = mensaje + "Error: " + result[2] ;

                                SendMail("guidi.matias@gmail.com", mensaje, "Error generando factura " + bdCliente, bdCliente);

                                lblError.Text = DateTime.Now.ToShortTimeString()+" " + result[1];
                                lblError.Refresh();


                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Factura electronica calle", "Principal", ex, "0");
                lblError.Text = DateTime.Now.ToShortTimeString() + "Factura electronica calle" + ex.Message.ToString();
                lblError.Refresh();
            }
        }

        #endregion

        #region ComprobantesElectronicosUruguay

        private void Migrate_EnviaFactura(string codmov, string nromov, string codigo_empresa, string clave_empresa, string clave_comunicacion, bool testing, DataSet oEncabezado, DataSet oDetalle, DataSet oPie, DataSet oReferencia)
        {
            string xml_comp = "";
            InvoicyManual cliente = new InvoicyManual();
            ComprobantesUruguay oComp = new ComprobantesUruguay ();
            Comprobantes oComprobante = new Comprobantes();

            xml_comp = oComp.FacturaEmission_get_xml(codmov, nromov, codigo_empresa, clave_empresa, clave_comunicacion, oEncabezado, oDetalle, oPie, oReferencia);

            cliente.UrlWs = cliente.get_url("EmissionCFE", testing);
            cliente.Soap = cliente.WriteSOAP(xml_comp, "WS_EmissionFactura.Execute", "Xmlrecepcao");
            cliente.ExecutaWSEnvioCFE();

            if (cliente.tiene_error == false)
            {
                oComprobante.Actualizar(oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString(), codmov, nromov, cliente.status, cliente.statusdesc, "0", cliente.errcode,cliente.errdesc );
            }
            else
            {
                oComprobante.Actualizar(oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString(), codmov, nromov, "-1", cliente.errdesc, "0", cliente.errcode, cliente.errdesc);
            }

        }

        private void Uruguay_ExportarFacturas()
        {
            DataSet oEmp = new DataSet();
            DataSet oDs = new DataSet();
            DataSet oTxt = new DataSet();

            DataSet oEncabezado = new DataSet();
            DataSet oTributos = new DataSet();
            DataSet oIva = new DataSet();
            DataSet oDetalle = new DataSet();
            DataSet oPie = new DataSet();
            DataSet oNotas = new DataSet();
            DataSet oReferencia = new DataSet();

            String[] datos;
            List<String> errCodmovList = new List<String>();
            object cae;
           
            String facturaID, errores, ResulErrores, EmpresaID = "",clave_empresa = "",clave_comunicacion = "";
            int ErrorCorrelatividad = 0, cantcomprobantes = 0, cantcomprobantesok = 0, cantcomprobanteserror = 0;
            String errorMail = "", tipo = "", codmov = "", nromov = "", origen_error = "";
            Int32 error;
            bool testing = true;

            oEmp = oConfFE.ObtenerEmpresasParametrosFE();
            try
            {
                foreach (DataRow emp in oEmp.Tables[0].Rows)
                {
                    EmpresaID = emp["clienteid"].ToString();
                    clave_empresa = emp["clave_empresa"].ToString();
                    clave_comunicacion = emp["clave_comunicacion"].ToString();

                    if (emp["ambiente_testing"].ToString()!= "1")
                    {
                        testing = false;
                    }

                    oDs = oConfFE.ObtenerFacturasExportar(long.Parse(emp["id"].ToString()));
                    foreach (DataRow row in oDs.Tables[0].Rows)
                    {
                        try
                        {
                            ResulErrores = "";
                            facturaID = "0";

                            //actualizar visual con cantidades
                            FechaAct = DateTime.Now;
                            cantcomprobantes = oDs.Tables[0].Rows.Count - 1;
                            resul = "";

                            codmov = row["codmov"].ToString();
                            nromov = row["nromov"].ToString();

                            lblCantLlamadas.Text = cantcomprobantes.ToString();
                            lblCantRecargas.Text = cantcomprobantesok.ToString();
                            lblCantClientes.Text = cantcomprobanteserror.ToString();

                            lblCantLlamadas.Refresh();
                            lblCantRecargas.Refresh();
                            lblCantClientes.Refresh();

                            foreach (string item in errCodmovList)
                            {
                                if (item == row["codmov"].ToString())
                                {
                                    ErrorCorrelatividad = 1;
                                    break;
                                }
                            }
                            if (ErrorCorrelatividad == 1)
                            {
                                ErrorCorrelatividad = 0;
                                //break;
                            }
                            else
                            {
                                origen_error = "0";
                                tipo = " Error obteniendo CR comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                oEncabezado = oConfFE.getResultadoFE("CR", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                tipo = " Error obteniendo DE comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                oDetalle = oConfFE.getResultadoFE("DE", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                tipo = " Error obteniendo PI comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                oPie = oConfFE.getResultadoFE("PI", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                                tipo = " Error obteniendo RE comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                                oReferencia = oConfFE.getResultadoFE("RE", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));


                                //Pregunta si exporta a archivo (1) o envia directamente (0)
                                if (oConfFE.Tarea2 == "0")
                                {
                                    origen_error = "1";
                                    if (oEncabezado.IsInitialized & oDetalle.IsInitialized)
                                    {
                                        Migrate_EnviaFactura(row["codmov"].ToString(), row["nromov"].ToString(),
                                            EmpresaID, clave_empresa, clave_comunicacion, testing, oEncabezado, oDetalle, oPie, oReferencia);
                                    }
                                }
                            }
                        }

                        catch (Exception exx)
                        {
                            log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Error Enviando Comprobantes -" + tipo + " ", "Principal", exx, "0");
                            lblError.Text = DateTime.Now.ToShortTimeString() + "Erro Enviando Comprobantes -" + tipo + " " + exx.Message.ToString();
                            lblError.Refresh();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Error Enviando Comprobantes -" + tipo + " ", "Principal", ex, "0");
                lblError.Text = DateTime.Now.ToShortTimeString() + "Error Enviando Comprobantes -"+tipo +" "+ ex.Message.ToString();
                lblError.Refresh();
            }
        }

        private void Uruguay_ExportarRemitos()
        {
            DataSet oEmp = new DataSet();
            DataSet oDs = new DataSet();
            DataSet oTxt = new DataSet();

            DataSet oEncabezado = new DataSet();
            DataSet oTributos = new DataSet();
            DataSet oIva = new DataSet();
            DataSet oDetalle = new DataSet();
            DataSet oPie = new DataSet();
            DataSet oNotas = new DataSet();
            DataSet oReferencia = new DataSet();

            String[] datos;
            List<String> errCodmovList = new List<String>();
            object cae;

            String facturaID, errores, ResulErrores, EmpresaID = "", clave_empresa = "", clave_comunicacion = "";
            int ErrorCorrelatividad = 0, cantcomprobantes = 0, cantcomprobantesok = 0, cantcomprobanteserror = 0;
            String errorMail = "", tipo = "", codmov = "", nromov = "", origen_error = "";
            Int32 error;
            bool testing = true;

            oEmp = oConfFE.ObtenerEmpresasParametrosFE();
            try
            {
                foreach (DataRow emp in oEmp.Tables[0].Rows)
                {
                    EmpresaID = emp["clienteid"].ToString();
                    clave_empresa = emp["clave_empresa"].ToString();
                    clave_comunicacion = emp["clave_comunicacion"].ToString();

                    if (emp["ambiente_testing"].ToString()!= "0")
                    {
                        testing = false;
                    }



                    oDs = oConfFE.ObtenerRemitosExportar(long.Parse(emp["id"].ToString()));
                    foreach (DataRow row in oDs.Tables[0].Rows)
                    {
                        ResulErrores = "";
                        facturaID = "0";

                        //actualizar visual con cantidades
                        FechaAct = DateTime.Now;
                        cantcomprobantes = oDs.Tables[0].Rows.Count - 1;
                        resul = "";

                        codmov = row["codmov"].ToString();
                        nromov = row["nromov"].ToString();

                        lblCantLlamadas.Text = cantcomprobantes.ToString();
                        lblCantRecargas.Text = cantcomprobantesok.ToString();
                        lblCantClientes.Text = cantcomprobanteserror.ToString();

                        lblCantLlamadas.Refresh();
                        lblCantRecargas.Refresh();
                        lblCantClientes.Refresh();

                        foreach (string item in errCodmovList)
                        {
                            if (item == row["codmov"].ToString())
                            {
                                ErrorCorrelatividad = 1;
                                break;
                            }
                        }
                        if (ErrorCorrelatividad == 1)
                        {
                            ErrorCorrelatividad = 0;
                            //break;
                        }
                        else
                        {
                            origen_error = "0";
                            tipo = " Error obteniendo CR comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                            oEncabezado = oConfFE.getResultadoFE_Remito("CR", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                            tipo = " Error obteniendo DE comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                            oDetalle = oConfFE.getResultadoFE_Remito("DE", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                            tipo = " Error obteniendo PI comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                            oPie = oConfFE.getResultadoFE_Remito("PI", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                            tipo = " Error obteniendo RE comprobante " + row["codmov"].ToString() + " " + row["nromov"].ToString();
                            oReferencia = oConfFE.getResultadoFE_Remito("RE", long.Parse(emp["id"].ToString()), row["codmov"].ToString(), long.Parse(row["nromov"].ToString()));

                            //Pregunta si exporta a archivo (1) o envia directamente (0)
                            if (oConfFE.Tarea2 == "0")
                            {
                                origen_error = "1";
                                if (oEncabezado.IsInitialized & oDetalle.IsInitialized)
                                {
                                    Migrate_EnviaRemito(row["codmov"].ToString(), row["nromov"].ToString(),
                                        EmpresaID, clave_empresa, clave_comunicacion, testing, oEncabezado, oDetalle, oPie, oReferencia);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }       

        private void Migrate_EnviaRemito(string codmov, string nromov, string codigo_empresa, string clave_empresa, string clave_comunicacion, bool testing, DataSet oEncabezado, DataSet oDetalle, DataSet oPie, DataSet oReferencia)
        {
            string xml_comp = "";
            InvoicyManual cliente = new InvoicyManual();
            ComprobantesUruguay oComp = new ComprobantesUruguay();
            Comprobantes oComprobante = new Comprobantes();

            xml_comp = oComp.RemitoEmission_get_xml (codmov, nromov, codigo_empresa, clave_empresa, clave_comunicacion, oEncabezado, oDetalle, oPie, oReferencia);

            cliente.UrlWs = cliente.get_url("EmissionCFE", testing);
            cliente.Soap = cliente.WriteSOAP(xml_comp, "WS_EmissionFactura.Execute", "Xmlrecepcao");
            cliente.ExecutaWSEnvioCFE();

            if (cliente.tiene_error == false)
            {
                oComprobante.Actualizar(oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString(), codmov, nromov, cliente.status, cliente.statusdesc, "0", cliente.errcode,cliente.errdesc );
            }
            else
            {
                oComprobante.Actualizar(oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString(), codmov, nromov, "-1", cliente.errdesc, "0", cliente.errcode, cliente.errdesc);
            }


        }


        private void Migrate_ConsultarCFE(string codmov,string tipo_comprobante, string fecha_desde,string fecha_hasta,
                                            string nromov_desde,string nromov_hasta, 
                                            string codigo_empresa, string clave_empresa, 
                                            string clave_comunicacion, bool testing)
        {
            string xml_comp = "";
            string status_consulta = "";
            string comp_status = "",comp_status_desc = "", nromov = "",CfeTipo = "", error = "", error_desc ="";
            InvoicyManual cliente = new InvoicyManual();
            ComprobantesUruguay oComp = new ComprobantesUruguay();
            Comprobantes oComprobante = new Comprobantes();

            xml_comp = oComp.ConsultaCFE_get_xml(tipo_comprobante,fecha_desde,fecha_hasta,nromov_desde,nromov_hasta, codigo_empresa, clave_empresa, clave_comunicacion);

            cliente.UrlWs = cliente.get_url("ConsultaCFE", testing);
            cliente.Soap = cliente.WriteSOAP(xml_comp, "WS_ConsultaFactura.Execute", "Xmlconsulta");
            cliente.ExecutaWS();


            status_consulta = cliente.xmlresultado.DocumentElement.SelectSingleNode("/ConsultaCFERetorno/Encabezado/MsgCod").InnerText;
            if (status_consulta == "100")
            {
                XmlNodeList nodes = cliente.xmlresultado.DocumentElement.SelectNodes("/ConsultaCFERetorno/ListaCFE/CFE");

                foreach (XmlNode node in nodes)
                {
                    //1 - No procesado
                    //2 - Firmado
                    //3 - Rechazado   
                    //4 – Enviado DGI
                    //5 - Autorizado
                    //6 - Anulado
                    //9 - En Digitacíon
                    nromov = node.SelectSingleNode("CFENro").InnerText;
                    CfeTipo  = node.SelectSingleNode("CFETipo").InnerText;
                    comp_status = node.SelectSingleNode("CFEStatus").InnerText;
                    comp_status_desc = node.SelectSingleNode("CFEMsgDsc").InnerText;
                    if (node.SelectSingleNode("Erros/ErrosItem/CFEErrCod") != null)
                    {
                        error = node.SelectSingleNode("Erros/ErrosItem/CFEErrCod").InnerText;
                        error_desc = node.SelectSingleNode("Erros/ErrosItem/CFEErrDesc").InnerText;
                    }
                    else 
                    {
                        error = "-1";
                        error_desc = "";

                    }

                    if (cliente.tiene_error == false)
                    {
                        oComprobante.Actualizar(CfeTipo,codmov, nromov, comp_status, comp_status_desc, "0",error,error_desc);
                    }


                }

            }


        }

        private void Migrate_AnularCFE(string tipo_comprobante, string serie_comprobante,
                                    string codmov ,string nromov_desde, string nromov_hasta,
                                    string codigo_empresa, string clave_empresa,
                                    string clave_comunicacion, bool testing)
        {
            string xml_comp = "";
            string status_consulta = "" ,status_consulta_desc = "";
            string comp_status = "";
            string elim_status = "", elim_status_desc = "";
            InvoicyManual cliente = new InvoicyManual();
            ComprobantesUruguay oComp = new ComprobantesUruguay();
            Comprobantes oComprobante = new Comprobantes();
            xml_comp = oComp.AnulaCFE_get_xml(tipo_comprobante, serie_comprobante, nromov_desde, nromov_hasta, codigo_empresa, clave_empresa, clave_comunicacion);

            cliente.UrlWs = cliente.get_url("AnulaCFE", testing);
            cliente.Soap = cliente.WriteSOAP(xml_comp, "WS_Anulacion.Execute", "Xmlanulacion");
            cliente.ExecutaWS();

            //100 – Éxito
            //180 – Falla
            //181 – Éxito parcial
            status_consulta = cliente.xmlresultado.DocumentElement.SelectSingleNode("/AnulacionRetorno/Encabezado/MsgCod").InnerText;
            status_consulta_desc = cliente.xmlresultado.DocumentElement.SelectSingleNode("/AnulacionRetorno/Encabezado/MsgDsc").InnerText;
            elim_status = cliente.xmlresultado.DocumentElement.SelectSingleNode("/AnulacionRetorno/RespuestaAnulacionCFE/RangoCFE/CFEStatus").InnerText;
            elim_status_desc = cliente.xmlresultado.DocumentElement.SelectSingleNode("/AnulacionRetorno/RespuestaAnulacionCFE/RangoCFE/Mensaje").InnerText;

            if (cliente.tiene_error == false)
            {
                oComprobante.Actualizar(tipo_comprobante, codmov, nromov_desde, elim_status, elim_status_desc, "0", status_consulta, status_consulta_desc);
            }



        }


        private void Uruguay_ActualizarComprobantes()
        {
            DataSet oEmp = new DataSet();
            DataSet oDs = new DataSet();
            DataSet oTxt = new DataSet();

            String  EmpresaID = "", clave_empresa = "", clave_comunicacion = "";
            int cantcomprobantes = 0;
            String codmov = "", nromov = "";
            bool testing = true;


            oEmp = oConfFE.ObtenerEmpresasParametrosFE();
            try
            {
                foreach (DataRow emp in oEmp.Tables[0].Rows)
                {
                    EmpresaID = emp["clienteid"].ToString();
                    clave_empresa = emp["clave_empresa"].ToString();
                    clave_comunicacion = emp["clave_comunicacion"].ToString();

                    if (emp["ambiente_testing"].ToString() != "1")
                    {
                        testing = false;
                    }

                    oDs = oConfFE.ObtenerComprobantes_Actualizar(long.Parse(emp["id"].ToString()));
                    foreach (DataRow row in oDs.Tables[0].Rows)
                    {
                        try
                        {
                            //actualizar visual con cantidades
                            FechaAct = DateTime.Now;
                            cantcomprobantes = oDs.Tables[0].Rows.Count - 1;
                            resul = "";

                            codmov = row["codmov"].ToString();

                            Migrate_ConsultarCFE(row["codmov"].ToString(), row["tipo_comprobante"].ToString(),
                                row["fchmov_desd"].ToString(), row["fchmov_hasta"].ToString(),
                                row["nromov_desde"].ToString(), row["nromov_hasta"].ToString(),
                                EmpresaID, clave_empresa, clave_comunicacion, testing);
                        }
                        catch (Exception exx)
                        {
                            log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Error Actualizando Comprobantes -" + codmov + " ", "Principal", exx, "0");
                            lblError.Text = DateTime.Now.ToShortTimeString() + "Error Actualizando Comprobantes -" + codmov + " " + exx.Message.ToString();
                            lblError.Refresh();
                        }

                        
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Uruguay_EliminarComprobantes()
        {
            DataSet oEmp = new DataSet();
            DataSet oDs = new DataSet();
            DataSet oTxt = new DataSet();

            String EmpresaID = "", clave_empresa = "", clave_comunicacion = "";
            int cantcomprobantes = 0;
            String codmov = "", nromov = "";
            bool testing = true;


            oEmp = oConfFE.ObtenerEmpresasParametrosFE();
            try
            {
                foreach (DataRow emp in oEmp.Tables[0].Rows)
                {
                    EmpresaID = emp["clienteid"].ToString();
                    clave_empresa = emp["clave_empresa"].ToString();
                    clave_comunicacion = emp["clave_comunicacion"].ToString();

                    if (emp["ambiente_testing"].ToString() != "1")
                    {
                        testing = false;
                    }

                    oDs = oConfFE.ObtenerComprobantes_Eliminados(long.Parse(emp["id"].ToString()));
                    foreach (DataRow row in oDs.Tables[0].Rows)
                    {
                        try
                        {
                            //actualizar visual con cantidades
                            FechaAct = DateTime.Now;
                            cantcomprobantes = oDs.Tables[0].Rows.Count - 1;
                            resul = "";


                            Migrate_AnularCFE(row["tipo_comprobante"].ToString(), row["serie"].ToString(),                            
                                row["codmov"].ToString(),row["nromov"].ToString(), row["nromov"].ToString(),
                                EmpresaID, clave_empresa, clave_comunicacion, testing);
                        }
                        catch (Exception exx)
                        {
                            log.escribirArchivo(DateTime.Now.ToShortTimeString(), "Error Anulando Comprobantes -" , "Principal", exx, "0");
                            lblError.Text = DateTime.Now.ToShortTimeString() + "Error Anulando Comprobantes -" + exx.Message.ToString();
                            lblError.Refresh();
                        }


                    }
                }   
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

    }
}
