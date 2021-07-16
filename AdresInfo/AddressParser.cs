using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdresInfoCL
{
    public class AddressParser
    {
        private string _rawData { get; set; }

        public void Fetch(string path)
        {
            if (File.Exists(path))
            {
                _rawData = File.ReadAllText(path);
            }
            else
            {
                throw new AccessViolationException($"Het bestand die nodig is om AddressParser te gebruiken kon niet gevonden worden.\nOpgegeven pad:{path}");
            }
        }

        public List<Adres> Converteer(string rawData = null)
        {
            if (rawData != null) { _rawData = rawData; } // kan verschillende datasets converteren zonder een nieuw object te hoeven instantieren

            List<Adres> adresLijst = new List<Adres>();

            using (StringReader reader = new StringReader(_rawData))
            {
                string line = string.Empty;
                while(line != null)
                {
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        string[] adresParams = line.Split(',');
                        adresLijst.Add(new Adres(adresParams[0], adresParams[1], adresParams[2]));
                    }
                }
            }

            return adresLijst;
        }
    }
}
