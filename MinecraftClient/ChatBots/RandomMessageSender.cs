using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using MinecraftClient;

namespace MinecraftClient.ChatBots
{
    public class RandomMessageSender : ChatBot
    {
        private string ConfigPath = "TLBotConfig.txt";
        private List<string> Messages = new List<string>();
        private int Interval;
        private string Password;

        public RandomMessageSender()
        {
            PrintAsciiArt();
            LoadOrCreateConfig();
        }

        public override void Initialize()
        {
            SimulateLogin();
            ScheduleTask();
        }

        private void PrintAsciiArt()
        {
            Console.WriteLine(@"
  _____   _               ____     ___    _____ 
 |_   _| | |             | __ )   / _ \  |_   _|
   | |   | |             |  _ \  | | | |   | |  
   | |   | |___          | |_) | | |_| |   | |  
   |_|   |_____|  _____  |____/   \___/    |_|  
                 |_____|
");
            Console.WriteLine("TL_BOT Version 1.0");
        }

        private void LoadOrCreateConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                Console.WriteLine("Config file not found. Creating default config...");
                using (StreamWriter writer = new StreamWriter(ConfigPath))
                {
                    writer.WriteLine("# TL_BOT Configuration File");
                    writer.WriteLine("# Each line is a private message to send randomly.");
                    writer.WriteLine("Hello! This is a message from TL_BOT.");
                    writer.WriteLine("Welcome to the server! Enjoy your stay.");
                    writer.WriteLine("Need help? Just ask me!");
                    writer.WriteLine();
                    writer.WriteLine("# Interval in seconds: Define the interval between messages");
                    writer.WriteLine("Interval=60");
                    writer.WriteLine();
                    writer.WriteLine("# Login password: Define the password for logging in");
                    writer.WriteLine("Password=your_password_here");
                }
                Console.WriteLine("Default configuration file created.");
            }

            foreach (var line in File.ReadAllLines(ConfigPath))
            {
                if (line.StartsWith("Interval="))
                    Interval = int.Parse(line.Substring("Interval=".Length));
                else if (line.StartsWith("Password="))
                    Password = line.Substring("Password=".Length);
                else if (!line.StartsWith("#") && !string.IsNullOrWhiteSpace(line))
                    Messages.Add(line);
            }

            if (Messages.Count == 0)
            {
                Console.WriteLine("No messages found in the config file. Add some messages to send.");
                StopBot();
            }
        }

        private void SimulateLogin()
        {
            Console.WriteLine("Logging in...");
            Console.WriteLine($"Using password: {Password}");
            Thread.Sleep(2000); // Simulating login delay

            Console.WriteLine("Switching to item slot 3...");
            Thread.Sleep(1000); // Simulating item switching
            Console.WriteLine("Right-clicking with the item...");
            Thread.Sleep(1000); // Simulating right-click
            Console.WriteLine("Login complete.");
        }

        private void ScheduleTask()
        {
            Console.WriteLine($"Message interval set to {Interval} seconds.");
            ScheduleOnMainThread(SendRandomMessage, Interval * 10);
        }

        private void SendRandomMessage()
        {
            Random random = new Random();
            string message = Messages[random.Next(Messages.Count)];
            Console.WriteLine($"Sending private message using /tell: {message}");
            SendText($"/tell <target_player> {message}");
            ScheduleTask(); // Reschedule task to run again
        }
    }
}
