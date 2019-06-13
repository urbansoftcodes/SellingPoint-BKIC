USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[MIG_IntegrateHomeEndorsement]    Script Date: 3/24/2019 2:49:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[MIG_IntegrateHomeEndorsement]
(
 @EndorsementID bigint
)
AS
BEGIN 
BEGIN TRY
DECLARE @InsuredCode NVARCHAR(50),
        @ENDORSEMENTNO NVARCHAR(50),
        @DocumentNo NVARCHAR(50),
		@LinkID NVARCHAR(250), 
		@Agency NVARCHAR(50),
		@CreatedBy int, 
		@AuthorizedBy int

SELECT @DocumentNo=DOCUMENTNO,
       @LinkID=LINKID,
	   @Agency = AGENCY,
	   @ENDORSEMENTNO = ENDORSEMENTNO,
	   @CreatedBy = CREATEDBY, 
	   @AuthorizedBy = UPDATEDBY
       FROM SellingPoint.dbo.HomeEndorsement 
	   WHERE HomeEndorsementID=@EndorsementID

DECLARE @CreatedUser NVARCHAR(100)
SELECT @CreatedUser =  UserName FROM SellingPoint.dbo.USER_MASTER WHERE id = @CreatedBy


DECLARE @AuthorizedUser NVARCHAR(100)
SELECT @AuthorizedUser =  UserName FROM SellingPoint.dbo.USER_MASTER WHERE id = @AuthorizedBy

IF NOT Exists(SELECT * from OrclBKICDB..ADMIN.FROMPOS_HOMEENDORSEMENT WHERE ENDORSEMENTNO  = @ENDORSEMENTNO)
BEGIN

INSERT INTO OrclBKICDB..ADMIN.FROMPOS_HOMEENDORSEMENT
           (ENDORSEMENTTYPE,LINKID,DOCUMENTNO,ENDORSEMENTNO,FINANCECOMPANY,FINANCECOMPANYDESCRIPTION,BUILDINGSUMINSURED,
		    CONTENTSSUMINSURED,TYPE,CITYCODE,STREETNO,BLOCKNUMBER,BUILDINGNAME,
			STREETNAME,ADDRESS,COMMENCEDATE,EXPIRYDATE,CREATEDBY,CREATEDDATE,BRANCHCODE,
			PREMIUMBEFOREDISCOUNT,PREMIUMAFTERDISCOUNT,COMMISSIONBEFOREDISCOUNT,COMMISSIONAFTERDISCOUNT,
			CHARGEAMOUNT,DISCOUNTAMOUNT,REMARKS,TAXAMOUNT)
			
			 
			 SELECT  ENDORSEMENTTYPE,LINKID,DOCUMENTNO,
					 ENDORSEMENTNO,FINANCECOMPANY,FINANCECOMPANYDESCRIPTION,BuildingSumInsured,
					 ContentSumInsured,TYPE,CITYCODE,STREETNO,BLOCKNUMBER,
					 BULIDINGNAME,STREETNAME,ADDRESS2,COMMENCEDATE,
					 EXPIRYDATE,@AuthorizedUser,CREATEDDATE,BRANCHCODE,
					 PREMIUMBEFOREDISCOUNT,PREMIUMAFTERDISCOUNT,COMMISSIONBEFOREDISCOUNT,
					 COMMISSIONAFTERDISCOUNT,CHARGEAMOUNT,DISCOUNTAMOUNT,REMARK,TaxOnPremium
			 FROM SellingPoint.dbo.HomeEndorsement 
			 WHERE HomeEndorsementID = @EndorsementID





   	INSERT INTO OrclBKICDB..ADMIN.FROMPOS_HOMEDOMESTIC_COVER(LINKID,DOCUMENTNO,[LineNO],SERIALNO,ITEMSERIALNO,ITEMCODE,ITEMNAME,MEMBERSERIALNO,
				NAME,CPRNUMBER,TITLE,SEX,AGE,DATEOFBIRTH,SUMINSURED,PREMIUMAMOUNT)
				SELECT LINKID,DOCUMENTNO,[LINENO], SERIALNO, ITEMSERIALNO,ITEMCODE,ITEMNAME,MEMBERSERIALNO,
				NAME,CPRNUMBER,TITLE,SEX,AGE,DATEOFBIRTH,SUMINSURED,PREMIUMAMOUNT 
	FROM SellingPoint.dbo.HomeDomesticHelp
	WHERE DOCUMENTNO = @ENDORSEMENTNO





DECLARE @ValueType NVARCHAR(50) ='P'

INSERT INTO OrclBKICDB..ADMIN.FROMPOS_CATEGORY(LINKID, DOCUMENTNO, DOCUMENTTYPE,ENDORSEMENTNO, ENDORSEMENTCOUNT, AGENTCODE, [LINENO],
                                                CATEGORY, CODE, VALUETYPE, VALUE, PREMIUM, CALCULATEDVALUE, PARENTLINKID, AGENCY, UPDATEDVALUE)

         SELECT  LINKID, DOCUMENTNO,DOCUMENTTYPE, ENDORSEMENTNO, ENDORSEMENTCOUNT, AGENTCODE, [LINENO],
				 Category,CODE, @ValueType, VALUE, PremiumAfterDiscount, 
				 CommissionBeforeDiscount, PARENTLINKID,@Agency,CommissionAfterDiscount
		 FROM  SellingPoint.dbo.PolicyCategory
		 WHERE HomeEndorsementID = @EndorsementID


  UPDATE SellingPoint.dbo.HomeEndorsement
  SET IsMovedToTemp = 1 where  HomeEndorsement.HomeEndorsementID = @EndorsementID


DECLARE @FundType NVARCHAR(50)

  IF(@Agency ='TISCO')
	  BEGIN
			SELECT @FundType = 'AGT'
	  END
  ELSE IF(@Agency ='BBK')
	  BEGIN
			SELECT @FundType = 'BBK'
	  END

	EXECUTE('Call Import_from_POS.POS_HomeEndorsements(?,?,?,?,?)','00001','00001', @FundType,@LinkID,@DocumentNo)
	AT ORCLBKICDB

  UPDATE SellingPoint.dbo.HomeEndorsement
  SET IsMovedToOracle = 1 where  HomeEndorsement.HomeEndorsementID = @EndorsementID

END
END TRY 
 BEGIN CATCH
  UPDATE SellingPoint.dbo.HomeEndorsement
  SET IsException = 1 where HomeEndorsement.HomeEndorsementID = @EndorsementID

  DECLARE @ErrorNumber INT = ERROR_NUMBER();
  DECLARE @ErrorMessage NVARCHAR(1000) = ERROR_MESSAGE() 
  RAISERROR('Error Number-%d : Error Message-%s', 16, 1, 
  @ErrorNumber, @ErrorMessage)
  --ROLLBACK TRANSACTION
END CATCH
END