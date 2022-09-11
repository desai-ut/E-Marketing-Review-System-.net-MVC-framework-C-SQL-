use dbEMRS
go
create table dbo.StateMst
(
    StateID tinyint primary key identity(1,1),
	StateName varchar(20) not null
)

create table dbo.CategoryMst	
(
	CategoryID tinyint primary key identity(1,1),
	CategoryDisc varchar(10) not null
)

create table dbo.QueryMst
(
	QueryID int primary key identity(1,1),
	QueryDisc varchar(5000),
	EmailID varchar(50) unique not null,
	ReplyStatus bit 
)

create table dbo.EmployeeMst
(	
	EmpID tinyint primary key identity(1,1),
	EmpName varchar(50) not null,
	Gender bit not null,
	EmpAddress varchar(500) not null,
	MobileNo varchar(13) not null,
	EmailID varchar(50) unique not null,
	DtBirth date,
	JoiningDate date not null,
	Designation varchar(15) not null,
	Passwd binary(20),
	IsActive bit,
	ActiveLeaveDt date,
	EmpImage varchar(20),
	LastLoginDt date
)

create table dbo.ClientMst
(
	ClientID smallint primary key identity(1,1),
	CompName varchar(50) not null,
	CompAddress varchar(500) not null,
	OwnerName varchar(50) not null,
	CompanyLogo varchar(20),
	ContactNo varchar(12) not null,
	MobileNo varchar(13),
	GSTNo varchar(50),
	WebsiteName varchar(50),
	EmailID varchar(50) unique not null,
	Passwd binary(20),
	IsActive bit,
	ActiveLeaveDt date,
	LastLoginDt date,
	ApprovedEmpID tinyint foreign key references EmployeeMst(EmpID) 
)

create table dbo.CityMst
(
	StateID tinyint foreign key references StateMst(StateID),
	CityID tinyint,
	CityName varchar(20) not null,
	primary key (StateID,CityID)
)

create table dbo.PackageMst
(
	PackageID tinyint primary key identity(1,1),
	PackageTitle varchar(30) not null,
	Discription  varchar(500) not null,
	Price smallmoney not null,
	Duration date not null,
	NoOfProduct tinyint not null,
	NoOfSurveyQues tinyint,
	NoOfReviewQues tinyint,
	NoOfResponse tinyint,
	IsActive bit,
	EmpID tinyint foreign key references EmployeeMst(EmpID)
)

create table dbo.PurchasePackage
(
	PackageID tinyint foreign key references PackageMst(PackageID),
	ClientID smallint foreign key references ClientMst(ClientID),
	PurchaseID smallint primary Key identity(1,1),
	Price smallmoney not null,
	StartingDate date not null,
	EndingDate date not null,
	PaidStatus bit,
	RefNo varchar(20),
	PaidDate date
)

create table dbo.ProductMst
(
	ClientID smallint foreign key references ClientMst(ClientID),
	ProductID smallint,
	ProductName varchar(50) not null,
	ProductCompany varchar(50),
	ModelName varchar(20),
	ShortDiscription varchar(1500),
	BriefDiscription varchar(5000),
	Facility varchar(1000),
	Price smallmoney,
	ProductImage1 varchar(20),
	ProductImage2 varchar(20),
	ProductImage3 varchar(20),
	ProductImage4 varchar(20),
	Video varchar(20),
	TotalSurveyAmt smallmoney,
	SurveyPerAmount smallmoney,
	NoOfSurvey smallint,
	IsActive bit,
	SurveyRewardPoint smallint,
	IsPaid bit,
	primary key(ClientID,ProductID)
)

create table dbo.SurveyMst
(
	ClientID smallint foreign key references ClientMst(ClientID),
	ProductID smallint ,
	SurveyID smallint primary key identity(1,1),
	SurveyDt date,
	SurveyCnt smallint,
	IsPublished bit,
	foreign key (ClientID,ProductID) references ProductMst(ClientID,ProductID)
)

create table dbo.SurveyQues
(
	SurveyID smallint foreign key references SurveyMst(SurveyID),
	CategoryID tinyint foreign key references CategoryMst(CategoryID),
	QuestionID smallint,
	Question varchar(5000) not null,
	Answer1 varchar(2000) not null,
	Answer2 varchar(2000),
	Answer3 varchar(2000),
	Answer4 varchar(2000),
	Answer5 varchar(2000),
	Answer6 varchar(2000),
	Answer7 varchar(2000),
	primary key(SurveyID,CategoryID,QuestionID)
)

create table dbo.ReviewQues
(
	SurveyID smallint foreign key references SurveyMst(SurveyID),
	CategoryID tinyint foreign key references CategoryMst(CategoryID),
	QuestionID smallint,
	Question varchar(5000) not null, 
	primary key(SurveyID,CategoryID,QuestionID)
)

create table dbo.UserMst
(
	UserID smallint primary key identity(1,1),
	UserName varchar(50) not null,
	Gender bit not null,
	UserAddress varchar(500) not null,
	CityID tinyint,
	StateID tinyint foreign key references StateMst(StateID),
	MobNo varchar(13) not null,
	EmailID varchar(50) unique not null,
	DtBirth date,
	JoiningDate date,
	Passwd binary(20),
	IsActive bit,
	ActiveLeaveDate date,
	UserImage varchar(20),
	LastLoginDt date,
	RedeemRewardPoint smallint,
	ApprovedEmpID tinyint,
    foreign key (StateID,CityID) references CityMst(StateID,CityID)
)

create table dbo.SurveyAns
(
	SurveyID smallint foreign key references SurveyMst(SurveyID),
	CategoryID tinyint foreign key references CategoryMst(CategoryID),
	QuestionID smallint,
	UserID smallint foreign key references UserMst(UserID),
	Answer tinyint not null,  
	primary key(SurveyID,CategoryID,QuestionID,UserID),
	foreign key (SurveyID,CategoryID,QuestionID) references SurveyQues(SurveyID,CategoryID,QuestionID)
)

create table dbo.ReviewAns
(
	SurveyID smallint foreign key references SurveyMst(SurveyID),
	CategoryID tinyint foreign key references CategoryMst(CategoryID),
	QuestionID smallint,
	UserID smallint foreign key references UserMst(UserID),
	Answer varchar(5000) not null, 
	primary key(SurveyID,CategoryID,QuestionID,UserID),
	foreign key (SurveyID,CategoryID,QuestionID) references ReviewQues(SurveyID,CategoryID,QuestionID)
)

create table dbo.EwalletMst
(
	SurveyID smallint foreign key references SurveyMst(SurveyID),
	UserID smallint foreign key references UserMst(UserID),
	NoOfAttemptedSurveyQues tinyint,
	NoOfAttemptedReviewQues tinyint,
	IsApproved bit,
	RewardPoint smallint,
	primary key(SurveyID,UserID)
)

create table dbo.TransferAmount
(
	UserID smallint foreign key references UserMst(UserID),
	TAID int primary key identity(1,1),
	TADate date,
	TotalTransferRewardPoint smallint,
	TotalTransferAmount smallint,
	TransferTo varchar(20),
	TransferToUserName varchar(50),
	RefNo varchar(20)
)

create table dbo.SurveyPayment
(
	SPID int primary key identity(1,1),
	SPDate date,
	SurveyID smallint foreign key references SurveyMst(SurveyID),
	TotalAmt smallint,
	RefNo varchar(20)
)

create table dbo.FeedbackMst
(
	FeedbackID int primary key identity(1,1),
	SurveyID smallint foreign key references SurveyMst(SurveyID),
	FeedbackDisc varchar(5000),
	Ratings tinyint not null,
	UserEmail  varchar(50) not null,
	UserContactNo varchar(13)
)

create table dbo.HelpingQues
(
    CategoryID tinyint foreign key references CategoryMst(CategoryID),
	QuestionID smallint,
	Question varchar(5000) not null,
	Answer1 varchar(2000),
	Answer2 varchar(2000),
	Answer3 varchar(2000),
	Answer4 varchar(2000),
	Answer5 varchar(2000),
	Answer6 varchar(2000),
	Answer7 varchar(2000),
	primary key(CategoryID,QuestionID)
)

