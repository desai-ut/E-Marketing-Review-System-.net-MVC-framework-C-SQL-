use dbEMRS
go
create PROCEDURE uspGetState
     -- @State VARCHAR(30)
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT *
      FROM StateMst
     -- WHERE StateName LIKE '%' + @State + '%'
END

--uspGetState 