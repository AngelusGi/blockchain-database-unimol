
namespace BlockChain
{
    internal class Transazione
    {
        /// <summary>
        /// Gestione della transazione associata ad un blocco della catena
        /// </summary>
        #region Membri

        public string IndirizzoMittente { get; private set; }
        public string IndirizzoDestinatario { get; private set; }
        public int Valore { get; private set; }

        #endregion


        public Transazione(string mittente, string destinatario, int valore)
        {

            IndirizzoMittente = mittente;
            IndirizzoDestinatario = destinatario;
            Valore = valore;

        }
    }
}
