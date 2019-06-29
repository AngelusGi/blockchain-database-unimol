using System;

//libreria per la gestione delle liste
using System.Collections.Generic;

namespace BlockChain
{

    /// <summary>Classe che, per mezzo di una lista, si occupa di emulare il funzionamento della blockchain</summary>
    internal class BlockChain
    {

        #region Membri


        /// <summary>gestisce le transazioni che devono ancora essere processate (minate)</summary>
        public IList<Transazione> TransazioniInAttesa = new List<Transazione>();

        //inizializza una lista concatenata di blocchi
        public IList<Blocco> Catena { set; get; }


        /// <summary>Sistema per aumentare la complessità dei calcoli necessari per validare la blockchain all'aumentare della dimensione (Proof of Work)</summary>
        /// <value>The difficoltà.</value>
        public int Difficoltà { get; set; } = 2;


        /// <summary>Con le transazioni si introduce il concetto di ricompensa, 1 moneta (UniMolCoin) per il lavoro svolto</summary>
        public int Ricompensa = 1;

        #endregion


        #region Costruttore

        //costruttore della classe blockchain che si occupa di istanziare il
        //primo blocco della catena ed eventuali successivi
        public BlockChain()
        {
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
            //Catena.Add(AggiungiBlocco());
            Catena.Add(CreaBloccoIniziale());
        }

        /// <summary>Nel caso in cui la catena fosse vuota, si occupa di generare un primo blocco</summary>
        /// <returns>Il primo blocco della blockchain</returns>
        public Blocco CreaBloccoIniziale()
        {
            Blocco blocco = new Blocco(DateTime.Now, null, TransazioniInAttesa);
            blocco.Mina(Difficoltà);
            TransazioniInAttesa = new List<Transazione>();
            return blocco;
        }

        public Blocco GetUltimoBlocco()
        {
            return Catena[Catena.Count - 1];
        }

        /// <summary>Si aggancia all'ultimo blocco disponibile e genera tutto il necessario per inserire in coda il blocco che si sta creando.</summary>
        /// <param name="blocco">Blocco della catena generato dal costruttore</param>
        public void AggiungiBlocco(Blocco blocco)
        {
            //prende i dati inerenti al blocco precedente rispetto a quello da aggiungere
            Blocco ultimoBlocco = GetUltimoBlocco();

            //aumenta l'indice del blocco +1 rispetto a precedente
            blocco.Indice = ultimoBlocco.Indice + 1;

            // calcola il suo hash partendo da quello del precedente
            blocco.HashPrecedente = ultimoBlocco.HashBloccoCorrente;

            //dopo aver inserito difficoltà posso effettuare il mining
            blocco.Mina(Difficoltà);

            //aggiunge il blocco alla catena
            Catena.Add(blocco);

        }

        /// <summary>
        /// Scorre tutta la catena e ricalcola a runtime l'hash del blocco che sta analizzando in quel momento e lo confronta con quello del precedente.
        /// Nel caso in cui uno dei due fosse alterati (quindi mancata coincidenza degli hash) allora restituisce false e invalida la catena.
        /// </summary>
        /// <returns>Restituisce lo stato di validità di un blocco</returns>
        public bool IsValido()
        {

            //finché ci sono blocchi
            for (int pos = 1; pos < Catena.Count; pos++)
            {
                Blocco bloccoCorrente = Catena[pos];
                Blocco bloccoPrecedente = Catena[pos - 1];

                //ricalcola l'hash del blocco analizzato, se è diverso da quello memorizzato ritorna false (catena non valida)
                if (bloccoCorrente.HashBloccoCorrente != bloccoCorrente.CalcolaHash())
                {
                    return false;
                }

                //ricalcola l'hash del blocco precedente, se è diverso da quello memorizzato ritorna false (catena non valida)
                if (bloccoCorrente.HashPrecedente != bloccoPrecedente.HashBloccoCorrente)
                {
                    return false;
                }
            }

            //se tutti i blocchi sono coerenti tra valore presente e valore aspetta, ritorna true (catena valida)
            return true;

        }

        public void CreaTransazione(Transazione transazione)
        {
            TransazioniInAttesa.Add(transazione);
        }

        /// <summary>Genera un nuovo blocco e lo aggiunge alla catena al fine di validare una delle transazioni che devono essere ancora minate</summary>
        /// <param name="indirizzoMiner">Prende l'indirizzo del miner della transazione</param>
        /// <returns>Restituisce il nuovo saldo</returns>
        public void MinaTransazioni(string indirizzoMiner)
        {
            Blocco blocco = new Blocco(DateTime.Now, GetUltimoBlocco().HashBloccoCorrente, TransazioniInAttesa);
            AggiungiBlocco(blocco);

            TransazioniInAttesa = new List<Transazione>();

            CreaTransazione(new Transazione(null, indirizzoMiner, Ricompensa));
        }

        /// <summary>Il primo if decrementa saldo del mittente, il secondo aumenta saldo del destinatario</summary>
        /// <param name="indirizzo">Prende l'indirizzo di destinazione della transazione</param>
        /// <returns>Restituisce il nuovo saldo</returns>
        public int GetBilancio(string indirizzo)
        {
            int bilancio = 0;

            foreach (Blocco blocco in Catena)
            {
                foreach (Transazione transazione in blocco.Transazioni)
                {

                    if (transazione.IndirizzoMittente == indirizzo)
                    {
                        bilancio -= transazione.Valore;
                    }

                    if (transazione.IndirizzoDestinatario == indirizzo)
                    {
                        bilancio += transazione.Valore;
                    }
                }
            }

            return bilancio;
        }
    }
}
