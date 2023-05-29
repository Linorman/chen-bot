using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace chen_bot.Modules.msghandle
{
    [Table("chatMsg")]
    public class Msg
    {
        public string? userQQ { get; set; }
        public int time { get; set; }
        public string? context { get; set; }

        [Key]
        public int id { get; set; }
    }

    public class MsgContext : DbContext
    {
        public DbSet<Msg> Messages { get; set; }
        public String DbPath { get; }

        public MsgContext()
        {
            DbPath = @"msgDb.db";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }
}
