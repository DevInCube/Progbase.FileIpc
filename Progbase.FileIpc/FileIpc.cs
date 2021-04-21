using System.IO;  // Path
using System.Collections.Generic;

namespace Progbase.FileIpc
{
    public class FileIpc
    {
        private Medium _medium;
        private string _inputFile;
        private string _outputFile;
        private int _thisId;
        private Queue<int> _clientIdQueue;

        public FileIpc(Medium medium)
        {
            this._medium = medium;
        }

        public FileIpc(Medium medium, int inputId, int outputId)
        {
            this._medium = medium;
            this._inputFile = _medium.CreateFilePath(inputId);
            this._outputFile = _medium.CreateFilePath(outputId);
        }

        public void Send(string message)
        {
            FileSystem.WriteTo(_outputFile, message);
        }

        public string Receive()
        {
            return FileSystem.ReadFrom(_inputFile);
        }

        public void Connect(int serverId)
        {
            // create own file
            int thisId = _medium.GetNewId();
            this._inputFile = _medium.CreateFilePath(thisId);
            // write id to server
            string serverConnectFile = _medium.CreateFilePath(serverId);
            FileSystem.WriteTo(serverConnectFile, thisId.ToString());
            // wait for server new id for this connection
            string response = FileSystem.ReadFrom(this._inputFile);
            int pairedId = int.Parse(response);
            this._outputFile = _medium.CreateFilePath(pairedId);
        }

        public void Bind(int serverId)
        {
            this._thisId = serverId;
            this._inputFile = _medium.CreateFilePath(serverId);
        }

        public void Listen(int backlog)
        {
            _clientIdQueue = new Queue<int>(backlog);
        }

        private void WaitForClients()
        {
            string text = this.Receive();
            string[] clientIdValues = text.Split("\r\n");

            foreach (string clientIdValue in clientIdValues)
            {
                int clientId;
                if (!int.TryParse(clientIdValue, out clientId))
                {
                    throw new System.Exception($"Protocol error: client id number expected on Listen, got `{clientIdValue}`");
                }

                _clientIdQueue.Enqueue(clientId);
            }
        }

        public FileIpc Accept()
        {
            if (_clientIdQueue.Count == 0)
            {
                WaitForClients();
            }

            int firstClientId = _clientIdQueue.Dequeue();
            int pairId = _medium.GetNewId();
            FileIpc clientIpc = new FileIpc(_medium, pairId, firstClientId);
            clientIpc.Send(pairId.ToString());
            return clientIpc;
        }
    }
}