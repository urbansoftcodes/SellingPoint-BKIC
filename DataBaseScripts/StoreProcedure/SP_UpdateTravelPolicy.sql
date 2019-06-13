USE SellingPoint
GO
/****** Object:  StoredProcedure [dbo].[UpdateTravelPolicy]    Script Date: 6/5/2018 12:28:46 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter procedure [dbo].[SP_UpdateTravelPolicy]
(
@TravelID int,
@PackageCode nvarchar(25),/*Code*/
@PolicyPeroidYears int,
@InsuredCode nvarchar(50),
@InsuredName nvarchar(50),
@SumInsured decimal(38,3),/* USD*/
@PremiumAmount decimal(38,3),
@InsuranceStartDate datetime,
@MainClass nvarchar(50),
@PassportNumber nvarchar(50),
@Renewal char,
@Occupation nvarchar(50),
@PeroidOfCoverCode nvarchar(10),
@DiscountPremiumAmount decimal(38,0),
@CPR nvarchar(50),
@MobileNumber nvarchar(50),
@FFPNumber nvarchar(20),
@QuestionaireCode nvarchar(25),/* newly added*/
@IsPhysicalDefect nvarchar(10),/* pas Yes or NO*/
@PhysicalDescription nvarchar(250),
@CreatedBy int,
@dt As dbo.TravelMembersDataTable readonly,
@IsSaved bit,
@LoadAmount decimal(18,3),
@DiscountAmount decimal(18,3),
@Remarks nvarchar(max),
@CoverageType nvarchar(200),
@IsHIR bit out
)
as
begin

 Declare @Age int,@IsHIRStatus bit,@HIRStatus int,@ExpiryDate datetime,@LinkID nvarchar(50),@DocumentNo nvarchar(50),
 @FamilyMemberDOB date,@Subclass nvarchar(25),@DOB datetime
 set @IsHIR=0

 select @DOB=DATEOFBIRTH from [dbo].InsuredMaster where INSUREDCODE=@InsuredCode
 Set @Age=convert(int,DATEDIFF(d, @DOB, getdate())/365.25) 
 if(@Age>80)
 begin
	set @IsHIR=1
	set @HIRStatus=1

 end
  if(@IsPhysicalDefect='Yes')
 begin
 set @IsHIRStatus=1
	set @HIRStatus=1
	
 end

 Declare @AgeLoading decimal(38,3)

  Set @Age=convert(int,DATEDIFF(d, @DOB, getdate())/365.25) 
 if(@PackageCode='FM001')
 begin
	select @FamilyMemberDOB=DATEOFBIRTH from @dt where DATEOFBIRTH=(select max(DATEOFBIRTH) from @dt) 
	Declare @MaxMemberAge int
	set @MaxMemberAge=convert(int,DATEDIFF(d, @FamilyMemberDOB, getdate())/365.25) 
	if(@MaxMemberAge>@Age)
	begin
		set @Age=@MaxMemberAge
		set @DOB=@FamilyMemberDOB
	end
 end
 select @SumInsured=USDAmount from [dbo].[SumInsuredUSD] where InsuranceType='Travel' 
set @ExpiryDate= dateadd(YY, @PolicyPeroidYears,@InsuranceStartDate-1)
if(@PackageCode='FM001')
begin
set @SubClass='FAMIL'
end
if(@PackageCode='IN001')
begin
set @SubClass='PEARL'
end



 select @LinkID=LINKID,@DocumentNo=DOCUMENTNO from Travel where TRAVELID=@TravelID 

update Travel set PREMIUMAFTERDISCOUNT=@DiscountPremiumAmount,COMMENCEDATE=@InsuranceStartDate,EXPIRYDATE=@ExpiryDate,
PERIODOFCOVER=@PeroidOfCoverCode,FFPNUMBER=@FFPNumber,PREMIUMBEFOREDISCOUNT=@PremiumAmount,REMARK=@Remarks,
LOADAMOUNT=@LoadAmount,DISCOUNTAMOUNT=@DiscountAmount,
CREATEDBY=@CreatedBy,CREATEDDATE=GETDATE(),UPDATEDBY=@CreatedBy,UPDATEDDATE=GETDATE()
where TRAVELID=@TravelID and INSUREDCODE=@InsuredCode

/* Travel member details*/
  if object_id('tempdb..#TravelMemberUpdateDetails') is not null
	 drop table #TravelMemberUpdateDetails

create Table #TravelMemberUpdateDetails
(TRAVELID INT,DOCUMENTNO nvarchar(50),ITEMSERIALNO int,ITEMNAME NVARCHAR(50),SUMINSURED decimal(38,3),FOREIGNSUMINSURED decimal(38,3),
  CATEGORY nvarchar(50),TITLE nchar(10),SEX nchar(10),DATEOFBIRTH date,AGE int,PREMIUMAMOUNT decimal(38,3),MAKE nvarchar(50),
  OCCUPATIONCODE nvarchar(50),CPR nvarchar(50),PASSPORT nvarchar(50),FIRSTNAME nvarchar(50),MIDDLENAME nvarchar(50),LASTNAME nvarchar(50),
  CREATEDBY int,CREATEDDATE datetime,UPDATEDBY int,UPDATEDDATE datetime,LINKID varchar(250)
)
Insert into #TravelMemberUpdateDetails(TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID)  select TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID from @dt

update #TravelMemberUpdateDetails set TRAVELID=@TravelId,SUMINSURED= @SumInsured,LINKID=@LinkID,DOCUMENTNO=@DocumentNo

delete from TravelMembers where TRAVELID=@TravelID

insert into TravelMembers(TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID) select TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID from #TravelMemberUpdateDetails


  if object_id('tempdb..#TravelMemberDetails') is not null
	 drop table #TravelMemberUpdateDetails

end


