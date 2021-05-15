ALTER TABLE [dbo].tblInvoice
	ADD CONSTRAINT [tblInvoice_AppointmentId]
	FOREIGN KEY (AppointmentId)
	REFERENCES tblAppointment (Id) ON DELETE CASCADE