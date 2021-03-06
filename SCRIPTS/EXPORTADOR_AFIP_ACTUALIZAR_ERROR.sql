
ALTER PROCEDURE [dbo].[EXPORTADOR_AFIP_ACTUALIZAR_ERROR]
	@CODMOV VARCHAR(10),
	@NROMOV NUMERIC(18),
	@TIPO_ERROR VARCHAR(255),
	@OBS VARCHAR(8000),
	@NOTIFICO INT
AS
DECLARE @RESULTADO VARCHAR(255)

SET @RESULTADO = 'NO-NOTIFICAR'

	if not exists(select * from EXPORTADOR_AFIP_ERRORES where codmov = @codmov and nromov = @nromov and tipo_error = @tipo_error)
	begin
		INSERT INTO EXPORTADOR_AFIP_ERRORES (codmov,nromov,tipo_error,descripcion_error,fecha,notificado)
		VALUES (@CODMOV,@NROMOV,@TIPO_ERROR,@OBS,GETDATE(),@NOTIFICO)

		SET @RESULTADO = 'NOTIFICAR'
	end
	
select resultado = @RESULTADO