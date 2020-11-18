using System;
using HL7FHIRConsole31.Boundary;

namespace HL7FHIRConsole31
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test af HL7FHIR!");
            HL7FHIRR4Boundary hl7fhirclient = new HL7FHIRR4Boundary();
            hl7fhirclient.Boundary_HL7FHIR_REST();

        }
    }
}
