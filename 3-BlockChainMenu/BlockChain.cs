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
            Blocco blocco = new Blocco(DateTime.Now, null, TransazioniInAttesa);
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
            Blocco ultimoBlocco = GetUltimoBlocco();

            // aumenta l'indice del blocco +1 rispetto a precedente
            blocco.Indice = ultimoBlocco.Indice + 1;

            // calcola il suo hash partendo da quello del precedente
            blocco.HashPrecedente = ultimoBlocco.HashBloccoCorrente;

            // dopo aver inserito difficoltà posso effettuare il mining
            blocco.Mina(Difficoltà);

            // aggiunge il blocco alla catena
            Catena.Add(blocco);

        }

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

        /// <summary>
        /// Ricerca un utente all'interno della lista degli utenti.
        /// </summary>
        /// <param name="nome">Nome utente.</param>
        /// <returns>Oggetto di tipo utente</returns>
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

        /// <summary>
        /// Ricerca un utente.
        /// </summary>
        /// <param name="idUtente">ID univoco dell'utente.</param>
        /// <returns>Oggetto di tipo utentea</returns>
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

        /// <summary>
        /// Verifica l'esistenza dell'utente all'interno della lista degli utenti.
        /// </summary>
        /// <param name="idUtente">ID univoco dell'utente da verificare.</param>
        /// <returns>Utente esiste (true/false)</returns>
        public bool VerificaUtente(int idUtente)
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

            return Utenti.Any(utente => utente.IdUnivoco == idUtente);
        }

        #region Documentazione
        /// <summary>Aggiorna il portafogli di tutti gli utenti appartenenti alla blockchain</summary>
        #endregion 
        public void AggiornaSaldoUtenti()
        {
            //Transazione ultimaTransazione = GetUltimaTransazione();

            ////equivale a scrivere if ((ultimaTransazione != null) && (ultimaTransazione.IdMittente != null))
            //if (ultimaTransazione?.IdMittente != null)
            //{
            //    Utente mittente = RicercaUtente(ultimaTransazione.IdMittente);
            //    if ((ultimaTransazione.IdMittente != null) && (ultimaTransazione.IdMittente == mittente.IdUnivoco))
            //    {
            //        mittente.Saldo -= ultimaTransazione.Valore;
            //    }
            //}


            //if (ultimaTransazione != null)
            //{
            //    Utente utenteCercato = RicercaUtente(ultimaTransazione.IdDestinatario);
            //    if (ultimaTransazione.IdDestinatario == utenteCercato.IdUnivoco)
            //    {
            //        utenteCercato.Saldo += ultimaTransazione.Valore;
            //    }
            //}

            foreach (Blocco blocco in Catena)
            {
                foreach (Transazione transazione in blocco.Transazioni)
                {
                    if (!transazione.Contabilizzata)
                    {
                        Utente mittente = RicercaUtente(transazione.IdMittente);
                        Utente destinatario = RicercaUtente(transazione.IdDestinatario);
                        if ((transazione.IdMittente != null) && (transazione.IdMittente == mittente.IdUnivoco) && (transazione.IdDestinatario == destinatario.IdUnivoco))
                        {
                            SmartContract.VerificaSaldo(mittente.Nome, destinatario.Nome, transazione.Valore);
                        }

                        transazione.Contabilizzata = true;
                    }
                }
            }

            //foreach (Transazione transazione in TransazioniInAttesa)
            //{
            //    transazione.Contabilizzata = false;
            //}
        }


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

            Blocco blocco = new Blocco(DateTime.Now, GetUltimoBlocco().HashBloccoCorrente, TransazioniInAttesa);
            AggiungiBlocco(blocco);
            Difficoltà++;
            SmartContract.RicompensaMiner(miner);
            AggiornaSaldoUtenti();

        }

        #region Documentazione
        /// <summary>Restituisce il numero totale di coin in circolazione</summary>
        /// <returns>Num monete</returns>
        #endregion 
        public int AggiornaBilancio()
        {

            //AggiornaSaldoUtenti();

            int bilancio = 0;

            foreach (Utente utente in Utenti)
            {
                bilancio += utente.Saldo.Count;
            }

            return bilancio;
        }
    }
}
