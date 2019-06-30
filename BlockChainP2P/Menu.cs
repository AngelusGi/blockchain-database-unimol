using System;
using Newtonsoft.Json;

namespace BlockChainMenu
{

    internal class Menu
    {

        public static BlockChain UniMolCoin { get; private set; }

        private const int Esci = 4;
        private const int AggiungiTransazione = 1;
        private const int MostraBlockchain = 2;
        private const int MostraSmartContract = 3;
        private const int Annulla = 0;

        private string _risposta;
        private int _numUtentiTest;


        public Menu()
        {
            UniMolCoin = new BlockChain();
            _risposta = null;
            _numUtentiTest = 0;

            AvviaMenu();
        }

        private void NormalizzaNome(ref string nome)
        {
            nome = nome.Trim();
            nome = nome.ToLowerInvariant();
        }

        private void Benvenuto()
        {
            //todo inserire nomeProf + nome cognome e matricole nostre
            string[] datiUtenti = new string[4];

            for (int i = 0; i < datiUtenti.Length; i++)
            {
                if (!string.IsNullOrEmpty(datiUtenti[i]))
                {
                    datiUtenti[i] = datiUtenti[i].ToUpperInvariant();
                }
            }


            Console.WriteLine("*** PROGETTO BASI DI DATI E SISTEMI INFORMATIVI: BLOCKCHAIN 'UNIMOL COIN' ***");
            Console.WriteLine($"*** PROF. {datiUtenti[0]}***");
            Console.WriteLine("*** CDL IN INFORMATICA - UNIVERSITÀ DEGLI STUDI DEL MOLISE ***");
            Console.WriteLine("*** CANDIDATI:" +
                              $"\t{datiUtenti[1]}" +
                              $"\t{datiUtenti[2]}" +
                              $"\t{datiUtenti[3]}");

        }


        private string CreaUtenteTest(ref int num)
        {
            Console.WriteLine("Creato utente di prova");
            ++num;
            return $"prova{num}";

        }

        private void AvviaMenu()
        {

            Benvenuto();

            SmartContract.Inizializza();

            do
            {
                Console.WriteLine("\n\t*** CREA UTENTI PER UNIMOL COIN ***");
                Console.WriteLine("Inserisci un nome utente:");
                string nome = Console.ReadLine();

                if (!string.IsNullOrEmpty(nome))
                {
                    NormalizzaNome(ref nome);
                    Console.WriteLine($"Utente corrente: {nome}");
                }
                else
                {
                    nome = CreaUtenteTest(ref _numUtentiTest);
                }

                UniMolCoin.Utenti.Add(new Utente(nome));

                Console.Write("\tVuoi inserire un altro utente? Digita 0 per terminare: ");
                _risposta = Console.ReadLine();
                Console.WriteLine();

            } while (!Annulla.ToString().Equals(_risposta));


            while (UniMolCoin.Utenti.Count < 3)
            {
                UniMolCoin.Utenti.Add(new Utente(CreaUtenteTest(ref _numUtentiTest)));
            }



            int selezione = Annulla;

            while (!Esci.Equals(selezione))
            {

                Console.WriteLine("\n\t*** MENU UNIMOL COIN ***");
                Console.WriteLine("=========================");
                Console.WriteLine($"{AggiungiTransazione}. Aggiungere una transazione.");
                Console.WriteLine($"{MostraBlockchain}. Mostra la BlockChain.");
                Console.WriteLine($"{MostraSmartContract}. Mostra lo SmartContract di UniMol Coin.");
                Console.WriteLine($"{Esci}. Esci.");
                Console.WriteLine("=========================");
                Console.Write("\tInserisci una scelta: ");

                string azione = Console.ReadLine();

                try
                {
                    selezione = Convert.ToInt16(azione);
                    Console.Clear();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Operazione non riconosciuta, riprova!");
                    Console.WriteLine(exception.Message);
                }

                switch (selezione)
                {

                    case AggiungiTransazione:

                        Console.WriteLine("*** REGISTRA TRANSAZIONE ***");
                        Console.WriteLine($"Per favore, inserisci il nome del mittente ( {Annulla} per annullare)");
                        string mittente = Console.ReadLine();

                        if ((mittente == Annulla.ToString()) || (string.IsNullOrEmpty(mittente)))
                        {
                            break;
                        }

                        NormalizzaNome(ref mittente);

                        Console.WriteLine($"Per favore, inserisci il nome del destinatario ( {Annulla} per annullare)");
                        string destinatario = Console.ReadLine();
                        if ((destinatario == Annulla.ToString()) || (string.IsNullOrEmpty(destinatario)))
                        {
                            break;
                        }

                        NormalizzaNome(ref destinatario);

                        Console.WriteLine($"Per favore, inserisci l'importo ( {Annulla} per annullare)");
                        string importo = Console.ReadLine();

                        if (importo == Annulla.ToString())
                        {
                            break;
                        }

                        if ((UniMolCoin.VerificaUtente(mittente)) && (UniMolCoin.VerificaUtente(destinatario)))
                        {

                            if (SmartContract.ValidaTransazione(mittente, Convert.ToInt32(importo)))
                            {

                                UniMolCoin.CreaTransazione(new Transazione(UniMolCoin.RicercaUtente(mittente),
                                    UniMolCoin.RicercaUtente(destinatario), Convert.ToInt32(importo)));

                                Random randomMiner = new Random();

                                //il miner sarà estratto casualmente tra la lista degli utenti
                                UniMolCoin.MinaTransazioni(UniMolCoin.Utenti[randomMiner.Next(0, UniMolCoin.Utenti.Count)]);

                                Console.WriteLine(JsonConvert.SerializeObject(UniMolCoin));

                                UniMolCoin.AggiornaBilancio();
                            }
                            else
                            {
                                Console.WriteLine("Transazione non valida, importo più alto della capacità di spesa del mittente.");
                                break;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Verificare i valori inseriti di mittente e destinatario.");
                        }

                        break;

                    case MostraBlockchain:
                        Console.WriteLine("*** MOSTRA BLOCKCHAIN ***");
                        int moneteCircolanti = UniMolCoin.AggiornaBilancio();
                        Console.WriteLine($"Sono in circolazione {moneteCircolanti} UniMolCoin");
                        Console.WriteLine(JsonConvert.SerializeObject(UniMolCoin, Formatting.Indented));
                        break;

                    case MostraSmartContract:
                        SmartContract.MostraContratto();
                        break;

                    case Esci:
                        Console.WriteLine("Arrivederci!");
                        break;

                    default:
                        Console.WriteLine("Operazione non riconosciuta, riprova!");
                        break;

                }

            }

            #region VecchiaImplementazione

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
            //Console.WriteLine($"\tBilancio Angelo: { unimolCoin.AggiornaBilancio("Angelo")}");
            //Console.WriteLine($"\tBilancio Giusy: { unimolCoin.AggiornaBilancio("Giusy")}");
            //Console.WriteLine($"\tBilancio Carmen: { unimolCoin.AggiornaBilancio("Carmen")}");

            //Console.WriteLine("----------------------------");

            //Console.WriteLine("\nUniMolCoin:");
            //Console.WriteLine($"unimolCoin");
            //Console.WriteLine(JsonConvert.SerializeObject(unimolCoin, Formatting.Indented));

            ////aspetta la pressione di un tasto per la terminazione del programma
            //Console.WriteLine("\nEsecuzione terminata.\nPremere un tasto per uscire...");

            #endregion
        }
    }
}