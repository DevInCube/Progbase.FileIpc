using System;
using System.IO;  // Path
using Progbase.FileIpc;

namespace AppA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is app A!");
            string filesDir = "./_files";
            string inFile = Path.Combine(filesDir, "1.txt");
            string outFile = Path.Combine(filesDir, "2.txt");
            FileIpc ipc = new FileIpc(inFile, outFile);

            string outgoing = "Hello from A!";
            Console.WriteLine("# sending request: ");
            Console.WriteLine($"\"{outgoing}\"");
            ipc.Send(outgoing);
            //
            Console.WriteLine("# waiting for response...");
            string incoming = ipc.Receive();
            Console.WriteLine("# got response:");
            Console.WriteLine($"\"{incoming}\"");
        }
    }
}
