ALTER TABLE [dbo].tblAppointment
	ADD CONSTRAINT [tblAppointment_CustomerId]
	FOREIGN KEY (CustomerId)
	REFERENCES tblCustomer (Id) ON DELETE CASCADE
