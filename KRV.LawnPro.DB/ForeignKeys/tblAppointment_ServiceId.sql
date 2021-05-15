ALTER TABLE [dbo].tblAppointment
	ADD CONSTRAINT [tblAppointment_ServiceTypeId]
	FOREIGN KEY (ServiceId)
	REFERENCES tblServiceType (Id) ON DELETE CASCADE
