using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KBIC.Utility;

namespace UtilityTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //DataServiceManager service = new DataServiceManager("https://support.beyontec.com/",
            //    "", "beyontecadmin", "+mg2-VFm%L++@6&", true);

            //AccessTokenResponse accesstoken = service.PostData<AccessTokenResponse,string>("authserver/oauth/token?grant_type=password&username=remoteinvapi&password=9zE96GvqY", "");

            //var a = accesstoken;

        }
    }

    public class AccessTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }
    }

}
