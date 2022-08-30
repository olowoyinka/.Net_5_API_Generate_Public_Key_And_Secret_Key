using System.ComponentModel.DataAnnotations;

namespace EncryptionExample.ViewModels
{
    public class FoodRequest
    {
        [Required]
        public string PublicKey { get; set; }

        [Required]
        public string SecretKey { get; set; }
    }
}