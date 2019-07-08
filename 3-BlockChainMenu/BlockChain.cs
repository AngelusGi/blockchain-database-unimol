using System;
using System.Collections.Generic;
using System.Linq;

//libreria per la gestione delle liste

namespace _3_BlockChainMenu
{

    /// <summary>Classe che, per mezzo di una lista, si occupa di emulare il funzionamento della blockchain</summary>

    internal class BlockChain
    {

        #region Membri        
        /// <summary> Lista di utenti partecipanti alla blockchain.</summary>
        public IList<Utente> Utenti { get; set; }

        /// <summary>gestisce le transazioni che devono ancora essere processate (minate)</summary>
        public IList<Transazione> TransazioniInAttesa = new List<Transazione>();

        //inizializza una lista concatenata di blocchi
        public IList<Blocco> Catena { set; get; }


        /// <summary>Sistema per aumentare la complessità dei calcoli necessari per validare la blockchain all'aumentare della dimensione (Proof of Work)</summary>
        /// <value>The difficoltà.</value>
        public int Difficoltà { get; set; } = 1;


        /// <summary>Con le transazioni si introduce il concetto di ricompensa, 1 moneta (UniMolCoin) per il lavoro svolto</summary>
        public static readonly int Ricompensa = 1;

        #endregion


        #region Costruttore

        //costruttore della classe blockchain che si occupa di istanziare il
        //primo blocco della catena ed eventuali successivi
        //istanzia anche una lista di utenti
        public BlockChain()
        {
            Utenti = new List<Utente>(3);
            InizializzaCatena();
            AggiungiBloccoIniziale();
        }

        #endregion


        #region MetodiPerBlocchi

        public void InizializzaCatena()
        {
            //creo il primo blocco della catena
            Catena = new List<Blocco>();
        }


        public void AggiungiBloccoIniziale()
        {
            Catena.Add(CreaBloccoIniziale());
        }


        #region Documentazione
        /// <summary>Nel caso in cui la catena fosse vuota, si occupa di generare un primo blocco</summary>
        /// <returns>Il primo blocco della blockchain</returns>
        #endregion
        public Blocco CreaBloccoIniziale()
        {
            TransazioniInAttesa = new List<Transazione>();
            var blocco = new Blocco(DateTime.Now, null, TransazioniInAttesa);
            blocco.Mina(Difficoltà);
            return blocco;
        }


        public Blocco GetUltimoBlocco()
        {
            return Catena[Catena.Count - 1];
        }


        #region Documentazione
        /// <summary>Si aggancia all'ultimo blocco disponibile e genera tutto il necessario per inserire in coda il blocco che si sta creando.</summary>
        /// <param name="blocco">Blocco della catena generato dal costruttore</param>
        #endregion
        public void AggiungiBlocco(Blocco blocco)
        {
            // prende i dati inerenti al blocco precedente rispetto a quello da aggiungere
            var ultimoBlocco = GetUltimoBlocco();

            // aumenta l'indice del blocco +1 rispetto a precedente
            blocco.Indice = ultimoBlocco.Indice + 1;

            // calcola il suo hash partendo da quello del precedente
            blocco.HashPrecedente = ultimoBlocco.HashBloccoCorrente;

            // dopo aver inserito difficoltà posso effettuare il mining
            blocco.Mina(Difficoltà);

            // aggiunge il blocco alla catena
            Catena.Add(blocco);

        }

        #endregion


        #region MetodiPerUtenti

        #region Documentazione

        /// <summary>
        /// Ricerca un utente all'interno della lista degli utenti.
        /// </summary>
        /// <param name="nome">Nome utente.</param>
        /// <returns>Oggetto di tipo utente</returns>
        #endregion
        public Utente RicercaUtente(string nome)
        {
            #region spiegazioneCodice
            //foreach (var utente in Utenti)
            //{
            //    if (utente.Nome == nome)
            //    {
            //        return utente;
            //    }
            //}
            //return null;
            //questo blocco di codice equivale all'espressione seguente
            #endregion

            return Utenti.FirstOrDefault(utente => utente.Nome == nome);
        }


        #region Documentazione

        /// <summary>
        /// Ricerca un utente.
        /// </summary>
        /// <param name="idUtente">ID univoco dell'utente.</param>
        /// <returns>Oggetto di tipo utentea</returns>

        #endregion
        public Utente RicercaUtente(int? idUtente)
        {
            #region spiegazioneCodice
            //foreach (var utente in Utenti)
            //{
            //    if (utente.Nome == nome)
            //    {
            //        return utente;
            //    }
            //}
            //return null;
            //questo blocco di codice equivale all'espressione seguente
            #endregion

            return Utenti.FirstOrDefault(utente => utente.IdUnivoco == idUtente);
        }

        #region Documentazione

        /// <summary>
        /// Verifica l'esistenza dell'utente all'interno della lista degli utenti.
        /// </summary>
        /// <param name="nome">Nome dell'utente da verificare.</param>
        /// <returns>Utente esiste (true/false)</returns>

        #endregion
        public bool VerificaUtente(string nome)
        {
            #region spiegazioneCodice
            //foreach (var utente in Utenti)
            //{
            //    if (utente.Nome == nome)
            //    {
            //        return true;
            //    }
            //}
            //return false;
            //questo blocco di codice equivale all'espressione seguente
            #endregion

            return Utenti.Any(utente => utente.Nome == nome);
        }

        #region Documentazione
        /// <summary>Aggiorna il portafogli di tutti gli utenti appartenenti alla blockchain</summary>
        #endregion 
        public void AggiornaSaldoUtenti()
        {

            foreach (var blocco in Catena)
            {
                foreach (var transazione in blocco.Transazioni)
                {
                    if (!transazione.Contabilizzata)
                    {
                        var mittente = RicercaUtente(transazione.IdMittente);
                        var destinatario = RicercaUtente(transazione.IdDestinatario);

                        if ((transazione.IdMittente != null) && (transazione.IdMittente == mittente.IdUnivoco) && (transazione.IdDestinatario == destinatario.IdUnivoco))
                        {
                            SmartContract.VerificaSaldo(mittente.Nome, destinatario.Nome, transazione.Valore);
                        }

                        transazione.Contabilizzata = true;
                    }
                }
            }
        }

        #endregion


        #region MetodiTransazioni

        public void CreaTransazione(Transazione transazione)
        {
            TransazioniInAttesa.Add(transazione);
        }

        #region Documentazione
        /// <summary>Genera un nuovo blocco e lo aggiunge alla catena al fine di validare una delle transazioni che devono essere ancora minate</summary>
        /// <param name="miner">Prende un oggetto di tipo utente (miner)</param>
        #endregion
        public void MinaTransazioni(Utente miner)
        {

            TransazioniInAttesa = new List<Transazione>();

            CreaTransazione(new Transazione(null, miner, Ricompensa));

            var blocco = new Blocco(DateTime.Now, GetUltimoBlocco().HashBloccoCorrente, TransazioniInAttesa);

            AggiungiBlocco(blocco);

            Difficoltà++;

            SmartContract.RicompensaMiner(miner);

            AggiornaSaldoUtenti();

        }

        #endregion


        #region Documentazione
        /// <summary>
        /// Scorre tutta la catena e ricalcola a runtime l'hash del blocco che sta analizzando in quel momento e lo confronta con quello del precedente.
        /// Nel caso in cui uno dei due fosse alterati (quindi mancata coincidenza degli hash) allora restituisce false e invalida la catena.
        /// </summary>
        /// <returns>Restituisce lo stato di validità di un blocco</returns>
        #endregion
        public bool IsValido()
        {
            return SmartContract.ValidaBlockchain();
        }


        #region Documentazione
        /// <summary>Restituisce il numero totale di coin in circolazione</summary>
        /// <returns>Num monete</returns>
        #endregion
        public int AggiornaBilancio()
        {
            #region SpiegazioneCodice

            //foreach (Utente utente in Utenti)
            //{
            //    bilancio += utente.Saldo.Count;
            //}
            //return bilancio;

            #endregion

            return Utenti.Sum(utente => utente.Saldo.Count);
        }
    }
}
