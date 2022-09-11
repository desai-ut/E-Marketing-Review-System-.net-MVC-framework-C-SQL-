use dbEMRS
go
create PROCEDURE uspGetCategory
      --@Category VARCHAR(30)
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM CategoryMst
     -- WHERE CategoryDisc LIKE '%' + @Category + '%'
END

--uspGetCategory 