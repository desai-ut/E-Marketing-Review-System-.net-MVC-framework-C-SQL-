use dbEMRS
go
alter PROCEDURE uspGetReviewQues
     @Client smallint,
	 @Product smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT R.*
      FROM ReviewQues R join SurveyMst SM on R.SurveyID=SM.SurveyID
      WHERE SM.ClientID like @Client and SM.ProductID like @Product
END

--uspGetReviewQues '1','1'