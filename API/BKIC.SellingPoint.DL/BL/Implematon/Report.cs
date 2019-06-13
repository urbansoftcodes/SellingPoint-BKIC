using BKIC.SellingPoint.DL.BL.Implementation;
using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BKIC.SellingPoint.DL.BL.Implematon
{
    public class Report : IReport
    {
        /// <summary>
        /// Get motor report based on type(Report by age or branch or user)
        /// </summary>
        /// <param name="reportReq">Report request.</param>
        /// <returns>Report details.</returns>
        public MotorReportResponse GetMotorReport(AdminFetchReportRequest reportReq)
        {
            try
            {
                if (reportReq.InsuranceType == Constants.Insurance.Motor)
                {
                    if (reportReq.ReportType == Constants.ReportType.MotorAgeReport)
                    {
                        SqlParameter[] paras = new SqlParameter[]
                         {
                           new SqlParameter("@Agency",reportReq.Agency),
                           new SqlParameter("@AgentCode", reportReq.AgentCode),
                           new SqlParameter("@AgeFrom", reportReq.AgeFrom),
                           new SqlParameter("@AgeTo", reportReq.AgeTo),
                           new SqlParameter("@DateFrom", reportReq.DateFrom),
                           new SqlParameter("@DateTo", reportReq.DateTo)
                         };
                        DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetMotorAgeReport, paras);
                        List<MotorReportDetails> response = new List<MotorReportDetails>();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                var res = new MotorReportDetails();
                                res.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                                res.Age = dr.IsNull("Age") ? 0 : Convert.ToInt32(dr["Age"]);
                                res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                                res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                                res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                                res.VehicleType = dr.IsNull("VehicleType") ? string.Empty : Convert.ToString(dr["VehicleType"]);
                                res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                                res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                                res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                                res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                                res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                                res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["Vat"]);

                                response.Add(res);
                            }
                        }
                        return new MotorReportResponse
                        {
                            IsTransactionDone = true,
                            MotorReportDetails = response,
                        };
                    }
                    else if (reportReq.ReportType == Constants.ReportType.MotorBranchReport)
                    {
                        SqlParameter[] paras = new SqlParameter[]
                         {
                       new SqlParameter("@Agency",reportReq.Agency),
                       new SqlParameter("@AgentCode", reportReq.AgentCode),
                       new SqlParameter("@BranchCode", reportReq.BranchCode),
                       new SqlParameter("@DateFrom", reportReq.DateFrom),
                       new SqlParameter("@DateTo", reportReq.DateTo)
                         };
                        DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetMotorBranchReport, paras);
                        List<MotorReportDetails> response = new List<MotorReportDetails>();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                var res = new MotorReportDetails();
                                res.BranchCode = dr.IsNull("BranchCode") ? string.Empty : Convert.ToString(dr["BranchCode"]);
                                res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                                res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                                res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                                res.VehicleType = dr.IsNull("VehicleType") ? string.Empty : Convert.ToString(dr["VehicleType"]);
                                res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                                res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                                res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                                res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                                res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                                res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["Vat"]);

                                response.Add(res);
                            }
                        }
                        return new MotorReportResponse
                        {
                            IsTransactionDone = true,
                            MotorReportDetails = response,
                        };
                    }
                    else if (reportReq.ReportType == Constants.ReportType.MotorUserReport)
                    {
                        SqlParameter[] paras = new SqlParameter[]
                         {
                       new SqlParameter("@Agency",reportReq.Agency),
                       new SqlParameter("@AgentCode", reportReq.AgentCode),
                       new SqlParameter("@AuthorizedUserID", reportReq.AuthorizedUserID),
                       new SqlParameter("@DateFrom", reportReq.DateFrom),
                       new SqlParameter("@DateTo", reportReq.DateTo)
                         };
                        DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetMotorUserReport, paras);
                        List<MotorReportDetails> response = new List<MotorReportDetails>();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                var res = new MotorReportDetails();
                                res.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);
                                res.Age = dr.IsNull("Age") ? 0 : Convert.ToInt32(dr["Age"]);
                                res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                                res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                                res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                                res.VehicleType = dr.IsNull("VehicleType") ? string.Empty : Convert.ToString(dr["VehicleType"]);
                                res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                                res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                                res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                                res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                                res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                                res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["Vat"]);

                                response.Add(res);
                            }
                        }
                        return new MotorReportResponse
                        {
                            IsTransactionDone = true,
                            MotorReportDetails = response,
                        };
                    }
                    else if (reportReq.ReportType == Constants.ReportType.MotorVehicleReport)
                    {
                        SqlParameter[] paras = new SqlParameter[]
                         {
                       new SqlParameter("@Agency",reportReq.Agency),
                       new SqlParameter("@AgentCode", reportReq.AgentCode),
                       new SqlParameter("@VehicleMake", reportReq.VehicleMake),
                       new SqlParameter("@DateFrom", reportReq.DateFrom),
                       new SqlParameter("@DateTo", reportReq.DateTo)
                         };
                        DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetMotorVehicleReport, paras);
                        List<MotorReportDetails> response = new List<MotorReportDetails>();
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                var res = new MotorReportDetails();
                                res.VehicleMake = dr.IsNull("VehicleMake") ? string.Empty : Convert.ToString(dr["VehicleMake"]);
                                res.VehicleModel = dr.IsNull("VehicleModel") ? string.Empty : Convert.ToString(dr["VehicleModel"]);
                                res.Year = dr.IsNull("CPR") ? 0 : Convert.ToInt32(dr["CPR"]);
                                res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                                res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                                res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                                res.VehicleType = dr.IsNull("VehicleType") ? string.Empty : Convert.ToString(dr["VehicleType"]);
                                res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                                res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                                res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                                res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                                res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                                res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["Vat"]);

                                response.Add(res);
                            }
                        }
                        return new MotorReportResponse
                        {
                            IsTransactionDone = true,
                            MotorReportDetails = response,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new MotorReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message,
                };
            }
            return null;
        }

        /// <summary>
        /// Get travel report based on type(Report by branch or user)
        /// </summary>
        /// <param name="reportReq">Report request.</param>
        /// <returns>Report details.</returns>
        public TravelHomeReportResponse GetTravelReport(AdminFetchReportRequest reportReq)
        {
            try
            {
                if (reportReq.ReportType == Constants.ReportType.TravelUserReport)
                {
                    SqlParameter[] paras = new SqlParameter[]
                     {
                       new SqlParameter("@Agency",reportReq.Agency),
                       new SqlParameter("@AgentCode", reportReq.AgentCode),
                       new SqlParameter("@AuthorizedUserID", reportReq.AuthorizedUserID),
                       new SqlParameter("@DateFrom", reportReq.DateFrom),
                       new SqlParameter("@DateTo", reportReq.DateTo)
                     };
                    DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetTravelUserReport, paras);
                    List<TravelHomeReportDetails> response = new List<TravelHomeReportDetails>();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var res = new TravelHomeReportDetails();
                            res.AuthorizedCode = dr.IsNull("AuthorizedCode") ? string.Empty : Convert.ToString(dr["AuthorizedCode"]);
                            res.HandledBy = dr.IsNull("HandledBy") ? string.Empty : Convert.ToString(dr["HandledBy"]);
                            res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                            res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                            res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                            res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                            res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                            res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                            res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                            res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                            response.Add(res);
                        }
                    }
                    return new TravelHomeReportResponse
                    {
                        IsTransactionDone = true,
                        TravelHomeReportDetails = response
                    };
                }
                else if (reportReq.ReportType == Constants.ReportType.TravelBranchReport)
                {
                    SqlParameter[] paras = new SqlParameter[]
                     {
                       new SqlParameter("@Agency",reportReq.Agency),
                       new SqlParameter("@AgentCode", reportReq.AgentCode),
                       new SqlParameter("@BranchCode", reportReq.BranchCode),
                       new SqlParameter("@DateFrom", reportReq.DateFrom),
                       new SqlParameter("@DateTo", reportReq.DateTo)
                     };
                    DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetTravelBranchReport, paras);
                    List<TravelHomeReportDetails> response = new List<TravelHomeReportDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var res = new TravelHomeReportDetails();
                            res.BranchCode = dr.IsNull("BranchCode") ? string.Empty : Convert.ToString(dr["BranchCode"]);
                            res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                            res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                            res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                            res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                            res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                            res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                            res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                            res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                            response.Add(res);
                        }
                    }
                    return new TravelHomeReportResponse
                    {
                        IsTransactionDone = true,
                        TravelHomeReportDetails = response
                    };
                }
            }
            catch (Exception ex)
            {
                return new TravelHomeReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message,
                };
            }
            return null;
        }

        /// <summary>
        /// Get home report based on type(Report by branch or user)
        /// </summary>
        /// <param name="reportReq">Report request.</param>
        /// <returns>Report details.</returns>
        public TravelHomeReportResponse GetHomeReport(AdminFetchReportRequest reportReq)
        {
            try
            {
                if (reportReq.ReportType == Constants.ReportType.HomeUserReport)
                {
                    SqlParameter[] paras = new SqlParameter[]
                     {
                       new SqlParameter("@Agency",reportReq.Agency),
                       new SqlParameter("@AgentCode", reportReq.AgentCode),
                       new SqlParameter("@AuthorizedUserID", reportReq.AuthorizedUserID),
                       new SqlParameter("@DateFrom", reportReq.DateFrom),
                       new SqlParameter("@DateTo", reportReq.DateTo)
                     };
                    DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetHomeUserReport, paras);
                    List<TravelHomeReportDetails> response = new List<TravelHomeReportDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var res = new TravelHomeReportDetails();

                            res.AuthorizedCode = dr.IsNull("AuthorizedCode") ? string.Empty : Convert.ToString(dr["AuthorizedCode"]);
                            res.HandledBy = dr.IsNull("HandledBy") ? string.Empty : Convert.ToString(dr["HandledBy"]);
                            res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                            res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                            res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                            res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                            res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                            res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                            res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                            res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                            res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["vat"]);
                            response.Add(res);
                        }
                    }
                    return new TravelHomeReportResponse
                    {
                        IsTransactionDone = true,
                        TravelHomeReportDetails = response
                    };
                }
                else if (reportReq.ReportType == Constants.ReportType.HomeBranchReport)
                {
                    SqlParameter[] paras = new SqlParameter[]
                     {
                       new SqlParameter("@Agency",reportReq.Agency),
                       new SqlParameter("@AgentCode", reportReq.AgentCode),
                       new SqlParameter("@BranchCode", reportReq.BranchCode),
                       new SqlParameter("@DateFrom", reportReq.DateFrom),
                       new SqlParameter("@DateTo", reportReq.DateTo)
                     };
                    DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetHomeBranchReport, paras);
                    List<TravelHomeReportDetails> response = new List<TravelHomeReportDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var res = new TravelHomeReportDetails();
                            res.BranchCode = dr.IsNull("BranchCode") ? string.Empty : Convert.ToString(dr["BranchCode"]);
                            res.PolicyNo = dr.IsNull("Policy No") ? string.Empty : Convert.ToString(dr["Policy No"]);
                            res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                            res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);
                            res.SumInsured = dr.IsNull("SumInsured") ? decimal.Zero : Convert.ToDecimal(dr["SumInsured"]);
                            res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                            res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                            res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                            res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                            res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["vat"]);

                            response.Add(res);
                        }
                    }
                    return new TravelHomeReportResponse
                    {
                        IsTransactionDone = true,
                        TravelHomeReportDetails = response
                    };
                }
            }
            catch (Exception ex)
            {
                return new TravelHomeReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message,
                };
            }
            return null;
        }

        /// <summary>
        /// Get main report based on insurance type(Motor or Travel or Home)
        /// </summary>
        /// <param name="reportReq">Report request.</param>
        /// <returns>Report details.</returns>
        public MainReportResponse GetMainReport(AdminFetchReportRequest reportReq)
        {
            try
            {
                if (reportReq.InsuranceType == Constants.Insurance.Motor)
                {
                    if (reportReq.ReportType == Constants.ReportType.MotorMainReport)
                    {
                        SqlParameter[] paras = new SqlParameter[]
                         {
                               new SqlParameter("@Agency",reportReq.Agency),
                               new SqlParameter("@AgentCode", reportReq.AgentCode),
                               new SqlParameter("@DateFrom", reportReq.DateFrom),
                               new SqlParameter("@DateTo", reportReq.DateTo)
                         };
                        DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetMotorMainReport, paras);
                        List<MainReportDetails> response = new List<MainReportDetails>();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                var res = new MainReportDetails();
                                res.BranchCode = dr.IsNull("BranchCode") ? string.Empty : Convert.ToString(dr["BranchCode"]);
                                res.BranchName = dr.IsNull("BranchName") ? string.Empty : Convert.ToString(dr["BranchName"]);
                                res.InsuredName = dr.IsNull("InsuredName") ? string.Empty : Convert.ToString(dr["InsuredName"]);
                                res.AuthorizedUser = dr.IsNull("AuthorizedBy") ? string.Empty : Convert.ToString(dr["AuthorizedBy"]);
                                res.AuthorizedDate = dr.IsNull("AuthorizedDate") ? string.Empty : Convert.ToDateTime(dr["AuthorizedDate"]).Date.CovertToLocalFormat();
                                res.HandledBy = dr.IsNull("HandledBy") ? string.Empty : Convert.ToString(dr["HandledBy"]);
                                res.CommenceDate = dr.IsNull("CommenceDate") ? string.Empty : Convert.ToDateTime(dr["CommenceDate"]).Date.CovertToLocalFormat();
                                res.ExpiryDate = dr.IsNull("ExpiryDate") ? string.Empty : Convert.ToDateTime(dr["ExpiryDate"]).Date.CovertToLocalFormat();
                                res.PaymentMethod = dr.IsNull("PaymentMethod") ? string.Empty : Convert.ToString(dr["PaymentMethod"]);
                                res.PolicyNo = dr.IsNull("PolicyNo") ? string.Empty : Convert.ToString(dr["PolicyNo"]);
                                res.EndorsementNo = dr.IsNull("EndorsementNo") ? string.Empty : Convert.ToString(dr["EndorsementNo"]);
                                res.SubClass = dr.IsNull("SubClass") ? string.Empty : Convert.ToString(dr["SubClass"]);

                                res.Commission = dr.IsNull("Commission") ? decimal.Zero : Convert.ToDecimal(dr["Commission"]);
                                res.RefundCommision = dr.IsNull("RefundCommission") ? decimal.Zero : Convert.ToDecimal(dr["RefundCommission"]);
                                res.Discount = dr.IsNull("Discount") ? decimal.Zero : Convert.ToDecimal(dr["Discount"]);
                                res.PremiumLessCredit = dr.IsNull("PremiumLessCredit") ? decimal.Zero : Convert.ToDecimal(dr["PremiumLessCredit"]);

                                res.PremiumReference = dr.IsNull("PremiumReference") ? string.Empty : Convert.ToString(dr["PremiumReference"]);
                                res.CommisionReference = dr.IsNull("CommissionReference") ? string.Empty : Convert.ToString(dr["CommissionReference"]);
                                res.BatchDate = dr.IsNull("BatchDate") ? string.Empty : Convert.ToString(dr["BatchDate"]);
                                res.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);

                                res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                                res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                                res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                                res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                                res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["vat"]);

                                response.Add(res);
                            }
                        }
                        return new MainReportResponse
                        {
                            IsTransactionDone = true,
                            MainReportDetails = response,
                        };
                    }
                }
                else if (reportReq.InsuranceType == Constants.Insurance.Travel
                    && reportReq.ReportType == Constants.ReportType.TravelMainReport)
                {
                    SqlParameter[] paras = new SqlParameter[]
                   {
                               new SqlParameter("@Agency",reportReq.Agency),
                               new SqlParameter("@AgentCode", reportReq.AgentCode),
                               new SqlParameter("@DateFrom", reportReq.DateFrom),
                               new SqlParameter("@DateTo", reportReq.DateTo)
                   };
                    DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetTravelMainReport, paras);
                    List<MainReportDetails> response = new List<MainReportDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var res = new MainReportDetails();
                            res.BranchCode = dr.IsNull("BranchCode") ? string.Empty : Convert.ToString(dr["BranchCode"]);
                            res.BranchName = dr.IsNull("BranchName") ? string.Empty : Convert.ToString(dr["BranchName"]);
                            res.InsuredName = dr.IsNull("InsuredName") ? string.Empty : Convert.ToString(dr["InsuredName"]);
                            res.PolicyNo = dr.IsNull("POLICYNO") ? string.Empty : Convert.ToString(dr["POLICYNO"]);
                            res.EndorsementNo = dr.IsNull("ENDORSEMENTNO") ? string.Empty : Convert.ToString(dr["ENDORSEMENTNO"]);
                            res.SubClass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]);

                            res.AuthorizedUser = dr.IsNull("AuthorizedBy") ? string.Empty : Convert.ToString(dr["AuthorizedBy"]);
                            res.AuthorizedDate = dr.IsNull("AuthorizedDate") ? string.Empty : Convert.ToDateTime(dr["AuthorizedDate"]).Date.CovertToLocalFormat();
                            res.HandledBy = dr.IsNull("HandledBy") ? string.Empty : Convert.ToString(dr["HandledBy"]);
                            res.CommenceDate = dr.IsNull("CommenceDate") ? string.Empty : Convert.ToDateTime(dr["CommenceDate"]).Date.CovertToLocalFormat();
                            res.ExpiryDate = dr.IsNull("ExpiryDate") ? string.Empty : Convert.ToDateTime(dr["ExpiryDate"]).Date.CovertToLocalFormat();
                            res.PaymentMethod = dr.IsNull("PaymentMethod") ? string.Empty : Convert.ToString(dr["PaymentMethod"]);

                            res.Commission = dr.IsNull("Commission") ? decimal.Zero : Convert.ToDecimal(dr["Commission"]);
                            res.Discount = dr.IsNull("Discount") ? decimal.Zero : Convert.ToDecimal(dr["Discount"]);
                            res.PremiumLessCredit = dr.IsNull("PremiumLessCredit") ? decimal.Zero : Convert.ToDecimal(dr["PremiumLessCredit"]);

                            res.PremiumReference = dr.IsNull("PremiumReference") ? string.Empty : Convert.ToString(dr["PremiumReference"]);
                            res.CommisionReference = dr.IsNull("CommissionReference") ? string.Empty : Convert.ToString(dr["CommissionReference"]);
                            res.BatchDate = dr.IsNull("BatchDate") ? string.Empty : Convert.ToString(dr["BatchDate"]);
                            res.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);

                            res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                            res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                            res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                            res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);

                            response.Add(res);
                        }
                    }
                    return new MainReportResponse
                    {
                        IsTransactionDone = true,
                        MainReportDetails = response,
                    };
                }
                else if (reportReq.InsuranceType == Constants.Insurance.Home
                    && reportReq.ReportType == Constants.ReportType.HomeMainReport)
                {
                    SqlParameter[] paras = new SqlParameter[]
                   {
                               new SqlParameter("@Agency",reportReq.Agency),
                               new SqlParameter("@AgentCode", reportReq.AgentCode),
                               new SqlParameter("@DateFrom", reportReq.DateFrom),
                               new SqlParameter("@DateTo", reportReq.DateTo)
                   };
                    DataTable dt = BKICSQL.edt(StoredProcedures.ReportSP.GetHomeMainReport, paras);
                    List<MainReportDetails> response = new List<MainReportDetails>();
                    if (dt !=  null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            var res = new MainReportDetails();
                            res.BranchCode = dr.IsNull("BranchCode") ? string.Empty : Convert.ToString(dr["BranchCode"]);
                            res.BranchName = dr.IsNull("BranchName") ? string.Empty : Convert.ToString(dr["BranchName"]);
                            res.InsuredName = dr.IsNull("InsuredName") ? string.Empty : Convert.ToString(dr["InsuredName"]);
                            res.PolicyNo = dr.IsNull("POLICYNO") ? string.Empty : Convert.ToString(dr["POLICYNO"]);
                            res.EndorsementNo = dr.IsNull("ENDORSEMENTNO") ? string.Empty : Convert.ToString(dr["ENDORSEMENTNO"]);
                            res.SubClass = dr.IsNull("SUBCLASS") ? string.Empty : Convert.ToString(dr["SUBCLASS"]);
                            res.AuthorizedUser = dr.IsNull("AuthorizedBy") ? string.Empty : Convert.ToString(dr["AuthorizedBy"]);
                            res.AuthorizedDate = dr.IsNull("AuthorizedDate") ? string.Empty : Convert.ToDateTime(dr["AuthorizedDate"]).Date.CovertToLocalFormat();
                            res.HandledBy = dr.IsNull("HandledBy") ? string.Empty : Convert.ToString(dr["HandledBy"]);
                            res.CommenceDate = dr.IsNull("CommenceDate") ? string.Empty : Convert.ToDateTime(dr["CommenceDate"]).Date.CovertToLocalFormat();
                            res.ExpiryDate = dr.IsNull("ExpiryDate") ? string.Empty : Convert.ToDateTime(dr["ExpiryDate"]).Date.CovertToLocalFormat();
                            res.PaymentMethod = dr.IsNull("PaymentMethod") ? string.Empty : Convert.ToString(dr["PaymentMethod"]);

                            res.Commission = dr.IsNull("Commission") ? decimal.Zero : Convert.ToDecimal(dr["Commission"]);
                            res.RefundCommision = dr.IsNull("RefundCommission") ? decimal.Zero : Convert.ToDecimal(dr["RefundCommission"]);
                            res.Discount = dr.IsNull("Discount") ? decimal.Zero : Convert.ToDecimal(dr["Discount"]);
                            res.PremiumLessCredit = dr.IsNull("PremiumLessCredit") ? decimal.Zero : Convert.ToDecimal(dr["PremiumLessCredit"]);

                            res.PremiumReference = dr.IsNull("PremiumReference") ? string.Empty : Convert.ToString(dr["PremiumReference"]);
                            res.CommisionReference = dr.IsNull("CommissionReference") ? string.Empty : Convert.ToString(dr["CommissionReference"]);
                            res.BatchDate = dr.IsNull("BatchDate") ? string.Empty : Convert.ToString(dr["BatchDate"]);
                            res.CPR = dr.IsNull("CPR") ? string.Empty : Convert.ToString(dr["CPR"]);

                            res.NewPremium = dr.IsNull("NewPremium") ? decimal.Zero : Convert.ToDecimal(dr["NewPremium"]);
                            res.RenewalPremium = dr.IsNull("RenewalPremium") ? decimal.Zero : Convert.ToDecimal(dr["RenewalPremium"]);
                            res.AdditionalPremium = dr.IsNull("AdditionalPremium") ? decimal.Zero : Convert.ToDecimal(dr["AdditionalPremium"]);
                            res.RefundPremium = dr.IsNull("RefundPremium") ? decimal.Zero : Convert.ToDecimal(dr["RefundPremium"]);
                            res.Vat = dr.IsNull("Vat") ? decimal.Zero : Convert.ToDecimal(dr["Vat"]);

                            response.Add(res);
                        }
                    }
                    return new MainReportResponse
                    {
                        IsTransactionDone = true,
                        MainReportDetails = response,
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                return new MainReportResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message,
                };
            }
        }
    }
}