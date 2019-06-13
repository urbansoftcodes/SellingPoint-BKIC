--USE [SellingPoint]

Create Procedure [dbo].[SP_GetMyTravelSavedQuotationsByTravelId]
(
@TravelId int, 
@InsuredCode varchar(50),
@Type nvarchar(20)
)
as
BEGIN
if(@Type='Portal')
begin
SELECT travel.INSUREDNAME,travel.DOCUMENTNO,travel.SUMINSURED,travel.PREMIUMBEFOREDISCOUNT,travel.COMMENCEDATE,
		travel.EXPIRYDATE,travel.MAINCLASS,travel.SUBCLASS,travel.PASSPORT,travel.OCCUPATION,travel.PERIODOFCOVER,
		travel.LOADAMOUNT,travel.PREMIUMAFTERDISCOUNT,travel.CODE,travel.CPR,travel.MOBILENUMBER,travel.CREATEDDATE,travel.UPDATEDDATE,
		question.ANSWER,question.REMARKS,travel.IsHIR,travel.FFPNUMBER,travel.CoverageType FROM Travel travel with(nolock)  left join 
		Questionnaire question with(nolock) on travel.LINKID=question.LINKID WHERE travel.TRAVELID = @TravelId 
		
		SELECT * FROM TravelMembers with(nolock) WHERE TRAVELID = @TravelId 
		select * from InsuredMaster with(nolock) where INSUREDCODE=@InsuredCode
end
else
	begin
	if exists(SELECT * FROM Travel with(nolock)  WHERE TRAVELID = @TravelId)
	begin
		SELECT travel.INSUREDNAME,travel.DOCUMENTNO,travel.SUMINSURED,travel.PREMIUMBEFOREDISCOUNT,travel.COMMENCEDATE,
		travel.EXPIRYDATE,travel.MAINCLASS,travel.SUBCLASS,travel.PASSPORT,travel.OCCUPATION,travel.PERIODOFCOVER,
		travel.LOADAMOUNT,travel.PREMIUMAFTERDISCOUNT,travel.CODE,travel.CPR,travel.MOBILENUMBER,travel.CREATEDDATE,travel.UPDATEDDATE,
		question.ANSWER,question.REMARKS,travel.IsHIR,travel.FFPNUMBER,travel.CoverageType FROM Travel travel with(nolock)  left join Questionnaire question with(nolock) on travel.LINKID=question.LINKID WHERE travel.TRAVELID = @TravelId AND travel.INSUREDCODE = @InsuredCode 
		SELECT * FROM TravelMembers with(nolock) WHERE TRAVELID = @TravelId 
		select * from InsuredMaster with(nolock) where INSUREDCODE=@InsuredCode
	end

	end
END
