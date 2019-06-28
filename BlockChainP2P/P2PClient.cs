﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using WebSocketSharp;

namespace BlockChain
{
    internal class P2PClient
    {
        /***
         * TODO
         * implementare connessione alla web socket da parte del client per la gestione delle transazioni e per il mining
         * **/

        private readonly IDictionary<string, WebSocket> _webSocketDictionary = new Dictionary<string, WebSocket>();

        public void Connetti(string url)
        {
            if (!_webSocketDictionary.ContainsKey(url))
            {
                WebSocket webSocket = new WebSocket(url);

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

                            // TODO, DA RISOLVERE

                            nuoveTransazioni.AddRange(nuovaCatena.MinaTransazioni);
                            nuoveTransazioni.AddRange(Program.UniMolCoin.MinaTransazioni);

                            nuoveTransazioni = nuovaCatena.MinaTransazioni();

                            nuovaCatena.MinaTransazioni() = nuoveTransazioni;
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
