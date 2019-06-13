USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_FetchInformationForAdmin]    Script Date: 5/26/2018 7:01:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[SP_FetchInformationForAdmin]
(
@Type varchar(50)
)
as
begin
if(@Type='MT_BranchMaster')
begin
select * from BranchMaster bm with(nolock) where bm.ISACTIVE =1
end
if(@Type='MT_InsuranceProductMaster')
begin
select * from InsuranceProductMaster im with(nolock) where im.ISACTIVE=1
end

if(@Type='MT_AgentMaster')
begin
select * from [dbo].[AGENTMASTER] am with(nolock) where am.ISACTIVE=1 
end


if(@Type='MT_UserMaster')
begin
select * from [dbo].[USER_MASTER] um with(nolock) where um.ISACTIVE=1 
end

if(@Type='MT_CategoryMaster')
begin
select * from [dbo].[CATEGORY_MASTER] um with(nolock) where um.Status=1 
end

if(@Type='MT_MotorProductCover')
begin
select * from [dbo].[MotorProductCover] with(nolock)
end


if(@Type='MT_MotorCoverMaster')
begin
select * from [dbo].[MotorCoverMaster] with(nolock)
end

if(@Type='MT_InsuredMaster')
begin
select * from [dbo].[InsuredMaster] with(nolock)
end

if(@Type='MT_IntroducedbyMaster')
begin
select * from [dbo].[IntroducedbyMaster] with(nolock) 
end

end