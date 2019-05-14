//libreria per la gestione dei json
using Newtonsoft.Json;
using System;

namespace BlockChain
{
    internal class Program
    {
        private static void Main()
        {
            DateTime tempoInizio = DateTime.Now;

            //instanzia un oggetto che rappresenta il mining della moneta
            BlockChain unimolCoin = new BlockChain();

            ////implementazione base con gestione validità
            //        //aggiunge 4 blocchi alla blochchain con un numero di monete variabile
            //        unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Angelo,ricevente:Giusy,ammontare:10}"));
            //        unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Giusy,ricevente:Angelo,ammontare:5}"));
            //        unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Carmen,ricevente:Giusy,ammontare:5}"));
            //        unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Giusy,ricevente:Carmen,ammontare:10}"));

            //        DateTime tempoFine = DateTime.Now;

            //        //deserializza serializza l'oggetto in formato JSON e lo stampa
            //        Console.WriteLine(JsonConvert.SerializeObject(unimolCoin, Formatting.Indented));

            //        //inseririsci i dati relativi alla fine dell'operazione per generare i blocchi
            //        // tempo impiegato: 3 blocchi SHA256 : 00:00:00.1395926
            //        // tempo impiegato: 3 blocchi SHA384 : 00:00:00.0903751
            //        Console.WriteLine($"Durata: {tempoFine - tempoInizio}");

            //        //controlla se la catena è valida sugli hash
            //        //  oracolo: true
            //        Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //        //controlla se la catena è valida sugli hash
            //        //  oracolo: false perché ho aggiornato l'ammontare del blocco 1 senza aggiornare l'hash della blockchain
            //        Console.WriteLine($"\nAggiorno ammontare a 1000");
            //        unimolCoin.Catena[1].DatiTransazione = "{sender:Giusy,receiver:Angelo,amount:1000}";
            //        Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //        //aggiornamento hash del blocco modificato
            //        Console.WriteLine($"\nAggiornamento Hash");
            //        unimolCoin.Catena[1].HashBloccoCorrente = unimolCoin.Catena[1].CalcolaHash();

            //        //  oracolo: true
            //        Console.WriteLine($"\nLa catena è valida: {unimolCoin.IsValido()}");

            //        //aggiornamento degli hash dei restanti blocchi della catena per eliminare problemi di incongruenza
            //        Console.WriteLine($"\nAggiorno intera catena");
            //        unimolCoin.Catena[2].HashPrecedente = unimolCoin.Catena[1].HashBloccoCorrente;
            //        unimolCoin.Catena[2].HashBloccoCorrente = unimolCoin.Catena[2].CalcolaHash();
            //        unimolCoin.Catena[3].HashPrecedente = unimolCoin.Catena[2].HashBloccoCorrente;
            //        unimolCoin.Catena[3].HashBloccoCorrente = unimolCoin.Catena[3].CalcolaHash();
            //        unimolCoin.Catena[4].HashPrecedente = unimolCoin.Catena[3].HashBloccoCorrente;
            //        unimolCoin.Catena[4].HashBloccoCorrente = unimolCoin.Catena[4].CalcolaHash();

            //        //  oracolo: true
            //        Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");


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
