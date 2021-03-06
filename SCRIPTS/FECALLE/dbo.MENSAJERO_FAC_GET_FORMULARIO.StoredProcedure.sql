
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_GET_FORMULARIO]    Script Date: 15/09/2020 11:12:12 ******/
DROP PROCEDURE [dbo].[MENSAJERO_FAC_GET_FORMULARIO]
GO
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_GET_FORMULARIO]    Script Date: 15/09/2020 11:12:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MENSAJERO_FAC_GET_FORMULARIO]
	@sucursal varchar(10),
	@cndiva int
AS


create table #formularios (cndiva int,form varchar(10) collate SQL_Latin1_General_CP1_CI_AS,comprobante varchar(10))

insert into #formularios
select c.cniva1,formu1,c.codigo 
from sucursales s
inner join ctevtas c on c.codigo =s.cte_factura
where sucursal = @sucursal
union all
select c.cniva2,formu2,c.codigo 
from sucursales s
inner join ctevtas c on c.codigo =s.cte_factura
where sucursal = @sucursal
union all
select c.cniva3,formu3,c.codigo 
from sucursales s
inner join ctevtas c on c.codigo =s.cte_factura
where sucursal = @sucursal
union all
select c.cniva4,formu4,c.codigo 
from sucursales s
inner join ctevtas c on c.codigo =s.cte_factura
where sucursal = @sucursal
union all
select c.cniva5,formu5,c.codigo 
from sucursales s
inner join ctevtas c on c.codigo =s.cte_factura
where sucursal = @sucursal

select * from #formularios t
inner join frmvtas f on f.codigo = t.form
 where cndiva = @cndiva

GO
