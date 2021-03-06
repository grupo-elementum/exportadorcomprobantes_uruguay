
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INS_PEDIDO]    Script Date: 15/09/2020 11:12:12 ******/
DROP PROCEDURE [dbo].[MENSAJERO_FAC_INS_PEDIDO]
GO
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INS_PEDIDO]    Script Date: 15/09/2020 11:12:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MENSAJERO_FAC_INS_PEDIDO]


@IDPEDIDO NUMERIC(18),
@IDVENDEDOR INT,
@FECHA DATETIME,
@IDCLIENTE NUMERIC(18),
@FACTURA CHAR(1),
@IDCLIENTENUEVO INT,
@IDPRODUCTO CHAR(6),
@TIPO CHAR(1),
@CANTIDAD INT,
@PRECIO NUMERIC(18,3),
@encabezado int ,
@cobrado NUMERIC(18,2),
@nrorecibo varchar(50)

AS

declare @@resultado varchar(50)

set @@resultado = ''

if (@encabezado = 1) 
begin
	if not exists(select * from pedidos where idpedido =@IDPEDIDO )
	begin
		INSERT INTO PEDIDOS (IDPEDIDO, IDVENDEDOR, FECHA_PEDIDO, IDCLIENTE, FACTURA, NRO_COMPROBANTE,IDCLIENTENUEVO) 
		VALUES (@IDPEDIDO, @IDVENDEDOR, @FECHA, @IDCLIENTE, @FACTURA, 0,ISNULL(@IDCLIENTENUEVO,0))

		INSERT INTO Movimientos_Caja(  idReparto, idPedido, idCliente, fecha, importe, Descripcion,idClienteNuevo,Nro_Recibo) 
		values( @IDVENDEDOR, @idPedido, @idCliente, convert(varchar(10),@fecha,103),0, 'Su Compra',ISNULL(@idClienteNuevo,0),0)


		if @nrorecibo= '' set @nrorecibo = '0'
		if (@cobrado >0)
		begin
			INSERT INTO Movimientos_Caja(  idReparto, idPedido, idCliente, fecha, importe, Descripcion,idClienteNuevo,Nro_Recibo) 
			VALUES ( @IDVENDEDOR, @idPedido, @idCliente, convert(varchar(10),@fecha,103), @cobrado*-1, 'Su Pago',ISNULL(@idClienteNuevo,0),ISNULL(@nrorecibo,0))
		end
	end
	else
	begin
		set @@resultado = 'ya existe'
	end
end
if @@resultado = ''
begin
	INSERT INTO Pedidos_Productos (idPedido, idProducto, Tipo, Cantidad, Precio,TipoBonificacion) 
	VALUES (@idPedido, ltrim(@idProducto), @TIPO, @Cantidad, @Precio,'M')

	update movimientos_caja 
	set importe = isnull((select sum(cantidad*precio) 
							from Pedidos_Productos
							where idpedido =@idPedido ),0)
	where idpedido = @idPedido 
	and idcliente = @idCliente 
	and IDCLIENTENUEVO = ISNULL(@idClienteNuevo,0)
	and fecha = convert(varchar(10),@fecha,103)
	and importe >= 0

end

select resultado = @@resultado

GO
