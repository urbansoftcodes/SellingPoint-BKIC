
Alter Procedure SP_InsertPolicyCategory
(
@LinkID varchar(50),
@ParentLinkID varchar(50),/*only for Endorsement */
@DocumentNo varchar(50),
@DocumentType varchar(50),/*for policy POL and endorsement POLE*/
@EndorsementNo varchar(50),
@EndoresementCount varchar(50),
@AgentCode varchar(50),
@MainClass varchar(50),
@Subclass varchar(50),
@PremiumAmount decimal(18,3)
)
as
begin
Declare @CurrentDate datetime
set @CurrentDate=GETDATE()

Create table #policycategory(
AgentCode varchar(50),DocumentNo varchar(50),DocumentType varchar(50),
EndorsementNo varchar(50),EndorsementCount varchar(50),LinkID varchar(50),
[LineNo] int identity(1,1),Code varchar(50),Category varchar(50),ValueType varchar(10),
Value decimal(18,3),Premium decimal(18,3),ParentLinkId varchar(50),CatValue decimal(18,1)
)

insert into #policycategory(AgentCode,DocumentNo,DocumentType,EndorsementNo,EndorsementCount,LinkID
,Code,Category,ValueType,ParentLinkId,CatValue) 
select AgenctCode,@DocumentNo,@DocumentType,@EndorsementNo,@EndoresementCount,
@LinkID,Code,Category,ValueType,@ParentLinkID,Value from CATEGORY_MASTER with(nolock) where AgenctCode=@AgentCode and 
(EffectiveFrom <= @CurrentDate AND EffectiveTo   >= @CurrentDate) and [Status]=1

/*Define All Codes*/
/*Percentage*/
Update #policycategory set Value=@PremiumAmount*(CatValue/100) where Code='AGTCOMM' and ValueType='P'
Update #policycategory set Value=@PremiumAmount*(CatValue/100) where Code='NEWCOMM' and ValueType='P'
Update #policycategory set Value=@PremiumAmount*(CatValue/100) where Code='SRCCCOMM' and ValueType='P'

/*Amount*/
Update #policycategory set Value=CatValue where Code='AGTCOMM' and ValueType='A'
Update #policycategory set Value=CatValue where Code='NEWCOMM' and ValueType='A'
Update #policycategory set Value=CatValue where Code='SRCCCOMM' and ValueType='A'

/*Value*/


Insert into PolicyCategory(AGENTCODE,DOCUMENTNO,DOCUMENTTYPE,ENDORSEMENTNO,ENDORSEMENTCOUNT,LINKID,
CODE,CATEGORY,VALUETYPE,PARENTLINKID,VALUE)
select AgentCode,DocumentNo,DocumentType,EndorsementNo,EndorsementCount,LinkID
,Code,Category,ValueType,ParentLinkId,value from #policycategory


IF OBJECT_ID('tempdb..#policycategory') IS NOT NULL
  /*Then it exists*/
  DROP TABLE #policycategory

end