using BKIC.SellingPoint.DL.BL.Implementation;
using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using Spire.Doc;
using System;
using System.IO;
using System.Text;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class Schedule : ISchedule
    {
        public readonly IAdmin _admin;

        public Schedule()
        {
            _admin = new Admin();
        }

        /// <summary>
        /// Get domestic schedule.
        /// </summary>
        /// <param name="domestic">domestic policy properties.</param>
        /// <param name="isMailSend">Mail send to the client or not.</param>
        /// <returns>Schedule file path.</returns>
        public string CreateDomesticSchudles(DomesticHelpSavedQuotationResponse domestic, bool isMailSend = true)
        {
            try
            {
                string FileName = domestic.DomesticHelp.DocumentNo + ".pdf";
                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + domestic.DomesticHelp.InsuredCode + "/" + domestic.DomesticHelp.DocumentNo + "/";

                if (!System.IO.Directory.Exists(FileSavePath))
                {
                    System.IO.Directory.CreateDirectory(FileSavePath);
                }                

                string FullPath = FileSavePath + FileName;
                //ExtensionMethods.CopyCompanyLogos(AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/", FileSavePath);

                string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/DomesticHelp.html",
                    FileSavePath + "DomesticHelp.html"));
                string DomesticWorkersDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/DomesticWorkersDiv.txt",
                    FileSavePath + "DomesticWorkersDiv.txt"));

                string memberslist = string.Empty;
                string memebrs = string.Empty;
                int MembersCount = 1;

                foreach (var list in domestic.DomesticHelpMemberList)
                {
                    string sex = list.Sex == 'M' ? "Male" : "Female";
                    memebrs = DomesticWorkersDiv.Replace("{{Name}}", list.Name)
                                                .Replace("{{SEX}}", sex)
                                                .Replace("{{DOB}}", list.DOB.CovertToLocalFormat())
                                                .Replace("{{Nationality}}", list.Nationality)
                                                .Replace("{{CPR}}", list.CPRNumber)
                                                .Replace("{{Passport}}", list.Passport)
                                                .Replace("{{Occupation}}", list.Occupation)
                                                .Replace("{{Address}}", list.AddressType)
                                                .Replace("{{WorkerCount}}", Convert.ToString(MembersCount));
                    MembersCount++;
                    memberslist += memebrs;
                }

                htmlCode = htmlCode.Replace("{{DomesticWorkersDiv}}", memberslist)
                                   .Replace("{{PolicyStartDate}}", domestic.DomesticHelp.PolicyStartDate.CovertToLocalFormat())
                                   .Replace("{{PolicyExpiryDate}}", domestic.DomesticHelp.PolicyExpiryDate.CovertToLocalFormat())
                                   .Replace("{{SumInsured}}", Convert.ToString(domestic.DomesticHelp.SumInsured))
                                   .Replace("{{Premium}}", Convert.ToString(domestic.DomesticHelp.PremiumAfterDiscount))
                                   .Replace("{{CurrentDate}}", DateTime.Now.CovertToLocalFormat())
                                   .Replace("{{InsuredName}}", domestic.DomesticHelp.FullName)
                                   .Replace("{{EmployedUnder}}", domestic.DomesticHelp.DomesticWorkType)
                                   .Replace("{{PhysicalDefect}}", domestic.DomesticHelp.IsPhysicalDefect)
                                   .Replace("{{PolicyNo}}", domestic.DomesticHelp.DocumentNo)
                                   .Replace("{{Vat}}", Convert.ToString(domestic.DomesticHelp.TaxOnPremium))
                                   .Replace("{{Total}}", Convert.ToString(domestic.DomesticHelp.PremiumAfterDiscount + domestic.DomesticHelp.TaxOnPremium));

                if (System.IO.File.Exists(FullPath))
                {
                    System.IO.File.Delete(FullPath);
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(htmlCode);
                MemoryStream stream = new MemoryStream(byteArray);
                HtmlToPdf.Generate(stream, FullPath, "", "");

                return FullPath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Get travel schedule.
        /// </summary>
        /// <param name="travel">Travel policy properties.</param>
        /// <param name="isMailSend">Mail send to the client or not.</param>
        /// <returns>Schedule file path.</returns>
        public string CreateTravelSchudles(TravelSavedQuotationResponse travel, bool isMailSend = true)
        {
            try
            {
                string FileName = travel.TravelInsurancePolicyDetails.DocumentNumber + ".pdf";
                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + travel.TravelInsurancePolicyDetails.InsuredCode + "/" + travel.TravelInsurancePolicyDetails.DocumentNumber + "/";

                if (!System.IO.Directory.Exists(FileSavePath))
                {
                    System.IO.Directory.CreateDirectory(FileSavePath);
                } 

                string FullPath = FileSavePath + FileName;
                string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/Travel2.html",
                    FileSavePath + "Travel2.html"));

                string DomesticWorkersDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/TravelMembers2.html",
                    FileSavePath + "TravelMembers2.html"));

                string memberDetailsDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/TravelMemberDetails.html",
                    FileSavePath + "TravelMemberDetails.html"));

                string memberslist = string.Empty;
                string members = string.Empty;
                string endorsementType = string.Empty;
                string scheduleName = "Travel Schedule";
                int MembersCount = 1;

                TravelMembers owner = new TravelMembers();

                travel.TravelMembers.ForEach(m =>
                {
                    if (m.ItemSerialNo == 1)
                    {
                        owner = m;
                        owner.ItemName = m.ItemName;//travel.TravelInsurancePolicyDetails.InsuredName.ToUpper();
                    }
                });

                foreach (var list in travel.TravelMembers)
                {
                    if (list.ItemSerialNo != 1)
                    {
                        string membersex = list.Sex.Trim() == "M" ? "Male" : "Female";
                        members = memberDetailsDiv.Replace("{{MemberName}}", list.ItemName.ToUpper())
                                .Replace("{{Sex}}", membersex.ToUpper()).Replace("{{DOB}}", list.DateOfBirth.Value.CovertToLocalFormat())
                                .Replace("{{Nationality}}", list.MakeDescription.ToUpper())
                                .Replace("{{CPR}}", list.CPR.ToUpper()).Replace("{{Passport}}", list.Passport.ToUpper())
                                .Replace("{{Occupation}}", list.OccupationCode.ToUpper())
                                .Replace("{{Count}}", Convert.ToString(MembersCount))
                                .Replace("{{Relationship}}", list.Category.ToUpper());
                        MembersCount++;
                        memberslist += members;
                    }
                }

                if (travel.TravelMembers.Count > 1)
                {
                    DomesticWorkersDiv = DomesticWorkersDiv.Replace("{{TravelMembersDetailsDiv}}", memberslist);
                    htmlCode = htmlCode.Replace("{{TravelMembersDiv}}", DomesticWorkersDiv);
                }
                else
                {
                    DomesticWorkersDiv = DomesticWorkersDiv.Replace("{{TravelMembersDetailsDiv}}", "");
                    htmlCode = htmlCode.Replace("{{TravelMembersDiv}}", "");
                }
                if(!string.IsNullOrEmpty(travel.TravelInsurancePolicyDetails.EndorsementType))
                {
                    endorsementType = Utility.GetTravelEndorsementType(travel.TravelInsurancePolicyDetails.EndorsementType, travel.TravelInsurancePolicyDetails);
                }

                travel.TravelInsurancePolicyDetails.CoverageType = travel.TravelInsurancePolicyDetails.CoverageType.Replace("&", " AND ");

                htmlCode = htmlCode
                                   .Replace("{{StartDate}}", travel.TravelInsurancePolicyDetails.InsuranceStartDate.Value.CovertToLocalFormat())
                                   .Replace("{{ExpiryDate}}", travel.TravelInsurancePolicyDetails.ExpiryDate.Value.CovertToLocalFormat())
                                   .Replace("{{SumInsured}}", Convert.ToString(travel.TravelInsurancePolicyDetails.CoverageType).ToUpper() == "SCHENGEN" ? "18900" : Convert.ToString(travel.TravelInsurancePolicyDetails.SumInsured))
                                   .Replace("{{SumInsuredFormat}}", Convert.ToString(travel.TravelInsurancePolicyDetails.CoverageType).ToUpper() == "SCHENGEN" ? "EURO" : "USD")
                                   .Replace("{{Premium}}", Convert.ToString(travel.TravelInsurancePolicyDetails.DiscountAmount))
                                   .Replace("{{PolicyNumber}}", travel.TravelInsurancePolicyDetails.DocumentNumber)
                                   .Replace("{{PolicyHolderName}}", owner.ItemName.ToUpper())
                                   .Replace("{{IsPhysical}}", travel.TravelInsurancePolicyDetails.IsPhysicalDefect.ToUpper())
                                   .Replace("{{PolicyType}}", travel.TravelInsurancePolicyDetails.SubClass == "FAMIL" ? "FAMILY" : "INDIVIDUAL")
                                   .Replace("{{CurrentDate}}", DateTime.Now.CovertToLocalFormatTime())
                                   .Replace("{{Geographical}}", travel.TravelInsurancePolicyDetails.CoverageType.ToUpper())
                                   .Replace("{{PerPerson}}", travel.TravelInsurancePolicyDetails.SubClass == "STP" ? "Per Person" : "")
                                   .Replace("{{TravelSchedule}}", scheduleName)
                                   .Replace("{{EndorsementType}}", endorsementType);

                string sex = owner.Sex.Trim() == "M" ? "Male" : "Female";

                htmlCode = htmlCode.Replace("{{MemberName}}", owner.ItemName.ToUpper())
                                   .Replace("{{Sex}}", sex.ToUpper())
                                   .Replace("{{DOB}}", owner.DateOfBirth.Value.CovertToLocalFormat())
                                   .Replace("{{Nationality}}", owner.MakeDescription.ToUpper())
                                   .Replace("{{CPR}}", owner.CPR.ToUpper())
                                   .Replace("{{Passport}}", owner.Passport.ToUpper())
                                   .Replace("{{Occupation}}", owner.OccupationCode.ToUpper())
                                   .Replace("{{Count}}", Convert.ToString(MembersCount))
                                   .Replace("{{Relationship}}", owner.Category.ToUpper());

                if (System.IO.File.Exists(FullPath))
                {
                    System.IO.File.Delete(FullPath);
                }
                byte[] byteArray = Encoding.UTF8.GetBytes(htmlCode);
                MemoryStream stream = new MemoryStream(byteArray);
                HtmlToPdf.Generate(stream, FullPath, "", "");

                return FullPath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Get home schedule.
        /// </summary>
        /// <param name="home">home policy properties.</param>
        /// <param name="isMailSend">Mail send to the client or not.</param>
        /// <returns>Schedule file path.</returns>
        public string CreateHomeSchudles(HomeSavedQuotationResponse home, bool isMailSend = true)
        {
            try
            {
                string FileName = home.HomeInsurancePolicy.DocumentNo + ".pdf";
                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + home.HomeInsurancePolicy.InsuredCode + "/" + home.HomeInsurancePolicy.DocumentNo + "/";

                if (!System.IO.Directory.Exists(FileSavePath))
                {
                    System.IO.Directory.CreateDirectory(FileSavePath);
                }
               

                string FullPath = FileSavePath + FileName;             

                string htmlCode = File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/Home.html",
                    FileSavePath + "Home.html"));
                string DomesticWorkersDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/HomeDomestic.html",
                    FileSavePath + "HomeDomestic.html"));
                string HomesubItemsDiv = System.IO.File.ReadAllText(ExtensionMethods.CopyFile(AppDomain.CurrentDomain.BaseDirectory + "/Templates/HomeSingleItems.html",
                    FileSavePath + "HomeSingleItems.html"));

                string memberslist = string.Empty;
                string members = string.Empty;
                string homeitems = string.Empty;
                string homesubitems = string.Empty;
                string endorsementtype = string.Empty;
                string scheduleName = "Home Schedule";

                int WorkerCount = 1;

                foreach (var list in home.DomesticHelp)
                {
                    string sex = list.Sex == 'M' ? "Male" : "Female";
                    members = DomesticWorkersDiv.Replace("{{Name}}", list.Name)
                                                .Replace("{{Sex}}", sex)
                                                .Replace("{{DOB}}", list.DOB.CovertToLocalFormat())
                                                .Replace("{{CPR}}", list.CPR)
                                                .Replace("{{SumInsured}}", Convert.ToString(list.SumInsured))
                                                .Replace("{{WorkerCount}}", WorkerCount.ToString())
                                                .Replace("{{Occupation}}", list.Occupation);
                    memberslist += members;
                    WorkerCount++;
                }
                foreach (var list in home.HomeSubItems)
                {
                    homeitems = HomesubItemsDiv.Replace("{{Item_description}}", list.SubItemName)
                                               .Replace("{{Item_Value}}", Convert.ToString(list.SumInsured));
                    homesubitems += homeitems;
                }
                if (!string.IsNullOrEmpty(home.HomeInsurancePolicy.EndorsementType))
                {
                    endorsementtype =  Utility.GetHomeEndorsementType(home.HomeInsurancePolicy.EndorsementType, home.HomeInsurancePolicy);
                }
                scheduleName = home.HomeInsurancePolicy.RenewalCount > 0 ? "Home Renewal Schedule" : "Home Schedule";

                htmlCode = htmlCode.Replace("{{DomesticWorkersDiv}}", memberslist)
                                    .Replace("{{PolicyNo}}", home.HomeInsurancePolicy.DocumentNo)
                                    .Replace("{{SingleItemsDiv}}", homesubitems)
                                    .Replace("{{InsuredName}}", home.HomeInsurancePolicy.InsuredName)                  
                                    .Replace("{{BuildingNo}}", !string.IsNullOrEmpty(home.HomeInsurancePolicy.BuildingNo) ? Convert.ToString("Building No:" + home.HomeInsurancePolicy.BuildingNo) : "")
                                    .Replace("{{BlockNo}}", home.HomeInsurancePolicy.BlockNo)
                                    .Replace("{{RoadNo}}", home.HomeInsurancePolicy.RoadNo)
                                    .Replace("{{Town}}", home.HomeInsurancePolicy.Area)
                                    .Replace("{{BuildingAge}}", Convert.ToString(home.HomeInsurancePolicy.BuildingAge))
                                    .Replace("{{BuildingValue}}", Convert.ToString(home.HomeInsurancePolicy.BuildingValue))
                                    .Replace("{{ContentValue}}", Convert.ToString(home.HomeInsurancePolicy.ContentValue))
                                    .Replace("{{JewelleryCover}}", home.HomeInsurancePolicy.JewelleryCover)
                                    .Replace("{{TotalSumInsured}}", Convert.ToString((home.HomeInsurancePolicy.BuildingValue + home.HomeInsurancePolicy.ContentValue)))
                                    .Replace("{{PolicyStartDate}}", home.HomeInsurancePolicy.PolicyStartDate.CovertToLocalFormat())
                                    .Replace("{{ExpiryDate}}", home.HomeInsurancePolicy.PolicyExpiryDate.CovertToLocalFormat())
                                    .Replace("{{SumInsured}}", Convert.ToString(home.HomeInsurancePolicy.SumInsured))
                                    .Replace("{{Premium}}", Convert.ToString(home.HomeInsurancePolicy.PremiumAfterDiscount))
                                    .Replace("{{Bank}}", Convert.ToString(home.HomeInsurancePolicy.FinancierCode))
                                    .Replace("{{1}}", home.HomeInsurancePolicy.IsSafePropertyInsured == 'Y' ? "Yes" : "No")
                                    .Replace("{{2}}", home.HomeInsurancePolicy.IsRiotStrikeDamage == 'Y' ? "Yes" : "No")
                                    .Replace("{{3}}", home.HomeInsurancePolicy.IsRequireDomestic == 'Y' ? "Yes" : "No")
                                    .Replace("{{4}}", home.HomeInsurancePolicy.IsSingleItemAboveContents == 'Y' ? "Yes" : "No")
                                    .Replace("{{5}}", home.HomeInsurancePolicy.IsJointOwnership == 'Y' ? "Yes" : "No")
                                    .Replace("{{6}}", home.HomeInsurancePolicy.IsPropertyInConnectionTrade == 'Y' ? "Yes" : "No")
                                    .Replace("{{7}}", home.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance == 'Y' ? "Yes" : "No")
                                    .Replace("{{8}}", home.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss == 'Y' ? "Yes" : "No")
                                    .Replace("{{9}}", home.HomeInsurancePolicy.IsPropertyUndergoingConstruction == 'Y' ? "Yes" : "No")
                                    .Replace("{{Vat}}", Convert.ToString(home.HomeInsurancePolicy.TaxOnPremium))
                                    .Replace("{{Total}}", Convert.ToString(home.HomeInsurancePolicy.PremiumAfterDiscount + home.HomeInsurancePolicy.TaxOnPremium))
                                    .Replace("{{HomeSchedule}}", scheduleName)
                                    .Replace("{{EndorsementType}}", endorsementtype);


                if (home.HomeInsurancePolicy.ResidanceTypeCode == "H")
                {
                    htmlCode = htmlCode.Replace("{{FlatNo}}", "House No : " + home.HomeInsurancePolicy.HouseNo);
                }
                else if (home.HomeInsurancePolicy.ResidanceTypeCode == "F")
                {
                    htmlCode = htmlCode.Replace("{{FlatNo}}", "Flat No : " + home.HomeInsurancePolicy.FlatNo);
                }

                if (home.HomeSubItems.Count == 0)
                {
                    string singleitemdiv = @"<TABLE WIDTH=638 CELLPADDING=7 CELLSPACING=0><COL WIDTH=463>
                                            <COL WIDTH=145><TR><TD COLSPAN=2 WIDTH=622 VALIGN=TOP STYLE='border: 1px solid #00000a; padding-top: 0in; padding-bottom: 0in; padding-left: 0.08in; padding-right: 0.08in'>
                                            <P ALIGN = CENTER><FONT FACE = 'serif'><FONT SIZE =3 STYLE ='font-size: 11pt'><B> Single items </B></FONT></FONT></P></TD></TR></TABLE>";

                    htmlCode = htmlCode.Replace(singleitemdiv, "");
                }

                if (System.IO.File.Exists(FullPath))
                {
                    System.IO.File.Delete(FullPath);
                }

                byte[] byteArray = Encoding.UTF8.GetBytes(htmlCode);
                MemoryStream stream = new MemoryStream(byteArray);
                HtmlToPdf.Generate(stream, FullPath, "", "");

                return FullPath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Get motor schedule.
        /// </summary>
        /// <param name="motordetails">Motor policy properties.</param>
        /// <param name="isMailSend">Mail send to the client or not.</param>
        /// <returns>Schedule file path.</returns>
        public string CreateMotorSchudles(MotorInsurancePolicy motordetails, bool isMailSend = true)
        {
            try
            {
                string FileName = motordetails.DocumentNo + ".pdf";
                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + motordetails.InsuredCode + "/" + motordetails.DocumentNo + "/";

                if (!System.IO.Directory.Exists(FileSavePath))
                {
                    System.IO.Directory.CreateDirectory(FileSavePath);
                }               

                string FullPath = FileSavePath + FileName;

                Document document = new Document();

                var applicationPath = AppDomain.CurrentDomain.BaseDirectory + "/Templates/";

                if(motordetails.Agency == "BBK")
                {
                    document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "Motor-Secura.docx", FileSavePath + "Motor-Secura.docx"));
                }
                else
                {
                    document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "Motor-Tisco.docx", FileSavePath + "Motor-Tisco.docx"));
                }                            

                var InsuredName = motordetails.IsUnderBCFC ? "BAHRAIN COMMERCIAL FACILITIE COMPANY" : motordetails.InsuredName;

                var scheduleName = motordetails.RenewalCount > 0 ? "Motor Renewal Schedule" : "Motor Schedule";

                document.Replace("{{Motor Schedule}}", scheduleName, false, true);

                string endorsementType = string.Empty;
                if (!string.IsNullOrEmpty(motordetails.EndorsementType))
                {
                    endorsementType = Utility.GetEndorsementType(motordetails.EndorsementType, motordetails);
                }               
                document.Replace("{{EndorsementType}}", endorsementType, false, true);               
                document.Replace("{{PolicyNo}}", motordetails.DocumentNo, false, true);
                document.Replace("{{InsuredName}}", InsuredName, false, true);
                document.Replace("{{RegistrationNo}}", Convert.ToString(motordetails.RegistrationNumber), false, true);
                document.Replace("{{ChasisNo}}", motordetails.ChassisNo, false, true);
                document.Replace("{{VehicleMake}}", motordetails.VehicleMake, false, true);
                document.Replace("{{VehicleModel}}", motordetails.VehicleModel, false, true);
                document.Replace("{{EngineCapacity}}", Convert.ToString(motordetails.EngineCC), false, true);
                document.Replace("{{YearOfMake}}", Convert.ToString(motordetails.YearOfMake), false, true);
                document.Replace("{{StartDate}}", motordetails.PolicyCommencementDate.CovertToLocalFormat(), false, true);
                document.Replace("{{ExpiryDate}}", motordetails.ExpiryDate.CovertToLocalFormat(), false, true);
                document.Replace("{{Product}}", string.IsNullOrEmpty(motordetails.Subclass) ? "" : motordetails.Subclass, false, true);
                document.Replace("{{VehicleValue}}", Convert.ToString(motordetails.VehicleValue), false, true);
                document.Replace("{{Premium}}", Convert.ToString(motordetails.PremiumAfterDiscount), false, true);
                document.Replace("{{ExcessValue}}", Convert.ToString(motordetails.ExcessAmount), false, true);
                document.Replace("{{ExcessType}}", motordetails.ExcessType, false, true);
                document.Replace("{{CurrentDateTime}}", Convert.ToString(DateTime.Now), false, true);
                document.Replace("{{Vat}}", Convert.ToString(motordetails.TaxOnPremium), false, true);
                document.Replace("{{Total}}", Convert.ToString(motordetails.PremiumAfterDiscount + motordetails.TaxOnPremium ), false, true);

                if (string.IsNullOrEmpty(motordetails.FinancierCompanyCode))
                {
                    document.Replace("{{FinancierType}}", "Privately owned", false, true);
                }
                else
                {
                    document.Replace("{{FinancierType}}", motordetails.FinancierCompanyCode, false, true);
                }
                string coverCodes = string.Empty;

                if (motordetails.Covers != null)
                {
                    for (int i = 0; i < motordetails.Covers.Count; i++)
                    {                        
                        coverCodes += motordetails.Covers[i].CoverCode.ToLower() + ", ";
                    }
                    //Optional covers.
                    if (motordetails.OptionalCovers != null && motordetails.OptionalCovers.Count > 0)
                    {
                        for (int i = 0; i < motordetails.OptionalCovers.Count; i++)
                        {
                            coverCodes += motordetails.OptionalCovers[i].CoverCode.ToLower() + ", ";
                        }
                    }
                    coverCodes = coverCodes.TrimStart(' ').TrimEnd(' ').TrimStart(',').Trim(',');
                    document.Replace("{{Covers}}", coverCodes, false, true);
                }
                else
                {
                    document.Replace("{{Covers}}", "", false, true);
                }
                if (System.IO.File.Exists(FullPath))
                {
                    System.IO.File.Delete(FullPath);
                }               
                document.SaveToFile(FileSavePath + FileName, Spire.Doc.FileFormat.PDF);
                return FullPath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Get motor proposal.
        /// </summary>
        /// <param name="motordetails">motor policy properties.</param>
        /// <param name="isMailSend">Mail send to the client or not.</param>
        /// <returns>Schedule file path.</returns>
        public string CreateMotorProposal(MotorInsurancePolicy motordetails, bool isMailSend = true)
        {
            try
            {
                string FileName = motordetails.DocumentNo + ".pdf";
                string FileSavePath = AppDomain.CurrentDomain.BaseDirectory + "/ScheduleDocuments/" + motordetails.InsuredCode + "/" + motordetails.DocumentNo + "/";

                if (!System.IO.Directory.Exists(FileSavePath))
                {
                    System.IO.Directory.CreateDirectory(FileSavePath);
                }              
                
                string FullPath = FileSavePath + FileName;
                Document document = new Document();
                var applicationPath = AppDomain.CurrentDomain.BaseDirectory + "/Templates/";

                if(motordetails.Agency == "BBK")
                {
                    document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "MotorProposal-Secura.docx", FileSavePath + "MotorProposal-Secura.docx"));
                }
                else
                {
                    document.LoadFromFile(ExtensionMethods.CopyFile(applicationPath + "MotorProposal-Tisco.docx", FileSavePath + "MotorProposal-Tisco.docx"));
                }           
                

                var InsuredName = motordetails.IsUnderBCFC ? "BAHRAIN COMMERCIAL FACILITIE COMPANY" : motordetails.InsuredName;

                var proposalName = motordetails.RenewalCount > 0 ? "Motor Renewal Proposal" : "Motor Proposal";

                document.Replace("{{Motor Proposal}}", proposalName, false, true);
                document.Replace("{{PolicyNo}}", motordetails.DocumentNo, false, true);
                document.Replace("{{InsuredName}}", InsuredName, false, true);
                document.Replace("{{RegistrationNo}}", Convert.ToString(motordetails.RegistrationNumber), false, true);
                document.Replace("{{ChasisNo}}", motordetails.ChassisNo, false, true);
                document.Replace("{{VehicleMake}}", motordetails.VehicleMake, false, true);
                document.Replace("{{VehicleModel}}", motordetails.VehicleModel, false, true);
                document.Replace("{{EngineCapacity}}", Convert.ToString(motordetails.EngineCC), false, true);
                document.Replace("{{YearOfMake}}", Convert.ToString(motordetails.YearOfMake), false, true);
                document.Replace("{{StartDate}}", motordetails.PolicyCommencementDate.CovertToLocalFormat(), false, true);
                document.Replace("{{ExpiryDate}}", motordetails.ExpiryDate.CovertToLocalFormat(), false, true);
                document.Replace("{{Product}}", string.IsNullOrEmpty(motordetails.Subclass) ? "" : motordetails.Subclass, false, true);
                document.Replace("{{VehicleValue}}", Convert.ToString(motordetails.VehicleValue), false, true);
                document.Replace("{{Premium}}", Convert.ToString(motordetails.PremiumAfterDiscount), false, true);
                document.Replace("{{ExcessValue}}", Convert.ToString(motordetails.ExcessAmount), false, true);
                document.Replace("{{ExcessType}}", motordetails.ExcessType, false, true);
                document.Replace("{{CurrentDateTime}}", Convert.ToString(DateTime.Now), false, true);
                document.Replace("{{Vat}}", Convert.ToString(motordetails.TaxOnPremium), false, true);
                document.Replace("{{Total}}", Convert.ToString(motordetails.PremiumAfterDiscount + motordetails.TaxOnPremium), false, true);

                if (string.IsNullOrEmpty(motordetails.FinancierCompanyCode))
                {
                    document.Replace("{{FinancierType}}", "Privately owned", false, true);
                }
                else
                {
                    document.Replace("{{FinancierType}}", motordetails.FinancierCompanyCode, false, true);
                }

                string coverCodes = string.Empty;
                if (motordetails.Covers != null)
                {
                    for (int i = 0; i < motordetails.Covers.Count; i++)
                    {                        
                        coverCodes += motordetails.Covers[i].CoverCode.ToLower() + ", ";
                    }
                    //Optional covers.
                    if (motordetails.OptionalCovers != null && motordetails.OptionalCovers.Count > 0)
                    {
                        for (int i = 0; i < motordetails.OptionalCovers.Count; i++)
                        {
                            coverCodes += motordetails.OptionalCovers[i].CoverCode.ToLower() + ", ";
                        }
                    }
                    coverCodes = coverCodes.TrimStart(' ').TrimEnd(' ').TrimStart(',').Trim(',');
                    document.Replace("{{Covers}}", coverCodes, false, true);
                }
                else
                {
                    document.Replace("{{Covers}}", "", false, true);
                }                               
                if (System.IO.File.Exists(FullPath))
                {
                    System.IO.File.Delete(FullPath);
                }
                document.SaveToFile(FileSavePath + FileName, Spire.Doc.FileFormat.PDF);
                return FullPath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Get motor proposal file.
        /// </summary>
        /// <param name="request">Schedule request.</param>
        /// <returns>Schedule file path.</returns>
        public DownloadScheduleResponse GetProposalFilePath(DownloadScheuleRequest request)
        {
            try
            {
                string FilePath = string.Empty;
                if (request.InsuranceType == Constants.Insurance.Motor)
                {
                    MotorInsurance motor = new MotorInsurance();
                    MotorSavedQuotationResponse motorresult = motor.GetSavedMotorPolicy(request.DocNo, "", request.AgentCode, false, 0, request.RenewalCount);
                    FilePath = this.CreateMotorProposal(motorresult.MotorPolicyDetails, false);
                }
                return new DownloadScheduleResponse { FilePath = FilePath, IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new DownloadScheduleResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        /// <summary>
        /// Get the schedule for specific insurance type (Motor or Home or Travel or Domestic)
        /// </summary>
        /// <param name="request">Schedule request.</param>
        /// <returns>Schedule file path.</returns>
        public DownloadScheduleResponse GetScheduleFilePath(DownloadScheuleRequest request)
        {
            try
            {
                string FilePath = string.Empty;             

                if (request.InsuranceType == Constants.Insurance.Travel)
                {
                    TravelInsurance travel = new TravelInsurance();
                    TravelSavedQuotationResponse travelresult = travel.GetSavedQuotationByPolicy(request.DocNo, "portal", request.AgentCode,
                                                                request.IsEndorsement, request.EndorsementID);
                    FilePath = this.CreateTravelSchudles(travelresult, false);
                }
                else if (request.InsuranceType == Constants.Insurance.Motor)
                {
                    MotorInsurance motor = new MotorInsurance();
                    MotorSavedQuotationResponse motorresult = motor.GetSavedMotorPolicy(request.DocNo, "", request.AgentCode,
                                                              request.IsEndorsement, request.EndorsementID, request.RenewalCount);
                    FilePath = this.CreateMotorSchudles(motorresult.MotorPolicyDetails, false);
                }
                else if (request.InsuranceType == Constants.Insurance.DomesticHelp)
                {
                    DomesticHelp domestic = new DomesticHelp();
                    DomesticHelpSavedQuotationResponse domesticresult = domestic.GetSavedDomesticPolicy(request.DocNo, request.AgentCode, 
                                                                        request.IsEndorsement, request.EndorsementID);
                    FilePath = this.CreateDomesticSchudles(domesticresult, false);
                }
                else
                {
                    HomeInsurance home = new HomeInsurance();
                    HomeSavedQuotationResponse homeresult = home.GetSavedQuotationPolicy(request.DocNo, "", request.AgentCode, 
                                                             request.IsEndorsement, request.EndorsementID, request.RenewalCount);
                    FilePath = CreateHomeSchudles(homeresult, false);
                }
                return new DownloadScheduleResponse
                {
                    FilePath = FilePath,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new DownloadScheduleResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }        
    }
}