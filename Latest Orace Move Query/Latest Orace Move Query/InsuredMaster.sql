USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[MIG_IntegrateInsuredDetails]    Script Date: 3/24/2019 12:40:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[MIG_IntegrateInsuredDetails]
(
	@InsuredCode nvarchar(50)
)
AS
BEGIN
--begin TRANSACTION
DECLARE @CPR NVARCHAR(30),@Nationality NVARCHAR(50),@DOB DATETIME,@SpecialCode NVARCHAR(50),@Title NVARCHAR(5),@FlatNo NCHAR(10),
@BuildingNo NCHAR(10),@Address NVARCHAR(250),@RoadNo NCHAR(10),@BlockNo NCHAR(10),@MobileNo NVARCHAR(50),@POBOX NCHAR(10),
@Town NVARCHAR(250),@OracleInsuredCode NVARCHAR(30)

SELECT @CPR=CPR,@Nationality=NATIONALITY,@DOB=DATEOFBIRTH,@RoadNo=ROAD,@BlockNo=BLOCK,@FlatNo=FLAT,@BuildingNo=BUILDING,
       @Town = Area
FROM SellingPoint.dbo.InsuredMaster WHERE INSUREDCODE=@InsuredCode

--select @RoadNo=ROADNUMBER,@BlockNo=BLOCKNUMBER,@FlatNo=FLATNO,@BuildingNo=BUILDINGNO,@Address=ADDRESS1,@POBOX=POBOX,

--@Town=Town from BKICDB.dbo.BK_InsuredAddressDetails where INSUREDCODE=@InsuredCode



IF exists(SELECT * FROM  OrclBKICDB..ADMIN.FROMPOS_INSURED_MASTER WHERE PASSPORT_ICNUMBER=@CPR)
BEGIN
   UPDATE OrclBKICDB..ADMIN.FROMPOS_INSURED_MASTER 
          SET NATIONALITY=@Nationality,
		      DATEOFBIRTH=@DOB 
			  WHERE PASSPORT_ICNUMBER=@CPR

   SELECT  @OracleInsuredCode =  INSUREDCODE 
           FROM  OrclBKICDB..ADMIN.FROMPOS_INSURED_MASTER
		   WHERE PASSPORT_ICNUMBER=@CPR

    UPDATE OrclBKICDB..ADMIN.FROMPOS_INSURED_ADDRESS_DET 
	        SET FLATNO=@FlatNo,
			    BUILDINGNO=@BuildingNo,
			    ROADNUMBER=@RoadNo,
				BLOCKNUMBER=@BlockNo,
	          --  ADDRESS1=@Address,
				TELEPHONEMOBILE=@MobileNo
			--	POBOX=@POBOX 
				WHERE INSUREDCODE=@OracleInsuredCode
   
 
EXECUTE('Call Import_from_POS.Insert_Insured_Details(?,?,?)','00001','00001',@InsuredCode) AT ORCLBKICDB
 
END
ELSE
BEGIN
	INSERT INTO OrclBKICDB..ADMIN.FROMPOS_INSURED_MASTER
	            (INSUREDCODE,
				 TITLE,
				 INSUREDNAME,
				 SEX,
				 YEAROFBIRTH,
				 PASSPORT_ICNUMBER,
				 NATIONALITY,
	             OCCUPATION,
				 DATEOFBIRTH,
				 MARITALSTATUS,
				 FIRSTNAME,
				 LASTNAME,
				 MIDDLENAME)
				SELECT INSUREDCODE,
				'',
				INSUREDNAME,
				LEFT(Gender, 1),
				year(DateOfBirth),
				CPR,
				NATIONALITY,
				OCCUPATION,
				DATEOFBIRTH,
				'',
				FIRSTNAME,
				LASTNAME,
				MIDDLENAME 
				FROM SellingPoint.dbo.InsuredMaster 
				WHERE CPR=@CPR   

	INSERT INTO OrclBKICDB..ADMIN.FROMPOS_INSURED_ADDRESS_DET
				(INSUREDCODE,
				  ROADNUMBER,
				  BLOCKNUMBER,
				  FLATNO,
				  BUILDINGNO,
				  ADDRESS1,
				  TELEPHONE,
				  FAX,
				  EMAILADDRESS,
				  WEBSITEADDRESS,
				  TELEPHONERESIDENCE,
				  TELEPHONEMOBILE,
				  POBOX,
				  [LINENO],
				  COUNTRYCODE,
				  ADDRESSTYPE,
				  TOWN
				  )

	SELECT INSUREDCODE,
	       ROAD,
		   BLOCK,
		   FLAT,
		   BUILDING,
		   '',
		   Mobile,
		   '',
	      Email,
		  '',
		  '',
		  '',
		  '',
		  1,
		  'BHR',
	      '', 
		  Area 
		  FROM SellingPoint.dbo.InsuredMaster
		  WHERE INSUREDCODE=@InsuredCode

	EXECUTE('Call Import_from_POS.Insert_Insured_Details(?,?,?)','00001','00001',@InsuredCode) AT ORCLBKICDB


END
--Commit TRANSACTION

END