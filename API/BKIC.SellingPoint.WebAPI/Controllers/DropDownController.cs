using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.DTO.Constants;
using BKIC.SellingPoint.WebAPI.Framework;
using BKIC.SellingPont.DTO.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;

namespace BKIC.SellingPoint.WebAPI.Controllers
{
    public class DropDownController : ApiController
    {
        public readonly IDropDowns _dropdownRepository;
        private readonly AutoMapper.IMapper _mapper;

        public DropDownController(IDropDowns repository)
        {
            _dropdownRepository = repository;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetAdminAutoMapper();
        }

        /// <summary>
        /// Get dropdowns data for the page needed.
        /// </summary>
        /// <param name="type">Page type(it is motor or travel).</param>
        /// <returns>Dropdown data.</returns>
        [HttpGet]
        [Route(DropdownURI.GetPageDropDowns)]
        public RR.FetchDropDownsResponse GetPageDropDowns(string type)
        {
            var result = _dropdownRepository.FetchDropDown(type);

            return new RR.FetchDropDownsResponse()
            {
                IsTransactionDone = result.IsTransactionDone,
                TransactionErrorMessage = result.TransactionErrorMessage,
                dropdownresult = result.dropdownds.DataSetToJSON()
            };
        }

        /// <summary>
        /// Get the vehicle model's based on the make.
        /// e.g for make (BMW) fetch all the models that make 318i,320,535I..
        /// </summary>
        /// <param name="vehicleMake">vehicle make</param>
        /// <returns>vehicle model's</returns>
        [HttpGet]
        [Route(DropDownURI.GetVehicleModel)]
        public RR.VehicleModelResponse GetVehicleModel(string vehicleMake)
        {
            var result = _dropdownRepository.GetVehicleModel(vehicleMake);
            return new RR.VehicleModelResponse()
            {
                IsTransactionDone = true,
                TransactionErrorMessage = result.TransactionErrorMessage,
                VehicleModeldt = result.VehicleModeldt.DataTableToJSON()
            };

            return null;
        }

        /// <summary>
        /// Get vehicle body type for the particular make and model.
        /// </summary>
        /// <param name="request">Vehicle body request.</param>
        /// <returns>Vehicle body response.</returns>
        [HttpPost]
        [Route(DropDownURI.GetVehicleBody)]
        public RR.VehicleBodyResponse GetVehicleBody(RR.MotorBodyRequest request)
        {
            var result = _dropdownRepository.GetVehicleBody(request.VehicleMake, request.VehicleModel);
            return new RR.VehicleBodyResponse()
            {
                IsTransactionDone = true,
                TransactionErrorMessage = result.TransactionErrorMessage,
                VehicleBodydt = result.VehicleBodydt.DataTableToJSON(),
                VehicleEngineCCdt = result.VehicleEngineCCdt.DataTableToJSON()
            };

            return null;
        }

        /// <summary>
        /// Get agency product's based on the insurance type.
        /// e.g for the agency SECURA and insurance type MOTOR fetch the product's (Product 1 and Prodcut 2)
        /// for the agency TISCO and insurance type MOTOR fetch the product's (GLD,NMC,PLT etc.)
        /// </summary>
        /// <param name="agency">Agency.</param>
        /// <param name="agencyCode">Agent code.</param>
        /// <param name="mainclass">MainClass(e.g MOTOR product's for SECURA -SECUR for TISCO - TISCO)</param>
        /// <param name="type">Insurance type.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(DropDownURI.GetAgencyProducts)]
        public RR.FetchDropDownsResponse GetAgencyProducts(string agency, string agencycode, string mainclass, string page)
        {
            var result = _dropdownRepository.FetchAgencyProducts(agency, agencycode, mainclass, page);
            return new RR.FetchDropDownsResponse()
            {
                IsTransactionDone = result.IsTransactionDone,
                TransactionErrorMessage = result.TransactionErrorMessage,
                dropdownresult = result.dropdownds.DataSetToJSON()
            };

            return null;
        }

        /// <summary>
        /// Fetch insurance product codes.
        /// e.g for the insurance type domestichelp MAINCLASS is PADN.
        /// </summary>
        /// <param name="agency">Agency.</param>
        /// <param name="agencyCode">Agent Code.</param>
        /// <param name="insuranceTypeID">Insurance TypeID(1-domesticHelp, 2-Travel)</param>
        /// <returns></returns>
        [HttpGet]
        [Route(DropDownURI.GetInsuranceProductCode)]
        public RR.FetchProductCodeResponse GetInsuranceProductCode(string agency, string agencyCode, string insuranceTypeID)
        {
            var result = _dropdownRepository.FetchInsuranceProductCode(agency, agencyCode, Convert.ToInt32(insuranceTypeID));
            if (!string.IsNullOrEmpty(result))
            {
                return new RR.FetchProductCodeResponse
                {
                    IsTransactionDone = true,
                    productCode = result
                };
            }
            return null;
        }
    }
}