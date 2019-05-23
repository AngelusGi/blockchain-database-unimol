using System;

//libreria per la gestione delle liste
using System.Collections.Generic;

namespace BlockChain
{
    class BlockChain
    {

        //inizializza una lista concatenata di blocchi
        public IList<Blocco> Catena { set; get; }

        //sistema per aumentare la complessità all'aumentare della dimensione della catena (Proof of Work)
        private int Difficolta { get; set; } = 2;

        //costruttore della classe blockchain che si occupa di istanzare il
        //primo blocco della caena ed eventuali successivi
        public BlockChain()
        {
            InizializzaCatena();
            AggiungiBlocco();
        }

        public void InizializzaCatena()
        {
            //creo il primo blocco della catena
            Catena = new List<Blocco>();
        }

        public Blocco AggiungiBLocco()
        {
            //aggiungo un blocco passando al costruttore (data, il valore dell'hash precedente, transazione)
            return new Blocco(DateTime.Now, null, "{}");
        }

        public void AggiungiBlocco()
        {
            Catena.Add(AggiungiBLocco());
        }

        public Blocco GetUltimoBlocco()
        {
            return Catena[Catena.Count - 1];
        }

        public void AggiungiBlocco(Blocco blocco)
        {
            //prende i dati inerenti al blocco precedente rispetto a quello da aggiungere
            Blocco latestBlock = GetUltimoBlocco();

            //aumenta l'indice del blocco +1 rispetto a precedente
            blocco.Indice = latestBlock.Indice + 1;

            // calcola il suo hash partendo da quello del precedente
            blocco.HashPrecedente = latestBlock.HashAttuale;

            //instruzione non necessaria quando si introduce il concetto di MINING
            //blocco.HashAttuale = blocco.CalcolaHash();

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
                if (bloccoCorrente.HashAttuale != bloccoCorrente.CalcolaHash())
                {
                    return false;
                }

                //ricalcola l'hash del blocco precedente, se è diverso da quello memorizzato ritorna false (catena non valida)
                if (bloccoCorrente.HashPrecedente != bloccoPrecedente.HashAttuale)
                {
                    return false;
                }
            }

            //se tutti i blocchi sono coerenti tra valore presente e valore aspetta, ritorna true (catena valida)
            return true;

        }
    }
}
