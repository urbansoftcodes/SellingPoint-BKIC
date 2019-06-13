USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetOracleMotorRenewalPolicies]    Script Date: 4/21/2019 5:17:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  Procedure [dbo].[SP_GetOracleHomeRenewalPolicies]
@Agency Nvarchar(100),
@AgentCode Nvarchar(100),
@AgentBranch Nvarchar(100),
@IncludeHIR bit,
@IsRenewal bit,
@DocumentNo nvarchar(100)
AS
BEGIN   
      SELECT * FROM OPENQUERY (OrclBKICDB, 'SELECT *  from  TOPOS_REN_DOCUMENT_FIRE_DET')
            where DOCUMENTNO LIKE @DocumentNo + '%' 

      
	  SELECT * FROM OPENQUERY (OrclBKICDB, 'SELECT *  from  TOPOS_REN_DOCUMENT_ITEMS')
            where DOCUMENTNO LIKE @DocumentNo + '%' 

      
	  SELECT * FROM OPENQUERY (OrclBKICDB, 'SELECT *  from  TOPOS_REN_DOCUMENT_SUBITEMS')
            where DOCUMENTNO LIKE @DocumentNo + '%' 

      SELECT * FROM OPENQUERY (OrclBKICDB, 'SELECT *  from  TOPOS_REN_HOMEDOMESTIC_COVER')
           where DOCUMENTNO LIKE @DocumentNo + '%' 


     SELECT * FROM OPENQUERY (OrclBKICDB, 'SELECT *  from  TOPOS_REN_QUESTIONNAIRE')
          where DOCUMENTNO LIKE @DocumentNo + '%' 


END