USE [SellingPoint]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter Procedure [dbo].[SP_InsuredMasterDetail]
(
@InsuredId bigint,
@InsuredCode varchar(50),
@CPR varchar(50),
@FirstName varchar(50),
@MiddleName varchar(50),
@LastName varchar(50),
@Gender varchar(50),
@Flat varchar(50),
@Building varchar(50),
@Road varchar(50),
@Block varchar(50),
@Area varchar(50),
@Mobile varchar(50),
@Email varchar(50),
@DateOfBirth Datetime,
@Nationality varchar(50),
@Occupation varchar(50),
@IsActive varchar(50),
@Type varchar(50)
)
as
begin

if(@Type='edit')
begin
update  InsuredMaster set FirstName=@FirstName,MiddleName=@MiddleName,LastName =@LastName,Gender = @Gender,Flat = @Flat,Building = @Building,Road = @Road, Block = @Block, Area =@Area,Mobile=@Mobile, Email = @Email, Nationality = @Nationality, Occupation = @Occupation where InsuredId = @InsuredId
end
if(@Type='delete')
begin
 update InsuredMaster set IsActive=0 where InsuredId=@InsuredId
 end
if(@Type='insert')
begin
 insert into InsuredMaster(
InsuredCode,
CPR,
FirstName ,
MiddleName,
LastName,
Gender,
Flat,
Building,
Road,
Block,
Area,
Mobile,
Email,
DateOfBirth,
Nationality,
Occupation,IsActive) 
 values
 (
@InsuredCode,
@CPR,
@FirstName ,
@MiddleName,
@LastName,
@Gender,
@Flat,
@Building,
@Road,
@Block,
@Area,
@Mobile,
@Email,
@DateOfBirth,
@Nationality,
@Occupation,1) 
	                    
end
end