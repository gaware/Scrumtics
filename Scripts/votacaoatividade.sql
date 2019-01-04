USE [INFRA]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VotacaoAtividade](
	[Id] [numeric](10, 0) IDENTITY NOT NULL,
	[Id_atividade] [numeric](10, 0) NOT NULL,
	[Id_usuario] [numeric](10, 0) NOT NULL,
	[Tempo] [numeric](3, 0) NOT NULL,
 CONSTRAINT [VotacaoAtividade_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[VotacaoAtividade]  WITH CHECK ADD  CONSTRAINT [FK_VotacaoAtividade_Atividade] FOREIGN KEY([Id_atividade])
REFERENCES [dbo].[AtividadePacote] ([Id])
GO

ALTER TABLE [dbo].[VotacaoAtividade] CHECK CONSTRAINT [FK_VotacaoAtividade_Atividade]
GO

ALTER TABLE [dbo].[VotacaoAtividade]  WITH CHECK ADD  CONSTRAINT [FK_VotacaoAtividade_Usuario] FOREIGN KEY([Id_usuario])
REFERENCES [dbo].[Usuario] ([Id])
GO

ALTER TABLE [dbo].[VotacaoAtividade] CHECK CONSTRAINT [FK_VotacaoAtividade_Usuario]
GO

CREATE NONCLUSTERED INDEX [VotacaoAtividade_Atividade] ON [dbo].[VotacaoAtividade]
(
	[Id_atividade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE NONCLUSTERED INDEX [VotacaoAtividade_Usuario] ON [dbo].[VotacaoAtividade]
(
	[Id_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE UNIQUE NONCLUSTERED INDEX [VotacaoAtividade_Atividade_Usuario] ON [dbo].[VotacaoAtividade]
(
	[Id_atividade] ASC,
	[Id_usuario] ASC	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO
