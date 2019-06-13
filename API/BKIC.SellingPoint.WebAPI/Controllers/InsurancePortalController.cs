using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DL.BO;
using BKIC.SellingPoint.DTO.Constants;
using BKIC.SellingPoint.WebAPI.Framework;
using BKIC.SellingPont.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using BLO = BKIC.SellingPoint.DL.BO;

using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;

using URI = BKIC.SellingPoint.DTO.Constants;

namespace BKIC.SellingPoint.WebAPI.Controllers
{
    /// <summary>
    /// Methods for portal - Commissions, HIR, Active policies...
    /// </summary>
    public class InsurancePortalController : ApiController
    {
        private readonly IInsurancePortal _insurancePortalRepository;
        private readonly IMotorInsurance _motorInsuranceRepository;
        private readonly AutoMapper.IMapper _mapper;

        public InsurancePortalController(IInsurancePortal portal, IMotorInsurance motor)
        {
            _insurancePortalRepository = portal;
            _motorInsuranceRepository = motor;

            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetPortalAutoMapper();
        }


        /// <summary>
        /// Get commission for the specified insurance type.
        /// </summary>
        /// <param name="request">Commission request.</param>
        /// <returns>Commission amount.</returns>
        //[ApiAuthorize]
        [HttpPost]
        [Route("api/insurance/Commission")]
        public RR.CommissionResponse GetCommision(RR.CommissionRequest request)
        {
            try
            {
                BLO.CommissionRequest requestdetails = _mapper.Map<RR.CommissionRequest, BLO.CommissionRequest>(request);
                BLO.CommissionResponse result = _insurancePortalRepository.GetCommission(requestdetails);
                return _mapper.Map<BLO.CommissionResponse, RR.CommissionResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.CommissionResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get commission for the home insurance Basic and SRCC commission used in Endorsement page.
        /// </summary>
        /// <param name="request">Home commission request.</param>
        /// <returns>Basic and SRCC premium.</returns>
        [HttpPost]
        [Route("api/insurance/HomeCommission")]
        public RR.HomeCommissionResponse GetHomeCommision(RR.HomeCommissionRequest request)
        {
            try
            {
                BLO.HomeCommissionRequest requestdetails = _mapper.Map<RR.HomeCommissionRequest, BLO.HomeCommissionRequest>(request);
                BLO.HomeCommissionResponse result = _insurancePortalRepository.GetHomeEndorsementCommission(requestdetails);
                return _mapper.Map<BLO.HomeCommissionResponse, RR.HomeCommissionResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeCommissionResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }




        /// <summary>
        /// Get commission for the home insurance Basic and SRCC commission used in Policy buy page.
        /// </summary>
        /// <param name="request">Home commission request.</param>
        /// <returns>Total Commmision.</returns>
        [HttpPost]
        [Route("api/insurance/HomePolicyCommission")]
        public RR.HomeCommissionResponse GetHomePolicyCommision(RR.HomeCommissionRequest request)
        {
            try
            {
                BLO.HomeCommissionRequest requestdetails = _mapper.Map<RR.HomeCommissionRequest, BLO.HomeCommissionRequest>(request);
                BLO.HomeCommissionResponse result = _insurancePortalRepository.GetHomePolicyCommission(requestdetails);
                return _mapper.Map<BLO.HomeCommissionResponse, RR.HomeCommissionResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.HomeCommissionResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Get vat amount for the premium.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Vat amount for policy premium and vat amount for commission.</returns>
        [HttpPost]
        [Route(URI.VatURI.CalculateVat)]
        public RR.VatResponse GetVat(RR.VatRequest request)
        {
            try
            {
                BLO.VatRequest req = _mapper.Map<RR.VatRequest, BLO.VatRequest>(request);
                BLO.VatResponse result = _insurancePortalRepository.GetVat(req);
                return _mapper.Map<BLO.VatResponse, RR.VatResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.VatResponse
                {
                    IsTransactionDone = true,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        // <summary>
        /// Fetch the deomestichelp policies by type(Active or HIR).
        /// </summary>
        /// <param name="request">Search the policies by branch or agency or user.</param>
        /// <returns>List of domestichelp policy details.</returns>
        [HttpPost]
        [Route(URI.AdminURI.FetchDomesticPolicyDetails)]
        //[ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.AdminFetchDomesticDetailsResponse GetDomesticPolicyDetails(RR.AdminFetchDomesticDetailsRequest request)
        {
            try
            {
                BLO.AdminFetchDomesticDetailsRequest requestdetails = _mapper.Map<RR.AdminFetchDomesticDetailsRequest, BLO.AdminFetchDomesticDetailsRequest>(request);
                BLO.AdminFetchDomesticDetailsResponse result = _insurancePortalRepository.FetchDomesticPolicyDetails(requestdetails);
                return _mapper.Map<BLO.AdminFetchDomesticDetailsResponse, RR.AdminFetchDomesticDetailsResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AdminFetchDomesticDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }

            return null;
        }

        /// <summary>
        /// Fetch the travel policies by type(Active or HIR).
        /// </summary>
        /// <param name="request">Search the policies by branch or agency or user.</param>
        /// <returns>List of travel policy details.</returns>
        [HttpPost]
        [Route(URI.AdminURI.FetchTravelPolicyDetails)]
        //[ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.AdminFetchTravelDetailsResponse GetTravelPolicyDetails(RR.AdminFetchTravelDetailsRequest request)
        {
            try
            {
                BLO.AdminFetchTravelDetailsRequest requestdetails = _mapper.Map<RR.AdminFetchTravelDetailsRequest, BLO.AdminFetchTravelDetailsRequest>(request);
                BLO.AdminFetchTravelDetailsResponse result = _insurancePortalRepository.FetchTravelPolicyDetails(requestdetails);
                return _mapper.Map<BLO.AdminFetchTravelDetailsResponse, RR.AdminFetchTravelDetailsResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AdminFetchTravelDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Fetch the home policies by type(Active or HIR).
        /// </summary>
        /// <param name="request">Search the policies by branch or agency or user.</param>
        /// <returns>List of home policy details.</returns>
        [HttpPost]
        [Route(URI.AdminURI.FetchHomePolicyDetails)]
        //[ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.AdminFetchHomeDetailsResponse GetHomePolicyDetails(RR.AdminFetchHomeDetailsRequest request)
        {
            try
            {
                BLO.AdminFetchHomeDetailsRequest req = _mapper.Map<RR.AdminFetchHomeDetailsRequest, BLO.AdminFetchHomeDetailsRequest>(request);
                BLO.AdminFetchHomeDetailsResponse result = _insurancePortalRepository.FetchHomePolicyDetails(req);
                return _mapper.Map<BLO.AdminFetchHomeDetailsResponse, RR.AdminFetchHomeDetailsResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AdminFetchHomeDetailsResponse
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        //[ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]      
        /// <summary>
        /// Fetch motor policy details by status HIR or Active.
        /// </summary>
        /// <param name="request">Admin motor policy fetch request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(URI.AdminURI.FetchMotorPolicyDetails)]
        // [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.AdminFetchMotorDetailsResponse FetchMotorPolicyDetails(RR.AdminFetchMotorDetailsRequest request)
        {
            try
            {
                BLO.AdminFetchMotorDetailsRequest requestdetails = _mapper.Map<RR.AdminFetchMotorDetailsRequest, BLO.AdminFetchMotorDetailsRequest>(request);
                BLO.AdminFetchMotorDetailsResponse result = _motorInsuranceRepository.FetchMotorPolicyDetails(requestdetails);
                return _mapper.Map<BLO.AdminFetchMotorDetailsResponse, RR.AdminFetchMotorDetailsResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.AdminFetchMotorDetailsResponse
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
        [HttpPost]
        [Route(URI.InsurancePortalURI.UpdateHIRStatus)]
        //[ApiAuthorize]
        public RR.UpdateHIRStatusResponse UpdateStatus(RR.UpdateHIRStatusRequest request)
        {
            try
            {
                BLO.UpdateHIRStatusRequest req = _mapper.Map<RR.UpdateHIRStatusRequest, BLO.UpdateHIRStatusRequest>(request);
                BLO.UpdateHIRStatusResponse result = _insurancePortalRepository.UpdateHIRStatusCode(req);
                return _mapper.Map<BLO.UpdateHIRStatusResponse, RR.UpdateHIRStatusResponse>(result);               
            }
            catch (Exception ex)
            {
                return new RR.UpdateHIRStatusResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(URI.InsurancePortalURI.UpdateHIRRemarks)]
        //[ApiAuthorize]
        public RR.UpdateHIRRemarksResponse UpdateRemarks(RR.UpdateHIRRemarksRequest request)
        {
            try
            {
                BLO.UpdateHIRRemarksRequest req = _mapper.Map<RR.UpdateHIRRemarksRequest, BLO.UpdateHIRRemarksRequest>(request);
                BLO.UpdateHIRRemarksResponse result = _insurancePortalRepository.UpdateHIRRemarks(req);
                return _mapper.Map<BLO.UpdateHIRRemarksResponse, RR.UpdateHIRRemarksResponse>(result);
            }
            catch (Exception ex)
            {
                return new RR.UpdateHIRRemarksResponse
                {
                    TransactionErrorMessage = ex.Message,
                    IsTransactionDone = false
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="motorID"></param>
        /// <param name="InsuredCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(URI.InsurancePortalURI.MotorDetails)]
        public RR.MotorDetailsPortalResponse GetMotorDetails(long motorID, string InsuredCode)
        {
            try
            {
                BLO.MotorDetailsPortalResponse result = new BLO.MotorDetailsPortalResponse();
                result = _insurancePortalRepository.GetMotorDetails(motorID, InsuredCode);

                RR.MotorDetailsPortalResponse portalresponse = new RR.MotorDetailsPortalResponse();

                RR.MotorInsurancePolicy response = new RR.MotorInsurancePolicy()
                {
                    InsuredCode = result.MotorInsurancePolicy.InsuredCode,
                    InsuredName = result.MotorInsurancePolicy.InsuredName,
                    DocumentNo = result.MotorInsurancePolicy.DocumentNo,
                    ExpiryDate = result.MotorInsurancePolicy.ExpiryDate,
                    DOB = result.MotorInsurancePolicy.DOB,
                    YearOfMake = result.MotorInsurancePolicy.YearOfMake,
                    VehicleMake = result.MotorInsurancePolicy.VehicleMake,
                    VehicleModel = result.MotorInsurancePolicy.VehicleModel,
                    VehicleTypeCode = result.MotorInsurancePolicy.vehicleTypeCode,
                    PolicyCode = result.MotorInsurancePolicy.PolicyCode,
                    IsNCB = result.MotorInsurancePolicy.IsNCB,
                    NCBStartDate = result.MotorInsurancePolicy.NCBStartDate,
                    NCBEndDate = result.MotorInsurancePolicy.NCBEndDate,
                    VehicleValue = result.MotorInsurancePolicy.VehicleValue,
                    PremiumAmount = result.MotorInsurancePolicy.PremiumAmount,
                    PolicyCommencementDate = result.MotorInsurancePolicy.PolicyCommencementDate,
                    DeliveryOption = result.MotorInsurancePolicy.DeliveryOption,
                    DeliveryBranch = result.MotorInsurancePolicy.DeliveryBranch,
                    RegistrationNumber = result.MotorInsurancePolicy.RegistrationNumber,
                    ChassisNo = result.MotorInsurancePolicy.ChassisNo,
                    EngineCC = result.MotorInsurancePolicy.EngineCC,
                    FinancierCompanyCode = result.MotorInsurancePolicy.FinancierCompanyCode,
                    ExcessType = result.MotorInsurancePolicy.ExcessType,
                    ExcessAmount = result.MotorInsurancePolicy.ExcessAmount,
                    AgentBranch = result.MotorInsurancePolicy.Branch,
                    MobileNumber = result.MotorInsurancePolicy.MobileNumber,
                    CPR = result.MotorInsurancePolicy.CPR,
                    IsHIR = result.MotorInsurancePolicy.IsHIR
                };
                portalresponse.MotorInsurancePolicy = response;
                portalresponse.LoadAmount = result.LoadAmount;
                portalresponse.DiscountAmount = result.DiscountAmount;
                portalresponse.Remarks = result.Remarks;
                portalresponse.IsTransactionDone = result.IsTransactionDone;
                portalresponse.TransactionErrorMessage = result.TransactionErrorMessage;
                return portalresponse;
            }
            catch (Exception ex)
            {
                return new RR.MotorDetailsPortalResponse { TransactionErrorMessage = ex.Message, IsTransactionDone = false };
            }
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.UploadTempFile)]
        [ApiAuthorize(Roles.BranchAdmin, Roles.User, Roles.SuperAdmin)]
        public bool UploadTempFiles(string insuranceType, string insuredCode, string policyNo, string linkId)
        {
            try
            {
                string originalFileName = "";
                string convertedFileName = "";
                string contenttype = "";

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        var directoryPath = "~/InsurancePortalTempFile/" + insuranceType + "/" + insuredCode + "/" + policyNo + "/" + linkId;
                        var fileExtenstion = postedFile.FileName.Split('.')[1];
                        Stream checkStream = postedFile.InputStream;
                        BinaryReader chkBinary = new BinaryReader(checkStream);
                        Byte[] chkbytes = chkBinary.ReadBytes(0x10);

                        string data_as_hex = BitConverter.ToString(chkbytes);
                        string magicCheck = data_as_hex.Substring(0, 11);
                        //Set the contenttype based on File Extension
                        switch (magicCheck)
                        {
                            case "FF-D8-FF-E1":
                                contenttype = "image/jpg";
                                break;

                            case "FF-D8-FF-E0":
                                contenttype = "image/jpeg";
                                break;

                            case "25-50-44-46":
                                contenttype = "text/pdf";
                                break;

                            case "89-50-4E-47":
                                contenttype = "image/png";
                                break;

                            case "42-4D":
                                contenttype = "image/bmp";
                                break;

                            case "50-B-05-06":
                                contenttype = "text/docx";
                                break;

                            case "50-4B-03-04":
                                contenttype = "text/docx";
                                break;
                        }
                        if (contenttype == "image/jpg" || contenttype == "image/jpeg" || contenttype == "text/pdf" || contenttype == "text/png"
                            || contenttype == "text/bmp" || contenttype == "text/docx")
                        {
                            if (fileExtenstion == "png" || fileExtenstion == "jpg" || fileExtenstion == "gif" || fileExtenstion == "bmp"
                                || fileExtenstion == "jpeg" || fileExtenstion == "docx" || fileExtenstion == "pdf")
                            {
                                //convertedFileName = Guid.NewGuid().ToString() + "." + fileExtenstion;
                                originalFileName = postedFile.FileName;

                                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(directoryPath)))
                                {
                                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(directoryPath));
                                }

                                directoryPath = HttpContext.Current.Server.MapPath(directoryPath + "/" + originalFileName);

                                postedFile.SaveAs(directoryPath);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        [HttpGet]
        [Route(URI.InsurancePortalURI.DownloadTempFile)]
        [ApiAuthorize]
        public HttpResponseMessage DownloadTempFile(string insuranceType, string insuredCode, string policyNo, string linkId, string fileName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string filePath = "~/InsurancePortalTempFile/" + insuranceType + "/" + insuredCode + "/" + policyNo + "/" + linkId + "/" + fileName;

            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath)))
            {
                FileStream stream = File.OpenRead(HttpContext.Current.Server.MapPath(filePath));

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Utility.GetMimeType(fileName));
                return response;
            }
            return response = new HttpResponseMessage(HttpStatusCode.Gone);
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.DeleteTempFile)]
        [ApiAuthorize]
        public bool DeleteTempFile(string insuranceType, string insuredCode, string policyNo, string linkId, string fileName)
        {
            string filePath = "~/InsurancePortalTempFile/" + insuranceType + "/" + insuredCode + "/" + policyNo + "/" + linkId + "/" + fileName;
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath)))
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath(filePath));
            }
            return true;
        }

        [HttpPost]
        [ApiAuthorize]
        [Route(URI.InsurancePortalURI.DeleteAllTempFiles)]
        public RR.TransactionWrapper DeleteAllTempFiles(string insuranceType, string insuredCode, string policyNo, string linkId)
        {
            try
            {
                var directoryPath = "~/InsurancePortalTempFile/" + insuranceType + "/" + insuredCode + "/" + policyNo + "/" + linkId;

                if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(directoryPath)))
                {
                    System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(directoryPath), true);
                }

                return new RR.TransactionWrapper()
                {
                    IsTransactionDone = true
                };
            }
            catch (Exception exc)
            {
                return new DTO.RequestResponseWrappers.TransactionWrapper()
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = exc.Message
                };
            }
        }

        [HttpGet]
        [Route(URI.InsurancePortalURI.DownloadInsranceFile)]
        [ApiAuthorize]
        public HttpResponseMessage DownloadInsranceFile(string insuranceType, string insuredCode, string policyNo, string linkId, string fileName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            string filePath = "~/InsurancePortalFile/" + insuranceType + "/" + insuredCode + "/" + policyNo + "/" + linkId + "/" + fileName;

            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(filePath)))
            {
                FileStream stream = File.OpenRead(HttpContext.Current.Server.MapPath(filePath));

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Utility.GetMimeType(fileName));
                return response;
            }
            return response = new HttpResponseMessage(HttpStatusCode.Gone);
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.UploadHIRDocuments)]
        [ApiAuthorize]
        public DTO.RequestResponseWrappers.TransactionWrapper UploadHIRDocuments(string insuranceType, string insuredCode, string policyNo, string linkId, string refID)
        {
            string originalFolder = "~/InsurancePortalFile/" + insuranceType + "/" + insuredCode + "/" + policyNo + "/" + linkId;
            string tempFolder = "~/InsurancePortalTempFile/" + insuranceType + "/" + insuredCode + "/" + policyNo + "/" + linkId;

            if (System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(tempFolder)))
            {
                if (!System.IO.Directory.Exists(HttpContext.Current.Server.MapPath(originalFolder)))
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(originalFolder));
                }

                string[] files = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath(tempFolder));

                DataTable uploadFiles = new DataTable();
                uploadFiles.Columns.Add("InsuredCode");
                uploadFiles.Columns.Add("PolicyDocumentNo");
                uploadFiles.Columns.Add("LinkID");
                uploadFiles.Columns.Add("FilesURL");
                uploadFiles.Columns.Add("FileName");

                foreach (string file in files)
                {
                    string fileName = System.IO.Path.GetFileName(file);

                    string fileDownloadApi = URI.InsurancePortalURI.DownloadInsranceFile.Replace("{insuranceType}", insuranceType)
                        .Replace("{insuredCode}", insuredCode).Replace("{policyNo}", policyNo).Replace("{linkId}", linkId)
                        .Replace("{fileName}", fileName);

                    string destFile = System.IO.Path.Combine(HttpContext.Current.Server.MapPath(originalFolder), fileName);
                    System.IO.File.Copy(file, destFile, true);

                    uploadFiles.Rows.Add(insuredCode, policyNo, linkId, fileDownloadApi, fileName);
                }

                BLO.TransactionWrapper dbTransaction = _insurancePortalRepository.UploadHIRDocuments(uploadFiles, insuredCode,
                    policyNo, linkId, insuranceType, refID);

                System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(tempFolder), true);

                return new RR.TransactionWrapper()
                {
                    IsTransactionDone = dbTransaction.IsTransactionDone,
                    TransactionErrorMessage = dbTransaction.TransactionErrorMessage
                };
            }

            return new RR.TransactionWrapper() { IsTransactionDone = true };
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.FetchDocuments)]
        [ApiAuthorize(Roles.SuperAdmin)]
        public RR.FetchDocumentsResponse FetchHIRDoc(RR.FetchDocumentsRequest req)
        {
            try
            {
                BLO.FetchDocumentsRequest request = new BLO.FetchDocumentsRequest();
                request.DocumentNo = req.DocumentNo;
                request.InsuredCode = req.InsuredCode;
                request.LinkID = req.LinkID;

                BLO.FetchDocumentsResponse results = _insurancePortalRepository.FetchHIRDocuments(request);
                List<RR.HIRRequestDocuments> fetchDocList = new List<RR.HIRRequestDocuments>();
                fetchDocList = (from DataRow row in results.HIRDocdt.Rows

                                select new RR.HIRRequestDocuments
                                {
                                    FileName = Convert.ToString(row["FileName"]),
                                    FileURL = Convert.ToString(row["FilesURL"]),
                                    LinkID = Convert.ToString(row["LinkID"]),
                                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                                }).ToList();

                return new RR.FetchDocumentsResponse { FilesDocuments = fetchDocList, IsTransactionDone = results.IsTransactionDone, TransactionErrorMessage = results.TransactionErrorMessage };
            }
            catch (Exception ex)
            {
                return new RR.FetchDocumentsResponse { TransactionErrorMessage = ex.Message };
            }
        }

        [HttpGet]
        [Route(URI.InsurancePortalURI.PrecheckHIRDocumentUpload)]
        [ApiAuthorize(Roles.User, Roles.BranchAdmin)]
        public DTO.RequestResponseWrappers.HIRDocumentsUploadPrecheckResponse PrecheckHIRDocumentUpload(string insuredCode, string policyNo, string linkId, string insuranceType)
        {
            try
            {
                BLO.DocumentsUploadPrecheckResponse results = _insurancePortalRepository.PrecheckHIRDocumentUpload(insuredCode, policyNo, linkId, insuranceType);
                return new RR.HIRDocumentsUploadPrecheckResponse
                {
                    IsValidUser = results.IsValidUser,
                    IsTransactionDone = results.IsTransactionDone,
                    IsDocumentsCanUpload = results.IsDocumentsCanUpload,
                    TransactionErrorMessage = results.TransactionErrorMessage
                };
            }
            catch (Exception ex)
            {
                return new RR.HIRDocumentsUploadPrecheckResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        [HttpGet]
        [Route(URI.InsurancePortalURI.GetEmailMessageForRecord)]
        [ApiAuthorize(Roles.SuperAdmin)]
        public DTO.RequestResponseWrappers.EmailMessageAuditResult GetEmailMessageForRecord(string insuredCode, string policyNo, string linkId)
        {
            try
            {
                BLO.EmailMessageAuditResult dlRelsult = _insurancePortalRepository.GetEmailMessageForRecord(insuredCode, policyNo, linkId);

                return _mapper.Map<BLO.EmailMessageAuditResult,
                       RR.EmailMessageAuditResult>(dlRelsult);
            }
            catch (Exception exc)
            {
                return new DTO.RequestResponseWrappers.EmailMessageAuditResult() { IsTransactionDone = false, TransactionErrorMessage = exc.Message };
            }
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.FetchUserDetails)]
        [ApiAuthorize(Roles.SuperAdmin)]
        public RR.FetchUserDetailsResponse FetchUserDetails(RR.FetchUserDetailsRequest requestdetails)
        {
            try
            {
                BLO.FetchUserDetailsRequest request = new BLO.FetchUserDetailsRequest();
                request.Search = requestdetails.Search;
                request.FilterType = requestdetails.FilterType;

                BLO.FetchUserDetailsResponse response = _insurancePortalRepository.FetchUserDetails(request);

                return _mapper.Map<BLO.FetchUserDetailsResponse,
                       RR.FetchUserDetailsResponse>(response);
            }
            catch (Exception ex)
            {
                return new RR.FetchUserDetailsResponse { IsTransactionDone = false, TransactionErrorMessage = ex.Message };
            }
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.UpdatePassword)]
        [ApiAuthorize(Roles.SuperAdmin)]
        public RR.ResetPasswordResult ResetPassword(RR.ResetPassword resetPassword)
        {
            RR.ResetPasswordResult result = new DTO.RequestResponseWrappers.ResetPasswordResult();

            BLO.ResetPasswordResult boResult = _insurancePortalRepository.ResetPassword(resetPassword.NewPassword, resetPassword.InsuredCode);

            result.IsPasswordChanged = boResult.IsPasswordChanged;
            result.IsTransactionDone = boResult.IsTransactionDone;
            result.TransactionErrorMessage = boResult.TransactionErrorMessage;

            return result;
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.UpdateUserStatus)]
        [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.ChangeUserStatusResponse ChangeUserStatus(RR.ChangeUserStatusRequest prequest)
        {
            BLO.ChangeUserStatusRequest request = new BLO.ChangeUserStatusRequest();
            request.InsuredCode = prequest.InsuredCode;
            request.IsActive = prequest.IsActive;
            BLO.ChangeUserStatusResponse boResult = _insurancePortalRepository.ChangeUserStatus(request);

            return new RR.ChangeUserStatusResponse { IsTransactionDone = boResult.IsTransactionDone };
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.deletedueRenewal)]
        [ApiAuthorize(BKIC.SellingPont.DTO.Constants.Roles.SuperAdmin)]
        public RR.TransactionWrapper DeleteDueRenewal(RR.DeleteDueRenewalRequest request)
        {
            //BLO.TransactionWrapper result = _motorInsuranceRepository.DeleteDueRenewal(request.LinkID, request.InsuranceType);
            BLO.TransactionWrapper result = new BLO.TransactionWrapper();
            return new RR.TransactionWrapper { IsTransactionDone = result.IsTransactionDone, TransactionErrorMessage = result.TransactionErrorMessage };
        }

        [HttpPost]
        [Route(URI.InsurancePortalURI.FetchDashboard)]
        //[ApiAuthorize(Roles.Admin)]
        public RR.DashboardResponse FetchDashboard(RR.DashboardRequest request)
        {
            try
            {
                BLO.DashboardResponse result = _insurancePortalRepository.GetPortalDashboard(request.FromDate, request.ToDate, request.AgencyCode, request.Agent, request.BranchCode);
                return _mapper.Map<BLO.DashboardResponse,
                       RR.DashboardResponse>(result);
            }
            catch(Exception ex)
            {
                return new RR.DashboardResponse
                          {
                             IsTransactionDone = false, TransactionErrorMessage = ex.Message
                };
            }
                   
        }
    }
}