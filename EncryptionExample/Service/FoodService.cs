using EncryptionExample.Data;
using EncryptionExample.Model;
using System;
using System.Linq;

namespace EncryptionExample.Service
{
    public class FoodService : IFoodService
    {
        private readonly ApplicationDbContext _context;

        public FoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Food GetFood(string name)
        {
            return _context.Foods.Where(s => s.Name == name).FirstOrDefault();
        }
    }
}
