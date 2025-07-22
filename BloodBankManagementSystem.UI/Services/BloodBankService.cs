using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using BloodBankManagementSystem.Core.Models;

namespace BloodBankManagementSystem.UI.Services
{
    public class PincodeAddressService
    {
        private readonly HttpClient _httpClient;

        public PincodeAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PostOffice?> GetAddressFromPinCodeAsync(string pinCode)
        {
            if (!string.IsNullOrWhiteSpace(pinCode) && pinCode.Length == 6)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<List<PincodeApiRoot>>($"https://api.postalpincode.in/pincode/{pinCode}");

                    if (response != null && response.Count > 0 && response[0].Status == "Success")
                    {
                        return response[0].PostOffice?.FirstOrDefault();
                    }
                }
                catch
                {
                    // Handle/log exception if needed
                }
            }

            return null;
        }

        public async Task<List<PostOffice>> GetAllAddressesFromPinCodeAsync(string pinCode)
        {
            if (!string.IsNullOrWhiteSpace(pinCode) && pinCode.Length == 6)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<List<PincodeApiRoot>>($"https://api.postalpincode.in/pincode/{pinCode}");

                    if (response != null && response.Count > 0 && response[0].Status == "Success")
                    {
                        return response[0].PostOffice ?? new List<PostOffice>();
                    }
                }
                catch
                {
                    // Handle/log exception if needed
                }
            }

            return new List<PostOffice>();
        }
    }
}
