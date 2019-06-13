USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetOracleMotorRenewalByDocumentNo]    Script Date: 3/19/2019 7:38:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[SP_GetOracleMotorRenewalByDocumentNo]
@OracleDocumentNo nvarchar(100),
@Agency nvarchar(100),
@AgentCode nvarchar(100)
AS
BEGIN
      
	  -- DECLARE @RENEWALCOUNT int = 0;
	  -- DECLARE @DOCUMENTNO Nvarchar(max)
     
	  --IF Exists(SELECT * FROM RENEWALMOTOR WHERE DOCUMENTNO = @OracleDocumentNo)
	  --BEGIN	
		 --SELECT  @RENEWALCOUNT = ISNULL(Max(RENEWALCOUNT), 0) FROM  RenewalMotor
		 --                    WHERE DOCUMENTNO = @OracleDocumentNo


   --      IF EXISTS(SELECT * FROM Motor WHERE OldDocumentNumber = @OracleDocumentNo AND RENEWALCOUNT = @RENEWALCOUNT + 1)
		 --BEGIN		 
		 		   
   --         SELECT @DOCUMENTNO = DOCUMENTNO 
			--       FROM Motor 
			--	   WHERE OLDDOCUMENTNUMBER = @OracleDocumentNo

		 --   DECLARE @CommissionBeforeDiscount decimal(18,3) = 0
   --         DECLARE @CommissionAfterDiscount decimal(18,3) = 0
   --         DECLARE @TaxOnCommission decimal(18,3) = 0


			--SELECT @CommissionBeforeDiscount =  SUM(CommissionBeforeDiscount),
			--	   @CommissionAfterDiscount = SUM(CommissionAfterDiscount) ,
			--	   @TaxOnCommission = SUM(TaxOnCommission)
			--	   FROM PolicyCategory
			--	   WHERE DOCUMENTNO = @DocumentNo
			--	   AND  RenewalCount = @RenewalCount + 1
			--	   AND IsDeductable = 1

		 --   SELECT     M.MOTORID,M.AGENCY,M.AGENTCODE,
			--		   M.INSUREDCODE,IM.INSUREDNAME,M.CPR,
			--		   M.EXPIRYDATE, M.FINANCECOMPANY, M.BODY,
			--		   M.YEAR,m.MAKE,M.MODEL,
			--		   M.VEHICLETYPE,M.SUBCLASS,
			--		   M.VEHICLEVALUE,
			--		   M.COMMENCEDATE,M.REGISTRATIONNO,M.CHASSISNO,M.TONNAGE,M.PURPOSEFUSE,
			--		   M.EXCESSTYPE,M.GROSSPREMIUM,M.EXCESSAMOUNT,M.SOURCE,M.DOCUMENTNO,M.CREATEDDATE,
			--		   M.IsHIR, M.HIRStatus, M.MAINCLASS,M.PREMIUMBEFOREDISCOUNT,M.PREMIUMAFTERDISCOUNT,M.LOADAMOUNT,
			--		  -- M.COMMISSIONBEFOREDISCOUNT,
			--		  -- M.COMMISSIONAFTERDISCOUNT,
			--		   @CommissionBeforeDiscount AS COMMISSIONBEFOREDISCOUNT,
			--		   @CommissionAfterDiscount AS COMMISSIONAFTERDISCOUNT,
			--		   M.TaxOnPremium,
			--		   --M.TaxOnCommission,
			--		   @TaxOnCommission AS TaxOnCommission,
			--		   M.SEATINGCAPACITY,M.ISUNDERBCFC,
			--		   M.IsActive,M.IsSaved,M.ISCANCELLED,
			--		   IM.DateOfBirth,M.PAYMENT_TYPE AS PaymentType,
			--		   M.ACCOUNTNO,M.REMARK,M.ENDORSEMENTCOUNT,M.RENEWALCOUNT,
			--		   M.CLAIMAMOUNT,M.LOADAMOUNT,M.OldDocumentNumber
			--		   FROM Motor AS M WITH(NOLOCK)

		 --             --Join Insuredmaster as IM on M.CPR = @CPR
			--		   Join Insuredmaster AS IM ON IM.CPR = '35643654' 
			--		   WHERE DOCUMENTNO = @DocumentNo 
			--		  -- AND M.AGENTCODE = @AgentCode			   
			--		   AND M.RENEWALCOUNT = @RenewalCount + 1


   --             -- motor default covers 
			--	  SELECT CoverCode,COVERCODEDESCRIPTION,COVERAMOUNT,mc.AddedByEndorsement
			--			 FROM MotorCover mc	
			--			 WHERE mc.DOCUMENTNO = @DocumentNo 
			--			 AND mc.RenewalCount = @RenewalCount + 1
			--			 AND IsOptionalCover = 0

			--	   -- motor product covers 
			--	  SELECT CoverCode,COVERCODEDESCRIPTION,COVERAMOUNT,0
			--			 FROM  MotorCoverCodes
			--			 WHERE --AgentCode = @AgentCode AND
			--			 MainClass = 'SECUR' AND SubClass = 'PRD1'
				

			--	  --Motor Optional Covers
			--	   SELECT CoverCode,COVERCODEDESCRIPTION,COVERAMOUNT,mc.AddedByEndorsement
			--			  FROM MotorCover mc	
			--			  WHERE mc.DOCUMENTNO = @DocumentNo 
			--			  AND mc.RenewalCount = @RenewalCount + 1
			--			  AND IsOptionalCover = 1
		 --END	
		 --ELSE
		 --BEGIN	  
		 --   SELECT * FROM RenewalMotor WHERE DOCUMENTNO = @OracleDocumentNo
		 --END
	  --END

	  DECLARE @RENEWALCOUNT int = 0;
	  DECLARE @DOCUMENTNO Nvarchar(max)
     
	  IF EXISTS(SELECT * FROM MOTOR WHERE OldDocumentNumber = @OracleDocumentNo)
	  BEGIN	

	        DECLARE @MainClass nvarchar(max),@SubClass nvarchar(max),@CPR nvarchar(max)
	        DECLARE @CommissionBeforeDiscount decimal(18,3) = 0
            DECLARE @CommissionAfterDiscount decimal(18,3) = 0
            DECLARE @TaxOnCommission decimal(18,3) = 0

		    SELECT  @RENEWALCOUNT = RENEWALCOUNT,
		            @DOCUMENTNO = DOCUMENTNO,
					@CPR = CPR,
					@MainClass = MainClass,
					@SubClass = SUBCLASS
					FROM  MOTOR
				    WHERE OldDocumentNumber = @OracleDocumentNo		    

			SELECT @CommissionBeforeDiscount =  SUM(CommissionBeforeDiscount),
				   @CommissionAfterDiscount = SUM(CommissionAfterDiscount) ,
				   @TaxOnCommission = SUM(TaxOnCommission)
				   FROM PolicyCategory
				   WHERE DOCUMENTNO = @DocumentNo
				   AND  RenewalCount = @RENEWALCOUNT
				   AND IsDeductable = 1

		    SELECT     M.MOTORID,M.AGENCY,M.AGENTCODE,
					   M.INSUREDCODE,IM.INSUREDNAME,M.CPR,
					   M.EXPIRYDATE, M.FINANCECOMPANY, M.BODY,
					   M.YEAR,m.MAKE,M.MODEL,
					   M.VEHICLETYPE,M.SUBCLASS,
					   M.VEHICLEVALUE,
					   M.COMMENCEDATE,M.REGISTRATIONNO,M.CHASSISNO,M.TONNAGE,M.PURPOSEFUSE,
					   M.EXCESSTYPE,M.GROSSPREMIUM,M.EXCESSAMOUNT,M.SOURCE,M.DOCUMENTNO,M.CREATEDDATE,
					   M.IsHIR, M.HIRStatus, M.MAINCLASS,M.PREMIUMBEFOREDISCOUNT,M.PREMIUMAFTERDISCOUNT,M.LOADAMOUNT,
					  -- M.COMMISSIONBEFOREDISCOUNT,
					  -- M.COMMISSIONAFTERDISCOUNT,
					   @CommissionBeforeDiscount AS COMMISSIONBEFOREDISCOUNT,
					   @CommissionAfterDiscount AS COMMISSIONAFTERDISCOUNT,
					   M.TaxOnPremium,
					   --M.TaxOnCommission,
					   @TaxOnCommission AS TaxOnCommission,
					   M.SEATINGCAPACITY,M.ISUNDERBCFC,
					   M.IsActive,M.IsSaved,M.ISCANCELLED,
					   IM.DateOfBirth,M.PAYMENT_TYPE AS PaymentType,
					   M.ACCOUNTNO,M.REMARK,M.ENDORSEMENTCOUNT,M.RENEWALCOUNT,
					   M.CLAIMAMOUNT,M.LOADAMOUNT,M.OldDocumentNumber, 1 As IsSystem,
					   M.RenewalDelayedDays,M.ActualRenewalStartDate
					   FROM Motor AS M WITH(NOLOCK)

		              --Join Insuredmaster as IM on M.CPR = @CPR
					   Join Insuredmaster AS IM ON IM.CPR = @CPR
					   WHERE DOCUMENTNO = @DocumentNo 
					  -- AND M.AGENTCODE = @AgentCode			   
					   AND M.RENEWALCOUNT = @RENEWALCOUNT


                -- motor default covers 
				  SELECT CoverCode,COVERCODEDESCRIPTION,COVERAMOUNT,mc.AddedByEndorsement
						 FROM MotorCover mc	
						 WHERE mc.DOCUMENTNO = @DocumentNo 
						 AND mc.RenewalCount = @RENEWALCOUNT
						 AND IsOptionalCover = 0

				   -- motor product covers 
				  SELECT CoverCode,COVERCODEDESCRIPTION,COVERAMOUNT,0
						 FROM  MotorCoverCodes
						 WHERE --AgentCode = @AgentCode AND
						 MainClass = @MainClass AND SubClass = @SubClass
				

				  --Motor Optional Covers
				   SELECT CoverCode,COVERCODEDESCRIPTION,COVERAMOUNT,mc.AddedByEndorsement
						  FROM MotorCover mc	
						  WHERE mc.DOCUMENTNO = @DocumentNo 
						  AND mc.RenewalCount = @RENEWALCOUNT
						  AND IsOptionalCover = 1
		 END	
		 ELSE
		 BEGIN
			   EXECUTE('SELECT * from  TOPOS_REN_MOTOR WHERE DOCUMENTNO = ?', @OracleDocumentNo) AT OrclBKICDB
			   EXECUTE('SELECT *  from  TOPOS_REN_MOTOR_COVERS WHERE DOCUMENTNO = ?', @OracleDocumentNo) AT OrclBKICDB
		 END
END