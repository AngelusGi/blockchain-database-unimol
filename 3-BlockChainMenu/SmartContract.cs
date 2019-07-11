using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace _3_BlockChainMenu
{

    /// <summary>Questa classe implementa e gestisce lo SmartContract proveniente da un JSON</summary>
    internal static class SmartContract
    {


        #region Membi

        #region DefinizioneProprietàOggettoJson


        public class ContrattoJson
        {
            public string Schema { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public Properties Properties { get; set; }
        }

        public class Properties
        {
            public Validationdata ValidationData { get; set; }
            public Clause Clause { get; set; }
            public Revision Revision { get; set; }
        }

        public class Validationdata
        {
            public string Type { get; set; }
            public string Description { get; set; }
            public string Datetime { get; set; }
        }

        public class Clause
        {
            public string Type { get; set; }
            public string Description { get; set; }
            public string IdContract { get; set; }
            public string TransactionContract { get; set; }
            public string DoubleSpendingContract { get; set; }
            public string BalanceCheck { get; set; }
        }

        public class Revision
        {
            public string Type { get; set; }
            public string Description { get; set; }
            public string NumVersion { get; set; }
        }


        #endregion

        private static readonly BlockChain UniMolCoin = Menu.UniMolCoin;

        private static ContrattoJson _contratto;

        #endregion


        #region Documentazione

        /// <summary>
        /// Inizializza l'oggetto SmartContract a partire dal file JSON locale in base alla piattaforma d'esecuzione
        /// </summary>

        #endregion
        public static void Inizializza()
        {
            // //verifica se sono su windows o meno e in base al sistema operativo fornisce il path corretto
            // var jsonPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "../../../Resources/SmartContract.json" : "./3 - BlockChainMenu/Resources/SmartContract.json";
            
            // using var lettoreFileJson = new StreamReader(jsonPath);

            // _contratto = JsonConvert.DeserializeObject<ContrattoJson>(lettoreFileJson.ReadToEnd());
            
            string jsonPath;
            StreamReader lettoreFileJson = null;

            try
            {
                //verifica se sono su windows o meno e in base al sistema operativo fornisce il path corretto
                jsonPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "../../../Resources/SmartContract.json" : "./4-BlockChainP2P/Resources/SmartContract.json";
                lettoreFileJson = new StreamReader(jsonPath);
            }
            catch (Exception)
            {
                jsonPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "../../../Resources/SmartContract.json" : "./4-BlockChainP2P/Resources/SmartContract.json";
                lettoreFileJson = new StreamReader(jsonPath);
            }
            finally
            {
                _contratto = JsonConvert.DeserializeObject<ContrattoJson>(lettoreFileJson.ReadToEnd());
                lettoreFileJson.Dispose();
            }

        }


        #region Documentazione
        /// <summary>
        /// Mostra i dati presenti all'interno del contratto: titolo, versione, data di ultima modifica e clausole
        /// </summary>
        #endregion
        public static void MostraContratto()
        {
            Console.WriteLine($"\t{_contratto.Title.ToUpperInvariant()}");
            Console.WriteLine($"\t{_contratto.Properties.Revision.Description}: {_contratto.Properties.Revision.NumVersion}");
            Console.WriteLine($"\t{_contratto.Properties.ValidationData.Description}: {_contratto.Properties.ValidationData.Datetime}");
            Console.WriteLine($"\t{_contratto.Properties.Clause.Description}:" +
                              $"\n\t\t{_contratto.Properties.Clause.IdContract}" +
                              $"\n\t\t{_contratto.Properties.Clause.DoubleSpendingContract}" +
                              $"\n\t\t{_contratto.Properties.Clause.TransactionContract}" +
                              $"\n\t\t{_contratto.Properties.Clause.BalanceCheck}");
        }


        #region Documentazione
        /// <summary>Valida la transazione verificando se il saldo dell'utente è necessario a coprire l'importo che si vole spendere.</summary>
        /// <param name="nomeMittente">Nome di colui che vuole fare la transazione.</param>
        /// <param name="nomeDestinatario">Nome del beneficiario della transazione</param>
        /// <param name="importoTransazione">Importo della transazione.</param>
        /// <returns>Il saldo è sufficiente (true/false)</returns>
        #endregion
        public static bool VerificaSaldo(string nomeMittente, string nomeDestinatario, int importoTransazione)
        {

            var mittente = UniMolCoin.RicercaUtente(nomeMittente);

            if (mittente.Saldo.Count >= importoTransazione)
            {
                var destinatario = UniMolCoin.RicercaUtente(nomeDestinatario);

                //TrasferisciMoneta(importoTransazione, mittente, destinatario);
                return !VerificaMonete(mittente, destinatario);
            }

            return false;

        }


        #region Documentazione
        /// <summary>
        /// Verifica lo stato di validità dei blocchi presenti all'interno della blockchain.
        /// </summary>
        /// <returns>La blockchain è valida (true/false)</returns>
        #endregion
        public static bool ValidaBlockchain()
        {
            //finché ci sono blocchi
            for (var pos = 1; pos < UniMolCoin.Catena.Count; pos++)
            {
                var bloccoCorrente = UniMolCoin.Catena[pos];
                var bloccoPrecedente = UniMolCoin.Catena[pos - 1];

                #region VecchiaImplementazione

                //ricalcola l'hash del blocco analizzato, se è diverso da quello memorizzato ritorna false (catena non valida)

                //if (!bloccoCorrente.Equals(bloccoCorrente.CalcolaHash()))
                //{
                //    return false;
                //}
                //if (bloccoCorrente.HashBloccoCorrente != bloccoCorrente.CalcolaHash())
                //{
                //    return false;
                //}

                //ricalcola l'hash del blocco precedente, se è diverso da quello memorizzato ritorna false (catena non valida)
                //if (bloccoCorrente.HashPrecedente != bloccoPrecedente.CalcolaHash())
                //{
                //    return false;
                //}

                #endregion

                if (bloccoPrecedente.HashBloccoCorrente != bloccoCorrente.HashPrecedente)
                {
                    return false;
                }
            }

            //se tutti i blocchi sono coerenti tra valore presente e valore aspetta, ritorna true (catena valida)
            return true;

        }


        #region Documentazione
        /// <summary>
        /// Verifica se l'utente che si vuole inserire esiste già all'interno degli utenti inseriti nella blockain
        /// </summary>
        /// <param name="utente"></param>
        /// <returns>UtenteNonPresente (true/false)</returns>
        #endregion
        public static bool VerificaOmonimie(Utente utente)
        {

            #region Spiegazione codice
            //foreach (Utente utenteCorrente in UniMolCoin.Utenti)
            //{
            //    //se l'utente da inserire non è già presente o per hash o per nome, allora posso autenticarlo e ritorno true
            //    if (utenteCorrente.Nome == utente.Nome)
            //    {
            //        utenteNonPresente = false;
            //        break;
            //    }
            //}
            //questa porzione di codice, equivale alla riga sottostante
            #endregion


            var utenteNonPresente = UniMolCoin.Utenti.All(utenteCorrente => utenteCorrente.Nome != utente.Nome);


            if (utenteNonPresente)
            {
                //nel caso in cui non esista già l'utente (nome o hash associato) allora lo autentico
                AutenticaUtente(utente);
            }

            return utenteNonPresente;
        }


        #region Documentazione
        /// <summary>
        /// Assegna un ID univoco agli utenti della lista (hash code)
        /// </summary>
        /// <param name="utente">Singolo utente da autenticare.</param>
        #endregion
        private static void AutenticaUtente(Utente utente)
        {
            utente.IdUnivoco = utente.GetHashCode();
        }


        #region Documentazione
        /// <summary>
        /// Verificano che non vi siano nè nel portafogli del mittente, nè nel destinario monete con lo stesso ID,
        /// al fine di evitare il double spending.
        /// </summary>
        /// <param name="mittente">Committente della transazione</param>
        /// <param name="ricevente">Beneficiario della transazione</param>
        /// <returns> False se non è presente un caso di doubleSpending, in caso contrario true </returns>
        #endregion
        private static bool VerificaMonete(Utente mittente, Utente ricevente)
        {
            var doppiaSpesa = false;
            foreach (var monetaMittente in mittente.Saldo)
            {
                foreach (var monetaRicevuta in ricevente.Saldo)
                {
                    if (monetaMittente.IdMoneta == monetaRicevuta.IdMoneta)
                    {
                        doppiaSpesa = true;
                        break;
                    }

                }
            }

            return doppiaSpesa;
        }


        public static void RicompensaMiner(Utente miner)
        {
            try
            {
                if (miner.IdUnivoco != null)
                {
                    miner.Saldo.Push(new Moneta((int)miner.IdUnivoco));
                }
            }
            catch (InvalidOperationException eccezione)
            {
                Console.WriteLine(eccezione);
                throw;
            }
        }


        #region Documentazione

        /// <summary>
        /// Trasferisce l'importo che si vuole spendere dal mittente al beneficiario.
        /// </summary>
        /// <param name="numMonete">Numero di monete che si vogliono spendere</param>
        /// <param name="mittente">Committente della transazione</param>
        /// <param name="ricevente">Beneficiario della transazione</param>

        #endregion
        public static void TrasferisciMoneta(int numMonete, Utente mittente, Utente ricevente)
        {

            for (var i = 0; i < numMonete; i++)
            {
                ricevente.Saldo.Push(mittente.Saldo.Pop());
            }

        }

    }
}
