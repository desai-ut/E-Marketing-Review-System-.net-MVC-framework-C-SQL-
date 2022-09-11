use dbEMRS
go
alter PROCEDURE uspGetUserEwallet
      @UserID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      declare @RewardPointTL smallint,@NoOfAttemptedSurveyQues smallint,@NoOfAttemptedReviewQues smallint,@RewardPoint smallint,@SurveyID smallint
	  select @RewardPointTL=ISNULL(SUM(TotalTransferRewardPoint),0) from TransferAmount
	  where UserID=@UserID


	  SELECT @SurveyID=COUNT(SurveyID),@NoOfAttemptedSurveyQues=SUM(NoOfAttemptedSurveyQues),
	  @NoOfAttemptedReviewQues=SUM(NoOfAttemptedReviewQues),@RewardPoint=(isnull(SUM(RewardPoint),0)-@RewardPointTL)
      FROM EwalletMst 
	  
      WHERE  UserID like @UserID and IsApproved=1

	  group by UserID,IsApproved

	  select isnull(@SurveyID,0) as SurveyID,isnull(@NoOfAttemptedSurveyQues,0) as NoOfAttemptedSurveyQues,
	  isnull(@NoOfAttemptedReviewQues,0) as NoOfAttemptedReviewQues,isnull(@RewardPoint,0) RewardPoint
	  
	  END

--uspGetUserEwallet 22