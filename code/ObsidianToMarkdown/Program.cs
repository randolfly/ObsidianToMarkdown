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
            //ConfigLogger();

            //FileConfig fileConfig = FileHelper.ReadFileConfig();
            //Log.Information($"{fileConfig.VaultPath}, {fileConfig.DestinationPath}");

            //string testfile = File.ReadAllText("testfile.md");
            //string sha256Result = FileHelper.ComputeSha256Hash(testfile);
            //Log.Information(sha256Result);

            //Log.Information("开始测试数据库");

            // 提取yaml信息

            string text = File.ReadAllText("testfile.md");
            string pattern = @"^---(.|\n)*---";
            MatchCollection mc = Regex.Matches(text, pattern);

            string yml = mc[0].ToString().Replace("---", "");
            

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
