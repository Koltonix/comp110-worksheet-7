using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Stores all of the data that is relevant to directory of this
/// task, but can be used elsewhere provided it is given a root folder
/// in the constructor.
/// </summary>
public class DirectoryData
{
    public string rootPath;
    public string[] filePaths;
    public int pathDepth;
    private List<string> allFilePaths = new List<string>();

    public DirectoryData(string directory)
    {
        this.rootPath = directory;
        filePaths = GetAllFilePaths(Directory.GetDirectories(directory));
    }

    /// <summary>
    /// Uses a depth search to go through all of the files and folders of the 
    /// root folder and then adds the file paths to a list of strings. It also
    /// keeps track of the total depth.
    /// </summary>
    /// <param name="paths"></param>
    /// <returns>
    /// Returns all of the file paths in the root folder
    /// </returns>
    private string[] GetAllFilePaths(string[] paths)
    {
        foreach (string childPath in paths)
        {
            for (int i = 0; i < Directory.GetFiles(childPath).Length; i++)
            {
                string fileDirectoryPath = Directory.GetFiles(childPath)[i];
                allFilePaths.Add(fileDirectoryPath);
            }

            pathDepth++;
            GetAllFilePaths(Directory.GetDirectories(childPath));
        }

        return allFilePaths.ToArray();
    }
}


namespace comp110_worksheet_7
{
	public static class DirectoryUtils
	{
        private static DirectoryData directoryData;

        // Return the size, in bytes, of the given file
        public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		// Return true if the given path points to a directory, false if it points to a file
		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}             
     
        #region Get Total File Size
        // Return the total size, in bytes, of all the files below the given directory
        public static long GetTotalSize(string directory)
		{
            directoryData = new DirectoryData(directory);
            return GetTotalFileSize(directoryData.filePaths);
		}

        /// <summary>
        /// Uses all of the files and gets their sizes and then adds them up.
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns>
        /// The file size of all of the file paths provided by the parameters.
        /// </returns>
        private static long GetTotalFileSize(string[] filePaths)
        {
            long fileSize = 0;
            for (int i = 0; i < filePaths.Length; i++)
            {
                fileSize += new FileInfo(filePaths[i]).Length;
            }

            return fileSize;
        }

        #endregion

        #region Count Files
        // Return the number of files (not counting directories) below the given directory
        public static int CountFiles(string directory)
		{
            directoryData = new DirectoryData(directory);
            return directoryData.filePaths.Length;
		}
        #endregion

        #region Get File Depth
        // Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
        public static int GetDepth(string directory)
		{
            directoryData = new DirectoryData(directory);
            return directoryData.pathDepth;
		}
        #endregion

        #region Get Smallest File
        // Get the path and size (in bytes) of the smallest file below the given directory
        public static Tuple<string, long> GetSmallestFile(string directory)
		{
            directoryData = new DirectoryData(directory);
            return CheckSmallestFile(directoryData.filePaths);
		}

        /// <summary>
        /// Checks for the smallest file size using the provided file paths.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>
        /// The smallest file path and the size of the file.
        /// </returns>
        private static Tuple<string, long> CheckSmallestFile(string[] paths)
        {
            Tuple<string, long> smallestFileData = new Tuple<string, long>("", long.MaxValue);

            for (int i = 0; i < paths.Length; i++)
            {
                long currentPathFileSize = new FileInfo(paths[i]).Length;
                if (currentPathFileSize < smallestFileData.Item2)
                {
                    smallestFileData = new Tuple<string, long>(paths[i], currentPathFileSize);
                }   
            }

            return smallestFileData;
        }

        #endregion

        #region Get Largest File Size
        // Get the path and size (in bytes) of the largest file below the given directory
        public static Tuple<string, long> GetLargestFile(string directory)
		{
            directoryData = new DirectoryData(directory);
            return CheckLargestFile(directoryData.filePaths);
        }

        /// <summary>
        /// Checks for the largest file size using the provided file paths.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>
        /// The largest file path and the size of the file.
        /// </returns>
        private static Tuple<string, long> CheckLargestFile(string[] paths)
        {
            Tuple<string, long> largestFileData = new Tuple<string, long>("", 0);

            for (int i = 0; i < paths.Length; i++)
            {
                long currentPathFileSize = new FileInfo(paths[i]).Length;
                if (currentPathFileSize > largestFileData.Item2)
                {
                    largestFileData = new Tuple<string, long>(paths[i], currentPathFileSize);
                }
            }

            return largestFileData;
        }

        #endregion

        #region Get Files Paths of a Given Size
        // Get all files whose size is equal to the given value (in bytes) below the given directory
        public static IEnumerable<string> GetFilesOfSize(string directory, long size)
		{
            directoryData = new DirectoryData(directory);
            return CheckFilesOfSize(directoryData.filePaths, size);
		}

        /// <summary>
        /// Checks all of the given file paths against a given file size provided to see
        /// if they are the same and then assigns it to a list.
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="fileSize"></param>
        /// <returns>
        /// All of the file paths that are equal to the provided parameter
        /// </returns>
        private static List<string> CheckFilesOfSize(string[] paths, long fileSize)
        {
            List<string> sameFileSizePaths = new List<string>();
            for (int i = 0; i < paths.Length; i++)
            {
                long currentPathFileSize = new FileInfo(paths[i]).Length;
                if (currentPathFileSize == fileSize)
                {
                    sameFileSizePaths.Add(paths[i]);
                }
            }

            return sameFileSizePaths;
        }

        #endregion
    }
}
