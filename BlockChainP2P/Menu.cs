using System;
using System.Runtime.InteropServices;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace BlockChainMenu
{

    internal class Menu
    {

        public static BlockChain UniMolCoin { get; private set; }

        private const int Esci = 5;
        private const int AggiungiTransazione = 1;
        private const int MostraBlockchain = 2;
        private const int MostraSmartContract = 3;
        private const int VerificaSaldo = 4;
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

        private void Benvenuto()
        {
            ColoreRecap();
            //todo inserire nomeProf + nome cognome e matricole nostre
            string[] unimol = new string[4];

            for (int i = 0; i < unimol.Length; i++)
            {
                if (!string.IsNullOrEmpty(unimol[i]))
                {
                    unimol[i] = unimol[i].ToUpperInvariant();
                }
            }

            Console.WriteLine("\t*** PROGETTO BASI DI DATI E SISTEMI INFORMATIVI: BLOCKCHAIN 'UNIMOL COIN' ***");
            Console.WriteLine($"*** PROF. {unimol[0]}***");
            Console.WriteLine("*** CDL IN INFORMATICA - UNIVERSITÀ DEGLI STUDI DEL MOLISE ***");
            Console.WriteLine("*** CANDIDATI:" +
                              $"\t{unimol[1]}" +
                              $"\t{unimol[2]}" +
                              $"\t{unimol[3]}");

            ColoreNormale();
        }


        private string CreaUtenteTest(ref int num)
        {
            ColoreAvvisi();
            Console.WriteLine("Creato utente di prova");
            ColoreNormale();
            ++num;
            return $"prova{num}";
        }

        private void GestioneUtenti()
        {
            do
            {
                ColoreTitoli();
                Console.WriteLine("\n\t*** CREA UTENTI PER UNIMOL COIN ***");
                ColoreNormale();

                Console.WriteLine("Inserisci un nome utente (minimo 4 caratteri):");
                string nome = Console.ReadLine();

                if (!string.IsNullOrEmpty(nome) && nome.Length > 3)
                {
                    NormalizzaNome(ref nome);
                }
                else
                {
                    nome = CreaUtenteTest(ref _numUtentiTest);
                }

                Utente utenteDaRegistrare = new Utente(nome);

                if (SmartContract.VerificaOmonimie(utenteDaRegistrare))
                {
                    Console.WriteLine($"Utente registrato: {nome}");
                    UniMolCoin.Utenti.Add(utenteDaRegistrare);
                }
                else
                {
                    ColoreAvvisi();
                    Console.WriteLine("\t*** Errore utente già presente. ***");
                    ColoreNormale();
                }
                
                ColoreRecap();
                Console.Write("\tVuoi inserire un altro utente? Digita 0 per terminare o qualunque altro tasto per continuare: ");

                ColoreNormale();

                _risposta = Console.ReadLine();
                Console.WriteLine();

            } while (!Annulla.ToString().Equals(_risposta));


            while (UniMolCoin.Utenti.Count < 3)
            {
                Utente utenteTest = new Utente(CreaUtenteTest(ref _numUtentiTest));
                if (SmartContract.VerificaOmonimie(utenteTest))
                {
                    UniMolCoin.Utenti.Add(utenteTest);
                }
                
            }

            

            ColoreRecap();
            Console.WriteLine("\n\t*** Riepilogo utenti creati ***");
            ColoreNormale();
            foreach (Utente utente in UniMolCoin.Utenti)
            {
                Console.WriteLine($"\tNome: {utente.Nome} \t ID assciato: {utente.IdUnivoco}");

            }

        }

        private void GestioneMenu()
        {

            int selezione = Annulla;

            do
            {
                int moneteCircolanti = UniMolCoin.AggiornaBilancio();

                ColoreTitoli();
                Console.WriteLine("\n\t*** MENU UNIMOL COIN ***");
                Console.WriteLine("\t=========================");
                Console.WriteLine($"\t{AggiungiTransazione}. Aggiungere una transazione.");
                Console.WriteLine($"\t{MostraBlockchain}. Mostra la BlockChain.");
                Console.WriteLine($"\t{MostraSmartContract}. Mostra lo SmartContract di UniMol Coin.");
                Console.WriteLine($"\t{VerificaSaldo}. Verifica saldo di uno specifico utente.");
                Console.WriteLine($"\t{Esci}. Esci.");
                Console.WriteLine("\t=========================");
                ColoreNormale();
                Console.Write("\tInserisci una scelta: ");

                string azione = Console.ReadLine();

                try
                {
                    selezione = Convert.ToInt16(azione);
                    Console.Clear();
                }
                catch (Exception exception)
                {
                    ColoreAvvisi();
                    Console.WriteLine("\t\n*** Errore. Operazione non riconosciuta, riprova! ***");
                    ColoreNormale();
                    Console.WriteLine(exception.Message);
                }

                switch (selezione)
                {

                    case AggiungiTransazione:
                        ColoreTitoli();
                        Console.WriteLine("\t\n*** REGISTRA TRANSAZIONE ***");
                        ColoreNormale();
                        Console.WriteLine($"Per favore, inserisci il nome del mittente ( {Annulla} per annullare)");
                        string nomeMittente = Console.ReadLine();

                        if ((nomeMittente == Annulla.ToString()) || (string.IsNullOrEmpty(nomeMittente)))
                        {
                            break;
                        }

                        NormalizzaNome(ref nomeMittente);

                        Console.WriteLine($"Per favore, inserisci il nome del destinatario ( {Annulla} per annullare)");
                        string nomeDestinatario = Console.ReadLine();
                        if ((nomeDestinatario == Annulla.ToString()) || (string.IsNullOrEmpty(nomeDestinatario)))
                        {
                            break;
                        }

                        NormalizzaNome(ref nomeDestinatario);

                        Console.WriteLine($"Per favore, inserisci l'importo ( {Annulla} per annullare)");
                        string importo = Console.ReadLine();

                        if (importo == Annulla.ToString())
                        {
                            break;
                        }

                        Utente mittente = UniMolCoin.RicercaUtente(nomeMittente);
                        Utente destinatario = UniMolCoin.RicercaUtente(nomeDestinatario);

                        if ((UniMolCoin.VerificaUtente((int)mittente.IdUnivoco)) &&
                            (UniMolCoin.VerificaUtente((int)destinatario.IdUnivoco)))
                        {

                            if (SmartContract.VerificaSaldo(nomeMittente, Convert.ToInt32(importo)))
                            {

                                UniMolCoin.CreaTransazione(new Transazione(mittente, destinatario,Convert.ToInt32(importo)));

                                Random randomMiner = new Random();

                                //il miner sarà estratto casualmente tra la lista degli utenti
                                UniMolCoin.MinaTransazioni(UniMolCoin.Utenti[randomMiner.Next(0, UniMolCoin.Utenti.Count)]);

                                Console.WriteLine(JsonConvert.SerializeObject(UniMolCoin, Formatting.Indented));
                                
                            }
                            else
                            {
                                ColoreAvvisi();
                                Console.WriteLine("\t*** Errore: Transazione non valida, importo più alto della capacità di spesa del mittente. ***");
                                ColoreNormale();
                            }

                        }
                        else
                        {
                            ColoreAvvisi();
                            Console.WriteLine("\t*** Errore. Verificare i valori inseriti di mittente e destinatario. ***");
                            ColoreNormale();
                        }

                        break;

                    case MostraBlockchain:
                        ColoreTitoli();
                        Console.WriteLine("\t\n*** MOSTRA BLOCKCHAIN ***");
                        ColoreNormale();
                        Console.WriteLine(JsonConvert.SerializeObject(UniMolCoin, Formatting.Indented));
                        ColoreRecap();
                        Console.WriteLine($"Sono in circolazione {moneteCircolanti} UniMolCoin");
                        ColoreNormale();
                        break;

                    case MostraSmartContract:
                        ColoreTitoli();
                        Console.WriteLine("\t\n*** MOSTRA SMART CONTRACT ***\n");
                        ColoreRecap();
                        SmartContract.MostraContratto();
                        ColoreNormale();
                        break;

                    case VerificaSaldo:
                        ColoreTitoli();
                        Console.WriteLine("\t\n*** MOSTRA SALDO UTENTE ***");
                        ColoreNormale();
                        Console.WriteLine("Inserisci il nome dell'utente di cui mostrare il saldo: ");
                        string nomeUtente = Console.ReadLine();
                        
                        try
                        {
                            NormalizzaNome(ref nomeUtente);
                            Utente utenteCercato = UniMolCoin.RicercaUtente(nomeUtente);
                            ColoreRecap();
                            Console.WriteLine($"\tNome: {utenteCercato.Nome}" +
                                              $"\n\tID: {utenteCercato.IdUnivoco}" +
                                              $"\n\tSaldo: {utenteCercato.Saldo}");
                        }
                        catch (Exception eccezione)
                        {
                            ColoreAvvisi();
                            Console.WriteLine("\t*** Errore. Input non valido o utente non trovato. ***");
                            
                        }
                        
                        ColoreNormale();
                        break;

                    case Esci:
                        ColoreAvvisi();
                        Console.WriteLine("\t\n*** Arrivederci! ***");
                        ColoreNormale();
                        break;

                    default:
                        Console.Clear();
                        ColoreAvvisi();
                        Console.WriteLine("\t\n*** Errore. Operazione non riconosciuta, riprova! ***");
                        ColoreNormale();
                        break;
                }

            } while (!Esci.Equals(selezione));
        }

        private void AvviaMenu()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ColoreAvvisi();
                Console.WriteLine("CapsLock attivato: {0}\nNumLock attivato: {1}\n", Console.CapsLock, Console.NumberLock);
            }
            

            Benvenuto();

            SmartContract.Inizializza();

            GestioneUtenti();

            GestioneMenu();

        }

        #region GestioneColori

        private void NormalizzaNome(ref string nome)
        {
            nome = nome.Trim();
            nome = nome.ToLowerInvariant();
        }

        private void ColoreAvvisi()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        private void ColoreRecap()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private void ColoreTitoli()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }

        private void ColoreNormale()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion

    }
}