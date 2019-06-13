

create table CATEGORY_MASTER
(
id int not null primary key identity(1,1),
AgenctCode varchar(50),
MainClass varchar(50),
SubClass varchar(50),
Category varchar(50),
Code varchar(50),
ValueType varchar(50),
Value varchar(50),
EffectiveFrom datetime,
EffectiveTo datetime,
Status bit
)

select * from CATEGORY_MASTER


