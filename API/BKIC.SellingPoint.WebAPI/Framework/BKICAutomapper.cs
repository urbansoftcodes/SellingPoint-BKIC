using BKIC.SellingPoint.DTO.RequestResponseWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLO = BKIC.SellingPoint.DL.BO;

using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;

namespace BKIC.SellingPoint.WebAPI.Framework
{
    public class BKICAutomapper
    {
        private AutoMapper.MapperConfiguration _mapperConfig;
        private AutoMapper.IMapper _mapper;

        public AutoMapper.IMapper GetUserAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.UserMaster,
                            RR.UserMaster>().ReverseMap();
                cfg.CreateMap<BLO.PostUserDetailsResult,
                          RR.PostUserDetailsResult>().ReverseMap();

                cfg.CreateMap<BLO.AdminRegister,
                            RR.AdminRegister>().ReverseMap();
                cfg.CreateMap<BLO.PostAdminUserResult,
                          RR.PostAdminUserResult>().ReverseMap();

                //cfg.CreateMap<DL.BO.OracleClaimType.InvitaClaimHistroyInfoType, DL.BO.Claims.clmHistoryView>().ReverseMap();
            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetAdminAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.InsuranceProductMasterResponse,
                            RR.InsuranceProductMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.InsuranceProductMaster,
                            RR.InsuranceProductMaster>().ReverseMap();
                //cfg.CreateMap<DL.BO.OracleClaimType.InvitaClaimHistroyInfoType, DL.BO.Claims.clmHistoryView>().ReverseMap();

                cfg.CreateMap<BLO.BranchMaster,
                            RR.BranchMaster>().ReverseMap();

                cfg.CreateMap<BLO.BranchMasterResponse,
                            RR.BranchMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorCoverMaster,
                            RR.MotorCoverMaster>().ReverseMap();

                cfg.CreateMap<BLO.MotorCoverMasterResponse,
                            RR.MotorCoverMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorProductCover,
                            RR.MotorProductCover>().ReverseMap();

                cfg.CreateMap<BLO.MotorProductCoverResponse,
                            RR.MotorProductCoverResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgentMaster,
                           RR.AgentMaster>().ReverseMap();

                cfg.CreateMap<BLO.AgentMasterResponse,
                            RR.AgentMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.UserMasterDetails,
                           RR.UserMasterDetails>().ReverseMap();

                cfg.CreateMap<BLO.UserMasterDetailsResponse,
                            RR.UserMasterDetailsResponse>().ReverseMap();

                cfg.CreateMap<BLO.CategoryMaster,
                          RR.CategoryMaster>().ReverseMap();

                cfg.CreateMap<BLO.CategoryMasterResponse,
                            RR.CategoryMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.InsuredMasterDetails,
                          RR.InsuredMasterDetails>().ReverseMap();

                cfg.CreateMap<BLO.InsuredMasterDetailsResponse,
                            RR.InsuredMasterDetailsResponse>().ReverseMap();

                cfg.CreateMap<BLO.InsuredResponse,
                           RR.InsuredResponse>().ReverseMap();

                cfg.CreateMap<BLO.InsuredRequest,
                           RR.InsuredRequest>().ReverseMap();
                cfg.CreateMap<BLO.AgencyInsuredRequest,
                           RR.AgencyInsuredRequest>().ReverseMap();

                cfg.CreateMap<BLO.AgencyInsuredResponse,
                            RR.AgencyInsuredResponse>().ReverseMap();

                cfg.CreateMap<BLO.DocumentDetailsResponse,
                    RR.DocumentDetailsResult>().ReverseMap();

                cfg.CreateMap<BLO.DocumentDetailsRequest,
                    RR.DocumentDetailsRequest>().ReverseMap();

                cfg.CreateMap<BLO.AgencyUserRequest,
                    RR.AgencyUserRequest>().ReverseMap();

                cfg.CreateMap<BLO.AgencyUserResponse,
                   RR.AgencyUserResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgecyProductRequest,
                   RR.AgecyProductRequest>().ReverseMap();

                cfg.CreateMap<BLO.MotorProduct,
                  RR.MotorProduct>().ReverseMap();

                cfg.CreateMap<BLO.HomeProduct,
                 RR.HomeProduct>().ReverseMap();

                cfg.CreateMap<BLO.AgencyProduct,
                RR.AgencyProduct>().ReverseMap();

                cfg.CreateMap<BLO.AgencyProductResponse,
                RR.AgencyProductResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorCoverRequest,
                RR.MotorCoverRequest>().ReverseMap();

                cfg.CreateMap<BLO.MotorCoverResponse,
                RR.MotorCoverResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorCovers,
                RR.MotorCovers>().ReverseMap();

                cfg.CreateMap<BLO.MotorVehicleMaster,
               RR.MotorVehicleMaster>().ReverseMap();

                cfg.CreateMap<BLO.MotorVehicleMasterResponse,
               RR.MotorVehicleMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorProductMasterResponse,
              RR.MotorProductMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorProductMaster,
               RR.MotorProductMaster>().ReverseMap();


               cfg.CreateMap<BLO.MotorProductMaster,
               RR.MotorProductMaster>().ReverseMap();


               cfg.CreateMap<BLO.MotorEngineCCMaster,
               RR.MotorEngineCCMaster>().ReverseMap();

                cfg.CreateMap<BLO.MotorEngineCCResponse,
              RR.MotorEngineCCResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorYearMaster,
               RR.MotorYearMaster>().ReverseMap();

                cfg.CreateMap<BLO.MotorYearMasterResponse,
                RR.MotorYearMasterResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorClaim,
                RR.MotorClaim>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsementMaster, RR.MotorEndorsementMaster>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementMaster, RR.HomeEndorsementMaster>().ReverseMap();

                cfg.CreateMap<BLO.CategoryMaster, RR.CategoryMaster>().ReverseMap();

                cfg.CreateMap<BLO.RenewalPrecheckRequest, RR.RenewalPrecheckRequest>().ReverseMap();

                cfg.CreateMap<BLO.RenewalPrecheckResponse, RR.RenewalPrecheckResponse>().ReverseMap();

            });

            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetTravelAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.TravelInsurancePolicy,
                            RR.TravelInsurancePolicy>().ReverseMap();

                cfg.CreateMap<BLO.TravelInsuranceQuoteResponse,
                            RR.TravelInsuranceQuoteResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgencyTravelRequest,
                         RR.AgencyTravelRequest>().ReverseMap();

                cfg.CreateMap<BLO.AgencyTravelPolicy,
                         RR.AgencyTravelPolicy>().ReverseMap();

                cfg.CreateMap<BLO.AgencyTravelPolicyResponse,
                           RR.AgencyTravelPolicyResponse>().ReverseMap();

                cfg.CreateMap<BLO.TravelInsuranceQuote,
                           RR.TravelInsuranceQuote>().ReverseMap();

                cfg.CreateMap<BLO.TravelInsuranceQuoteResponse,
                          RR.TravelInsuranceQuoteResponse>().ReverseMap();

                cfg.CreateMap<BLO.TravelMembers,
                         RR.TravelMembers>().ReverseMap();

                cfg.CreateMap<BLO.TravelSavedQuotationResponse,
                         RR.TravelSavedQuotationResponse>().ReverseMap();

                cfg.CreateMap<BLO.InsuredMaster,
                        RR.InsuredMaster>().ReverseMap();
                

                cfg.CreateMap<BLO.TravelInsurancePolicy,
                        RR.TravelInsurancePolicy>().ReverseMap();

                cfg.CreateMap<BLO.UserAdderDetails,
                        RR.UserAdderDetails>().ReverseMap();

                cfg.CreateMap<BLO.UserDetails,
                        RR.UserDetails>().ReverseMap();               

            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetMotorAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.MotorInsuranceQuote,
                            RR.MotorInsuranceQuote>().ReverseMap();

                cfg.CreateMap<BLO.MotorInsuranceQuoteResponse,
                            RR.MotorInsuranceQuoteResponse>().ReverseMap();                

                cfg.CreateMap<BLO.MotorInsurance,
                            RR.MotorInsurance>().ReverseMap();                

                cfg.CreateMap<BLO.MotorInsurancePolicy,
                            RR.MotorInsurancePolicy>().ReverseMap();                

                cfg.CreateMap<BLO.MotorInsurancePolicyResponse,
                            RR.MotorInsurancePolicyResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorSavedQuotationResponse,
                            RR.MotorSavedQuotationResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgencyMotorRequest,
                          RR.AgencyMotorRequest>().ReverseMap();

                cfg.CreateMap<BLO.AgencyMotorPolicyResponse,
                         RR.AgencyMotorPolicyResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgencyMotorPolicyResponse,
                           RR.AgencyMotorPolicyResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorCovers,
                        RR.MotorCovers>().ReverseMap();

                cfg.CreateMap<BLO.InsuredMaster,
                         RR.InsuredMaster>().ReverseMap();

                cfg.CreateMap<BLO.ExcessAmountRequest,
                         RR.ExcessAmountRequest>().ReverseMap();

                cfg.CreateMap<BLO.ExcessAmountResponse,
                        RR.ExcessAmountResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgencyMotorPolicy,
                        RR.AgencyMotorPolicy>().ReverseMap();

                cfg.CreateMap<BLO.OptionalCoverRequest,
                        RR.OptionalCoverRequest>().ReverseMap();

                cfg.CreateMap<BLO.OptionalCoverResponse,
                       RR.OptionalCoverResponse>().ReverseMap();

                cfg.CreateMap<BLO.CalculateCoverAmountRequest,
                       RR.CalculateCoverAmountRequest>().ReverseMap();

                cfg.CreateMap<BLO.CalculateCoverAmountResponse,
                       RR.CalculateCoverAmountResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgencyPolicyDetailsRequest,
                       RR.AgencyPolicyDetailsRequest>().ReverseMap();

            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetMotorEndorsementAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.MotorEndorsementQuote,
                            RR.MotorEndorsementQuote>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsementQuoteResult,
                          RR.MotorEndorsementQuoteResult>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsement,
                          RR.MotorEndorsement>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsementPreCheckRequest,
                         RR.MotorEndorsementPreCheckRequest>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsementPreCheckResponse,
                       RR.MotorEndorsementPreCheckResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndoRequest,
                       RR.MotorEndoRequest>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndoResult,
                       RR.MotorEndoResult>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsementOperation,
                      RR.MotorEndorsementOperation>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsementOperationResponse,
                      RR.MotorEndorsementOperationResponse>().ReverseMap();

                cfg.CreateMap<BLO.MotorCovers,
                        RR.MotorCovers>().ReverseMap();

                cfg.CreateMap<BLO.MotorEndorsementResult,
                       RR.MotorEndorsementResult>().ReverseMap();

                
            });
            return (_mapper = _mapperConfig.CreateMapper());
        }
        public AutoMapper.IMapper GetTravelEndorsementAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.TravelEndorsementQuote,
                            RR.TravelEndorsementQuote>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndorsementQuoteResponse,
                          RR.TravelEndorsementQuoteResponse>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndorsement,
                          RR.TravelEndorsement>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndorsementPreCheckRequest,
                         RR.TravelEndorsementPreCheckRequest>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndorsementPreCheckResponse,
                       RR.TravelEndorsementPreCheckResponse>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndoRequest,
                       RR.TravelEndoRequest>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndoResponse,
                       RR.TravelEndoResponse>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndorsementOperation,
                      RR.TravelEndorsementOperation>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndorsementOperationResponse,
                      RR.TravelEndorsementOperationResponse>().ReverseMap();

                cfg.CreateMap<BLO.TravelMembers,
                     RR.TravelMembers>().ReverseMap();

                cfg.CreateMap<BLO.TravelEndorsementResponse,
                    RR.TravelEndorsementResponse>().ReverseMap();               

                
            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetHomeEndorsementAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.HomeEndorsementQuote,
                            RR.HomeEndorsementQuote>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementQuoteResponse,
                          RR.HomeEndorsementQuoteResponse>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsement,
                          RR.HomeEndorsement>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementPreCheckRequest,
                         RR.HomeEndorsementPreCheckRequest>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementPreCheckResponse,
                       RR.HomeEndorsementPreCheckResponse>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndoRequest,
                       RR.HomeEndoRequest>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndoResponse,
                       RR.HomeEndoResponse>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementOperation,
                      RR.HomeEndorsementOperation>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementOperationResponse,
                      RR.HomeEndorsementOperationResponse>().ReverseMap();

                cfg.CreateMap<BLO.HomeDomesticHelp,
                     RR.HomeDomesticHelp>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementDomesticHelpQuote,
                   RR.HomeEndorsementDomesticHelpQuote>().ReverseMap();

                cfg.CreateMap<BLO.HomeEndorsementResponse,
                  RR.HomeEndorsementResponse>().ReverseMap();             


            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetDomesticAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.DomesticHelpQuote,
                            RR.DomesticHelpQuote>().ReverseMap();
                cfg.CreateMap<BLO.DomesticHelpQuoteResponse,
                            RR.DomesticHelpQuoteResponse>().ReverseMap();
                cfg.CreateMap<BLO.DomesticPolicyDetails,
                            RR.DomesticPolicyDetails>().ReverseMap();
                cfg.CreateMap<BLO.DomesticHelpPolicyResponse,
                            RR.DomesticHelpPolicyResponse>().ReverseMap();
                cfg.CreateMap<BLO.AgencyDomesticRequest,
                          RR.AgencyDomesticRequest>().ReverseMap();
                cfg.CreateMap<BLO.AgencyDomesticPolicyResponse,
                         RR.AgencyDomesticPolicyResponse>().ReverseMap();
                cfg.CreateMap<BLO.DomesticHelpMember,
                         RR.DomesticHelpMember>().ReverseMap();
                cfg.CreateMap<BLO.AgencyDomesticPolicy,
                        RR.AgencyDomesticPolicy>().ReverseMap();
                cfg.CreateMap<BLO.DomesticHelpPolicy,
                       RR.DomesticHelpPolicy>().ReverseMap();
                cfg.CreateMap<BLO.DomesticHelpSavedQuotationResponse,
                       RR.DomesticHelpSavedQuotationResponse>().ReverseMap();

            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetReportAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.MotorReportDetails, RR.MotorReportDetails>().ReverseMap();

                cfg.CreateMap<BLO.AdminFetchReportRequest, RR.AdminFetchReportRequest>().ReverseMap();

                cfg.CreateMap<BLO.MotorReportResponse, RR.MotorReportResponse>().ReverseMap();

                cfg.CreateMap<BLO.TravelHomeReportDetails, RR.TravelHomeReportDetails>().ReverseMap();

                cfg.CreateMap<BLO.TravelHomeReportResponse, RR.TravelHomeReportResponse>().ReverseMap();

                cfg.CreateMap<BLO.MainReportDetails, RR.MainReportDetails>().ReverseMap();

                cfg.CreateMap<BLO.MainReportResponse, RR.MainReportResponse>().ReverseMap();

            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetHomeAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.HomeInsuranceQuote,
                              RR.HomeInsuranceQuote>().ReverseMap();

                cfg.CreateMap<BLO.HomeInsuranceQuoteResponse,
                              RR.HomeInsuranceQuoteResponse>().ReverseMap();

                cfg.CreateMap<BLO.HomeSavedQuotationResponse,
                              RR.HomeSavedQuotationResponse>().ReverseMap();

                cfg.CreateMap<BLO.HomeInsurancePolicyDetails,
                            RR.HomeInsurancePolicyDetails>().ReverseMap();

                cfg.CreateMap<BLO.HomeInsurancePolicyResponse,
                            RR.HomeInsurancePolicyResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgencyHomeRequest,
                       RR.AgencyHomeRequest>().ReverseMap();

                cfg.CreateMap<BLO.AgencyHomePolicyResponse,
                         RR.AgencyHomePolicyResponse>().ReverseMap();

                cfg.CreateMap<BLO.AgencyHomePolicy,
                           RR.AgencyHomePolicy>().ReverseMap();

                cfg.CreateMap<BLO.HomeInsurancePolicy,
                          RR.HomeInsurancePolicy>().ReverseMap();

                cfg.CreateMap<BLO.HomeDomesticHelp,
                          RR.HomeDomesticHelp>().ReverseMap();

                cfg.CreateMap<BLO.HomeSubItems,
                          RR.HomeSubItems>().ReverseMap();

            });
            return (_mapper = _mapperConfig.CreateMapper());
        }

        public AutoMapper.IMapper GetPortalAutoMapper()
        {
            _mapperConfig = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BLO.EmailMessageAuditResult,
                            RR.EmailMessageAuditResult>().ReverseMap();

                cfg.CreateMap<BLO.MotorPolicyDetails,
                               MotorPolicyDetails>().ReverseMap();

                cfg.CreateMap<BLO.HomeInsurancePolicy,
                            HomeInsurancePolicy>().ReverseMap();

                cfg.CreateMap<BLO.TravelInsurancePolicy,
                            TravelInsurancePolicy>().ReverseMap();

                cfg.CreateMap<BLO.TravelPolicyDetails,
                        RR.TravelPolicyDetails>().ReverseMap();

                cfg.CreateMap<BLO.DomesticHelpPolicy,
                            DomesticHelpPolicy>().ReverseMap();

                cfg.CreateMap<BLO.HomePolicyDetails,
                           RR.HomePolicyDetails>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchMotorDetailsResponse,
                          BLO.AdminFetchMotorDetailsResponse>().ReverseMap();

                cfg.CreateMap<RR.FetchUserDetailsResponse,
                           BLO.FetchUserDetailsResponse>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchMotorDetailsRequest,
                    BLO.AdminFetchMotorDetailsRequest>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchTravelDetailsRequest,
                    BLO.AdminFetchTravelDetailsRequest>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchTravelDetailsResponse,
                    BLO.AdminFetchTravelDetailsResponse>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchDomesticDetailsResponse,
                   BLO.AdminFetchDomesticDetailsResponse>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchDomesticDetailsRequest,
                  BLO.AdminFetchDomesticDetailsRequest>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchDomesticDetailsResponse,
               BLO.AdminFetchDomesticDetailsResponse>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchHomeDetailsRequest,
                        BLO.AdminFetchHomeDetailsRequest>().ReverseMap();

                cfg.CreateMap<RR.AdminFetchHomeDetailsResponse,
                     BLO.AdminFetchHomeDetailsResponse>().ReverseMap();

                cfg.CreateMap<RR.CommissionRequest,
                       BLO.CommissionRequest>().ReverseMap();

                cfg.CreateMap<RR.CommissionResponse,
                    BLO.CommissionResponse>().ReverseMap();


                cfg.CreateMap<RR.HomeCommissionRequest,
                      BLO.HomeCommissionRequest>().ReverseMap();

                cfg.CreateMap<RR.HomeCommissionResponse,
                    BLO.HomeCommissionResponse>().ReverseMap();

                cfg.CreateMap<BLO.HIR,
                    RR.HIR>().ReverseMap();

                cfg.CreateMap<BLO.Active,
                    RR.Active>().ReverseMap();

                cfg.CreateMap<BLO.DashboardResponse,
                     RR.DashboardResponse>().ReverseMap();

                cfg.CreateMap<BLO.UpdateHIRStatusRequest,
                    RR.UpdateHIRStatusRequest>().ReverseMap();

                cfg.CreateMap<BLO.UpdateHIRStatusResponse,
                             RR.UpdateHIRStatusResponse>().ReverseMap(); 

                 cfg.CreateMap<BLO.AdminFetchDomesticDetailsResponse,
                             RR.AdminFetchDomesticDetailsResponse>().ReverseMap();

                cfg.CreateMap<BLO.DomesticInsurancePolicyDetails,
                             RR.DomesticInsurancePolicyDetails>().ReverseMap();

                cfg.CreateMap<BLO.VatRequest,
                             RR.VatRequest>().ReverseMap();

                cfg.CreateMap<BLO.VatResponse,
                             RR.VatResponse>().ReverseMap();

                cfg.CreateMap<BLO.UpdateHIRRemarksRequest,
                            RR.UpdateHIRRemarksRequest>().ReverseMap();

                cfg.CreateMap<BLO.UpdateHIRRemarksResponse,
                             RR.UpdateHIRRemarksResponse>().ReverseMap();

                
            });

            return (_mapper = _mapperConfig.CreateMapper());
        }
    }
}