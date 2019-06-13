USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_InsertUserMaster]    Script Date: 5/3/2018 5:55:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[SP_InsertUserMaster]
(
@Id int,
@Agency varchar(50),
@AgentCode varchar(50),
@AgentBranch varchar(50),
@UserID varchar(50),
@UserName varchar(50),
@CreatedDate datetime,
--@Password varchar(25),
--@PasswordExpiryDate datetime,
@Mobile varchar(10),
@Email varchar(50),
@IsActive bit,
@StaffNo int,
@Role varchar(50),
@CreatedBy varchar(50),
@Type varchar(50)
)
as
begin

if(@Type='edit')
begin
update  USER_MASTER set USERID=@UserID,USERNAME=@UserName where Id =@Id
end
if(@Type='delete')
begin
 update USER_MASTER set ISACTIVE=0 where Id =@Id
end

if(@Type='insert')
begin
Insert into USER_MASTER(AGENCY,AGENTCODE,AGENTBRANCH,USERID,USERNAME,CREATEDDATE,MOBILE,EMAIL,IsActive,STAFFNO,Role,CreatedBy) values(@Agency,@AgentCode,@AgentBranch,
@UserID,@UserName,@CreatedDate,@Mobile,@Email,@IsActive,@StaffNo,@Role,@CreatedBy)
end
end


