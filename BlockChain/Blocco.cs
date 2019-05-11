using System;
using System.Text;

//libreria per la crittografia
using System.Security.Cryptography;

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
        public string HashAttuale { get; set; }

        //dati di transazione (mittente, destinatario, importo)
        public string DatiTransazione { get; set; }

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

            SHA384 cifraturaSHA384 = SHA384.Create();

            byte[] byteInput = Encoding.ASCII.GetBytes($"{DataOra} - {HashPrecedente ?? ""} - {DatiTransazione}");
            byte[] byteOutput = cifraturaSHA384.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);
        }
    }
}
