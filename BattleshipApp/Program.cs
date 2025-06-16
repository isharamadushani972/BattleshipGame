using System.Net.Http.Json;

class Program
{
    private const string ActualGridEndpoint = "api/Grid/actualgrid";
    private const string UserGridEndpoint = "api/Grid/usergrid";
    private const string GridEndpoint = "api/Grid";
    private const string BaseUrl = "https://localhost:7258/";

    static async Task Main()
    {
        using var httpClient = new HttpClient
        {
            BaseAddress = new Uri(BaseUrl)
        };

        while (true)
        {
            await GetUserGrid(httpClient);
            Console.WriteLine("Enter your move (e.g., A5) or type 'exit' to quit:");
            string? userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("No input provided.");
                continue;
            }

            if (userInput.Trim().ToLower() == "exit")
                break;

            await GetUserInputStatus(httpClient, userInput.Trim().ToUpper());

        }
        await GetGrid(httpClient);
        Console.WriteLine("Game ended.");
    }

    public static async Task GetGrid(HttpClient httpClient)
    {
        try
        {
            var response = await httpClient.GetAsync(ActualGridEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling API: {ex.Message}");
        }

        Console.WriteLine();
    }

    public static async Task GetUserInputStatus(HttpClient httpClient, string input)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(GridEndpoint, input);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling API: {ex.Message}");
        }

        Console.WriteLine();
    }
    public static async Task GetUserGrid(HttpClient httpClient)
    {
        try
        {
            var response = await httpClient.GetAsync(UserGridEndpoint);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine(error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error calling API: {ex.Message}");
        }

        Console.WriteLine();
    }

}
