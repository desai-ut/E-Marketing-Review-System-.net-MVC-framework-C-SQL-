use dbEMRS
go
alter PROCEDURE uspGetSurveyProduct
    @ClientID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT S.*,P.ProductImage1,P.ProductName,P.TotalSurveyAmt
      FROM SurveyMst S join ProductMst P on S.ClientID=P.ClientID and S.ProductID=P.ProductID
	  WHERE S.ClientID like @ClientID and P.IsActive=0
END

--uspGetSurveyProduct 1

