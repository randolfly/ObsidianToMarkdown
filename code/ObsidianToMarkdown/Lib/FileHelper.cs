using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ObsidianToMarkdown.Shared;
using Serilog;

namespace ObsidianToMarkdown.Lib
{
    public class FileHelper
    {
        /// <summary>
        /// 计算文件的HASH值(SHA256)
        /// </summary>
        /// <param name="rawData">文本数据</param>
        /// <returns>SHA256数据</returns>
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        /// <summary>
        /// 读取config.yml配置的运行参数
        /// </summary>
        /// <returns></returns>
        public static FileConfig ReadFileConfig()
        {
            string configYml = System.IO.File.ReadAllText(ObsidianSystemInfo.ConfigPath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
            //yml contains a string containing your YAML
            FileConfig fileConfig = deserializer.Deserialize<FileConfig>(configYml);
            Log.Information("读取系统Config文件完成");
            return fileConfig;
        }
    }
}
