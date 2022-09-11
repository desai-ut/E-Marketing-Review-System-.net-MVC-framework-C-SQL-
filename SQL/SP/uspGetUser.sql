use dbEMRS
go
alter PROCEDURE uspGetUser
    @User smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM UserMst
      WHERE isnull(IsActive,0)=case when @User=0 then isnull(IsActive,0) else @User END
END

--uspGetUserMst 1