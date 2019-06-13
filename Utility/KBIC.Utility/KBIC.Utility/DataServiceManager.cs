using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json.Converters;

namespace KBIC.Utility
{
    public class DataServiceManager
    {
        public readonly string WebApiPath;
        public readonly string AccessToken;
        public readonly bool IsLoginAction;
        
        public DataServiceManager(string pWebApiUri, string pAccessToken, bool pIsLogin = false)
        {
            IsLoginAction = pIsLogin;
            WebApiPath = pWebApiUri;
            AccessToken = pAccessToken;
        }

        /// <summary>
        /// Make put request to service and return reference object
        /// </summary>
        /// <typeparam name="T">Return type of service</typeparam>
        /// <param name="method">Name of the rest method</param>
        /// <returns></returns>
        public T GetData<T>(string method) where T : class
        {
            using (var proxy = new BKICWebClient())
            {
                try
                {
                    SetRequestHeaders(proxy);

                    var data = proxy.GetAsync(new Uri(WebApiPath + method));
                    data.Wait();
                    HttpResponseMessage result = data.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var response = JsonConvert.DeserializeObject<T>(result.Content.ReadAsStringAsync().Result.ToString());

                        return response ?? (T)Activator.CreateInstance(typeof(T));
                    }
                    else
                    {
                        return BindBadRequestDetails<T>(result);
                    }
                }
                catch (Exception exc)
                {
                    return BindRequestExceptionDetails<T>(exc);
                }
            }
        }


        /// <summary>
        /// Post values to service and return reference type 
        /// </summary>
        /// <typeparam name="T1"> Return object type of service</typeparam>
        /// <typeparam name="T2"> Input object type of service</typeparam>
        /// <param name="method"> Name of the rest method</param>
        /// <param name="t2"> Object needs to be posted to service</param>
        /// <returns></returns>
        public T1 PostData<T1, T2>(string method, T2 t2) where T1 : class
        {
            using (var proxy = new BKICWebClient())
            {
                try
                {
                    SetRequestHeaders(proxy);

                    var serializedObject = JsonConvert.SerializeObject(t2);
                    var mediaType = "";
                    HttpContent postContent;

                    if (IsLoginAction)
                    {
                        var tokenSecurityInfo = t2 as BKIC.SellingPoint.DTO.RequestResponseWrappers.OAuthRequest;
                        var login = new Dictionary<string, string>
                        {
                            {"grant_type", "password"},
                            {"username", tokenSecurityInfo.UserName},
                            {"password", tokenSecurityInfo.Password},
                        };

                        postContent = new FormUrlEncodedContent(login);
                    }
                    else
                    {
                        mediaType = "application/json";
                        postContent = new StringContent(serializedObject.ToString(), System.Text.Encoding.UTF8, mediaType);
                    }                    

                    //mediaType = "application/json";
                    //postContent = new StringContent(serializedObject.ToString(), System.Text.Encoding.UTF8, mediaType);

                    var proxyPost = proxy.PostAsync(new Uri(WebApiPath + method), postContent);
                    proxyPost.Wait();
                    HttpResponseMessage result = proxyPost.Result;

                    if (result.IsSuccessStatusCode)
                    {

                        var response = JsonConvert.DeserializeObject<T1>(result.Content.ReadAsStringAsync().Result);

                        return response ?? (T1)Activator.CreateInstance(typeof(T1));
                    }
                    else
                    {
                        return BindBadRequestDetails<T1>(result);
                    }
                }
                catch (Exception exc)
                {
                    return BindRequestExceptionDetails<T1>(exc);
                }
            }
        }

        public T1 PostDataWithNormalDate<T1, T2>(string method, T2 t2) where T1 : class
        {
            using (var proxy = new BKICWebClient())
            {
                try
                {
                    SetRequestHeaders(proxy);

                    var serializedObject = JsonConvert.SerializeObject(t2, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
                    var mediaType = "";
                    HttpContent postContent;
                    
                    
                    mediaType = "application/json";
                    postContent = new StringContent(serializedObject.ToString(), System.Text.Encoding.UTF8, mediaType);

                    var proxyPost = proxy.PostAsync(new Uri(WebApiPath + method), postContent);
                    proxyPost.Wait();
                    HttpResponseMessage result = proxyPost.Result;

                    if (result.IsSuccessStatusCode)
                    {

                        var response = JsonConvert.DeserializeObject<T1>(result.Content.ReadAsStringAsync().Result, new JsonSerializerSettings() {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        return response ?? (T1)Activator.CreateInstance(typeof(T1));
                    }
                    else
                    {
                        return BindBadRequestDetails<T1>(result);
                    }
                }
                catch (Exception exc)
                {
                    return BindRequestExceptionDetails<T1>(exc);
                }
            }
        }

        public T UploadFile<T>(string method, string filePath,string accessToken="") where T : class
        {
            using (var proxy = new BKICWebClient())
            {
                proxy.MaxResponseContentBufferSize = int.MaxValue;
                proxy.DefaultRequestHeaders.Accept.Clear();

                if(!string.IsNullOrEmpty(accessToken))
                {
                    proxy.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }

                try
                {
                    string fileName = GetFileName(filePath);
                    FileStream fileStream = GetFileStream(filePath);
                    HttpContent bytesContent = new StreamContent(fileStream);
                    using (var client = new HttpClient())
                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(bytesContent, "uploadfile", fileName);
                        var response = client.PostAsync(new Uri(WebApiPath + method), formData).Result;
                        if (!response.IsSuccessStatusCode)
                        {
                            return BindBadRequestDetails<T>(response);
                        }

                        var result = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result.ToString());

                        return result ?? (T)Activator.CreateInstance(typeof(T));
                    }
                }
                catch(Exception exc)
                {
                    return BindRequestExceptionDetails<T>(exc);
                }
            }
                
        }

        private void SetRequestHeaders(HttpClient proxy)
        {
            proxy.MaxResponseContentBufferSize = int.MaxValue;
            proxy.DefaultRequestHeaders.Accept.Clear();
            proxy.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!IsLoginAction)
            {
                proxy.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);
            }

        }

        private T1 BindBadRequestDetails<T1>(HttpResponseMessage result) where T1 : class
        {
            var response = new BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper<string>();
            response.StatusCode = 400;
            response.ErrorMessage = result.ReasonPhrase;
            object genericRespone = (T1)Activator.CreateInstance(typeof(T1));
            var statusCode = genericRespone.GetType().GetProperty("StatusCode");
            var errorMessage = genericRespone.GetType().GetProperty("ErrorMessage");

            if (statusCode != null)
            {
                statusCode.SetValue(genericRespone, response.StatusCode);
            }
            if (statusCode != null)
            {
                errorMessage.SetValue(genericRespone, response.ErrorMessage);
            }

            return (T1)(Object)genericRespone;
        }

        private T1 BindRequestExceptionDetails<T1>(Exception exc)
        {
            var response = new BKIC.SellingPoint.DTO.RequestResponseWrappers.ApiResponseWrapper<string>();
            response.StatusCode = 404;
            response.ErrorMessage = exc.InnerException != null ? (exc.InnerException.InnerException != null ? exc.InnerException.InnerException.Message : exc.InnerException.Message) : exc.Message;
            object genericRespone = (T1)Activator.CreateInstance(typeof(T1));
            var statusCode = genericRespone.GetType().GetProperty("StatusCode");
            var errorMessage = genericRespone.GetType().GetProperty("ErrorMessage");

            if (statusCode != null)
            {
                statusCode.SetValue(genericRespone, response.StatusCode);
            }
            if (statusCode != null)
            {
                errorMessage.SetValue(genericRespone, response.ErrorMessage);
            }

            return (T1)(Object)genericRespone;
        }

        /// <summary>
        /// Override GetWebRequest for set Timeout
        /// </summary>
        private class BKICWebClient : HttpClient
        {
        }
        
        public byte[] GetFileBytes(string filePath)
        {
            FileStream stream = File.OpenRead(filePath);
            byte[] fileBytes = new byte[stream.Length];
            return fileBytes;
        }

        public FileStream GetFileStream(string filePath)
        {
            FileStream stream = File.OpenRead(filePath);
            return stream;
        }

        public string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        public string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

    }


}
