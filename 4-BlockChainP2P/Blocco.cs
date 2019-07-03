using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
//libreria per Ilist e liste
//libreria per la crittografia
//libreria per la formattazione del testo
//libreria gestione JSON


namespace _4_BlockChainP2P
{
    /// <summary>Classe che si occupa di gestire il singolo blocco della catena</summary>
    internal class Blocco
    {

        #region Membri

        /// <summary>
        ///   <para>ID del blocco della catena</para>
        /// </summary>
        public int Indice { get; set; }


        /// <summary>
        ///   <para>
        ///  Gets or sets the data ora di creazione del blocco con precisione fino a ms</para>
        /// </summary>
        /// <value>The data ora.</value>
        public DateTime DataOra { get; set; }


        /// <summary>Chiave di cifratura del blocco precedente. Gets or sets the hash precedente.</summary>
        /// <value>The hash precedente.</value>
        public string HashPrecedente { get; set; }


        /// <summary>
        ///   <para>
        /// chiave di cifratura del blocco precedente. Gets or sets the hash blocco corrente.
        /// </para>
        /// </summary>
        /// <value>The hash blocco corrente.</value>
        public string HashBloccoCorrente { get; set; }

        public IList<Transazione> Transazioni { get; set; }


        /// <summary>
        /// Assicura che i dati scambiati non possano alterati (cfr. Nonce Cryptography <a href="https://it.wikipedia.org/wiki/Nonce">https://it.wikipedia.org/wiki/Nonce</a> . Gets or sets the nonce.
        /// </summary>
        /// <value>The nonce.</value>
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


        #region Documentazione
        /// <summary>Calcola l'hash del blocco basandosi su SHA512</summary>
        /// <returns>Impronta digitale del blocco</returns>
        #endregion
        public string CalcolaHash()
        {
            SHA512 cifraturaSha = SHA512.Create();

            byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{JsonConvert.SerializeObject(Transazioni)}-{Nonce}");
            byte[] byteOutput = cifraturaSha.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }


        #region Documentazione
        /// <summary>
        /// Calcola l'hash per veificare la validità di un blocco
        /// </summary>
        /// <param name="difficoltà">Difficoltà dei calcoli necessari per validare la blockchain.</param>
        #endregion
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

