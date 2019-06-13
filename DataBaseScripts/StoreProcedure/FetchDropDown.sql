IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_FetchDropDowns]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_FetchDropDowns]
go

create procedure [dbo].[SP_FetchDropDowns]
(
@PageType varchar(50)
)
as
begin
	if(@PageType='UserMaster')
	begin
		select 'AgentCode,AgentBranch'
		select Agency,AgentCode from [dbo].[AGENTMASTER]
		select Agency,AgentBranch from [dbo].[AGENTMASTER]
	end

	--if(@PageType='UserProfile')
	--begin
	--    select 'BK_Nationality,BK_UserTitle,BK_Gender'
	--	select Code,Description from BK_Nationality order by Code
	--end

	if(@PageType='TravelInsurance')
	begin
	    select 'TravelInsurancePackage,TravelInsurancePeroid,Nationality,FamilyRelationShip
		,TravelCoverage,Introducedby,InsuredMasterDD,PaymentType'
		select Code,Name from TravelInsurancePackage 
		select Code,Name from TravelInsurancePeroid
	    select Code,Description from Nationality order by Code
		select ID,Relationship from FamilyRelationShip order by Relationship
		select Code,CoverageType from TravelCoverage
		select LookupCode,Description from IntroducedbyMaster
		select CPR,InsuredCode from InsuredMaster
		select Code,VALUE from DROPDOWN_LOOKUPMASTER where LOOKUPTYPECODE=200
	end

	--if(@PageType = 'MotorQuote')
	--begin
	--	select 'BK_MotorYearOfMake,BK_MotorExcess,BK_MotorVehicleModel,BK_MotorInsuranceType,BK_MotorType,BK_Months'
	--	select ID, Year from BK_MotorYearOfMake where [Year] >=  Year(getdate())-5 order by [Year] desc
	--	select distinct(MANUFACTURERID) as Make from [dbo].[BK_MotorExcess] order by MANUFACTURERID
	--	select distinct(MODELID) from [dbo].[BK_MotorExcess]
	--	select Code,ProductType from BK_MotorInsuranceType 
	--end

	--if(@PageType = 'MotorInsurance')
	--begin
	--	select 'BK_MotorYearOfMake,BK_MotorMake,BK_MotorInsuranceType,BK_MotorInsuranceCardDeliveryBranch,BK_MotorEngineCapacity,BK_ExcessOptions,BK_MotorFinancier,BK_MotorVehicleModel,BK_MotorType,BK_RegistrationMonth,BK_DeliveryOptions,BK_AddressDeliveryOptions,BK_CarReplacementDays'
	--	select ID, Year from BK_MotorYearOfMake where [Year] >=  Year(getdate())-5 order by [Year] desc
	--	select distinct(MANUFACTURERID) as Make from [dbo].[BK_MotorExcess] order by MANUFACTURERID
	--	select Code,ProductType from BK_MotorInsuranceType 
	--	select ID, Branch from BK_MotorInsuranceCardDeliveryBranch
	--	select Tonnage, Capacity from BK_MotorEngineCapacity
	--	select ExcessType,ExcessValue from BK_MotorExcessTypes
	--	select Code,Financier from BK_MotorFinancier
	--	select MODELID as Model from [dbo].[BK_MotorExcess] as BK_MotorVehicleModel
	--End

	--if(@PageType = 'HomeQuote')
	--begin
	--select 'BK_JewelleryCover,BK_PropertyInsured,BK_MaliciousCover,BK_HelpCover,BK_WorkersCover'
	--	select KeyType, ValueType from BK_JewelleryCover
	--end
	if(@PageType='DomesticHelp')
	begin
		select 'Nationality,DomesticWorkerOccupation'
		select Code,Description from Nationality order by Code
		select ID,Occupation from [dbo].DomesticWorkerOccupation
	end

	--if(@PageType = 'HomeInsurance')
	--begin
	--select 'BK_JewelleryCover,BK_MotorFinancier,BK_HomeInsuranceCategory,BK_PropertyInsured,BK_MaliciousCover,BK_HelpCover,BK_WorkersCover,BK_YesOrNo,BK_BuildingAge,BK_ResidentialType'
	--	select KeyType, ValueType from BK_JewelleryCover
	--	select Code,Financier from BK_MotorFinancier
	--	select * from [dbo].[BK_HomeInsuranceCategory]
	--end

	--if(@PageType='InsurancePortal')
	--begin
	--select 'BK_HIRStatus'
	--select * from BK_HIRStatus
	--end

end