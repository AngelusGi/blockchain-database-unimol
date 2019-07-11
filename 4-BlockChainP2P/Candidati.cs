using System;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace _4_BlockChainP2P
{
    internal class Candidati
    {
        private static CandidatiJson _candidati;

        private static void Inizializza()
        {
            //verifica se sono su windows o meno e in base al sistema operativo fornisce il path corretto
            string jsonPath;
            StreamReader lettoreFileJson = null;

            try
            {
                jsonPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "../../../Resources/Candidati.json"
                    : "./4-BlockChainP2P/Resources/Candidati.json";
                lettoreFileJson = new StreamReader(jsonPath);
            }
            catch (Exception)
            {
                jsonPath = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? "../../../Resources/CandidatiTest.json"
                    : "./4-BlockChainP2P/Resources/CandidatiTest.json";
                lettoreFileJson = new StreamReader(jsonPath);
            }
            finally
            {
                _candidati = JsonConvert.DeserializeObject<CandidatiJson>(lettoreFileJson.ReadToEnd());
                lettoreFileJson.Dispose();
            }



        }


        #region Documentazione

        /// <summary>
        /// Mostra i dati candidati all'esame (nome, cognome e matricola) e nome e cognome del docente di riferimento.
        /// Inoltre richiama la funzione per il parse dei JSON
        /// </summary>

        #endregion

        public static void MostraCanidati()
        {
            Inizializza();

            Console.WriteLine("\t*** PROGETTO BASI DI DATI E SISTEMI INFORMATIVI: BLOCKCHAIN 'UNIMOL COIN' ***");
            Console.WriteLine($"*** PROF. {_candidati.Properties.Professor.Name.ToUpperInvariant()} ***");
            Console.WriteLine("*** CDL IN INFORMATICA - UNIVERSITÀ DEGLI STUDI DEL MOLISE ***");
            Console.WriteLine("*** CANDIDATI:" +
                              $"\n\t{_candidati.Properties.Agv.Name.ToUpperInvariant()} - {_candidati.Properties.Agv.Matricola.ToUpperInvariant()}" +
                              $"\n\t{_candidati.Properties.Gm.Name.ToUpperInvariant()} - {_candidati.Properties.Gm.Matricola.ToUpperInvariant()}" +
                              $"\n\t{_candidati.Properties.Ca.Name.ToUpperInvariant()} - {_candidati.Properties.Ca.Matricola.ToUpperInvariant()}");
        }
    }

    internal class CandidatiJson
    {
        public Properties Properties { get; set; }
    }

    internal class Properties
    {
        public Professor Professor { get; set; }
        public Agv Agv { get; set; }
        public Ca Ca { get; set; }
        public Gm Gm { get; set; }
    }

    internal class Professor
    {
        public string Name { get; set; }
    }

    internal class Agv
    {
        public string Name { get; set; }
        public string Matricola { get; set; }
    }

    internal class Ca
    {
        public string Name { get; set; }
        public string Matricola { get; set; }
    }

    internal class Gm
    {
        public string Name { get; set; }
        public string Matricola { get; set; }
    }

}

