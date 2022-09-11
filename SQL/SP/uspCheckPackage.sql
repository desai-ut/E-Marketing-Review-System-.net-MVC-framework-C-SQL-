use dbEMRS
go
--uspCheckPackage 8
alter procedure uspCheckPackage
	@ClientID smallint

with Encryption
as
declare @errno int,@maxClientID smallint,@TranName Varchar(50),@SysDate date
set @TranName='uspCheckPackage'
set @maxClientID=@ClientID
Begin Tran @TranName
select @SysDate=cast(GETDATE() as date)

	if(select count(*) from PurchasePackage
	where ClientID=@ClientID and PaidStatus=1 and ProductCnt>0 and EndingDate>=@SysDate)>0
	begin 
				set	@maxClientID=1

			set @errno=@@ERROR
			if @errno<>0 goto Err

	end
	else
	begin
			set @maxClientID=0

			set @errno=@@ERROR
			if @errno<>0 goto Err
	end

	select @maxClientID ClientID
	goto success


	err:
	rollback Tran @Tranname
	return @errno

	success:
	Commit Tran @Tranname
	return 0