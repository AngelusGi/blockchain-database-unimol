using System;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Server;


namespace _4_BlockChainP2P
{

    /// <summary>Gestisce la parte server della Socket</summary>
    /// <seealso cref="WebSocketSharp.Server.WebSocketBehavior" />
    internal class P2PServer : WebSocketBehavior
    {
        private const int MinPorta = 1024;
        private const int MaxPorta = 49151;
        private bool _bloccoSincronizzato = false;
        private WebSocketServer _webSocketServer = null;

        public void Start()
        {
            int portaServer = SelezionaPorta();

            _webSocketServer = new WebSocketServer($"ws://127.0.0.1:{portaServer}");
            _webSocketServer.AddWebSocketService<P2PServer>("/Blockchain");
            _webSocketServer.Start();
            Console.WriteLine($"Server inizializzato a ws://127.0.0.1:{portaServer}");
        }


        private int SelezionaPorta()
        {
            Random portaRandom = new Random();
            return portaRandom.Next(MinPorta, MaxPorta);
        }


        protected override void OnMessage(MessageEventArgs evento)
        {
            if (evento.Data.Contains("Ciao Server"))
            {
                Console.WriteLine(evento.Data);
                Send($"Da porta {P2PClient.Porta}: Ciao Client");
            }
            else
            {
                BlockChain nuovaCatena = JsonConvert.DeserializeObject<BlockChain>(evento.Data);

                if (nuovaCatena.IsValido() && nuovaCatena.Catena.Count > Menu.UniMolCoin.Catena.Count)
                {
                    Menu.UniMolCoin.Catena = nuovaCatena.Catena;
                }

                if (!_bloccoSincronizzato)
                {
                    Send(JsonConvert.SerializeObject(Menu.UniMolCoin));
                    _bloccoSincronizzato = true;
                }
            }
        }

    }

}

