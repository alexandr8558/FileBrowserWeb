namespace FileBrowserWeb.Models
{
    public class FileSystem
    {
        #region Properties

        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }

        public string CurrentPath { get; set; }

        public FolderStatistic Statistic { get; set; }

        public FileSystemItem[] Items { get; set; }

        #endregion
    }
}