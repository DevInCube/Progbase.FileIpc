namespace Progbase.FileIpc
{
    public class FileIpc
    {
        private string _inputFile;
        private string _outputFile;

        public FileIpc(string inputFile, string outputFile)
        {
            this._inputFile = inputFile;
            this._outputFile = outputFile;
        }

        public void Send(string message)
        {
            FileSystem.WriteTo(_outputFile, message);
        }

        public string Receive()
        {
            return FileSystem.ReadFrom(_inputFile);

        }
    }
}