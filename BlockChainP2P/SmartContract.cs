using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BlockChainMenu
{

    /// <summary>Questa classe implementa e gestisce lo SmartContract proveniente da un JSON</summary>
    internal static class SmartContract
    {

        #region DefinizioneProprietàOggettoJson

        private class ContrattoJson
        {
            public string schema { get; set; }
            public string title { get; set; }
            public string type { get; set; }
            public Properties properties { get; set; }
        }

        public class Properties
        {
            public Validationdata validationData { get; set; }
            public Clause clause { get; set; }
            public Versions versions { get; set; }
        }

        public class Validationdata
        {
            public string type { get; set; }
            public string description { get; set; }
            public string datetime { get; set; }
        }

        public class Clause
        {
            public string type { get; set; }
            public string description { get; set; }
            public string IdContract { get; set; }
            public string TransactionContract { get; set; }
            public string DoubleSpendingContract { get; set; }
            public string BalanceCheck { get; set; }
        }

        public class Versions
        {
            public string type { get; set; }
            public string description { get; set; }
            public string numVersion { get; set; }
            public Items items { get; set; }
        }

        public class Items
        {
            public string type { get; set; }
        }

        #endregion

        private static readonly BlockChain UniMolCoin = Menu.UniMolCoin;

        private static ContrattoJson _contratto;


        /// <summary>
        /// Inizializza l'oggetto SmartContract a partire dal file JSON locale
        /// </summary>
        public static void Inizializza()
        {
            //verifica se sono su windows o meno e in base al sistema operativo fornisce il path corretto
            var jsonPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "..\\..\\..\\Resources\\SmartContract.json" : "./Resources/SmartContract.json";

            StreamReader lettoreFileJson = new StreamReader(jsonPath);
            _contratto = JsonConvert.DeserializeObject<ContrattoJson>(lettoreFileJson.ReadToEnd());

        }

        /// <summary>
        /// Mostra i dati presenti all'interno del contratto: titolo, versione, data di ultima modifica e clausole
        /// </summary>
        public static void MostraContratto()
        {
            Console.WriteLine(_contratto.title);
            Console.WriteLine($"Versione: {_contratto.properties.versions.numVersion}");
            Console.WriteLine($"Ultima modifica: {_contratto.properties.validationData.datetime}");
            Console.WriteLine($"Clausole:" +
                              $"\n\t{_contratto.properties.clause.IdContract}" +
                              $"\n\t{_contratto.properties.clause.DoubleSpendingContract}" +
                              $"\n\t{_contratto.properties.clause.TransactionContract}" +
                              $"\n\t{_contratto.properties.clause.BalanceCheck}");
        }

        /// <summary>
        /// Valida la transazione verificando se il saldo dell'utenti è necessario a coprire l'importo che si vole spendere.
        /// </summary>
        /// <param name="nomeMittente">Nome di colui che vuole fare la transazione.</param>
        /// <param name="importoTransazione">Importo della transazione.</param>
        /// <returns>Il saldo è sufficiente (true/false)</returns>
        public static bool ValidaTransazione(string nomeMittente, int importoTransazione)
        {
            return UniMolCoin.RicercaUtente(nomeMittente).Saldo >= importoTransazione;
        }

        /// <summary>
        /// Verifica lo stato di validità dei blocchi presenti all'interno della blockchain.
        /// </summary>
        /// <returns>La blockchain è valida (true/false)</returns>
        public static bool ValidaBlockchain()
        {
            //finché ci sono blocchi
            for (int pos = 1; pos < UniMolCoin.Catena.Count; pos++)
            {
                Blocco bloccoCorrente = UniMolCoin.Catena[pos];
                Blocco bloccoPrecedente = UniMolCoin.Catena[pos - 1];

                //ricalcola l'hash del blocco analizzato, se è diverso da quello memorizzato ritorna false (catena non valida)
                if (bloccoCorrente.HashBloccoCorrente != bloccoCorrente.CalcolaHash())
                {
                    return false;
                }

                //ricalcola l'hash del blocco precedente, se è diverso da quello memorizzato ritorna false (catena non valida)
                if (bloccoCorrente.HashPrecedente != bloccoPrecedente.HashBloccoCorrente)
                {
                    return false;
                }
            }

            //se tutti i blocchi sono coerenti tra valore presente e valore aspetta, ritorna true (catena valida)
            return true;

        }

        /// <summary>
        /// Assegna un ID univoco all'utenti
        /// </summary>
        /// <param name="utenti">Utente da autenticare.</param>
        /// <returns>ID univoco dell'utenti (SHA)</returns>
        public static void AutenticaUtente(IList<Utente> utenti)
        {

            foreach (var utente in utenti)
            {
                utente.IdUnivoco = utente.GetHashCode().ToString();
            }

        }

    }
}
