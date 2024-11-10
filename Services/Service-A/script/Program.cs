using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Timer = System.Timers.Timer;
using ElapsedEventArgs = System.Timers.ElapsedEventArgs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    private static Timer timer;
    private static List<double> values = new List<double>();
    private static double currentPrice = 0;
    private static double averagePrice = 0;
    private static readonly string ApiUrl = Environment.GetEnvironmentVariable("API_URL") ?? "https://api.coindesk.com/v1/bpi/currentprice/BTC.json";
    private static readonly int CheckInterval = int.Parse(Environment.GetEnvironmentVariable("CHECK_INTERVAL") ?? "60000");
    private static readonly int AverageWindow = int.Parse(Environment.GetEnvironmentVariable("AVERAGE_WINDOW") ?? "600000");

    static async Task Main(string[] args)
    {
        // Start the Bitcoin price checker
        StartBitcoinChecker();

        // Start the web server
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // Main endpoint showing both current price and average
        app.MapGet("/", () => 
        {
            return $"Current Bitcoin Price: ${currentPrice:F2}\nAverage Bitcoin Price (10 min): ${averagePrice:F2}";
        });
        app.MapGet("/health", () => "Healthy");

        await app.RunAsync("http://0.0.0.0:80");
    }

    private static void StartBitcoinChecker()
    {
        timer = new Timer(CheckInterval);
        timer.Elapsed += OnTimerElapsed;
        timer.Start();

        // Initial price fetch
        GetBitcoinValue().Wait();

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(AverageWindow);
                CalculateAverageValue();
            }
        });
    }

    private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        GetBitcoinValue().Wait();
    }

    private static async Task GetBitcoinValue()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                using (JsonDocument document = JsonDocument.Parse(responseBody))
                {
                    double value = document.RootElement
                        .GetProperty("bpi")
                        .GetProperty("USD")
                        .GetProperty("rate_float")
                        .GetDouble();
                   
                    Console.WriteLine($"Current Bitcoin value: ${value}");
                    currentPrice = value;  // Update current price
                    values.Add(value);
                    if (values.Count > 10)
                    {
                        values.RemoveAt(0);
                    }
                    CalculateAverageValue();  // Calculate average whenever we get a new price
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting Bitcoin value: {ex.Message}");
        }
    }

    private static void CalculateAverageValue()
    {
        if (values.Count > 0)
        {
            averagePrice = values.Average();
            Console.WriteLine($"Average Bitcoin value: ${averagePrice}");
        }
    }
}