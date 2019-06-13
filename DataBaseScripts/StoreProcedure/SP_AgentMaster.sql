USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_AgentMaster]    Script Date: 5/10/2018 6:44:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[SP_AgentMaster]
(
@Id int,
@Agency varchar(50),
@AgentCode varchar(50),
@AgentBranch varchar(50),
@IsActive bit,
@Type varchar(50)
)
as
begin

if(@Type='edit')
begin
update  AgentMaster set AgentCode=@AgentCode,AgentBranch=@AgentBranch where Id=@Id
end
if(@Type='delete')
begin
 update AgentMaster set IsActive=0 where Id=@Id
 end
if(@Type='insert')
begin
 insert into AgentMaster(Agency,AgentCode,AgentBranch,IsActive) values
	                    (@Agency,@AgentCode,@AgentBranch,1)
end
end