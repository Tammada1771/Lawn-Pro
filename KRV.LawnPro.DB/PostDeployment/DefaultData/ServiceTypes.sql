BEGIN
	INSERT INTO DBO.tblServiceType (Id, Description, CostPerSqFt)
	VALUES 
		(NEWID(), 'Mow', .0015),
		(NEWID(), 'Dethatching', .0025),
		(NEWID(), 'Aeration', .002)

END