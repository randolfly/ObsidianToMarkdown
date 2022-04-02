using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsidianToMarkdown.Context
{
    public class ObsidianFileInfo
    {
        /// <summary>
        /// 文件相对Vault文件夹下相对路径，包含文件名，是主键
        /// </summary>
        [Key]
        public string Path { get; set; }
        /// <summary>
        /// 文件计算的Sha256哈希
        /// </summary>
        public string Sha256 { get; set; }
    }
}
