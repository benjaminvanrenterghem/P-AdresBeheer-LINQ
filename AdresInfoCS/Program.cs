using System;
using System.Collections.Generic;
using System.IO;
using AdresInfoCL;

namespace AdresInfoCS
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data/LinqAdresInfo.txt");

            // bestand uitlezen en parsen
            AddressParser AddressParser = new AddressParser();
            AddressParser.Fetch(path);
            List<Adres> parseResultaat = AddressParser.Converteer();

            // parsed data naar processor
            AddressProcessor AddressProcessor = new AddressProcessor(parseResultaat);

            // executie van verschillende functies toebehorend aan processor

            Console.WriteLine("Long-running? (Inclusief uniekeStraatnamenVolledig) (y/n):");
            var x = Console.ReadLine();
            if (x.Contains("y"))
            {
                AddressProcessor.uniekeStraatnamenVolledig(); // 10 
            }

            AddressProcessor.lijstProvinciesAlfabetisch(); // 1

            AddressProcessor.straatnamenVoorGemeente("Aartselaar"); // 2
            AddressProcessor.straatnamenVoorGemeente("Kuurne"); // 2

            AddressProcessor.populairsteStraatnaam(); // 3
            AddressProcessor.analogeFunctiePopulaireStraatnamen(10);
            AddressProcessor.gemeenschappelijkeStraatnamenDuoGemeente("Aartselaar", "Gent");
            AddressProcessor.zoekUniekeStraatnamen("Aartselaar");
            AddressProcessor.gemeenteMetMeesteStraten();
            AddressProcessor.langsteStraatnaam();
            AddressProcessor.langsteStraatnaamVolledig(); // 9
            AddressProcessor.uniekeStraatnamenVoorGemeente("Aartselaar"); // 11
            
        }
    }
}
