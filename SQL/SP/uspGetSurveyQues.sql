use dbEMRS
go
alter PROCEDURE uspGetSurveyQues
    @Client smallint,
	@Product smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT S.*
      FROM SurveyQues S join SurveyMst SM on S.SurveyID=SM.SurveyID
      WHERE SM.ClientID like @Client and SM.ProductID like @Product
END

--uspGetSurveyQues '2','1'