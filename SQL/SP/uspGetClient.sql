use dbEMRS
go
alter PROCEDURE uspGetClient
    @Client smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM ClientMst
      WHERE isnull(IsActive,0)=case when @Client=0 then isnull(IsActive,0) else @Client END
END

--uspGetClient 1