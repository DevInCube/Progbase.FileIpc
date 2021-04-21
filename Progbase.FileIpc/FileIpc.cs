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
            int thisId = _medium.GetNewId();
            this._inputFile = _medium.CreateFilePath(thisId);
            this._outputFile = _medium.CreateFilePath(serverId);
            FileSystem.WriteTo(_outputFile, thisId.ToString());
            FileSystem.ReadFrom(_inputFile);  // ack and ignore
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
            FileIpc clientIpc = new FileIpc(_medium, _thisId, firstClientId);
            clientIpc.Send("ACK");
            return clientIpc;
        }
    }
}