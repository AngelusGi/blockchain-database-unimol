
namespace _2_BlockChainPoW
{
    internal class Transazione
    {
        #region Membri

        public string IndirizzoSorgente { get; private set; }
        public string IndirizzoDestinazione { get; private set; }
        public int Valore { get; private set; }

        #endregion


        public Transazione(string sorgente, string destinazione, int valore)
        {

            IndirizzoSorgente = sorgente;
            IndirizzoDestinazione = destinazione;
            Valore = valore;

        }
    }
}
