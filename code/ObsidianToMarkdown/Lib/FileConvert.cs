using ObsidianToMarkdown.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ObsidianToMarkdown.Lib
{
    public static class FileConvert
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
        /// 解析文件tags
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <returns>解析的Tags</returns>
        public static List<String> ParseTags(string filePath)
        {
            string fileText = GetFileContent(filePath);
            //string tagPattern = @"tags[:|：]\s*(?:#[\w|\u4e00-\u9fa5]+\s?)+";
            string tagPattern = @"tags[:|：]\s*(?:#[\S]+\s?)+";

            string matche = Regex.Match(fileText, tagPattern).Value;
            var tagsListRaw = matche.Split("#").ToList();
            List<String> tagsList = new List<String>();

            // 实际上必定存在tags
            if (tagsListRaw.Count > 1)
            {
                for (int i = 1; i < tagsListRaw.Count; i++)
                    tagsList.Add(tagsListRaw[i].Trim());
            }
            else
            {
                tagsList.Add("default");
            }

            //foreach(string tag in tagsList)
            //{
            //    Console.WriteLine(tag);
            //}
            return tagsList;
        }

        /// <summary>
        /// 获取文件对应的Yaml信息
        /// </summary>
        /// <param name="filePath">文件全局路径</param>
        /// <param name="vaultPath">Obsidian Vault路径</param>
        /// <returns>解析的Yaml头文件</returns>
        public static FileYamlHead GetFileYamlHead(string filePath, string vaultPath)
        {
            FileYamlHead yamlHead = new FileYamlHead();
            // 替换 @xx 为 literature-xx，避免hexo解析出错
            string title = Path.GetFileNameWithoutExtension(filePath).Replace("@", "literature-");
            
            FileInfo fileInfo = new FileInfo(filePath);
            DateTime dateTime = fileInfo.CreationTime;

            string relativePath = Path.GetRelativePath(vaultPath, filePath).Replace( @"\", "/").Replace("@", "literature-");
            List<string> categories = relativePath.Split(@"/").ToList();

            if(categories.Count > 0)
            {
                categories.RemoveAt((categories.Count - 1));
            }

            List<string> Tags = ParseTags(filePath);

            string htmlLink = "_posts/" + relativePath.Replace(".md", ".html");

            yamlHead.Title = title;
            yamlHead.Categories = categories;
            yamlHead.Tags = Tags;
            yamlHead.DateTime = dateTime;
            yamlHead.Link = htmlLink;
            
            return yamlHead;
        }
        /// <summary>
        /// 删除原有text中的yaml
        /// </summary>
        /// <param name="fileText">需要删除的文本</param>
        /// <returns>删除后的文本</returns>
        public static string DeleteRawYaml(string fileText)
        {
            string pattern = @"---\s*[\S\s]*?---";
            var matches = Regex.Matches(fileText, pattern, RegexOptions.IgnoreCase);
            MatchEvaluator evaluator = new MatchEvaluator(DeleteYaml);
            string newFileText = Regex.Replace(fileText, pattern, evaluator);
            return newFileText;

            static string DeleteYaml(Match match)
            {
                return "";
            }
        }

        /// <summary>
        /// 添加文件头部Hexo需要的Yaml信息
        /// </summary>
        /// <param name="fileText">需要添加yaml的文本</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="vaultPath">vault的目录</param>
        /// <returns>添加yaml后的文本</returns>
        public static string AppendHexoYamlInfo(this string fileText, string filePath, string vaultPath)
        {
            FileYamlHead fileYamlHead = GetFileYamlHead(filePath, vaultPath);
            StringBuilder stringBuilder = new StringBuilder();
            string yamlHead = fileYamlHead.ToString();
            stringBuilder.Append(yamlHead + "\n");
            // 删除原先可能有的yaml头部
            stringBuilder.Append(DeleteRawYaml(fileText));
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 替换文件中ad-xx为hexo格式
        /// </summary>
        /// <param name="fileText">替换文本</param>
        /// <returns>替换完成的文本</returns>
        public static string ReplaceAdToHexo(this string fileText)
        {
            //string pattern = @"```ad-(?<head>\w+)\s*((title[:|：]\s*)(?<title>[\w\u4e00-\u9fa5]+))*(?<content>[\s\S]*?)(?<tail>```)";
            string pattern = @"```ad-(?<head>\w+)\s*((title[:|：]\s*)(?<title>[\S]+))*(?<content>[\s\S]*?)(?<tail>```)";
            var matches = Regex.Matches(fileText, pattern, RegexOptions.IgnoreCase);
            MatchEvaluator evaluator = new MatchEvaluator(ChangeAdTag);
            string newFileText = Regex.Replace(fileText, pattern, evaluator);
            return newFileText;

            static string ChangeAdTag(Match match)
            {
                string head = match.Groups["head"].Value.Trim();
                string title = match.Groups["title"].Value.Trim();
                string content = match.Groups["content"].Value;

                string hexoHead = string.Concat("{% admonition ", $"{head} ", $"{title} ", "%}\n");
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(hexoHead);
                stringBuilder.Append(content);
                stringBuilder.Append("{% endadmonition %}\n");
                return stringBuilder.ToString();
            }

        }

        /// <summary>
        /// 替换文件中的维基link为markdown格式link
        /// </summary>
        /// <param name="filePath">替换文本</param>
        /// <returns>替换完成的文本</returns>
        public static string ReplaceWikiLink(this string fileText)
        {
            string pattern = @"\[\[\s*(?<content>[^\|]+?)(?<name>\|\S+?)*?\s*\]\]";
            var matches = Regex.Matches(fileText, pattern, RegexOptions.IgnoreCase);
            MatchEvaluator evaluator = new MatchEvaluator(ChangeWikiLink);
            string newFileText = Regex.Replace(fileText, pattern, evaluator);
            return newFileText;

            static string ChangeWikiLink(Match match)
            {
                // https://regexr.com/
                string? content = match?.Groups["content"].Value.Trim();
                string? name = match?.Groups["name"].Value.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    name = content.Split("/").Last();
                }
                else
                {
                    name = name.Substring(1);
                }

                StringBuilder stringBuilder = new StringBuilder();
                switch (content?.Substring(0, 1))
                {
                    case "@":
                        // Obsidian Citations 文件，不做展示
                        stringBuilder.Append("[暂时不显示]");
                        stringBuilder.Append($"(literature-{content.Replace("@", "")})");
                        break;
                    case ".":
                        stringBuilder.Append($"[{name}]");
                        stringBuilder.Append($"({content})");
                        break;
                    default:
                        stringBuilder.Append($"[{name}]");
                        stringBuilder.Append($"(./{content})");
                        break;
                }

                return stringBuilder.ToString();
            }
        }
    }
}
