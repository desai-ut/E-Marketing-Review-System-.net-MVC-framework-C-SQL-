use dbEMRS
go
alter PROCEDURE uspGetEmployee
    @Emp tinyint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM EmployeeMst
      WHERE EmpID not LIKE @Emp
END

--uspGetEmployee '1'