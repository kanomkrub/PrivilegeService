using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivilegeMobileService.Model
{
    /// <summary>
    /// options static content storage
    /// </summary>
    public class ImageStoreOptions
    {
        /// <summary>
        /// max upload filesize
        /// </summary>
        public int MaxFileSize { get; set; }
        /// <summary>
        /// physical path to store images
        /// </summary>
        public string PathRoot { get; set; }
    }
}
