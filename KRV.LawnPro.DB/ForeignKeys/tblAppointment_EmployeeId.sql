ALTER TABLE [dbo].tblAppointment
	ADD CONSTRAINT [tblAppointment_EmployeeId]
	FOREIGN KEY (EmployeeId)
	REFERENCES tblEmployee (Id) ON DELETE CASCADE
