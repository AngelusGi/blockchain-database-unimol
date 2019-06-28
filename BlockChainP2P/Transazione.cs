
namespace BlockChain
{
    internal class Transazione
    {
        #region Membri

        public string IndirizzoSorgente { get; set; }
        public string IndirizzoDestinazione { get; set; }
        public int Valore { get; set; }

        #endregion


        public Transazione(string sorgente, string destinazione, int valore)
        {

            IndirizzoSorgente = sorgente;
            IndirizzoDestinazione = destinazione;
            Valore = valore;

        }
    }
}
