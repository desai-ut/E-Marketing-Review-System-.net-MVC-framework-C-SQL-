use dbEMRS
go
alter PROCEDURE uspGetUserEIDAndPWD
    @UserID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
	  declare @Passwd varchar(20)


	  select @Passwd= charindex('#',Passwd)
	  from dbo.UserMst
	  where UserID=@UserID
	  if @Passwd=0
	  begin
			select @Passwd=Passwd 
			from dbo.UserMst
			where UserID=@UserID
	  end
	  else
	  begin
			select @Passwd=left(Passwd,charindex('#',Passwd)-1)
			from dbo.UserMst
			where UserID=@UserID
	  end
      SELECT EmailID,convert(varchar(20),@Passwd) as Passwd
      FROM UserMst
      WHERE UserID like @UserID and IsActive=1
	  END

	 

--uspGetUserEIDAndPWD 3