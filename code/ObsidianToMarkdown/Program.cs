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

            DirectoryInfo vaultPath = Directory.CreateDirectory(fileConfig.VaultPath);
            DirectoryInfo targetPath = Directory.CreateDirectory(fileConfig.DestinationPath);

            List<DirectoryInfo>? vaultDictories = vaultPath.GetDirectories("*.*", System.IO.SearchOption.AllDirectories).ToList();
            vaultDictories.Add(vaultPath);

            vaultDictories.RemoveAll((DirectoryInfo vaultDir) =>
            {
                string dirRelativePath = Path.GetRelativePath(vaultPath.FullName, vaultDir.FullName);
                // delelte ignore Paths
                foreach (var ignorePath in fileConfig.IgnorePaths)
                {
                    if (dirRelativePath.StartsWith(ignorePath))
                    {
                        return true;
                    }
                }
                return false;
            });

            foreach (var addPath in fileConfig.AddPaths)
            {
                var addFullPath = Path.Combine(fileConfig.VaultPath, addPath);
                DirectoryInfo addDirectory = Directory.CreateDirectory(addFullPath);

                List<DirectoryInfo>? addVaultDictories = addDirectory.GetDirectories("*.*", System.IO.SearchOption.AllDirectories).ToList();
                vaultDictories.AddRange(addVaultDictories);
            }
            // create dir
            foreach (DirectoryInfo vaultDictoriesDir in vaultDictories)
            {
                string dirRelativePath = Path.GetRelativePath(vaultPath.FullName, vaultDictoriesDir.FullName);
                string targetDirPath = Path.Combine(targetPath.FullName, dirRelativePath);
                Directory.CreateDirectory(targetDirPath);
                // 复制对应文件夹下所有文件
                foreach (var fileInfo in vaultDictoriesDir.GetFiles("*.*"))
                {
                    string targetFilePath = "";
                    if (fileInfo.Name.StartsWith("@"))
                    {
                        targetFilePath = Path.Combine(targetDirPath, "literature-" + fileInfo.Name.Replace("@", ""));
                    }
                    else
                    {
                        targetFilePath = Path.Combine(targetDirPath, fileInfo.Name);
                    }
                    // 检查文件信息
                    if (fileInfo.Extension == ".md")
                    {
                        string fileText = File.ReadAllText(fileInfo.FullName);
                        string sha256Result = FileHelper.ComputeSha256Hash(fileText);
                        string fileRelativePath = Path.Combine(dirRelativePath, fileInfo.Name);
                        ObsidianFileInfo obsidianFileInfo = new ObsidianFileInfo { Path = fileRelativePath, Sha256 = sha256Result };
                        int isUpdate = DbHelper.UpdateFile(obsidianFileInfo);
                        switch (isUpdate)
                        {
                            case -1 or 1:
                                // 不存在/修改文件，转换过去
                                if (File.Exists(targetFilePath))
                                    File.Delete(targetFilePath);
                                string newText = fileText.ReplaceAdToHexo()
                                    .ReplaceWikiLink()
                                    .AppendHexoYamlInfo(Path.GetFullPath(fileInfo.FullName), fileConfig.VaultPath);
                                File.WriteAllText(targetFilePath, newText);
                                break;
                            case 0:
                                Log.Information($"file: {targetFilePath} already exists, innored");
                                break;
                        }
                    }
                    else
                    {
                        // 跳过.json .html file，影响hexo配置...
                        if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png" || fileInfo.Extension == ".pdf" || fileInfo.Extension == ".svg")
                        {
                            if (!File.Exists(targetFilePath))
                            {
                                File.Copy(fileInfo.FullName, targetFilePath);
                                Log.Information($"copy file: {fileInfo.FullName} to {targetFilePath}");
                            }
                        }
                    }
                }
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
            .WriteTo.File(ObsidianSystemInfo.LogPath)
            .CreateLogger();
        }
    }
}
