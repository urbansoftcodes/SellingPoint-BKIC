USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_BranchMaster]    Script Date: 5/10/2018 12:57:26 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[SP_BranchMaster]
(
@Id int,
@Agency varchar(50),
@AgentCode varchar(50),
@AgentBranch varchar(50),
@BranchName varchar(50),
@BranchAddress varchar(max),
@Phone varchar(50),
@Incharge varchar(100),
@Email varchar(50),
@CreatedBy varchar(50),
@Type varchar(50)
)
as
begin

if(@Type='edit')
begin
update  BRANCHMASTER set BRANCHNAME=@BranchName,BRANCHADDRESS=@BranchAddress,TELEPHONENO=@Phone,INCHARGE=@Incharge,EMAIL=@Email where Id =@Id
end
if(@Type='delete')
begin
 update BRANCHMASTER set ISACTIVE=0 where Id =@Id
end

if(@Type='insert')
begin
 insert into BRANCHMASTER(AGENCY,AGENTCODE,AGENTBRANCH,BRANCHNAME,BRANCHADDRESS,TELEPHONENO,
	 INCHARGE,EMAIL,ISACTIVE,CreatedBy) values(@Agency,@AgentCode,@AgentBranch,@BranchName,@BranchAddress,
	 @Phone,@Incharge,@Email,1,@CreatedBy)
end
end