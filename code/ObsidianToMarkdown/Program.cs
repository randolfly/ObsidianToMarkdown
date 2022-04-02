using System;
using ObsidianToMarkdown.Lib;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using Serilog;
using ObsidianToMarkdown.Shared;
using ObsidianToMarkdown.Context;

namespace ObsidianToMarkdown
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigLogger();

            FileConfig fileConfig = FileHelper.ReadFileConfig();
            Log.Information($"{fileConfig.VaultPath}, {fileConfig.DestinationPath}");

            string testfile = File.ReadAllText("testfile.md");
            string sha256Result = FileHelper.ComputeSha256Hash(testfile);
            Log.Information(sha256Result);

            Log.Information("开始测试数据库");
            using (var db = new ObsidianFileInfoContext())
            {
                //Log.Information($"Database path: {db.DbPath}.");
                //ObsidianFileInfo fileInfo = new ObsidianFileInfo()
                //{
                //    Path = "./",
                //    Sha256 = sha256Result
                //};
                //db.ObsidianFiles.Add(fileInfo);

                //db.SaveChanges();
                //Log.Information("存储测试数据");

                var query = from f in db.ObsidianFiles
                            orderby f.Path
                            select f;
                foreach(var f in query)
                {
                    Log.Information(f.Sha256);
                }

                Log.Information("完成数据库输出");
            }


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
            .WriteTo.File(ObsidianSystemInfo.LogPath,
                rollOnFileSizeLimit: true)
            .CreateLogger();
        }
    }
}
