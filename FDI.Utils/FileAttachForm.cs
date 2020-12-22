using System;

namespace FDI.Utils
{
    [Serializable]
    public class FileAttachForm
    {
        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        private string _fileServer;
        public string FileServer
        {
            get { return _fileServer; }
            set { _fileServer = value; }
        }
        public FileAttachForm()
        {

        }
        public FileAttachForm(string filename, string fileserver)
        {
            _fileName = filename;
            _fileServer = fileserver;
        }
    }
}
