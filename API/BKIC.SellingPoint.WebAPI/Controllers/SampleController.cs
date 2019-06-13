using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using KBIC.DTO;
using System.Web;

namespace WebApi.TokenBasedAuthentication.Controllers
{
    /// <summary>
    /// Sample
    /// </summary>
    public class SampleController : ApiController
    {
        /// <summary>
        /// Get Server Name
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/data/forall")]
        public IHttpActionResult Get()
        {
            return Ok("Now server time is: " + DateTime.Now.ToString());
        }
        
        [HttpGet]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello " + identity.Name);
        }
        
        [HttpGet]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));

        }

        [HttpGet]
        [Route("api/sample/getApiLogDetails")]
        public LogModel GetApiLogDetails()
        {
            var logModel = new LogModel();
            logModel.TimeStamp = DateTime.Now;
            logModel.CallerIp = HttpContext.Current.Request.UserHostAddress;
            logModel.CallerAgent = HttpContext.Current.Request.UserAgent;
            logModel.CalledUrl = HttpContext.Current.Request.Url.OriginalString;
            return logModel;
        }

        [HttpGet]
        [Route("api/sample/posts/{p}")]
        public string Post(string p)
        {
            return p;
        }

    }

    public class LogModel
    {
        public DateTime TimeStamp { get; set; }
        public string CallerIp { get; set; }
        public string CallerAgent { get; set; }
        public string CalledUrl { get; set; }
    }
}
