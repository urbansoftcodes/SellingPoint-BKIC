USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[MIG_IntegrateTravelEndorsement]    Script Date: 3/24/2019 10:02:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[MIG_IntegrateTravelEndorsement]
(
 @EndorsementID bigint
)
AS
BEGIN 
BEGIN TRY

DECLARE @TravelInsertedRows INT,
        @Message NVARCHAR(250),
		@InsuredCode NVARCHAR(50),
		@ENDORSEMENTNO NVARCHAR(50),
        @DocumentNo NVARCHAR(50),
		@LinkID NVARCHAR(250), 
		@Agency NVARCHAR(50),
		@UserID INT

SELECT @DocumentNo=DOCUMENTNO,
       @LinkID=LINKID,
	   @Agency = AGENCY,
	   @ENDORSEMENTNO = ENDORSEMENTNO,
	   @UserID = CREATEDBY
       FROM SellingPoint.dbo.TravelEndorsement
	   WHERE TravelEndorsementID=@EndorsementID

IF NOT Exists(SELECT * FROM OrclBKICDB..ADMIN.FROMPOS_TRAVELENDORSEMENT WHERE ENDORSEMENTNO  = @ENDORSEMENTNO)
BEGIN


DECLARE @AuthorizedBy NVARCHAR(100)
SELECT @AuthorizedBy =  UserName FROM SellingPoint.dbo.USER_MASTER WHERE id = @UserID

INSERT INTO OrclBKICDB..ADMIN.FROMPOS_TRAVELENDORSEMENT
           (ENDORSEMENTTYPE,
		   LINKID,
		   DOCUMENTNO,
		   ENDORSEMENTNO,
		   COMMENCEDATE,
		   EXPIRYDATE,
		   CREATEDBY,
		   CREATEDDATE,
		   BRANCHCODE,
		   PREMIUMBEFOREDISCOUNT,
		   PREMIUMAFTERDISCOUNT,
		   COMMISSIONBEFOREDISCOUNT,
		   COMMISSIONAFTERDISCOUNT,
		   CHARGEAMOUNT,
		   DISCOUNTAMOUNT,
		   REMARKS)			
			 
		  SELECT ENDORSEMENTTYPE,
			        LINKID,
			        DOCUMENTNO,
			        ENDORSEMENTNO,
					COMMENCEDATE,
					EXPIRYDATE,
					@AuthorizedBy,
					CREATEDDATE,
					BRANCHCODE,
			        PREMIUMBEFOREDISCOUNT,PREMIUMAFTERDISCOUNT,COMMISSIONBEFOREDISCOUNT,
			        COMMISSIONAFTERDISCOUNT,CHARGEAMOUNT,DISCOUNTAMOUNT,REMARK
			        FROM SellingPoint.dbo.TravelEndorsement
					WHERE TravelEndorsementID = @EndorsementID

  UPDATE SellingPoint.dbo.TravelEndorsement
  SET IsMovedToTemp = 1 where TravelEndorsement.TravelEndorsementID = @EndorsementID


DECLARE @FundType NVARCHAR(50)
  IF(@Agency ='TISCO')
  BEGIN
    SELECT @FundType = 'AGT'
  END
  ELSE IF(@Agency ='BBK')
  BEGIN
    SELECT @FundType = 'BBK'
  END


	EXECUTE('Call Import_from_POS.POS_TravelEndorsements(?,?,?,?,?)','00001','00001',@FundType,@LinkID,@DocumentNo)
	AT ORCLBKICDB

  UPDATE SellingPoint.dbo.TravelEndorsement
  SET IsMovedToOracle = 1 where TravelEndorsement.TravelEndorsementID = @EndorsementID

END
 END TRY 
 BEGIN CATCH
  UPDATE SellingPoint.dbo.TravelEndorsement
  SET IsException = 1 where TravelEndorsement.TravelEndorsementID = @EndorsementID

  DECLARE @ErrorNumber INT = ERROR_NUMBER();
  DECLARE @ErrorMessage NVARCHAR(1000) = ERROR_MESSAGE() 
  RAISERROR('Error Number-%d : Error Message-%s', 16, 1, 
  @ErrorNumber, @ErrorMessage)
  --ROLLBACK TRANSACTION
END CATCH
END