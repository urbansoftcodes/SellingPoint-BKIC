using BKIC.SellingPoint.DL.BL.Implementation;
using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OracleDBIntegration = SellingPoint.OracleDBIntegration;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class MotorCalculator
    {
        public readonly IAdmin _adminRepository;
        private readonly IMotorInsurance _motorInsuranceRepository;
        private readonly IInsurancePortal _insurancePortalRepository;
        public readonly IMail _mail;
        public readonly OracleDBIntegration.Implementation.MotorInsurance _oracleMotorInsurance;
        public decimal ProductRate { get; set; }
        public decimal PremiumBeforeDiscount { get; set; }
        public decimal PremiumAfterDiscount { get; set; }
        public decimal CommissionBeforeDiscount { get; set; }
        public decimal CommissionAfterDiscount { get; set; }
        public decimal TaxOnPremiumBeforeDiscount { get; set; }
        public decimal TaxOnPremiumAfterDiscount { get; set; }
        public decimal TaxOnCommissionBeforeDiscount { get; set; }
        public decimal TaxOnCommissionAfterDiscount { get; set; }
        public decimal BasePremium { get; set; }
        public decimal ExcessAmount { get; set; }
        public decimal ExcessDiscountAmount { get; set; }
        public decimal ExcessAdditionalAmount { get; set; }
        public decimal AgeLoadingAmount { get; set; }
        public decimal ClaimLoadPercent { get; set; }
        public decimal ClaimLoadingAmount { get; set; }
        public int Age { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal Discount { get; set; }
        public decimal AdditionalDaysAmount { get; set; }
        public decimal RenewalDelayedDaysAmount { get; set; }
        public DateTime PolicyExpireDate { get; set; }
        public bool IsHIR { get; set; }
        public string HIRReason { get; set; }
        public int HIRStatus { get; set; }
        public decimal DeductableCommission { get; set; }
        public decimal NonDeductableCommission { get; set; }
        public decimal TaxRate { get; set; }
        public decimal ExcessDiscountPercent { get; set; }
        public decimal ProductMinimumPremium { get; set; }

        public MotorCalculator()
        {
            _adminRepository = new Admin();
            _motorInsuranceRepository = new MotorInsurance();
            _insurancePortalRepository = new InsurancePortal();
            _oracleMotorInsurance = new OracleDBIntegration.Implementation.MotorInsurance();
            _mail = new Mail();
        }

        public BKIC.SellingPoint.DL.BO.MotorInsurancePolicyResponse InsertMotor(BO.MotorInsurancePolicy policy)
        {
            try
            {

                var req = new BO.MotorProductRequest
                {
                    Type = "fetch",
                    Agency = policy.Agency,
                    AgentCode = policy.AgencyCode,
                    MainClass = policy.Mainclass,
                    SubClass = policy.Subclass,
                };
                BO.MotorProductMasterResponse productRes = _adminRepository.GetMotorProduct(req);
                if (productRes != null && productRes.IsTransactionDone && productRes.motorProductMaster.Count > 0)
                {
                    BO.MotorProductMaster product = productRes.motorProductMaster[0];
                    if (product != null)
                    {
                        Calculate(policy, product);
                        var policyRecord = InsertMotorMain(policy, policy.IsRenewal ? MotorInsuranceSP.MotorRenewalInsert : MotorInsuranceSP.MotorInsert);
                        if (policyRecord != null && policyRecord.IsInserted)
                        {
                            CalculateCommission(product, policy, policyRecord.NewMotorID, policyRecord.DocumentNumber,
                                                policyRecord.LinkID, policyRecord.RenewalCount);

                            if (!IsHIR && policy.IsActivePolicy)
                            {
                                try
                                {
                                    new Task(() =>
                                    {
                                       SqlParameter[] para = new SqlParameter[]
                                       {
                                             new SqlParameter("@MotorID", policyRecord.NewMotorID)
                                       };
                                       SellingPointSQL.eds("MIG_IntegrateMotorDetails", para);
                                    }).Start();
                                }
                                catch (AggregateException ex)
                                {
                                    foreach (Exception inner in ex.InnerExceptions)
                                    {
                                        _mail.SendMailLogError(inner.Message, policy.InsuredCode, "MotorInsurance", policy.Agency, true);
                                    }
                                }
                            }
                            return new BKIC.SellingPoint.DL.BO.MotorInsurancePolicyResponse()
                            {
                                IsTransactionDone = true,
                                IsHIR = IsHIR,
                                MotorID = policyRecord.NewMotorID,
                                DocumentNo = policyRecord.DocumentNumber,
                                RenewalCount = policyRecord.RenewalCount
                            };
                        }
                    }
                    return new BKIC.SellingPoint.DL.BO.MotorInsurancePolicyResponse()
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = "Product not found"
                    };
                }
                return new BKIC.SellingPoint.DL.BO.MotorInsurancePolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = "Product not found"
                };
            }
            catch (Exception ex)
            {
                _mail.SendMailLogError(ex.Message, policy.InsuredCode, "MotorInsurance", policy.Agency, false);
                return new MotorInsurancePolicyResponse()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.ToString()
                };
            }
        }

        private PolicyRecord InsertMotorMain(MotorInsurancePolicy policy, string spName)
        {
            DataTable optionalCovers = new DataTable();
            optionalCovers.Columns.Add("CoverCode", typeof(string));
            optionalCovers.Columns.Add("CoverDescription", typeof(string));
            optionalCovers.Columns.Add("CoverAmount", typeof(decimal));
            optionalCovers.Columns.Add("IsOptionalCover", typeof(bool));

            if (policy.OptionalCovers != null && policy.OptionalCovers.Count > 0)
            {
                foreach (var cover in policy.OptionalCovers)
                {
                    optionalCovers.Rows.Add(cover.CoverCode, cover.CoverDescription,
                        cover.CoverAmount, 1);
                }
            }
            SqlParameter[] paras = new SqlParameter[]
            {
                    new SqlParameter("@MotorID", policy.MotorID),
                    new SqlParameter("@InsuredCode", policy.InsuredCode),
                    new SqlParameter("@DOB", policy.DOB),
                    new SqlParameter("@YearOfMake", policy.YearOfMake),
                    new SqlParameter("@VehicleMake", policy.VehicleMake),
                    new SqlParameter("@VehicleModel", policy.VehicleModel),
                    new SqlParameter("@vehicleTypeCode", policy.vehicleTypeCode),
                    new SqlParameter("@vehicleBodyType", policy.vehicleBodyType),
                    new SqlParameter("@VehicleSumInsured", policy.VehicleValue),
                    new SqlParameter("@BasePremium", policy.PremiumAmount),
                    new SqlParameter("@PolicyCommenceDate", policy.PolicyCommencementDate),
                    new SqlParameter("@PolicyEndDate", policy.PolicyEndDate),
                    new SqlParameter("@RegistrationNumber", policy.RegistrationNumber ?? ""),
                    new SqlParameter("@ChassisNo", policy.ChassisNo),
                    new SqlParameter("@EngineCC", policy.EngineCC),
                    new SqlParameter("@FinancierCompanyCode", !string.IsNullOrEmpty(policy.FinancierCompanyCode) ? policy.FinancierCompanyCode : ""),
                    new SqlParameter("@ExcessType", !string.IsNullOrEmpty(policy.ExcessType) ? policy.ExcessType : ""),
                    new SqlParameter("@dt", optionalCovers),
                    new SqlParameter("OptionalCoverAmount", policy.OptionalCoverAmount),
                    new SqlParameter("@IsUnderBCFC", policy.IsUnderBCFC),
                    new SqlParameter("@SeatingCapacity", policy.SeatingCapacity),
                    new SqlParameter("@Createdby", policy.CreatedBy),
                    new SqlParameter("@AuthorizedBy", policy.AuthorizedBy),
                    new SqlParameter("@IsSaved", policy.IsSaved),
                    new SqlParameter("@IsActive", policy.IsActivePolicy),
                    new SqlParameter("@PaymentAuthorization", policy.PaymentAuthorizationCode ?? ""),
                    new SqlParameter("@TransactionNo", policy.TransactionNo ?? ""),
                    new SqlParameter("@PaymentType", policy.PaymentType ?? ""),
                    new SqlParameter("@AccountNumber", policy.AccountNumber ?? ""),
                    new SqlParameter("@Remarks", policy.Remarks ?? ""),
                    new SqlParameter("@MainClass", policy.Mainclass ?? ""),
                    new SqlParameter("@SubClass", policy.Subclass ?? ""),
                    new SqlParameter("@Agency", policy.Agency),
                    new SqlParameter("@AgentCode", policy.AgencyCode),
                    new SqlParameter("@AgentBranch", policy.AgentBranch),
                    new SqlParameter("ExcessAmount", ExcessAmount),
                    new SqlParameter("@PremiumBeforeDiscount", PremiumBeforeDiscount),
                    new SqlParameter("@PremiumAfterDiscount",  PremiumAfterDiscount),
                    new SqlParameter("@CommissionBeforeDiscount", CommissionBeforeDiscount),
                    new SqlParameter("@CommissionAfterDiscount", CommissionAfterDiscount),
                    new SqlParameter("@TaxOnPremiumBeforeDiscount", TaxOnPremiumBeforeDiscount),
                    new SqlParameter("@TaxOnPremiumAfterDiscount", TaxOnPremiumAfterDiscount),
                    new SqlParameter("@TaxOnCommissionBeforeDiscount", TaxOnCommissionBeforeDiscount),
                    new SqlParameter("@TaxOnCommissionAfterDiscount", TaxOnCommissionAfterDiscount),
                    new SqlParameter("@Discount", Discount),
                    new SqlParameter("@IsHIR", IsHIR),
                    new SqlParameter("@HIRReason", HIRReason ?? string.Empty),
                    new SqlParameter("@HIRStatus", HIRStatus),
                    new SqlParameter("@UserChangedPremium", policy.UserChangedPremium),
                    new SqlParameter("@AgeLoadingAmount", AgeLoadingAmount),
                    new SqlParameter("@ExcessDiscountPercent", ExcessDiscountPercent),
                    new SqlParameter("@ExcessAdditionalAmount", ExcessAdditionalAmount),
                    new SqlParameter("@ClaimLoadingPercent", ClaimLoadPercent),
                    new SqlParameter("@ClaimLoadingAmount", ClaimLoadingAmount),
                    new SqlParameter("@ClaimAmount", policy.ClaimAmount),
                    new SqlParameter("@OtherLoadingAmount", policy.LoadAmount),
                    new SqlParameter("@OldDocumentNumber",  policy.OldDocumentNumber ?? string.Empty),
                    new SqlParameter("@RenewalDocumentNumber", policy.DocumentNo ?? string.Empty),
                    new SqlParameter("@OldRenewalCount", policy.RenewalCount),
                    new SqlParameter("@RenewalDelayedDays", policy.RenewalDelayedDays),
                    new SqlParameter("@ActualRenewalStartDate", policy.ActualRenewalStartDate.HasValue ? policy.ActualRenewalStartDate : (object)DBNull.Value)
            };
            List<SPOut> outParams = new List<SPOut>()
            {
               new SPOut() { OutPutType = SqlDbType.BigInt, ParameterName= "@NewMotorID"},
               new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@DocumentNumber", Size=100},
               new SPOut() { OutPutType = SqlDbType.NVarChar, ParameterName= "@LinkIDNew", Size=100 },
               new SPOut() { OutPutType = SqlDbType.Int, ParameterName= "@RenewalCount"},
            };
            object[] dataSet = BKICSQL.GetValues(spName, paras, outParams);
            var MotorID = Convert.ToInt64(dataSet[0] != null ? dataSet[0] : 0);
            var DocNo = Convert.ToString(dataSet[1]);
            var LinkID = Convert.ToString(dataSet[2]);
            var RenewalCount = Convert.ToInt32(dataSet[3]);
            return new PolicyRecord
            {
                IsInserted = true,
                DocumentNumber = DocNo,
                LinkID = LinkID,
                NewMotorID = MotorID,
                RenewalCount = RenewalCount
            };
        }


        private void Calculate(MotorInsurancePolicy policy, MotorProductMaster product)
        {
            Age = Utility.CalculateAgeCorrect(policy.DOB, DateTime.Now);

            ProductMinimumPremium = product.MinimumPremium;
            PolicyExpireDate = policy.PolicyCommencementDate.AddYears(1).AddDays(-1);
            ExcessDiscountPercent = product.MotorOptionalBenefits.Find(c => c.Value == policy.ExcessType).Percentage;
            ExcessAmount = GetExcess(policy, product, Age);
            BasePremium = product.Rate * (policy.VehicleValue / 100);

            if (Age < product.UnderAge && policy.Agency == "TISCO")
            {
                BasePremium = product.UnderAgeRate * (policy.VehicleValue / 100);
                if (BasePremium < product.UnderAgeminPremium)
                {
                    BasePremium = product.UnderAgeminPremium;
                }
            }

            ExcessDiscountAmount = GetExcessDiscount(policy.ExcessType, BasePremium);
            ExcessAdditionalAmount = GetExcessAdditional(policy.ExcessType, BasePremium);
            AgeLoadingAmount = GetAgeLoading(product.AgeLoadingPercent, BasePremium, product);

            TotalPremium = BasePremium - ExcessDiscountAmount + AgeLoadingAmount + ExcessAdditionalAmount + policy.OptionalCoverAmount + policy.LoadAmount;

            if (TotalPremium < ProductMinimumPremium)
            {
                TotalPremium = ProductMinimumPremium + AgeLoadingAmount + policy.OptionalCoverAmount + policy.LoadAmount;
            }
            AdditionalDaysAmount = GetAdditionalDays(policy, product, TotalPremium);
            RenewalDelayedDaysAmount = GetRenewalDelayedDaysAmount(policy, product, TotalPremium);
            TotalPremium = Math.Round(TotalPremium + AdditionalDaysAmount - RenewalDelayedDaysAmount, 3, MidpointRounding.AwayFromZero);
            ClaimLoadingAmount = GetClaimLoading(policy.ClaimAmount, TotalPremium, product);
            TotalPremium = Math.Round(TotalPremium + ClaimLoadingAmount, 3, MidpointRounding.AwayFromZero);
            PremiumBeforeDiscount = TotalPremium;
            DeductableCommission = GetCommision(policy, TotalPremium, true);
            NonDeductableCommission = GetCommision(policy, TotalPremium, false);
            CommissionBeforeDiscount = DeductableCommission + NonDeductableCommission;
            if (!policy.UserChangedPremium)
            {
                PremiumAfterDiscount = PremiumBeforeDiscount;
                CommissionAfterDiscount = CommissionBeforeDiscount;
            }
            else
            {
                PremiumAfterDiscount = policy.PremiumAfterDiscount;
                if (policy.Agency == "TISCO")
                {
                    NonDeductableCommission = GetCommision(policy, PremiumAfterDiscount, false);
                }
                CommissionAfterDiscount = policy.CommissionAfterDiscount + NonDeductableCommission;
            }
            TaxOnPremiumBeforeDiscount = GetTax(PremiumBeforeDiscount, product.TaxRate);
            TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, product.TaxRate);
            TaxOnCommissionBeforeDiscount = GetTax(CommissionBeforeDiscount, product.TaxRate);
            TaxOnCommissionAfterDiscount = GetTax(CommissionAfterDiscount, product.TaxRate);
            Discount = PremiumBeforeDiscount - PremiumAfterDiscount;
            SetHIR(product, policy);
        }

        public void CalculateCommission(BO.MotorProduct motorProduct, BO.MotorInsurancePolicy policy,
            long motorID, string documentNo, string LinkID, int renewalCount)
        {
            try
            {
                int lineNo = 0;
                if (motorProduct.Category != null && motorProduct.Category.Count > 0)
                {
                    List<PolicyCategory> policyCategories = new List<PolicyCategory>();
                    foreach (var dr in motorProduct.Category)
                    {
                        lineNo++;
                        if (!policy.UserChangedPremium)
                        {
                            if (dr.ValueType == "Percent")
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = Math.Round(dr.IsDeductable ?
                                                                          PremiumBeforeDiscount * dr.Value / 100
                                                                          : PremiumAfterDiscount * dr.Value / 100, 3, MidpointRounding.AwayFromZero);

                                policyCategory.CommissionAfterDiscount = Math.Round(PremiumAfterDiscount * dr.Value / 100, 3, MidpointRounding.AwayFromZero);
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = motorID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = PremiumBeforeDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(PremiumBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, motorProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.MotorID = motorID;

                                policyCategories.Add(policyCategory);
                            }
                        }
                        else
                        {
                            if (dr.ValueType == "Percent" && dr.IsDeductable)
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = Math.Round(PremiumBeforeDiscount * dr.Value / 100, 3, MidpointRounding.AwayFromZero);
                                policyCategory.CommissionAfterDiscount = CommissionAfterDiscount - NonDeductableCommission;
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = motorID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = PremiumBeforeDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(PremiumBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, motorProduct.TaxRate);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.MotorID = motorID;

                                policyCategories.Add(policyCategory);
                            }
                            if (dr.ValueType == "Percent" && !dr.IsDeductable)
                            {
                                var policyCategory = new PolicyCategory();
                                policyCategory.Agency = dr.Agency;
                                policyCategory.AgentCode = dr.AgentCode;
                                policyCategory.Code = dr.Code;
                                policyCategory.Category = dr.Category;
                                policyCategory.CommissionBeforeDiscount = Math.Round(PremiumAfterDiscount * dr.Value / 100, 3, MidpointRounding.AwayFromZero);
                                policyCategory.CommissionAfterDiscount = Math.Round(PremiumAfterDiscount * dr.Value / 100, 3, MidpointRounding.AwayFromZero);
                                policyCategory.DocumentNo = documentNo;
                                policyCategory.EndorsementCount = 0;
                                policyCategory.EndorsementNo = string.Empty;
                                policyCategory.DocumentID = motorID;
                                policyCategory.LineNo = lineNo.ToString();
                                policyCategory.LinkID = LinkID;
                                policyCategory.PremiumAfterDiscount = PremiumAfterDiscount;
                                policyCategory.PremiumBeforeDiscount = PremiumAfterDiscount;
                                policyCategory.TaxOnCommissionBeforeDiscount = GetTax(policyCategory.CommissionBeforeDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnCommissionAfterDiscount = GetTax(policyCategory.CommissionAfterDiscount, motorProduct.TaxRate);
                                policyCategory.TaxOnPremiumBeforeDiscount = GetTax(PremiumAfterDiscount, 5);
                                policyCategory.TaxOnPremiumAfterDiscount = GetTax(PremiumAfterDiscount, 5);
                                policyCategory.Value = dr.Value;
                                policyCategory.ValueType = dr.ValueType;
                                policyCategory.IsDeductable = dr.IsDeductable;
                                policyCategory.RenewalCount = renewalCount;
                                policyCategory.MotorID = motorID;

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
                                    pc.TaxOnCommissionAfterDiscount = GetTax(pc.CommissionBeforeDiscount, motorProduct.TaxRate);
                                    commissionDiscount = commissionDiscount - pc.CommissionBeforeDiscount;
                                }
                                else
                                {
                                    pc.CommissionAfterDiscount = commissionDiscount;
                                    pc.TaxOnCommissionAfterDiscount = GetTax(commissionDiscount, motorProduct.TaxRate);
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

        private void InsertCategory(MotorInsurancePolicy policy, List<PolicyCategory> policyCategories)
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
                                new SqlParameter("@HomeID", DBNull.Value),
                                new SqlParameter("@MotorID", dr.MotorID),
                                new SqlParameter("@MotorEndorsementID",  DBNull.Value),
                                new SqlParameter("@TravelEndorsementID", DBNull.Value),
                                new SqlParameter("@HomeEndorsementID", DBNull.Value),
                     };
                    BKICSQL.edt(MotorInsuranceSP.PolicyCategoryInsert, paras);
                }
            }
        }

        public decimal GetExcess(BO.MotorInsurancePolicy policy, BO.MotorProductMaster product, int Age)
        {
            decimal excessAmount = decimal.Zero;

            var underAgeLimit = product != null ? product.UnderAge : 25;

            var excessRequest = new BKIC.SellingPoint.DL.BO.ExcessAmountRequest
            {
                VehicleMake = policy.VehicleMake,
                VehicleModel = policy.VehicleModel,
                ExcessType = policy.ExcessType,
                Agency = policy.Agency,
                AgentCode = policy.AgencyCode,
                MainClass = policy.Mainclass,
                SubClass = policy.Subclass,
                IsUnderAge = Age < underAgeLimit ? true : false
            };

            var excessResponse = _motorInsuranceRepository.GetExcessCalcualtion(excessRequest);

            if (excessResponse.IsTransactionDone)
            {
                excessAmount = excessResponse.ExcessAmount;
                return excessAmount;
            }
            return excessAmount;
        }

        public decimal GetExcessDiscount(string ExcessType, decimal BasePremium)
        {
            decimal excessDiscount = decimal.Zero;

            if (ExcessType == "Twice" || ExcessType == "4 Times")
            {
                excessDiscount = BasePremium * ExcessDiscountPercent / 100;
            }
            return excessDiscount;
        }

        public decimal GetExcessAdditional(string ExcessType, decimal BasePremium)
        {
            decimal excessAdditional = decimal.Zero;

            if (ExcessType == "None")
            {
                excessAdditional = BasePremium * ExcessDiscountPercent / 100;
            }
            return excessAdditional;
        }

        public decimal GetAgeLoading(decimal AgeLoadingPercent, decimal BasePremium, BO.MotorProductMaster product)
        {
            decimal ageLoading = decimal.Zero;

            if (product.HasAgeLoading && Age < product.UnderAge)
            {
                ageLoading = BasePremium * AgeLoadingPercent / 100;
                if (product.Agency == "TISCO")
                {
                    ProductMinimumPremium = product.UnderAgeminPremium;
                }
            }
            return ageLoading;
        }
        public decimal GetClaimLoading(decimal ClaimAmount, decimal BasePremium, BO.MotorProductMaster product)
        {
            decimal claimLoading = decimal.Zero;
            if (product.MotorClaim != null && product.MotorClaim.Count > 0 && ClaimAmount > 0)
            {
                var claimRow = product.MotorClaim.Find(x => x.AmountFrom <= ClaimAmount && x.AmountTo >= ClaimAmount);
                if (claimRow != null)
                {
                    ClaimLoadPercent = claimRow.Percentage;
                    claimLoading = BasePremium * claimRow.Percentage / 100;
                }
            }
            return claimLoading;
        }

        public decimal GetAdditionalDays(BO.MotorInsurancePolicy policy, BO.MotorProductMaster product, decimal netPremium)
        {
            decimal additionalDaysAmount = 0;

            if (product.HasAdditionalDays)
            {
                if (policy.Agency == "BBK")
                {
                    var actualExpireDate = policy.PolicyCommencementDate.AddYears(1).AddDays(-1);

                    decimal additionalDays = Convert.ToDecimal((policy.PolicyEndDate - actualExpireDate).TotalDays);

                    if (additionalDays > 0)
                    {
                        additionalDaysAmount = netPremium * additionalDays / 365;
                    }
                }
                else
                {
                    DateTime actualExpireDate = DateTime.Now;
                    if (policy.vehicleTypeCode == "Used" || string.IsNullOrEmpty(policy.vehicleTypeCode))
                    {
                        actualExpireDate = policy.PolicyCommencementDate.AddYears(1).AddDays(-1);
                    }
                    else
                    {
                        actualExpireDate = policy.PolicyCommencementDate.AddMonths(13).AddDays(-1);
                    }
                    decimal additionalDays = Convert.ToDecimal((policy.PolicyEndDate - actualExpireDate).TotalDays);
                    if (additionalDays > 0)
                    {
                        additionalDaysAmount = netPremium * additionalDays / 365;
                    }
                }
            }
            return additionalDaysAmount;
        }

        public decimal GetRenewalDelayedDaysAmount(BO.MotorInsurancePolicy policy, BO.MotorProductMaster product, decimal netPremium)
        {
            decimal delayedDaysAmount = 0;

            if (policy.RenewalDelayedDays > 0)
            {
                delayedDaysAmount = (netPremium * policy.RenewalDelayedDays) / 365;
            }
            return delayedDaysAmount;
        }

        public decimal GetCommision(BO.MotorInsurancePolicy policy, decimal totalPremium, bool isDeductable)
        {
            decimal CommissionAmount = 0;

            var commisionRequest = new BO.CommissionRequest
            {
                AgentCode = policy.AgencyCode,
                Agency = policy.Agency,
                SubClass = policy.Subclass,
                PremiumAmount = totalPremium,
                IsDeductable = isDeductable
            };

            var commissionResponse = _insurancePortalRepository.GetCommission(commisionRequest);

            if (commissionResponse.IsTransactionDone)
            {
                CommissionAmount = commissionResponse.CommissionAmount;
            }
            return CommissionAmount;
        }

        public decimal GetTax(decimal premium, decimal taxRate)
        {
            return premium * taxRate / 100;
        }

        public void SetHIR(BO.MotorProductMaster product, BO.MotorInsurancePolicy policy)
        {
            bool isAdded = false;
            if (product.MaximumVehicleAge < DateTime.Now.Year - policy.YearOfMake)
            {
                IsHIR = true;
                HIRReason = "Vehicle year exceed the limit";
                HIRStatus = 1;
                isAdded = true;
            }
            if (product.MaximumVehicleValue < policy.VehicleValue)
            {
                IsHIR = true;
                HIRReason = isAdded ? ", " + "Vehicle value exceeds the limit" : "Vehicle value exceeds the limit";
                HIRStatus = 1;
                isAdded = true;
            }
            if (product.UnderAgeToHIR && Age < product.UnderAge)
            {
                IsHIR = true;
                HIRReason = isAdded ? ", " + "Insured under age" : "Insured under age";
                HIRStatus = 1;
                isAdded = true;
            }
            if (policy.ClaimAmount > 0 && product.MotorClaim != null && product.MotorClaim.Count > 0)
            {
                if (policy.ClaimAmount > product.MotorClaim[0].MaximumClaimAmount)
                {
                    IsHIR = true;
                    HIRReason = isAdded ? ", " + "Claim amount exceed" : "Claim amount exceed";
                    HIRStatus = 1;
                    isAdded = true;
                }

            }
        }
    }
}