use dbEMRS
go
alter PROCEDURE uspGetResponse
      @SurveyID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      
	  SELECT E.NoOfAttemptedSurveyQues,E.NoOfAttemptedReviewQues,E.IsApproved,U.UserID,U.UserImage,U.UserName,
	  COUNT(distinct SQ.Question) as SQuestion,COUNT(distinct RQ.Question) as RQuestion
      FROM  EwalletMst E join UserMst U on E.UserID=U.UserID
			join SurveyQues SQ on SQ.SurveyID=E.SurveyID 
			join ReviewQues RQ on RQ.SurveyID=E.SurveyID
      WHERE  E.SurveyID like @SurveyID

	  group by U.UserID,U.UserImage,U.UserName,E.NoOfAttemptedSurveyQues,E.NoOfAttemptedReviewQues,E.IsApproved

END

--uspGetResponse 1