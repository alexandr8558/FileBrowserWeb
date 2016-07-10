using FileBrowserWeb.Models;
using System;
using System.IO;
using System.Linq;

namespace FileBrowserWeb.Managers
{
    public class FileSystemManager
    {
        #region Constants

        private const string STATISTICERROR = "Unable to get files count:";
        private readonly int MB10 = 10 * 1024 * 1024;
        private readonly int MB50 = 50 * 1024 * 1024;
        private readonly int MB100 = 100 * 1024 * 1024;

        #endregion

        #region Methods

        /// <summary>
        /// Gets drives info.
        /// </summary>
        /// <returns>Drives info.</returns>
        public FileSystem GetFileSystem()
        {
            return new FileSystem()
            {
                CurrentPath = "\\" ,
                Items = DriveInfo.GetDrives().Select( x => new FileSystemItem() { Name = x.Name , Path = x.RootDirectory.FullName.Substring( 0 , 1 ) , IsFile = false } ).ToArray()
            };
        }

        /// <summary>
        /// Gets folder info for given path.
        /// </summary>
        /// <param name="path">Folder path.</param>
        /// <returns>Folder info.</returns>
        public FileSystem GetFileSystem( string path )
        {
            string[] pathParts;
            string actualPath;
            string drivePathPart;
            string directioryPathPart;
            FileSystem fileSystem;

            pathParts = path.Split( new char[] { '/' } , StringSplitOptions.RemoveEmptyEntries );
            drivePathPart = String.Format( "{0}:\\" , pathParts[ 0 ] );
            directioryPathPart = pathParts.Length > 1 ? string.Join( "\\" , pathParts , 1 , pathParts.Length - 1 ) : String.Empty;
            actualPath = Path.Combine( drivePathPart , directioryPathPart );

            fileSystem = new FileSystem()
            {
                CurrentPath = actualPath ,
                Statistic = GetFolderStatistic( actualPath ) ,
                Items = new FileSystemItem[] { CreateParentLink( actualPath ) }
            };

            try
            {
                fileSystem.Items = fileSystem.Items.Union( Directory.GetDirectories( actualPath ).Select( x => new FileSystemItem() { Path = BuildSafePath( x ) , Name = Path.GetFileName( x ) , IsFile = false } ) )
                .Union( Directory.GetFiles( actualPath ).Select( x => new FileSystemItem() { Path = BuildSafePath( x ) , Name = Path.GetFileName( x ) , IsFile = true } ) ).ToArray();
            }
            catch ( UnauthorizedAccessException e )
            {
                fileSystem.ErrorMessage = e.Message;
                fileSystem.HasError = true;
            }

            return fileSystem;
        }

        /// <summary>
        /// Change path to be able to use it in url.
        /// </summary>
        /// <param name="path">Path to rebuild.</param>
        /// <returns>Rebuilded path.</returns>
        private string BuildSafePath( string path )
        {
            if ( !string.IsNullOrEmpty( path ) )
            {
                return path.Replace( ":\\" , "\\" );
            }
            return path;
        }

        /// <summary>
        /// Creates link to parent folder for given path.
        /// </summary>
        /// <param name="path">Folder path.</param>
        /// <returns>Link to parent folder.</returns>
        private FileSystemItem CreateParentLink( string path )
        {
            return new FileSystemItem()
            {
                Name = ".." ,
                Path = BuildSafePath( Path.GetDirectoryName( path ) ) ,
                IsFile = false
            };
        }

        /// <summary>
        /// Calculates statistic for folder with given path.
        /// </summary>
        /// <param name="path">Folder path.</param>
        /// <returns>Folder statistic.</returns>
        private FolderStatistic GetFolderStatistic( string path )
        {
            string[] files;
            FileInfo fileInfo;
            FolderStatistic statistic;
            
            statistic = new FolderStatistic();
            files = new string[ 0 ];
            try
            {
                files = Directory.GetFiles( path , "*.*" , SearchOption.AllDirectories );
            }
            catch ( UnauthorizedAccessException e )
            {
                statistic.ErrorMessage = String.Format( "{0} {1}" , STATISTICERROR , e.Message );
                statistic.HasError = true;
            }

            foreach ( string filePath in files )
            {
                fileInfo = new FileInfo( filePath );
                if ( fileInfo.Length <= MB10 )
                {
                    statistic.Less10Count++;
                }
                else if ( fileInfo.Length > MB10 && fileInfo.Length <= MB50 )
                {
                    statistic.More10Less50Count++;
                }
                else if ( fileInfo.Length >= MB100 )
                {
                    statistic.More100Count++;
                }
            }
            return statistic;
        }

        #endregion
    }
}