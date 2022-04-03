using System;
using ObsidianToMarkdown.Lib;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Serilog;
using ObsidianToMarkdown.Shared;
using ObsidianToMarkdown.Context;
using System.Text.RegularExpressions;

namespace ObsidianToMarkdown
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigLogger();

            FileConfig fileConfig = FileHelper.ReadFileConfig();
            Log.Information($"{fileConfig.VaultPath}, {fileConfig.DestinationPath}");

            //string testfile = File.ReadAllText("testfile.md");
            //string sha256Result = FileHelper.ComputeSha256Hash(testfile);
            //Log.Information(sha256Result);

            //Log.Information("开始测试数据库");

            // 提取ad-xx信息
            string text = File.ReadAllText("testfile.md");
            FileTransfer.ReplaceAdToHexo(text);

            //Log.Information("测试Parse Tags");

            //string fp = Path.GetFullPath("testfile.md");
            //string vp = @"E:\Code\C#\Randolf.Blog\code\ObsidianToMarkdown";
            //var yamlHead = FileTransfer.GetFileYamlHead(fp, vp);

            ////File.WriteAllText("regexFile.md", yml);
            //Log.Information(yamlHead.DateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            //Log.Information(yamlHead.Title);
            //foreach(var kv in yamlHead.Tags)
            //{
            //    Log.Information($"tags:\t{kv}",kv);
            //}
            //foreach (var kv in yamlHead.Categories)
            //{
            //    Log.Information($"categories:\t{kv}", kv);
            //}

            Log.CloseAndFlush();
        }

        static void ConfigLogger()
        {
            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Information()
            //.WriteTo.Console()
            //.WriteTo.File(ObsidianSystemInfo.LogPath,
            //    rollingInterval: RollingInterval.Day,
            //    rollOnFileSizeLimit: true)
            //.CreateLogger();
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(ObsidianSystemInfo.LogPath)
            .CreateLogger();
        }
    }
}
