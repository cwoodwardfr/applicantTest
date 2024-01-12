using System.Diagnostics;
using System.Text;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplicantTest
{
    class Program
    {
        static string uniqueURL = "<YOU MUST CHANGE THIS>";
        static Random random = new Random();

        static async Task Main()
        {
            int arraySize = 1000;
            int iterations = 5;
            double sum = 0;
            string key = "CaféBabe15this4Str0ngPASS?";
            string cipherText = "";
            string decryptedText = "";


            for (int i = 0; i < iterations; i++)
            {
                string[] randomStrings = await GenerateRandomStrings(arraySize);

                // Start program timer
                var watch = Stopwatch.StartNew();

                foreach (string plainText in randomStrings)
                {
                    cipherText = RC4.Encrypt(plainText, key);
                    decryptedText = RC4.Decrypt(cipherText, key);
                    if (string.Compare(plainText, decryptedText) != 0)
                    {
                        Console.WriteLine("There was an issue, aborting.");
                        Environment.Exit(-1);
                    }
                }

                // End program timer    
                watch.Stop();
                sum += watch.ElapsedMilliseconds;
            }

            // Display the total program runtime
            Console.WriteLine("Processed " + arraySize*iterations + " samples.");
            Console.WriteLine("Encryption and decryption took " + sum/iterations + " ms on average.");
        }

        static async Task<string?> GetRandomString()
        {
            using (HttpClient client = new())
            {
                try
                {
                    // Make the HTTP request
                    HttpResponseMessage response = await client.GetAsync(uniqueURL);
                    response.EnsureSuccessStatusCode();
                    byte[] responseData = await response.Content.ReadAsByteArrayAsync();

                    // Decode the data as an UTF8 string
                    string decodedString = Encoding.UTF8.GetString(responseData);

                    return decodedString;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
        }

        static async Task<string[]> GenerateRandomStrings(int count)
        {
            string[] randomStrings = new string[count];
            string random = await GetRandomString();

            for (int i = 0; i < count; i++)
            {
                randomStrings[i] = random;
            }

            return randomStrings;
        }
    }
}