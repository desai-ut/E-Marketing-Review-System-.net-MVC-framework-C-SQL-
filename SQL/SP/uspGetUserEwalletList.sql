use dbEMRS
go
create PROCEDURE uspGetUserEwalletList
      @UserID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      
	  SELECT P.*

      FROM EwalletMst E join SurveyMst S on E.SurveyID=S.SurveyID
			join ProductMst P on S.ProductID=P.ProductID and S.ClientID = P.ClientID
			join UserMst U on U.UserID=E.UserID
	  
      WHERE  E.UserID like @UserID and IsApproved=1


END

--uspGetUserEwalletList 1