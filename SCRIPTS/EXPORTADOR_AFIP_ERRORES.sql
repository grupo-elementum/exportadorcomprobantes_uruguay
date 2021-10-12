
CREATE TABLE [dbo].[EXPORTADOR_AFIP_ERRORES](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[codmov] [varchar](10) NULL,
	[nromov] [numeric](18, 0) NULL,
	[tipo_error] [varchar](50) NULL,
	[descripcion_error] [varchar](8000) NULL,
	[notificado] [int] NULL,
	[fecha] [datetime] NULL,
 CONSTRAINT [PK_EXPORTADOR_AFIP_ERRORES] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

