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

            //aggiunge 4 blocchi alla blochchain con un numero di monete variabile
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Angelo,ricevente:Giusy,ammontare:10}"));
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Giusy,ricevente:Angelo,ammontare:5}"));
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Carmen,ricevente:Giusy,ammontare:5}"));
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Giusy,ricevente:Carmen,ammontare:10}"));

            DateTime tempoFine = DateTime.Now;

            //deserializza serializza l'oggetto in formato JSON e lo stampa
            Console.WriteLine(JsonConvert.SerializeObject(unimolCoin, Formatting.Indented));

            //inseririsci i dati relativi alla fine dell'operazione per generare i blocchi
            // tempo impiegato: 3 blocchi SHA256 : 00:00:00.1395926
            // tempo impiegato: 3 blocchi SHA384 : 00:00:00.0903751
            Console.WriteLine($"Durata: {tempoFine - tempoInizio}");

            //controlla se la catena è valida sugli hash
            //  oracolo: true
            Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //controlla se la catena è valida sugli hash
            //  oracolo: false perché ho aggiornato l'ammontare del blocco 1 senza aggiornare l'hash della blockchain
            Console.WriteLine($"\nAggiorno ammontare a 1000");
            unimolCoin.Catena[1].DatiTransazione = "{sender:Giusy,receiver:Angelo,amount:1000}";
            Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //aggiornamento hash del blocco modificato
            Console.WriteLine($"\nAggiornamento Hash");
            unimolCoin.Catena[1].HashAttuale = unimolCoin.Catena[1].CalcolaHash();

            //  oracolo: true
            Console.WriteLine($"\nLa catena è valida: {unimolCoin.IsValido()}");

            //aggiornamento degli hash dei restanti blocchi della catena per eliminare problemi di incongruenza
            Console.WriteLine($"\nAggiorno intera catena");
            unimolCoin.Catena[2].HashPrecedente = unimolCoin.Catena[1].HashAttuale;
            unimolCoin.Catena[2].HashAttuale = unimolCoin.Catena[2].CalcolaHash();
            unimolCoin.Catena[3].HashPrecedente = unimolCoin.Catena[2].HashAttuale;
            unimolCoin.Catena[3].HashAttuale = unimolCoin.Catena[3].CalcolaHash();
            unimolCoin.Catena[4].HashPrecedente = unimolCoin.Catena[3].HashAttuale;
            unimolCoin.Catena[4].HashAttuale = unimolCoin.Catena[4].CalcolaHash();

            //  oracolo: true
            Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //aspetta la pressione di un tasto per la terminazione del programma
            Console.WriteLine("\nEsecuzione terminata.\nPremere un tasto per uscire...");

        }
    }
}
