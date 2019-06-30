﻿
namespace BlockChainMenu
{

    /// <summary>Gestione della transazione associata ad un blocco della catena</summary>
    internal class Transazione
    {
        #region Membri


        /// <summary>Indirizzo di chi effettua (<em>fa partire</em>) la transazione</summary>
        /// <value>Indirizzo mittente.</value>
        public string IndirizzoMittente { get; private set; }

        /// <summary>
        ///   <para>Indirizzo del destinatario della transazione</para>
        /// </summary>
        /// <value>Indirizzo destinatario.</value>
        public string IndirizzoDestinatario { get; private set; }


        /// <summary>Valore della transazione.</summary>
        /// <value>Valore.</value>
        public int Valore { get; private set; }

        #endregion


        public Transazione(Utente mittente, Utente destinatario, int valore)
        {
            //se indirizzo mittente non è null, allora assegna il nome del mittente
            IndirizzoMittente = mittente?.Nome;
            IndirizzoDestinatario = destinatario.Nome;
            Valore = valore;

        }
    }
}