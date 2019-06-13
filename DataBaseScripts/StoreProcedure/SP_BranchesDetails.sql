
USE [SellingPoint]
GO
IF EXISTS ( SELECT  *
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'[dbo].[SP_BranchMaster]')
                    AND type IN ( N'P', N'PC' ) ) 
					
BEGIN
	DROP PROCEDURE [dbo].[SP_BranchMaster]
END
GO
Create Procedure [dbo].[SP_BranchMaster]
(
@Agency varchar(50),
@AgentCode varchar(50),
@AgentBranch varchar(50),
@BranchName varchar(50),
@BranchAddress varchar(max),
@Phone varchar(50),
@Incharge varchar(100),
@Email varchar(50),
@Type varchar(50)

)
as
begin

if(@Type='edit')
begin
update  BRANCHMASTER set BRANCHNAME=@BranchName,BRANCHADDRESS=@BranchAddress where AGENTCODE=@AgentCode and AGENTBRANCH=@AgentBranch
end
if(@Type='delete')
begin
 update BRANCHMASTER set ISACTIVE=0 where AGENTCODE=@AgentCode and AGENTBRANCH=@AgentBranch
end

if(@Type='insert')
begin
 insert into BRANCHMASTER(AGENCY,AGENTCODE,AGENTBRANCH,BRANCHNAME,BRANCHADDRESS,TELEPHONENO,
	 INCHARGE,EMAIL,ISACTIVE) values(@Agency,@AgentCode,@AgentBranch,@BranchName,@BranchAddress,
	 @Phone,@Incharge,@Email,1)
end
end