CREATE TABLE [dbo].[tblInvoice]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [CustomerId] UNIQUEIDENTIFIER NOT NULL, 
    [ServiceName] VARCHAR(50) NOT NULL, 
    [ServiceRate] DECIMAL(18, 4) NOT NULL, 
    [ServiceDate] DATETIME NOT NULL, 
    [EmployeeName] VARCHAR(50) NOT NULL, 
    [Status] VARCHAR(25) NOT NULL
)
