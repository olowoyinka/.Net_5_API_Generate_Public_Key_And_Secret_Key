using EncryptionExample.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionExample.ApiKey
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string PublicKeyHeaderName = "publicKey";
        private const string SecretKeyHeaderName = "secretKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(PublicKeyHeaderName, out var potentialPublicKey))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    status = "401",
                    message = "Invalid access token"
                });
                return;
            }
            
            if (!context.HttpContext.Request.Headers.TryGetValue(SecretKeyHeaderName, out var potentialSecretKey))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    status = "401",
                    message = "Invalid access token"
                });
                return;
            }

            var dbService = context.HttpContext.RequestServices.GetRequiredService<IFoodService>();

            var findPublicKey = dbService.GetFood(potentialPublicKey);

            if (findPublicKey == null)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    status = "401",
                    message = "Invalid access token"
                });
                return;
            }

            var decrypt = EncryptionHMACSHA512(potentialPublicKey, potentialSecretKey);

            if (!findPublicKey.SecretKey.Equals(decrypt))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    status = "401",
                    message = "Invalid access token"
                });
                return;
            }

            await next();
        }

        private static String EncryptionHMACSHA512(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);
            Byte[] hashBytes;

            using (HMACSHA512 hash = new HMACSHA512(keyBytes))hashBytes = hash.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}