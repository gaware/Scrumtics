USE [INFRA]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TarefaAtividade](
	[Id] [numeric](10, 0) IDENTITY NOT NULL,
	[Id_atividade] [numeric](10, 0) NOT NULL,
	[Descricao] [varchar](500) NOT NULL,
	[Tempo_previsto] [numeric](5, 2) NOT NULL,
	[Tempo_realizado] [numeric](5, 2) NOT NULL,
	[Status] [char](1) NOT NULL,
 CONSTRAINT [TarefaAtividade_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TarefaAtividade]  WITH CHECK ADD  CONSTRAINT [FK_TarefaAtividade_Atividade] FOREIGN KEY([Id_atividade])
REFERENCES [dbo].[AtividadePacote] ([Id])
GO

ALTER TABLE [dbo].[TarefaAtividade] CHECK CONSTRAINT [FK_TarefaAtividade_Atividade]
GO

CREATE NONCLUSTERED INDEX [TarefaAtividade_Atividade] ON [dbo].[TarefaAtividade]
(
	[Id_atividade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE UNIQUE NONCLUSTERED INDEX [TarefaAtividade_Atividade_Descricao] ON [dbo].[TarefaAtividade] 
(
	[Id_atividade] ASC,
	[Descricao] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
