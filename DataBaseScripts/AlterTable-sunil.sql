
/*Travel Insurance Package*/ 
  alter table travelInsurancePackage  add Agency varchar(50)
   alter table travelInsurancePackage  add AgencyCode varchar(50)


   /* Travel*/
   
   
   
alter table CATEGORY_MASTER alter column EffectiveFrom date
alter table CATEGORY_MASTER alter column EffectiveTo date



CREATE INDEX NCI_LINKID ON PolicyCategory (LINKID);  
 CREATE INDEX NCI_DOCUMENTNO ON PolicyCategory (DOCUMENTNO);  

  CREATE INDEX NCI_PARENTLINKID ON PolicyCategory (PARENTLINKID);  


