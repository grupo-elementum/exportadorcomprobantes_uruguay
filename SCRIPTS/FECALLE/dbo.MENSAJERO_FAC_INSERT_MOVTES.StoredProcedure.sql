
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INSERT_MOVTES]    Script Date: 15/09/2020 11:12:12 ******/
DROP PROCEDURE [dbo].[MENSAJERO_FAC_INSERT_MOVTES]
GO
/****** Object:  StoredProcedure [dbo].[MENSAJERO_FAC_INSERT_MOVTES]    Script Date: 15/09/2020 11:12:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MENSAJERO_FAC_INSERT_MOVTES]

  @CodMov Varchar(6), @NroMov Numeric(18, 0), @NrItem Int, @FchMov DateTime, 
  @TipCta Char(1), @NroCta numeric(18), @Refern Char(30), @CodCpt Char(3), 
  @NroBco Char(3), @Sucurs Char(3), @CPBcos as Int,  @Cheque Char(8), 
  @Cuenta Char(11), @Titular Char(80), @FchVnc DateTime, @CtaCte Char(2), 
  @Import Numeric(18, 2), @DebHab Char(1), @CodAsi Char(2), @NroAsi Numeric(18,0) 

AS INSERT INTO MovTes(CodMov, NroMov, NrItem, FchMov, TipCta, NroCta, 
  Refern, CodCpt, NroBco, Sucurs, CPBcos, Cheque, Cuenta, Titular, FchVnc, 
  CtaCte, Import, DebHab, CodAsi, NroAsi)

VALUES(@CodMov, @NroMov, @NrItem, @FchMov, @TipCta, @NroCta, 
  @Refern, @CodCpt, @NroBco, @Sucurs, @CPBcos, @Cheque, @Cuenta, 
  @Titular, @FchVnc, @CtaCte, @Import, @DebHab, @CodAsi, @NroAsi)


GO
