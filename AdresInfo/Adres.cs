namespace AdresInfoCL
{
    public class Adres
    {
        public string Provincie { get; set; }
        public string Gemeente { get; set; }
        public string Straat { get; set; }


        public Adres(string provincie, string gemeente, string straat)
        {
            Provincie = provincie;
            Gemeente = gemeente;
            Straat = straat;
        }
    }
}
