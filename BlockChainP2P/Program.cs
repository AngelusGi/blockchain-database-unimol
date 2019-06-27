using System;
//libreria per la gestione dei json
using Newtonsoft.Json;

namespace BlockChain
{
    class Program
    {

        public static int Porta = 0;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();
        public static BlockChain UniMolCoin = new BlockChain();
        public static string Nome = null;

        private const int ESCI = 4;
        private const int URL_SERVER = 1;
        private const int TRANSAZIONE = 2;
        private const int BLOCKCHAIN = 3;
        private const int ANNULLA = 0;


        private static void Main(String[] args)
        {

            UniMolCoin.InizializzaCatena();

            if (args.Length >= 1)
                Porta = int.Parse(args[0]);
            if (args.Length >= 2)
                Nome = args[1];

            if (Porta > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }
            if (Nome != null)
            {
                Console.WriteLine($"Utente corrente: {Nome}");
            }

            int Selezione = ANNULLA;
            while (Selezione != ESCI)
            {
                switch (Selezione)
                {
                    case URL_SERVER:
                        Console.WriteLine("Per favore, inserisci l'URL del server (0 per annullare)");
                        string serverURL = Console.ReadLine();
                        if (serverURL == "0")
                        {
                            break;
                        }
                            
                        Client.Connetti($"{serverURL}/Blockchain");
                        break;

                    case TRANSAZIONE:
                        Console.WriteLine("Per favore, inserisci il nome del destinatario (0 per annullare)");
                        string ricevente = Console.ReadLine();
                        if (ricevente == "0")
                        {
                            break;
                        }
                            
                        Console.WriteLine("Per favore, inserisci l'importo (0 per annullare)");
                        string importo = Console.ReadLine();
                        if (importo == "0")
                        {
                            break;
                        }
                            
                        UniMolCoin.CreaTransazione(new Transazione(Nome, ricevente, int.Parse(importo)));
                        UniMolCoin.MinaTransazioni(Nome);
                        Client.Broadcast(JsonConvert.SerializeObject(UniMolCoin));


                        break;
                    case BLOCKCHAIN:
                        Console.WriteLine("Blockchain");
                        Console.WriteLine(JsonConvert.SerializeObject(UniMolCoin, Formatting.Indented));
                        break;

                }

                Console.WriteLine("=========================");
                Console.WriteLine("1. Connessione al server.");
                Console.WriteLine("2. Aggiungere una transazione.");
                Console.WriteLine("3. Mostra la BlockChain");
                Console.WriteLine("4. Esci");
                Console.WriteLine("=========================");
                Console.WriteLine("Inserisci una scelta:");
                string Azione = Console.ReadLine();
                Selezione = int.Parse(Azione);
            }

            if (Client != null)
            {
                try
                {
                    Client.Close();
                }
                finally
                {
                    Client = null;
                }
            }



            //DateTime tempoInizio = DateTime.Now;

            ////instanzia un oggetto che rappresenta il mining della moneta
            //BlockChain unimolCoin = new BlockChain();


            ////implementazione con gestione transazioni
            //unimolCoin.CreaTransazione(new Transazione("Angelo", "Giusy", 10));
            //unimolCoin.GesticiTransazioniInAttesa("Carmen");

            //unimolCoin.CreaTransazione(new Transazione("Giusy", "Angelo", 5));
            //unimolCoin.CreaTransazione(new Transazione("Giusy", "Angelo", 5));
            //unimolCoin.GesticiTransazioniInAttesa("Carmen");


            //DateTime tempoFine = DateTime.Now;

            //Console.WriteLine($"Durata: {tempoFine - tempoInizio}");

            //Console.WriteLine("----------------------------");

            //Console.WriteLine("\nRiepilogo bilanci:");
            //Console.WriteLine($"\tBilancio Angelo: { unimolCoin.GetBilancio("Angelo")}");
            //Console.WriteLine($"\tBilancio Giusy: { unimolCoin.GetBilancio("Giusy")}");
            //Console.WriteLine($"\tBilancio Carmen: { unimolCoin.GetBilancio("Carmen")}");

            //Console.WriteLine("----------------------------");

            //Console.WriteLine("\nUniMolCoin:");
            //Console.WriteLine($"unimolCoin");
            //Console.WriteLine(JsonConvert.SerializeObject(unimolCoin, Formatting.Indented));

            ////aspetta la pressione di un tasto per la terminazione del programma
            //Console.WriteLine("\nEsecuzione terminata.\nPremere un tasto per uscire...");

        }
    }
}
