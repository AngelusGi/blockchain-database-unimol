using System;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace BlockChain
{

    /// <summary>Gestisce la parte server della Socket</summary>
    /// <seealso cref="WebSocketSharp.Server.WebSocketBehavior" />
    internal class P2PServer : WebSocketBehavior
    {
        
        private bool _bloccoSincronizzato = false;
        private WebSocketServer _webSocketServer = null;

        public void Start()
        {
            _webSocketServer = new WebSocketServer($"ws://127.0.0.1:{Program.Porta}");
            _webSocketServer.AddWebSocketService<P2PServer>("/Blockchain");
            _webSocketServer.Start();
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
                BlockChain nuovaCatena = JsonConvert.DeserializeObject<BlockChain>(evento.Data);

                if (nuovaCatena.IsValido() && nuovaCatena.Catena.Count > Program.UniMolCoin.Catena.Count)
                {
                    Program.UniMolCoin.Catena = nuovaCatena.Catena;
                }

                if (!_bloccoSincronizzato)
                {
                    Send(JsonConvert.SerializeObject(Program.UniMolCoin));
                    _bloccoSincronizzato = true;
                }
            }
        }

    }

}

