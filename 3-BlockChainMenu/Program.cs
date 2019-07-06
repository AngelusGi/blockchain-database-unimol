using System;

namespace _3_BlockChainMenu
{
    internal class Program
    {
        private static void TemaChiaro()
        {
            //metodo usato solo per fare gli screenshot da allegare alla documentazione, si attiva se compilato in modalità debug
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
        }

        public static void Main()
        {

#if DEBUG
            TemaChiaro();
#endif

            var menu = new Menu();
            menu.AvviaMenu();

        }
    }
}
