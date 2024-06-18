using BE.Entities;
using BE.Models;
using Newtonsoft.Json;

namespace BE.Services
{
    public class SpotMetalPriceService
    {
        private readonly JewelrySystemDbContext _context;
        private readonly HttpClient _httpClient;

        public SpotMetalPriceService (JewelrySystemDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }   

        public async Task UpdateSpotMetalPrice()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync(); 
            var goldData = JsonConvert.DeserializeObject<GoldApiResponse>(responseData);

            var goldPrice = new SpotMetalPrice
            {
                MetalType = goldData.MetalType,
                SpotPrice = goldData.Price,
                DateRecorded = goldData.DateCreated
            };

            _context.SpotMetalPrices.Add(goldPrice);
            await _context.SaveChangesAsync();
        }
    }
}
