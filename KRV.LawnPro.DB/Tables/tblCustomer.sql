CREATE TABLE [dbo].[tblCustomer]
(
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
	[FirstName] [varchar](25) NOT NULL,
	[LastName] [varchar](35) NOT NULL,
	[StreetAddress] [varchar](50) NOT NULL,
	[City] [varchar](25) NOT NULL,
	[State] [varchar](2) NOT NULL,
	[ZipCode] [varchar](10) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Phone] [varchar](15) NOT NULL,
	[PropertySqFt] [int] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL
)
