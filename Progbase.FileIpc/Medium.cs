using System.IO;  // Path

namespace Progbase.FileIpc
{
    public class Medium
    {
        private string _dirPath;
        private System.Random _random;

        public Medium(string dirPath)
        {
            this._dirPath = dirPath;
            this._random = new System.Random();
        }

        public string CreateFilePath(int id)
        {
            return Path.Combine(this._dirPath, $"{id}.txt");
        }

        public int GetNewId()
        {
            return 100 + this._random.Next() % 1000;
        }
    }
}