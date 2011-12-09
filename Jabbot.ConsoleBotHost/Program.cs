﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Jabbot.ConsoleBotHost
{
    class Program
    {
        private static readonly string _serverUrl = ConfigurationManager.AppSettings["Bot.Server"];
        private static readonly string _botName = ConfigurationManager.AppSettings["Bot.Name"];
        private static readonly string _botPassword = ConfigurationManager.AppSettings["Bot.Password"];
        private static readonly string _botRooms = ConfigurationManager.AppSettings["Bot.RoomList"];

        static void Main(string[] args)
        {
            Console.WriteLine("Jabbot Bot Runner Starting...");
            Bot bot = new Bot(_serverUrl, _botName, _botPassword);
            bot.PowerUp();
            JoinRooms(bot);
            Console.Write("Press enter to quit...");
            Console.ReadLine();
            bot.ShutDown();

        }
        private static void JoinRooms(Bot bot)
        {
            foreach (var room in _botRooms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Console.Write("Joining {0}...", room);
                if (TryCreateRoomIfNotExists(room, bot))
                {
                    bot.Join(room);
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.WriteLine("Failed");
                }
            }
        }
        private static bool TryCreateRoomIfNotExists(string roomName, Bot bot)
        {
            try
            {
                bot.CreateRoom(roomName);
            }
            catch (AggregateException e)
            {
                if (!e.InnerExceptions.FirstOrDefault().Message.Contains("exists"))
                    return false;
            }

            return true;
        }
    }
}
