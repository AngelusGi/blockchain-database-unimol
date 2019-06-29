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
    /// <summary>
    /// Classe che si occupa di gestire il singolo blocco della catena
    /// </summary>
    internal class Blocco
    {

        #region Membri

        ///ID del blocco
        public int Indice { get; set; }

        ///data e ora di riferimento del blocco con precisione fino a ms
        public DateTime DataOra { get; set; }

        ///chiave di cifratura del blocco precedente
        public string HashPrecedente { get; set; }

        ///chiave di cifratura del blocco precedente
        public string HashBloccoCorrente { get; set; }

        public IList<Transazione> Transazioni { get; set; }

        ///assicura che i dati scambiati non possano alterati (cfr. Nonce Cryptography https://it.wikipedia.org/wiki/Nonce
        private int Nonce { get; set; }

        #endregion


        #region Costruttore

        public Blocco(DateTime dataOra, string hashPrecedente, IList<Transazione> transazioni)
        {
            Indice = 0;
            DataOra = dataOra;
            HashPrecedente = hashPrecedente;
            Transazioni = transazioni;
        }

        #endregion

        ///<summary>
        /// Calcola l'hash del blocco basandosi su SHA512
        /// </summary>
        /// <returns>Impronta digitale del blocco</returns>
        public string CalcolaHash()
        {
            SHA512 cifraturaSha = SHA512.Create();

            byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{JsonConvert.SerializeObject(Transazioni)}-{Nonce}");
            byte[] byteOutput = cifraturaSha.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }

        public void Mina(int difficoltà)
        {

            string zeroIniziali = new string('0', difficoltà);
            while (HashBloccoCorrente == null || HashBloccoCorrente.Substring(0, difficoltà) != zeroIniziali)
            {
                Nonce++;
                HashBloccoCorrente = CalcolaHash();
            }
        }

    }
}

