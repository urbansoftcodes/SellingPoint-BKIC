USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[MIG_IntegrateMotorEndorsement]    Script Date: 3/24/2019 12:41:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[MIG_IntegrateMotorEndorsement]
(
 @EndorsementID bigint
)
AS
BEGIN 
BEGIN TRY

declare @InsuredCode nvarchar(50),@DocumentNo nvarchar(50),@EndorsementNo nvarchar(100),
@LinkID nvarchar(250), @Agency nvarchar(50),@CreatedBy int, @AuthorizedBy int

select @DocumentNo=DOCUMENTNO,
       @LinkID=LINKID,
	   @ENDORSEMENTNO = ENDORSEMENTNO, 
	   @CreatedBy = CreatedBy,
	   @AuthorizedBy = UpdatedBy,
       @Agency = AGENCY 
	   from SellingPoint.dbo.MotorEndorsement where MotorEndorsementID=@EndorsementID



DECLARE @CreatedUser NVARCHAR(100)
SELECT @CreatedUser =  UserName FROM SellingPoint.dbo.USER_MASTER WHERE id = @CreatedBy


DECLARE @AuthorizedUser NVARCHAR(100)
SELECT @AuthorizedUser =  UserName FROM SellingPoint.dbo.USER_MASTER WHERE id = @AuthorizedBy


IF NOT ExistS(select * from OrclBKICDB..ADMIN.FROMPOS_MOTORENDORSEMENT WHERE ENDORSEMENTNO  = @ENDORSEMENTNO)
BEGIN

DECLARE @AgentCommissionBeforeDiscount decimal(18,3) = 0;
DECLARE @AgentCommissionAfterDiscount decimal(18,3) = 0;

select  @AgentCommissionBeforeDiscount = CommissionBeforeDiscount,
        @AgentCommissionAfterDiscount = CommissionAfterDiscount
        FROM  
		SellingPoint.dbo.PolicyCategory where MotorEndorsementID = @EndorsementID
		and CODE = 'AGTCOMM'

insert into OrclBKICDB..ADMIN.FROMPOS_MOTORENDORSEMENT
           (ENDORSEMENTTYPE,AGENCY,AGENTCODE,BRANCHCODE,DOCUMENTNO,ENDORSEMENTNO,INSUREDCODE,
		    INSUREDNAME,OLDINSUREDCODE,OLDINSUREDNAME,REGISTRATIONNO,CHASSISNO,OLDREGISTRATIONNO,
			OLDCHASSISNO,VEHICLEVALUE,GROSSPREMIUM,FINANCECOMPANY,MAINCLASS,SUBCLASS,FINANCECOMPANYDESCRIPTION,
			COMMENCEDATE,EXPIRYDATE,EXTENDEDEXPIREDATE,CANCELDATE,REMARKS,PAYMENTDATE,PAYMENT_TYPE,ACCOUNTNO,
			PREMIUMBEFOREDISCOUNT,SOURCE,CREATEDBY,CREATEDDATE,UPDATEDBY,UPDATEDDATE,LINKID,PREMIUMAFTERDISCOUNT,REFUNDAMOUNT,
		    REFUNDAFTERDISCOUNT,COMMISSIONBEFOREDISCOUNT,COMMISSIONAFTERDISCOUNT,CHARGEAMOUNT,DISCOUNTAMOUNT,EXCESSAMOUNT, TAXAMOUNT) 
			 
			 select ENDORSEMENTTYPE,AGENCY,AGENTCODE,BRANCHCODE,DOCUMENTNO,ENDORSEMENTNO,INSUREDCODE,
			        INSUREDNAME,OLDINSUREDCODE,OLDINSUREDNAME,REGISTRATIONNO,CHASSISNO,OLDREGISTRATIONNO,
					OLDCHASSISNO,VEHICLEVALUE,GROSSPREMIUM,FINANCECOMPANY,MAINCLASS,SUBCLASS,FINANCECOMPANYDESCRIPTION,
					COMMENCEDATE,EXPIRYDATE,EXTENDEDEXPIREDATE,CANCELDATE,REMARK,PAYMENTDATE,
					PAYMENT_TYPE,ACCOUNTNO,PREMIUMBEFOREDISCOUNT,SOURCE,@CreatedUser,CREATEDDATE,@AuthorizedUser,UPDATEDDATE,LINKID, PREMIUMAFTERDISCOUNT,
					REFUNDAMOUNT,REFUNDAFTERDISCOUNT,@AgentCommissionBeforeDiscount,@AgentCommissionAfterDiscount,
					ChargeAmount,DiscountAmount,ExcessAmount,TaxOnPremium
					from SellingPoint.dbo.MotorEndorsement where MotorEndorsementID = @EndorsementID




		INSERT INTO OrclBKICDB..ADMIN.FROMPOS_MOTOR_COVERS
		            (LINKID,
					DOCUMENTNO,
					COVERCODE,
					COVERAMOUNT,
					ClauseCode,
					[TYPE],
					TYPESERIALNO,
					COVERCODEDESCRIPTION) 
					SELECT LINKID,
					       DOCUMENTNO,
						   COVERCODE,
						   COVERAMOUNT,
						   'YES',
						   [TYPE],
						   TYPESERIALNO,
						   COVERCODEDESCRIPTION 
						   FROM SellingPoint.dbo.MotorCover
						   WHERE DOCUMENTNO = @EndorsementNo





DECLARE @ValueType NVARCHAR(50) ='P'

INSERT INTO OrclBKICDB..ADMIN.FROMPOS_CATEGORY(LINKID, DOCUMENTNO, DOCUMENTTYPE,ENDORSEMENTNO, ENDORSEMENTCOUNT, AGENTCODE, [LINENO],
                                                CATEGORY, CODE, VALUETYPE, VALUE, PREMIUM, CALCULATEDVALUE, PARENTLINKID, AGENCY, UPDATEDVALUE)

         SELECT LINKID, DOCUMENTNO,DOCUMENTTYPE, ENDORSEMENTNO, ENDORSEMENTCOUNT, AGENTCODE, [LINENO],
		 Category,CODE, @ValueType, VALUE, PremiumAfterDiscount, 
		 CommissionBeforeDiscount, PARENTLINKID,@Agency,CommissionAfterDiscount
		 FROM  SellingPoint.dbo.PolicyCategory where MotorEndorsementID = @EndorsementID



	UPDATE SellingPoint.dbo.MotorEndorsement
    SET IsMovedToTemp = 1 where  MotorEndorsement.MotorEndorsementID = @EndorsementID

 Declare @FundType nvarchar(50)
  if(@Agency ='TISCO')
  BEGIN
    SELECT @FundType = 'AGT'
  END
  ELSE IF(@Agency ='BBK')
  BEGIN
    SELECT @FundType = 'BBK'
  END

	EXECUTE('Call Import_from_POS.POS_MotorEndorsements(?,?,?,?,?)','00001','00001',@FundType,@LinkID,@DocumentNo)
	AT ORCLBKICDB

	UPDATE SellingPoint.dbo.MotorEndorsement
    SET IsMovedToOracle = 1 where  MotorEndorsement.MotorEndorsementID = @EndorsementID

END
END TRY 
 BEGIN CATCH
  UPDATE SellingPoint.dbo.MotorEndorsement
  SET IsException = 1 where MotorEndorsement.MotorEndorsementID = @EndorsementID

  DECLARE @ErrorNumber INT = ERROR_NUMBER();
  DECLARE @ErrorMessage NVARCHAR(1000) = ERROR_MESSAGE() 
  RAISERROR('Error Number-%d : Error Message-%s', 16, 1, 
  @ErrorNumber, @ErrorMessage)
  --ROLLBACK TRANSACTION
END CATCH
END


