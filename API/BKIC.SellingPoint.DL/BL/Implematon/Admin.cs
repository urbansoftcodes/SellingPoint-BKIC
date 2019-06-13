using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DL.StoredProcedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OracleDBIntegration = SellingPoint.OracleDBIntegration;

namespace BKIC.SellingPoint.DL.BL.Implementation
{
    /// <summary>
    /// Portal operations.
    /// Branch - Create,Read,Update,Delete.
    /// Agency - Create,Read,Update,Delete.
    /// User - Create,Read,Update,Delete.
    /// Insured - Create,Read,Update,Delete.
    /// Products - Create,Read,Update,Delete.
    /// </summary>
    public class Admin : IAdmin
    {
        public readonly IMail _mail;
        public readonly OracleDBIntegration.Implementation.InsuranceMaster _oracleInsuredMaster;

        public Admin()
        {
            _oracleInsuredMaster = new OracleDBIntegration.Implementation.InsuranceMaster();
        }

        /// <summary>
        /// Convert table columns into property of the appropriate class type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns>Master table columns with converted class properties</returns>
        public MasterTableResponse<T> GetMasterTableByTableName<T>(string tableName) where T : class
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Type",tableName)
                };
                DataTable dt = BKICSQL.edt(AdminSP.FetchInformation, paras);
                var result = new MasterTableResponse<T>();
                result.TableRows = dt.BindList<T>();
                result.IsTransactionDone = true;

                return result;
            }
            catch (Exception ex)
            {
                return new MasterTableResponse<T>()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on agent master.
        /// </summary>
        /// <param name="agentDetails">agent details</param>
        /// <returns></returns>
        public AgentMasterResponse AgentOperation(AgentMaster agentDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Id",agentDetails.Id),
                   new SqlParameter("@Agency",agentDetails.Agency??string.Empty),
                   new SqlParameter("@AgentCode",agentDetails.AgentCode??string.Empty),
                   new SqlParameter("@AgentBranch",agentDetails.AgentBranch??string.Empty),
                   new SqlParameter("@CustomerCode", agentDetails.CustomerCode ?? string.Empty),
                   new SqlParameter("@IsActive",agentDetails.IsActive),
                   new SqlParameter("@Type",agentDetails.Type)
                };
                DataTable dt = BKICSQL.edt(AdminSP.AgentMasterOperation, paras);
                return new AgentMasterResponse { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new AgentMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on branch master.
        /// </summary>
        /// <param name="branchDetails">Branch details</param>
        /// <returns></returns>
        public BranchMasterResponse BranchOperation(BranchMaster branchDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Id",branchDetails.Id),
                   new SqlParameter("@Agency",branchDetails.Agency??string.Empty),
                   new SqlParameter("@AgentCode",branchDetails.AgentCode??string.Empty),
                   new SqlParameter("@AgentBranch",branchDetails.AgentBranch??string.Empty),
                   new SqlParameter("@BranchName",branchDetails.BranchName??string.Empty),
                   new SqlParameter("@BranchAddress",branchDetails.BranchAddress??string.Empty),
                   new SqlParameter("@Phone ",branchDetails.Phone??string.Empty),
                   new SqlParameter("@Incharge",branchDetails.Incharge??string.Empty),
                   new SqlParameter("@Email",branchDetails.Email??string.Empty),
                   new SqlParameter("@Type",branchDetails.Type??string.Empty),
                   new SqlParameter("@CreatedBy",branchDetails.CreatedBy??string.Empty)
                };
                DataTable dt = BKICSQL.edt(AdminSP.BranchDetailsOperation, paras);
                return new BranchMasterResponse { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new BranchMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on insurance product master.
        /// </summary>
        /// <param name="branchDetails">Branch details</param>
        /// <returns></returns>
        public InsuranceProductMasterResponse InsuranceProductOperation(InsuranceProductMaster productDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Id",productDetails.Id),
                   new SqlParameter("@Agency",productDetails.Agency??string.Empty),
                   new SqlParameter("@AgentCode",productDetails.AgentCode??string.Empty),
                   new SqlParameter("@MainClass",productDetails.Mainclass??string.Empty),
                   new SqlParameter("@Subclass",productDetails.SubClass??string.Empty),
                   new SqlParameter("@EffectiveDateFrom",productDetails.EffectiveDateFrom ?? DateTime.Now),
                   new SqlParameter("@EffectiveDateTo",productDetails.EffectiveDateTo??DateTime.Now),
                   new SqlParameter("@IsActive",productDetails.IsActive),
                   new SqlParameter("@CreatedDate",productDetails.CreatedDate??DateTime.Now),
                   new SqlParameter("@UpdatedDate",productDetails.UpdatedDate??DateTime.Now),
                   new SqlParameter("@Type",productDetails.Type ?? string.Empty),
                   new SqlParameter("@CreatedBy",productDetails.CreatedBy??string.Empty)
                };
                DataTable dt = BKICSQL.edt(AdminSP.InsuranceMasterOperation, paras);
                return new InsuranceProductMasterResponse { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new InsuranceProductMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on motorcover master.
        /// </summary>
        /// <param name="coverDetails">cover details</param>
        /// <returns></returns>
        public MotorCoverMasterResponse MotorCoverOperation(MotorCoverMaster coverDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@CoverId",coverDetails.CoverId),
                   new SqlParameter("@CoverDescription",coverDetails.CoversDescription ?? string.Empty),
                   new SqlParameter("@CoverCode",coverDetails.CoversCode ?? string.Empty),
                   new SqlParameter("@Type",coverDetails.Type ?? string.Empty),
                };
                DataTable dt = BKICSQL.edt(AdminSP.MotorCoverMasterOperation, paras);
                return new MotorCoverMasterResponse { IsTransactionDone = true };
            }
            catch (Exception ex)
            {
                return new MotorCoverMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on motor product cover master.
        /// </summary>
        /// <param name="coverDetails">cover details</param>
        /// <returns></returns>
        public MotorProductCoverResponse MotorProductCoverOperation(MotorProductCover productCoverDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@CoverId",productCoverDetails.CoverId),
                   new SqlParameter("@Agency",productCoverDetails.Agency ?? string.Empty),
                   new SqlParameter("@AgentCode",productCoverDetails.AgencyCode ?? string.Empty),
                   new SqlParameter("@Mainclass",productCoverDetails.Mainclass ?? string.Empty),
                   new SqlParameter("@SubClass",productCoverDetails.SubClass ?? string.Empty),
                   new SqlParameter("@CoverAmount",productCoverDetails.CoverAmount),
                   new SqlParameter("@CoverCode",productCoverDetails.CoverCode ?? string.Empty),
                    new SqlParameter("@CoverDescription", productCoverDetails.CoverDescription ?? string.Empty),
                   new SqlParameter("@IsOptionalCover", productCoverDetails.IsOptionalCover),
                   new SqlParameter("@CoverType",productCoverDetails.CoverType??string.Empty),
                   new SqlParameter("@Type",productCoverDetails.Type??string.Empty)
                };
                DataTable dt = BKICSQL.edt(AdminSP.MotorProductCoverOperation, paras);

                if (productCoverDetails.Type == "fetch")
                {
                }
                var coverDescriptions = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coverDescriptions += dt.Rows[i].IsNull("CoverDescription") ? "" : dt.Rows[i]["CoverDescription"].ToString() + " , ";
                }
                coverDescriptions = coverDescriptions.TrimStart(' ').TrimEnd(' ').TrimStart(',').Trim(',');
                return new MotorProductCoverResponse
                {
                    IsTransactionDone = true,
                    CoverDescription = coverDescriptions
                };
            }
            catch (Exception ex)
            {
                return new MotorProductCoverResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        ///  CRUD opeartion on motor vehicle master.
        /// </summary>
        /// <param name="motorVehicleMaster">motor vehicle master.</param>
        /// <returns></returns>
        public MotorVehicleMasterResponse MotorVehicleMasterOperation(MotorVehicleMaster motorVehicleMaster)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                    new SqlParameter("@ID",motorVehicleMaster.ID),
                   new SqlParameter("@Make",motorVehicleMaster.Make ?? string.Empty),
                   new SqlParameter("@MakeDescription",motorVehicleMaster.MakeDescription ?? string.Empty),
                   new SqlParameter("@Model",motorVehicleMaster.Model ?? string.Empty),
                   new SqlParameter("@ModelDescription",motorVehicleMaster.ModelDescription ?? string.Empty),
                   new SqlParameter("@Body",motorVehicleMaster.Body ?? string.Empty),
                   new SqlParameter("@VehicleValue",motorVehicleMaster.VehicleValue),
                   new SqlParameter("@Tonnage",motorVehicleMaster.Tonnage),
                   new SqlParameter("@NewExcessAmount",motorVehicleMaster.NewExcessAmount),
                   new SqlParameter("@Year",motorVehicleMaster.Year),
                   new SqlParameter("@Category",string.Empty),
                   new SqlParameter("@SeatingCapacity",motorVehicleMaster.SeatingCapacity),
                   new SqlParameter("@Type",motorVehicleMaster.Type),
                };
                DataTable dt = BKICSQL.edt(AdminSP.MotorVehicleMasterOperation, paras);
                List<MotorVehicleMaster> VehicleMasters = new List<MotorVehicleMaster>();
                if (motorVehicleMaster.Type == "fetch")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var dr = dt.Rows[i];
                            VehicleMasters.Add(
                                    new MotorVehicleMaster
                                    {
                                        ID = dr.IsNull("ID") ? 0 : Convert.ToInt32(dr["ID"]),
                                        Make = dr.IsNull("Make") ? string.Empty : Convert.ToString(dr["Make"]),
                                        MakeDescription = dr.IsNull("MakeDescription") ? string.Empty : Convert.ToString(dr["MakeDescription"]),
                                        Model = dr.IsNull("Model") ? string.Empty : Convert.ToString(dr["Model"]),
                                        ModelDescription = dr.IsNull("ModelDescription") ? string.Empty : Convert.ToString(dr["ModelDescription"]),
                                        Body = dr.IsNull("Body") ? string.Empty : Convert.ToString(dr["Body"]),
                                        Tonnage = dr.IsNull("Tonnage") ? 0 : Convert.ToInt32(dr["Tonnage"]),
                                        // Type = dr.IsNull("Type") ? string.Empty : Convert.ToString(dr["Type"]),
                                        //  Category = dr.IsNull("Category") ? string.Empty : Convert.ToString(dr["Category"]),
                                        NewExcessAmount = dr.IsNull("NewExcessAmount") ? decimal.Zero : Convert.ToDecimal(dr["NewExcessAmount"]),
                                        VehicleValue = dr.IsNull("VehicleValue") ? 0 : Convert.ToInt32(dr["VehicleValue"]),
                                        SeatingCapacity = dr.IsNull("SeatingCapacity") ? 0 : Convert.ToInt32(dr["SeatingCapacity"]),
                                        Year = dr.IsNull("Year") ? 0 : Convert.ToInt32(dr["Year"])
                                    });
                        }
                    }
                }
                return new MotorVehicleMasterResponse
                {
                    IsTransactionDone = true,
                    MotorVehicleMaster = VehicleMasters
                };
            }
            catch (Exception ex)
            {
                return new MotorVehicleMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on user master.
        /// </summary>
        /// <param name="userDetails">user details</param>
        /// <returns></returns>
        public UserMasterDetailsResponse UserOperation(UserMasterDetails userDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Id",userDetails.Id),
                   new SqlParameter("@Agency",userDetails.Agency??string.Empty),
                   new SqlParameter("@AgentCode",userDetails.AgentCode??string.Empty),
                   new SqlParameter("@AgentBranch",userDetails.AgentBranch??string.Empty),
                   new SqlParameter("@UserID",userDetails.UserId??string.Empty),
                   new SqlParameter("@UserName",userDetails.UserName??string.Empty),
                   new SqlParameter("@CreatedDate",userDetails.CreatedDate),
                   new SqlParameter("@Mobile",userDetails.Mobile??string.Empty),
                   new SqlParameter("@Email",userDetails.Email??string.Empty),
                   new SqlParameter("@StaffNo",userDetails.StaffNo),
                   new SqlParameter("@Role",userDetails.Role ?? ""),
                   new SqlParameter("@CreatedBy",userDetails.CreatedBy ?? ""),
                   new SqlParameter("@IsActive",userDetails.IsActive),
                   new SqlParameter("@Type",userDetails.Type?? string.Empty)
                };

                DataTable dt = BKICSQL.edt(AdminSP.UserMasterOperation, paras);
                var ListUsers = new List<UserMasterDetails>();
                if (userDetails.Type == "fetch")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListUsers.Add(
                                    new UserMasterDetails
                                    {
                                        Agency = dt.Rows[i]["Agency"].ToString(),
                                        AgentCode = dt.Rows[i]["AgentCode"].ToString(),
                                        UserName = dt.Rows[i]["UserName"].ToString(),
                                        UserId = dt.Rows[i]["UserId"].ToString(),
                                        Id = Convert.ToInt32(dt.Rows[i]["Id"].ToString())
                                    });
                        }
                    }
                }
                return new UserMasterDetailsResponse
                {
                    IsTransactionDone = true,
                    UserMaster = ListUsers
                };
            }
            catch (Exception ex)
            {
                return new UserMasterDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on Category(Commission) master.
        /// </summary>
        /// <param name="categoryDetails">category(commission) details</param>
        /// <returns></returns>
        public CategoryMasterResponse CategoryOperation(CategoryMaster categoryDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@id",categoryDetails.id),
                    new SqlParameter("@Agency",categoryDetails.Agency??string.Empty),
                   new SqlParameter("@AgenctCode",categoryDetails.AgentCode??string.Empty),
                   new SqlParameter("@MainClass",categoryDetails.MainClass??string.Empty),
                   new SqlParameter("@SubClass",categoryDetails.SubClass??string.Empty),
                   new SqlParameter("@Category",categoryDetails.Category??string.Empty),
                   new SqlParameter("@Code",categoryDetails.Code??string.Empty),
                   new SqlParameter("@ValueType",categoryDetails.ValueType ?? string.Empty),
                   new SqlParameter("@Value",categoryDetails.Value),
                   new SqlParameter("@EffectiveFrom",categoryDetails.EffectiveFrom ?? DateTime.Now),
                   new SqlParameter("@EffectiveTo",categoryDetails.EffectiveTo ?? DateTime.Now),
                   new SqlParameter("@Status",categoryDetails.Status),
                   new SqlParameter("@Type",categoryDetails.Type ?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(AdminSP.CategoryMasterOperation, paras);
                List<CategoryMaster> categories = new List<CategoryMaster>();
                if (categoryDetails.Type == "fetch" && dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CategoryMaster category = new CategoryMaster();
                        category.Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]);
                        category.AgentCode = dr.IsNull("AgentCode") ? string.Empty : Convert.ToString(dr["AgentCode"]);
                        category.Category = dr.IsNull("Category") ? string.Empty : Convert.ToString(dr["Category"]);
                        category.Code = dr.IsNull("Code") ? string.Empty : Convert.ToString(dr["Code"]);
                        category.Value = dr.IsNull("Value") ? decimal.Zero : Convert.ToDecimal(dr["Value"]);
                        category.ValueType = dr.IsNull("ValueType") ? string.Empty : Convert.ToString(dr["ValueType"]);
                        category.id = dr.IsNull("id") ? 0 : Convert.ToInt32(dr["id"]);
                        category.IsDeductable = dr.IsNull("IsDeductable") ? false : Convert.ToBoolean(dr["IsDeductable"]);
                        category.MainClass = dr.IsNull("MainClass") ? string.Empty : Convert.ToString(dr["MainClass"]);
                        category.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);

                        categories.Add(category);
                    }

                }
                return new CategoryMasterResponse
                {
                    IsTransactionDone = true,
                    Categories = categories
                };
            }
            catch (Exception ex)
            {
                return new CategoryMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// CRUD opeartion on insured master.
        /// </summary>
        /// <param name="insuredDetails">insured details</param>
        /// <returns></returns>
        public InsuredMasterDetailsResponse InsuredOperation(InsuredMasterDetails insuredDetails)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@InsuredId",insuredDetails.InsuredId),
                   new SqlParameter("@Agency",insuredDetails.Agency??string.Empty),
                   new SqlParameter("@AgentCode",insuredDetails.AgentCode??string.Empty),
                   new SqlParameter("@AgentBranch",insuredDetails.AgentBranch??string.Empty),
                   new SqlParameter("@CPR",insuredDetails.CPR??string.Empty),
                   new SqlParameter("@FirstName",insuredDetails.FirstName??string.Empty),
                   new SqlParameter("@MiddleName",insuredDetails.MiddleName??string.Empty),
                   new SqlParameter("@LastName",insuredDetails.LastName??string.Empty),
                   new SqlParameter("@Gender",insuredDetails.Gender??string.Empty),
                   new SqlParameter("@Flat",insuredDetails.Flat??string.Empty),
                   new SqlParameter("@Building",insuredDetails.Building??string.Empty),
                   new SqlParameter("@Road",insuredDetails.Road??string.Empty),
                   new SqlParameter("@Block",insuredDetails.Block??string.Empty),
                   new SqlParameter("@Area",insuredDetails.Area??string.Empty),
                   new SqlParameter("@Mobile",insuredDetails.Mobile??string.Empty),
                   new SqlParameter("@Email",insuredDetails.Email??string.Empty),
                   new SqlParameter("@DateOfBirth",insuredDetails.DateOfBirth ?? DateTime.Now ),
                   new SqlParameter("@Nationality",insuredDetails.Nationality??string.Empty),
                   new SqlParameter("@Occupation",insuredDetails.Occupation??string.Empty),
                   new SqlParameter("@PassportNo",insuredDetails.PassportNo??string.Empty),
                   new SqlParameter("@IsActive",insuredDetails.IsActive),
                   new SqlParameter("@Type",insuredDetails.Type)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() {   OutPutType = SqlDbType.NVarChar, ParameterName= "@InsuredCode", Size=50},
                    new SPOut() {   OutPutType = SqlDbType.NVarChar, ParameterName= "@InsuredName", Size=50},
                };
                object[] dataSet = BKICSQL.GetValues(AdminSP.InsuredMasterOperation, paras, outParams);
                string insuredCode = string.Empty;
                string insuredName = string.Empty;
                if (dataSet != null)
                {
                    if (dataSet[0] != null)
                        insuredCode = Convert.ToString(dataSet[0]);
                    if (dataSet[1] != null)
                        insuredName = Convert.ToString(dataSet[1]);
                }
                if (!string.IsNullOrEmpty(insuredCode))
                {
                    //Task moveToOracleTask = Task.Factory.StartNew(() =>
                    //                        {
                    //                            OracleDBIntegration.DBObjects.TransactionWrapper
                    //                            oracleResult = _oracleInsuredMaster.IntegrateInsuredMasterToOracle(insuredCode);
                    //                        });

                    try
                    {
                        //moveToOracleTask.Wait();
                        new Task(() => {
                            OracleDBIntegration.DBObjects.TransactionWrapper
                                                oracleResult = _oracleInsuredMaster.IntegrateInsuredMasterToOracle(insuredCode);
                        }).Start();
                    }
                    catch (AggregateException ex)
                    {
                        foreach (Exception inner in ex.InnerExceptions)
                        {
                            _mail.SendMailLogError(ex.Message, insuredCode, "Insured", "", true);
                        }
                    }
                }

                return new InsuredMasterDetailsResponse
                {
                    IsTransactionDone = true,
                    InsuredCode = insuredCode,
                    InsuredName = insuredName
                };
            }
            catch (Exception ex)
            {
                return new InsuredMasterDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get insured details by cpr or insured code.
        /// </summary>
        /// <param name="request">Insured request.</param>
        /// <returns>insured details.</returns>
        public InsuredResponse GetInsuredDetailsByCPRInsuredCode(InsuredRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@CPR", request.CPR ?? string.Empty),
                    new SqlParameter("@InsuredCode",request.InsuredCode ?? string.Empty),
                    new SqlParameter("@Agency",request.Agency ?? string.Empty),
                    new SqlParameter("@AgentCode",request.AgentCode ?? string.Empty),
                };

                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.FetchUserDetailsByCPRInsuredCode, para);
                var insuredMaster = new InsuredMasterDetails();
                var InsuredResult = new InsuredResponse();
                if (resultdt != null && resultdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        insuredMaster.InsuredId = dr.IsNull("InsuredId") ? 0 : Convert.ToInt64(dr["InsuredId"]);
                        insuredMaster.InsuredCode = dr.IsNull("InsuredCode") ? string.Empty : Convert.ToString(dr["InsuredCode"]);
                        insuredMaster.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                        insuredMaster.FirstName = dr.IsNull("FirstName") ? string.Empty : Convert.ToString(dr["FirstName"]);
                        insuredMaster.MiddleName = dr.IsNull("MiddleName") ? string.Empty : Convert.ToString(dr["MiddleName"]);
                        insuredMaster.LastName = dr.IsNull("LastName") ? string.Empty : Convert.ToString(dr["LastName"]);
                        insuredMaster.Gender = dr.IsNull("Gender") ? string.Empty : Convert.ToString(dr["Gender"]);
                        insuredMaster.Nationality = dr.IsNull("NATIONALITY") ? string.Empty : Convert.ToString(dr["NATIONALITY"]);
                        insuredMaster.Flat = dr.IsNull("Flat") ? string.Empty : Convert.ToString(dr["Flat"]);
                        insuredMaster.Building = dr.IsNull("Building") ? string.Empty : Convert.ToString(dr["Building"]);
                        insuredMaster.Road = dr.IsNull("Road") ? string.Empty : Convert.ToString(dr["Road"]);
                        insuredMaster.Block = dr.IsNull("Block") ? string.Empty : Convert.ToString(dr["Block"]);
                        insuredMaster.Area = dr.IsNull("Area") ? string.Empty : Convert.ToString(dr["Area"]);
                        insuredMaster.Mobile = dr.IsNull("Mobile") ? string.Empty : Convert.ToString(dr["Mobile"]);
                        insuredMaster.Email = dr.IsNull("Email") ? string.Empty : Convert.ToString(dr["Email"]);
                        insuredMaster.DateOfBirth = DateTime.ParseExact(Convert.ToDateTime(dr["DateOfBirth"]).CovertToLocalFormat(), "dd/MM/yyyy", null);
                        insuredMaster.Occupation = dr.IsNull("Occupation") ? string.Empty : Convert.ToString(dr["Occupation"]);
                        insuredMaster.PassportNo = dr.IsNull("PassportNo") ? string.Empty : Convert.ToString(dr["PassportNo"]);
                        insuredMaster.InsuredName = dr.IsNull("InsuredName") ? string.Empty : Convert.ToString(dr["InsuredName"]);
                    }
                    InsuredResult.InsuredDetails = insuredMaster;
                    InsuredResult.IsTransactionDone = true;
                    return InsuredResult;
                }
                else
                {
                    return new InsuredResponse
                    {
                        IsTransactionDone = false,
                        TransactionErrorMessage = "Insured not found",
                    };
                }
                
            }
            catch (Exception ex)
            {
                return new InsuredResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all the insured belonging to the agency.(To show the all the cpr's in policy buying page)
        /// </summary>
        /// <param name="request">Insured request</param>
        /// <returns>list of insured belonging to the agency.</returns>
        public AgencyInsuredResponse GetAgencyInsured(AgencyInsuredRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Agency", request.Agency??string.Empty),
                    new SqlParameter("@AgentCode",request.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch",request.AgentBranch??string.Empty),
                    new SqlParameter("@CPR",request.CPR??string.Empty),
                };

                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.GetAgencyInsured, para);
                List<InsuredMasterDetails> insuredMasterDetails = new List<InsuredMasterDetails>();
                if (resultdt != null && resultdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        var insuredMaster = new InsuredMasterDetails();

                        insuredMaster.InsuredId = dr.IsNull("InsuredId") ? 0 : Convert.ToInt64(dr["InsuredId"]);
                        insuredMaster.InsuredCode = dr.IsNull("InsuredCode") ? string.Empty : Convert.ToString(dr["InsuredCode"]);
                        insuredMaster.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                        insuredMaster.FirstName = dr.IsNull("FirstName") ? string.Empty : Convert.ToString(dr["FirstName"]);
                        insuredMaster.MiddleName = dr.IsNull("MiddleName") ? string.Empty : Convert.ToString(dr["MiddleName"]);
                        insuredMaster.LastName = dr.IsNull("LastName") ? string.Empty : Convert.ToString(dr["LastName"]);
                        insuredMaster.Gender = dr.IsNull("Gender") ? string.Empty : Convert.ToString(dr["Gender"]);
                        insuredMaster.Nationality = dr.IsNull("NATIONALITY") ? string.Empty : Convert.ToString(dr["NATIONALITY"]);
                        insuredMaster.Flat = dr.IsNull("Flat") ? string.Empty : Convert.ToString(dr["Flat"]);
                        insuredMaster.Building = dr.IsNull("Building") ? string.Empty : Convert.ToString(dr["Building"]);
                        insuredMaster.Road = dr.IsNull("Road") ? string.Empty : Convert.ToString(dr["Road"]);
                        insuredMaster.Block = dr.IsNull("Block") ? string.Empty : Convert.ToString(dr["Block"]);
                        insuredMaster.Area = dr.IsNull("Area") ? string.Empty : Convert.ToString(dr["Area"]);
                        insuredMaster.Mobile = dr.IsNull("Mobile") ? string.Empty : Convert.ToString(dr["Mobile"]);
                        insuredMaster.Email = dr.IsNull("Email") ? string.Empty : Convert.ToString(dr["Email"]);
                        insuredMaster.DateOfBirth = dr.IsNull("DateOfBirth") ? DateTime.Now : DateTime.ParseExact(Convert.ToDateTime(dr["DateOfBirth"]).CovertToLocalFormat(), "dd/MM/yyyy", null);
                        insuredMaster.Occupation = dr.IsNull("Occupation") ? string.Empty : Convert.ToString(dr["Occupation"]);
                        insuredMaster.PassportNo = dr.IsNull("PassportNo") ? string.Empty : Convert.ToString(dr["PassportNo"]);

                        insuredMasterDetails.Add(insuredMaster);
                    }
                }
                return new AgencyInsuredResponse
                {
                    AgencyInsured = insuredMasterDetails,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyInsuredResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        /// <summary>
        /// Get all the insured belonging to the agency.(To show the all the cpr's in policy buying page)
        /// </summary>
        /// <param name="request">Insured request</param>
        /// <returns>list of insured belonging to the agency.</returns>
        public AgencyInsuredResponse GetAgencyCPR(string CPR, string Agency)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Agency", Agency),
                    new SqlParameter("@CPR",CPR)
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.GetAgencyCPR, para);
                List<InsuredMasterDetails> insuredMasterDetails = new List<InsuredMasterDetails>();
                if (resultdt != null && resultdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        var insuredMaster = new InsuredMasterDetails();

                        insuredMaster.InsuredId = dr.IsNull("InsuredId") ? 0 : Convert.ToInt64(dr["InsuredId"]);
                        insuredMaster.InsuredCode = dr.IsNull("InsuredCode") ? string.Empty : Convert.ToString(dr["InsuredCode"]);
                        insuredMaster.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                        insuredMaster.FirstName = dr.IsNull("FirstName") ? string.Empty : Convert.ToString(dr["FirstName"]);
                        insuredMaster.MiddleName = dr.IsNull("MiddleName") ? string.Empty : Convert.ToString(dr["MiddleName"]);
                        insuredMaster.LastName = dr.IsNull("LastName") ? string.Empty : Convert.ToString(dr["LastName"]);
                        insuredMaster.Gender = dr.IsNull("Gender") ? string.Empty : Convert.ToString(dr["Gender"]);
                        insuredMaster.Nationality = dr.IsNull("NATIONALITY") ? string.Empty : Convert.ToString(dr["NATIONALITY"]);
                        insuredMaster.Flat = dr.IsNull("Flat") ? string.Empty : Convert.ToString(dr["Flat"]);
                        insuredMaster.Building = dr.IsNull("Building") ? string.Empty : Convert.ToString(dr["Building"]);
                        insuredMaster.Road = dr.IsNull("Road") ? string.Empty : Convert.ToString(dr["Road"]);
                        insuredMaster.Block = dr.IsNull("Block") ? string.Empty : Convert.ToString(dr["Block"]);
                        insuredMaster.Area = dr.IsNull("Area") ? string.Empty : Convert.ToString(dr["Area"]);
                        insuredMaster.Mobile = dr.IsNull("Mobile") ? string.Empty : Convert.ToString(dr["Mobile"]);
                        insuredMaster.Email = dr.IsNull("Email") ? string.Empty : Convert.ToString(dr["Email"]);
                        insuredMaster.DateOfBirth = dr.IsNull("DateOfBirth") ? DateTime.Now : DateTime.ParseExact(Convert.ToDateTime(dr["DateOfBirth"]).CovertToLocalFormat(), "dd/MM/yyyy", null);
                        insuredMaster.Occupation = dr.IsNull("Occupation") ? string.Empty : Convert.ToString(dr["Occupation"]);
                        insuredMaster.PassportNo = dr.IsNull("PassportNo") ? string.Empty : Convert.ToString(dr["PassportNo"]);

                        insuredMasterDetails.Add(insuredMaster);
                    }
                }
                return new AgencyInsuredResponse
                {
                    AgencyInsured = insuredMasterDetails,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyInsuredResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get the documents(policies) belonging to certain CPR.
        /// </summary>
        /// <param name="request">document details request.</param>
        /// <returns>list of documents belonging to certain CPR.</returns>
        public DocumentDetailsResponse GetDocumentsByCPR(DocumentDetailsRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@CPR", request.CPR??string.Empty),
                    new SqlParameter("@AgentCode",request.AgentCode??string.Empty)
                };

                DataSet resultdt = BKICSQL.eds(StoredProcedures.AdminSP.GetDocumentsByCPR, para);
                var documentResult = new DocumentDetailsResponse();
                var documentDetails = new List<DocumentDetails>();

                if (resultdt != null && resultdt.Tables.Count > 0)
                {
                    for (int i = 0; i < resultdt.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = resultdt.Tables[0].Rows[i];
                        documentDetails.Add(new DocumentDetails
                        {
                            DocumentNo = Convert.ToString(dr["PolicyNo"]),
                            PolicyType = "DomesticHelp",
                            ExpireDate = Convert.ToDateTime(dr["ExpiryDate"]).Date.CovertToLocalFormat(),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                        });
                    }
                    for (int i = 0; i < resultdt.Tables[1].Rows.Count; i++)
                    {
                        DataRow dr = resultdt.Tables[1].Rows[i];
                        documentDetails.Add(new DocumentDetails
                        {
                            DocumentNo = Convert.ToString(dr["PolicyNo"]),
                            PolicyType = "Travel",
                            ExpireDate = Convert.ToDateTime(dr["ExpiryDate"]).Date.CovertToLocalFormat(),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                        });
                    }
                    for (int i = 0; i < resultdt.Tables[2].Rows.Count; i++)
                    {
                        DataRow dr = resultdt.Tables[2].Rows[i];
                        documentDetails.Add(new DocumentDetails
                        {
                            DocumentNo = Convert.ToString(dr["PolicyNo"]),
                            PolicyType = "Home",
                            ExpireDate = Convert.ToDateTime(dr["ExpiryDate"]).Date.CovertToLocalFormat(),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                        });
                    }
                    for (int i = 0; i < resultdt.Tables[3].Rows.Count; i++)
                    {
                        DataRow dr = resultdt.Tables[3].Rows[i];
                        documentDetails.Add(new DocumentDetails
                        {
                            DocumentNo = Convert.ToString(dr["PolicyNo"]),
                            PolicyType = "Motor",
                            ExpireDate = Convert.ToDateTime(dr["ExpiryDate"]).Date.CovertToLocalFormat(),
                            RenewalCount = dr.IsNull("RenewalCount") ? 0 : Convert.ToInt32(dr["RenewalCount"])
                        });
                    }
                }
                return new DocumentDetailsResponse
                {
                    DocumentDetails = documentDetails,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new DocumentDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get all the users belonging to certain agency.
        /// </summary>
        /// <param name="request">Agency request.</param>
        /// <returns>list of agency users.</returns>
        public AgencyUserResponse GetAgencyUser(AgencyUserRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Agency", request.Agency??string.Empty),
                    new SqlParameter("@AgentCode",request.AgentCode??string.Empty),
                    new SqlParameter("@AgentBranch",request.AgentBranch??string.Empty),
                    new SqlParameter("@UserName", request.UserName ?? string.Empty),
                    new SqlParameter("@UserId", request.UserId ?? string.Empty),
                };

                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.GetAgencyUsers, para);
                List<UserMasterDetails> userMasterDetails = new List<UserMasterDetails>();
                if (resultdt != null && resultdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        var userMaster = new UserMasterDetails();

                        userMaster.UserId = dr.IsNull("UserId") ? string.Empty : Convert.ToString(dr["UserId"]);
                        userMaster.UserName = dr.IsNull("UserName") ? string.Empty : Convert.ToString(dr["UserName"]);
                        userMaster.Role = dr.IsNull("Role") ? string.Empty : Convert.ToString(dr["Role"]);
                        userMaster.Id = dr.IsNull("Id") ? 0 : Convert.ToInt32(dr["Id"]);
                        userMaster.Email = dr.IsNull("Email") ? string.Empty : Convert.ToString(dr["Email"]);
                        userMaster.IsActive = dr.IsNull("IsActive") ? false : Convert.ToBoolean(dr["IsActive"]);
                        userMaster.Mobile = dr.IsNull("Mobile") ? string.Empty : Convert.ToString(dr["Mobile"]);
                        userMaster.StaffNo = dr.IsNull("StaffNo") ? 0 : Convert.ToInt32(dr["StaffNo"]);
                        userMaster.Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]);
                        userMaster.AgentCode = dr.IsNull("AgentCode") ? string.Empty : Convert.ToString(dr["AgentCode"]);
                        userMaster.AgentBranch = dr.IsNull("AgentBranch") ? string.Empty : Convert.ToString(dr["AgentBranch"]);
                        // userMaster.Type = dr.IsNull("Type") ? string.Empty : Convert.ToString(dr["Type"]);

                        userMasterDetails.Add(userMaster);
                    }
                }
                return new AgencyUserResponse
                {
                    AgencyUsers = userMasterDetails,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyUserResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get agency product's,
        /// To validate the minimum and maximum sum insured limits in the time of endoresment.
        /// admin can override the maximum value user can't,
        /// </summary>
        /// <param name="request">Agency product request.</param>
        /// <returns>list of product by insurance type and agency</returns>
        public AgencyProductResponse GetAgencyProducts(AgecyProductRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[] {
                    new SqlParameter("@Agency", request.Agency??string.Empty),
                    new SqlParameter("@AgentCode",request.AgentCode??string.Empty),
                    new SqlParameter("@MainClass",request.MainClass??string.Empty),
                    new SqlParameter("@SubClass", request.SubClass ?? string.Empty),
                    new SqlParameter("@Type", request.Type?? string.Empty),
                };

                DataSet resultdt = BKICSQL.eds(StoredProcedures.AdminSP.GETAgencyProductByType, para);
                List<MotorProduct> motorProducts = new List<MotorProduct>();
                List<HomeProduct> homeProducts = new List<HomeProduct>();

                if (resultdt != null && resultdt.Tables[0].Rows.Count > 0)
                {
                    if (request.Type == "MotorInsurance")
                    {
                        foreach (DataRow dr in resultdt.Tables[0].Rows)
                        {
                            var motorProduct = new MotorProduct();

                            motorProduct.Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]);
                            motorProduct.AgentCode = dr.IsNull("AgentCode") ? string.Empty : Convert.ToString(dr["AgentCode"]);
                            motorProduct.Description = dr.IsNull("Description") ? string.Empty : Convert.ToString(dr["Description"]);
                            motorProduct.HasAdditionalDays = dr.IsNull("HasAdditionalDays") ? false : Convert.ToBoolean(dr["HasAdditionalDays"]);
                            motorProduct.HasAgeLoading = dr.IsNull("HasAgeLoading") ? false : Convert.ToBoolean(dr["HasAgeLoading"]);
                            motorProduct.MinimumPremium = dr.IsNull("MinimumPermium") ? decimal.Zero : Convert.ToDecimal(dr["MinimumPermium"]);
                            motorProduct.MainClass = dr.IsNull("MainClass") ? string.Empty : Convert.ToString(dr["MainClass"]);
                            motorProduct.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                            motorProduct.UnderAgeminPremium = dr.IsNull("UnderAgeminPremium") ? decimal.Zero : Convert.ToDecimal(dr["UnderAgeminPremium"]);
                            motorProduct.Rate = dr.IsNull("Rate") ? decimal.Zero : Convert.ToDecimal(dr["Rate"]);
                            motorProduct.MaximumVehicleAge = dr.IsNull("MaximumVehicleAge") ? 0 : Convert.ToInt32(dr["MaximumVehicleAge"]);
                            motorProduct.MaximumVehicleValue = dr.IsNull("MaxVehicleValue") ? 0 : Convert.ToInt32(dr["MaxVehicleValue"]);
                            motorProduct.AllowUnderAge = dr.IsNull("AllowUnderAge") ? false : Convert.ToBoolean(dr["AllowUnderAge"]);
                            motorProduct.UnderAge = dr.IsNull("UnderAge") ? 0 : Convert.ToInt32(dr["UnderAge"]);
                            motorProduct.AllowMaxVehicleAge = dr.IsNull("AllowMaxVehicleAge") ? false : Convert.ToBoolean(dr["AllowMaxVehicleAge"]);
                            motorProduct.GCCCoverRangeInYears = dr.IsNull("GCCCoverRangeInYears") ? 0 : Convert.ToInt32(dr["GCCCoverRangeInYears"]);
                            motorProduct.HasGCC = dr.IsNull("HasGCC") ? false : Convert.ToBoolean(dr["HasGCC"]);
                            motorProduct.IsProductSport = dr.IsNull("IsProductSport") ? false : Convert.ToBoolean(dr["IsProductSport"]);
                            motorProduct.AllowUsedVehicle = dr.IsNull("AllowUsedVehicle") ? false : Convert.ToBoolean(dr["AllowUsedVehicle"]);

                            motorProducts.Add(motorProduct);
                        }
                        if (resultdt.Tables[1] != null && resultdt.Tables[1].Rows.Count > 0)
                        {
                            foreach (var mp in motorProducts)
                            {
                                mp.MotorClaim = new List<MotorClaim>();
                                foreach (DataRow drow in resultdt.Tables[1].Rows)
                                {
                                    mp.MotorClaim.Add(new MotorClaim
                                    {
                                        AmountFrom = drow.IsNull("AmountFrom") ? 0 : Convert.ToDecimal(drow["AmountFrom"]),
                                        AmountTo = drow.IsNull("AmountTo") ? 0 : Convert.ToDecimal(drow["AmountTo"]),
                                        Percentage = drow.IsNull("Percentage") ? 0 : Convert.ToDecimal(drow["Percentage"]),
                                        MaximumClaimAmount = drow.IsNull("MaxClaimAmount") ? 0 : Convert.ToDecimal(drow["MaxClaimAmount"]),
                                        MainClass = drow.IsNull("MainClass") ? string.Empty : Convert.ToString(drow["MainClass"]),
                                        SubClass = drow.IsNull("SubClass") ? string.Empty : Convert.ToString(drow["SubClass"])
                                    });
                                }
                            }
                        }
                        if (resultdt.Tables[2] != null && resultdt.Tables[2].Rows.Count > 0)
                        {
                            foreach (var mp in motorProducts)
                            {
                                mp.MotorEndorsementMaster = new List<MotorEndorsementMaster>();
                                foreach (DataRow drow in resultdt.Tables[2].Rows)
                                {
                                    mp.MotorEndorsementMaster.Add(new MotorEndorsementMaster
                                    {
                                        EndorsementType = drow.IsNull("EndorsementType") ? string.Empty : Convert.ToString(drow["EndorsementType"]),
                                        ChargeAmount = drow.IsNull("ChargeAmount") ? 0 : Convert.ToDecimal(drow["ChargeAmount"]),
                                        EndorsementCode = drow.IsNull("EndorsementCode") ? string.Empty : Convert.ToString(drow["EndorsementCode"]),
                                        HasCommission = drow.IsNull("HasCommission") ? false : Convert.ToBoolean(drow["HasCommission"]),
                                    });
                                }
                            }
                        }
                    }
                    else if (request.Type == "HomeInsurance")
                    {
                        foreach (DataRow dr in resultdt.Tables[0].Rows)
                        {
                            var homeProduct = new HomeProduct();

                            homeProduct.Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]);
                            homeProduct.AgentCode = dr.IsNull("AgentCode") ? string.Empty : Convert.ToString(dr["AgentCode"]);
                            homeProduct.Description = dr.IsNull("Description") ? string.Empty : Convert.ToString(dr["Description"]);
                            homeProduct.MinimumPremium = dr.IsNull("MinimumPremium") ? decimal.Zero : Convert.ToDecimal(dr["MinimumPremium"]);
                            homeProduct.MainClass = dr.IsNull("MainClass") ? string.Empty : Convert.ToString(dr["MainClass"]);
                            homeProduct.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                            homeProduct.Rate = dr.IsNull("Rate") ? decimal.Zero : Convert.ToDecimal(dr["Rate"]);
                            homeProduct.DomesticHelperAmount = dr.IsNull("DomesticHelperAmount") ? decimal.Zero : Convert.ToDecimal(dr["DomesticHelperAmount"]);
                            homeProduct.MaximumBuildingValue = dr.IsNull("MaximumBuildingValue") ? decimal.Zero : Convert.ToDecimal(dr["MaximumBuildingValue"]);
                            homeProduct.MaximumContentValue = dr.IsNull("MaximumContentValue") ? decimal.Zero : Convert.ToDecimal(dr["MaximumContentValue"]);
                            homeProduct.MaximumHomeAge = dr.IsNull("MaximumHomeAge") ? 0 : Convert.ToInt32(dr["MaximumHomeAge"]);
                            homeProduct.RiotCoverMinAmount = dr.IsNull("RiotCoverMinAmount") ? 0 : Convert.ToDecimal(dr["RiotCoverMinAmount"]);
                            homeProduct.RiotCoverRate = dr.IsNull("RiotCoverRate") ? 0 : Convert.ToDecimal(dr["RiotCoverRate"]);
                            homeProduct.MaximumTotalValue = dr.IsNull("MaximumTotalValue") ? 0 : Convert.ToDecimal(dr["MaximumTotalValue"]);

                            homeProducts.Add(homeProduct);
                        }
                        if (resultdt.Tables[1] != null && resultdt.Tables[1].Rows.Count > 0)
                        {
                            foreach (var hp in homeProducts)
                            {
                                hp.HomeEndorsementMaster = new List<HomeEndorsementMaster>();
                                foreach (DataRow drow in resultdt.Tables[1].Rows)
                                {
                                    hp.HomeEndorsementMaster.Add(new HomeEndorsementMaster
                                    {
                                        EndorsementType = drow.IsNull("EndorsementType") ? string.Empty : Convert.ToString(drow["EndorsementType"]),
                                        ChargeAmount = drow.IsNull("ChargeAmount") ? 0 : Convert.ToDecimal(drow["ChargeAmount"]),
                                        EndorsementCode = drow.IsNull("EndorsementCode") ? string.Empty : Convert.ToString(drow["EndorsementCode"]),
                                        HasCommission = drow.IsNull("HasCommission") ? false : Convert.ToBoolean(drow["HasCommission"]),
                                    });
                                }
                            }

                        }
                    }
                }
                return new AgencyProductResponse
                {
                    HomeProducts = homeProducts,
                    MotorProducts = motorProducts,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new AgencyProductResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MotorCoverResponse GetProductCover(MotorCoverRequest request)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency", request.Agency??string.Empty),
                    new SqlParameter("@AgentCode",request.AgentCode??string.Empty),
                    new SqlParameter("@MainClass",request.MainClass??string.Empty),
                    new SqlParameter("@SubClass", request.SubClass ?? string.Empty),
                   // new SqlParameter("@Type", request.Type?? string.Empty),
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.GetMotorProductCover, para);
                List<MotorCovers> motorCovers = new List<MotorCovers>();
                if (resultdt != null && resultdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        var motorCover = new MotorCovers();
                        motorCover.CoverCode = dr.IsNull("CoverCode") ? string.Empty : Convert.ToString(dr["CoverCode"]);
                        motorCover.CoverDescription = dr.IsNull("CoverCodeDescription") ? string.Empty : Convert.ToString(dr["CoverCodeDescription"]);
                        motorCover.CoverAmount = dr.IsNull("CoverAmount") ? decimal.Zero : Convert.ToDecimal(dr["CoverAmount"]);
                        motorCover.IsOptional = dr.IsNull("IsOptionalCover") ? false : Convert.ToBoolean(dr["IsOptionalCover"]);
                        motorCover.ID = dr.IsNull("ID") ? 0 : Convert.ToInt32(dr["ID"]);

                        motorCovers.Add(motorCover);
                    }
                }
                return new MotorCoverResponse
                {
                    IsTransactionDone = true,
                    Covers = motorCovers
                };
            }
            catch (Exception ex)
            {
                return new MotorCoverResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public MotorProductMasterResponse MotorProductMasterOperation(MotorProductMaster details)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@ID", details.ID),
                    new SqlParameter("@Agency", details.Agency??string.Empty),
                    new SqlParameter("@AgentCode",details.AgentCode??string.Empty),
                    new SqlParameter("@MainClass",details.MainClass??string.Empty),
                    new SqlParameter("@SubClass", details.SubClass ?? string.Empty),
                    new SqlParameter("@Description", details.Description ?? string.Empty),
                    new SqlParameter("@MinimumPremium", details.MinimumPremium),
                    new SqlParameter("@Rate", details.Rate),
                    new SqlParameter("@AllowUnderAge", details.AllowUnderAge),
                    new SqlParameter("@UnderAge", details.UnderAge),
                    new SqlParameter("@HasAgeLoading", details.HasAgeLoading),
                    new SqlParameter("@HasAdditionalDays", details.HasAdditionalDays),
                    new SqlParameter("@UnderAgeminPremium", details.UnderAgeminPremium),
                    new SqlParameter("@AllowMaxVehicleAge", details.AllowMaxVehicleAge),
                    new SqlParameter("@MaximumVehicleAge", details.MaximumVehicleAge),
                    new SqlParameter("@MaximumVehicleValue", details.MaximumVehicleValue),
                    new SqlParameter("@GCCCoverRangeInYears", details.GCCCoverRangeInYears),
                    new SqlParameter("@HasGCC", details.HasGCC),
                    new SqlParameter("@IsProductSport", details.IsProductSport),
                    new SqlParameter("@PolicyCode", details.PolicyCode ?? string.Empty),
                    new SqlParameter("@ExcessAmount", details.ExcessAmount),
                    new SqlParameter("@UnderAgeExcessAmount", details.UnderAgeExcessAmount),
                    new SqlParameter("@AgeLoadingPercent", details.AgeLoadingPercent),
                    new SqlParameter("@UnderAgeToHIR", details.UnderAgeToHIR),
                    new SqlParameter("@LastSeries", details.LastSeries),
                    new SqlParameter("@SeriesFormatLength", details.SeriesFormatLength),
                    new SqlParameter("@AllowUsedVehicle", details.AllowUsedVehicle),
                    new SqlParameter("@GulfAssitAmount", details.GulfAssitAmount),
                    new SqlParameter("@Type", details.Type)
                };
                DataTable resultdt = BKICSQL.edt(StoredProcedures.AdminSP.MotorProductMaster, para);
                List<MotorProductMaster> motorProducts = new List<MotorProductMaster>();
                if (resultdt != null && resultdt.Rows.Count > 0)
                {
                    foreach (DataRow dr in resultdt.Rows)
                    {
                        var motorProductMaster = new MotorProductMaster();
                        motorProductMaster.Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]);
                        motorProductMaster.AgentCode = dr.IsNull("AgentCode") ? string.Empty : Convert.ToString(dr["AgentCode"]);
                        motorProductMaster.MainClass = dr.IsNull("MainClass") ? string.Empty : Convert.ToString(dr["MainClass"]);
                        motorProductMaster.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                        motorProductMaster.AgeLoadingPercent = dr.IsNull("AgeLoadingPercent") ? decimal.Zero : Convert.ToDecimal(dr["AgeLoadingPercent"]);
                        motorProductMaster.AllowMaxVehicleAge = dr.IsNull("AllowMaxVehicleAge") ? false : Convert.ToBoolean(dr["AllowMaxVehicleAge"]);
                        motorProductMaster.AllowUnderAge = dr.IsNull("AllowUnderAge") ? false : Convert.ToBoolean(dr["AllowUnderAge"]);
                        motorProductMaster.Description = dr.IsNull("Description") ? string.Empty : Convert.ToString(dr["Description"]);
                        motorProductMaster.ExcessAmount = dr.IsNull("ExcessAmount") ? decimal.Zero : Convert.ToDecimal(dr["ExcessAmount"]);
                        motorProductMaster.GCCCoverRangeInYears = dr.IsNull("GCCCoverRangeInYears") ? 0 : Convert.ToInt32(dr["GCCCoverRangeInYears"]);
                        motorProductMaster.HasAdditionalDays = dr.IsNull("HasAdditionalDays") ? false : Convert.ToBoolean(dr["HasAdditionalDays"]);
                        motorProductMaster.HasAgeLoading = dr.IsNull("HasAgeLoading") ? false : Convert.ToBoolean(dr["HasAgeLoading"]);
                        motorProductMaster.HasGCC = dr.IsNull("HasGCC") ? false : Convert.ToBoolean(dr["HasGCC"]);
                        motorProductMaster.ID = dr.IsNull("ID") ? 0 : Convert.ToInt32(dr["ID"]);
                        motorProductMaster.IsProductSport = dr.IsNull("IsProductSport") ? false : Convert.ToBoolean(dr["IsProductSport"]);
                        motorProductMaster.LastSeries = dr.IsNull("LastSeries") ? 0 : Convert.ToInt64(dr["LastSeries"]);
                        motorProductMaster.MaximumVehicleAge = dr.IsNull("MaximumVehicleAge") ? 0 : Convert.ToInt32(dr["MaximumVehicleAge"]);
                        motorProductMaster.MaximumVehicleValue = dr.IsNull("MaxVehicleValue") ? decimal.Zero : Convert.ToDecimal(dr["MaxVehicleValue"]);
                        motorProductMaster.MinimumPremium = dr.IsNull("MinimumPermium") ? decimal.Zero : Convert.ToDecimal(dr["MinimumPermium"]);
                        motorProductMaster.PolicyCode = dr.IsNull("PolicyCode") ? string.Empty : Convert.ToString(dr["PolicyCode"]);
                        motorProductMaster.Rate = dr.IsNull("Rate") ? decimal.Zero : Convert.ToDecimal(dr["Rate"]);
                        motorProductMaster.SeriesFormatLength = dr.IsNull("SeriesFormatLength") ? 0 : Convert.ToInt32(dr["SeriesFormatLength"]);
                        motorProductMaster.UnderAge = dr.IsNull("UnderAge") ? 0 : Convert.ToInt32(dr["UnderAge"]);
                        motorProductMaster.UnderAgeExcessAmount = dr.IsNull("UnderAgeExcessAmount") ? decimal.Zero : Convert.ToDecimal(dr["UnderAgeExcessAmount"]);
                        motorProductMaster.UnderAgeminPremium = dr.IsNull("UnderAgeminPremium") ? decimal.Zero : Convert.ToDecimal(dr["UnderAgeminPremium"]);
                        motorProductMaster.UnderAgeToHIR = dr.IsNull("UnderAgeToHIR") ? false : Convert.ToBoolean(dr["UnderAgeToHIR"]);
                        motorProductMaster.GulfAssitAmount = dr.IsNull("GulfAssitAmount") ? 0 : Convert.ToDecimal(dr["GulfAssitAmount"]);
                        motorProductMaster.AllowUsedVehicle = dr.IsNull("AllowUsedVehicle") ? false : Convert.ToBoolean(dr["AllowUsedVehicle"]);

                        motorProducts.Add(motorProductMaster);
                    }
                }
                return new MotorProductMasterResponse
                {
                    IsTransactionDone = true,
                    motorProductMaster = motorProducts
                };
            }
            catch (Exception ex)
            {
                return new MotorProductMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public MotorProductMasterResponse GetMotorProduct(MotorProductRequest details)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency", details.Agency??string.Empty),
                    new SqlParameter("@AgentCode",details.AgentCode??string.Empty),
                    new SqlParameter("@MainClass",details.MainClass??string.Empty),
                    new SqlParameter("@SubClass", details.SubClass ?? string.Empty),
                    new SqlParameter("@Type", details.Type ?? string.Empty)
                };
                List<MotorProductMaster> motorProductMasterList = new List<MotorProductMaster>();
                DataSet resultdt = BKICSQL.eds(StoredProcedures.AdminSP.GetMotorProduct, para);
                if (resultdt != null && resultdt.Tables[0] != null)
                {
                    var motorProductMaster = new MotorProductMaster();

                    var dr = resultdt.Tables[0].Rows[0];
                    motorProductMaster.Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]);
                    motorProductMaster.AgentCode = dr.IsNull("AgentCode") ? string.Empty : Convert.ToString(dr["AgentCode"]);
                    motorProductMaster.MainClass = dr.IsNull("MainClass") ? string.Empty : Convert.ToString(dr["MainClass"]);
                    motorProductMaster.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                    motorProductMaster.AgeLoadingPercent = dr.IsNull("AgeLoadingPercent") ? decimal.Zero : Convert.ToDecimal(dr["AgeLoadingPercent"]);
                    motorProductMaster.AllowMaxVehicleAge = dr.IsNull("AllowMaxVehicleAge") ? false : Convert.ToBoolean(dr["AllowMaxVehicleAge"]);
                    motorProductMaster.AllowUnderAge = dr.IsNull("AllowUnderAge") ? false : Convert.ToBoolean(dr["AllowUnderAge"]);
                    motorProductMaster.Description = dr.IsNull("Description") ? string.Empty : Convert.ToString(dr["Description"]);
                    motorProductMaster.ExcessAmount = dr.IsNull("ExcessAmount") ? decimal.Zero : Convert.ToDecimal(dr["ExcessAmount"]);
                    motorProductMaster.GCCCoverRangeInYears = dr.IsNull("GCCCoverRangeInYears") ? 0 : Convert.ToInt32(dr["GCCCoverRangeInYears"]);
                    motorProductMaster.HasAdditionalDays = dr.IsNull("HasAdditionalDays") ? false : Convert.ToBoolean(dr["HasAdditionalDays"]);
                    motorProductMaster.HasAgeLoading = dr.IsNull("HasAgeLoading") ? false : Convert.ToBoolean(dr["HasAgeLoading"]);
                    motorProductMaster.HasGCC = dr.IsNull("HasGCC") ? false : Convert.ToBoolean(dr["HasGCC"]);
                    motorProductMaster.ID = dr.IsNull("ID") ? 0 : Convert.ToInt32(dr["ID"]);
                    motorProductMaster.IsProductSport = dr.IsNull("IsProductSport") ? false : Convert.ToBoolean(dr["IsProductSport"]);
                    motorProductMaster.LastSeries = dr.IsNull("LastSeries") ? 0 : Convert.ToInt64(dr["LastSeries"]);
                    motorProductMaster.MaximumVehicleAge = dr.IsNull("MaximumVehicleAge") ? 0 : Convert.ToInt32(dr["MaximumVehicleAge"]);
                    motorProductMaster.MaximumVehicleValue = dr.IsNull("MaxVehicleValue") ? decimal.Zero : Convert.ToDecimal(dr["MaxVehicleValue"]);
                    motorProductMaster.MinimumPremium = dr.IsNull("MinimumPermium") ? decimal.Zero : Convert.ToDecimal(dr["MinimumPermium"]);
                    motorProductMaster.PolicyCode = dr.IsNull("PolicyCode") ? string.Empty : Convert.ToString(dr["PolicyCode"]);
                    motorProductMaster.Rate = dr.IsNull("Rate") ? decimal.Zero : Convert.ToDecimal(dr["Rate"]);
                    motorProductMaster.SeriesFormatLength = dr.IsNull("SeriesFormatLength") ? 0 : Convert.ToInt32(dr["SeriesFormatLength"]);
                    motorProductMaster.UnderAge = dr.IsNull("UnderAge") ? 0 : Convert.ToInt32(dr["UnderAge"]);
                    motorProductMaster.UnderAgeExcessAmount = dr.IsNull("UnderAgeExcessAmount") ? decimal.Zero : Convert.ToDecimal(dr["UnderAgeExcessAmount"]);
                    motorProductMaster.UnderAgeminPremium = dr.IsNull("UnderAgeMinimumPremium") ? decimal.Zero : Convert.ToDecimal(dr["UnderAgeMinimumPremium"]);
                    motorProductMaster.UnderAgeToHIR = dr.IsNull("UnderAgeToHIR") ? false : Convert.ToBoolean(dr["UnderAgeToHIR"]);
                    motorProductMaster.UnderAgeRate = dr.IsNull("UnderAgeRate") ? decimal.Zero : Convert.ToDecimal(dr["UnderAgeRate"]);

                    if (resultdt != null && resultdt.Tables[1] != null)
                    {
                        motorProductMaster.Category = new List<CategoryMaster>();
                        foreach (DataRow drow in resultdt.Tables[1].Rows)
                        {
                            motorProductMaster.Category.Add(
                                new CategoryMaster
                                {
                                    Agency = drow.IsNull("Agency") ? string.Empty : Convert.ToString(drow["Agency"]),
                                    AgentCode = drow.IsNull("AgentCode") ? string.Empty : Convert.ToString(drow["AgentCode"]),
                                    MainClass = drow.IsNull("MainClass") ? string.Empty : Convert.ToString(drow["MainClass"]),
                                    SubClass = drow.IsNull("SubClass") ? string.Empty : Convert.ToString(drow["SubClass"]),
                                    Code = drow.IsNull("Code") ? string.Empty : Convert.ToString(drow["Code"]),
                                    Value = drow.IsNull("Value") ? decimal.Zero : Convert.ToDecimal(drow["Value"]),
                                    Category = drow.IsNull("Category") ? string.Empty : Convert.ToString(drow["Category"]),
                                    ValueType = drow.IsNull("ValueType") ? string.Empty : Convert.ToString(drow["ValueType"]),
                                    IsDeductable = drow.IsNull("IsDeductable") ? false : Convert.ToBoolean(drow["IsDeductable"])
                                }
                            );
                        }
                    }
                    if (resultdt != null && resultdt.Tables[2] != null && resultdt.Tables[2].Rows.Count > 0)
                    {
                        DataRow row = (DataRow)resultdt.Tables[2].Rows[0];
                        motorProductMaster.TaxRate = row.IsNull("Rate") ? 0 : Convert.ToDecimal(row["Rate"]);
                    }
                    if (resultdt != null && resultdt.Tables[3] != null)
                    {
                        motorProductMaster.MotorOptionalBenefits = new List<MotorOptionalBenefit>();
                        foreach (DataRow drow in resultdt.Tables[3].Rows)
                        {
                            motorProductMaster.MotorOptionalBenefits.Add(
                                new MotorOptionalBenefit
                                {
                                    ID = drow.IsNull("ID") ? 0 : Convert.ToInt32(drow["ID"]),
                                    Value = drow.IsNull("Value") ? string.Empty : Convert.ToString(drow["Value"]),
                                    Text = drow.IsNull("Text") ? string.Empty : Convert.ToString(drow["Text"]),
                                    Percentage = drow.IsNull("Percentage") ? decimal.Zero : Convert.ToDecimal(drow["Percentage"])

                                });
                        }
                    }
                    if (resultdt != null && resultdt.Tables[4] != null && resultdt.Tables[4].Rows.Count > 0)
                    {
                        motorProductMaster.MotorClaim = new List<MotorClaim>();
                        foreach (DataRow drow1 in resultdt.Tables[4].Rows)
                        {
                            motorProductMaster.MotorClaim.Add(
                                new MotorClaim
                                {
                                    AmountFrom = drow1.IsNull("AmountFrom") ? 0 : Convert.ToDecimal(drow1["AmountFrom"]),
                                    AmountTo = drow1.IsNull("AmountTo") ? 0 : Convert.ToDecimal(drow1["AmountTo"]),
                                    Percentage = drow1.IsNull("Percentage") ? 0 : Convert.ToDecimal(drow1["Percentage"]),
                                    MaximumClaimAmount = drow1.IsNull("MaxClaimAmount") ? decimal.Zero : Convert.ToDecimal(drow1["MaxClaimAmount"]),
                                    MainClass = drow1.IsNull("MainClass") ? string.Empty : Convert.ToString(drow1["MainClass"]),
                                    SubClass = drow1.IsNull("SubClass") ? string.Empty : Convert.ToString(drow1["SubClass"])
                                });
                        }
                    }
                    if (resultdt != null && resultdt.Tables[5] != null && resultdt.Tables[5].Rows.Count > 0)
                    {
                        motorProductMaster.MotorEndorsementMaster = new List<MotorEndorsementMaster>();
                        foreach (DataRow drow in resultdt.Tables[5].Rows)
                        {
                            motorProductMaster.MotorEndorsementMaster.Add(new MotorEndorsementMaster
                            {
                                EndorsementType = drow.IsNull("EndorsementType") ? string.Empty : Convert.ToString(drow["EndorsementType"]),
                                ChargeAmount = drow.IsNull("ChargeAmount") ? 0 : Convert.ToDecimal(drow["ChargeAmount"]),
                                EndorsementCode = drow.IsNull("EndorsementCode") ? string.Empty : Convert.ToString(drow["EndorsementCode"]),
                                HasCommission = drow.IsNull("HasCommission") ? false : Convert.ToBoolean(drow["HasCommission"]),
                            });
                        }
                    }
                    motorProductMasterList.Add(motorProductMaster);
                }

                return new MotorProductMasterResponse
                {
                    IsTransactionDone = true,
                    motorProductMaster = motorProductMasterList
                };
            }
            catch (Exception ex)
            {
                return new MotorProductMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public HomeProductResponse GetHomeProduct(HomeProductRequest details)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[]
                {
                    new SqlParameter("@Agency", details.Agency??string.Empty),
                    new SqlParameter("@AgentCode",details.AgentCode??string.Empty),
                    new SqlParameter("@MainClass",details.MainClass??string.Empty),
                    new SqlParameter("@SubClass", details.SubClass ?? string.Empty),
                    new SqlParameter("@Type", details.Type ?? string.Empty)
                };
                List<HomeProduct> homeProducts = new List<HomeProduct>();
                DataSet resultdt = BKICSQL.eds(StoredProcedures.AdminSP.GetHomeProduct, para);
                if (resultdt != null && resultdt.Tables[0] != null && resultdt.Tables[0].Rows.Count > 0)
                {
                    var homeProduct = new HomeProduct();
                    var dr = resultdt.Tables[0].Rows[0];

                    homeProduct.Agency = dr.IsNull("Agency") ? string.Empty : Convert.ToString(dr["Agency"]);
                    homeProduct.AgentCode = dr.IsNull("AgentCode") ? string.Empty : Convert.ToString(dr["AgentCode"]);
                    homeProduct.MinimumPremium = dr.IsNull("MinimumPremiumAmount") ? decimal.Zero : Convert.ToDecimal(dr["MinimumPremiumAmount"]);
                    homeProduct.MainClass = dr.IsNull("MainClass") ? string.Empty : Convert.ToString(dr["MainClass"]);
                    homeProduct.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                    homeProduct.Rate = dr.IsNull("BuildingRate") ? decimal.Zero : Convert.ToDecimal(dr["BuildingRate"]);
                    homeProduct.DomesticHelperAmount = dr.IsNull("DomesticHelperAmount") ? decimal.Zero : Convert.ToDecimal(dr["DomesticHelperAmount"]);
                    homeProduct.MaximumBuildingValue = dr.IsNull("MaxBuildingValue") ? decimal.Zero : Convert.ToDecimal(dr["MaxBuildingValue"]);
                    homeProduct.MaximumContentValue = dr.IsNull("MaxContentValue") ? decimal.Zero : Convert.ToDecimal(dr["MaxContentValue"]);
                    homeProduct.MaximumHomeAge = dr.IsNull("MaximumHomeAge") ? 0 : Convert.ToInt32(dr["MaximumHomeAge"]);
                    homeProduct.RiotCoverMinAmount = dr.IsNull("RiotCoverMinimum") ? 0 : Convert.ToInt32(dr["RiotCoverMinimum"]);
                    homeProduct.RiotCoverRate = dr.IsNull("RiotCover") ? 0 : Convert.ToDecimal(dr["RiotCover"]);
                    homeProduct.MaximumJewelleryValue = dr.IsNull("MaxJewelleryValue") ? 0 : Convert.ToDecimal(dr["MaxJewelleryValue"]);
                    homeProduct.MaximumTotalValue = dr.IsNull("MaximumTotalValue") ? 0 : Convert.ToDecimal(dr["MaximumTotalValue"]);

                    homeProducts.Add(homeProduct);
                }
                if (resultdt != null && resultdt.Tables[1] != null)
                {
                    homeProducts[0].Category = new List<CategoryMaster>();
                    foreach (DataRow drow in resultdt.Tables[1].Rows)
                    {
                        homeProducts[0].Category.Add(
                                new CategoryMaster
                                {
                                    Agency = drow.IsNull("Agency") ? string.Empty : Convert.ToString(drow["Agency"]),
                                    AgentCode = drow.IsNull("AgentCode") ? string.Empty : Convert.ToString(drow["AgentCode"]),
                                    MainClass = drow.IsNull("MainClass") ? string.Empty : Convert.ToString(drow["MainClass"]),
                                    SubClass = drow.IsNull("SubClass") ? string.Empty : Convert.ToString(drow["SubClass"]),
                                    Code = drow.IsNull("Code") ? string.Empty : Convert.ToString(drow["Code"]),
                                    Value = drow.IsNull("Value") ? decimal.Zero : Convert.ToDecimal(drow["Value"]),
                                    Category = drow.IsNull("Category") ? string.Empty : Convert.ToString(drow["Category"]),
                                    ValueType = drow.IsNull("ValueType") ? string.Empty : Convert.ToString(drow["ValueType"]),
                                    IsDeductable = drow.IsNull("IsDeductable") ? false : Convert.ToBoolean(drow["IsDeductable"])
                                }
                            );
                    }
                }
                if (resultdt != null && resultdt.Tables[2] != null && resultdt.Tables[2].Rows.Count > 0)
                {
                    DataRow row = (DataRow)resultdt.Tables[2].Rows[0];
                    homeProducts[0].TaxRate = row.IsNull("Rate") ? 0 : Convert.ToDecimal(row["Rate"]);
                }
                if (resultdt != null && resultdt.Tables[3] != null && resultdt.Tables[3].Rows.Count > 0)
                {
                    homeProducts[0].JewelleryCover = new List<JewelleryCover>();
                    foreach (DataRow drow in resultdt.Tables[3].Rows)
                    {
                        homeProducts[0].JewelleryCover.Add(
                        new JewelleryCover
                        {
                            Rate = drow.IsNull("Rate") ? decimal.Zero : Convert.ToDecimal(drow["Rate"]),
                            Amount = drow.IsNull("Amount") ? decimal.Zero : Convert.ToDecimal(drow["Amount"]),
                            KeyType = drow.IsNull("KeyType") ? string.Empty : Convert.ToString(drow["KeyType"]),
                            ValueType = drow.IsNull("ValueType") ? string.Empty : Convert.ToString(drow["ValueType"])
                        });
                    }
                }
                if (resultdt != null && resultdt.Tables[4] != null && resultdt.Tables[4].Rows.Count > 0)
                {
                    homeProducts[0].HomeEndorsementMaster = new List<HomeEndorsementMaster>();
                    foreach (DataRow drow in resultdt.Tables[4].Rows)
                    {
                        homeProducts[0].HomeEndorsementMaster.Add(
                        new HomeEndorsementMaster
                        {
                            EndorsementType = drow.IsNull("EndorsementType") ? string.Empty : Convert.ToString(drow["EndorsementType"]),
                            ChargeAmount = drow.IsNull("ChargeAmount") ? 0 : Convert.ToDecimal(drow["ChargeAmount"]),
                            EndorsementCode = drow.IsNull("EndorsementCode") ? string.Empty : Convert.ToString(drow["EndorsementCode"]),
                            HasCommission = drow.IsNull("HasCommission") ? false : Convert.ToBoolean(drow["HasCommission"]),
                        });
                    }
                }
                return new HomeProductResponse
                {
                    IsTransactionDone = true,
                    HomeProducts = homeProducts
                };
            }
            catch (Exception ex)
            {
                return new HomeProductResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public MotorYearMasterResponse MotorYearMasterOperation(MotorYearMaster details)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Id",details.ID),
                   new SqlParameter("@Year", details.Year),
                   new SqlParameter("@Type",details.Type?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(AdminSP.MotorYearMaster, paras);
                var ListYears = new List<MotorYearMaster>();
                if (details.Type == "fetch")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListYears.Add(
                                    new MotorYearMaster
                                    {
                                        ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString()),
                                        Year = Convert.ToInt32(dt.Rows[i]["Year"].ToString())
                                    });
                        }
                    }
                }
                return new MotorYearMasterResponse
                {
                    IsTransactionDone = true,
                    MotorYears = ListYears
                };
            }
            catch (Exception ex)
            {
                return new MotorYearMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public MotorEngineCCResponse MotorEngineCCOperation(MotorEngineCCMaster details)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Id",details.ID),
                   new SqlParameter("@Tonnage",details.Tonnage),
                   new SqlParameter("@Capacity",details.Capacity ?? string.Empty),
                   new SqlParameter("@Type",details.Type ?? string.Empty)
                };
                DataTable dt = BKICSQL.edt(AdminSP.MotorEngineCCMaster, paras);
                var ListEngineCC = new List<MotorEngineCCMaster>();
                if (details.Type == "fetch")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ListEngineCC.Add(
                                    new MotorEngineCCMaster
                                    {
                                        ID = Convert.ToInt32(dt.Rows[i]["Id"].ToString()),
                                        Capacity = dt.Rows[i]["Capacity"].ToString(),
                                        Tonnage = Convert.ToInt32(dt.Rows[i]["Tonnage"].ToString())
                                    });
                        }
                    }
                }
                return new MotorEngineCCResponse
                {
                    IsTransactionDone = true,
                    MotorEngineCC = ListEngineCC
                };
            }
            catch (Exception ex)
            {
                return new MotorEngineCCResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        public RenewalPrecheckResponse RenewalPrecheck(RenewalPrecheckRequest request)
        {
            try
            {
                SqlParameter[] paras = new SqlParameter[]
                {
                   new SqlParameter("@Agency", request.Agency),
                   new SqlParameter("@AgentCode", request.AgentCode),
                   new SqlParameter("@InsuranceType", request.InsuranceType),
                   new SqlParameter("@DocumentNo", request.DocumentNo),
                   new SqlParameter("@CurrentRenewalCount", request.CurrentRenewalCount)
                };
                List<SPOut> outParams = new List<SPOut>()
                {
                    new SPOut() { OutPutType = SqlDbType.Bit, ParameterName= "@IsAlreadyRenewed"},

                };
                object[] dataSet = BKICSQL.GetValues(AdminSP.RenewalPrecheck, paras, outParams);
                if (dataSet != null && dataSet[0] != null)
                {
                    return new RenewalPrecheckResponse
                    {
                        IsAlreadyRenewed = Convert.ToBoolean(dataSet[0]),
                        IsTransactionDone = true
                    };
                }
                return new RenewalPrecheckResponse
                {
                    IsAlreadyRenewed = false,
                    IsTransactionDone = true
                };
            }
            catch (Exception ex)
            {
                return new RenewalPrecheckResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
    }
}
