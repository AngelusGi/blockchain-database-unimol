using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp;

namespace BlockChain
{
    internal class P2PClient
    {
        /// <summary>
        /// Gestisce la parte client della socket
        /// </summary>

        private readonly IDictionary<string, WebSocket> _webSocketDictionary = new Dictionary<string, WebSocket>();

        public void Connetti(string url)
        {
            if (!_webSocketDictionary.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);

                //espressione lambda =>
                //aggiunge mittente ed evento al messaggio trasmesso dalla socket. L'if verifica che si tratti della fase di instaurazione
                //della connessione o se si tratta di un blocco
                webSocket.OnMessage += (mittente, evento) =>
                {
                    if (evento.Data.Contains("Ciao Client"))
                    {
                        Console.WriteLine(evento.Data);
                    }
                    else
                    {
                        BlockChain nuovaCatena = JsonConvert.DeserializeObject<BlockChain>(evento.Data);
                        if (nuovaCatena.IsValido() && nuovaCatena.Catena.Count > Program.UniMolCoin.Catena.Count)
                        {

                            List<Transazione> nuoveTransazioni = new List<Transazione>();

                            nuoveTransazioni.AddRange(nuovaCatena.TransazioniInAttesa);
                            nuoveTransazioni.AddRange(Program.UniMolCoin.TransazioniInAttesa);

                            nuovaCatena.TransazioniInAttesa = nuoveTransazioni;

                            Program.UniMolCoin = nuovaCatena;
                        }
                    }
                };

                webSocket.Connect();

                webSocket.Send($"Dalla porta {Program.Porta}: Ciao Server");
                webSocket.Send(JsonConvert.SerializeObject(Program.UniMolCoin));

                _webSocketDictionary.Add(url, webSocket);
            }
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
