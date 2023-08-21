using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Text;

namespace UserInteractionProgram
{
    class Program
    {
        static void Main(string[] args)
        {

            // Get user prompt from the console
            //Console.Write("Enter your prompt : ");
            //string prompt = Console.ReadLine();
            

            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Need answer");
                Console.WriteLine("2. Need code");
                Console.WriteLine("3. Need suggestion");
                Console.WriteLine("Type 'exit' to quit.");

                string choice = Console.ReadLine();

                if (choice.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                ProcessUserChoice(choice);

                Console.WriteLine();
            }
        }

        static async void ProcessUserChoice(string choice)
        {
            string option = GetOptionText(choice);
            if (option != null)
            {
                const string apiKey = "Use Your Api key"; // Replace with your actual API key
                Console.WriteLine("You selected: " + option);
                // Get user prompt from the console
                Console.Write("Enter your prompt : ");
                string prompt = Console.ReadLine();
                string assistantResponse = await GenerateChatGptResponse(apiKey, option + " for: " + prompt);
                Console.WriteLine(assistantResponse);
            }
            else
            {
                Console.WriteLine("Invalid option. Please select a valid option.");
            }
        }

        static string GetOptionText(string choice)
        {
            switch (choice.ToLower())
            {
                case "1":
                    return "Need answer";
                case "2":
                    return "Need code";
                case "3":
                    return "Need suggestion";
                default:
                    return null;
            }
        }

        static async Task<string> GenerateChatGptResponse(string apiKey, string prompt)
        {
            string apiUrl = "https://api.openai.com/v1/chat/completions";
            string authorizationHeader = "Bearer " + apiKey;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", authorizationHeader);
                    string requestData = $@"{{
                    ""model"": ""gpt-3.5-turbo"",
                    ""messages"": [{{
                        ""role"": ""system"",
                        ""content"": ""{prompt}""
                    }}]
                }}";
                    StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string responseJson = await response.Content.ReadAsStringAsync();

                    // Extract the assistant response from the JSON response
                    // Assuming responseJson contains a valid JSON structure, you may want to add error handling for production code.
                    dynamic responseData = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);
                    string assistantResponse = responseData.choices[0].message.content.ToString().Trim();

                    Console.WriteLine("Assistant: " + assistantResponse);
                    return assistantResponse;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred during ChatGPT API request: " + ex.Message);
                return null;
            }
        }
    }
}
