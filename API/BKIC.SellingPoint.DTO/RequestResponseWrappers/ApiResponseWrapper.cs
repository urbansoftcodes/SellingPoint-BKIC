using Newtonsoft.Json;
using System;
using System.Net;

namespace BKIC.SellingPoint.DTO.RequestResponseWrappers
{
    public class ApiResponseWrapper<T>
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { get { return "1.0"; } set { } }

        [JsonProperty(PropertyName = "licensedBy")]
        public string LicensedBy { get { return "GIG BH - SellingPoint"; } set { } }

        [JsonProperty(PropertyName = "statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "requestUrl")]
        public Uri RequestUri { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "errorMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ErrorMessage { get; set; }

        [JsonProperty(PropertyName = "result", NullValueHandling = NullValueHandling.Ignore)]
        public T Result { get; set; }

        //public ApiResponseWrapper(Uri requestUrl, HttpStatusCode statusCode, string method, object result = null, string errorMessage = null)
        //{
        //    RequestUri = requestUrl;
        //    StatusCode = (int)statusCode;
        //    Result = result;
        //    ErrorMessage = errorMessage;
        //    Method = method;
        //}
    }
}