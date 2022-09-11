use dbEMRS
go
alter PROCEDURE uspGetCity
     -- @City VARCHAR(30)
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM CityMst
     -- WHERE CityName LIKE '%' + @City + '%'
END

--uspGetCity 