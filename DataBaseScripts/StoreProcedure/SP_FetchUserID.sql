Create Procedure SP_FetchUserID
(
@UserID varchar(50)
)
as
begin

select * from USER_MASTER with(nolock) where userid=@UserID
end




