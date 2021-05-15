ALTER TABLE [dbo].tblCustomer
	ADD CONSTRAINT [tblCustomer_UserId]
	FOREIGN KEY (UserId)
	REFERENCES tblUser (Id) ON DELETE CASCADE
