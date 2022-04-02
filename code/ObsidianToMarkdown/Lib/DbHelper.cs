using ObsidianToMarkdown.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsidianToMarkdown.Lib
{
    public class DbHelper
    {
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        public static void AddFile(ObsidianFileInfo fileInfo)
        {
            using (var ObsidianDb = new ObsidianFileInfoContext()) 
            {
                var dbFileInfos = ObsidianDb.ObsidianFiles
                    .Where(f => f.Path == fileInfo.Path);
                if (dbFileInfos.Count() == 0)
                    ObsidianDb.Add(fileInfo);
                ObsidianDb.SaveChanges();
            }
        }
        /// <summary>
        /// 删除filePath对应的地址
        /// </summary>
        /// <param name="filePath">文件相对地址</param>
        public static void DeleteFile(string filePath)
        {
            using (var ObsidianDb = new ObsidianFileInfoContext())
            {
                var dbFileInfos = ObsidianDb.ObsidianFiles
                    .Where(f => f.Path == filePath);
                if(dbFileInfos.Count() != 0)
                {
                    var dbFileInfo = dbFileInfos.First();
                    ObsidianDb.Remove(dbFileInfo);
                }
                ObsidianDb.SaveChanges();
            }
        }
        /// <summary>
        /// 修改文件信息
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        public static void UpdateFile(ObsidianFileInfo fileInfo)
        {
            using (var ObsidianDb = new ObsidianFileInfoContext())
            {
                var dbFileInfos = ObsidianDb.ObsidianFiles
                    .Where(f => f.Path == fileInfo.Path);
                if (dbFileInfos.Count() != 0)
                {
                    var dbFileInfo = dbFileInfos.First();
                    dbFileInfo.Sha256 = fileInfo.Sha256;
                }
                ObsidianDb.SaveChanges();
            }
        }
    }
}
