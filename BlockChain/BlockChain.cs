using System;

//libreria per la gestione delle liste
using System.Collections.Generic;

namespace BlockChain
{
    class BlockChain
    {
        //inizializza una lista concatenata di blocchi
        public IList<Blocco> Chain { set; get; }

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
            Chain = new List<Blocco>();
        }

        public Blocco AggiungiBLocco()
        {
            //aggiungo un blocco passando al costruttore (data, il valore dell'hash precedente, transazione)
            return new Blocco(DateTime.Now, null, "{}");
        }

        public void AggiungiBlocco()
        {
            Chain.Add(AggiungiBLocco());
        }

        public Blocco GetUltimoBlocco()
        {
            return Chain[Chain.Count - 1];
        }

        public void AggiungiBlocco(Blocco blocco)
        {
            //prende i dati inerenti al blocco precedente rispetto a quello da aggiungere
            Blocco latestBlock = GetUltimoBlocco();

            //aumenta l'indice del blocco +1 rispetto a precedente
            blocco.Indice = latestBlock.Indice + 1;

            // calcola il suo hash partendo da quello del precedente
            blocco.HashPrecedente = latestBlock.HashAttuale;
            blocco.HashAttuale = blocco.CalcolaHash();

            //aggiunge il blocco alla catena
            Chain.Add(blocco);
        }

        //verifica l'integrità della blockchain
        public bool IsValido()
        {

            //finché ci sono blocchi
            for (int i = 1; i < Chain.Count; i++)
            {
                Blocco bloccoCorrente = Chain[i];
                Blocco bloccoPrecedente = Chain[i - 1];

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
