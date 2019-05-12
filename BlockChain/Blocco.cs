using System;
//libreria per la crittografia
using System.Security.Cryptography;
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
        public string HashAttuale { get; set; }

        //dati di transazione (mittente, destinatario, importo)
        public string DatiTransazione { get; set; }

        //assicura che i dati scambiati non possano alterati (cfr. Nonce Cryptography https://it.wikipedia.org/wiki/Nonce
        private int Nonce { get; set; } = 0;

        public Blocco(DateTime dataOra, string hashPrecedente, string transazione)
        {
            Indice = 0;
            DataOra = dataOra;
            HashPrecedente = hashPrecedente;
            DatiTransazione = transazione;
            HashAttuale = CalcolaHash();
        }

        public string CalcolaHash()
        {
            SHA256 cifraturaSHA256 = SHA256.Create();

            //byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra} - {HashPrecedente ?? ""} - {DatiTransazione}");

            //TODO: DEBUG NON STAMPA "NONCE"
            byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra}-{HashPrecedente ?? ""}-{DatiTransazione}-{Nonce}");
            byte[] byteOutput = cifraturaSHA256.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }

        public void Mina(int difficolta)
        {

            string zeroIniziali = new string('0', difficolta);
            while (this.HashAttuale == null || this.HashAttuale.Substring(0, difficolta) != zeroIniziali)
            {
                this.Nonce++;
                this.HashAttuale = CalcolaHash();
            }
        }

    }
}

