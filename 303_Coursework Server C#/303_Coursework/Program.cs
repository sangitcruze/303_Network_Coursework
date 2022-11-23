using _303_Coursework;
using System;

namespace Coursework // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            Server.Start(50, 26950);
            Console.ReadKey();

        }
    }
}