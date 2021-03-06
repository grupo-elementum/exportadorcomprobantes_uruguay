
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_GET_IIBB]    Script Date: 15/09/2020 11:12:12 ******/
DROP PROCEDURE [dbo].[MENSAJERO_FAC_GET_IIBB]
GO
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_GET_IIBB]    Script Date: 15/09/2020 11:12:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[MENSAJERO_FAC_GET_IIBB]

	@NROCTA NUMERIC(18)
AS
select pcia_iibb,situacion_iibb,porc_iibb,tipo_base_calculo,monto_base_calculo,concepto_iibb ,descrp,tipo='IB'
from clientes c
inner join iibb i on i.Jurisdiccion = c.Pcia_IIBB and i.cndiva = c.cndiva
inner join cptvtas v on v.codcpt = concepto_iibb
where nrocta = @NROCTA AND C.PORC_IIBB <> 0
union all 
select c.pcia_iibb,c.situacion_iibb,c.porc_iibb,tipo_base_calculo,monto_base_calculo,concepto_iibb,descrp,tipo='IB2'
from clientes_iibb c
inner join clientes cc on cc.nrocta = c.nrocta
inner join iibb i on i.Jurisdiccion = c.Pcia_IIBB and i.cndiva = cc.cndiva
inner join cptvtas v on v.codcpt = concepto_iibb
where C.nrocta = @NROCTA AND C.PORC_IIBB <> 0



GO
