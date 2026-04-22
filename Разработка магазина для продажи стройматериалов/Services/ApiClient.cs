using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Magaz_Stroitelya.DTO.Auth;
using MVVM.Model.DTO.Response;

namespace Magaz_Stroitelya.Services
{
    public class ApiClient
    {
        private readonly HttpClient client;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClient()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5002")
            };
        }

        //Auth
        public void SetAccessToken(string? accessToken)
        {
            client.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(accessToken)
                ? null
                : new AuthenticationHeaderValue("Bearer", accessToken);
        }
        public async Task<(LoginResponse? Data, string? Error)> LoginAsync(string login, string password) =>
            await PostAuthJsonAsync<LoginRequest, LoginResponse>("/api/Auth/login", new LoginRequest { Login = login, Password = password });
        public async Task<(LoginResponse? Data, string? Error)> RegisterAsync(LoginRequest request) =>
            await PostAuthJsonAsync<LoginRequest, LoginResponse>("/api/Auth/register", request);
        public async Task<(UserResponse? Data, string? Error)> GetMe(int id) =>
            await GetAsync<UserResponse>($"/api/Auth/me/{id}");

        //Order
        public async Task<(IReadOnlyCollection<OrderResponse> Data, string? Error)> GetListOrder() =>
            await GetListAsync<OrderResponse>($"/api/Order");
        public async Task<(OrderResponse? Data, string? Error)> GetOrder(int id) =>
            await GetAsync<OrderResponse>($"/api/Order/{id}");
        public async Task<string?> PostOrder(OrderResponse request) =>
            await PostJsonAsync("/api/Order", request);
        public async Task<string?> PatchOrder(int id, OrderResponse request) =>
            await PatchJsonAsync($"/api/Order/{id}", request);
        public async Task<string?> DeleteOrder(int id) =>
            await DeleteAsync($"/api/Order/{id}");

        //OrderStructure
        public async Task<(IReadOnlyCollection<OrderStructureResponse> Data, string? Error)> GetListOrderStructure() =>
            await GetListAsync<OrderStructureResponse>($"/api/OrderStructure");

        //Parameter
        public async Task<(IReadOnlyCollection<ParameterResponse> Data, string? Error)> GetListParameter() =>
            await GetListAsync<ParameterResponse>($"/api/Parameter");
        public async Task<(ParameterResponse? Data, string? Error)> GetParameter(int id) =>
            await GetAsync<ParameterResponse>($"/api/Parameter/{id}");
        public async Task<string?> PostParameter(ParameterResponse request) =>
            await PostJsonAsync("/api/Parameter", request);
        public async Task<string?> PatchParameter(int id, ParameterResponse request) =>
            await PatchJsonAsync($"/api/Parameter/{id}", request);
        public async Task<string?> DeleteParameter(int id) =>
            await DeleteAsync($"/api/Parameter/{id}");

        //Product
        public async Task<(IReadOnlyCollection<ProductResponse> Data, string? Error)> GetListProduct() =>
            await GetListAsync<ProductResponse>($"/api/Product");
        public async Task<(ProductResponse? Data, string? Error)> GetProduct(int id) =>
            await GetAsync<ProductResponse>($"/api/Product/{id}");
        public async Task<string?> PostProduct(ProductRequest request) =>
            await PostJsonAsync("/api/Product", request);
        public async Task<string?> PatchProduct(int id, ProductRequest request) =>
            await PatchJsonAsync($"/api/Product/{id}", request);
        public async Task<string?> DeleteProduct(int id) =>
            await DeleteAsync($"/api/Product/{id}");

        //ProductParameter
        public async Task<(IReadOnlyCollection<ProductParameterResponse> Data, string? Error)> GetListProductParameter() =>
            await GetListAsync<ProductParameterResponse>($"/api/ProductParameter");
        public async Task<(ProductParameterResponse? Data, string? Error)> GetProductParameter(int id) =>
            await GetAsync<ProductParameterResponse>($"/api/ProductParameter/{id}");
        public async Task<string?> PostProductParameter(ProductParameterResponse request) =>
            await PostJsonAsync("/api/ProductParameter", request);
        public async Task<string?> PatchProductParameter(int id, ProductParameterResponse request) =>
            await PatchJsonAsync($"/api/ProductParameter/{id}", request);
        public async Task<string?> DeleteProductParameter(int id) =>
            await DeleteAsync($"/api/ProductParameter/{id}");

        //ProductType
        public async Task<(IReadOnlyCollection<ProductTypeResponse> Data, string? Error)> GetListProductType() =>
            await GetListAsync<ProductTypeResponse>($"/api/ProductType");
        public async Task<(ProductTypeResponse? Data, string? Error)> GetProductType(int id) =>
            await GetAsync<ProductTypeResponse>($"/api/ProductType/{id}");
        public async Task<string?> PostProductType(ProductTypeResponse request) =>
            await PostJsonAsync("/api/ProductType", request);
        public async Task<string?> PatchProductType(int id, ProductTypeResponse request) =>
            await PatchJsonAsync($"/api/ProductType/{id}", request);
        public async Task<string?> DeleteProductType(int id) =>
            await DeleteAsync($"/api/ProductType/{id}");




        //Other
        private async Task<(IReadOnlyList<T> Data, string? Error)> GetListAsync<T>(string url)
        {
            var (data, error) = await GetAsync<List<T>>(url);
            IReadOnlyList<T> result = data ?? new List<T>();
            return (result, error);
        }
        private async Task<(TResponse? Data, string? Error)> GetAsync<TResponse>(string url)
        {
            try
            {
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return (default, await ReadErrorAsync(response));

                var model = await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
                return model is null ? (default, "Пустой ответ от API") : (model, null);
            }
            catch (Exception ex)
            {
                return (default, ex.Message);
            }
        }
        private async Task<string?> PostJsonAsync<TRequest>(string url, TRequest payload)
        {
            try
            {
                var response = await client.PostAsJsonAsync(url, payload);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return error;
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task<(TResponse? Data, string? Error)> PostAuthJsonAsync<TRequest, TResponse>(string url, TRequest payload)
        {
            try
            {
                var response = await client.PostAsJsonAsync(url, payload);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return (default, error);
                }

                var model = await response.Content.ReadFromJsonAsync<TResponse>(JsonOptions);
                return model is null ? (default, "Пустой ответ от API") : (model, null);
            }
            catch (Exception ex)
            {
                return (default, ex.Message);
            }
        }
        private async Task<string?> PatchJsonAsync<TRequest>(string url, TRequest payload)
        {
            try
            {
                var response = await client.PatchAsJsonAsync(url, payload);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return error;
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private async Task<string?> DeleteAsync(string url)
        {
            try
            {
                var response = await client.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await ReadErrorAsync(response);
                    return error;
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
        {
            var text = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(text))
                return $"HTTP {(int)response.StatusCode}";

            try
            {
                var apiError = JsonSerializer.Deserialize<ApiErrorResponse>(text, JsonOptions);
                if (apiError is null)
                    return text;

                if (apiError.Errors is { Count: > 0 })
                {
                    var first = apiError.Errors.FirstOrDefault().Value?.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(first))
                        return first;
                }

                return string.IsNullOrWhiteSpace(apiError.Message) ? text : apiError.Message;
            }
            catch
            {
                return text;
            }
        }


        private sealed class ApiErrorResponse
        {
            public string Message { get; set; } = string.Empty;
            public Dictionary<string, string[]>? Errors { get; set; }
        }
    }
}
