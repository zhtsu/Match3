using System.IO;

namespace Ink
{
    public interface IFileHandler
    {
        string ResolveInkFilename (string includeName);
        string LoadInkFileContents (string fullFilename);
    }

    public class DefaultFileHandler : Ink.IFileHandler {
        public string ResolveInkFilename (string includeName)
        {
            var workingDir = Directory.GetCurrentDirectory ();
            var fullRootInkPath = Path.Combine (workingDir, includeName);
            return fullRootInkPath;
        }

        public string LoadInkFileContents (string fullFilename)
        {
        	return File.ReadAllText (fullFilename);
        }
    }

    public class UnityFileHandler : Ink.IFileHandler
    {
        private readonly string rootDirectory;

        public UnityFileHandler(string rootDirectory)
        {
            this.rootDirectory = rootDirectory;
        }

        public string ResolveInkFilename(string includeName)
        {
            // Convert to Unix style, and then use FileInfo.FullName to parse any ..\
            return new FileInfo(Path.Combine(rootDirectory, includeName).Replace('\\', '/')).FullName;
        }

        public string LoadInkFileContents(string fullFilename)
        {
            return File.ReadAllText(fullFilename);
        }
    }
}
