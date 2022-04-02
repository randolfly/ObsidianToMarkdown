using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsidianToMarkdown.Lib
{
    public class FileConfig
    {
           /// <summary>
        /// obsidian vault 文件夹路径
        /// </summary>
        public string VaultPath { get; set; }

        /// <summary>
        /// 输出目标文件夹地址
        /// </summary>
        public string DestinationPath { get; set; }
        //Todo: 添加文件过滤选项
    }
}
