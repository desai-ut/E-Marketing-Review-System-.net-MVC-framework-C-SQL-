use dbEMRS
go
create PROCEDURE uspGetQuery
      --@ClientID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM   QueryMst 

END

--uspGetQuery 