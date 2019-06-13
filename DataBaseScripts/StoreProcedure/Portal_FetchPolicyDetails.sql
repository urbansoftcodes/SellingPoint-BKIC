USE [SellingPoint]
GO
/****** Object:  StoredProcedure [dbo].[Portal_FetchPolicyDetails]    Script Date: 6/13/2018 7:29:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create Procedure [dbo].[Portal_FetchPolicyDetails]
(
@HIRStatus int,
@InsuranceType nvarchar(25),
@DocumentNo nvarchar(50),
@InsuredCode nvarchar(50),
@CPR nvarchar(50),
@Source nvarchar(50),
@IsHIR bit,
@FilterType nvarchar(50),
@PolicyType nvarchar(20)

)
as
begin

/*Motor*/
if(@InsuranceType='MotorInsurance')
begin
if(@PolicyType='HIR')
	begin
	if(@FilterType='')
		begin
			select motor.MOTORID as ID,motor.INSUREDCODE,motor.LINKID,motor.DOCUMENTNO,motor.CPR,motor.INSUREDNAME,
			motor.CPR,hir.HIRStatus as [STATUS],
			motor.GROSSPREMIUM as NETPREMIUM,motor.SOURCE,ISNULL(motor.HIRStatus,0) as HIRStatus,motor.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyDocumentNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,motor.GROSSPREMIUM,motor.COMMENCEDATE,motor.EXPIRYDATE,motor.CREATEDDATE,motor.SUBCLASS,motor.MAINCLASS,motor.RENEWALCOUNT
				,(CASE WHEN (ISNULL(motor.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions
			from [dbo].[Motor] motor with(nolock) left join  HIRStatus hir with(nolock) on motor.HIRStatus=hir.StatusID
			--left join  HIRReason hirreason on motor.HIRReasonID =hirreason.StatusID
			where  motor.IsHIR=@IsHIR and motor.HIRStatus<>8 order by motor.createddate desc
		end
	else
		begin
		select motor.MOTORID as ID,motor.INSUREDCODE,motor.LINKID,motor.DOCUMENTNO,motor.CPR,motor.INSUREDNAME,
			motor.CPR,hir.HIRStatus as [STATUS],
			motor.GROSSPREMIUM as NETPREMIUM,motor.SOURCE,ISNULL(motor.HIRStatus,0) as HIRStatus,motor.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyDocumentNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable,
				motor.GROSSPREMIUM,motor.COMMENCEDATE,motor.EXPIRYDATE,motor.CREATEDDATE,motor.SUBCLASS,motor.SUBCLASS,motor.RENEWALCOUNT
				,(CASE WHEN (ISNULL(motor.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions,motor.TRANSACTION_NO as TransactionNo
			from [dbo].[ Motor] motor with(nolock) left join  HIRStatus hir with(nolock) on motor.HIRStatus=hir.StatusID
			--left join  HIRReason hirreason with(nolock) on motor.HIRReasonID =hirreason.StatusID
			where  motor.IsHIR=@IsHIR and  motor.HIRStatus<>8  and 
			motor.HIRStatus in (@HIRStatus) and(	  
			( motor.DOCUMENTNO like '%'+@DocumentNo +'%') or
			 (motor.INSUREDCODE like '%'+@DocumentNo +'%') or 
			 (motor.CPR like '%'+@DocumentNo +'%') or 
			  (motor.SOURCE like '%'+@DocumentNo+'%') or
			  (motor.MOBILENUMBER like '%'+@DocumentNo+'%') or
			   (motor.PAYMENT_AUTHORIZATION_CODE like '%'+@DocumentNo+'%') ) 


		end
	end

	if(@PolicyType='ActivePolicy')
	begin
	if(@FilterType='')
		begin
			select motor.MOTORID as ID,motor.INSUREDCODE,motor.LINKID,motor.DOCUMENTNO,motor.CPR,motor.INSUREDNAME,
			motor.CPR,hir.HIRStatus as [STATUS],
			motor.GROSSPREMIUM as NETPREMIUM,motor.SOURCE,ISNULL(motor.HIRStatus,0) as HIRStatus,motor.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from  HIRRequestDocuments WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyDocumentNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable,
				motor.GROSSPREMIUM,motor.COMMENCEDATE,motor.EXPIRYDATE,motor.CREATEDDATE,motor.SUBCLASS,motor.MAINCLASS,(Case when 
				motor.RENEWALCOUNT>0 then 1 else 0 end) as RENEWALCOUNT,'' as Actions,motor.TRANSACTION_NO as TransactionNo
			from [dbo].[ Motor] motor with(nolock) left join   HIRStatus hir with(nolock) on motor.HIRStatus=hir.StatusID
			--left join  HIRReason hirreason on motor.HIRReasonID =hirreason.StatusID
			where  motor.IsActive=1 and motor.SOURCE in ('BKIC')  order by  motor.paymentdate desc,motor.MOTORID desc 

		end
	else	
		begin
		 select motor.MOTORID as ID,motor.INSUREDCODE,motor.LINKID,motor.DOCUMENTNO,motor.CPR,motor.INSUREDNAME,motor.COMMENCEDATE,motor.EXPIRYDATE,
			motor.CPR,hir.HIRStatus as [STATUS],
			motor.GROSSPREMIUM as NETPREMIUM,motor.SOURCE,ISNULL(motor.HIRStatus,0) as HIRStatus,motor.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = motor.INSUREDCODE 
			    and PolicyDocumentNo = motor.DOCUMENTNO and LinkId = motor.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,motor.MAINCLASS,motor.SUBCLASS,motor.CREATEDDATE,(Case when 
				motor.RENEWALCOUNT>0 then 1 else 0 end) as RENEWALCOUNT,'' as Actions,motor.TRANSACTION_NO as TransactionNo
			from [dbo].[ Motor] motor with(nolock) left join   HIRStatus hir with(nolock) on motor.HIRStatus=hir.StatusID
		--	left join  HIRReason hirreason on motor.HIRReasonID =hirreason.StatusID
			where  motor.IsActive=1 and  (( motor.DOCUMENTNO like '%'+@DocumentNo +'%') or
			 (motor.INSUREDCODE like '%'+@DocumentNo +'%') or 
			 (motor.CPR like '%'+@DocumentNo +'%') or 
			  (motor.SOURCE like '%'+@DocumentNo+'%') or
			  (motor.MOBILENUMBER like '%'+@DocumentNo+'%') or
			   (motor.PAYMENT_AUTHORIZATION_CODE like '%'+@DocumentNo+'%') )
		end
end
	if(@PolicyType='Renewal')
	begin
	if(@FilterType='')
	begin
		select motor.MOTORID as ID,motor.INSUREDCODE,motor.LINKID,motor.DOCUMENTNO,motor.CPR,motor.INSUREDNAME,
			motor.CPR,
			motor.GROSSPREMIUM as NETPREMIUM,motor.SOURCE,0 as [Status],0 as HIRStatus,null as StatusDescription,0 as IsMessageAvailable 
			,0 as IsDocumentAvailable,motor.GROSSPREMIUM,motor.COMMENCEDATE,motor.LASTYEAREXPIRYDATE as EXPIRYDATE,motor.CREATEDDATE,motor.SUBCLASS,
			motor.MAINCLASS,motor.RENEWALCOUNT,'' as Actions,motor.TRANSACTION_NO as TransactionNo
			from [dbo].[RenewalMotor] motor with(nolock) 
			
	end
		else
	begin
	select motor.MOTORID as ID,motor.INSUREDCODE,motor.LINKID,motor.DOCUMENTNO,motor.CPR,motor.INSUREDNAME,
			motor.CPR,
			motor.GROSSPREMIUM as NETPREMIUM,motor.SOURCE,0 as [Status],null as StatusDescription,0 as IsMessageAvailable 
			,0 as IsDocumentAvailable,motor.GROSSPREMIUM,motor.COMMENCEDATE,motor.LASTYEAREXPIRYDATE as EXPIRYDATE,motor.CREATEDDATE,motor.SUBCLASS,motor.MAINCLASS,
			motor.RENEWALCOUNT,'' as Actions,0 as HIRStatus,motor.TRANSACTION_NO as TransactionNo
			from [dbo].[RenewalMotor] motor with(nolock) where  ((motor.DOCUMENTNO like '%'+@DocumentNo +'%')
			or (motor.CPR like '%'+@DocumentNo+'%') or (motor.INSUREDNAME like '%'+@DocumentNo+'%')) 
	end
end
end


/*Travel*/
if(@InsuranceType='TravelInsurance')
begin
if(@PolicyType='HIR')
begin
	if(@FilterType='')
	begin
		select travel.travelId as ID,travel.INSUREDCODE,travel.LINKID,travel.DOCUMENTNO,travel.CPR,travel.INSUREDNAME,
		travel.CPR,hir.HIRStatus as [STATUS],
		travel.PREMIUMAFTERDISCOUNT as NETPREMIUM,travel.SOURCE,ISNULL(travel.HIRStatus,0) as HIRStatus,travel.HIRReason as StatusDescription,
		(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyDocumentNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,travel.PREMIUMAFTERDISCOUNT,travel.COMMENCEDATE,travel.EXPIRYDATE,travel.CREATEDDATE,travel.MAINCLASS,travel.SUBCLASS,
				travel.RENEWALCOUNT,(CASE WHEN (ISNULL(travel.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions,travel.TRANSACTION_NO as TransactionNo
		from [dbo].[Travel] travel with(nolock) left join HIRStatus hir with(nolock) on travel.HIRStatus=hir.StatusID
		--left join  HIRReason hirreason on travel.HIRReasonID =hirreason.StatusID
		where  travel.IsHIR=@IsHIR and travel.HIRStatus<>8 order by travel.createddate desc
	end
	else
	begin
		select travel.travelId as ID,travel.INSUREDCODE,travel.LINKID,travel.DOCUMENTNO,travel.CPR,travel.INSUREDNAME,
		travel.CPR,hir.HIRStatus as [STATUS],
		travel.PREMIUMAFTERDISCOUNT as NETPREMIUM,travel.SOURCE,ISNULL(travel.HIRStatus,0) as HIRStatus,travel.HIRReason as StatusDescription,
		(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyDocumentNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,travel.PREMIUMAFTERDISCOUNT,travel.COMMENCEDATE,travel.EXPIRYDATE,travel.CREATEDDATE,travel.MAINCLASS,travel.SUBCLASS,
				travel.RENEWALCOUNT,(CASE WHEN (ISNULL(travel.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions,travel.TRANSACTION_NO as TransactionNo
		from [dbo].[Travel] travel with(nolock) left join  HIRStatus hir with(nolock) on travel.HIRStatus=hir.StatusID
		--left join  HIRReason hirreason on travel.HIRReasonID =hirreason.StatusID
		where travel.IsHIR=@IsHIR and travel.HIRStatus<>8  and travel.HIRStatus in (@HIRStatus) and(		 
		(travel.DOCUMENTNO like '%'+@DocumentNo+'%')or
		(travel.INSUREDCODE like '%'+@DocumentNo+'%') or
		(travel.CPR like '%'+@DocumentNo +'%') or 
		(travel.SOURCE like '%'+@DocumentNo+'%'))
	end
end
else
begin
		if(@PolicyType='ActivePolicy' and @FilterType='' )
			begin
			select travel.travelId as ID,travel.INSUREDCODE,travel.LINKID,travel.DOCUMENTNO,travel.CPR,travel.INSUREDNAME,
				travel.CPR,hir.HIRStatus as [STATUS],
				travel.PREMIUMAFTERDISCOUNT as NETPREMIUM,travel.SOURCE,ISNULL(travel.HIRStatus,0) as HIRStatus,travel.HIRReason as StatusDescription,
				(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyDocumentNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,travel.MAINCLASS,travel.SUBCLASS,travel.RENEWALCOUNT
				,travel.PREMIUMAFTERDISCOUNT,travel.COMMENCEDATE,travel.EXPIRYDATE,travel.CREATEDDATE,'' as Actions
				from [dbo].[Travel] travel with(nolock) left join  HIRStatus hir with(nolock) on travel.HIRStatus=hir.StatusID
				--left join  HIRReason hirreason on travel.HIRReasonID =hirreason.StatusID
				where  travel.IsActive=1 and travel.Source in ('BKIC') order by travel.paymentdate desc,travel.TRAVELID desc  
			end
		else
			begin
			select travel.travelId as ID,travel.INSUREDCODE,travel.LINKID,travel.DOCUMENTNO,travel.CPR,travel.INSUREDNAME,
				travel.CPR,hir.HIRStatus as [STATUS],
				travel.PREMIUMAFTERDISCOUNT as NETPREMIUM,travel.SOURCE,ISNULL(travel.HIRStatus,0) as HIRStatus,travel.HIRReason as StatusDescription,
				(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = travel.INSUREDCODE 
			    and PolicyDocumentNo = travel.DOCUMENTNO and LinkId = travel.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,travel.PREMIUMAFTERDISCOUNT,travel.COMMENCEDATE,travel.EXPIRYDATE,travel.CREATEDDATE,travel.MAINCLASS,travel.SUBCLASS
				,travel.RENEWALCOUNT,'' as Actions
				from [dbo].[Travel] travel with(nolock) left join  HIRStatus hir with(nolock) on travel.HIRStatus=hir.StatusID
				--left join  HIRReason hirreason on travel.HIRReasonID =hirreason.StatusID
				where  travel.IsActive=1   and (
		(travel.DOCUMENTNO like '%'+@DocumentNo+'%')or
		(travel.INSUREDCODE like '%'+@DocumentNo+'%') or
		(travel.CPR like '%'+@DocumentNo +'%') or 
		(travel.SOURCE like '%'+@DocumentNo+'%'))
			end
	end
end


/*Domestic*/
if( @InsuranceType='DomesticInsurance')
begin
if (@PolicyType='HIR')
begin
	if(@FilterType='')
	begin
		select domestic.DOMESTICID as ID,domestic.INSUREDCODE,domestic.LINKID,domestic.DOCUMENTNO,domestic.CPR,domestic.INSUREDNAME,
		domestic.CPR,hir.HIRStatus as [STATUS],
		domestic.PREMIUMAMOUNT as NETPREMIUM,domestic.SOURCE,ISNULL(domestic.HIRStatus,0) as HIRStatus,hirreason.StatusDescription,
		(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyDocumentNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,domestic.PREMIUMAMOUNT,domestic.COMMENCEDATE,domestic.EXPIRYDATE,domestic.CREATEDDATE,domestic.MAINCLASS,domestic.SUBCLASS
				,domestic.RENEWALCOUNT,(CASE WHEN (ISNULL(domestic.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions,domestic.TRANSACTION_NO as TransactionNo
		from [dbo].[ DomesticHelp] domestic with(nolock) left join  HIRStatus hir with(nolock) on domestic.HIRStatus=hir.StatusID
		left join  HIRReason hirreason on domestic.HIRReasonID =hirreason.StatusID
		where domestic.IsHIR=@IsHIR and domestic.HIRStatus<>8 order by domestic.PAYMENTDATE desc,domestic.createddate desc 
	end
	else
	begin
		select domestic.DOMESTICID as ID,domestic.INSUREDCODE,domestic.LINKID,domestic.DOCUMENTNO,domestic.CPR,domestic.INSUREDNAME,
		domestic.CPR,hir.HIRStatus as [STATUS],
		domestic.PREMIUMAMOUNT as NETPREMIUM,domestic.SOURCE,ISNULL(domestic.HIRStatus,0) as HIRStatus,hirreason.StatusDescription,
		(CASE WHEN (select COUNT(*) from  EMAILMESSAGEAUDIT WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from  HIRRequestDocuments WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyDocumentNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,domestic.PREMIUMAMOUNT,domestic.COMMENCEDATE,domestic.EXPIRYDATE,domestic.CREATEDDATE,domestic.MAINCLASS,domestic.SUBCLASS
				,domestic.RENEWALCOUNT,(CASE WHEN (ISNULL(domestic.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions,domestic.TRANSACTION_NO as TransactionNo
		from [dbo].[ DomesticHelp] domestic with(nolock) left join   HIRStatus hir with(nolock) on domestic.HIRStatus=hir.StatusID
		left join  HIRReason hirreason on domestic.HIRReasonID =hirreason.StatusID
		where domestic.IsHIR=@IsHIR and domestic.HIRStatus<>8 and  domestic.HIRStatus in (@HIRStatus) and   
			(( domestic.DOCUMENTNO like '%'+@DocumentNo +'%') or
			 (domestic.INSUREDCODE like '%'+@DocumentNo +'%') or 
			 (domestic.CPR like '%'+@DocumentNo +'%') or 
			  (domestic.SOURCE like '%'+@DocumentNo+'%'))
	end
end
if(@PolicyType='ActivePolicy' and @FilterType='')
begin
select domestic.DOMESTICID as ID,domestic.INSUREDCODE,domestic.LINKID,domestic.DOCUMENTNO,domestic.CPR,domestic.INSUREDNAME,
	domestic.CPR,hir.HIRStatus as [STATUS],
	domestic.PREMIUMAMOUNT as NETPREMIUM,domestic.SOURCE,ISNULL(domestic.HIRStatus,0) as HIRStatus,hirreason.StatusDescription,
	(CASE WHEN (select COUNT(*) from  EMAILMESSAGEAUDIT WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from  HIRRequestDocuments WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyDocumentNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,domestic.RENEWALCOUNT,'' as Actions
				,domestic.PREMIUMAMOUNT,domestic.COMMENCEDATE,domestic.EXPIRYDATE,domestic.CREATEDDATE,domestic.MAINCLASS,domestic.SUBCLASS
	from [dbo].[ DomesticHelp] domestic with(nolock) left join   HIRStatus hir with(nolock) on domestic.HIRStatus=hir.StatusID
	left join  HIRReason hirreason on domestic.HIRReasonID =hirreason.StatusID
		where  domestic.IsActive=1 order by domestic.PAYMENTDATE desc,domestic.DOMESTICID desc
end
else
begin
select domestic.DOMESTICID as ID,domestic.INSUREDCODE,domestic.LINKID,domestic.DOCUMENTNO,domestic.CPR,domestic.INSUREDNAME,
	domestic.CPR,hir.HIRStatus as [STATUS],
	domestic.PREMIUMAMOUNT as NETPREMIUM,domestic.SOURCE,ISNULL(domestic.HIRStatus,0) as HIRStatus,hirreason.StatusDescription,
	(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from HIRRequestDocuments WHERE INSUREDCODE = domestic.INSUREDCODE 
			    and PolicyDocumentNo = domestic.DOCUMENTNO and LinkId = domestic.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,domestic.PREMIUMAMOUNT,domestic.COMMENCEDATE,domestic.EXPIRYDATE,domestic.CREATEDDATE,domestic.MAINCLASS,domestic.SUBCLASS
				,domestic.RENEWALCOUNT,'' as Actions
	from [dbo].[DomesticHelp] domestic with(nolock) left join  HIRStatus hir with(nolock) on domestic.HIRStatus=hir.StatusID
	left join  HIRReason hirreason on domestic.HIRReasonID =hirreason.StatusID
		where  domestic.IsActive=1 and (
			( domestic.DOCUMENTNO like '%'+@DocumentNo +'%') or
			 (domestic.INSUREDCODE like '%'+@DocumentNo +'%') or 
			 (domestic.CPR like '%'+@DocumentNo +'%') or 
			  (domestic.SOURCE like '%'+@DocumentNo+'%'))

end
end
/*Home Insurance*/
if(@InsuranceType='HomeInsurance')
begin
if( @PolicyType='HIR')
begin
if(@FilterType='')
		begin
			select home.HOMEID as ID,home.INSUREDCODE,home.LINKID,home.DOCUMENTNO,insured.CPR,home.INSUREDNAME,
			hir.HIRStatus as [STATUS],
			home.PREMIUMAMOUNT as NETPREMIUM,home.SOURCE,ISNULL(home.HIRStatus,0) as HIRStatus,
			home.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = home.INSUREDCODE 
			    and PolicyNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from  HIRRequestDocuments WHERE INSUREDCODE = home.INSUREDCODE 
			    and PolicyDocumentNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,home.PREMIUMAMOUNT,home.COMMENCEDATE,home.EXPIRYDATE,home.CREATEDDATE,home.MAINCLASS,home.SUBCLASS,0 as RENEWALCOUNT
				,(CASE WHEN (ISNULL(home.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions,home.TRANSACTION_NO as TransactionNo
			 from [dbo].[Home] home with(nolock)
			 left join  HIRStatus hir with(nolock) on home.HIRStatus=hir.StatusID inner join [dbo].[InsuredMaster] insured with(nolock)
			  on home.INSUREDCODE=insured.INSUREDCODE 
			-- left join  HIRReason hirreason on home.HIRReasonID =hirreason.StatusID 
			 where home.IsHIR=@IsHIR and home.HIRStatus<>8 order by home.createddate desc
	
	
		end
else
		begin
			select home.HOMEID as ID,home.INSUREDCODE,home.LINKID,home.DOCUMENTNO,insured.CPR,home.INSUREDNAME,
			hir.HIRStatus as [STATUS],
			home.PREMIUMAMOUNT as NETPREMIUM,home.SOURCE,ISNULL(home.HIRStatus,0) as HIRStatus,
			home.HIRReason as StatusDescription,
			(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = home.INSUREDCODE 
			    and PolicyNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
				(CASE WHEN (select COUNT(*) from  HIRRequestDocuments WHERE INSUREDCODE = home.INSUREDCODE 
			    and PolicyDocumentNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
				,home.PREMIUMAMOUNT,home.COMMENCEDATE,home.EXPIRYDATE,home.CREATEDDATE,home.MAINCLASS,home.SUBCLASS,0 as RENEWALCOUNT
				,(CASE WHEN (ISNULL(home.HIRStatus,0) in (4,8,9)) then 'Client' else 'BKIC' end) as Actions,home.TRANSACTION_NO as TransactionNo
			 from [dbo].[Home] home with(nolock)
			 left join  HIRStatus hir with(nolock) on home.HIRStatus=hir.StatusID inner join [dbo].[ InsuredMaster] insured with(nolock)
			 on home.INSUREDCODE=insured.INSUREDCODE
			-- left join  HIRReason hirreason on home.HIRReasonID =hirreason.StatusID
			  where home.IsHIR=@IsHIR and home.HIRStatus<>8 and  home.HIRStatus in (@HIRStatus) and  
			(( home.DOCUMENTNO like '%'+@DocumentNo +'%' ) or
			 (home.INSUREDCODE like '%'+@DocumentNo +'%') or 
			 (insured.CPR like '%'+@DocumentNo +'%') or 
			  (home.SOURCE like '%'+@DocumentNo+'%'))
	
		end
end
if(@PolicyType='ActivePolicy')
begin
if(@FilterType='')
begin
	 select home.HOMEID as ID,home.INSUREDCODE,home.LINKID,home.DOCUMENTNO,home.CPR,home.INSUREDNAME,
		hir.HIRStatus as [STATUS],
		home.PREMIUMAMOUNT as NETPREMIUM,home.SOURCE,ISNULL(home.HIRStatus,0) as HIRStatus,
		home.HIRReason as StatusDescription,(CASE WHEN (select COUNT(*) from  EMAILMESSAGEAUDIT WHERE INSUREDCODE = home.INSUREDCODE 
					and PolicyNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
					(CASE WHEN (select COUNT(*) from  HIRRequestDocuments WHERE INSUREDCODE = home.INSUREDCODE 
					and PolicyDocumentNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
					,home.PREMIUMAMOUNT,home.COMMENCEDATE,home.EXPIRYDATE,home.CREATEDDATE,home.MAINCLASS,home.SUBCLASS,0 as RENEWALCOUNT,
					'' as Actions,home.TRANSACTION_NO as TransactionNo
		 from [dbo].[ Home] home with(nolock)
		 left join   HIRStatus hir with(nolock) on home.HIRStatus=hir.StatusID 
		 --left join [dbo].[ InsuredMaster] insured with(nolock)
		 -- on home.INSUREDCODE=insured.INSUREDCODE 
		-- left join  HIRReason hirreason on home.HIRReasonID =hirreason.StatusID 
		where  home.IsActive=1 and home.Source in ('BKIC') order by home.PAYMENTDATE desc,home.HOMEID desc
end
else
	begin
		 select home.HOMEID as ID,home.INSUREDCODE,home.LINKID,home.DOCUMENTNO,home.CPR,home.INSUREDNAME,
		hir.HIRStatus as [STATUS],
		home.PREMIUMAMOUNT as NETPREMIUM,home.SOURCE,ISNULL(home.HIRStatus,0) as HIRStatus,
		home.HIRReason as StatusDescription,(CASE WHEN (select COUNT(*) from EMAILMESSAGEAUDIT WHERE INSUREDCODE = home.INSUREDCODE 
					and PolicyNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsMessageAvailable,
					(CASE WHEN (select COUNT(*) from  HIRRequestDocuments WHERE INSUREDCODE = home.INSUREDCODE 
					and PolicyDocumentNo = home.DOCUMENTNO and LinkId = home.LINKID ) > 0 THEN 1 ELSE 0 END) as IsDocumentAvailable
					,home.PREMIUMAMOUNT,home.COMMENCEDATE,home.EXPIRYDATE,home.CREATEDDATE,home.MAINCLASS,home.SUBCLASS,0 as RENEWALCOUNT,
					'' as Actions,home.TRANSACTION_NO as TransactionNo
		 from [dbo].[Home] home with(nolock)
		 left join  HIRStatus hir with(nolock) on home.HIRStatus=hir.StatusID 
		 --left join [dbo].[ InsuredMaster] insured with(nolock)
		  --on home.INSUREDCODE=insured.INSUREDCODE 
		 --left join  HIRReason hirreason on home.HIRReasonID =hirreason.StatusID 
		where  home.IsActive=1  and ( 
				( home.DOCUMENTNO like '%'+@DocumentNo +'%' ) or
				 (home.INSUREDCODE like '%'+@DocumentNo +'%') or 
				 (home.CPR like '%'+@DocumentNo +'%') or 
				  (home.SOURCE like '%'+@DocumentNo+'%'))
	end
end
end
if(@PolicyType='Renewal')
	begin
	if(@FilterType='')
	begin
		select home.HOMEID as ID,home.INSUREDCODE,home.LINKID,home.DOCUMENTNO,home.CPR,home.INSUREDNAME,
			home.CPR,
			home.PREMIUMAMOUNT as NETPREMIUM,home.SOURCE,0 as [Status],0 as HIRStatus,null as StatusDescription,0 as IsMessageAvailable 
			,0 as IsDocumentAvailable,home.ORIGINALPREMIUMAMOUNT,home.COMMENCEDATE,home.LASTYEAREXPIRYDATE as EXPIRYDATE,home.CREATEDDATE,home.SUBCLASS,
			home.MAINCLASS,'' as RENEWALCOUNT,'' as Actions,home.TRANSACTION_NO as TransactionNo
			from [dbo].renewalhome home with(nolock)  order by home.CREATEDDATE desc
			
	end
		else
	begin
		select home.HOMEID as ID,home.INSUREDCODE,home.LINKID,home.DOCUMENTNO,home.CPR,home.INSUREDNAME,
			home.CPR,
			home.PREMIUMAMOUNT as NETPREMIUM,home.SOURCE,0 as [Status],0 as HIRStatus,null as StatusDescription,0 as IsMessageAvailable 
			,0 as IsDocumentAvailable,home.ORIGINALPREMIUMAMOUNT,home.COMMENCEDATE,home.LASTYEAREXPIRYDATE as EXPIRYDATE,home.CREATEDDATE,home.SUBCLASS,
			home.MAINCLASS,'' as RENEWALCOUNT,'' as Actions,home.TRANSACTION_NO as TransactionNo
			from [dbo].renewalhome home with(nolock) where  home.DOCUMENTNO like '%'+@DocumentNo +'%' or home.DOCUMENTNO is Null
	end
end

end
