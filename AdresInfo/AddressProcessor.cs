using System;
using System.Linq;
using System.Collections.Generic;

namespace AdresInfoCL
{
    public class AddressProcessor
    {
        private List<Adres> _adressen { get; set; }
        private string segmentBarrier = "\n██████████████████████████████████████████████████\n";

        public AddressProcessor(List<Adres> adressen)
        {
            _adressen = (adressen.Count > 0) ? adressen : throw new ArgumentException("Er moet tenminste 1 adres in de lijst zitten om deze te kunnen verwerken.");
        }
        
        // 1
        public void lijstProvinciesAlfabetisch()
        {
            // GROUP BY: unieke lijst provincies
            // SELECT: dit selecteren
            // ORDER BY: sorteren adhv kolom Provincie uit geselecteerde data
            var res = _adressen.GroupBy(x => x.Provincie)
                               .Select((x, Provincie) => new { Provincie = x.Key })
                               .OrderBy(x => x.Provincie);

            Console.WriteLine(segmentBarrier);
            Console.WriteLine(" ▼ Lijst met provincies");
            foreach(var item in res)
            {
                Console.WriteLine($"\t{item.Provincie}");
            }

            Console.WriteLine(segmentBarrier);
        }

        // 2
        public void straatnamenVoorGemeente(string naam)
        {
            // WHERE: geef lijst van straatnamen voor opgegeven gemeente
            // SELECT: enkel de straatnamen (dit kan als overbodig aanschouwd worden)
            var res = _adressen.Where(x => x.Gemeente == naam)
                               .Select((x, Straat) => new { Straat = x.Straat });

            Console.WriteLine(segmentBarrier);
            Console.WriteLine($" ▼ Lijst van straatnamen voor Gemeente {naam}");
            foreach(var item in res)
            {
                Console.WriteLine($"\t{item.Straat}");
            }
            Console.WriteLine(segmentBarrier);

        }

        // 3
        public void populairsteStraatnaam()
        {
            // Selecteer de straatnaam die het meest keren voorkomt
            // Druk voor elk voorkomen de provincie, gemeente, straat af
            // Sorteer op basis van provincie en gemeente -- OrderBy
            var resStraatNaam = _adressen.GroupBy(x => x.Straat)
                                         .Select((x, Straat) => new { aantal = x.Count(), Straat = x.Key })
                                         .OrderByDescending(x => x.aantal)
                                         .Take(1)
                                         .First()
                                         .Straat;

            var resExtraInfo = _adressen.Where(x => x.Straat == resStraatNaam);

            Console.WriteLine(segmentBarrier);
            Console.WriteLine($" ▼ Populairste straatnaam: {resStraatNaam}");
            foreach(var item in resExtraInfo)
            {
                Console.WriteLine($"\t{resStraatNaam} gelegen in {item.Gemeente}, {item.Provincie}");
            }
            Console.WriteLine(segmentBarrier);
        }

        // 4
        public Dictionary<string,int> analogeFunctiePopulaireStraatnamen(int limit)
        {
            // de meest voorkomende straatnamen WEERGEEFT (Console.Writeline) met (GROUP BY, SELECT)
            // een parameter die aangeeft hoeveel straatnamen (LIMIT)
            // extratje (staat niet in opgave): sortering adhv ORDER BY
            var res = _adressen.GroupBy(x => x.Straat)
                               .Select((x, Straat) => new { aantal = x.Count(), Straat = x.Key })
                               .OrderByDescending(x => x.aantal)
                               .Take(limit);

            Dictionary<string, int> output = new Dictionary<string, int>();

            Console.WriteLine(segmentBarrier);
            if (res.Count() < limit)
            {
                Console.WriteLine($"Waarschuwing: er werden {limit} adressen gevraagd maar er konden er slechts {res.Count()} gevonden worden.\n");
            }

            Console.WriteLine(" ▼ Lijst met populairste straatnamen");
            foreach (var item in res)
            {
                output.Add(item.Straat, item.aantal);
                Console.WriteLine($"\t{item.Straat}: {item.aantal}x gevonden");
            }
            Console.WriteLine(segmentBarrier);

            return output;
            // "Output analoog aan voorgaande functie" is geinterpreteerd als zijnde het retourneren van een data object
            // indien dit niet gewenst is kan dit weggelaten worden (void + geen output variabele)
        }

        // 5
        public void gemeenschappelijkeStraatnamenDuoGemeente(string gemeente1, string gemeente2)
        {
            var res1 = _adressen.Where(x => x.Gemeente == gemeente1);
            var res2 = _adressen.Where(x => x.Gemeente == gemeente2);

            List<string> gemeenschappelijkeStraten = new List<string>();

            // kon dit efficienter met een join?
            foreach(var item in res1)
            {
                foreach(var item2 in res2)
                {
                    if(item2.Straat == item.Straat)
                    {
                        gemeenschappelijkeStraten.Add(item.Straat);
                    }
                }
            }

            Console.WriteLine(segmentBarrier);
            Console.WriteLine($" ▼ Gemeenschappelijke straatnamen tussen {gemeente1} en {gemeente2}");
            foreach(string straat in gemeenschappelijkeStraten)
            {
                Console.WriteLine($"\t{straat}");
            }
            Console.WriteLine(segmentBarrier);
        }

        // 6
        public void zoekUniekeStraatnamen(string gemeente)
        {
            var res1 = _adressen.Where(x => x.Gemeente == gemeente)
                                .Select((x, Straat) => new { Straat = x.Straat })
                                .ToList();

            var res2 = _adressen.Where(x => x.Gemeente != gemeente)
                                .Select((x, Straat) => new { Straat = x.Straat })
                                .ToList();

            List<string> uniekeStraten = new List<string>();

            // kon dit efficienter met een join ?
            foreach(var item in res1)
            {
                if (!res2.Contains(item))
                {
                    uniekeStraten.Add(item.Straat);
                }
            }

            Console.WriteLine(segmentBarrier);
            Console.WriteLine($" ▼ Unieke straatnamen uit {gemeente} welke nergens anders voorkomen:");
            foreach(string s in uniekeStraten)
            {
                Console.WriteLine($"\t{s}");
            }

            Console.WriteLine(segmentBarrier);
        }

        // 7
        public void gemeenteMetMeesteStraten()
        {
            var res = _adressen.GroupBy(x => x.Gemeente)
                               .Select((x, Gemeente) => new { aantal = x.Count(), Gemeente = x.Key})
                               .OrderByDescending(x => x.aantal)
                               .Take(1)
                               .First();

            Console.WriteLine(segmentBarrier);
            Console.WriteLine(" ▼ De gemeente met de meeste straten:");
            Console.WriteLine($"\t{res.Gemeente}: {res.aantal} straten");
            Console.WriteLine(segmentBarrier);
        }

        // 8
        public void langsteStraatnaam()
        {
            var res = _adressen.GroupBy(x => x.Straat)
                               .Select((x, Straat) => new { lengte = x.Key.Length, Straat = x.Key })
                               .OrderByDescending(x => x.lengte)
                               .Take(1)
                               .First();

            Console.WriteLine(segmentBarrier);
            Console.WriteLine(" ▼ De langste straatnaam:");
            Console.WriteLine($"\t{res.Straat}: {res.lengte} karakters");
            Console.WriteLine(segmentBarrier);
        }

        // 9
        // "Geef naast de langste straatnaam ook de gemeente en provincie weer"
        // Interpretatie 1: Geef de langste straatnaam, voor deze straatnaam geef de gemeente en provincie die deze straatnaam bevat
        // De eerste interpretatie werd uitgewerkt, deze leek het moeilijkst van de 2.
        // Interpretatie 2: Geef de langste straatnaam, langste gemeente en provincie.
        public void langsteStraatnaamVolledig()
        {
            var res = (from adres in _adressen orderby adres.Straat.Length descending select adres).ToList().First();

            Console.WriteLine(segmentBarrier);
            Console.WriteLine(" ▼ De langste straatnaam (volledig):");
            Console.WriteLine($"\t{res.Straat} is de langste straat, gelegen in {res.Gemeente} - {res.Provincie}");
            Console.WriteLine(segmentBarrier);
        }

        // 10
        public void uniekeStraatnamenVolledig()
        {
            var res = _adressen.GroupBy(x => x.Straat)
                               .Select((x, Straat) => new { count = x.Count(), Straat = x.Key })
                               .Where(x => x.count == 1)
                               .OrderBy(x => x.count)
                               .ToList();

            Console.WriteLine(segmentBarrier);
            Console.WriteLine(" ▼ Unieke straatnamen:");
            foreach (var i in res)
            {
                var addit = (from adres in _adressen where adres.Straat == i.Straat select adres).ToList().First();
                Console.WriteLine($"\t{i.Straat} uit {addit.Gemeente}, {addit.Provincie}");
            }
            Console.WriteLine(segmentBarrier);
        }

        // 11
        public void uniekeStraatnamenVoorGemeente(string gemeente)
        {
            var resGlobal = _adressen.GroupBy(x => x.Straat)
                               .Select((x, Straat) => new { count = x.Count(), Straat = x.Key })
                               .Where(x => x.count == 1)
                               .OrderBy(x => x.count)
                               .ToDictionary(x=> x.Straat, x=>x.count);

            var resGemeente = _adressen.Where(x => x.Gemeente == gemeente);

            Console.WriteLine(segmentBarrier);
            Console.WriteLine($" ▼ Unieke straatnamen voor {gemeente}:");
            
            foreach(var row in resGemeente)
            {
                if (resGlobal.ContainsKey(row.Straat)) // containskey aangezien de resGlobal.Where reeds filtert
                {
                    Console.WriteLine($"\t{row.Straat}");
                }
            }

            Console.WriteLine(segmentBarrier);
        }
    }
}
