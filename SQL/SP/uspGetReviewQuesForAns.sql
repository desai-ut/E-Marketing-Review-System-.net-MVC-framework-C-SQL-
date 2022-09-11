use dbEMRS
go
alter PROCEDURE uspGetReviewQuesForAns
   -- @Client smallint,
	--@Product smallint
	@Survey smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT R.*,C.CategoryDisc
      FROM ReviewQues R join SurveyMst SM on R.SurveyID=SM.SurveyID join CategoryMst C on C.CategoryID=R.CategoryID
      WHERE SM.SurveyID like @Survey
END

--uspGetReviewQuesForAns '2'