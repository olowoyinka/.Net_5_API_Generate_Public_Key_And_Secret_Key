using EncryptionExample.Model;

namespace EncryptionExample.Service
{
    public interface IFoodService
    {
        public Food GetFood(string name);
    }
}