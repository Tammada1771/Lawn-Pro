CREATE PROCEDURE [dbo].[spGetCustomerBalance]
	@customerId uniqueidentifier
AS
	BEGIN
		SELECT  ISNULL(SUM(i.ServiceRate * c.PropertySqFt), 0) as Balance
		FROM tblInvoice i
		JOIN tblCustomer c on i.CustomerId = c.Id 
		WHERE i.CustomerId = @customerId and i.Status = 'ISSUED'
	END

return 0;