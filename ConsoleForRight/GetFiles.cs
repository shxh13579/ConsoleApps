using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleForRight
{
    public class FileServer
    {
        public List<string> GetFilesFromDirectory(string path)
        {
            path = path.TrimStart().TrimEnd();
            var result = new List<string>();
            if (Directory.Exists(path))
            {
                result.AddRange(Directory.GetFiles(path));
                var dirs = Directory.GetDirectories(path);
                GetAllFiles(dirs, ref result);
            }
            return result;
        }

        private List<string> GetAllFiles(string[] pathList, ref List<string> files)
        {
            var list = new List<string>();
            foreach (var path in pathList)
            {
                var dirs = Directory.GetDirectories(path);
                list.AddRange(dirs);
                files.AddRange(Directory.GetFiles(path));
                GetAllFiles(dirs, ref files);
            }
            return list;
        }

        private void ManipulateFiles(string inputPath, string outputPath, out int success, out int sum, bool isCopy, bool overwrite)
        {
            success = 0;
            sum = 0;
            try
            {
                inputPath = inputPath.TrimStart().TrimEnd();
                outputPath = outputPath.TrimEnd();
                if (!Directory.Exists(outputPath))
                {
                    var dir = Directory.CreateDirectory(outputPath);
                }
                var pathList = GetFilesFromDirectory(inputPath);
                foreach (var path in pathList)
                {
                    sum++;
                    var filePath = outputPath + "\\" + Path.GetFileName(path);
                    if (!overwrite && File.Exists(filePath))
                    {
                        continue;
                    }
                    if (isCopy)
                    {
                        File.Copy(path, filePath, overwrite);
                    }
                    else
                    {
                        File.Move(path, filePath, overwrite);
                    }
                    success++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void MoveFile(string inputPath, string outputPath, out int success, out int sum, bool overwrite = false)
        {
            ManipulateFiles(inputPath, outputPath, out success, out sum, false, overwrite);
        }


        public void CopyFile(string inputPath, string outputPath, out int success, out int sum, bool overwrite = false)
        {
            ManipulateFiles(inputPath, outputPath, out success, out sum, true, overwrite);
        }
    }
}
