use dbEMRS
go
alter PROCEDURE uspGetPackage
      @Package tinyint
with Encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM PackageMst
      WHERE isnull(IsActive,0)=case when @Package=0 then isnull(IsActive,0) else @Package END
END

--uspGetPackage 0