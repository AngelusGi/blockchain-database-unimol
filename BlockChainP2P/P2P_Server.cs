using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace BlockChain
{
    class P2PServer : WebSocketBehavior
    {
        /***
         * implementare web socket per la gestione dei client connessi
         */


        bool BloccoSincronizzato = false;
        WebSocketServer webSocketServer = null;

        public void Start()
        {
            webSocketServer = new WebSocketServer($"ws://127.0.0.1:{Program.Porta}");
            webSocketServer.AddWebSocketService<P2PServer>("/Blockchain");
            webSocketServer.Start();
            Console.WriteLine($"Server inizializzato a ws://127.0.0.1:{Program.Porta}");
        }

        protected override void OnMessage(MessageEventArgs evento)
        {
            if (evento.Data.Contains("Ciao Server"))
            {
                Console.WriteLine(evento.Data);
                Send($"Da porta {Program.Porta}: Ciao Client");
            }
            else
            {
                BlockChain NuovaCatena = JsonConvert.DeserializeObject<BlockChain>(evento.Data);

                if (NuovaCatena.IsValido() && NuovaCatena.Catena.Count > Program.UniMolCoin.Catena.Count)
                {
                    Program.UniMolCoin.Catena = NuovaCatena.Catena;
                }

                if (!BloccoSincronizzato)
                {
                    Send(JsonConvert.SerializeObject(Program.UniMolCoin));
                    BloccoSincronizzato = true;
                }
            }
        }

    }

}

