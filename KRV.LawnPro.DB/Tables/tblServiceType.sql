CREATE TABLE [dbo].[tblServiceType]
(
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[Description] VARCHAR(50) NOT NULL,
	[CostPerSqFt] [decimal](18, 4) NOT NULL
)
