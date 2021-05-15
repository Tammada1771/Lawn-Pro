ALTER TABLE [dbo].tblEmployee
	ADD CONSTRAINT [tblEmployee_UserId]
	FOREIGN KEY (UserId)
	REFERENCES tblUser (Id) ON DELETE NO ACTION
