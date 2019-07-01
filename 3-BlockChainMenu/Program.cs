using System;

namespace _3_BlockChainMenu
{
    internal class Program
    {

        private static void TemaChiaro()
        {
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
