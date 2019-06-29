
namespace BlockChainP2P
{

    /// <summary>Gestione della transazione associata ad un blocco della catena</summary>
    internal class Transazione
    {
        #region Membri


        /// <summary>Indirizzo di chi effettua (<em>fa partire</em>) la transazione</summary>
        /// <value>The indirizzo mittente.</value>
        public string IndirizzoMittente { get; private set; }

        /// <summary>
        ///   <para>Indirizzo del destinatario della transazione</para>
        /// </summary>
        /// <value>The indirizzo destinatario.</value>
        public string IndirizzoDestinatario { get; private set; }


        /// <summary>Valore della transazione.</summary>
        /// <value>The valore.</value>
        public int Valore { get; private set; }

        #endregion


        public Transazione(Utente mittente, Utente destinatario, int valore)
        {
            //if (mittente != null)
            //{
            //    IndirizzoMittente = mittente.Nome;
            //}
            //else
            //{
            //    IndirizzoMittente = null;
            //}
            //questo blocco di codice equivale all'espressione seguente

            IndirizzoMittente = mittente?.Nome;

            IndirizzoDestinatario = destinatario.Nome;
            Valore = valore;

        }

    }
}
