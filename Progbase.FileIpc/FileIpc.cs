using System.IO;  // Path

namespace Progbase.FileIpc
{
    public class FileIpc
    {
        private Medium _medium;
        private string _inputFile;
        private string _outputFile;

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
        }
    }
}