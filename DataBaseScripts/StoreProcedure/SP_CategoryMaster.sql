
create Procedure [dbo].[SP_CategoryMaster]
(
@id int,
@AgenctCode varchar(50),
@MainClass varchar(50),
@SubClass varchar(50),
@Category varchar(50),
@Code varchar(50),
@ValueType varchar(50),
@Value varchar(50),
@EffectiveFrom datetime,
@EffectiveTo datetime,
@Status bit,
@Type varchar(50)
)
as
begin

if(@Type='edit')
begin
update CATEGORY_MASTER set Category=@Category,Code=@Code where id=@id
end
if(@Type='delete')
begin
 update CATEGORY_MASTER set Status=0 where id=@id
 end
if(@Type='insert')
begin
 insert into CATEGORY_MASTER(AgenctCode,MainClass,SubClass,Category,Code,ValueType,Value,EffectiveFrom,EffectiveTo,Status) 
                      values(@AgenctCode,@MainClass,@SubClass,@Category,@Code,@ValueType,@Value,@EffectiveFrom,@EffectiveTo,@Status)
end
end