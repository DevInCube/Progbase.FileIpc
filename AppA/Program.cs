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
            Medium medium = new Medium(filesDir);
            FileIpc ipc = new FileIpc(medium, 1, 2);

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
