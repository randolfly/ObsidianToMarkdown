using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ObsidianToMarkdown.Context
{
    public class ObsidianFileInfoContext : DbContext
    {
        public DbSet<ObsidianFileInfo> ObsidianFiles { get; set; }
    
        public string DbPath { get; }

        public ObsidianFileInfoContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "obsidianToMarkdown.db");
        }
        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}