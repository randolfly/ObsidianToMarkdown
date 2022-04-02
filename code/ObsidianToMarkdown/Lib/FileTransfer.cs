using ObsidianToMarkdown.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ObsidianToMarkdown.Lib
{
    public static class FileTransfer
    {
        /// <summary>
        /// 获取文件的所有数据
        /// </summary>
        /// <param name="filePath">文件全局路径</param>
        /// <returns></returns>
        public static string GetFileContent(string filePath)
        {
            return File.ReadAllText(filePath);
        }
        /// <summary>
        /// 获取文件对应的Yaml信息
        /// </summary>
        /// <param name="filePath">文件全局路径</param>
        /// <param name="vaultPath">Obsidian Vault路径</param>
        /// <returns></returns>
        public static FileYamlHead GetFileYamlHead(string filePath, string vaultPath)
        {
            FileYamlHead yamlHead = new FileYamlHead();
            string title = Path.GetFileNameWithoutExtension(filePath);
            
            FileInfo fileInfo = new FileInfo(filePath);
            DateTime dateTime = fileInfo.CreationTime;

            string relativePath = Path.GetRelativePath(vaultPath, filePath);
            List<string> categories = relativePath.Split(Path.DirectorySeparatorChar).ToList();

            if(categories.Count > 0)
            {
                categories.RemoveAt((categories.Count - 1));
            }

            List<string> Tags = ParseTags(filePath);
            
            yamlHead.Title = title;
            yamlHead.Categories = categories;
            yamlHead.Tags = Tags;
            yamlHead.DateTime = dateTime;
            
            return yamlHead;
        }

        public static string AppendYamlInfo()
        {
            return "";
        }

        public static List<String> ParseTags(string filePath)
        {
            string fileText = GetFileContent(filePath);
            string tagPattern = @"tags[:|：]\s*(?:#[\w|\u4e00-\u9fa5]+\s?)+";

            string matche = Regex.Match(fileText, tagPattern).Value;
            var tagsListRaw = matche.Split("#").ToList();
            List<String> tagsList = new List<String>();

            // 实际上必定存在tags
            if (tagsListRaw.Count > 1)
            {
                for (int i = 1; i < tagsListRaw.Count; i++)
                    tagsList.Add(tagsListRaw[i].Trim());
            }

            //foreach(string tag in tagsList)
            //{
            //    Console.WriteLine(tag);
            //}

            return tagsList;
        }

    }
}
