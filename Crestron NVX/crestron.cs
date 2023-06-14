using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CrestronNvxDisplayInfo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Prompt the user to enter the IP address, username, and password
            Console.Write("Enter the IP address: ");
            string ipAddress = Console.ReadLine();

            Console.Write("Enter the username: ");
            string username = Console.ReadLine();

            Console.Write("Enter the password: ");
            string password = Console.ReadLine();

            using (HttpClient client = new HttpClient())
            {
                // Set the base URL of the Crestron NVX device
                client.BaseAddress = new Uri($"https://{ipAddress}/Device/AudioVideoInputOutput/");

                // Create an instance of the StringContent class to send the username, password, and IP address
                var credentials = new StringContent($"username={username}&password={password}&ip={ipAddress}");

                // Send a POST request to the Crestron NVX device to authenticate the user
                var response = await client.PostAsync("login", credentials);

                // Check if the authentication was successful
                if (response.IsSuccessStatusCode)
                {
                    // Send a GET request to retrieve the display status
                    var displayStatusResponse = await client.GetAsync("/Device/AudioVideoInputOutput/Outputs/x/Ports/x/");

                    // Check if the display status retrieval was successful
                    if (displayStatusResponse.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        var displayStatusJson = await displayStatusResponse.Content.ReadAsStringAsync();

                        // Parse the display status JSON
                        var displayStatus = JsonSerializer.Deserialize<DisplayStatus>(displayStatusJson);

                        // Extract the required information
                        int horizontalResolution = displayStatus.horizontalResolution;
                        int verticalResolution = displayStatus.verticalResolution;
                        bool isSyncDetected = displayStatus.isSyncDetected;

                        // Output the display information
                        Console.WriteLine("Display Information:");
                        Console.WriteLine($"Horizontal Resolution: {horizontalResolution}");
                        Console.WriteLine($"Vertical Resolution: {verticalResolution}");
                        Console.WriteLine($"Sync Detected: {isSyncDetected}");
                    }
                }
            }
        }
    }

    public class DisplayStatus
    {
        public int horizontalResolution { get; set; }
        public int verticalResolution { get; set; }
        public bool isSyncDetected { get; set; }
    }
}




