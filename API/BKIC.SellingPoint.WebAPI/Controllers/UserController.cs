using BKIC.SellingPoint.DL.BL.Repositories;
using BKIC.SellingPoint.WebAPI;
using BKIC.SellingPoint.WebAPI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using BLO = BKIC.SellingPoint.DL.BO;
using RR = BKIC.SellingPoint.DTO.RequestResponseWrappers;
using URI = BKIC.SellingPoint.DTO.Constants;

namespace BKIC.SellingPoint.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        public readonly IUser _userRepository;
        private readonly AutoMapper.IMapper _mapper;

        public UserController(IUser repository)
        {
            _userRepository = repository;
            BKICAutomapper automap = new BKICAutomapper();
            _mapper = automap.GetUserAutoMapper();
        }

        /// <summary>
        /// Insert the new user to the database.
        /// </summary>
        /// <param name="details">User details.</param>
        /// <returns>User inserted or not.</returns>
        [HttpPost]
        [Route(URI.UserURI.PostUserMaster)]
        //[ApiAuthorize]
        public RR.PostUserDetailsResult PostUserMasterDetails(RR.UserMaster userdetails)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;

                BLO.UserMaster userMaster = _mapper.Map<RR.UserMaster, BLO.UserMaster>(userdetails);
                BLO.PostUserDetailsResult result = _userRepository.InsertUserMasterDetails(userMaster);
                return _mapper.Map<BLO.PostUserDetailsResult, RR.PostUserDetailsResult>(result);
            }
            catch (Exception ex)
            {
                return new RR.PostUserDetailsResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Register the new user based the role.
        /// </summary>
        /// <param name="details">User details.</param>
        /// <returns></returns>
        [HttpPost]
        [Route(URI.UserURI.RegisterAdminUser)]
        //[ApiAuthorize]
        public RR.PostAdminUserResult RegisterAdminUser(RR.AdminRegister admindetails)
        {
            try
            {
                BLO.AdminRegister details = _mapper.Map<RR.AdminRegister, BLO.AdminRegister>(admindetails);
                BLO.PostAdminUserResult result = _userRepository.PostAdminUser(details);
                return _mapper.Map<BLO.PostAdminUserResult, RR.PostAdminUserResult>(result);
            }
            catch (Exception ex)
            {
                return new RR.PostAdminUserResult
                {
                    IsTransactionDone = false,
                    TransactionErrorMessage = ex.Message
                };
            }
        }
    }
}