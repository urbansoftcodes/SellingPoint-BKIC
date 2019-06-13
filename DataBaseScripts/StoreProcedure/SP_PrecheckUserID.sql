
Create Procedure SP_PrecheckUserID
(
@UserID varchar(50),
@IsUerIDExists bit out
)
as
begin

if exists(select * from aspnet_Users with(nolock) where UserName=@UserID)
begin
set @IsUerIDExists=1
end
else
begin
set @IsUerIDExists=0
end
end