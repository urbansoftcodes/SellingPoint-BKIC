use sellingpoint

drop table MotorCoverMaster
Create Table MotorCoverMaster
(
COVERID int identity(1,1) PRIMARY KEY,
COVERCODE varchar(500),
COVERSDescription varchar(max)
)

Create table MotorProductCover
(
ProductCoverID int identity(1,1) Primary key,
AGENCY VARCHAR(50),
AGENTCODE varchar(50),
MAINCLASS varchar(50),
SUBCLASS VARCHAR(50),
COVERID INT FOREIGN KEY REFERENCES MotorCoverMaster(COVERID),
IsCovered char,
Rate decimal(18,3),
COVERSTYPE varchar(max),
CreatedBy varchar(500),
UpdatedBy varchar(500)
)


go


insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('DRIVERAGE','Under Age Cover')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Third Party Liability')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Accidental collision or overturning,fire,external explosion,self-ignition,
lightening,theft,damage during transit,impact of dropped or flying objects')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Perils of Nature:flood,storm,typhoon,hurricane,earthquake and hailstorm')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('RC','Car replacement')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Agency Repair(from purchasing date)')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Depreciation on Parts')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('SRCC','Riot,Strike and/or Civil Commotion')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Windscreen Protection')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Premium rate not affected by hit and run for one claim')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Premium rate not affected by it and run for one claim')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Premium rate not affected if claim is up to BD 500')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Life Insurance for the Insured due to natural death-Sum Insured:BD3,000')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('WWETA','World-Wide Emergency Travel Assistance')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','K.S.A cover')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('PICKUP_DELIVERY','Pick up & Delivery')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Battery Delivery 24hour Vehicle Assistance Services in Bahrain')
insert into MotorCoverMaster(COVERCODE,COVERSDescription) values('','Services in GCC countries Jordan,Lebanon and Syria')


select * from MotorCoverMaster
