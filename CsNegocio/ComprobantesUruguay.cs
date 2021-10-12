using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CsNegocio
{
    public class ComprobantesUruguay
    {
        public string FacturaEmission_get_xml(string codmov, string nromov, string codigo_empresa, string clave_empresa, string clave_comunicacion, DataSet oEncabezado, DataSet oDetalle, DataSet oPie, DataSet oReferencia)
        {
            string xml_resultado = "";
            string xml_cfe = "";
            string xml_cfe_hash = "";

            //ARMA INFORMACION DEL COMPROBANTE

            xml_cfe += "<CFE>";/*A06 PADRE A01*/
            xml_cfe += "<CFEItem>";/*A07 PADRE A06*/
            xml_cfe += "<IdDoc>";/*A08 PADRE A07*/
            xml_cfe += "<CFETipoCFE>"/*A09 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString() + "</CFETipoCFE>"; //tipo comprobante
            xml_cfe += "<CFESerie>"/*A10 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["cfe_nroserie"].ToString() + "</CFESerie>"; //nro de serie
            xml_cfe += "<CFENro>"/*A11 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["nromov"].ToString() + "</CFENro>"; // comprobante nro
            xml_cfe += "<CFEFchEmis>"/*A15 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["fchmov"].ToString() + "</CFEFchEmis>"; // fecha comprobante
            xml_cfe += "<CFEMntBruto>"/*A15 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["ivaincluido"].ToString() + "</CFEMntBruto>"; // montos con iva incluido
            xml_cfe += "<CFEFmaPago>"/*A15 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["formapago"].ToString() + "</CFEFmaPago>"; // forma de pago
            xml_cfe += "<CFEAdenda>"/*A15 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["adenda"].ToString() + "</CFEAdenda>"; // adenda           
            xml_cfe += "</IdDoc>";
            //DATOS EMPRESA EMISORA
            xml_cfe += "<Emisor>"/*A45 PADRE A07*/;
            xml_cfe += "<EmiRznSoc>" /*A46 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_nombre"].ToString() + "</EmiRznSoc>";  //nombre empresa
            xml_cfe += "<EmiDomFiscal>"/*A53 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_direcc"].ToString() + "</EmiDomFiscal>";  //direccion empresa (60 caracteres)
            xml_cfe += "<EmiCiudad>"/*A54 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_ciudad"].ToString() + "</EmiCiudad>";  //ciudad empresa (30 caracteres)
            xml_cfe += "<EmiDepartamento>"/*A55 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_departamento"].ToString() + "</EmiDepartamento>";  //departamento empresa (30 caracteres)            
            xml_cfe += "</Emisor>";
            //DATOS CLIENTE
            xml_cfe += "<Receptor>";/*A57 PADRE A07*/
            xml_cfe += "<RcpTipoDocRecep>"/*A58 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_tipo"].ToString() + "</RcpTipoDocRecep>";  //tipo documento
            xml_cfe += "<RcpCodPaisRecep>"/*A60 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_pais"].ToString() + "</RcpCodPaisRecep>";  //codigo pais
            xml_cfe += "<RcpDocRecep>"/*A61 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_nrodoc"].ToString() + "</RcpDocRecep>";  //nro documento
            xml_cfe += "<RcpRznSocRecep>"/*A62 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_nombre"].ToString() + "</RcpRznSocRecep>";  //cliente nombre
            xml_cfe += "<RcpDirRecep>"/*A63 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_direcc"].ToString() + "</RcpDirRecep>";  //cliente direccion
            xml_cfe += "<RcpCiudadRecep>"/*A64 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_ciudad"].ToString() + "</RcpCiudadRecep>";  //cliente ciudad
            xml_cfe += "<RcpCorreoRecep>"/*A67 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_mail"].ToString() + "</RcpCorreoRecep>";  //cliente mail
            xml_cfe += "</Receptor>";
            xml_cfe += "<Detalle>";/*A127 PADRE A07*/
            if (oDetalle.IsInitialized)
            {
                foreach (DataRow item in oDetalle.Tables[0].Rows)
                {
                    xml_cfe += "<Item>";/*A128 PADRE A127*/
                    //xml_cfe += "<CodItem>";/*A129 PADRE A128*/
                    //xml_cfe += "<CodItemItem>";/*A130 PADRE A129*/
                    xml_cfe += "<IteCodiTpoCod>"/*A131 PADRE A130*/ + item["tipo_producto"].ToString() + "</IteCodiTpoCod>";  //tipo producto
                    xml_cfe += "<IteCodiCod>"/*A132 PADRE A130*/ + item["codigo_producto"].ToString() + "</IteCodiCod>";  //codigo producto
                    xml_cfe += "<IteIndFact>"/*A133 PADRE A130*/ + item["tipo_facturacion"].ToString() + "</IteIndFact>";  //133 indicador de facturacion
                    xml_cfe += "<IteNomItem>"/*A136 PADRE A130*/ + item["abreviatura"].ToString() + "</IteNomItem>";  //136 nombre producto
                    xml_cfe += "<IteDscItem>"/*A137 PADRE A130*/ + item["descripcion"].ToString() + "</IteDscItem>";  //137 adicional nombre producto
                    xml_cfe += "<IteCantidad>"/*A138 PADRE A130*/ + item["cantidad"].ToString() + "</IteCantidad>";  //138 cantidad
                    xml_cfe += "<ItePrecioUnitario>"/*A140 PADRE A130*/ + item["precio"].ToString() + "</ItePrecioUnitario>";  //140 precio
                    xml_cfe += "<IteMontoItem>"/*A140 PADRE A130*/ + item["subtotal"].ToString() + "</IteMontoItem>";  //monto item
                    //xml_cfe += "</CodItemItem>";
                    //xml_cfe += "</CodItem>";
                    xml_cfe += "</Item>";
                }
            }
            xml_cfe += "</Detalle>";

            xml_cfe += "<Totales>";
            xml_cfe += "<TotTpoMoneda>" + oPie.Tables[0].Rows[0]["tipo_moneda"].ToString() + "</TotTpoMoneda>";
            xml_cfe += "<TotMntNoGrv>" + oPie.Tables[0].Rows[0]["impnogravado"].ToString() + "</TotMntNoGrv>";

            if (Double.Parse(oPie.Tables[0].Rows[0]["netotasamin"].ToString()) > 0)
            {
                xml_cfe += "<TotMntNetoIvaTasaMin>" + oPie.Tables[0].Rows[0]["netotasamin"].ToString() + "</TotMntNetoIvaTasaMin>";
                xml_cfe += "<TotIVATasaMin>" + oPie.Tables[0].Rows[0]["tasamin"].ToString() + "</TotIVATasaMin>";
                xml_cfe += "<TotMntIVATasaMin>" + oPie.Tables[0].Rows[0]["imptasamin"].ToString() + "</TotMntIVATasaMin>";
            }
            if (Double.Parse(oPie.Tables[0].Rows[0]["netotasabase"].ToString()) > 0)
            {
                xml_cfe += "<TotIVATasaBasica>" + oPie.Tables[0].Rows[0]["tasabase"].ToString() + "</TotIVATasaBasica>";
                xml_cfe += "<TotMntIVATasaBasica>" + oPie.Tables[0].Rows[0]["imptasabase"].ToString() + "</TotMntIVATasaBasica>";
                xml_cfe += "<TotMntNetoIVATasaBasica>" + oPie.Tables[0].Rows[0]["netotasabase"].ToString() + "</TotMntNetoIVATasaBasica>";
            }
            xml_cfe += "<TotMntTotal>" + oPie.Tables[0].Rows[0]["subtotal"].ToString() + "</TotMntTotal>";
            xml_cfe += "<TotMntPagar>" + oPie.Tables[0].Rows[0]["subtotal"].ToString() + "</TotMntPagar>";
            xml_cfe += "</Totales>";

            if (oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString() == "102" ||
                oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString() == "103" ||
                oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString() == "112" ||
                oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString() == "113"
                )
            {
                if (oReferencia.Tables[0].Rows.Count > 0)
                {
                    xml_cfe += "<Referencia>";
                    xml_cfe += "<ReferenciaItem>";
                    xml_cfe += "<RefNroLinRef>" + oReferencia.Tables[0].Rows[0]["nrolinea"].ToString() + "</RefNroLinRef>";
                    xml_cfe += "<RefTpoDocRef>" + oReferencia.Tables[0].Rows[0]["tipodoc"].ToString() + "</RefTpoDocRef>";
                    xml_cfe += "<RefSerie>" + oReferencia.Tables[0].Rows[0]["refserie"].ToString() + "</RefSerie>";
                    xml_cfe += "<RefNroCFERef>" + oReferencia.Tables[0].Rows[0]["nrodoc"].ToString() + "</RefNroCFERef>";
                    xml_cfe += "</ReferenciaItem>";
                    xml_cfe += "</Referencia>";
                }
                else
                {
                    xml_cfe += "<Referencia>";
                    xml_cfe += "<ReferenciaItem>";
                    xml_cfe += "<RefIndGlobal>" + "1" + "</RefIndGlobal>";
                    xml_cfe += "<RefRazonRef>" + "Sin comprobantes a referenciar" + "</RefRazonRef>";
                    xml_cfe += "<RefNroLinRef>" +"1"+ "</RefNroLinRef>";
                    xml_cfe += "</ReferenciaItem>";
                    xml_cfe += "</Referencia>";

                }
            }
            //xml_cfe += "<SubTotInfo>";/*A163 PADRE A07*/
            //xml_cfe += "<STIItem>";/*A164 PADRE A163*/
            //xml_cfe += "<SubTotNroSTI>"/*A165 PADRE A164*/ + oPie.Tables[0].Rows[0]["nritem"].ToString() + "</SubTotNroSTI>";  //165 nro subtotal
            //xml_cfe += "<SubTotGlosaSTI>"/*A166 PADRE A164*/ + oPie.Tables[0].Rows[0]["titulo_subtotal"].ToString() + "</SubTotGlosaSTI>";  //166 titulo subtotal
            //xml_cfe += "<SubTotValSubtotSTI>"/*A168 PADRE A164*/ + oPie.Tables[0].Rows[0]["subtotal"].ToString() + "</SubTotValSubtotSTI>";  //168 valor subtotal
            //xml_cfe += "</STIItem>";
            //xml_cfe += "</SubTotInfo>";

            xml_cfe += "</CFEItem>";
            xml_cfe += "</CFE>";


            //GENERA EL HASH CON EL XML DEL COMPROBANTE
            xml_cfe_hash = md5.getmd5(clave_comunicacion+xml_cfe);

            //AGREGA EL ENCABEZADO PARA EL ENVIO
            xml_resultado += "<EnvioCFE>";/*A01 PADRE -*/
            xml_resultado += "<Encabezado>";/*A02 PADRE A01*/
            xml_resultado += "<EmpCodigo>"/*A03 PADRE A02*/+ codigo_empresa + "</EmpCodigo>";
            xml_resultado += "<EmpPK>"/*A04 PADRE A02*/ + clave_empresa + "</EmpPK>";
            //xml_resultado += "<EmpCK>"/*A05 PADRE A02*/ + clave_comunicacion + xml_cfe_hash + "</EmpCK>";
            xml_resultado += "<EmpCK>"/*A05 PADRE A02*/ + xml_cfe_hash + "</EmpCK>";
            xml_resultado += "</Encabezado>";
            xml_resultado += xml_cfe; 
            xml_resultado += "</EnvioCFE>";


            return xml_resultado;
        }

        public string RemitoEmission_get_xml(string codmov, string nromov, string codigo_empresa, string clave_empresa, string clave_comunicacion, DataSet oEncabezado, DataSet oDetalle, DataSet oPie, DataSet oReferencia)
        {
            string xml_resultado = "";
            string xml_cfe = "";
            string xml_cfe_hash = "";

            //ARMA INFORMACION DEL COMPROBANTE

            xml_cfe += "<CFE>";/*A06 PADRE A01*/
            xml_cfe += "<CFEItem>";/*A07 PADRE A06*/
            xml_cfe += "<IdDoc>";/*A08 PADRE A07*/
            xml_cfe += "<CFETipoCFE>"/*A09 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["tipo_comprobante"].ToString() + "</CFETipoCFE>"; //tipo comprobante
            xml_cfe += "<CFESerie>"/*A10 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["cfe_nroserie"].ToString() + "</CFESerie>"; //nro de serie
            xml_cfe += "<CFENro>"/*A11 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["nromov"].ToString() + "</CFENro>"; // comprobante nro
            xml_cfe += "<CFEFchEmis>"/*A15 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["fchmov"].ToString() + "</CFEFchEmis>"; // fecha comprobante
            xml_cfe += "<CFETipoTraslado>"/*A15 PADRE A08*/ + oEncabezado.Tables[0].Rows[0]["tipotraslado"].ToString() + "</CFETipoTraslado>"; // forma de pago
            xml_cfe += "</IdDoc>";
            //DATOS EMPRESA EMISORA
            xml_cfe += "<Emisor>"/*A45 PADRE A07*/;
            xml_cfe += "<EmiRznSoc>" /*A46 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_nombre"].ToString() + "</EmiRznSoc>";  //nombre empresa
            xml_cfe += "<EmiDomFiscal>"/*A53 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_direcc"].ToString() + "</EmiDomFiscal>";  //direccion empresa (60 caracteres)
            xml_cfe += "<EmiCiudad>"/*A54 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_ciudad"].ToString() + "</EmiCiudad>";  //ciudad empresa (30 caracteres)
            xml_cfe += "<EmiDepartamento>"/*A55 PADRE A45*/ + oEncabezado.Tables[0].Rows[0]["emp_departamento"].ToString() + "</EmiDepartamento>";  //departamento empresa (30 caracteres)            
            xml_cfe += "</Emisor>";
            //DATOS CLIENTE
            xml_cfe += "<Receptor>";/*A57 PADRE A07*/
            xml_cfe += "<RcpTipoDocRecep>"/*A58 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_tipo"].ToString() + "</RcpTipoDocRecep>";  //tipo documento
            xml_cfe += "<RcpCodPaisRecep>"/*A60 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_pais"].ToString() + "</RcpCodPaisRecep>";  //codigo pais
            xml_cfe += "<RcpDocRecep>"/*A61 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_nrodoc"].ToString() + "</RcpDocRecep>";  //nro documento
            xml_cfe += "<RcpRznSocRecep>"/*A62 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_nombre"].ToString() + "</RcpRznSocRecep>";  //cliente nombre
            xml_cfe += "<RcpDirRecep>"/*A63 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_direcc"].ToString() + "</RcpDirRecep>";  //cliente direccion
            xml_cfe += "<RcpCiudadRecep>"/*A64 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_ciudad"].ToString() + "</RcpCiudadRecep>";  //cliente ciudad
            xml_cfe += "<RcpCorreoRecep>"/*A67 PADRE A57*/ + oEncabezado.Tables[0].Rows[0]["rcp_mail"].ToString() + "</RcpCorreoRecep>";  //cliente mail
            xml_cfe += "</Receptor>";
            xml_cfe += "<Detalle>";/*A127 PADRE A07*/
            if (oDetalle.IsInitialized)
            {
                foreach (DataRow item in oDetalle.Tables[0].Rows)
                {
                    xml_cfe += "<Item>";/*A128 PADRE A127*/
                    xml_cfe += "<IteCodiTpoCod>"/*A131 PADRE A130*/ + item["tipo_producto"].ToString() + "</IteCodiTpoCod>";  //tipo producto
                    xml_cfe += "<IteCodiCod>"/*A132 PADRE A130*/ + item["codigo_producto"].ToString() + "</IteCodiCod>";  //codigo producto
                    xml_cfe += "<IteNomItem>"/*A136 PADRE A130*/ + item["abreviatura"].ToString() + "</IteNomItem>";  //136 nombre producto
                    xml_cfe += "<IteDscItem>"/*A137 PADRE A130*/ + item["descripcion"].ToString() + "</IteDscItem>";  //137 adicional nombre producto
                    xml_cfe += "<IteCantidad>"/*A138 PADRE A130*/ + item["cantidad"].ToString() + "</IteCantidad>";  //138 cantidad
                    xml_cfe += "</Item>";
                }
            }
            xml_cfe += "</Detalle>";

            xml_cfe += "</CFEItem>";
            xml_cfe += "</CFE>";


            //GENERA EL HASH CON EL XML DEL COMPROBANTE
            xml_cfe_hash = md5.getmd5(clave_comunicacion + xml_cfe);

            //AGREGA EL ENCABEZADO PARA EL ENVIO
            xml_resultado += "<EnvioCFE>";/*A01 PADRE -*/
            xml_resultado += "<Encabezado>";/*A02 PADRE A01*/
            xml_resultado += "<EmpCodigo>"/*A03 PADRE A02*/+ codigo_empresa + "</EmpCodigo>";
            xml_resultado += "<EmpPK>"/*A04 PADRE A02*/ + clave_empresa + "</EmpPK>";
            //xml_resultado += "<EmpCK>"/*A05 PADRE A02*/ + clave_comunicacion + xml_cfe_hash + "</EmpCK>";
            xml_resultado += "<EmpCK>"/*A05 PADRE A02*/ + xml_cfe_hash + "</EmpCK>";
            xml_resultado += "</Encabezado>";
            xml_resultado += xml_cfe;
            xml_resultado += "</EnvioCFE>";


            return xml_resultado;
        }

        public string ConsultaCFE_get_xml(string tipo_comprobante, string fecha_desde,string fecha_hata,
            string nromov_desde, string nromov_hasta, string codigo_empresa, 
            string clave_empresa, string clave_comunicacion)
        {
            string xml_resultado = "";
            string xml_cfe = "";
            string xml_cfe_hash = "";


            xml_cfe += "<Filtros>";/*A06 PADRE A01*/
            if (tipo_comprobante != "") xml_cfe += "<TipoCFE>"/*A09 PADRE A08*/ + tipo_comprobante + "</TipoCFE>"; //tipo comprobante
            if (fecha_desde != "") xml_cfe += "<FechaIni>"/*A10 PADRE A08*/ + fecha_desde + "</FechaIni>"; //fecha inicial
            if (fecha_hata != "") xml_cfe += "<FechaFin>"/*A10 PADRE A08*/ + fecha_hata + "</FechaFin>"; //fecha inicial
            if (nromov_desde != "") xml_cfe += "<CFENroIni>"/*A11 PADRE A08*/ + nromov_desde + "</CFENroIni>"; // nro desde
            if (nromov_hasta != "") xml_cfe += "<CFENroFin>"/*A15 PADRE A08*/ + nromov_hasta + "</CFENroFin>"; // nro hasta
            xml_cfe += "<CFETodosErrores>N</CFETodosErrores>"; // nro hasta
            
            xml_cfe += "</Filtros>";

            //GENERA EL HASH CON EL XML DEL COMPROBANTE
            xml_cfe_hash = md5.getmd5(clave_comunicacion + xml_cfe);

            //AGREGA EL ENCABEZADO PARA EL ENVIO
            xml_resultado += "<ConsultaCFE>";/*A01 PADRE -*/
            xml_resultado += "<Encabezado>";/*A02 PADRE A01*/
            xml_resultado += "<EmpCodigo>"/*A03 PADRE A02*/+ codigo_empresa + "</EmpCodigo>";
            xml_resultado += "<EmpPK>"/*A04 PADRE A02*/ + clave_empresa + "</EmpPK>";
            //xml_resultado += "<EmpCK>"/*A05 PADRE A02*/ + clave_comunicacion + xml_cfe_hash + "</EmpCK>";
            xml_resultado += "<EmpCK>"/*A05 PADRE A02*/ + xml_cfe_hash + "</EmpCK>";
            xml_resultado += "</Encabezado>";
            xml_resultado += xml_cfe;
            xml_resultado += "</ConsultaCFE>";


            return xml_resultado;
        }

        public string AnulaCFE_get_xml(string tipo_comprobante, string serie_comprobante, string nromov_desde, string nromov_hasta, 
            string codigo_empresa, string clave_empresa, string clave_comunicacion)
        {
            string xml_resultado = "";
            string xml_cfe = "";
            string xml_cfe_hash = "";

            xml_cfe += "<AnulacionCFE>";/*A06 PADRE A01*/
            xml_cfe += "<RangoCFE>";/*A06 PADRE A01*/
            xml_cfe += "<CFETipo>"/*A09 PADRE A08*/ + tipo_comprobante + "</CFETipo>"; //tipo comprobante
            xml_cfe += "<CFENumeroInicial>"/*A11 PADRE A08*/ + nromov_desde + "</CFENumeroInicial>"; // nro desde
            xml_cfe += "<CFENumeroFinal>"/*A15 PADRE A08*/ + nromov_hasta + "</CFENumeroFinal>"; // nro hasta
            xml_cfe += "<CFESerie>"/*A09 PADRE A08*/ + serie_comprobante + "</CFESerie>"; //tipo comprobante
            xml_cfe += "</RangoCFE>";
            xml_cfe += "</AnulacionCFE>";

            //GENERA EL HASH CON EL XML DEL COMPROBANTE
            xml_cfe_hash = md5.getmd5(clave_comunicacion + xml_cfe);

            //AGREGA EL ENCABEZADO PARA EL ENVIO
            xml_resultado += "<Anulacion>";/*A01 PADRE -*/
            xml_resultado += "<Encabezado>";/*A02 PADRE A01*/
            xml_resultado += "<EmpCodigo>"/*A03 PADRE A02*/+ codigo_empresa + "</EmpCodigo>";
            xml_resultado += "<EmpPK>"/*A04 PADRE A02*/ + clave_empresa + "</EmpPK>";
            xml_resultado += "<EmpCK>"/*A05 PADRE A02*/ + xml_cfe_hash + "</EmpCK>";
            xml_resultado += "</Encabezado>";
            xml_resultado += xml_cfe;
            xml_resultado += "</Anulacion>";


            return xml_resultado;
        }

    }

}
