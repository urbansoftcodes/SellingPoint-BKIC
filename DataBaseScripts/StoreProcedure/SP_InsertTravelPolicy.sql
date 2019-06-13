
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_InsertTravelPolicyDetails]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_InsertTravelPolicyDetails]
go
Create procedure [dbo].[SP_InsertTravelPolicyDetails]
(
@DOB datetime,
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
@DiscountAmount decimal(18,0),
@CPR nvarchar(50),
@MobileNumber nvarchar(50),
@FFPNumber nvarchar(20),
@QuestionaireCode nvarchar(25),/* newly added*/
@IsPhysicalDefect nvarchar(10),/* pas Yes or NO*/
@PhysicalDescription nvarchar(250),
@CreatedBy int,
@dt As dbo.TravelMembersDataTable readonly,
@IsSaved bit,
@CoverageType nvarchar(500),
@Source nvarchar(25),
@TravelId int out,
@HIRStatusMessage nvarchar(50) out,
@IsHIRStatus bit out
)
as 
begin
Declare @DocumentNo nvarchar(50),@SubClass nvarchar(20),@LinkID	nvarchar(250),@HIRStatus int,@FamilyMemberDOB date,
@ForeignSumInsured decimal,@HIRReasonID nvarchar(50),@HIRReason nvarchar(max)

 Declare @Age int
 Declare @AgeLoading decimal(38,3)
 set @IsHIRStatus=0
 set @HIRStatus=0
  Set @Age=convert(int,DATEDIFF(d, @DOB, getdate())/365.25) 
 
 if(@PackageCode='FM001')
begin
set @MainClass='MISC'
set @SubClass='FAMIL'
end
if(@PackageCode='IN001')
begin
set @MainClass='MISC'
set @SubClass='PEARL'
end

 exec CalculatePremiumTravel @PackageCode,@PeroidOfCoverCode,@DOB,@PremiumAmount out,@DiscountAmount out

  Declare @DiscountPercent decimal(18,2),@DiscountDiffAmount decimal(18,3)
select @DiscountPercent=DiscountPercent from BK_InsuraceDiscounts where InsuranceType='Travel' and Source=@Source and SubClass=@SubClass
set @DiscountDiffAmount=(@PremiumAmount*@DiscountPercent/100)


 if(@Age>80)
 begin
	set @IsHIRStatus=1
	set @HIRStatus=1
	set @HIRStatusMessage='The form cannot be processed'
	set @IsSaved=0
	set @HIRReasonID=coalesce('10' + ',', '')
 end
 if(@IsPhysicalDefect='Yes')
 begin
 set @IsHIRStatus=1
	set @HIRStatus=1
	set @IsSaved=0
	set @HIRReasonID=cast(ISNULL(@HIRReasonID,'') as nvarchar(50))+coalesce('11' + ',', '')
	set @HIRStatusMessage='The form cannot be processed'
 end
 select @HIRReason= coalesce(@HIRReason + ',', '') + cast(statusdescription as nvarchar(250)) from BK_HIRReason where StatusID
  in (SELECT Value FROM fn_Split(@HIRReasonID, ','))

EXEC GetDocumentNumber 'TravelInsuranceDocNoFormat',@DocumentNo out
EXEC GetPolicyLinkId @LinkID out


select @SumInsured=SumInsuredBHD,@ForeignSumInsured=SumInsuredUSD from [dbo].BK_SumInsuredType where InsuranceType='TravelInsurance' 
Declare @ExpiryDate datetime
set @ExpiryDate= dateadd(YY, @PolicyPeroidYears,@InsuranceStartDate-1)



Insert into Travel(DOCUMENTNO,INSUREDCODE,INSUREDNAME,SUMINSURED,PREMIUMAFTERDISCOUNT,COMMENCEDATE,EXPIRYDATE,MAINCLASS,SUBCLASS,
DATEOFSUBMISSION,OTHERINSUREDNAME,PASSPORT,RENEWAL,PREVIOUSDOCUMENTNO,OCCUPATION,RENEWED,PERIODOFCOVER,FFPNUMBER,RENEWALCOUNT,
LASTYEARSUMINSURED,LASTYEARPREMIUMAMOUNT,LASTYEAREXPIRYDATE,LOADAMOUNT,DISCOUNTAMOUNT,CODE,REMARK,CPR,TRANSACTION_NO,PAYMENTDATE,
PAYMENT_TYPE,PAYMENT_AUTHORIZATION_CODE,PREMIUMBEFOREDISCOUNT,MOBILENUMBER,CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,
IsHIR,LINKID,HIRStatus,IsSaved,DiscountPercent,HIRReason,Source,CoverageType) values(
@DocumentNo,@InsuredCode,@InsuredName,@ForeignSumInsured,@DiscountAmount,@InsuranceStartDate,@ExpiryDate,@MainClass,@SubClass,null,
'',@PassportNumber,@Renewal,'',@Occupation,@Renewal,@PeroidOfCoverCode,@FFPNumber,0,0,0,null,0,@DiscountDiffAmount,'','',@CPR,'',null,'','',@PremiumAmount,@MobileNumber,@CreatedBy,GETDATE(),@CreatedBy,
GETDATE(),@IsHIRStatus,@LinkID,@HIRStatus,@IsSaved,@DiscountPercent,@HIRReason,@Source,@CoverageType)

  if object_id('tempdb..#TravelMemberDetails') is not null
	 drop table #TravelMemberDetails

set @TravelId=SCOPE_IDENTITY();
create Table #TravelMemberDetails
(
  TRAVELID INT,
  DOCUMENTNO nvarchar(50),
  ITEMSERIALNO int,
  ITEMNAME NVARCHAR(50),
  SUMINSURED decimal(38,3),
  FOREIGNSUMINSURED decimal(38,3),
  CATEGORY nvarchar(50),
  TITLE nchar(10),
  SEX nchar(10),
  DATEOFBIRTH date,
  AGE int,
  PREMIUMAMOUNT decimal(38,3),
  MAKE nvarchar(50),
  OCCUPATIONCODE nvarchar(50),
  CPR nvarchar(50),
  PASSPORT nvarchar(50),
  FIRSTNAME nvarchar(50),
  MIDDLENAME nvarchar(50),
  LASTNAME nvarchar(50),
  CREATEDBY int,
  CREATEDDATE datetime,
  UPDATEDBY int,
  UPDATEDDATE datetime,
  LINKID varchar(250)

)

Insert into #TravelMemberDetails(TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID)  select TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID from @dt
update #TravelMemberDetails set TRAVELID=@TravelId,SUMINSURED=@SumInsured ,FOREIGNSUMINSURED=@ForeignSumInsured ,LINKID=@LinkID,DOCUMENTNO=@DocumentNo

insert into TravelMembers(TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID) select TRAVELID,DOCUMENTNO,ITEMSERIALNO,ITEMNAME,SUMINSURED,FOREIGNSUMINSURED,CATEGORY,
TITLE,SEX,DATEOFBIRTH,AGE,PREMIUMAMOUNT,MAKE,OCCUPATIONCODE,CPR,PASSPORT,FIRSTNAME,MIDDLENAME,LASTNAME,CREATEDBY,CREATEDDATE,
UPDATEDBY,UPDATEDDATE,LINKID from #TravelMemberDetails

/* Questionaire */

Declare @Type varchar(50),@Description nvarchar(max)
select @Type=TYPE,@Description=DESCRIPTION from BK_Questionnaire where CODE=@QuestionaireCode
 Insert into BK_Questionnaire(DOCUMENTNO,TYPE,CODE,DESCRIPTION,ANSWER,REFERRAL,REMARKS,ITEMSERIALNO,ITEMNAME,CPR,
  EXISTINGILLNESSNAME,SUFFERINGSINCE,MEDICALCONDITION,STATUS,CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,LINKID)
  values(@DocumentNo,@Type,@QuestionaireCode, @Description,@IsPhysicalDefect,'',@PhysicalDescription,1,@InsuredName,@CPR,'','','','',@CreatedBy,GETDATE(),
  @CreatedBy,GETDATE(),@LinkID)

  if object_id('tempdb..#TravelMemberDetails') is not null
	 drop table #TravelMemberDetails

end

go
 


