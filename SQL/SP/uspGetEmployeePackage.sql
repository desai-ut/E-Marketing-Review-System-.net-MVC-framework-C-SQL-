use dbEMRS
go
create PROCEDURE uspGetEmployeePackage
      @EmpID tinyint
with Encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM PackageMst
      WHERE EmpID like @EmpID and IsActive=0
END

--uspGetEmployeePackage 2