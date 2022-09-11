use dbEMRS
go
alter PROCEDURE uspGetReviewAnsForUser
      @User smallint,
	  @Survey smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT RA.*,RQ.Question
      FROM ReviewAns RA join ReviewQues RQ on RA.QuestionID=RQ.QuestionID and RA.CategoryID=RQ.CategoryID and RA.SurveyID=RQ.SurveyID
      WHERE RA.UserID like @User and RA.SurveyID like @Survey
END

--uspGetReviewAnsForUser  1,1