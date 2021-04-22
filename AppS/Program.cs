using System;
using System.IO;  // Path
using Progbase.FileIpc;
using System.Threading;  // Thread
using System.Collections.Generic;

namespace AppA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is app S!");
            string filesDir = "./_files";
            Medium medium = new Medium(filesDir);
            FileIpc serverIpc = new FileIpc(medium);

            serverIpc.Bind(1);
            Console.WriteLine("# server with id 1");
            serverIpc.Listen(10);

            List<Thread> threads = new List<Thread>();

            while (true)
            {
                Console.WriteLine("# waiting for clients...");
                FileIpc clientIpc = serverIpc.Accept();
                Console.WriteLine("# new client connected");

                Thread clientThread = new Thread(ProcessClient);
                threads.Add(clientThread);
                clientThread.Start(clientIpc);  // to obj parameter
            }
        }

        static void ProcessClient(object obj)
        {
            FileIpc clientIpc = (FileIpc)obj;

            while (true)
            {
                Console.WriteLine("# waiting for request...");
                string incoming = clientIpc.Receive();
                Console.WriteLine("# got request:");
                Console.WriteLine($"\"{incoming}\"");
                //
                string outgoing = "Hi from S!";
                Console.WriteLine("# sending response:");
                Console.WriteLine($"\"{outgoing}\"");
                clientIpc.Send(outgoing);
            }
        }
    }
}
