use dbEMRS
go
create PROCEDURE uspGetEIDAndPWD
    @EmpID tinyint
with encryption
AS
BEGIN
      SET NOCOUNT ON;
	  declare @Passwd varchar(20)


	  select @Passwd= charindex('#',Passwd)
	  from dbo.EmployeeMst
	  where EmpID=@EmpID
	  if @Passwd=0
	  begin
			select @Passwd=Passwd 
			from dbo.EmployeeMst
			where EmpID=@EmpID
	  end
	  else
	  begin
			select @Passwd=left(Passwd,charindex('#',Passwd)-1)
			from dbo.EmployeeMst
			where EmpID=@EmpID
	  end
      SELECT EmailID,convert(varchar(20),@Passwd) as Passwd
      FROM EmployeeMst
      WHERE EmpID like @EmpID and IsActive=1
	  END

	 

--uspGetEIDAndPWD 4