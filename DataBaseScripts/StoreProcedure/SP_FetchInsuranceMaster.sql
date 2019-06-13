
--select * from InsuranceProductMaster
alter Procedure [dbo].[SP_FetchInsuranceMaster]
(
@Id int,
@Agency varchar(50),
@AgentCode varchar(50),
@MainClass varchar(50),
@Subclass varchar(50),
@EffectiveDateFrom Datetime,
@EffectiveDateTo Datetime,
@IsActive bit,
@CreatedDate Datetime,
@UpdatedDate Datetime,
@Type varchar(50)
)
as
begin

if(@Type='edit')
begin
update  InsuranceProductMaster set SUBCLASS=@Subclass,MAINCLASS=@MainClass where ID=@id
end
if(@Type='delete')
begin
 update InsuranceProductMaster set ISACTIVE=0 where ID=@Id
 end
if(@Type='insert')
begin
 insert into InsuranceProductMaster(AGENCY,AGENTCODE,MAINCLASS,SUBCLASS,EFFECTIVEDATEFROM,EFFECTIVEDATETO,
	 ISACTIVE,CREATEDDATE,UPDATEDDATE) values
	 (@Agency,@AgentCode,
@MainClass,
@Subclass ,
@EffectiveDateFrom,
@EffectiveDateTo,
@IsActive,
@CreatedDate,
@UpdatedDate
)
end
end