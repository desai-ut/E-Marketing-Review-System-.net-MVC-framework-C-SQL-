use dbEMRS
go
alter PROCEDURE uspGetHelpingQues
      --@Quest VARCHAR(30)
with encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT H.*,C.CategoryDisc
      FROM HelpingQues H join CategoryMst C on H.CategoryID=C.CategoryID
      --WHERE CategoryDisc LIKE '%' + @Quest + '%'
END

--uspGetHelpingQues 