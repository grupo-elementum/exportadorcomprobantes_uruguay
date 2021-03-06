
/*
exec FE_OBTENER_DATOS_FACTURA 'CR',1,'B106', 15581
GO
exec FE_OBTENER_DATOS_FACTURA 'TR',1,'B106', 15581
G
exec FE_OBTENER_DATOS_FACTURA 'IV',1,'B106', 15581
GO
exec FE_OBTENER_DATOS_FACTURA 'DE',1,'B106', 15581
GO
exec FE_OBTENER_DATOS_FACTURA 'PI',1,'B106', 15581
GO
*/
ALTER PROCEDURE [dbo].[FE_OBTENER_DATOS_FACTURA]
	@TIPO VARCHAR(2),
	@INTERNOID NUMERIC(18),
	@CODMOV VARCHAR(8),
	@NROMOV NUMERIC(18)
AS

DECLARE @@IVA INT
DECLARE @@COMP INT
DECLARE @@IDFACTURA NUMERIC(18)
DECLARE @NROCTA NUMERIC(18)
DECLARE @NROSUB NUMERIC(18)
DECLARE @NROCTA_VEND NUMERIC(18)
DECLARE @TIPO_FACTURA INT
DECLARE @IDCLIENTE NUMERIC(18,0)

SET @@IDFACTURA = (SELECT MAX(IDFACTURA) FROM FE_CALCULOFACTURA WHERE CODMOV = @CODMOV AND NROMOV = @NROMOV)

IF @TIPO = 'CR'
BEGIN

	SELECT @NROCTA = NROCTA ,@NROSUB= ISNULL(NROSUB,0) FROM FACTURASENC WHERE CODMOV = @CODMOV AND NROMOV = @NROMOV
	SET @TIPO_FACTURA = ISNULL((SELECT TIPO_FACTURA FROM CLIENTES WHERE NROCTA = @NROSUB),0)
	SET @NROCTA_VEND = ISNULL((SELECT NROCTA FROM VENDEDORES WHERE NROCTA = @NROSUB),0)

	

	DECLARE @PROVINCIA INT
	SET @PROVINCIA = (select top 1 idprovincia from clientes group by idprovincia order by count(*) desc)


	set @IDCLIENTE = (case @NROSUB
		when 0 then @NROCTA
		when @NROCTA_VEND then @NROCTA
		else (case @TIPO_FACTURA
		when 1 then @NROCTA else @NROSUB end)END)

	--select @NROCTA,@NROSUB,@TIPO_FACTURA,@NROCTA_VEND,@IDCLIENTE

	--SET @IDCLIENTE = (SELECT NROCTA FROM FACTURASENC WHERE CODMOV = @CODMOV AND NROMOV = @NROMOV)
	SET @@COMP = (SELECT COMPROBANTEFE FROM FRMVTAS WHERE CODIGO = @CODMOV)
	SET @@COMP = (CASE @@COMP WHEN 1 THEN 1 WHEN 2 THEN 1 WHEN 3 THEN 1 WHEN 4 THEN 1 WHEN 5 THEN 1 ELSE @@COMP END)

	/* ################## CORRECCION TICKET 26858 ################### */

	--UPDATE CLIENTES SET EMAILS = REPLACE(EMAILS,'','') WHERE EMAILS LIKE '%'+''+'%' AND NROCTA = @IDCLIENTE

	IF (SELECT COUNT(*) FROM CLIENTES WHERE NROCTA = @IDCLIENTE AND NRCUIT = '0')=1
	BEGIN
		SET NOCOUNT ON
		UPDATE CLIENTES SET NRCUIT = '' WHERE NROCTA = @IDCLIENTE AND NRCUIT = '0'
		SET NOCOUNT OFF	
	END	
	
	IF (SELECT COUNT(*) FROM FACTURASENC WHERE CODMOV = @CODMOV AND NROMOV = @NROMOV AND NRCUIT = '0')=1
	BEGIN
		SET NOCOUNT ON
		UPDATE FACTURASENC SET NRCUIT = '' WHERE CODMOV = @CODMOV AND NROMOV = @NROMOV AND NRCUIT = '0'
		SET NOCOUNT OFF	
	END
	
	


	select 
	tipo= (case replace(replace(replace(replace(replace(rtrim(ltrim(f.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','') when '' 
				then (case replace(replace(replace(replace(replace(rtrim(ltrim(c.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','') when '' then '99' else 
					(case len(replace(replace(replace(replace(replace(rtrim(ltrim(c.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) when 11 then '80' else
					(case when convert(numeric(18),replace(replace(replace(replace(replace(rtrim(ltrim(c.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) >= 90000000 then '91' else
					'96' end) END) end)
				else
					(case len(replace(replace(replace(replace(replace(rtrim(ltrim(f.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) when 11 then '80' else
					(case when convert(numeric(18),replace(replace(replace(replace(replace(rtrim(ltrim(f.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) >= 90000000 then '91' else
					'96' end) END) end),
	cuit= (case replace(replace(replace(replace(replace(rtrim(ltrim(f.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','') when '' 
				then (case replace(replace(replace(replace(replace(rtrim(ltrim(c.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','') when '' then '0' else 
					(case len(replace(replace(replace(replace(replace(rtrim(ltrim(c.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) when 11 then '' else
					(case when convert(numeric(18),replace(replace(replace(replace(replace(rtrim(ltrim(c.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) >= 90000000 then '' else
					'' end) END)+
					replace(replace(replace(replace(replace(rtrim(ltrim(c.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','') end)
				else
					(case len(replace(replace(replace(replace(replace(rtrim(ltrim(f.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) when 11 then '' else
					(case when convert(numeric(18),replace(replace(replace(replace(replace(rtrim(ltrim(f.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','')) >= 90000000 then '' else
					/*(case c.cndiva when 3 then '96' else '80' end)*/'' end) END)+
					replace(replace(replace(replace(replace(rtrim(ltrim(f.nrcuit)),'-',''),'.',''),',',''),'/',''),'|','') end),
			nombre = substring(f.nombre,1,255),
			nrocta = @IDCLIENTE,
			codalt = isnull(i.codalt,''),
			direcc = f.direcc,
			locali = f.locali,
			provincia = ISNULL((CASE (SELECT REPLICATE('0',2-LEN(IDPROV))+RTRIM(LTRIM(IDPROV)) FROM PROVINCIAS
				WHERE IDPROV IN (SELECT IDPROVINCIA FROM CLIENTES WHERE NROCTA = F.NROCTA))
			WHEN '-1' THEN REPLICATE('0',2-LEN(@PROVINCIA))+RTRIM(LTRIM(STR(@PROVINCIA)))
			ELSE (SELECT REPLICATE('0',2-LEN(IDPROV))+RTRIM(LTRIM(IDPROV)) FROM PROVINCIAS
				WHERE IDPROV IN (SELECT IDPROVINCIA FROM CLIENTES WHERE NROCTA = F.NROCTA)) END),
					REPLICATE('0',2-LEN(@PROVINCIA))+RTRIM(LTRIM(STR(@PROVINCIA)))),
			telefono = isnull(c.telefn,''),
			mail =	isnull(c.emails,''),
			fchmov = convert(varchar(8),f.fchmov,112),
			comprobantefe = isnull((select replicate('0',3-len(comprobantefe))+RTRIM(LTRIM(comprobantefe)) 
						from frmvtas where codigo = f.codmov),'001'),
			letra = isnull((select convert(varchar(1),letrid) from frmvtas where codigo = f.codmov),'A'),
			sucursal = isnull((select convert(varchar(4),sucurs) from ctevtas where codigo = f.cteori),'0991'),
			nromov = replicate('0',8-len(nromov))+rtrim(ltrim(str(nromov))),
			bienes = isnull((select top 1 prestadorID from PrestadorFE where prestadorid = (select convert(integer, prestadorid) from parametrosfe where id = @internoid)),'03'),
			codigobarra = rtrim(ltrim(isnull((select codigo_de_barra_txt from MetodosdePagoExternosFacturasCodBar where codmov = f.codmov and nromov = f.nromov),''))),
			reparto = 'REPARTO '+ rtrim(ltrim(str(isnull(f.vnddor,'0')))),
			frecuencia = rtrim(ltrim(substring(replace(isnull((select diarep from rutas where codigo = (select top 1 cdruta from clientesrutas where tipo = 'C' and cliente_ruteo = f.nrocta and left(cdruta, len(cdruta)-1) = f.vnddor)),''),'|',''),1,255))),
			condicion_pago = rtrim(ltrim(substring(isnull((select descrp from condpago where codigo = f.cndpag),''),1,255)))
	from facturasenc f with (nolock)
	left join clientes c with (nolock) on c.nrocta = @IDCLIENTE
	inner join iva i with (nolock) on i.codigo = f.cndiva where f.codmov = @CODMOV and nromov = @NROMOV
END

IF @TIPO = 'TR'
BEGIN

	select tipotributo =
		(case idproducto when 'IB' then '02'
						when 'IB2' then '02'
						when 'IM' then '03'
						when 'II' then '04' else '99' end),
		tributodescripcion =
		(case idproducto when 'IB' then 'IIBB'
						when 'IB2' then 'IIBB2'
						when 'IM' then 'Impuestos Municipales'
						when 'II' then 'Impuestos Internos' else 'Otros' end),
		neto =
		(case idproducto when 'IB' then rtrim(ltrim(str(isnull(( case (select tipo_base_calculo from iibb where cndiva = @@iva and jurisdiccion in 
																	(select jurisdiccioniibb from fe_calculofactura 
																	where IDFACTURA = @@IDFACTURA and idproducto in ('IB')))
																when 'I' then str(isnull((select sum(subtotal) from fe_calculofactura 
																		where IDFACTURA = @@IDFACTURA and tipo <> 'C'),0),18,2)
																when 'T' then str(isnull((select sum(subtotal) from fe_calculofactura 
																		where IDFACTURA = @@IDFACTURA and idproducto = 'TF'),0),18,2)
																else '' end ),0) ,18,2)))
						when 'IB2' then rtrim(ltrim(str(isnull(( case (select tipo_base_calculo from iibb where cndiva = @@iva and jurisdiccion in 
																	(select jurisdiccioniibb2 from fe_calculofactura 
																	where IDFACTURA = @@IDFACTURA and idproducto in ('IB2')))
																when 'I' then str(isnull((select sum(subtotal) from fe_calculofactura 
																		where IDFACTURA = @@IDFACTURA and tipo <> 'C'),0),18,2)
																when 'T' then str(isnull((select sum(subtotal) from fe_calculofactura 
																		where IDFACTURA = @@IDFACTURA and idproducto = 'TF'),0),18,2)
																else '' end ),0) ,18,2)))
						when 'IM' then rtrim(ltrim(str(isnull((select sum(subtotal) from fe_calculofactura 
										where IDFACTURA = @@IDFACTURA and tipo <> 'C'),0) ,18,2)))
						when 'II' then rtrim(ltrim(str(isnull((select sum(subtotal) from fe_calculofactura where IDFACTURA = @@IDFACTURA
																 and tipo <> 'C' and tasaimpuestointerno <> 0),0) ,18,2))) else '0' end),
		porcentaje =
		(case idproducto when 'IB' then rtrim(ltrim(str(tasaiibb,18,2)))
						when 'IB2' then rtrim(ltrim(str(tasaiibb2,18,2)))
						when 'IM' then rtrim(ltrim(str(tasaimpuestomunicipal,18,2)))
						when 'II' then rtrim(ltrim(str(isnull((select sum(tasaimpuestointerno) from fe_calculofactura 
										where IDFACTURA = @@IDFACTURA),0),18,2))) else '' end),
		tributoimporte = rtrim(ltrim(str(subtotal,18,2)))
	from fe_calculofactura
	where IDFACTURA = @@IDFACTURA
	and idproducto in ('IB','IB2','IM') and tipo = 'C'

	union all

	select tipotributo = '04', tributodescripcion='Impuestos Internos',
		neto =
		rtrim(ltrim(str(isnull(SUM(NETOGRAVADO),0) ,18,2))),
		porcentaje =
		rtrim(ltrim(str(isnull(SUM(TASAIMPUESTOINTERNO),0) ,18,2))),
		tributoimporte=
		rtrim(ltrim(str(isnull(SUM(IMPORTEIMPINTERNO),0) ,18,2)))	
	from fe_calculofactura
	where IDFACTURA = @@IDFACTURA
	and tipo <> 'C' and tasaimpuestointerno <> 0
	having isnull(SUM(IMPORTEIMPINTERNO),0) <> 0

END

IF @TIPO = 'IV'
BEGIN	

	SET @@IVA = (SELECT CNDIVA FROM FACTURASENC WHERE CODMOV = @CODMOV AND NROMOV = @NROMOV)
	
	IF (SELECT COUNT(*) FROM FE_CALCULOFACTURA 
			WHERE IDFACTURA = @@IDFACTURA AND IDPRODUCTO IN ('IVA')) >0
	BEGIN

	select tasaiva =ISNULL((SELECT 
			CASE (F.TASAIVA) 
				WHEN 0 THEN '01' 
				WHEN 10.5 THEN '04'
				WHEN 21 THEN '05'
				WHEN 27 THEN '06' 
				ELSE '05' END
			FROM FE_CALCULOFACTURA F
			WHERE IDFACTURA = @@IDFACTURA AND TIPO <> 'C' AND TASAIVA <> 0
			GROUP BY F.TASAIVA),'03'),
		neto = 
		ISNULL((SELECT	RTRIM(LTRIM(STR((SELECT SUM(ROUND(SUBTOTAL,2)) FROM FE_CALCULOFACTURA 
							WHERE IDFACTURA = @@IDFACTURA AND TASAIVA <> 0),18,2)))),0),
		importeiva =ISNULL((select 
			RTRIM(LTRIM(STR(ROUND(SUBTOTAL,2),18,2)))
		FROM FE_CALCULOFACTURA F
		WHERE IDFACTURA = @@IDFACTURA AND IDPRODUCTO IN ('IVA')),0)

	END
	ELSE
	BEGIN

		if not (select COMPROBANTEFE from frmvtas where Codigo = @codmov) in (11,12,13,15)
		BEGIN

			SELECT tasaiva = 
					(select CASE (F.TASAIVA) 
						WHEN 0 THEN '01' 
						WHEN 10.5 THEN '04'
						WHEN 21 THEN '05'
						WHEN 27 THEN '06' 
						ELSE '05' END
			FROM FE_CALCULOFACTURA F
			WHERE IDFACTURA = @@IDFACTURA AND TASAIVA <> 0 AND TIPO <> 'C'
			GROUP BY F.TASAIVA),
			neto = 
			(SELECT RTRIM(LTRIM(STR((SELECT SUM(ROUND(NETOGRAVADO,2)) FROM FE_CALCULOFACTURA 
			WHERE IDFACTURA = @@IDFACTURA AND TASAIVA <> 0 ),18,2)))),
			importeiva = 
			((select LTRIM(RTRIM(STR(SUM(IMPORTEIVA),10,2)))
			FROM FE_CALCULOFACTURA F
			WHERE IDFACTURA = @@IDFACTURA AND TASAIVA <> 0 AND TIPO <> 'C'
			HAVING SUM(IMPORTEIVA)>0))

		END
	END
END

IF @TIPO = 'DE'
BEGIN
	SET @@IVA = (SELECT CNDIVA FROM FACTURASENC WHERE CODMOV = @CODMOV AND NROMOV = @NROMOV)
	SET @@COMP = (SELECT COMPROBANTEFE FROM FRMVTAS WHERE CODIGO = @CODMOV)
	SET @@COMP = (CASE @@COMP WHEN 1 THEN 1 WHEN 2 THEN 1 WHEN 3 THEN 1 WHEN 4 THEN 1 WHEN 5 THEN 1 ELSE @@COMP END)

	select idproducto = (case idproducto when '' then 'D9999' else idproducto end),
		descripcion= (CASE rtrim(ltrim(descripcion)) WHEN rtrim(ltrim(idproducto)) THEN rtrim(ltrim((select top 1 descrp from facturasitem where codmov = @codmov and nromov = @nromov and codigo = f.idproducto)))
					ELSE substring(rtrim(ltrim(descripcion)),1,255) END),
		cantidad = rtrim(ltrim(str(cantidad,18,2))),
		unidad = (case precio when 0 then '97' else '07' end),
		precio = (CASE @@COMP 
		WHEN 1 THEN 
			rtrim(ltrim(str(precio,18,3)))
		ELSE
			rtrim(ltrim(case importeimpinterno when 0 then rtrim(ltrim(str(precio,18,3)))
			else str(precio-importeimpinterno/(case cantidad when 0 then 1 else cantidad end),18,3) end )) 
		END),
		neto = (CASE @@COMP 
		WHEN 1 THEN 
			rtrim(ltrim(str(round(netogravado,2),18,2)))
		ELSE 
			rtrim(ltrim(str(round(subtotal,2)-round(importeimpinterno,2),18,2)))
		END),
		tipoiva = (case precio when 0 then '01' else
		(CASE isnull((select tasapc from iva_productos where cndiva = @@iva and idproducto = (CASE f.TIPO when 'S' then ltrim(rtrim('SRV'+ltrim(rtrim( f.idproducto)))) else f.idproducto end)),0) 
			WHEN 0 THEN '01' 
			WHEN 10.5 THEN '04'
			WHEN 21 THEN '05'
			WHEN 27 THEN '06' 
			ELSE '05' END) end),
		iva = (CASE @@COMP 
				WHEN 1 THEN 
					rtrim(ltrim(str(round(importeiva,2),18,2))) 
				ELSE 
					'' 
				END),
		gln = isnull((select gln from productos where idproducto = f.idproducto and f.tipo = 'P'
					union all
				select gln from serviciosenc where idservicio = f.idproducto and f.tipo = 'S'),'7790001001054'),
		total = (CASE @@COMP 
		WHEN 1 THEN 
				rtrim(ltrim(str(round(netogravado,2)+ round(importeiva,2),18,2))) 
		ELSE 
				rtrim(ltrim(str(round(subtotal,2)-round(importeimpinterno,2),18,2))) 
		END)
	from fe_calculofactura f
	where IDFACTURA = @@IDFACTURA
	and tipo <> 'C'
END

IF @TIPO = 'PI'
BEGIN
	SET @@COMP = (SELECT COMPROBANTEFE FROM FRMVTAS WHERE CODIGO = @CODMOV)
	
	if @@COMP <> 11 AND @@COMP <> 12 AND @@COMP <> 13 AND @@COMP <> 15
	BEGIN

		select total =
			rtrim(ltrim(str(isnull((select sum(round(subtotal,2)) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and idproducto = 'TF'),0),18,2))),
			nogravado = rtrim(ltrim(str(
				isnull((select sum(round(subtotal,2)) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and tasaiva = 0 
				and tasaimpuestointerno = 0 and tipo <> 'C' ),0)
				+
				isnull((select sum(round(netogravado,2)) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and tasaiva = 0 
				and tasaimpuestointerno <> 0 and tipo <> 'C' ),0)
			,18,2))),
			gravado = rtrim(ltrim(str((
				isnull((select sum(round(netogravado,2)) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and tasaiva <> 0 and tipo <> 'C' ),0)
			),18,2))),
			iva = isnull((Case (select count(*) from fe_calculofactura where IDFACTURA = @@IDFACTURA 
					and idproducto in ('IVA')) 
			When 0 then
				rtrim(ltrim(str(isnull((select ltrim(rtrim(Str(sum(Importeiva),10,2)))
					from fe_calculofactura 
					where IDFACTURA = @@IDFACTURA	and tasaiva <> 0 and tipo <> 'C'
					having sum(importeiva)>0 ),0),18,2)))
			else
				rtrim(ltrim(str(isnull((select sum(round(subtotal,2)) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and idproducto = 'IVA'),0),18,2)))
			end),0),
			tributo = rtrim(ltrim(str(isnull((select sum(round(subtotal,2)) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and idproducto in ('IB','IB2','IM') and tipo = 'C'),0)
				+ ISNULL((select sum(importeimpinterno) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and tipo <> 'C' and tasaimpuestointerno <> 0),0),18,2))),

			--+'|'+ '0|'+ /* OPERACIONES EXENTAS */ '|||||||'+
			fecha_desde = convert(varchar(10),fchmov,112),
			fecha_hasta = convert(varchar(10),fchmov,112),
			fecha_vencimiento = convert(varchar(10),fchmov+5,112),
			moneda = 'PES',
			--+'|'+ '|PES|1||||||||||||||||'+
			subtotal = rtrim(ltrim(str((
				isnull((select sum(round(netogravado,2)) from fe_calculofactura 
					where IDFACTURA = @@IDFACTURA and tasaiva <> 0 and tipo <> 'C'),0)
				+
				isnull((select sum(round(subtotal,2)) from fe_calculofactura 
						where IDFACTURA = @@IDFACTURA and tasaiva = 0 and tipo <> 'C'),0)				
			),18,2)))
		from facturasenc f where CODMOV = @CODMOV AND NROMOV = @NROMOV
	END
	ELSE
	BEGIN
		
		select total = rtrim(ltrim(str(isnull((select sum(subtotal) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and idproducto = 'TF'),0),18,2))),
			nogravado = 0,
			gravado = rtrim(ltrim(str(isnull((select sum(subtotal) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and tasaiva = 0 
				and tasaimpuestointerno = 0 and tipo <> 'C'),0)
				+
				isnull((select sum(netogravado) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and tasaiva = 0 
				and tasaimpuestointerno <> 0 and tipo <> 'C'),0)
				,18,2))),
			iva = isnull((Case (select count(*) from fe_calculofactura where IDFACTURA = @@IDFACTURA 
					and idproducto in ('IVA')) 
			When 0 then
				rtrim(ltrim(str(isnull((select ltrim(rtrim(Str(sum(Importeiva),10,2)))
					from fe_calculofactura 
					where IDFACTURA = @@IDFACTURA	and tasaiva <> 0 and tipo <> 'C'
					having sum(importeiva)>0 ),0),18,2)))
			else
				rtrim(ltrim(str(isnull((select sum(subtotal) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and idproducto = 'IVA'),0),18,2)))
			end),0),
			tributo = rtrim(ltrim(str(isnull((select sum(subtotal) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and idproducto in ('IB','IB2','IM') and tipo = 'C'),0)
				+ ISNULL((select sum(importeimpinterno) from fe_calculofactura 
				where IDFACTURA = @@IDFACTURA and tipo <> 'C' and tasaimpuestointerno <> 0),0),18,2))),
			--+'|'+ '0|'+ /* OPERACIONES EXENTAS */ '|||||||'+
			fecha_desde = convert(varchar(10),fchmov,112),
			fecha_hasta = convert(varchar(10),fchmov,112),
			fecha_vencimiento = convert(varchar(10),fchmov+5,112),
			moneda = 'PES',
			--+'|'+ '|PES|1||||||||||||||||'+
			subtotal = rtrim(ltrim(str((
				isnull((select sum(round(netogravado,2)) from fe_calculofactura 
					where IDFACTURA = @@IDFACTURA and tasaiva <> 0 and tipo <> 'C'),0)
				+
				isnull((select sum(round(subtotal,2)) from fe_calculofactura 
						where IDFACTURA = @@IDFACTURA and tasaiva = 0 and tipo <> 'C'),0)				
			),18,2)))
		from facturasenc f where CODMOV = @CODMOV AND NROMOV = @NROMOV
	END
END

IF @TIPO = 'OB'
BEGIN

	SELECT  NRITEM, OBSERV
	FROM FACTURASNOTAS F WHERE F.CODMOV = @CODMOV AND NROMOV = @NROMOV
	AND ISNULL(OBSERV,'') <> ''
	ORDER BY NRITEM
END