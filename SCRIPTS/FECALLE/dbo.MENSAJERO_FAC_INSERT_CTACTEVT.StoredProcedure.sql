
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INSERT_CTACTEVT]    Script Date: 15/09/2020 11:12:12 ******/
DROP PROCEDURE [dbo].[MENSAJERO_FAC_INSERT_CTACTEVT]
GO
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INSERT_CTACTEVT]    Script Date: 15/09/2020 11:12:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MENSAJERO_FAC_INSERT_CTACTEVT]
	@CODMOV VARCHAR(10),
	@NROMOV NUMERIC(18),
	@NRITEM INT,
	@cteori VARCHAR(10),
	@NROCTA NUMERIC(18),
	@NROSUB NUMERIC(18),
	@FCHMOV DATETIME,
	@CODAPL VARCHAR(10),
	@NROAPL NUMERIC(18),
	@IMPORTE NUMERIC(18,2),
	@IDFACTURA NUMERIC(18)

AS

if @nrosub = 0 set @nrosub= null

INSERT INTO dbo.CTACTEVT (CodMov, NroMov, NrItem, cteori, fchmov, nrocta, nrosub, codapl, nroapl, fchvnc, Import, refern,IDFACTURA)
VALUES (@CODMOV, @NROMOV, @nrITEM, @cteori, @fchmov, @nrocta, @nrosub, @codapl, @nroapl, @fchmov, @importe, '',@IDFACTURA)
		

GO
