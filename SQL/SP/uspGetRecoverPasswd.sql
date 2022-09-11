use dbEMRS
go
create PROCEDURE uspGetRecoverPasswd
    @EmailID varchar(50)
with encryption
AS
BEGIN
      SET NOCOUNT ON;

	  declare @Passwd varchar(20)

	  if(select COUNT(*) from UserMst
	      where EmailID=@EmailID)>0
	  begin

		 select @Passwd= charindex('#',Passwd)
	   	 from dbo.UserMst
		 where EmailID=@EmailID
	  
	  if @Passwd=0
	  begin
			select @Passwd=Passwd 
			from dbo.UserMst
			where EmailID=@EmailID
	  end
	  else
	  begin
			select @Passwd=left(Passwd,charindex('#',Passwd)-1)
			from dbo.UserMst
			where EmailID=@EmailID
	  end
			SELECT convert(varchar(20),@Passwd) as Passwd
			FROM UserMst
			WHERE EmailID like @EmailID and IsActive=1

	  end

	  else if(select COUNT(*) from ClientMst
	      where EmailID=@EmailID)>0
	  begin
			select @Passwd= charindex('#',Passwd)
			from dbo.ClientMst
			where EmailID=@EmailID

	  if @Passwd=0
	  begin
			select @Passwd=Passwd 
			from dbo.ClientMst
			where EmailID=@EmailID
	  end
	  else
	  begin
			select @Passwd=left(Passwd,charindex('#',Passwd)-1)
			from dbo.ClientMst
			where EmailID=@EmailID
	  end
			
			 SELECT convert(varchar(20),@Passwd) as Passwd
             FROM ClientMst
             WHERE EmailID like @EmailID and IsActive=1

	  end

	  else if(select COUNT(*) from EmployeeMst
	      where EmailID=@EmailID)>0
	  begin
			select @Passwd= charindex('#',Passwd)
			from dbo.EmployeeMst
			where EmailID=@EmailID
	  
	  if @Passwd=0
	  begin
			select @Passwd=Passwd 
			from dbo.EmployeeMst
			where EmailID=@EmailID
	  end
	  else
	  begin
			select @Passwd=left(Passwd,charindex('#',Passwd)-1)
			from dbo.EmployeeMst
			where EmailID=@EmailID
	  end
			  SELECT convert(varchar(20),@Passwd) as Passwd
              FROM EmployeeMst
              WHERE EmailID like @EmailID and IsActive=1
	  end
	  END


--uspGetRecoverPasswd 'Utsav04desai@gmail.com'