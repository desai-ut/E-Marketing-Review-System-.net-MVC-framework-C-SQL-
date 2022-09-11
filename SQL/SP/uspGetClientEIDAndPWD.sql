use dbEMRS
go
alter PROCEDURE uspGetClientEIDAndPWD
    @ClientID smallint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
	  declare @Passwd varchar(20)


	  select @Passwd= charindex('#',Passwd)
	  from dbo.ClientMst
	  where ClientID=@ClientID
	  if @Passwd=0
	  begin
			select @Passwd=Passwd 
			from dbo.ClientMst
			where ClientID=@ClientID
	  end
	  else
	  begin
			select @Passwd=left(Passwd,charindex('#',Passwd)-1)
			from dbo.ClientMst
			where ClientID=@ClientID
	  end
      SELECT EmailID,convert(varchar(20),@Passwd) as Passwd
      FROM ClientMst
      WHERE ClientID like @ClientID and IsActive=1
	  END


--uspGetClientEIDAndPWD 2