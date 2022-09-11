use dbEMRS
go

--uspCheckLogin 'KalkaniJaydip21@gmail.com','KalkaniJaydip2173831'

alter procedure uspCheckLogin
	@EmailID varchar(50),
	@Passwd varchar(20)

with Encryption
as
declare @errno int,@TranName Varchar(50),@ID smallint,@Image varchar(60),@Designation varchar(15),@Pass varchar(20),@Name varchar(50),@LastLoginDt varchar(15)
set @TranName='CheckLogin'

if(LEN(@Passwd)<20)
begin
	select @Pass=@Passwd+REPLICATE('#',20-LEN(@Passwd))

	set @errno=@@ERROR
	if @errno<>0 goto Err
end
else
begin
	select @Pass=@Passwd

	set @errno=@@ERROR
	if @errno<>0 goto Err
end


Begin Tran @TranName

	if(select COUNT(*) from dbo.UserMst
		where EmailID like @EmailID and Passwd like convert(binary(20),@Pass) and IsActive=1)>0
	begin 
			
			select @ID=UserID,@Image=UserImage,@Name=UserName,@LastLoginDt=CONVERT(varchar(20),LastLoginDt,105)
			from dbo.UserMst
			where EmailID like @EmailID and Passwd like @Pass and IsActive=1
			
			set @Designation='User'

			set @errno=@@ERROR
			if @errno<>0 goto Err

	end
	else if (select COUNT(*) from dbo.ClientMst
			 where EmailID like @EmailID and Passwd like convert(binary(20),@Pass) and IsActive=1)>0
	begin
			select  @ID=ClientID,@Image=CompanyLogo,@Name=CompName,@LastLoginDt=CONVERT(varchar(20),LastLoginDt,105)
			from dbo.ClientMst
			where EmailID like @EmailID and Passwd like @Pass and IsActive=1

			set @Designation='Client'

			set @errno=@@ERROR
			if @errno<>0 goto Err
	end
	else if(select COUNT(*) from dbo.EmployeeMst
		where EmailID like @EmailID and Passwd like convert(binary(20),@Pass) and IsActive=1)>0
	begin
			select @ID=EmpID,@Image=EmpImage,@Designation=Designation,@Name=EmpName,@LastLoginDt=CONVERT(varchar(20),LastLoginDt,105)
			from dbo.EmployeeMst
			where EmailID like @EmailID and Passwd like @Pass and IsActive=1

			set @errno=@@ERROR
			if @errno<>0 goto Err
	end

	 select @ID ID,@Image Imag,@Designation Designation,@Name Name,@LastLoginDt LastLoginDt
	 goto success


	err:
	rollback Tran @Tranname
	return @errno

	success:
	Commit Tran @Tranname
	return 0