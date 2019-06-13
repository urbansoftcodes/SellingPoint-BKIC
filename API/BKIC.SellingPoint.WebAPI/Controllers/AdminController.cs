using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using BKIC.SellingPoint.WebAPI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using BLO = BKIC.SellingPoint.DL.BO;
using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;
using URI = BKIC.SellingPoint.DTO.Constants;

namespace BKIC.SellingPoint.WebAPI.Controllers
{
    public class AdminController : ApiController
    {
        public readonly IAdmin _adminRepository;
        private readonly AutoMapper.IMapper _mapper;

        public AdminController(IAdmin repository)
        {
            _adminRepository = repository;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetAdminAutoMapper();
        }

        [HttpPost]
        [Route(URI.AdminURI.CDUInsuranceProductMaster)]
        //  [ApiAuthorize]
        public RR.InsuranceProductMasterResponse PostAdminMasterDetails(RR.InsuranceProductMaster userdetails)
        {
            try
            {
                BLO.InsuranceProductMaster adminMaster = _mapper.Map<RR.InsuranceProductMaster,BLO.InsuranceProductMaster>(userdetails);
                BLO.InsuranceProductMasterResponse result = _adminRepository.InsuranceProductOperation(adminMaster);
                return _mapper.Map<BLO.InsuranceProductMasterResponse,RR.InsuranceProductMasterResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.InsuranceProductMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
           
        }

        [HttpPost]
        [Route(URI.AdminURI.BranchDetailsOperation)]
        //    [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.BranchMasterResponse PostBranchDetails(RR.BranchMaster request)
        {
            try
            {
                BLO.BranchMaster req = _mapper.Map<RR.BranchMaster, BLO.BranchMaster>(request);
                BLO.BranchMasterResponse result = _adminRepository.BranchOperation(req);
                return _mapper.Map<BLO.BranchMasterResponse, RR.BranchMasterResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.BranchMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
            
        }

        [HttpPost]
        [Route(URI.AdminURI.MotorCoverMasterOperation)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.MotorCoverMasterResponse MotorCoverMasterOperation(RR.MotorCoverMaster request)
        {
            try
            {
                BLO.MotorCoverMaster req = _mapper.Map<RR.MotorCoverMaster,BLO.MotorCoverMaster>(request);
                BLO.MotorCoverMasterResponse result = _adminRepository.MotorCoverOperation(req);
                return _mapper.Map<BLO.MotorCoverMasterResponse,RR.MotorCoverMasterResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.MotorCoverMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
           
        }

        [HttpPost]
        [Route(URI.AdminURI.MotorProductCoverOperation)]
        //[ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.MotorProductCoverResponse MotorProductCoverOperation(RR.MotorProductCover request)
        {
            BLO.MotorProductCover req = _mapper.Map<RR.MotorProductCover,BLO.MotorProductCover>(request);
            BLO.MotorProductCoverResponse result = _adminRepository.MotorProductCoverOperation(req);
            return _mapper.Map<BLO.MotorProductCoverResponse,RR.MotorProductCoverResponse>(result);
        }

        [HttpGet]
        [Route(URI.AdminURI.GetMasterTableByTableName)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        //[ApiAuthorize]
        public object GetMasterTableByTableName(string tableName)
        {
            try
            {
                var className = tableName.Replace("_", "");
                var assemplyNameSpace = "BKIC.SellingPoint.DL";
                var fullClassNameSpace = assemplyNameSpace + ".BO." + className;
                var tableObject = Activator.CreateInstance(Type.GetType(fullClassNameSpace + ", " + assemplyNameSpace, true));
                var tableType = tableObject.GetType();
                MethodInfo method = typeof(BKIC.SellingPoint.DL.BL.Implementation.Admin).GetMethod("GetMasterTableByTableName").MakeGenericMethod(new Type[] { tableType });
                var result = method.Invoke(Activator.CreateInstance(Type.GetType("BKIC.SellingPoint.DL.BL.Implementation.Admin, BKIC.SellingPoint.DL", true)), new object[] { tableName });

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.AgentOperation)]
        //[ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        //[ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.AgentMasterResponse AgentOperation(RR.AgentMaster request)
        {
            try
            {
                BLO.AgentMaster req = _mapper.Map<RR.AgentMaster, BLO.AgentMaster>(request);
                BLO.AgentMasterResponse result = _adminRepository.AgentOperation(req);
                return _mapper.Map<BLO.AgentMasterResponse, RR.AgentMasterResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgentMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.UserOperation)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        public RR.UserMasterDetailsResponse UserOperation(RR.UserMasterDetails request)
        {
            try
            {
                BLO.UserMasterDetails req = _mapper.Map<RR.UserMasterDetails, BLO.UserMasterDetails>(request);
                BLO.UserMasterDetailsResponse result = _adminRepository.UserOperation(req);
                return _mapper.Map<BLO.UserMasterDetailsResponse, RR.UserMasterDetailsResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.UserMasterDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.CategoryMasterOperation)]
        //  [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.CategoryMasterResponse CategoryMasterOperation(RR.CategoryMaster request)
        {
            try
            {
                BLO.CategoryMaster req = _mapper.Map<RR.CategoryMaster, BLO.CategoryMaster>(request);
                BLO.CategoryMasterResponse result = _adminRepository.CategoryOperation(req);
                return _mapper.Map<BLO.CategoryMasterResponse, RR.CategoryMasterResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.CategoryMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.InsuredMasterOperation)]
        //  [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        public RR.InsuredMasterDetailsResponse InsuredMasterOperation(RR.InsuredMasterDetails request)
        {
            try
            {
                BLO.InsuredMasterDetails req = _mapper.Map<RR.InsuredMasterDetails, BLO.InsuredMasterDetails>(request);
                BLO.InsuredMasterDetailsResponse result = _adminRepository.InsuredOperation(req);
                return _mapper.Map<BLO.InsuredMasterDetailsResponse, RR.InsuredMasterDetailsResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.InsuredMasterDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        //[HttpPost]
        //[Route(URI.AdminURI.FetchUserDetailsByCPR)]
        //[ApiAuthorize]
        //public RR.InsuredResult GetUserDetails(string cpr)
        //{
        //    try
        //    {
        //        var result = _adminRepository.GetUserDetails(cpr);

        //        var insureddetails = new RR.InsuredResult();
        //        insureddetails.InsuredDetails.InsuredCode = result.InsuredDetails.InsuredCode;
        //        insureddetails.InsuredDetails.CPR = result.InsuredDetails.CPR;
        //        insureddetails.InsuredDetails.FirstName = result.InsuredDetails.FirstName;
        //        insureddetails.InsuredDetails.MiddleName = result.InsuredDetails.MiddleName;
        //        insureddetails.InsuredDetails.LastName = result.InsuredDetails.LastName;
        //        insureddetails.InsuredDetails.Gender = result.InsuredDetails.Gender;
        //        insureddetails.InsuredDetails.Flat = result.InsuredDetails.Flat;
        //        insureddetails.InsuredDetails.Building = result.InsuredDetails.Building;
        //        insureddetails.InsuredDetails.Road = result.InsuredDetails.Road;
        //        insureddetails.InsuredDetails.Block = result.InsuredDetails.Block;
        //        insureddetails.InsuredDetails.Area = result.InsuredDetails.Area;
        //        insureddetails.InsuredDetails.Mobile = result.InsuredDetails.Mobile;
        //        insureddetails.InsuredDetails.Email = result.InsuredDetails.Email;
        //        insureddetails.InsuredDetails.DateOfBirth = result.InsuredDetails.DateOfBirth;

        //        insureddetails.InsuredDetails.Nationality = result.InsuredDetails.Nationality;
        //        insureddetails.InsuredDetails.Occupation = result.InsuredDetails.Occupation;

        //        return new RR.InsuredResult()
        //        {
        //            InsuredDetails = insureddetails.InsuredDetails,
        //            IsTransactionDone = result.IsTransactionDone,
        //            TransactionErrorMessage = result.TransactionErrorMessage
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new RR.InsuredResult { IsTransactionDone = false };
        //    }
        //}

        [HttpPost]
        [Route(URI.AdminURI.FetchUserDetailsByCPRInsuredCode)]
        //[ApiAuthorize]
        public RR.InsuredResponse GetInsuredDetailsByCPRInsuredCode(RR.InsuredRequest request)
        {
            try
            {
                BLO.InsuredRequest req = _mapper.Map<RR.InsuredRequest,BLO.InsuredRequest>(request);
                BLO.InsuredResponse result = _adminRepository.GetInsuredDetailsByCPRInsuredCode(req);
                return _mapper.Map<BLO.InsuredResponse,RR.InsuredResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.InsuredResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.GetAgencyInsured)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        public RR.AgencyInsuredResponse GetAgencyInsured(RR.AgencyInsuredRequest request)
        {
            try
            {
                BLO.AgencyInsuredRequest req = _mapper.Map<RR.AgencyInsuredRequest,BLO.AgencyInsuredRequest>(request);
                BLO.AgencyInsuredResponse result = _adminRepository.GetAgencyInsured(req);
                return _mapper.Map<BLO.AgencyInsuredResponse,RR.AgencyInsuredResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyInsuredResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.GetAgencyUsers)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        public RR.AgencyUserResponse GetAgencyUsers(RR.AgencyUserRequest request)
        {
            try
            {
                BLO.AgencyUserRequest req = _mapper.Map<RR.AgencyUserRequest, BLO.AgencyUserRequest>(request);
                BLO.AgencyUserResponse result = _adminRepository.GetAgencyUser(req);
                return _mapper.Map<BLO.AgencyUserResponse,RR.AgencyUserResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyUserResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpGet]
        [Route(URI.AdminURI.FetchDocumentDetailsByCPR)]
       // [ApiAuthorize]
       // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        public RR.DocumentDetailsResult GetDocumentsByCPR(string cpr, string agentcode)
        {
            try
            {
                var request = new RR.DocumentDetailsRequest();
                request.CPR = cpr;
                request.AgentCode = agentcode;

                BLO.DocumentDetailsRequest documentResult = _mapper.Map<RR.DocumentDetailsRequest,
                   BLO.DocumentDetailsRequest>(request);

                BLO.DocumentDetailsResponse result = _adminRepository.GetDocumentsByCPR(documentResult);

                if (result != null && result.IsTransactionDone && result.DocumentDetails != null)
                {
                    var docDetails = new List<RR.DocumentDetails>();

                    for (int i = 0; i < result.DocumentDetails.Count; i++)
                    {
                        docDetails.Add(new RR.DocumentDetails
                        {
                            DocumentNo = result.DocumentDetails[i].DocumentNo,
                            PolicyType = result.DocumentDetails[i].PolicyType,
                            ExpireDate = result.DocumentDetails[i].ExpireDate,
                            RenewalCount = result.DocumentDetails[i].RenewalCount
                        });
                    }
                    return new RR.DocumentDetailsResult
                    {
                        IsTransactionDone = true,
                        DocumentDetails = docDetails
                    };
                }
                return null;
               
            }
            catch (Exception ex)
            {
                return new RR.DocumentDetailsResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.FetchAgencyProductByType)]
        public RR.AgencyProductResponse GetAgencyProducts(RR.AgecyProductRequest request)
        {
            try
            {
                BLO.AgecyProductRequest agencyProductReq = _mapper.Map<RR.AgecyProductRequest,BLO.AgecyProductRequest>(request);
                BLO.AgencyProductResponse result = _adminRepository.GetAgencyProducts(agencyProductReq);
                return _mapper.Map<BLO.AgencyProductResponse,RR.AgencyProductResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AgencyProductResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.MotorCoverOperation)]
        public RR.MotorCoverResponse MotorCoverOperation(RR.MotorCoverRequest request)
        {
            try
            {
                BLO.MotorCoverRequest req = _mapper.Map<RR.MotorCoverRequest, BLO.MotorCoverRequest>(request);
                BLO.MotorCoverResponse result = _adminRepository.GetProductCover(req);
                return _mapper.Map<BLO.MotorCoverResponse, RR.MotorCoverResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorCoverResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.MotorVehicleOperation)]
        public RR.MotorVehicleMasterResponse MotorVehicleOperation(RR.MotorVehicleMaster request)
        {
            try
            {
                BLO.MotorVehicleMaster req = _mapper.Map<RR.MotorVehicleMaster, BLO.MotorVehicleMaster>(request);
                BLO.MotorVehicleMasterResponse result = _adminRepository.MotorVehicleMasterOperation(req);
                return _mapper.Map<BLO.MotorVehicleMasterResponse, RR.MotorVehicleMasterResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorVehicleMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }   

        [HttpPost]
        [Route(URI.AdminURI.MotorYearOperation)]
        public RR.MotorYearMasterResponse MotorYearOperation(RR.MotorYearMaster request)
        {
            try
            {
                BLO.MotorYearMaster req = _mapper.Map<RR.MotorYearMaster, BLO.MotorYearMaster>(request);
                BLO.MotorYearMasterResponse result = _adminRepository.MotorYearMasterOperation(req);
                return _mapper.Map<BLO.MotorYearMasterResponse, RR.MotorYearMasterResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorYearMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        [HttpPost]
        [Route(URI.AdminURI.MotorEngineCCOperation)]
        public RR.MotorEngineCCResponse MotorProductMasterOperation(RR.MotorEngineCCMaster request)
        {
            try
            {
                BLO.MotorEngineCCMaster req = _mapper.Map<RR.MotorEngineCCMaster, BLO.MotorEngineCCMaster>(request);
                BLO.MotorEngineCCResponse result = _adminRepository.MotorEngineCCOperation(req);
                return _mapper.Map<BLO.MotorEngineCCResponse, RR.MotorEngineCCResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorEngineCCResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }


        [HttpPost]
        [Route(URI.AdminURI.MotorProductOperation)]
        public RR.MotorProductMasterResponse MotorProductMasterOperation(RR.MotorProductMaster request)
        {
            try
            {
                BLO.MotorProductMaster req = _mapper.Map<RR.MotorProductMaster, BLO.MotorProductMaster>(request);
                BLO.MotorProductMasterResponse result = _adminRepository.MotorProductMasterOperation(req);
                return _mapper.Map<BLO.MotorProductMasterResponse, RR.MotorProductMasterResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.MotorProductMasterResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
        [HttpGet]
        [Route(URI.AdminURI.GetAgencyCPR)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        public List<string> GetAgencyCPR(string CPR, string Agency)
        {
            try
            {
                //BLO.AgencyInsuredRequest req = _mapper.Map<RR.AgencyInsuredRequest, BLO.AgencyInsuredRequest>(request);
                BLO.AgencyInsuredResponse result = _adminRepository.GetAgencyCPR(CPR, Agency);
                var cc = _mapper.Map<BLO.AgencyInsuredResponse, RR.AgencyInsuredResponse>(result);
                return cc.AgencyInsured.FindAll(c => c.CPR != null).Select(e => e.CPR).ToList();
                //return _mapper.Map<BLO.AgencyInsuredResponse, RR.AgencyInsuredResponse>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route(URI.AdminURI.RenewalPrecheckByInsuranceType)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin, BKIC.SellingPont.DTO.Constants.Roles.BranchAdmin)]
        public RenewalPrecheckResponse RenewalPrecheck(RR.RenewalPrecheckRequest request)
        {
            try
            {
                BLO.RenewalPrecheckRequest req = _mapper.Map<RR.RenewalPrecheckRequest, BLO.RenewalPrecheckRequest>(request);
                BLO.RenewalPrecheckResponse result = _adminRepository.RenewalPrecheck(req);
                return _mapper.Map<BLO.RenewalPrecheckResponse, RR.RenewalPrecheckResponse>(result);              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}