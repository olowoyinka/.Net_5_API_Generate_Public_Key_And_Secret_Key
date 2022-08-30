using System.ComponentModel.DataAnnotations;

namespace EncryptionExample.Model
{
    public class Food
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string SecretKey { get; set; }
    }
}