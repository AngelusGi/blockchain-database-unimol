using System;
//libreria per Ilist
using System.Collections.Generic;
//libreria per la crittografia
using System.Security.Cryptography;
//libreria per la formattazione del testo
using System.Text;
//libreria gestione JSON
using Newtonsoft.Json;


namespace _2_BlockChainPoW
{
    internal class Blocco
    {

        #region Membri

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
        private int Nonce { get; set; }

        #endregion


        #region Costruttore

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


        #endregion


        public string CalcolaHash()
        {
            //SHA256 cifraturaSha = SHA256.Create();
            var cifraturaSha = SHA512.Create();


            //byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra} - {HashPrecedente ?? ""} - {DatiTransazione}");
            //byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{DatiTransazione}-{Nonce}");


            var byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{JsonConvert.SerializeObject(Transazioni)}-{Nonce}");
            var byteOutput = cifraturaSha.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }

        public void Mina(int difficoltà)
        {

            var zeroIniziali = new string('0', difficoltà);
            while (HashBloccoCorrente == null || HashBloccoCorrente.Substring(0, difficoltà) != zeroIniziali)
            {
                Nonce++;
                HashBloccoCorrente = CalcolaHash();
            }
        }

    }
}

