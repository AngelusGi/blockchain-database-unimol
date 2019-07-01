using System;
//libreria per la crittografia
using System.Security.Cryptography;
//libreria per la formattazione del testo
using System.Text;

namespace _1_BlockChainValidazioneBlocchi
{
    internal class Blocco
    {

        #region Membri

        //indice del blocco
        public int Indice { get; set; }

        //data e ora fino a ms
        public DateTime DataOra { get; private set; }

        //chiave di cifratura del blocco precedente
        public string HashPrecedente { get; set; }

        //chiave di cifratura del blocco precedente
        public string HashAttuale { get; set; }

        //dati di transazione (mittente, destinatario, importo)
        public string DatiTransazione { get; set; }

        //assicura che i dati scambiati non possano alterati (cfr. Nonce Cryptography https://it.wikipedia.org/wiki/Nonce
        private int Nonce { get; set; }

        #endregion


        #region Costruttore


        public Blocco(DateTime dataOra, string hashPrecedente, string transazione)
        {
            Indice = 0;
            DataOra = dataOra;
            HashPrecedente = hashPrecedente;
            DatiTransazione = transazione;
            HashAttuale = CalcolaHash();
        }


        #endregion


        public string CalcolaHash()
        {
            SHA256 cifraturaSha256 = SHA256.Create();

            //byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra} - {HashPrecedente ?? ""} - {DatiTransazione}");

            byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{DatiTransazione}-{Nonce}");
            byte[] byteOutput = cifraturaSha256.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }

        public void Mina(int difficoltà)
        {

            string zeroIniziali = new string('0', difficoltà);
            while (HashAttuale == null || HashAttuale.Substring(0, difficoltà) != zeroIniziali)
            {
                Nonce++;
                HashAttuale = CalcolaHash();
            }
        }

    }
}

