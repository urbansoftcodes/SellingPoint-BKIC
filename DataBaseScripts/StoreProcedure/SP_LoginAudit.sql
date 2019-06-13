USE SellingPoint
GO
/****** Object:  StoredProcedure [dbo].[LoginAudit]    Script Date: 4/15/2018 12:06:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [dbo].[SP_LoginAudit]
(
 @UserName nvarchar(50),
 @IPAddress nvarchar(50),
 @LoginDate datetime,
 @LoginStatus nvarchar(50)
)
AS
BEGIN
	INSERT INTO [dbo].[BK_Audit](USERNAME,IPADDRESS,LOGINDATE,LOGINSTATUS) values(@UserName,@IPAddress,@LoginDate,@LoginStatus);
END
