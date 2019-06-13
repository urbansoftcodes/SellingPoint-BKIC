USE [SellingPoint]

create Procedure [dbo].[SP_GetMyDomesticSavedQuotationsByDomesticId]
(
@DomesticID int,
@InsuredCode varchar(50)
)
as
BEGIN
if exists(SELECT * FROM DomesticHelp with(nolock)  WHERE DOMESTICID = @DomesticID AND INSUREDCODE = @InsuredCode)
begin
Declare @linkid nvarchar(50),@DocumentNo nvarchar(50),@DomesticWorkType nvarchar(30),
@IsPhysicalDefect nvarchar(20),@PhysicalDefectDesc nvarchar(max)

select @linkid=LINKID,@DocumentNo=DOCUMENTNO  from DomesticHelp with(nolock) where 
DOMESTICID = @DomesticID AND INSUREDCODE = @InsuredCode

select @DomesticWorkType=ANSWER from Questionnaire with(nolock) where LINKID=@linkid and DOCUMENTNO=DOCUMENTNO and CODE='QST_DH_001'
select @IsPhysicalDefect=ANSWER,@PhysicalDefectDesc=REMARKS from Questionnaire with(nolock) where LINKID=@linkid and DOCUMENTNO=DOCUMENTNO and CODE='QST_DH_002'

	SELECT domestic.INSUREDCODE, domestic.INSURANCEPERIOD,domestic.COMMENCEDATE,domestic.EXPIRYDATE,details.CPR,
	domestic.INSUREDNAME,domestic.PREMIUMAMOUNT,domestic.ORIGINALPREMIUMAMOUNT,domestic.DOCUMENTNO,domestic.SUMINSURED,@DomesticWorkType as DomesticWorkerType,
	@IsPhysicalDefect as PhysicalDefect,@PhysicalDefectDesc as PhysicalDesc,domestic.IsHIR
	FROM DomesticHelp domestic with(nolock) inner join InsuredMaster details with(nolock) on domestic.INSUREDCODE=
	details.INSUREDCODE 
	 WHERE domestic.DOMESTICID = @DomesticID 
	
	select * from DomesticHelpMemberDetails  with(nolock) where DOMESTICID=@DomesticID
	
	end
END
