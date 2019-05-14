using System;
//libreria per la crittografia
using System.Security.Cryptography;
//libreria per Ilist
using System.Collections.Generic;
//libreria gestione JSON
using Newtonsoft.Json;
using System.Text;


namespace BlockChain
{
    internal class Blocco
    {
        //indice del blocco
        public int Indice { get; set; }

        //data e ora fino a ms
        public DateTime DataOra { get; set; }

        //chiave di cifratura del blocco precedente
        public string HashPrecedente { get; set; }

        //chiave di cifratura del blocco precedente
        public string HashBloccoCorrente { get; set; }

        //dati di transazione (mittente, destinatario, importo)
        //una volta implementata la classe per gestire le transazioni non c'è più bisogno di questo parametro
        //public string DatiTransazione { get; set; }
        public IList<Transazione> Transazioni { get; set; }

        //assicura che i dati scambiati non possano alterati (cfr. Nonce Cryptography https://it.wikipedia.org/wiki/Nonce
        private int Nonce { get; set; } = 0;

        //non più utile perché le transazioni vengono gestite come lista e non più come stringa
        //public Blocco(DateTime dataOra, string hashPrecedente, string transazione)
        public Blocco(DateTime dataOra, string hashPrecedente, IList<Transazione> transazioni)
        {
            Indice = 0;
            DataOra = dataOra;
            HashPrecedente = hashPrecedente;
            //non più utile perché gestito dalla lista Transazioni
            //DatiTransazione = transazione;
            //HashBloccoCorrente = CalcolaHash();
            Transazioni = transazioni;
        }

        public string CalcolaHash()
        {
            SHA256 cifraturaSHA256 = SHA256.Create();

            //byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra} - {HashPrecedente ?? ""} - {DatiTransazione}");

            //TODO: DEBUG NON STAMPA "NONCE"
            //byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{DatiTransazione}-{Nonce}");
            byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{JsonConvert.SerializeObject(Transazioni)}-{Nonce}");
            byte[] byteOutput = cifraturaSHA256.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }

        public void Mina(int difficolta)
        {

            string zeroIniziali = new string('0', difficolta);
            while (this.HashBloccoCorrente == null || this.HashBloccoCorrente.Substring(0, difficolta) != zeroIniziali)
            {
                this.Nonce++;
                this.HashBloccoCorrente = CalcolaHash();
            }
        }

    }
}

