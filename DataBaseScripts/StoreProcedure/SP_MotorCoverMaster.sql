

--select * from InsuranceProductMaster
Create Procedure [dbo].[SP_MotorProductCover]
(
@ProductCoverId int,
@Agency varchar(50),
@AgentCode varchar(50),
@MainClass varchar(50),
@Subclass varchar(50),
@CoverId Datetime,
@IsCovered Datetime,
@Rate bit,
@CoverType varchar(50),
@CreatedBy varchar(50),
@UpdatedBy varchar(50),
@Type varchar(50)
)
as
begin

if(@Type='edit')
begin
update  MotorProductCover set SUBCLASS=@Subclass,MAINCLASS=@MainClass where ProductCoverID=@ProductCoverId
end
--if(@Type='delete')
--begin
-- update MotorProductCover set ISACTIVE=0 where ProductCoverID=@ProductCoverId
-- end
if(@Type='insert')
begin
 insert into MotorProductCover(AGENCY,AGENTCODE,MAINCLASS,SUBCLASS,COVERID,IsCovered,Rate,COVERSTYPE,CreatedBy,UpdatedBy) values
	 (@Agency,@AgentCode,
@MainClass,
@Subclass ,
@CoverId,
@IsCovered,
@Rate,
@CoverType,
@UpdatedDate,
@CreatedBy
)
end
end