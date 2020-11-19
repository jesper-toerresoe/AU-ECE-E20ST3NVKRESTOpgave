using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HL7FHIRClient.Model
{
    public class BPMCompleteSequence
    {
        public BPMCompleteSequence()
        {
            SequenceOfBPMSamples = new List<BPMLocalSampleSequence>();
        }

        public long BPMCompleteSequenceId { get; set; } //Local Unique Id

        public string NameOfObject { get; set; } //Object is the person in focus for the Blood Pressure Measurement

        public long DurationInSeconds { get; set; } //Durations of BPM in seconds
        public DateTime StartTime { get; set; } //Start Time for BPM sequence
        public long BPMCounts { get; set; } //Actual count of BPM values in sequence
        public List<BPMLocalSampleSequence> SequenceOfBPMSamples { get; set; } //The collection of all sample sequences
    }

    public class BPMLocalSampleSequence
    {
        public long BPMLocalSampleSequenceId { get; set; } //LocalUnique Id
        public long SequenceNo { get; set; } //The number in the set  of Sample Sequences
        public long NoBPMValues { get; set; } //Number of samples in the sequence
        public byte[] BPMSamples { get; set; } //The actual sample values
    }



}
