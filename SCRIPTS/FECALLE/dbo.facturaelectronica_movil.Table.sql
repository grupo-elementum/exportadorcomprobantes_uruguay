/****** Object:  Table [dbo].[facturaelectronica_movil]    Script Date: 15/09/2020 11:12:12 ******/
DROP TABLE [dbo].[facturaelectronica_movil]
GO
/****** Object:  Table [dbo].[facturaelectronica_movil]    Script Date: 15/09/2020 11:12:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[facturaelectronica_movil](
	[idfe] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[idpedido] [numeric](18, 0) NULL,
	[idfactura] [numeric](18, 0) NULL,
	[codmov] [varchar](50) NULL,
	[nromov] [numeric](18, 0) NULL,
	[fecha] [datetime] NULL,
 CONSTRAINT [PK_facturaelectronica_movil] PRIMARY KEY CLUSTERED 
(
	[idfe] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
