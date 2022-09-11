use dbEMRS
go
create PROCEDURE uspGetActiveProduct
	 --@ClientID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM ProductMst 
	  WHERE IsActive=1
END

--uspGetActiveProduct 
