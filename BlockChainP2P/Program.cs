using System;
//libreria per la gestione dei json
using Newtonsoft.Json;

namespace BlockChain
{
    class Program
    {
        private static void Main()
        {
            DateTime tempoInizio = DateTime.Now;

            //instanzia un oggetto che rappresenta il mining della moneta
            BlockChain unimolCoin = new BlockChain();


            //implementazione con gestione transazioni
            unimolCoin.CreaTransazione(new Transazione("Angelo", "Giusy", 10));
            unimolCoin.GesticiTransazioniInAttesa("Carmen");

            unimolCoin.CreaTransazione(new Transazione("Giusy", "Angelo", 5));
            unimolCoin.CreaTransazione(new Transazione("Giusy", "Angelo", 5));
            unimolCoin.GesticiTransazioniInAttesa("Carmen");


            DateTime tempoFine = DateTime.Now;

            Console.WriteLine($"Durata: {tempoFine - tempoInizio}");

            Console.WriteLine("----------------------------");

            Console.WriteLine("\nRiepilogo bilanci:");
            Console.WriteLine($"\tBilancio Angelo: { unimolCoin.GetBilancio("Angelo")}");
            Console.WriteLine($"\tBilancio Giusy: { unimolCoin.GetBilancio("Giusy")}");
            Console.WriteLine($"\tBilancio Carmen: { unimolCoin.GetBilancio("Carmen")}");

            Console.WriteLine("----------------------------");

            Console.WriteLine("\nUniMolCoin:");
            Console.WriteLine($"unimolCoin");
            Console.WriteLine(JsonConvert.SerializeObject(unimolCoin, Formatting.Indented));

            //aspetta la pressione di un tasto per la terminazione del programma
            Console.WriteLine("\nEsecuzione terminata.\nPremere un tasto per uscire...");

        }
    }
}
