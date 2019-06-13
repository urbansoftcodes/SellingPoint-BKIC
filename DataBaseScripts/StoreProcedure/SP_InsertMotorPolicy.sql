USE SellingPoint
GO
/****** Object:  StoredProcedure [dbo].[InsertMotorPolicy]    Script Date: 6/15/2018 2:37:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Alter procedure [dbo].[InsertMotorPolicy]
(
@InsuredCode varchar(50),/*Total Params 24*/
@DOB Datetime,
@YearOfMake int,
@VehicleMake varchar(20),
@VehicleModel varchaR(25),
@vehicleTypeCode varchar(20),
@PolicyCode varchar(50),
@IsNCB bit,
@NCBFromDate Datetime,
@NCBToDate datetime,
@VehicleSumInsured decimal(38,3),
@PremiumAmount decimal(18,3),
@PolicyCommenceDate datetime,
@RegistrationNumber int,
@ChassisNo varchar(18),
@EngineCC int,
@FinancierCompanyCode varchar(50),
@ExcessType varchar(25),
@Branch varchar(50),
@Createdby varchar(100),
@IsSaved bit,
@AgencyCode varchar(50),
@Agency varchar(50),
@PaymentAuthorization varchar(50),
@TransactionNo varchar(50),
@PaymentType varchar(50),
@MotorId int out,
@IsHIR bit out

)
as
begin
Declare @DocumentNo nvarchar(50),@SubClass nvarchar(20),@LinkID	nvarchar(250),@PolicyExpiryDate datetime,@Rate decimal(18,8),@Financier nvarchar(50)
Declare @ExcessAmount decimal(38,3),@CPR nvarchar(50),@Mobile nvarchar(50),@HIRStatus bit,@InsuredAmountLimit decimal,
@HIRReasonCode nvarchar(50),@HIRReason nvarchar(max)
set @HIRStatus=0
set @IsHIR=0
/* excess Amount calculation*/
--exec [dbo].[ExcessCalculation] @VehicleMake,@VehicleModel,@vehicleTypeCode,@ExcessType,@ExcessAmount out

/* HIR Validation*/
Declare @CurrentYear int,@YearDiffernce int
set @CurrentYear=YEAR(getdate())
set @YearDiffernce=@CurrentYear-@YearOfMake
if(@YearDiffernce>5)
begin
	set @HIRStatus=1
	set @IsHIR=1
	set @HIRReasonCode= coalesce('9' + ',', '') 
	set @IsSaved=0
end
select @InsuredAmountLimit= AmountLimit from BK_InsuraceMaxInsuredAmount with(nolock) where InsuranceType='Motor'
if(@VehicleSumInsured>@InsuredAmountLimit)
begin
	set @HIRStatus=1
	set @IsHIR=1
	set @HIRReasonCode= cast(ISNULL(@HIRReasonCode,'') as nvarchar(50))+coalesce('2' + ',', '')
	set @IsSaved=0
end

if(@vehicleTypeCode='USED' and @IsNCB=0)
begin
	set @HIRStatus=1
	set @IsHIR=1
	set @HIRReasonCode=cast(ISNULL(@HIRReasonCode,'') as nvarchar(50))+coalesce('1' + ',', '')
	set @IsSaved=0
end
declare @a nvarchar(25)
set @a= SUBSTRING(@HIRReasonCode,1,DATALENGTH(@HIRReasonCode)-1)

select @HIRReason= coalesce(@HIRReason + ',', '') + cast(statusdescription as nvarchar(250)) from BK_HIRReason where StatusID in (SELECT Value FROM fn_Split(@HIRReasonCode, ','))

select @HIRReason as reason
/*Expiry Calculation*/
if(@vehicleTypeCode='USED')
	begin
		set @PolicyExpiryDate= dateadd(YY, 1,@PolicyCommenceDate-1)
	end
else
	begin
		DECLARE @dtDate DATETIME
		set @PolicyExpiryDate= DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@PolicyCommenceDate)+1,0))
		set @PolicyExpiryDate= dateadd(YY, 1,@PolicyExpiryDate)
	end

	if(@IsNCB=1)
	begin
	declare @NCByears int
	set @NCByears=CONVERT(int,DATEDIFF(YEAR,@NCBFromDate , @NCBToDate) )
	select @NCByears
	end
	else
	begin
		set @NCByears=0
	end
if(@vehicleTypeCode='Used')
	begin
		select @Rate=UsedVehicle from BK_MotorInsuranceRate with(nolock) where Code=@PolicyCode
	end
else
	begin
		select @Rate=NewVehicle from BK_MotorInsuranceRate with(nolock) where Code=@PolicyCode
	end

EXEC GetDocumentNumber 'MotorInsuranceDocNoFormat',@DocumentNo out
EXEC SP_GetPolicyLinkId @LinkID out


select @InsuredName=A.INSUREDNAME,@CPR=A.CPR,@Mobile=B.TELEPHONEMOBILE from InsuredMaster A with(nolock) 
where A.INSUREDCODE=@InsuredCode
	
select @Financier=Financier from BK_MotorFinancier with(nolock) where Code=@FinancierCompanyCode


Declare @Mainclass nvarchar(25),@DiscountAmount decimal(18,3),@DiscountPercent decimal(18,2)


set @Mainclass='PMTR'
set @MotorId=0


/*Motor Cover*/
IF OBJECT_ID('tempdb..#MotorCover') IS NOT NULL
  /*Then it exists*/
  DROP TABLE #MotorCover

Create table #MotorCover(MOTORID int,DOCUMENTNO nvarchar(50),COVERCODE nvarchar(50),COVERAMOUNT decimal(38,3),
MAINCLASS nvarchar(50),SUBCLASS nvarchar(50),PERCENTAGE decimal(38,6),FORMULATYPE nvarchar(50),FORMULAID nvarchar(50),TYPE nvarchar(50),
TYPESERIALNO int identity(1,1), COVERCODEDESCRIPTION nvarchar(50),CREATEDBY int,CREATEDDATE datetime,UPDATEDBY int,UPDATEDDATE datetime,LINKID nvarchar(250),REGISTRATIONNO nvarchar(50),VEHICLETYPE nvarchar(50))

Insert into #MotorCover(MOTORID,DOCUMENTNO,COVERCODE,COVERAMOUNT,MAINCLASS,SUBCLASS,PERCENTAGE,FORMULATYPE,FORMULAID,TYPE,
COVERCODEDESCRIPTION,CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,LINKID,REGISTRATIONNO,VEHICLETYPE)
	select @MotorId as MOTORID,@DocumentNo as DOCUMENTNO,COVERCODE,0 as COVERAMOUNT ,@Mainclass as MAINCLASS,@PolicyCode as SUBCLASS,0 as PERCENTAGE,TYPE as FORMULATYPE,
COVERCODE as FORMULAID,TYPE,COVERCODEDESCRIPTION,@Createdby as CREATEDBY,GETDATE() as  CREATEDDATE,
0 as UpdatedBy,GETDATE() as  UPDATEDDATE, @LinkID as  LINKID,@RegistrationNumber as REGISTRATIONNO,
@vehicleTypeCode as VEHICLETYPE from BK_MotorCoverCodes where 
COVERCODE in (select * from MotorProductCover)



/*Base Premium */
Declare @BasePremium decimal(38,3)
Declare @VehicleRate decimal
if(@vehicleTypeCode='New')
begin
	select @VehicleRate=NewVehicle from BK_MotorInsuranceRate with(nolock) where Code=@PolicyCode
end
else
begin
	select @VehicleRate=UsedVehicle from BK_MotorInsuranceRate with(nolock) where Code=@PolicyCode
end

set @BasePremium=@VehicleSumInsured * (@VehicleRate/100)

	
	Insert into BK_Motor(DOCUMENTNO,REGISTRATIONNO,VEHICLETYPE,MAKE,MODEL,YEAR,ENGINENO,CHASSISNO,VEHICLEVALUE,
GROSSPREMIUM,FINANCECOMPANY,MAINCLASS,SUBCLASS,FINANCECOMPANYDESCRIPTION,COMMENCEDATE,EXPIRYDATE,EXCESSAMOUNT,EXCESSTYPE,
INSUREDCODE,NCBFROMDATE,NCBTODATE,NCBYEARS,RATE,BASEPREMIUM,INSUREDNAME,RENEWAL,RENEWED,RENEWALCOUNT
,CPR,TRANSACTION_NO,PAYMENTDATE,PAYMENT_TYPE,PAYMENT_AUTHORIZATION_CODE,
ORIGINALPREMIUMAMOUNT,
MOBILENUMBER,SOURCE,CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,IsHIR,LINKID,IsSaved,HIRReason,HIRStatus) 
values(@DocumentNo,@RegistrationNumber,@vehicleTypeCode,@VehicleMake,
@VehicleModel,@YearOfMake,@EngineCC,@ChassisNo,@VehicleSumInsured,@PremiumAmount,@FinancierCompanyCode,@Mainclass,@PolicyCode,
 @Financier,@PolicyCommenceDate,
@PolicyExpiryDate,@ExcessAmount,@ExcessType,@InsuredCode,@NCBFromDate,@NCBToDate,@NCByears,@Rate,@BasePremium,@InsuredName,
'N','N',0,@CPR,@TransactionNo,@PaymentType,@PaymentAuthorization,@PremiumAmount,
@Mobile,@Source,@Createdby,GETDATE(),0,'',@IsHIR,@LinkID,@IsSaved,@HIRReason,@HIRStatus)

set @MotorId=SCOPE_IDENTITY()
select @MotorId
update #MotorCover set MOTORID=@MotorId

--select * from #MotorCover
	insert into BK_MotorCover(MOTORID,DOCUMENTNO,COVERCODE,COVERAMOUNT,MAINCLASS,SUBCLASS,PERCENTAGE,FORMULATYPE,FORMULAID,TYPE,TYPESERIALNO,
COVERCODEDESCRIPTION,CREATEDBY,CREATEDATE,UPDATEDBY,UPDATEDDATE,LINKID,REGISTRATIONNO,VEHICLETYPE) select MOTORID,DOCUMENTNO,COVERCODE,COVERAMOUNT,MAINCLASS,SUBCLASS,PERCENTAGE,FORMULATYPE,FORMULAID,TYPE,TYPESERIALNO,
COVERCODEDESCRIPTION,CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,LINKID,REGISTRATIONNO,VEHICLETYPE from #MotorCover

	/*Motor Loads*/
	IF OBJECT_ID('tempdb..#MotorLoads') IS NOT NULL
  /*Then it exists*/
  DROP TABLE #MotorLoads

	Create table #MotorLoads(MOTORID int,DOCUMENTNO nvarchar(50),LOADCODE nvarchar(50),LOADPERCENT decimal(38,6),
	LOADDESCRIPTION nvarchar(50),LOADAMOUNT decimal(38,3),FORMULATYPE nvarchar(50),FORMULAID nvarchar(50),
	TRANSACTIONBASEPREMIUM decimal(38,3),CREATEDBY int,CREATEDDATE datetime,
		UPDATEDBY int,UPDATEDDATE datetime,LINKID nvarchar(250),REGISTRATIONNO nvarchar(50),VEHICLETYPE nvarchar(50))

		insert into #MotorLoads (MOTORID,DOCUMENTNO,LOADCODE,LOADPERCENT,LOADDESCRIPTION,LOADAMOUNT,FORMULATYPE,FORMULAID,TRANSACTIONBASEPREMIUM,
		CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,LINKID,REGISTRATIONNO,VEHICLETYPE)
		 select @MotorId,@DocumentNo,COVERCODE,0,COVERCODEDESCRIPTION,0,
		[Type],COVERCODE,0,@Createdby,GETDATE(),0,GETDATE(),@LinkID,@RegistrationNumber,@vehicleTypeCode
		 From BK_MotorCoverCodes where COVERCODE in ('EXCESS_L','DRIVERAGE')

	/*Age Loading*/
 Declare @Age int
 Declare @AgeLoading decimal(38,3)
 Set @Age=convert(int,DATEDIFF(d, @DOB, getdate())/365.25) 
	Declare @AgePercent decimal(18,3)
 set @AgePercent=25
 if(@Age<25) 
	begin
		set  @AgeLoading=(@BasePremium)*(@AgePercent/100)
		update #MotorLoads set LOADPERCENT=@AgePercent,LOADAMOUNT=@AgeLoading,TRANSACTIONBASEPREMIUM=@PremiumAmount where 
		LOADCODE='DRIVERAGE'

	end
--if(@ExcessType='None')
--begin
--	update #MotorLoads set LOADPERCENT=@ExcessPercent,LOADAMOUNT=@ExcessAdditionAmount,TRANSACTIONBASEPREMIUM=@PremiumBeforeDiscount where 
--		LOADCODE='EXCESS_L'
--end

Insert into BK_MotorLoads(MOTORID,DOCUMENTNO,LOADCODE,LOADPERCENT,LOADDESCRIPTION,LOADAMOUNT,FORMULATYPE,FORMULAID,TRANSACTIONBASEPREMIUM,
		CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,LINKID,REGISTRATIONNO,VEHICLETYPE) select MOTORID,DOCUMENTNO,LOADCODE,LOADPERCENT,LOADDESCRIPTION,LOADAMOUNT,FORMULATYPE,FORMULAID,TRANSACTIONBASEPREMIUM,
		CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,LINKID,REGISTRATIONNO,VEHICLETYPE from #MotorLoads


	IF OBJECT_ID('tempdb..#MotorCover') IS NOT NULL
  DROP TABLE #MotorCover

	IF OBJECT_ID('tempdb..#MotorLoads') IS NOT NULL
  DROP TABLE #MotorLoads
end










select * from MotorCoverMaster