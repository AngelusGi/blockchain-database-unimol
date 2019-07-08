using System.Collections.Generic;

namespace _4_BlockChainP2P
{

    #region Documentazione
    /// <summary>
    /// Classe che gestisce gli utenti della blockchain
    /// </summary>
    #endregion
    internal class Utente
    {

        #region Documentazione
        /// <value>
        /// Nome dell'utente.
        /// </value>
        #endregion
        public string Nome { get; private set; }



        #region Documentazione
        /// <value>
        /// Saldo del portafogli dell'utente.
        /// </value>
        #endregion
        internal Stack<Moneta> Saldo { get; private set; }



        #region Documentazione
        /// <value>
        /// Numero identificativo univoco dell'utente.
        /// </value>
        #endregion
        public int? IdUnivoco { get; set; }


        private const int NumMoneteIniziale = 10;


        #region Documentazione
        /// <summary>
        /// Costruttore classe utente. In fase di inizializzazione IdUnivoco è imposto a null.
        /// Il saldo iniziale per ogni utente è pari a 10 monete.
        /// </summary>
        /// <param name="nome">Nome dell'utente</param>
        #endregion
        public Utente(string nome)
        {
            Nome = nome;
            Saldo = new Stack<Moneta>(10);
            IdUnivoco = null;
        }


        #region Documentazione
        /// <summary>
        /// Crea un portafogli iniziale pari a 10 monete.
        /// </summary>
        #endregion
        public void CreaPortafogli()
        {

            for (var i = 0; i < NumMoneteIniziale; i++)
            {
                if (IdUnivoco != null)
                {
                    Saldo.Push(new Moneta((int)IdUnivoco));
                }
            }

        }

    }
}