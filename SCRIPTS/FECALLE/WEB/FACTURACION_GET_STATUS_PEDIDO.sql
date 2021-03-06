

CREATE procedure [dbo].[FACTURACION_GET_STATUS_PEDIDO]

	@IDCLIENTE NUMERIC(18),
	@IDREPARTO INT,
	@FECHA DATETIME

AS


SELECT resultado = codmov + str(nromov) FROM PEDIDOS P
INNER JOIN PEDIDOS_FACTURADOS F ON F.IDPEDIDO = P.IDPEDIDO
WHERE FECHA_PEDIDO BETWEEN @FECHA AND DATEADD(MINUTE,1440,@FECHA)
AND IDVENDEDOR = @IDREPARTO AND IDCLIENTE = @IDCLIENTE
GO


