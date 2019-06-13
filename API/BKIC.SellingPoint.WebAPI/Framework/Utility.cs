using System.Security.Claims;
using System.Web.Http;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
//using BKIC.SellingPoint.DL.BL;

namespace BKIC.SellingPoint.WebAPI.Framework
{
    public class Utility
    {
        public static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        //public  IEnumerable<Claim> GetUserInformation()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    return claims;
        //}
    }
}