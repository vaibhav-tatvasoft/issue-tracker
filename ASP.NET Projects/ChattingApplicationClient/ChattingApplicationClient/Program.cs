﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRConsoleClient
{
    class Program
    {
        private static HubConnection connection;

        static async Task Main(string[] args)
        {
            string user = "akhil";

            Console.WriteLine("Starting SignalR Console Client...");

            // Initialize connection
            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7219/realtimehub?userId=akhil")  // Replace with your SignalR hub URL
                .Build();


            // Define what happens when a message is received
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine($"{user}: {message}");
            });

            // Start the connection
            await StartConnection();

            // Send messages to the server
            while (true)
            {
                Console.Write("Enter a message (or 'exit' to quit): ");
                var message = Console.ReadLine();
                if (message.ToLower() == "exit")
                    break;

                await SendMessage("ConsoleUser", message);
            }

            // Stop the connection
            await connection.StopAsync();
        }

        private static async Task StartConnection()
        {
            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connection started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }
        }

        private static async Task SendMessage(string user, string message)
        {
            try
            {
                await connection.InvokeAsync("SendMessage","vaibhav", message);
                Console.WriteLine($"Sent to vaibhav from : {user} : {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send failed: {ex.Message}");
            }
        }
    }
}