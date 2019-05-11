using System;

//libreria per la gestione dei json
using Newtonsoft.Json;

namespace BlockChain
{
    class Program
    {
        static void Main(string[] args)
        {
            //instanzia un oggetto che rappresenta il mining della moneta
            BlockChain unimolCoin = new BlockChain();


            //aggiunge 4 blocchi alla blochchain con un numero di monete variabile
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Angelo,ricevente:Giusy,ammontare:10}"));
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Giusy,ricevente:Angelo,ammontare:5}"));
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.Now, null, "{mittente:Carmen,ricevente:Giusy,ammontare:5}"));
            unimolCoin.AggiungiBlocco(new Blocco(DateTime.UtcNow, null, "{mittente:Giusy,ricevente:Carmen,ammontare:10}"));

            //deserializza serializza l'oggetto in formato JSON e lo stampa
            Console.WriteLine(JsonConvert.SerializeObject(unimolCoin, Formatting.Indented));

            //controlla se la catena è valida sugli hash
            //  oracolo: true
            Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //controlla se la catena è valida sugli hash
            //  oracolo: false perché ho aggiornato l'ammontare del blocco 1 senza aggiornare l'hash della blockchain
            Console.WriteLine($"\n\nAggiorno ammontare a 1000");
            unimolCoin.Chain[1].DatiTransazione = "{sender:Giusy,receiver:Angelo,amount:1000}";
            Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            Console.WriteLine($"\n\nAggiornamento Hash");
            unimolCoin.Chain[1].HashAttuale = unimolCoin.Chain[1].CalcolaHash();

            //  oracolo: true
            Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //aggiornamento degli hash dei restanti blocchi della catena per eliminare problemi di incongruenza
            Console.WriteLine($"Aggiorno intera catena");
            unimolCoin.Chain[2].HashPrecedente = unimolCoin.Chain[1].HashAttuale;
            unimolCoin.Chain[2].HashAttuale = unimolCoin.Chain[2].CalcolaHash();
            unimolCoin.Chain[3].HashPrecedente = unimolCoin.Chain[2].HashAttuale;
            unimolCoin.Chain[3].HashAttuale = unimolCoin.Chain[3].CalcolaHash();
            unimolCoin.Chain[4].HashPrecedente = unimolCoin.Chain[3].HashAttuale;
            unimolCoin.Chain[4].HashAttuale = unimolCoin.Chain[4].CalcolaHash();

            //  oracolo: true
            Console.WriteLine($"La catena è valida: {unimolCoin.IsValido()}");

            //aspetta la pressione di un tasto per la terminazione del programma
            Console.WriteLine("Esecuzione terminata.\nPremere un tasto per uscire...");
        }
    }
}
