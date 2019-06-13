using BKIC.SellingPoint.DL.BL.Implementation;
using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OracleDBIntegration = SellingPoint.OracleDBIntegration;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class HomeCalculator
    {
        public readonly IAdmin _adminRepository;
        private readonly IHomeInsurance _homeInsuranceRepository;
        private readonly IInsurancePortal _insurancePortalRepository;
        public readonly IMail _mail;
        public readonly OracleDBIntegration.Implementation.HomeInsurance _oracleHomeInsurance;
        public decimal BuildingPremium { get; set; }
        public decimal ContentPremium { get; set; }
        public decimal BuildingRiot { get; set; }
        public decimal ContentRiot { get; set; }
        public decimal JewelleryRiot { get; set; }
        public decimal TotalRiot { get; set; }
        public decimal DomesticHelperAmount { get; set; }
        public decimal JewellerAmount { get; set; }
        public decimal TotalBasicPremium { get; set; }
        public decimal TotalSRCCPremium { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal CommissionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public decimal TaxOnBasicPremium { get; set; }
        public decimal TaxOnSRCCPremium { get; set; }
        public decimal TaxOnPremiumBeforeDiscount { get; set; }
        public decimal TaxOnPremiumAfterDiscount { get; set; }
        public decimal TaxOnCommissionBeforeDiscount { get; set; }
        public decimal TaxOnCommissionAfterDiscount { get; set; }
        //  public decimal Premium { get; set; }
        public decimal Discount { get; set; }
        public DateTime PolicyExpireDate { get; set; }
        public bool IsHIR { get; set; }
        public string HIRReason { get; set; }
        public int HIRStatus { get; set; }
        public decimal DeductableCommission { get; set; }
        public decimal NonDeductableCommission { get; set; }
        public decimal TaxRate { get; set; }
        public decimal ProductMinimumPremium { get; set; }
        public decimal RiotRate { get; set; }
        public decimal BaseBuildingContentRate { get; set; }
        public decimal RenewalDelayedDaysAmount { get; set; }

        public HomeCalculator()
        {
            _adminRepository = new Admin();
            _homeInsuranceRepository = new HomeInsurance();
            _insurancePortalRepository = new InsurancePortal();
            _oracleHomeInsurance = new OracleDBIntegration.Implementation.HomeInsurance();
            _mail = new Mail();
        }
        public BKIC.SellingPoint.DL.BO.HomeInsurancePolicyResponse InsertHome(BO.HomeInsurancePolicyDetails policy)
        {
            try
            {

                var req = new BO.HomeProductRequest
                {
                    Type = "fetch",
                    Agency = policy.HomeInsurancePolicy.Agency,
                    AgentCode = policy.HomeInsurancePolicy.AgentCode,
                    MainClass = policy.HomeInsurancePolicy.MainClass,
                    SubClass = policy.HomeInsurancePolicy.SubClass
                };
                BO.HomeProductResponse productRes = _adminRepository.GetHomeProduct(req);
                if (productRes != null && productRes.IsTransactionDone && productRes.HomeProducts.Count > 0)
                {
                    var homeProduct = productRes.HomeProducts[0];
                    if (homeProduct != null)
                    {
                        Calculate(policy, homeProduct);
                        var policyRecord = InsertHomeMain(policy, policy.HomeInsurancePolicy.IsRenewal ? HomeInsuranceSP.InsertHomeRenewal : HomeInsuranceSP.InsertHome);
                        if (policyRecord != null && policyRecord.IsInserted)
                        {
                            CalculateCommission(homeProduct, policy.HomeInsurancePolicy, policyRecord.NewHomeID,
                                                policyRecord.DocumentNumber, policyRecord.LinkID, policyRecord.RenewalCount);

                            if (policyRecord.NewHomeID > 0 && !IsHIR && policy.HomeInsurancePolicy.IsActivePolicy)
                            {                                
                                try
                                {
                                    new Task(() =>
                                    {
                                        SqlParameter[] para = new SqlParameter[]
                                        {
                                                 new SqlParameter("@HomeID", policyRecord.NewHomeID)
                                        };
                                        SellingPointSQL.eds("MIG_IntegrateHomeDetails", para);
                                    }).Start();
                                }
                                catch (AggregateException ex)
                                {
                                    foreach (Exception inner in ex.InnerExceptions)
                                    {
                                        _mail.SendMailLogError(inner.Message, policy.HomeInsurancePolicy.InsuredCode,
                                            "HomeInsurance", policy.HomeInsurancePolicy.Agency, true);
                                    }
                                }
                            }
                            return new BO.HomeInsurancePolicyResponse()
                            {
                                IsTransactionDone = true,
                                HomeId = policyRecord.NewHomeID,
                                IsHIR = IsHIR,
                                DocumentNo = policyRecord.DocumentNumber,
                                RenewalCount = policyRecord.RenewalCount
                            };
                        }
                    }
                    return new BKIC.SellingPoint.DL.BO.HomeInsurancePolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = "Product not found"
                    };
                }
                return new BKIC.SellingPoint.DL.BO.HomeInsurancePolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = "Product not found"
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CalculateCommission(BO.HomeProduct homeProduct, BO.HomeInsurancePolicy policy,
            long homeID, string documentNo, string LinkID, int renewalCount)
        {
            try
            {
                int lineNo = 0;
                if (homeProduct.Category != null && homeProduct.Category.Count > 0)
                {
                    List<PolicyCategory> policyCategories = new List<PolicyCategory>();
                    foreach (var dr in homeProduct.Category)
                    {
                        lineNo++;
                        if (!policy.UserChangedPremium)
                        {
                            if (dr.ValueType == "Percent" && dr.Code == "BASICCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = TotalBasicPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = TotalBasicPremium * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = TotalBasicPremium;
                                policyCategory.PremiumBeforeDiscount = TotalBasicPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(TotalBasicPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(TotalBasicPremium, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "SRCCCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = TotalSRCCPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = TotalSRCCPremium * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = TotalSRCCPremium;
                                policyCategory.PremiumBeforeDiscount = TotalSRCCPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(TotalSRCCPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(TotalSRCCPremium, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "AGTCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = TotalPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = TotalPremium * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = TotalPremium;
                                policyCategory.PremiumBeforeDiscount = TotalPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(TotalPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(TotalPremium, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;

                                policyCategories.Add(policyCategory);

                            }
                        }
                        else
                        {
                            if (dr.ValueType == "Percent" && dr.Code == "BASICCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = TotalBasicPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = CommissionAfterDiscount;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = TotalBasicPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(TotalBasicPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "SRCCCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = TotalSRCCPremium * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = policy.IsRiotStrikeDamage.ToString().ToLower() == "y" ? CommissionAfterDiscount : 0;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = policy.IsRiotStrikeDamage.ToString().ToLower() == "y" ? PremiumAfterDiscount : 0;
                                policyCategory.PremiumBeforeDiscount = TotalSRCCPremium;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(TotalSRCCPremium, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && dr.Code == "AGTCOMM")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = PremiumBeforeDiscount * dr.Value / 100;
                                policyCategory.CommissionAfterDiscount = PremiumAfterDiscount * dr.Value / 100;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = homeID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = PremiumBeforeDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(PremiumBeforeDiscount, homeProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, homeProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.HomeID = homeID;

                                policyCategories.Add(policyCategory);
                            }
                        }
                    }
                    if (policy.UserChangedPremium)
                    {
                        var commissionDiscount = policy.CommissionAfterDiscount;
                        foreach (var pc in policyCategories)
                        {
                            if (pc.IsDeductable)
                            {
                                if (pc.CommissionBeforeDiscount < commissionDiscount)
                                {
                                    pc.CommissionAfterDiscount = pc.CommissionBeforeDiscount;
                                    pc.TaxOnCommissionAfterDiscount = GetTax(pc.CommissionBeforeDiscount, homeProduct.TaxRate);
                                    commissionDiscount = commissionDiscount - pc.CommissionBeforeDiscount;
                                }
                                else
                                {
                                    pc.CommissionAfterDiscount = commissionDiscount;
                                    pc.TaxOnCommissionAfterDiscount = GetTax(commissionDiscount, homeProduct.TaxRate);
                                    commissionDiscount = 0;
                                }
                            }
                        }
                    }
                    InsertCategory(policy, policyCategories);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertCategory(BO.HomeInsurancePolicy policy, List<PolicyCategory> policyCategories)
        {
            if (policyCategories != null && policyCategories.Count > 0)
            {
                foreach (var dr in policyCategories)
                {
                    SqlParameter[] paras = new SqlParameter[]
                    {
                                new SqlParameter("@DocumentID", dr.DocumentID),
                                new SqlParameter("@InsuredCode", policy.InsuredCode),
                                new SqlParameter("@LinkID", dr.LinkID),
                                new SqlParameter("@DocumentNo",dr.DocumentNo),
                                new SqlParameter("@EndorsementNo", dr.EndorsementNo ?? string.Empty),
                                new SqlParameter("@EndorsementCount", dr.EndorsementCount),
                                new SqlParameter("@AgentCode", dr.AgentCode),
                                new SqlParameter("@LineNo", dr.LineNo),
                                new SqlParameter("@Category", dr.Category),
                                new SqlParameter("@Code", dr.Code),
                                new SqlParameter("@ValueType", dr.ValueType),
                                new SqlParameter("@Value", dr.Value),
                                new SqlParameter("@PremiumBeforeDiscount", dr.PremiumBeforeDiscount),
                                new SqlParameter("@PremiumAfterDiscount", dr.PremiumAfterDiscount),
                                new SqlParameter("@CommissionBeforeDiscount", dr.CommissionBeforeDiscount),
                                new SqlParameter("@CommissionAfterDiscount", dr.CommissionAfterDiscount),
                                new SqlParameter("@TaxOnPremiumBeforeDiscount", dr.TaxOnPremiumBeforeDiscount),
                                new SqlParameter("@TaxOnPremiumAfterDiscount", dr.TaxOnPremiumAfterDiscount),
                                new SqlParameter("@TaxOnCommissionBeforeDiscount", dr.TaxOnCommissionBeforeDiscount),
                                new SqlParameter("@TaxOnCommissionAfterDiscount", dr.TaxOnCommissionAfterDiscount),
                                new SqlParameter("@IsDeductable", dr.IsDeductable),
                                 new SqlParameter("@RenewalCount", dr.RenewalCount),
                                new SqlParameter("@DomesticID", DBNull.Value),
                                new SqlParameter("@TravelID", DBNull.Value),
                                new SqlParameter("@HomeID", dr.HomeID),
                                new SqlParameter("@MotorID", DBNull.Value),
                                new SqlParameter("@MotorEndorsementID", DBNull.Value),
                                new SqlParameter("@TravelEndorsementID", DBNull.Value),
                                new SqlParameter("@HomeEndorsementID", DBNull.Value),

                     };
                    BKICSQL.edt(MotorInsuranceSP.PolicyCategoryInsert, paras);
                }
            }
        }

        private void Calculate(BO.HomeInsurancePolicyDetails policy, BO.HomeProduct homeProduct)
        {
            //Calcuate the base premium.
            BuildingPremium = homeProduct.Rate * policy.HomeInsurancePolicy.BuildingValue / 100;
            ContentPremium = homeProduct.Rate * policy.HomeInsurancePolicy.ContentValue / 100;
            //Premium = BuildingPremium + ContentPremium;
            BaseBuildingContentRate = homeProduct.Rate;

            //Calculate the riot premium.
            if (policy.HomeInsurancePolicy.IsRiotStrikeDamage.ToString().ToLower() == "y")
            {
                BuildingRiot = homeProduct.RiotCoverRate * policy.HomeInsurancePolicy.BuildingValue / 100;
                ContentRiot = homeProduct.RiotCoverRate * policy.HomeInsurancePolicy.ContentValue / 100;
                JewelleryRiot = homeProduct.RiotCoverRate * policy.HomeInsurancePolicy.JewelleryValue / 100;

                TotalRiot = BuildingRiot + ContentRiot;

                if (TotalRiot <= homeProduct.RiotCoverMinAmount)
                {
                    TotalRiot = homeProduct.RiotCoverMinAmount;
                }
                RiotRate = homeProduct.RiotCoverRate;
            }

            //Calculate the home domestic helper amount.
            if (policy.HomeInsurancePolicy.NoOfDomesticWorker > 0 || policy.HomeDomesticHelpdt.Rows.Count > 0)
            {
                var domesticHelpers = policy.HomeDomesticHelpdt.Rows.Count;
                DomesticHelperAmount = domesticHelpers > 0 ? (domesticHelpers - 1) * homeProduct.DomesticHelperAmount : 0;
            }

            //Calculate the jewellery amount.
            var jCover = homeProduct.JewelleryCover.Find(x => x.KeyType == policy.HomeInsurancePolicy.JewelleryCover);
            if (jCover != null)
            {
                JewellerAmount = jCover.Amount;
                if (jCover.KeyType == "EXTREME")
                {
                    JewellerAmount = jCover.Rate * policy.HomeInsurancePolicy.JewelleryValue / 100;
                }
            }

            // TotalPremium = Math.Round(Premium + TotalRiot + DomesticHelperAmount + JewellerAmount, 3, MidpointRounding.AwayFromZero);
            TotalBasicPremium = Math.Round(BuildingPremium + ContentPremium, 3, MidpointRounding.AwayFromZero);
            TotalSRCCPremium = Math.Round(BuildingRiot + ContentRiot + JewelleryRiot, 3, MidpointRounding.AwayFromZero);

            if (TotalBasicPremium <= homeProduct.MinimumPremium)
            {
                TotalBasicPremium = homeProduct.MinimumPremium;
            }

            TotalBasicPremium = TotalBasicPremium + DomesticHelperAmount + JewellerAmount;

            TotalPremium = TotalBasicPremium + TotalSRCCPremium;

            //Deduct the renewal delayed days amount from the premium.
            if (policy.HomeInsurancePolicy.RenewalDelayedDays > 0)
            {
                var renewalDelayedAmount = GetRenewalDelayedDaysAmount(policy, homeProduct, BuildingPremium + ContentPremium + TotalRiot + DomesticHelperAmount + JewellerAmount);
                TotalPremium = Math.Round(BuildingPremium + ContentPremium + TotalRiot + DomesticHelperAmount + JewellerAmount - renewalDelayedAmount, 3, MidpointRounding.AwayFromZero);

                var renewalDelayedAmount1 = GetRenewalDelayedDaysAmount(policy, homeProduct, BuildingPremium + ContentPremium + DomesticHelperAmount + JewellerAmount);
                TotalBasicPremium = Math.Round(BuildingPremium + ContentPremium + DomesticHelperAmount + JewellerAmount - renewalDelayedAmount1, 3, MidpointRounding.AwayFromZero);

                var renewalDelayedAmount2 = GetRenewalDelayedDaysAmount(policy, homeProduct, BuildingRiot + ContentRiot);
                TotalSRCCPremium = Math.Round(BuildingRiot + ContentRiot - renewalDelayedAmount2, 3, MidpointRounding.AwayFromZero);
            }

            PremiumBeforeDiscount = TotalPremium;

            //Calculate the commission.
            CommissionBeforeDiscount = GetCommision(policy.HomeInsurancePolicy, TotalBasicPremium, TotalSRCCPremium);

            //If the user give the discount
            if (!policy.HomeInsurancePolicy.UserChangedPremium)
            {
                PremiumAfterDiscount = PremiumBeforeDiscount;
                CommissionAfterDiscount = CommissionBeforeDiscount;
            }
            else
            {
                PremiumAfterDiscount = policy.HomeInsurancePolicy.PremiumAfterDiscount;
                CommissionAfterDiscount = policy.HomeInsurancePolicy.CommissionAfterDiscount;
            }

            //Calculate the VAT.
            TaxOnBasicPremium = GetTax(TotalBasicPremium, homeProduct.TaxRate);
            TaxOnSRCCPremium = GetTax(TotalSRCCPremium, homeProduct.TaxRate);
            TaxOnPremiumBeforeDiscount = GetTax(PremiumBeforeDiscount, homeProduct.TaxRate);
            TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, homeProduct.TaxRate);
            TaxOnCommissionBeforeDiscount = GetTax(CommissionBeforeDiscount, homeProduct.TaxRate);
            TaxOnCommissionAfterDiscount = GetTax(CommissionAfterDiscount, homeProduct.TaxRate);

            //Apply discount.
            Discount = PremiumBeforeDiscount - PremiumAfterDiscount;

            //Set HIR - building or content value exceeded the maximum value policy will move to HIR status admin need to approve.
            SetHIR(homeProduct, policy.HomeInsurancePolicy);
        }

        private BO.PolicyRecord InsertHomeMain(BO.HomeInsurancePolicyDetails policy, string spName)
        {
            SqlParameter[] paras = new SqlParameter[]
            {
                    new SqlParameter("@HomeID", policy.HomeInsurancePolicy.HomeID),
                    new SqlParameter("@InsuredCode", policy.HomeInsurancePolicy.InsuredCode),
                    new SqlParameter("@InsuredName", policy.HomeInsurancePolicy.InsuredName),
                    new SqlParameter("@CPR", policy.HomeInsurancePolicy.CPR),
                    new SqlParameter("@Agency", policy.HomeInsurancePolicy.Agency ),
                    new SqlParameter("@AgentCode",policy.HomeInsurancePolicy.AgentCode),
                    new SqlParameter("@BranchCode",policy.HomeInsurancePolicy.AgentBranch),
                    new SqlParameter("@MainClass", policy.HomeInsurancePolicy.MainClass),
                    new SqlParameter("@SubClass", policy.HomeInsurancePolicy.SubClass),
                    new SqlParameter("@MobileNumber",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.Mobile)
                                                                      ? policy.HomeInsurancePolicy.Mobile:""),

                    new SqlParameter("@PolicyStartDate",policy.HomeInsurancePolicy.PolicyStartDate!=null?
                                                        policy.HomeInsurancePolicy.PolicyStartDate:(object) DBNull.Value),

                    new SqlParameter("@BuildingValue",policy.HomeInsurancePolicy.BuildingValue),
                    new SqlParameter("@ContentValue",policy.HomeInsurancePolicy.ContentValue),
                    new SqlParameter("@JewelleryValue",policy.HomeInsurancePolicy.JewelleryValue),
                    new SqlParameter("@BuildingAge",policy.HomeInsurancePolicy.BuildingAge),
                    new SqlParameter("@IsPropertyMortgaged",policy.HomeInsurancePolicy.IsPropertyMortgaged),
                    new SqlParameter("@FinancierCode",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.FinancierCode)
                                                                       ? policy.HomeInsurancePolicy.FinancierCode : ""),

                    new SqlParameter("@IsSafePropertyInsured",policy.HomeInsurancePolicy.IsSafePropertyInsured),
                    new SqlParameter("@JewelleryCover",policy.HomeInsurancePolicy.JewelleryCover),
                    new SqlParameter("@IsRiotStrikeDamage",policy.HomeInsurancePolicy.IsRiotStrikeDamage),
                    new SqlParameter("@IsJointOwnership",policy.HomeInsurancePolicy.IsJointOwnership),

                    new SqlParameter("@JointOwnerName",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.JointOwnerName)
                                                                        ? policy.HomeInsurancePolicy.JointOwnerName :""),
                    new SqlParameter("@NamePolicyReasonSeekingReasons",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.NamePolicyReasonSeekingReasons)
                                                                                      ? policy.HomeInsurancePolicy.NamePolicyReasonSeekingReasons : ""),

                    new SqlParameter("@IsPropertyInConnectionTrade",policy.HomeInsurancePolicy.IsPropertyInConnectionTrade),
                    new SqlParameter("@IsPropertyCoveredOtherInsurance",policy.HomeInsurancePolicy.IsPropertyCoveredOtherInsurance),
                    new SqlParameter("@IsPropertyInsuredSustainedAnyLoss",policy.HomeInsurancePolicy.IsPropertyInsuredSustainedAnyLoss),
                    new SqlParameter("@IsPropertyUndergoingConstruction",policy.HomeInsurancePolicy.IsPropertyUndergoingConstruction),
                    new SqlParameter("@IsSingleItemAboveContents",policy.HomeInsurancePolicy.IsSingleItemAboveContents),

                    new SqlParameter("@BuildingNo",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.BuildingNo)?
                                                                    policy.HomeInsurancePolicy.BuildingNo:""),

                    new SqlParameter("@FlatNo",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.FlatNo) ?  policy.HomeInsurancePolicy.FlatNo:""),
                    new SqlParameter("@HouseNo",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.HouseNo) ? policy.HomeInsurancePolicy.HouseNo:""),
                    new SqlParameter("@NoOfFloors", policy.HomeInsurancePolicy.NoOfFloors),
                    new SqlParameter("@Area",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.Area) ? policy.HomeInsurancePolicy.Area:""),
                    new SqlParameter("@BuildingType", policy.HomeInsurancePolicy.BuildingType),
                    new SqlParameter("@RoadNo",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.RoadNo) ? policy.HomeInsurancePolicy.RoadNo:""),
                    new SqlParameter("@BlockNo",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.BlockNo) ? policy.HomeInsurancePolicy.BlockNo:""),
                    new SqlParameter("@ResidanceTypeCode",policy.HomeInsurancePolicy.BuildingType == 1 ? "H" : "F"),
                    new SqlParameter("@FFPNumber",!string.IsNullOrEmpty(policy.HomeInsurancePolicy.FFPNumber) ? policy.HomeInsurancePolicy.FFPNumber:""),
                    new SqlParameter("@IsRequireDomestic", policy.HomeInsurancePolicy.IsRequireDomestic),
                    new SqlParameter("@NumberOfDomesticWorker", policy.HomeInsurancePolicy.NoOfDomesticWorker),
                    new SqlParameter("@CreatedBy", policy.HomeInsurancePolicy.CreatedBy),
                    new SqlParameter("@AuthorizedBy", policy.HomeInsurancePolicy.AuthorizedBy),
                    new SqlParameter("@IsSaved", policy.HomeInsurancePolicy.IsSaved),
                    new SqlParameter("@IsActive", policy.HomeInsurancePolicy.IsActivePolicy),
                    new SqlParameter("@PaymentType",string.IsNullOrEmpty(policy.HomeInsurancePolicy.PaymentType) ? string.Empty : policy.HomeInsurancePolicy.PaymentType),
                    new SqlParameter("@AccountNumber", string.IsNullOrEmpty(policy.HomeInsurancePolicy.AccountNumber)? string.Empty : policy.HomeInsurancePolicy.AccountNumber),
                    new SqlParameter("@Remarks", string.IsNullOrEmpty(policy.HomeInsurancePolicy.Remarks) ? string.Empty : policy.HomeInsurancePolicy.Remarks),
                    new SqlParameter("@HomeSubItemsdt", policy.HomeSubItemsdt),
                    new SqlParameter("@HomeDomesticdt", policy.HomeDomesticHelpdt),
                    new SqlParameter("@PremiumBeforeDiscount", PremiumBeforeDiscount),
                    new SqlParameter("@PremiumAfterDiscount", PremiumAfterDiscount),
                    new SqlParameter("@CommissionBeforeDiscount", CommissionBeforeDiscount),
                    new SqlParameter("@CommissionAfterDiscount", CommissionAfterDiscount),
                    new SqlParameter("@TaxOnPremiumBeforeDiscount", TaxOnPremiumBeforeDiscount),
                    new SqlParameter("@TaxOnPremiumAfterDiscount", TaxOnPremiumAfterDiscount),
                    new SqlParameter("@TaxOnCommissionBeforeDiscount", TaxOnCommissionBeforeDiscount),
                    new SqlParameter("@TaxOnCommissionAfterDiscount", TaxOnCommissionAfterDiscount),
                    new SqlParameter("@BuildingPremium", BuildingPremium),
                    new SqlParameter("@ContentPremium", ContentPremium),
                    new SqlParameter("@TotalPremium", TotalPremium),
                    new SqlParameter("@BuildingRiot", BuildingRiot),
                    new SqlParameter("@ContentRiot", ContentRiot),
                    new SqlParameter("@TotalRiot", TotalRiot),
                    new SqlParameter("@JewelleryAmount", JewellerAmount),
                    new SqlParameter("@DomesticHelperAmount", DomesticHelperAmount),
                    new SqlParameter("@Discount", Discount),
                    new SqlParameter("@RiotRate", RiotRate),
                    new SqlParameter("@Rate", BaseBuildingContentRate),
                    new SqlParameter("@IsHIR", IsHIR),
                    new SqlParameter("@HIRReason", HIRReason ?? string.Empty),
                    new SqlParameter("@HIRStatus", HIRStatus),
                    new SqlParameter("@UserChangedPremium", policy.HomeInsurancePolicy.UserChangedPremium),
                    new SqlParameter("@OldDocumentNumber",  policy.HomeInsurancePolicy.OldDocumentNumber ?? string.Empty),
                    new SqlParameter("@RenewalDocumentNumber", policy.HomeInsurancePolicy.DocumentNo ?? string.Empty),
                    new SqlParameter("@OldRenewalCount", policy.HomeInsurancePolicy.RenewalCount),
                    new SqlParameter("@RenewalDelayedDays", policy.HomeInsurancePolicy.RenewalDelayedDays),
                    new SqlParameter("@ActualRenewalStartDate", policy.HomeInsurancePolicy.ActualRenewalStartDate.HasValue ? policy.HomeInsurancePolicy.ActualRenewalStartDate : (object)DBNull.Value)
            };
            List<SPOut> outParams = new List<SPOut>()
            {
                new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@NewHomeID"},
                new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=100},
                new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@LinkIDNew", Size=100},
                new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@RenewalCount"},
            };
            object[] dataSet = BKICSQL.GetValues(spName, paras, outParams);
            var HomeID = Convert.ToInt64(dataSet[0]);
            var DocNo = Convert.ToString(dataSet[1]);
            var LinkID = Convert.ToString(dataSet[2]);
            var RenewalCount = Convert.ToInt32(dataSet[3]);
            return new BO.PolicyRecord
            {
                IsInserted = true,
                DocumentNumber = DocNo,
                LinkID = LinkID,
                NewHomeID = HomeID,
                RenewalCount = RenewalCount
            };

        }

        public decimal GetCommision(BO.HomeInsurancePolicy policy, decimal totalBasicPremium, decimal totalSRCCPremium)
        {
            decimal CommissionAmount = 0;

            var commisionRequest = new BO.HomeCommissionRequest
            {
                AgentCode = policy.AgentCode,
                Agency = policy.Agency,
                SubClass = policy.SubClass,
                TotalBasicPremium = totalBasicPremium,
                TotalSRCCPremium = totalSRCCPremium
            };

            var commissionResponse = _insurancePortalRepository.GetHomePolicyCommission(commisionRequest);
            if (commissionResponse.IsTransactionDone)
            {
                CommissionAmount = commissionResponse.BasicCommission;
            }
            return CommissionAmount;
        }

        public decimal GetTax(decimal premium, decimal taxRate)
        {
            return premium * taxRate / 100;
        }

        public void SetHIR(BO.HomeProduct product, BO.HomeInsurancePolicy policy)
        {
            bool isAdded = false;
            if (product.MaximumBuildingValue < policy.BuildingValue)
            {
                IsHIR = true;
                HIRReason = "Building value exceeded the limit";
                HIRStatus = 1;
                isAdded = true;
            }
            if (product.MaximumContentValue < policy.ContentValue)
            {
                IsHIR = true;
                HIRReason = HIRReason = isAdded ? HIRReason + ", " + "Content Value exceeded the limit" : "Content Value exceeded the limit";
                HIRStatus = 1;
                isAdded = true;
            }
            if (product.MaximumJewelleryValue < policy.JewelleryValue)
            {
                IsHIR = true;
                HIRReason = HIRReason = isAdded ? HIRReason + ", " + "Jewellery Value exceeded the limit" : "Jewellery Value exceeded the limit";
                HIRStatus = 1;
                isAdded = true;
            }
            if (product.MaximumTotalValue < policy.BuildingValue + policy.ContentValue)
            {
                IsHIR = true;
                HIRReason = HIRReason = isAdded ? ", " + "Total Value exceeded the limit" : "Total Value exceeded the limit";
                HIRStatus = 1;
                isAdded = true;
            }

        }
        public decimal GetRenewalDelayedDaysAmount(BO.HomeInsurancePolicyDetails policy, BO.HomeProduct homeProduct, decimal netPremium)
        {
            decimal delayedDaysAmount = 0;
            if (policy.HomeInsurancePolicy.RenewalDelayedDays > 0)
            {
                delayedDaysAmount = (netPremium * policy.HomeInsurancePolicy.RenewalDelayedDays) / 365;
            }
            return delayedDaysAmount;
        }
    }
}
