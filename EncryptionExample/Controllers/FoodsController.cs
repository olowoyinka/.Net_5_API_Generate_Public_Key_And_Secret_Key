using EncryptionExample.ApiKey;
using EncryptionExample.Data;
using EncryptionExample.Model;
using EncryptionExample.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionExample.Controllers
{
    public class FoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("food")]
        [ApiKeyAuth]
        public IActionResult GetSecret()
        {
            return Ok("I have food");
        }


        [HttpPost("food")]
        public IActionResult PostSecret()
        {
            string PublicKey = Guid.NewGuid().ToString("N");

            var good = GeneratePassword(50, 20);

            var newfood = new Food
            {
                Name = PublicKey,
                SecretKey = EncryptionHMACSHA512(PublicKey, good)
            };
            _context.Foods.Add(newfood);
            _context.SaveChanges();

            return Ok(good);
        }

        private string GenerateToken()
        {
            StringBuilder builder = new StringBuilder();
            Random rstToken = new Random();

            char ch;
            for (int i = 0; i < 50; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * rstToken.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GeneratePassword(int Length, int NonAlphaNumericChars)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            string allowedNonAlphaNum = "!@#$%^&*()_-+=[{]};:<>|./?";
            string pass = "";
            Random rd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < Length; i++)
            {
                if (rd.Next(1) > 0 && NonAlphaNumericChars > 0)
                {
                    pass += allowedNonAlphaNum[rd.Next(allowedNonAlphaNum.Length)];
                    NonAlphaNumericChars--;
                }
                else
                {
                    pass += allowedChars[rd.Next(allowedChars.Length)];
                }
            }

            return $"test_sk_{pass}";
        }
        
        private static String EncryptionHMACSHA512(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA512 hash = new HMACSHA512(keyBytes)) hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}