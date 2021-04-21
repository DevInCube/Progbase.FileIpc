using System;
using System.IO;  // Path
using Progbase.FileIpc;

namespace AppA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is app S!");
            string filesDir = "./_files";
            string inFile = Path.Combine(filesDir, "2.txt");
            string outFile = Path.Combine(filesDir, "1.txt");
            FileIpc ipc = new FileIpc(inFile, outFile);

            Console.WriteLine("# waiting for request...");
            string incoming = ipc.Receive();
            Console.WriteLine("# got request:");
            Console.WriteLine($"\"{incoming}\"");
            //
            string outgoing = "Hi from S!";
            Console.WriteLine("# sending response:");
            Console.WriteLine($"\"{outgoing}\"");
            ipc.Send(outgoing);
        }
    }
}
