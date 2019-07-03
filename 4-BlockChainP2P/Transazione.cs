namespace _4_BlockChainP2P
{

    /// <summary>Gestione della transazione associata ad un blocco della catena</summary>
    internal class Transazione
    {

        #region Membri


        /// <summary>Indirizzo di chi effettua (<em>fa partire</em>) la transazione</summary>
        /// <value>Indirizzo mittente.</value>
        //la dicitura int? indica che potrebbe essere null
        public int? IdMittente { get; private set; }

        /// <summary>
        ///   <para>Indirizzo del destinatario della transazione</para>
        /// </summary>
        /// <value>Indirizzo destinatario.</value>
        public int IdDestinatario { get; private set; }


        /// <summary>Valore della transazione.</summary>
        /// <value>Valore.</value>
        public int Valore { get; private set; }

        public bool Contabilizzata { get; set; }

        #endregion


        public Transazione(Utente mittente, Utente destinatario, int valore)
        {
            //se indirizzo mittente non è null, allora assegna l'id del mittente al campo IdMittente
            IdMittente = mittente?.IdUnivoco;

            if (destinatario.IdUnivoco != null)
            {
                IdDestinatario = (int)destinatario.IdUnivoco;
            }

            Valore = valore;
            Contabilizzata = false;

        }
    }
}
