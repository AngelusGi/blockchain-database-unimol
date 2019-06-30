﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace BlockChainMenu
{
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

        private static readonly BlockChain _uniMolCoin = Menu.UniMolCoin;

        private static ContrattoJson _contratto;

        private static readonly string _jsonPath = "..\\..\\..\\Resources\\SmartContract.json";

        public static void Inizializza()
        {

            using StreamReader lettoreFileJson = new StreamReader(_jsonPath);
            _contratto = JsonConvert.DeserializeObject<ContrattoJson>(lettoreFileJson.ReadToEnd());

        }

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

        public static void ValidaTransazione()
        {

        }

        public static string AutenticaUtente(Utente utente)
        {

            SHA512 idSha = SHA512.Create();

            byte[] byteInput = Encoding.ASCII.GetBytes($"{utente}");
            byte[] byteOutput = idSha.ComputeHash(byteInput);

            return Convert.ToBase64String(byteOutput);

        }

    }
}
