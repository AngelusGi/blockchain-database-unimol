using System;

//libreria per la gestione delle liste
using System.Collections.Generic;

namespace BlockChain
{
    class BlockChain
    {

        //gestisce le transazioni che devono ancora essere processate
        private IList<Transazione> TransazioniInAttesa = new List<Transazione>();

        //inizializza una lista concatenata di blocchi
        public IList<Blocco> Catena { set; get; }

        //sistema per aumentare la complessità all'aumentare della dimensione della catena (Proof of Work)
        public int Difficolta { get; set; } = 2;

        //con le transazioni si intruce il concetto di ricompensa, 1 moneta (UniMolCoin) per il lavoro svolto
        public int Ricompensa = 1;

        //costruttore della classe blockchain che si occupa di istanzare il
        //primo blocco della caena ed eventuali successivi
        public BlockChain()
        {
            InizializzaCatena();
            AggiungiBloccoIniziale();
        }

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

        public Blocco CreaBloccoIniziale()
        {
            Blocco blocco = new Blocco(DateTime.Now, null, TransazioniInAttesa);
            blocco.Mina(Difficolta);
            TransazioniInAttesa = new List<Transazione>();
            return blocco;
        }

        public Blocco GetUltimoBlocco()
        {
            return Catena[Catena.Count - 1];
        }

        public void AggiungiBlocco(Blocco blocco)
        {
            //prende i dati inerenti al blocco precedente rispetto a quello da aggiungere
            Blocco ultimoBlocco = GetUltimoBlocco();

            //aumenta l'indice del blocco +1 rispetto a precedente
            blocco.Indice = ultimoBlocco.Indice + 1;

            // calcola il suo hash partendo da quello del precedente
            blocco.HashPrecedente = ultimoBlocco.HashBloccoCorrente;

            //dopo aver inserito difficoltà posso integrare operazioni di mining
            blocco.Mina(Difficolta);

            //aggiunge il blocco alla catena
            Catena.Add(blocco);

        }

        //verifica l'integrità della blockchain
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

        public void GesticiTransazioniInAttesa(string indirizzoMiner)
        {
            Blocco blocco = new Blocco(DateTime.Now, GetUltimoBlocco().HashBloccoCorrente, TransazioniInAttesa);
            AggiungiBlocco(blocco);

            TransazioniInAttesa = new List<Transazione>();

            CreaTransazione(new Transazione(null, indirizzoMiner, Ricompensa));
        }

        public int GetBilancio(string indirizzo)
        {
            int bilancio = 0;

            for (int posBlocco = 0; posBlocco < Catena.Count; posBlocco++)
            {
                for (int posTransazione = 0; posTransazione < Catena[posBlocco].Transazioni.Count; posTransazione++)
                {
                    Transazione transazione = Catena[posBlocco].Transazioni[posTransazione];

                    if (transazione.IndirizzoSorgente == indirizzo)
                    {
                        bilancio -= transazione.Valore;
                    }

                    if (transazione.IndirizzoDestinazione == indirizzo)
                    {
                        bilancio += transazione.Valore;
                    }

                }
            }

            return bilancio;
        }
    }
}
