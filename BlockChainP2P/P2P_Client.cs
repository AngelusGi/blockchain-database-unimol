using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;

namespace BlockChain
{
    class P2P_Client 
    {
        /***
         *todo
         * implementare connessione alla web socket da parte del client per la gestione delle transazioni e per il mining
         * **/

        IDictionary<string, WebSocket> WebSocketDictionary = new Dictionary<string, WebSocket>();

        public void Connetti(string url)
        {
            if (!WebSocketDictionary.ContainsKey(url))
            {
                WebSocket WebSocket = new WebSocket(url);

                WebSocket.OnMessage += (mittente, evento) =>
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

                            nuoveTransazioni.AddRange(nuovaCatena.MinaTransazioni);
                            nuoveTransazioni.AddRange(Program.UniMolCoin.MinaTransazioni);

                            nuovaCatena.MinaTransazioni() = nuoveTransazioni;
                            Program.UniMolCoin = nuovaCatena;
                        }
                    }
                };

                WebSocket.Connect();

                WebSocket.Send($"Dalla porta {Program.Porta}: Ciao Server");
                WebSocket.Send(JsonConvert.SerializeObject(Program.UniMolCoin));

                WebSocketDictionary.Add(url, WebSocket);
            }
        }

        public void Send(string url, string data)
        {
            foreach (var item in WebSocketDictionary)
            {
                if (item.Key == url)
                {
                    item.Value.Send(data);
                }
            }
        }

        public void Broadcast(string data)
        {
            foreach (var item in WebSocketDictionary)
            {
                item.Value.Send(data);
            }
        }

        public IList<string> GetServers()
        {
            IList<string> servers = new List<string>();
            foreach (var item in WebSocketDictionary)
            {
                servers.Add(item.Key);
            }
            return servers;
        }

        public void Close()
        {
            foreach (var item in WebSocketDictionary)
            {
                item.Value.Close();
            }
        }

    }
}
