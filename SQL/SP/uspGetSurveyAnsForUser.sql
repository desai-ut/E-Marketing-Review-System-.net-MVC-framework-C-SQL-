use dbEMRS
go
alter PROCEDURE uspGetSurveyAnsForUser
      @User smallint,
      @Survey smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT SA.*,SQ.Question,SQ.Answer1,SQ.Answer2,SQ.Answer3,SQ.Answer4,SQ.Answer5,SQ.Answer6,SQ.Answer7
      FROM SurveyAns SA join SurveyQues SQ on SA.QuestionID=SQ.QuestionID and SA.CategoryID=SQ.CategoryID and SA.SurveyID=SQ.SurveyID
      WHERE SA.UserID like @User and SA.SurveyID like @Survey
END

--uspGetSurveyAnsForUser  1,1