using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HL7FHIRClient.Data;
using HL7FHIRClient.Model;
using Hl7.Fhir.Model;
using HL7FHIRClient.Boundary.BPM;
using System.Linq;

namespace HL7FHIRClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test of HL7FHIR!");
            //Step 1 Get acces to a local SQLite database
            using (var localdbcache = new LocalCacheDBContext())
            {
                localdbcache.Database.Migrate(); //Ensures the database is created and if needed updated with pending migrations
            }

            //Step 2 simulation af  a time limited session with collection of BPMs
            //In this template we are using a 1:Many association between the BMPCompleteSequence (root) and the BMPLocalSampleSequence (child).
            //Use your own classes here
            //Here next, each step simualed covers 1 second with 50 sample values 
            using (var localdbcache = new LocalCacheDBContext())
            {
                var completeseq = new BPMCompleteSequence() { NameOfObject = "231145-2341", StartTime = DateTime.Now, BPMCounts = 3600, DurationInSeconds = 3600 };
                localdbcache.Add(completeseq); //Add the root object to db context
                localdbcache.SaveChanges(); //Save root to database
                //Start receiving sample sequence from RPI, here the data is generated 
                for (var step = 0; step < 100; step++) //Simulation 1 hour in steps of second, though here truncated to 100 steps for speding up test
                {                    
                    var samples = new BPMLocalSampleSequence() { NoBPMValues = 50, SequenceNo = step };
                    float[] data = new float[50];
                    for (var sampleno = 0; sampleno < 50; sampleno++)//Generating values
                    {
                        data[sampleno] = sampleno / 100F;
                    }
                    //https://stackoverflow.com/questions/4635769/how-do-i-convert-an-array-of-floats-to-a-byte-and-back
                    var rawdata = new byte[data.Length * 4]; //Four bytes per float
                    Buffer.BlockCopy(data, 0, rawdata, 0, data.Length); //Make floats a BLOB 
                    samples.BPMSamples = rawdata; //Add data to child object
                    completeseq.SequenceOfBPMSamples.Add(samples); //Add child object to root object
                    //localdbcache.Add(samples); //Add child object to db context
                    localdbcache.SaveChanges(); //Save current child to database with FK to root
                }

                //Step 3 when session is ended save all collected BPMs in one HL7FHIR Obersevation in the public HL7FHIR database
                //But first retrieve complete sequence from local cache (!!!! not using the other instance completeseq)

                var retId = completeseq.BPMCompleteSequenceId;
                var retrievedseq = localdbcache.BPMCompleteSequences.
                    Where(i => i.BPMCompleteSequenceId.Equals(retId)).
                    Include(c => c.SequenceOfBPMSamples).Single();
                    //.ToListAsync();

                //Then save sequence at 
                var hl7fhirclient = new HL7FHIRR4BPMClient();
                var returnedid = hl7fhirclient.CreateHl7FHIRBMPObservation(retrievedseq);


                //Step 4 Get a specfic Obesrvation from the public HL7FHIR database
                //And convert the BPM data to useable datatypes

                //Step 5 optional updates and deletes

                
            }
        }
    }
}
