namespace _3_BlockChainMenu
{
    internal class Moneta
    {
        public static readonly int Valore = 1;

        public int? IdVecchioProprietario { get; private set; }

        public int IdAttualeProprietario { get; private set; }

        public int IdMoneta { get; private set; }

        public Moneta(int idAttualeProprietario)
        {
            IdAttualeProprietario = idAttualeProprietario;
            IdMoneta = GetHashCode();
        }

        public bool TrasferisciMoneta(int idNuovoProprietario)
        {

            if (IdAttualeProprietario != idNuovoProprietario)
            {
                IdVecchioProprietario = IdAttualeProprietario;
                IdAttualeProprietario = idNuovoProprietario;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}