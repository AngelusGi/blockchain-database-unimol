namespace BlockChainMenu
{
    /// <summary>
    /// Classe che gestisce gli utenti della blockchain
    /// </summary>
    public class Utente
    {
        
        /// <value>
        /// Nome dell'utente.
        /// </value>
        public string Nome { get; private set; }
        
        /// <value>
        /// Saldo del portafogli dell'utente.
        /// </value>
        public int Saldo { get; set; }

        /// <value>
        /// Numero identificativo univoco dell'utente.
        /// </value>
        public int? IdUnivoco { get; set; }

        
        /// <summary>
        /// Costruttore classe utente. In fase di inizializzazione IdUnivoco è imposto a null.
        /// Il saldo iniziale per ogni utente è pari a 10 monete.
        /// </summary>
        /// <param name="nome">Nome dell'utente</param>
        public Utente(string nome)
        {
            Nome = nome;
            Saldo = 10;
            IdUnivoco = null;
        }
    }
}