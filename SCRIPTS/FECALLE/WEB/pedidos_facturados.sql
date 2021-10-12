

CREATE TABLE [dbo].[pedidos_facturados](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[idpedido] [numeric](18, 0) NULL,
	[codmov] [varchar](10) NULL,
	[nromov] [numeric](18, 0) NULL,
	[fecha_facturado] [datetime] NULL,
 CONSTRAINT [PK_pedidos_facturados] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Index [IX_pedidos_facturados]    Script Date: 15/09/2020 11:07:21 ******/
CREATE NONCLUSTERED INDEX [IX_pedidos_facturados] ON [dbo].[pedidos_facturados]
(
	[idpedido] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


