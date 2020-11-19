using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using HL7FHIRClient.Model;

namespace HL7FHIRClient.Data
{





    public class LocalCacheDBContext : DbContext
    {
        //protected LocalCacheDBContext() /*: base()*/
        //{

        //    //When migrations is not enabled i the database use make sure the datase is created
        //    //Database.EnsureCreated();

        //    //Following is Exlusive to "Data base.EnsureCreated();" and is used when Migration is enable and a at least one migration is added to the propject
        //    //Database.Migration();

        //}

        public DbSet<BPMCompleteSequence> BPMCompleteSequences { get; set; }
        public DbSet<BPMLocalSampleSequence> BPMLocalSampleSequences { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=LocalCache.db");
    }


}