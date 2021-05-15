CREATE TABLE [dbo].[tblAppointment]
(
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [uniqueidentifier] NULL,
	[StartDateTime] [datetime] NOT NULL,
	[EndDateTime] [datetime] NOT NULL,
	[ServiceId] UNIQUEIDENTIFIER NOT NULL,
	[Status] [varchar](25) NOT NULL
)
