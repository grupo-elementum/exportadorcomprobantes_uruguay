
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INSERT_MOVIMVTAS]    Script Date: 15/09/2020 11:12:12 ******/
DROP PROCEDURE [dbo].[MENSAJERO_FAC_INSERT_MOVIMVTAS]
GO
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INSERT_MOVIMVTAS]    Script Date: 15/09/2020 11:12:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MENSAJERO_FAC_INSERT_MOVIMVTAS]
	@CODMOV VARCHAR(10),
	@NROMOV NUMERIC(18),
	@CTEORI VARCHAR(10),
	@FCHMOV DATETIME,
	@NROCTA NUMERIC(18),
	@NOMBRE VARCHAR(255),
	@DIRECCION VARCHAR(255),
	@LOCALIDAD VARCHAR(255),
	@PROVINCIA VARCHAR(255),
	@CNDIVA VARCHAR(10),
	@NROCUIT VARCHAR(13),
	@CNDPAG VARCHAR(1),
	@IDVENDEDOR VARCHAR(10),
	@IDFACTURA NUMERIC(18)
AS

 INSERT INTO dbo.MovimVtas (CodMov, NroMov, FchMov, FchRep, FchVnc, CteOri, Sucurs, NroCta, 
 Nombre, Direcc, CodPos, Locali, Provin, CndIva, NrCuit, Abasto, Municp, ZonaVT, Vnddor, LisPre, 
 CndPag, Jurisd, Imputa, Refern, CodAsi, NroAsi, Origen, NroPed, PctDes, Rotulo, Bandej, TxtAdi, Camion, NroOrd,IDFACTURA)

VALUES (@CODMOV, @NROMOV, @FCHMOV, @FCHMOV, @FCHMOV, @CTEORI, '', @NROCTA, @NOMBRE, @DIRECCION, 0, @LOCALIDAD, @PROVINCIA,
@CNDIVA, @NROCUIT, '', '', '', @IDVENDEDOR, '', @CNDPAG, '', 'CO', '', '', 0, 'VT', '', 0.000, '', '', '', 0, 0,@IDFACTURA)



GO
