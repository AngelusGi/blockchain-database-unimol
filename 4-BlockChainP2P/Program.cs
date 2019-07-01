using System;

namespace _4_BlockChainP2P
{
    internal class Program
    {
        private static void TemaChiaro()
        {
            //metodo usato solo per fare gli screenshot da allegare alla documentazione
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
        }

        public static void Main()
        {

            //TemaChiaro();

            new Menu();
        }
    }
}
