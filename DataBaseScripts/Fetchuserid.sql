USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_FetchUserID]    Script Date: 6/15/2018 5:32:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--exec [SP_FetchUserID] 'sun'
ALTER Procedure [dbo].[SP_FetchUserID]
(
@UserID varchar(50)
)
as
begin

Declare @CurrentDate datetime,@AgentCode varchar(50)
select A.AGENCY,A.AGENTCODE,A.AGENTBRANCH,A.USERNAME,A.USERID from USER_MASTER A with(nolock) 
  where A.USERID=@UserID 
  
set @AgentCode= (select A.AGENTCODE from USER_MASTER A with(nolock) 
  where A.USERID=@UserID) 
set @CurrentDate=GETDATE()
  select distinct(Mainclass) from InsuranceProduct_Master with(nolock) where agentcode=@AgentCode and 
 (EFFECTIVEDATEFROM <= @CurrentDate AND EFFECTIVEDATETO   >= @CurrentDate)
end
