using System.Collections.Generic;

namespace _3_BlockChainMenu
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
        internal Stack<Moneta> Saldo { get; private set; }

        /// <value>
        /// Numero identificativo univoco dell'utente.
        /// </value>
        public int? IdUnivoco { get; set; }

        private static readonly int NumMoneteIniziale = 10;
        
        /// <summary>
        /// Costruttore classe utente. In fase di inizializzazione IdUnivoco è imposto a null.
        /// Il saldo iniziale per ogni utente è pari a 10 monete.
        /// </summary>
        /// <param name="nome">Nome dell'utente</param>
        public Utente(string nome)
        {
            Nome = nome;
            Saldo = new Stack<Moneta>(10);
            IdUnivoco = null;
        }

        /// <summary>
        /// Crea un portafogli iniziale pari a 10 monete.
        /// </summary>
        public void CreaPortafogli()
        {

            for (int i = 0; i < NumMoneteIniziale; i++)
            {
                Saldo.Push(new Moneta((int)IdUnivoco));
            }

        }

    }
}