using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using HL7FHIRClient.Model;


namespace HL7FHIRClient.Boundary.BPM
{
    public class HL7FHIRR4BPMClient
    {
        private FhirClient client;

        public HL7FHIRR4BPMClient()
        {
            client = new FhirClient("https://aseecest3fhirservice.azurewebsites.net"); //https://aseecest3fhirservice.azurewebsites.net
            client.Timeout = 120000;
        }

        public string CreateHl7FHIRBMPObservation(BPMCompleteSequence seqtocreate) //Signature may be changed
        {
            var saveobs = makeABPMObservation(seqtocreate);
            saveobs = client.Create(saveobs);
            return saveobs.Id;

        }

        public void ReadHL7FHIRVBPMObservation(string bpmobsid) //Signature may be changed
        {
                       
            //Here goes your code
        }

        public void DeleteL7FHIRVBPMObservation(string bpmobsid) //Signature may be changed
        {
            //Here goes your code
        }

        public void UpdateL7FHIRVBPMObservation(string bpmobsid) //Signature may be changed
        {
            //Here goes your code
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", ""); //?? should "-" be removed
        }

        private Observation makeABPMObservation(BPMCompleteSequence seqtomake)
        {
            // Link https://stackoverflow.com/questions/311165/how-do-you-convert-a-byte-array-to-a-hexadecimal-string-and-vice-versa

            string rawdata = new string("");
            foreach (var sample in seqtomake.SequenceOfBPMSamples)
            {
                rawdata += ByteArrayToString(sample.BPMSamples);
            }

            var eob = new Observation();
            eob.Id = seqtomake.NameOfObject;     //"EKG-odjvbhofdjghodfgofg"; //JRT Be careful when 
            eob.Status = ObservationStatus.Final;
            eob.Category.Add(new CodeableConcept() { Coding = new List<Coding>() });
            eob.Category[0].Coding.Add(new Coding() { Code = "BPM Measurement", System = "http://terminology.hl7.org/CodeSystem/observation-category", Display = "Procedure" });
            eob.Code = new CodeableConcept();
            eob.Code.Coding.Add(new Coding() { System = "urn:oid:1.2.3.4.5.6", Code = "AUH131328", Display = "MDC_BPM_Phys_Sequence" });
            eob.Subject = new ResourceReference();
            eob.Subject.Reference = "AU-ECE-ST-E20";
            eob.Subject.Display = "E20ST3PRJ3-NVK";

            //eob.Effective = new FhirDateTime("2015-02-19T09:30:35+01:00");
            //eob.Effective = new FhirDateTime(DateTime.Now);
            //For DateTime and DataTimeOffset se link https://docs.microsoft.com/en-us/dotnet/standard/datetime/converting-between-datetime-and-offset
            DateTimeOffset dtoff = DateTime.SpecifyKind(seqtomake.StartTime, DateTimeKind.Utc);
            eob.Effective = new FhirDateTime(dtoff); //JRT
            eob.Performer.Add(new ResourceReference() { Reference = "Student/E20", Display = "Students from E20STS3NVK" });
            eob.Device = new ResourceReference();
            eob.Device.Display = "1 Transducer Device mmHG Metric";
            var m = new Observation.ComponentComponent();
            m.Code = new CodeableConcept();
            m.Code.Coding.Add(new Coding() { System = "urn:oid:1.2.3.4.5.6", Code = "AUH131328", Display = "MDC_BPM_Phys_Sequence_1" });
            m.Value = new SampledData() 
            {
                Origin = new SimpleQuantity { Value = 2048 },//??
                Period = 3600, //??
                Factor = (decimal)1.612,//??
                LowerLimit = -3300,//??
                UpperLimit = 3300,//??
                Dimensions = 1,
                Data = rawdata,
            };
            eob.Component.Add(m);//Add the new data block to Observation
            
            //But then how to access the justed  in line 89  added SampleData Object again??
            //Well remark that Observation.ComponentComponent.Value attribute is an Element class Just place cursor over m.Value
            //Then important to notice is that SampleData class inherits the Element class! Why do you think this is the case?
            //Now a type cast is needed to acces Values as a SampleData Class just as shoen in next line
            var x = (SampledData)eob.Component[0].Value;
            string d = x.Data;
            //Lesson learned: As JSON does not cares about specific object type, class definition does not exist in JSON , in C# we do
            //ned to make polymorph classes shaping more than one classe. Element class shape all needed HL7 FHIR classes in Componet,
            //SampleData shape only one specific class but can be carried in a Elemet class


            return eob;
        }

        //Code from here are only proto type code  get inspiration 
        public void Boundary_HL7FHIR_REST()
        {
            var client1 = new FhirClient("https://vonk.fire.ly");
            var client = new FhirClient("https://aseecest3fhirservice.azurewebsites.net"); //https://aseecest3fhirservice.azurewebsites.net

            //var k = new Fhir
            //client.PreferredFormat = ResourceFormat.Unknown;
            // client.UseFormatParam = true; //depends on the sever  format in url or in header (default)
            // client.ReturnFullResource = false; //Give minimal response
            client1.Timeout = 120000; // The timeout is set in milliseconds, with a default of 100000

            client.Timeout = 120000; // The timeout is set in milliseconds, with a default of 100000
                        

            //var ekgobloc = makeAObservation();
            //var ekgobs = client.Create(ekgobloc);
            //var location_Obs = new Uri("https://aseecest3fhirservice.azurewebsites.net/Observation/" + ekgobs.Id);
            //var obs_A = client.Read<Observation>(location_Obs);
            //client.Update<Observation>(obs_A);
            //client.Delete(location_Obs);
            //var ekgobs1 = makeAObservation();
            //ekgobs1 = client1.Create(ekgobs1);
            //var location_Obs1 = new Uri("https://vonk.fire.ly/Observation/"+ekgobs1.Id);
            //var obs_A1 = client1.Read<Observation>(location_Obs1);
            ////client1.Delete(obs_A1);
            //client1.Delete(location_Obs1);


            //"https://vonk.fire.ly/Observation/6d3cd8f6-c9ff-4357-891e-7774508f3a56"
            //"https://vonk.fire.ly/Observation/6d3cd8f6-c9ff-4357-891e-7774508f3a56"

            //After Create retrive the patient ID from created_pat and use the ID to retrieve the patient in Postman/AdvancedRESTClient
            //client.Delete(created_pat);//Clean up the test. Check result in Postman/AdvancedRESTClient

        }     

        private Observation makeAObservation()
        {
            List<int> rawdata = new List<int> { 2041, 2043, 2037, 2047, 2060, 2062, 2051, 2023, 2014, 2027, 2034, 2033, 2040, 2047, 2047, 2053, 2058, 2064, 2059, 2063, 2061, 2052, 2053, 2038, 1966, 1885, 1884, 2009, 2129, 2166, 2137, 2102, 2086, 2077, 2067, 2067, 2060, 2059, 2062, 2062, 2060, 2057, 2045, 2047, 2057, 2054, 2042, 2029, 2027, 2018, 2007, 1995, 2001, 2012, 2024, 2039, 2068, 2092, 2111, 2125, 2131, 2148, 2137, 2138, 2128, 2128, 2115, 2099, 2097, 2096, 2101, 2101, 2091, 2073, 2076, 2077, 2084, 2081, 2088, 2092, 2070, 2069, 2074, 2077, 2075, 2068, 2064, 2060, 2062, 2074, 2075, 2074, 2075, 2063, 2058, 2058, 2064, 2064, 2070, 2074, 2067, 2060, 2062, 2063, 2061, 2059, 2048, 2052, 2049, 2048, 2051, 2059, 2059, 2066, 2077, 2073, };
            var eob = new Observation();
            eob.Id = "EKG-odjvbhofdjghodfgofg";
            eob.Status = ObservationStatus.Final;
            //eob.Text = new Narrative();
            //eob.Text.Status = Narrative.NarrativeStatus.Generated;
            //eob.Text.Div = "<div xmlns=\"http://www.w3.org/1999/xhtml\"><p><b>Generated Narrative with Details</b></p><p><b>id</b>: ekg</p><p><b>status</b>: final</p><p><b>category</b>: Procedure <span>(Details : {http://terminology.hl7.org/CodeSystem/observation-category code 'procedure' = 'Procedure', given as 'Procedure'})</span></p><p><b>code</b>: MDC_ECG_ELEC_POTL <span>(Details : {urn:oid:2.16.840.1.113883.6.24 code '131328' = '131328', given as 'MDC_ECG_ELEC_POTL'})</span></p><p><b>subject</b>: <a>P. van de Heuvel</a></p><p><b>effective</b>: 19/02/2015 9:30:35 AM</p><p><b>performer</b>: <a>A. Langeveld</a></p><p><b>device</b>: 12 lead EKG Device Metric</p><blockquote><p><b>component</b></p><p><b>code</b>: MDC_ECG_ELEC_POTL_I <span>(Details : {urn:oid:2.16.840.1.113883.6.24 code '131329' = '131329', given as 'MDC_ECG_ELEC_POTL_I'})</span></p><p><b>value</b>: Origin: (system = '[not stated]' code null = 'null'), Period: 10, Factor: 1.612, Lower: -3300, Upper: 3300, Dimensions: 1, Data: 2041 2043 2037 2047 2060 2062 2051 2023 2014 2027 2034 2033 2040 2047 2047 2053 2058 2064 2059 2063 2061 2052 2053 2038 1966 1885 1884 2009 2129 2166 2137 2102 2086 2077 2067 2067 2060 2059 2062 2062 2060 2057 2045 2047 2057 2054 2042 2029 2027 2018 2007 1995 2001 2012 2024 2039 2068 2092 2111 2125 2131 2148 2137 2138 2128 2128 2115 2099 2097 2096 2101 2101 2091 2073 2076 2077 2084 2081 2088 2092 2070 2069 2074 2077 2075 2068 2064 2060 2062 2074 2075 2074 2075 2063 2058 2058 2064 2064 2070 2074 2067 2060 2062 2063 2061 2059 2048 2052 2049 2048 2051 2059 2059 2066 2077 2073</p></blockquote><blockquote><p><b>component</b></p><p><b>code</b>: MDC_ECG_ELEC_POTL_II <span>(Details : {urn:oid:2.16.840.1.113883.6.24 code '131330' = '131330', given as 'MDC_ECG_ELEC_POTL_II'})</span></p><p><b>value</b>: Origin: (system = '[not stated]' code null = 'null'), Period: 10, Factor: 1.612, Lower: -3300, Upper: 3300, Dimensions: 1, Data: 2041 2043 2037 2047 2060 2062 2051 2023 2014 2027 2034 2033 2040 2047 2047 2053 2058 2064 2059 2063 2061 2052 2053 2038 1966 1885 1884 2009 2129 2166 2137 2102 2086 2077 2067 2067 2060 2059 2062 2062 2060 2057 2045 2047 2057 2054 2042 2029 2027 2018 2007 1995 2001 2012 2024 2039 2068 2092 2111 2125 2131 2148 2137 2138 2128 2128 2115 2099 2097 2096 2101 2101 2091 2073 2076 2077 2084 2081 2088 2092 2070 2069 2074 2077 2075 2068 2064 2060 2062 2074 2075 2074 2075 2063 2058 2058 2064 2064 2070 2074 2067 2060 2062 2063 2061 2059 2048 2052 2049 2048 2051 2059 2059 2066 2077 2073</p></blockquote><blockquote><p><b>component</b></p><p><b>code</b>: MDC_ECG_ELEC_POTL_III <span>(Details : {urn:oid:2.16.840.1.113883.6.24 code '131389' = '131389', given as 'MDC_ECG_ELEC_POTL_III'})</span></p><p><b>value</b>: Origin: (system = '[not stated]' code null = 'null'), Period: 10, Factor: 1.612, Lower: -3300, Upper: 3300, Dimensions: 1, Data: 2041 2043 2037 2047 2060 2062 2051 2023 2014 2027 2034 2033 2040 2047 2047 2053 2058 2064 2059 2063 2061 2052 2053 2038 1966 1885 1884 2009 2129 2166 2137 2102 2086 2077 2067 2067 2060 2059 2062 2062 2060 2057 2045 2047 2057 2054 2042 2029 2027 2018 2007 1995 2001 2012 2024 2039 2068 2092 2111 2125 2131 2148 2137 2138 2128 2128 2115 2099 2097 2096 2101 2101 2091 2073 2076 2077 2084 2081 2088 2092 2070 2069 2074 2077 2075 2068 2064 2060 2062 2074 2075 2074 2075 2063 2058 2058 2064 2064 2070 2074 2067 2060 2062 2063 2061 2059 2048 2052 2049 2048 2051 2059 2059 2066 2077 2073</p></blockquote></div>";
            eob.Category.Add(new CodeableConcept() { Coding = new List<Coding>() });
            eob.Category[0].Coding.Add(new Coding() { Code = "procedure", System = "http://terminology.hl7.org/CodeSystem/observation-category", Display = "Procedure" });
            eob.Code = new CodeableConcept();
            eob.Code.Coding.Add(new Coding() { System = "urn:oid:2.16.840.1.113883.6.24", Code = "131328", Display = "MDC_ECG_ELEC_POTL" });
            eob.Subject = new ResourceReference();
            eob.Subject.Reference = "reference";
            eob.Subject.Display = "P. van de Heuvel";
            eob.Effective = new FhirDateTime("2015-02-19T09:30:35+01:00");
            eob.Performer.Add(new ResourceReference() { Reference = "Practitioner/f005", Display = "A. Langeveld" });
            eob.Device = new ResourceReference();
            eob.Device.Display = "12 lead EKG Device Metric";
            var m = new Observation.ComponentComponent();
            m.Code = new CodeableConcept();
            m.Code.Coding.Add(new Coding() { System = "urn:oid:2.16.840.1.113883.6.24", Code = "131389", Display = "MDC_ECG_ELEC_POTL_I" });
            m.Value = new Hl7.Fhir.Model.SampledData()
            {
                Origin = new SimpleQuantity { Value = 2048 },
                Period = 10,
                Factor = (decimal)1.612,
                LowerLimit = -3300,
                UpperLimit = 3300,
                Dimensions = 1,
                Data = "2041 2043 2037 2047 2060 2062 2051 2023 2014 2027 2034 2033 2040 2047 2047 2053 2058 2064 2059 2063 2061 2052 2053 2038 1966 1885 1884 2009 2129 2166 2137 2102 2086 2077 2067 2067 2060 2059 2062 2062 2060 2057 2045 2047 2057 2054 2042 2029 2027 2018 2007 1995 2001 2012 2024 2039 2068 2092 2111 2125 2131 2148 2137 2138 2128 2128 2115 2099 2097 2096 2101 2101 2091 2073 2076 2077 2084 2081 2088 2092 2070 2069 2074 2077 2075 2068 2064 2060 2062 2074 2075 2074 2075 2063 2058 2058 2064 2064 2070 2074 2067 2060 2062 2063 2061 2059 2048 2052 2049 2048 2051 2059 2059 2066 2077 2073"
            };
            eob.Component.Add(m);
            var m1 = new Observation.ComponentComponent();
            m1.Code = new CodeableConcept();
            m1.Code.Coding.Add(new Coding() { System = "urn:oid:2.16.840.1.113883.6.24", Code = "131329", Display = "MDC_ECG_ELEC_POTL_I" });
            m1.Value = new Hl7.Fhir.Model.SampledData()
            {
                Origin = new SimpleQuantity() { Value = 2048 },
                Period = 10,
                Factor = (decimal)1.612,
                LowerLimit = -3300,
                UpperLimit = 3300,
                Dimensions = 1,
                Data = rawdata.ToString()
                //Data = "2041 2043 2037 2047 2060 2062 2051 2023 2014 2027 2034 2033 2040 2047 2047 2053 2058 2064 2059 2063 2061 2052 2053 2038 1966 1885 1884 2009 2129 2166 2137 2102 2086 2077 2067 2067 2060 2059 2062 2062 2060 2057 2045 2047 2057 2054 2042 2029 2027 2018 2007 1995 2001 2012 2024 2039 2068 2092 2111 2125 2131 2148 2137 2138 2128 2128 2115 2099 2097 2096 2101 2101 2091 2073 2076 2077 2084 2081 2088 2092 2070 2069 2074 2077 2075 2068 2064 2060 2062 2074 2075 2074 2075 2063 2058 2058 2064 2064 2070 2074 2067 2060 2062 2063 2061 2059 2048 2052 2049 2048 2051 2059 2059 2066 2077 2073"
            };
            eob.Component.Add(m1);
            var m2 = new Observation.ComponentComponent();
            m2.Code = new CodeableConcept();
            m2.Code.Coding.Add(new Coding() { System = "urn:oid:2.16.840.1.113883.6.24", Code = "131330", Display = "MDC_ECG_ELEC_POTL_I" });
            m2.Value = new Hl7.Fhir.Model.SampledData()
            {
                Origin = new SimpleQuantity() { Value = 2048 },
                Period = 10,
                Factor = (decimal)1.612,
                LowerLimit = -3300,
                UpperLimit = 3300,
                Dimensions = 1,
                Data = "2041 2043 2037 2047 2060 2062 2051 2023 2014 2027 2034 2033 2040 2047 2047 2053 2058 2064 2059 2063 2061 2052 2053 2038 1966 1885 1884 2009 2129 2166 2137 2102 2086 2077 2067 2067 2060 2059 2062 2062 2060 2057 2045 2047 2057 2054 2042 2029 2027 2018 2007 1995 2001 2012 2024 2039 2068 2092 2111 2125 2131 2148 2137 2138 2128 2128 2115 2099 2097 2096 2101 2101 2091 2073 2076 2077 2084 2081 2088 2092 2070 2069 2074 2077 2075 2068 2064 2060 2062 2074 2075 2074 2075 2063 2058 2058 2064 2064 2070 2074 2067 2060 2062 2063 2061 2059 2048 2052 2049 2048 2051 2059 2059 2066 2077 2073"
            };
            eob.Component.Add(m2);

            return eob;
        }
    }
}
