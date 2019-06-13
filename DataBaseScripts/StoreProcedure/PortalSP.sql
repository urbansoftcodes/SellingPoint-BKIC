

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Admin_GetEmailMessageForRecord]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Admin_GetEmailMessageForRecord]
go
Create Procedure [dbo].[SP_Admin_GetEmailMessageForRecord]
(
	@InsuredCode varchar(250),
	@PolicyNo varchar(250), 
	@LinkId varchar(250)
)
AS
BEGIN
	SELECT * FROM BK_EMAILMESSAGEAUDIT with(nolock) WHERE InsuredCode = @InsuredCode AND PolicyNo = @PolicyNo AND LinkId = @LinkId
		Order By CreatedDate;
END


go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Admin_UpdateHIRStatus]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Admin_UpdateHIRStatus]
go
Create Procedure [dbo].[SP_Admin_UpdateHIRStatus]
(
@ID bigint,
@PolicyNo nvarchar(50),
@HIRStatusCode int,
@InsuranceType nvarchar(25)
)
as
begin
Begin Tran
	if(@InsuranceType='MotorInsurance')
	begin
		update [dbo].[BK_Motor] set HIRStatus=@HIRStatusCode where MOTORID=@ID
 	end

	if(@InsuranceType='TravelInsurance')
	begin
		update [dbo].[BK_Travel] set HIRStatus=@HIRStatusCode where TRAVELID=@ID
 	end

	if(@InsuranceType='HomeInsurance')
	begin
		update [dbo].[BK_Home] set HIRStatus=@HIRStatusCode where HOMEID=@ID 
 	end

	if(@InsuranceType='DomesticInsurance')
	begin
		update [dbo].[BK_DomesticHelp] set HIRStatus=@HIRStatusCode where DOMESTICID=@ID 
 	end

Commit tran
end

go
/*type--start*/
CREATE TYPE [dbo].[HIRRequestDocumentsDataTable] AS TABLE(
	[InsuredCode] [nvarchar](50) NULL,
	[PolicyDocumentNo] [nvarchar](50) NULL,
	[LinkID] [nvarchar](200) NULL,
	[FilesURL] [nvarchar](max) NULL,
	[FileName] [nvarchar](max) NULL
)
/*type--end*/
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Admin_InsertHIRRequestDocuments]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Admin_InsertHIRRequestDocuments]
go
Create procedure [dbo].[SP_Admin_InsertHIRRequestDocuments]
(
	@Documents as dbo.HIRRequestDocumentsDataTable readonly
)
AS
BEGIN
	INSERT INTO [dbo].[BK_HIRRequestDocuments](InsuredCode,PolicyDocumentNo,LinkID,FilesURL,FileName,CreatedDate)
	 SELECT d.InsuredCode,d.PolicyDocumentNo,d.LinkID,d.FilesURL,d.FileName,GETDATE() from @Documents d
END
go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Admin_FetchHIRDocuments]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Admin_FetchHIRDocuments]
go
Create Procedure [dbo].[SP_Admin_FetchHIRDocuments]
(
@InsuredCode nvarchar(50),
@PolicyDocNo nvarchar(50),
@LinkID nvarchar(50)
)
as
begin
select * from [dbo].[BK_HIRRequestDocuments] where InsuredCode=@InsuredCode and PolicyDocumentNo=@PolicyDocNo and LinkID=@LinkID

end

go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Admin_HIRDocumentsUploadPrecheck]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Admin_HIRDocumentsUploadPrecheck]
go
Create Procedure [dbo].[SP_Admin_HIRDocumentsUploadPrecheck]
(
@InsuredCode nvarchar(50),
@PolicyDocNo nvarchar(50),
@LinkID nvarchar(50),
@InsuranceType nvarchar(50),
@IsValidUser bit out,
@IsDocumentsCanUploaded bit out
)
as
begin
Declare @HIRStatus int
set @IsValidUser=0
set @IsDocumentsCanUploaded=0
if(@InsuranceType='MotorInsurance')
	begin
	if exists(select * from BK_Motor with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo)
	begin
			set @IsValidUser=1
			select @HIRStatus=HIRStatus from BK_Motor with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo
			if(@HIRStatus=4)
			set @IsDocumentsCanUploaded=1
			else
			set @IsDocumentsCanUploaded=0
	end
	end
	if(@InsuranceType='TravelInsurance')
	begin
	if exists(select * from BK_Travel with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo)
	begin
			set @IsValidUser=1
			select @HIRStatus=HIRStatus from BK_Travel with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo
			if(@HIRStatus=4)
			set @IsDocumentsCanUploaded=1
			else
			set @IsDocumentsCanUploaded=0
	end
	end

	if(@InsuranceType='HomeInsurance')
	begin
	if exists(select * from BK_Home with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo)
	begin
			set @IsValidUser=1
			select @HIRStatus=HIRStatus from BK_Home with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo
			if(@HIRStatus=4)
			set @IsDocumentsCanUploaded=1
			else
			set @IsDocumentsCanUploaded=0
	end
	end

	if(@InsuranceType='DomesticInsurance')
	begin
	if exists(select * from BK_DomesticHelp with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo)
	begin
			set @IsValidUser=1
			select @HIRStatus=HIRStatus from BK_DomesticHelp with(nolock) where INSUREDCODE=@InsuredCode and LINKID=@LinkID and DOCUMENTNO=@PolicyDocNo
			if(@HIRStatus=4)
			set @IsDocumentsCanUploaded=1
			else
			set @IsDocumentsCanUploaded=0
	end
	end

end


go
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Admin_GetEmailMessageForRecord]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Admin_GetEmailMessageForRecord]
go
Create Procedure [dbo].[SP_Admin_GetEmailMessageForRecord]
(
	@InsuredCode varchar(250),
	@PolicyNo varchar(250), 
	@LinkId varchar(250)
)
AS
BEGIN
	SELECT * FROM BK_EMAILMESSAGEAUDIT with(nolock) WHERE InsuredCode = @InsuredCode AND PolicyNo = @PolicyNo AND LinkId = @LinkId
		Order By CreatedDate;
END


go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_Admin_InsertEmailMessageAudit]') 
AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_Admin_InsertEmailMessageAudit]
go

Create Procedure [dbo].[SP_Admin_InsertEmailMessageAudit]
(
@MessageKey varchar(500),
@Message varchar(max), 
@InsuredCode varchar(250),
@PolicyNo varchar(250), 
@LinkId varchar(250),
@InsuredType varchar(500),
@Subject nvarchar(500)
)
AS
BEGIN
	INSERT INTO [dbo].[BK_EMAILMESSAGEAUDIT](MessageKey,Message,InsuredCode,PolicyNo,LinkId,InsuredType,CreatedDate,
	Subject,TrackId) VALUES(@MessageKey,@Message,@InsuredCode,@PolicyNo,@LinkId,@InsuredType,
			GetDate(),@Subject,newid());
END
