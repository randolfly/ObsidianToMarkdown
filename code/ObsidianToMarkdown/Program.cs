using System;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using ObsidianToMarkdown.Lib;

namespace ObsidianToMarkdown
{
    class Program
    {
        static void Main(string[] args)
        {
            string configPath = "./config.yml";
            string configYml = System.IO.File.ReadAllText(configPath);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();

            //yml contains a string containing your YAML
            var fileConfig = deserializer.Deserialize<FileConfig>(configYml);
            string? vaultPath = fileConfig.VaultPath;
            Console.WriteLine(vaultPath);
        }
    }
}
