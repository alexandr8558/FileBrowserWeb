using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowserWeb.Models
{
    public class FolderStatistic
    {
        #region Properties

        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }

        public int Less10Count { get; set; }

        public int More10Less50Count { get; set; }

        public int More100Count { get; set; }

        #endregion
    }
}