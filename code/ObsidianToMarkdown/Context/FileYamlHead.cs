using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsidianToMarkdown.Context
{
    /// <summary>
    /// Markdown文件的Yaml文件头
    /// </summary>
    public class FileYamlHead
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 文件创建时间
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// 文件Tag
        /// </summary>
        public List<string> Tags { get; set; }
        /// <summary>
        /// 文件分类，实际上对应Obsidian中文件夹层级
        /// </summary>
        public List<string> Categories { get; set; }
        // 示例结构
        //title: 向量空间
        //date: 2019-10-10 10:00:00
        //tags: 
        //    - 线性代数
        //    - 测试1
        //    - 微积分
        //categories:
        //    - 从线性映射理解线性代数
        //    - test1
    }
}
