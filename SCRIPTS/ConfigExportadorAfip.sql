
CREATE TABLE [dbo].[ConfigExportadorAfip](
	[Codigo] [numeric](4, 0) IDENTITY(1,1) NOT NULL,
	[Tarea] [varchar](100) NOT NULL,
	[Tomado] [numeric](1, 0) NOT NULL,
	[Programacion] [datetime] NULL,
	[Tarea2] [varchar](100) NULL,
	[Tarea3] [varchar](100) NULL,
	[Tarea4] [varchar](100) NULL,
	[Corrio] [numeric](1, 0) NULL,
	[UltimaEjecucion] [datetime] NULL
) ON [PRIMARY]

