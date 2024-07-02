//using BE.Entities;
using BE.Entities;
using BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BE.Services
{
    public class SpotGoldPriceService
    {  
        private readonly JewelrySystemDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly GoldApiSettings _settings;

        public SpotGoldPriceService(JewelrySystemDbContext context, HttpClient httpClient, IOptions<GoldApiSettings> settings)
        {
            _context = context;
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task UpdateSpotMetalPrice()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://www.goldapi.io/api/XAU/USD"),
            };

            request.Headers.Add("x-access-token", _settings.APIGoldKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                response.EnsureSuccessStatusCode();
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var goldData = JsonConvert.DeserializeObject<GoldApiResponse>(responseData);

            var goldPrice = new SpotGoldPrice
            {
                GoldType = "Gold",
                SpotPrice = goldData.Price,
                DateRecorded = DateTime.UtcNow
            };

            _context.SpotGoldPrices.Add(goldPrice);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SpotGoldPrice>> GetGoldPricesForYear2024()
        {
            var startDate = new DateTime(2024, 1, 1);
            var endDate = DateTime.UtcNow;

            return await _context.SpotGoldPrices
                .Where(p => p.DateRecorded >= startDate && p.DateRecorded <= endDate)
                .ToListAsync();
        }

        
        public async Task<decimal> GetGoldPriceAtTime()
        {
            var lastestPrice = await _context.SpotGoldPrices
                .Where(smp => smp.GoldType == "Gold")
                .OrderByDescending(smp => smp.DateRecorded)
                .FirstOrDefaultAsync();

            if (lastestPrice == null)
            {
                throw new InvalidOperationException("No gold price recorded!!!");
            }

            return lastestPrice.SpotPrice.Value;
        } 
    }
}
