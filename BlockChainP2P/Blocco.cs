using System;
//libreria per Ilist
using System.Collections.Generic;
//libreria per la crittografia
using System.Security.Cryptography;
//libreria per la formattazione del testo
using System.Text;
//libreria gestione JSON
using Newtonsoft.Json;


namespace BlockChain
{
    class Blocco
    {
        //indice del blocco
        public int Indice { get; set; }

        //data e ora fino a ms
        public DateTime DataOra { get; set; }

        //chiave di cifratura del blocco precedente
        public string HashPrecedente { get; set; }

        //chiave di cifratura del blocco precedente
        public string HashBloccoCorrente { get; set; }
        
        public IList<Transazione> Transazioni { get; set; }

        //assicura che i dati scambiati non possano alterati (cfr. Nonce Cryptography https://it.wikipedia.org/wiki/Nonce
        private int Nonce { get; set; } = 0;

        public Blocco(DateTime dataOra, string hashPrecedente, IList<Transazione> transazioni)
        {
            Indice = 0;
            DataOra = dataOra;
            HashPrecedente = hashPrecedente;
            Transazioni = transazioni;
        }

        public string CalcolaHash()
        {
            SHA256 cifraturaSHA256 = SHA256.Create();

            byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{JsonConvert.SerializeObject(Transazioni)}-{Nonce}");
            byte[] byteOutput = cifraturaSHA256.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }

        public void Mina(int difficolta)
        {

            string zeroIniziali = new string('0', difficolta);
            while (HashBloccoCorrente == null || HashBloccoCorrente.Substring(0, difficolta) != zeroIniziali)
            {
                Nonce++;
                HashBloccoCorrente = CalcolaHash();
            }
        }

    }
}

