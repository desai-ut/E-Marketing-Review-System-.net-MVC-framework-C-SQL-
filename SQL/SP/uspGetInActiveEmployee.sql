use dbEMRS
go
create PROCEDURE uspGetInActiveEmployee
   -- @Emp tinyint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM EmployeeMst
      WHERE IsActive=0 
END

--uspGetInActiveEmployee 