drop procedure SP_Admin_FetchMotorPolicy
go
Create Procedure SP_Admin_FetchMotorPolicy
(
@Type varchar(50),
@Filter varchar(50),
@AgencyCode varchar(100),
@DocumentNo varchar(50)
)
as
begin
set nocount on
/*HIR */
if(@Type='HIR' and @Filter='All')
begin

 select motor.MOTORID as ID,motor.DOCUMENTNO,motor.INSUREDNAME,
			motor.GROSSPREMIUM as NETPREMIUM,motor.AGENTCODE,motor.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from SP_EMAILMESSAGEAUDIT with(nolock) WHERE INSUREDCODE = motor.INSUREDCODE 
			    and DOCUMENTNO = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from SP_HIRRequestDocuments with(nolock) WHERE INSUREDCODE = motor.INSUREDCODE 
			    and DOCUMENTNO = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,motor.MAINCLASS,motor.SUBCLASS,motor.CREATEDDATE
			from [dbo].[Motor] motor with(nolock) left join  SP_HIRStatus hir with(nolock) on motor.HIRStatus=hir.StatusID
			where  motor.IsHIR=1 

end

if(@Type='HIR' and @Filter='')
begin
 select motor.MOTORID as ID,motor.DOCUMENTNO,motor.INSUREDNAME,
			motor.GROSSPREMIUM,motor.AGENTCODE,motor.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from SP_EMAILMESSAGEAUDIT WHERE INSUREDCODE = motor.INSUREDCODE 
			    and DOCUMENTNO = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from SP_HIRRequestDocuments WHERE INSUREDCODE = motor.INSUREDCODE 
			    and DOCUMENTNO = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,motor.MAINCLASS,motor.SUBCLASS,motor.CREATEDDATE
			from [dbo].[Motor] motor with(nolock) left join  SP_HIRStatus hir with(nolock) on motor.HIRStatus=hir.StatusID
			where  motor.IsHIR=1 and  (( motor.DOCUMENTNO like '%'+@DocumentNo +'%') or
			 (motor.AGENTCODE like '%'+@AgencyCode +'%'))  
			
end


/*Active*/
if(@Type='Active' and @Filter='All')
begin

select MOTORID,INSUREDNAME,DOCUMENTNO,AGENTCODE,GROSSPREMIUM,PAYMENT_TYPE,PAYMENTDATE,ACCOUNTNO from Motor with(nolock) where 
IsAuthorized=1 
end
if(@Type='Active' and @Filter='')
begin
select MOTORID,INSUREDNAME,DOCUMENTNO,AGENTCODE,GROSSPREMIUM,PAYMENT_TYPE,PAYMENTDATE,ACCOUNTNO,SUBCLASS,CREATEDDATE from Motor with(nolock) where 
IsAuthorized=1 and (( DOCUMENTNO like '%'+@DocumentNo +'%') or
			 (AGENTCODE like '%'+@AgencyCode +'%'))

end
--if(@Type='Renewal')
--begin


--end

set nocount off
end