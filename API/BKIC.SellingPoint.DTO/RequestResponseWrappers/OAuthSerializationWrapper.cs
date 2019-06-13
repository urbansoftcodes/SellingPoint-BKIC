using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class OAuthTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty(PropertyName = ".issued")]
        public DateTime IssuedDate { get; set; }
        [JsonProperty(PropertyName = ".expires")]
        public DateTime ExpiresDate { get; set; }
        [JsonProperty(PropertyName = "roles")]
        public string Roles { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "userId")]
        public string UserID { get; set; }
        [JsonProperty(PropertyName = "Id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "agentCode")]
        public string AgentCode { get; set; }
        [JsonProperty(PropertyName = "agentBranch")]
        public string AgentBranch { get; set; }
        [JsonProperty(PropertyName = "agency")]
        public string Agency { get; set; }
        [JsonProperty(PropertyName = "products")]
        public string Products { get; set; }
        [JsonProperty(PropertyName = "statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty(PropertyName = "errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }
        [JsonProperty(PropertyName = "motorProducts")]
        List<MotorProduct> MotorProducts { get; set; }
        [JsonProperty(PropertyName = "homeProducts")]
        List<HomeProduct> HomeProducts { get; set; }
        [JsonProperty(PropertyName = "isShowPayments")]
        public bool IsShowPayments{ get; set; }

    }

    public class OAuthRequest
    {
        [JsonProperty(PropertyName="username")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName="password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName="grant_type")]
        public string GrantType { get; set; }
    }

}
