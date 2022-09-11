use dbEMRS
go
alter PROCEDURE uspGetProduct
      @Product smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM ProductMst
      WHERE ClientID LIKE @Product and IsActive=0
END

--uspGetProduct 1