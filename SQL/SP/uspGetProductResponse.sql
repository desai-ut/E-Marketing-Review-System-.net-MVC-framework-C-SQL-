use dbEMRS
go
create PROCEDURE uspGetProductResponse
      @ClientID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT S.*,P.ProductID,P.ProductName,P.ProductImage1,P.SurveyPerAmount
      FROM   SurveyMst S
		join  ProductMst P  on S.ClientID=P.ClientID and S.ProductID=P.ProductID 

      WHERE  S.ClientID like @ClientID and P.IsActive=1 

END

--uspGetProductResponse 1