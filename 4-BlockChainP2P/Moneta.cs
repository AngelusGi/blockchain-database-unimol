namespace _4_BlockChainP2P
{
    /// <summary>
    /// Classe per gestire la moneta
    /// </summary>
    internal class Moneta
    {
        public static readonly int Valore = 1;

        public int? IdVecchioProprietario { get; private set; }

        public int IdAttualeProprietario { get; private set; }

        /// <summary>
        /// Codice identificativo della moneta.
        /// </summary>
        /// <value>
        /// Codice hash della moneta (int)
        /// </value>
        public int IdMoneta { get; private set; }

        public Moneta(int idAttualeProprietario)
        {
            IdAttualeProprietario = idAttualeProprietario;
            IdMoneta = GetHashCode();
        }


        /// <summary>
        /// Trasferisce la moneta dal proprietario al destinatario della transazione.
        /// </summary>
        /// <param name="idNuovoProprietario">Identificativo dell'utente a cui deve essere inviata.</param>
        /// <returns>Trasferimento effettuato con successo (bool)</returns>
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