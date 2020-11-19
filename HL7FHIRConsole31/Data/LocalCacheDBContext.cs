using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using HL7FHIRClient.Model;

namespace HL7FHIRClient.Data
{





    public class LocalCacheDBContext : DbContext
    {
       
        public DbSet<BPMCompleteSequence> BPMCompleteSequences { get; set; }
        public DbSet<BPMLocalSampleSequence> BPMLocalSampleSequences { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=LocalCacheV1.db");
    }


}