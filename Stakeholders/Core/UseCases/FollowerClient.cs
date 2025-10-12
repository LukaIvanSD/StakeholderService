namespace Stakeholders.Core.UseCases
{
    public class FollowerClient
    {
        private readonly HttpClient _httpClient;

        public FollowerClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<bool> AddUserAsync(long userId)
        {
            try
            {
                var payload = new { userId = userId };
                var response = await _httpClient.PostAsJsonAsync("http://follower:8000/addUser", payload);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[FollowerClient] Failed with status: {response.StatusCode}");
                    return false;
                }

                Console.WriteLine("[FollowerClient] User successfully registered in follower service.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FollowerClient] Exception during request: {ex.Message}");
                return false;
            }
        }
    }
}