USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[MIG_IntegrateDomesticHelpDetails]    Script Date: 3/28/2019 11:09:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[MIG_IntegrateDomesticHelpDetails]
(
	@DomesticID BIGINT	
)
AS
BEGIN

BEGIN TRY

DECLARE @LinkId NVARCHAR(250),@CPR NVARCHAR(30),
        @InsuredCode NVARCHAR(50),@DocumentNo NVARCHAR(50),
		@DomesticHelpInsertedRows INT,
        @Message NVARCHAR(250), @Agency NVARCHAR(100), @UserID INT

SELECT @LinkId = LINKID,@InsuredCode = INSUREDCODE,
      @DocumentNo= DOCUMENTNO, @Agency = Agency, @UserID = AuthorizedBy
	  FROM SellingPoint.dbo.DomesticHelp WHERE DOMESTICID=@DomesticID;

IF NOT EXISTS(SELECT * FROM OrclBKICDB..ADMIN.FROMPOS_DOMESTICHELP WHERE DOCUMENTNO = @DocumentNo AND LINKID = @LinkId)
BEGIN


DECLARE @CustomerCode NVARCHAR(10)
SELECT @CustomerCode =  CustomerCode FROM SellingPoint.dbo.AGENTMASTER WHERE Agency = @Agency

DECLARE @AuthorizedBy NVARCHAR(100)
SELECT @AuthorizedBy =  UserName FROM SellingPoint.dbo.USER_MASTER WHERE id = @UserID

	INSERT INTO OrclBKICDB..ADMIN.FROMPOS_DOMESTICHELP(LINKID,
	                                                   DOCUMENTNO,
													   INSUREDCODE,
													   INSUREDNAME,
													   SUMINSURED,
													   PREMIUMAMOUNT,
													   EXPIRYDATE,
													   REMARKS,
													   TYPE,
													   ADDRESS1,
													   MAINCLASS,
													   SUBCLASS,
													   DOCUMENTDATE,
													   CREATEBY,
													   CREATEDT,
													   COMMENCEDATE,
													   IDENTITYNO,
													   RENEWAL,
													   PREVIOUSDOCUMENTNO,
													   RENEWED,
													   INSURANCEPERIOD,
													   TRANSACTION_NO,
													   SOURCE,
													   DISCOUNTPERCENT,
													   DISCOUNTAMOUNT,
													   COMMISSIONAMOUNT,
													   COMMISSIONBEFOREDISCOUNT,
													   PREMIUMBEFOREDISCOUNT,
													   AGENCYCODE,
													   CUSTOMERCODE,
													   AGENTCODE,
													   AGENCYBRANCH,
													   INTRODUCEDBY,
													   TAXPERCENT,
													   TAXAMOUNT)
    SELECT LINKID,DOCUMENTNO,INSUREDCODE,INSUREDNAME,SUMINSURED,(TAXONPREMIUM + PremiumAfterDiscount),
	EXPIRYDATE,REMARKS,TYPE,ADDRESS1,MAINCLASS,SUBCLASS,DATEOFSUBMISSION,@AuthorizedBy,CREATEDDATE,COMMENCEDATE,
	IDENTITYNO,RENEWAL,PREVIOUSDOCUMENTNO,RENEWED,INSURANCEPERIOD,TRANSACTION_NO,Source,DiscountPercent,DISCOUNTAMOUNT,CommissionAmount,
	CommissionBeforeDiscount,PremiumBeforeDiscount,AGENCY,@CustomerCode,AGENTCODE,BRANCHCODE,@AuthorizedBy,TAXPREMIUMPERCENT,TAXONPREMIUM
	FROM SellingPoint.dbo.DomesticHelp 
	WHERE DOMESTICID=@DomesticID




	INSERT INTO OrclBKICDB..ADMIN.FROMPOS_DOMESTIC_MEMBERS(
	                                                     LINKID,
														 DOCUMENTNO,
														 INSUREDCODE,
														 INSUREDNAME,
														 SUMINSURED,
														 PREMIUMAMOUNT,
														 EXPIRYDATE,
	                                                     REMARKS,
														 TYPE,
														 ADDRESS1,
														 OCCUPATION,
														 NATIONALITY,
														 PASSPORT,
														 DOB,
														 SEX,
														 ITEMSERIALNO,
														 MAINCLASS,
														 SUBCLASS,
														 DOCUMENTDATE,
														 CREATEBY,
														 CREATEDT,
														 COMMENCEDATE,
	                                                     IDENTITYNO,
														 OCCUPATIONOTHER) 
														  SELECT LINKID,DOCUMENTNO,INSUREDCODE,INSUREDNAME,SUMINSURED,PREMIUMAMOUNT,
	EXPIRYDATE,REMARKS,TYPE,ADDRESS1,OCCUPATION,NATIONALITY,PASSPORT,DOB,SEX,ITEMSERIALNO,MAINCLASS,SUBCLASS,DATEOFSUBMISSION,CREATEDBY,
	CREATEDDATE,COMMENCEDATE,IDENTITYNO,OCCUPATIONOTHER 
	FROM SellingPoint.dbo.DomesticHelpMemberDetails
	WHERE LINKID=@LinkId and DOMESTICID=@DomesticID

	

DECLARE @ValueType NVARCHAR(50) ='P'

INSERT INTO OrclBKICDB..ADMIN.FROMPOS_CATEGORY(LINKID, DOCUMENTNO, DOCUMENTTYPE,ENDORSEMENTNO, ENDORSEMENTCOUNT, AGENTCODE, [LINENO],
                                                CATEGORY, CODE, VALUETYPE, VALUE, PREMIUM, CALCULATEDVALUE, PARENTLINKID, AGENCY, UPDATEDVALUE)

         SELECT LINKID, DOCUMENTNO,DOCUMENTTYPE, ENDORSEMENTNO, 0, AGENTCODE, 1,
		 Category,CODE, @ValueType, VALUE, PremiumAfterDiscount, CommissionBeforeDiscount, PARENTLINKID,'', CommissionAfterDiscount
		 FROM  SellingPoint.dbo.PolicyCategory WHERE DOCUMENTNO = @DocumentNo



UPDATE SellingPoint.dbo.DomesticHelp
  SET IsMovedToTemp = 1
  WHERE DOMESTICID = @DomesticID


DECLARE @FundType NVARCHAR(10) = ''
	 
	
	 IF(@Agency = 'TISCO')
		BEGIN
			 SELECT @FundType = 'AGT'
		END
	ELSE IF(@Agency = 'BBK')
		BEGIN
			 SELECT @FundType = 'BBK'
		END


	BEGIN
		EXECUTE('Call Import_from_POS.Insert_DomesticHelp_Policy(?,?,?,?,?,?)','00001','00001',@FundType,@InsuredCode,@DocumentNo,@LinkID)
		AT ORCLBKICDB
	END

  UPDATE SellingPoint.dbo.DomesticHelp
  SET IsMovedToOracle = 1
  WHERE DOMESTICID = @DomesticID
END
END TRY
BEGIN CATCH

  UPDATE SellingPoint.dbo.DomesticHelp
  SET IsException = 1
  WHERE DOMESTICID = @DomesticID

  DECLARE @ErrorNumber INT = ERROR_NUMBER();
  DECLARE @ErrorMessage NVARCHAR(1000) = ERROR_MESSAGE() 
  RAISERROR('Error Number-%d : Error Message-%s', 16, 1, 
  @ErrorNumber, @ErrorMessage)
--ROLLBACK TRANSACTION
END CATCH

END
  





