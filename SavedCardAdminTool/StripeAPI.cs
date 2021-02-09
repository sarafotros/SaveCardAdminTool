using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace SavedCardAdminTool
{
    public class StripeAPI
    {
        private HttpClient _stripe;


        public StripeAPI()
        {
            var secretKey  = Environment.GetEnvironmentVariable("stripeApiKey");
            _stripe = new HttpClient()
            {
                BaseAddress = new Uri("https://api.stripe.com"),
            };
            _stripe.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
            
        }
        public async Task GetList()
        {
            var response = await _stripe.GetAsync("/v1/charges");
            var result = await response.Content.ReadAsStringAsync();
            // var testDeserialize = result.DeserializeObject<Payment>(result);
            Console.WriteLine(result);
        }
        
        public List<Payment> GetAllList()
        {
            var result = GetResponse(_stripe, "/v1/charges");
            return JsonConvert.DeserializeObject<List<Payment>>(result);
        }
        
        private string GetResponse(HttpClient client, string path)
        {
            var response =  client.GetAsync(path).GetAwaiter().GetResult();
            var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return result;
        }

        public async Task<StripePaymentMethod> CreateCard(string cardNumber, string expMonth, string expYear, string cvc)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>()
            {
                {"type", "card"},
                {"card[number]",cardNumber},
                {"card[exp_month]",expMonth},
                {"card[exp_year]", expYear},
                {"card[cvc]",cvc}
            };
            var response = await _stripe.SendAsync(new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(QueryHelpers.AddQueryString("https://api.stripe.com/v1/payment_methods", queryParams))
            });
            var stripeMethod = JsonConvert.DeserializeObject<StripePaymentMethod>(await response.Content.ReadAsStringAsync());
            return stripeMethod;
        }

        public StripePaymentMethod GetCard(string id)
        {
            var result = GetResponse(_stripe, $"https://api.stripe.com/v1/payment_methods/{id}");
            return  JsonConvert.DeserializeObject<StripePaymentMethod>(result);
        }
        
    }

    public class StripePaymentMethod
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("card")]
        public StripeCard Card { get; set; }
    }

    public class StripeCard
    {
        [JsonProperty("brand")] 
        public string Brand { get; set; }

        [JsonProperty("exp_month")]
        public string ExpMonth { get; set; }
        
        [JsonProperty("exp_year")]
        public string ExpYear { get; set; }
        
        [JsonProperty("last4")] 
        public string LastFour { get; set; }
    }

    public class Payment
    {
        public int Object { get; set; }
        public string Data { get; set; }
    }
    
}