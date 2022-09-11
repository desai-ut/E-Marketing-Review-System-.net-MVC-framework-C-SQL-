use dbEMRS
go
alter PROCEDURE uspGetPublishProduct
	 --@ClientID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT S.*,P.ProductImage1,P.ProductName,P.TotalSurveyAmt,P.ProductCompany,P.IsActive,P.IsPaid,P.SurveyPerAmount
      FROM SurveyMst S join ProductMst P on S.ClientID=P.ClientID and S.ProductID=P.ProductID 
	  WHERE IsPublished=1
END

--uspGetPublishProduct 












