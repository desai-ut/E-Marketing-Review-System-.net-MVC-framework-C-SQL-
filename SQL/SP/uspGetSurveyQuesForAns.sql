use dbEMRS
go
alter PROCEDURE uspGetSurveyQuesForAns
   -- @Client smallint,
	--@Product smallint
	@Survey smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT S.*,C.CategoryDisc
	  FROM SurveyQues S join SurveyMst SM on S.SurveyID=SM.SurveyID join CategoryMst C on C.CategoryID=S.CategoryID
      WHERE SM.SurveyID like @Survey
END

--uspGetSurveyQuesForAns '2'