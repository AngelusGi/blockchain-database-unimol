using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp;

namespace _4_BlockChainP2P
{

    /// <summary>Gestisce la parte client della socket</summary>
    internal class P2PClient
    {


        #region Membri

        private readonly IDictionary<string, WebSocket> _webSocketDictionary = new Dictionary<string, WebSocket>();
        public static int Porta { get; private set; }
        private const int MinPorta = 1024;
        private const int MaxPorta = 49151;

        #endregion


        public void Connetti(string url)
        {

            Porta = SelezionaPorta();

            if (!_webSocketDictionary.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);

                #region Spiegazione

                //espressione lambda =>
                //aggiunge mittente ed evento al messaggio trasmesso dalla socket. L'if verifica che si tratti della fase di instaurazione
                //della connessione o se si tratta di un blocco

                #endregion
                webSocket.OnMessage += (mittente, evento) =>
                {
                    if (evento.Data.Contains("Ciao Client"))
                    {
                        Console.WriteLine(evento.Data);
                    }
                    else
                    {
                        BlockChain nuovaCatena = JsonConvert.DeserializeObject<BlockChain>(evento.Data);
                        if (nuovaCatena.IsValido() && nuovaCatena.Catena.Count > Menu.UniMolCoin.Catena.Count)
                        {

                            List<Transazione> nuoveTransazioni = new List<Transazione>();

                            nuoveTransazioni.AddRange(nuovaCatena.TransazioniInAttesa);
                            nuoveTransazioni.AddRange(Menu.UniMolCoin.TransazioniInAttesa);

                            nuovaCatena.TransazioniInAttesa = nuoveTransazioni;

                            Menu.UniMolCoin = nuovaCatena;
                        }
                    }
                };

                webSocket.Connect();

                webSocket.Send($"Dalla porta {Porta}: Ciao Server");
                webSocket.Send(JsonConvert.SerializeObject(Menu.UniMolCoin));

                _webSocketDictionary.Add(url, webSocket);
            }
        }


        private int SelezionaPorta()
        {
            Random portaRandom = new Random();
            return portaRandom.Next(MinPorta, MaxPorta);
        }


        public void Send(string url, string data)
        {
            foreach (KeyValuePair<string, WebSocket> item in _webSocketDictionary)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }


        public void Broadcast(string data)
        {
            foreach (KeyValuePair<string, WebSocket> item in _webSocketDictionary)
            {
                item.Value.Send(data);
            }
        }


        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (KeyValuePair<string, WebSocket> item in _webSocketDictionary)
            {
                servers.Add(item.Key);
            }
            return servers;
        }


        public void Close()
        {
            foreach (KeyValuePair<string, WebSocket> item in _webSocketDictionary)
            {
                item.Value.Close();
            }
        }

    }
}
