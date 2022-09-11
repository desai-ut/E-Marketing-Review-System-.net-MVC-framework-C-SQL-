use dbEMRS
go
alter PROCEDURE uspGetPurchasePackage
      @PurchasePackage smallint
with Encryption
AS
BEGIN
      SET NOCOUNT ON;
      SELECT P.*,C.CompName,Pa.PackageTitle,Pa.Duration
      FROM PurchasePackage P join ClientMst C on P.ClientID=C.ClientID join PackageMst Pa on Pa.PackageID=p.PackageID
      WHERE isnull(PaidStatus,0)=case when @PurchasePackage=0 then isnull(PaidStatus,0) else @PurchasePackage END
END

--uspGetPurchasePackage '0'