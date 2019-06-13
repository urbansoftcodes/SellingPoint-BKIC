USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[SP_FetchDropDowns]    Script Date: 5/27/2018 2:03:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[SP_FetchDropDowns]
(
@PageType varchar(50)
)
as
begin
	if(@PageType='UserMaster')
	begin
		select 'AgentCodeDD,AgentBranchDD'
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
	    select 'TravelInsurancePackage,TravelInsurancePeroid,Nationality,FamilyRelationShip,TravelCoverage'
		select Code,Name from TravelInsurancePackage 
		select Code,Name from TravelInsurancePeroid
	    select Code,Description from Nationality order by Code
		select ID,Relationship from FamilyRelationShip order by Relationship
		select Code,CoverageType from TravelCoverage
		select LookupCode,Description from IntroducedbyMaster
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
	--if(@PageType='DomesticHelpInsurance')
	--begin
	--	select 'BK_Nationality,BK_DomesticWorkerOccupation,BK_DomesticInsuranceYear,BK_DomesticHelpWorkers,BK_DomesticWorkerType,BK_Gender'
	--select Code,Description from BK_Nationality order by Code
	--	select ID,Occupation from [dbo].BK_DomesticWorkerOccupation
	--end

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