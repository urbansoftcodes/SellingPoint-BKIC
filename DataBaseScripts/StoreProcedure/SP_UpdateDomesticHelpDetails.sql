

alter procedure [dbo].[SP_UpdateDomesticHelpDetails]
(
@InsuredCode nvarchar(50),
@InsuredName nvarchar(50),
@CPR nvarchar(50),
@InsurancePeroid int,
@NumberOfDomesticWorkers int,
@PolicyStartDate datetime,
@MobileNumber nvarchar(50),
@Createdby int,
@dt as DomesticHelpMemberDetailsDataTable readonly,
@IsSaved bit,
@DomesticID int,
@LoadAmount decimal(18,3),
@DiscountAmount decimal(18,3),
@Remarks nvarchar(max),
@PremiumBeforeDiscount decimal(18,3),
@PremiumAfterDiscount decimal(18,3),
@IsHIR int out
)
as
begin
Declare @LinkID nvarchar(50),@DocumentNo nvarchar(50),@SumInsured decimal,@MainClass nvarchar(25),@SubClass nvarchar(25),
@PolicyExpiryDate datetime,@HIRStatus bit,
@Address1 nvarchar(50),@FlatNo nvarchar(25),@RoadNo nvarchar(25),@BlockNo nvarchar(25), @BuildingNo nvarchar(25),@CityCode nvarchar(25)

set @IsHIR=0
Set @MainClass='LIAB'
set @SubClass='PADN'
set @PolicyExpiryDate= dateadd(YY, @InsurancePeroid,@PolicyStartDate-1)
select * from InsuredMaster
select @FlatNo=Flat,@BuildingNo=@BuildingNo,@RoadNo=@RoadNo,@BlockNo=Block from InsuredMaster where INSUREDCODE=@InsuredCode

set @Address1=@FlatNo+','+@BuildingNo+ ','+@RoadNo+','+@CityCode

select @DocumentNo=@DocumentNo,@LinkID=LinkID from DomesticHelp where DOMESTICID=@DomesticID

update DomesticHelp set PREMIUMAMOUNT=@PremiumAfterDiscount,EXPIRYDATE=@PolicyExpiryDate,ADDRESS1=@Address1,DATEOFSUBMISSION=GETDATE(),
COMMENCEDATE=@PolicyStartDate,INSURANCEPERIOD=@InsurancePeroid,DISCOUNTAMOUNT=@PremiumAfterDiscount,UPDATEDBY=@Createdby,
UPDATEDDATE=GETDATE(),LOADAMOUNT=@LoadAmount where DOMESTICID=@DomesticID and INSUREDCODE=@InsuredCode

Create table #DomesticHelpMemberDetails( DOMESTICID int,DOCUMENTNO nvarchar(50), INSUREDCODE nvarchar(50), INSUREDNAME NVARCHAR(50),
  SUMINSURED decimal,PREMIUMAMOUNT decimal(38,3), EXPIRYDATE datetime,REMARKS nvarchar(max),TYPE nvarchar(50), ADDRESS1 nvarchar(50),
  OCCUPATION nvarchar(50),NATIONALITY nvarchar(50),PASSPORT nvarchar(50),DOB date,SEX char,ITEMSERIALNO int,MAINCLASS nvarchar(50),
  SUBCLASS nvarchar(50), DATEOFSUBMISSION datetime,COMMENCEDATE date,IDENTITYNO nvarchar(50), OCCUPATIONOTHER nvarchar(50),
  CREATEDBY int,CREATEDDATE datetime,UPDATEDBY int,UPDATEDDATE datetime,LINKID varchar(250))

insert into #DomesticHelpMemberDetails(INSUREDCODE ,INSUREDNAME ,SUMINSURED,PREMIUMAMOUNT,EXPIRYDATE,
  ADDRESS1,OCCUPATION ,NATIONALITY,PASSPORT,DOB ,SEX ,ITEMSERIALNO,MAINCLASS,SUBCLASS ,
  DATEOFSUBMISSION,COMMENCEDATE,IDENTITYNO ,OCCUPATIONOTHER ,CREATEDBY ,CREATEDDATE)
select INSUREDCODE ,INSUREDNAME ,SUMINSURED,PREMIUMAMOUNT,EXPIRYDATE,
  ADDRESS1,OCCUPATION ,NATIONALITY,PASSPORT,DOB ,SEX ,ITEMSERIALNO,MAINCLASS,SUBCLASS ,
  DATEOFSUBMISSION,COMMENCEDATE,IDENTITYNO ,OCCUPATIONOTHER ,CREATEDBY ,CREATEDDATE  from @dt

update #DomesticHelpMemberDetails set DOCUMENTNO=@DocumentNo,LINKID=@LinkID,DOMESTICID=@DomesticID,MAINCLASS=@MainClass,
SUBCLASS=@SubClass,EXPIRYDATE=@PolicyExpiryDate,CREATEDDATE=GETDATE(),SUMINSURED=@SumInsured,PREMIUMAMOUNT=@PremiumAfterDiscount

/* Delete domestichelpmembers*/
delete DomesticHelpMemberDetails where DOMESTICID=@DomesticID 

Insert into DomesticHelpMemberDetails(DOMESTICID,DOCUMENTNO,INSUREDCODE ,INSUREDNAME ,SUMINSURED,PREMIUMAMOUNT,EXPIRYDATE,
  REMARKS,TYPE ,ADDRESS1,OCCUPATION ,NATIONALITY,PASSPORT,DOB ,SEX ,ITEMSERIALNO,MAINCLASS,SUBCLASS ,
  DATEOFSUBMISSION,COMMENCEDATE,IDENTITYNO ,OCCUPATIONOTHER ,CREATEDBY ,CREATEDDATE ,UPDATEDBY ,
  UPDATEDDATE ,LINKID)
select DOMESTICID,DOCUMENTNO,INSUREDCODE ,INSUREDNAME ,SUMINSURED,PREMIUMAMOUNT,EXPIRYDATE,
  REMARKS,TYPE ,ADDRESS1,OCCUPATION ,NATIONALITY,PASSPORT,DOB ,SEX ,ITEMSERIALNO,MAINCLASS,SUBCLASS ,
  DATEOFSUBMISSION,COMMENCEDATE,IDENTITYNO ,OCCUPATIONOTHER ,CREATEDBY ,CREATEDDATE ,UPDATEDBY ,
  UPDATEDDATE ,LINKID  from #DomesticHelpMemberDetails
end
