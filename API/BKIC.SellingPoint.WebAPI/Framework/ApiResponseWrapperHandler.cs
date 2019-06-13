using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BKIC.SellingPoint.WebAPI.Framework
{
    public class ApiResponseWrapperHandler : DelegatingHandler
    {
        public static string sgTest = ConfigurationManager.AppSettings["SGTest"].ToString();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        private static HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            string errorMessage = null;
            int statusCode = 0;

            if (request.RequestUri.ToString().Contains("getcmsfile")
                || request.RequestUri.ToString().Contains("insuranceportal/downloadtempfile") ||
                request.RequestUri.ToString().Contains("insuranceportal/uploadtempfile")
                || request.RequestUri.ToString().Contains("insuranceportal/deletetempfile")
                || request.RequestUri.ToString().Contains("insuranceportal/downloadinsuranceportalfile")
                || request.RequestUri.ToString().Contains("insuranceportal/deletealltempfiles")
                || request.RequestUri.ToString().Contains("insuranceportal/downloadschedule")
                || request.RequestUri.ToString().Contains("schedule/downloadschedule")
                || request.RequestUri.ToString().Contains("schedule/downloadproposal")
                || request.RequestUri.ToString().Contains("claims/claimsuploadtempfile")
                || request.RequestUri.ToString().Contains("claims/claimsdeletetempfile")
                || request.RequestUri.ToString().Contains("claims/claimsdeletealltempfile")
                || request.RequestUri.ToString().Contains("claims/claimsdownloadtempfile")
                || request.RequestUri.ToString().Contains("motorinsurance/fetchmotorcertificate")
                || request.RequestUri.ToString().Contains("admin/getagencycpr")
                )
            {
                //  SKIP you global logic
                return response;
            }

            if (sgTest == "1")
            {
                if (request.RequestUri.ToString().Contains("swagger"))
                {
                    return response;
                }
            }

            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;

                if (error != null)
                {
                    content = null;
                    errorMessage = error.Message;

#if DEBUG
                    errorMessage = string.Concat(errorMessage, error.ExceptionMessage, error.StackTrace);
#endif
                }
            }

            statusCode = (int)response.StatusCode;

            if (statusCode != 200)
            {
                errorMessage = response.ReasonPhrase;
            }

            if (sgTest != "1")
            {
                // for debug
                content = SetTrasactionCustomMessage(content);
            }

            var newResponse = request.CreateResponse(response.StatusCode, new ApiResponseWrapper(request.RequestUri, response.StatusCode, request.Method.Method, content, errorMessage));

            foreach (var header in response.Headers)
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }

        public static object SetTrasactionCustomMessage(object content)
        {
            try
            {
                var transactionErrorMessage = content.GetType().GetProperty("TransactionErrorMessage").GetValue(content).ToString();

                if (!string.IsNullOrEmpty(transactionErrorMessage))
                {
                    content.GetType().GetProperty("TransactionErrorMessage").SetValue(content, "Transaction Failed.", null);
                }

                return content;
            }
            catch (Exception exc)
            {
                return content;
            }
        }
    }
}