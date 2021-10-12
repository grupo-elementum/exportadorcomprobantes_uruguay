
CREATE PROCEDURE [dbo].[FACTURACION_SETPEDIDOFACTURADO]
	@idpedido numeric(18),
	@codmov varchar(10),
	@nromov numeric(18)

AS

if exists(select * from pedidos_facturados where idpedido = @idpedido)
begin
	update pedidos_facturados set codmov = @codmov,nromov = @nromov, fecha_facturado = getdate()
	where idpedido = @idpedido
end
else
begin
	insert into pedidos_facturados(idpedido,codmov,nromov,fecha_facturado)
	values(@idpedido,@codmov,@nromov,getdate())
end 
GO


