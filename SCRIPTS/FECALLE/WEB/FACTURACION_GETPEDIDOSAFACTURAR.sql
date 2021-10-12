
CREATE PROCEDURE [dbo].[FACTURACION_GETPEDIDOSAFACTURAR]
AS


set nocount on;
select p.idcliente,idped=min(p.idpedido)
into #tmppedidos
from pedidos p
inner join clientes c on c.idcliente = p.idcliente
where fecha_pedido >= convert(varchar(8),getdate(),112)
and DATEDIFF(MINUTE,fecha_pedido,GETdate()) > 20
and reqcomp = 2
and factura in ('A','B')
and borra_pedido = 0
--and idvendedor in (62,33,34,65)
and not p.idpedido in (select idpedido from pedidos_facturados)
and (select sum(cantidad*precio) from pedidos_productos where idpedido = p.idpedido) >0
group by p.idcliente
set nocount off;



select p.*,d.*
,nro_recibo=isnull((select nro_recibo from movimientos_caja 
				where idpedido = p.idpedido and idcliente = p.idcliente and importe <0 ),0) 
from pedidos p
inner join pedidos_productos d on d.idpedido = p.idpedido
inner join clientes c on c.idcliente = p.idcliente
where fecha_pedido >= convert(varchar(8),getdate(),112)
and DATEDIFF(MINUTE,fecha_pedido,GETdate()) > 20
--and idvendedor in (62,33,34,65)
and reqcomp = 2
and factura in ('A','B')
and borra_pedido = 0
and borra_item = 0
and not p.idpedido in (select idpedido from pedidos_facturados)
and (select sum(cantidad*precio) from pedidos_productos where idpedido = p.idpedido) >0
and p.idpedido in (select idped from #tmppedidos)




set nocount on;
insert into pedidos_facturados(idpedido,codmov,nromov,fecha_facturado)
select p.idpedido,'',0,getdate()
from pedidos p
inner join clientes c on c.idcliente = p.idcliente
where fecha_pedido >= convert(varchar(8),getdate(),112)
and DATEDIFF(MINUTE,fecha_pedido,GETdate()) > 20
and reqcomp = 2
and factura in ('A','B')
and borra_pedido = 0
--and idvendedor in (62,33,34,65)
and not p.idpedido in (select idpedido from pedidos_facturados)
and (select sum(cantidad*precio) from pedidos_productos where idpedido = p.idpedido) >0
and p.idpedido in (select idped from #tmppedidos)

set nocount off;



GO


