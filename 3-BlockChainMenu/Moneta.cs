namespace _3_BlockChainMenu
{
    /// <summary>
    /// Classe per gestire la moneta
    /// </summary>
    internal sealed class Moneta
    {


        #region Membri

        public static readonly int Valore = 1;

        public int? IdVecchioProprietario { get; private set; }

        public int IdAttualeProprietario { get; private set; }


        #region Documentazione
        /// <summary>
        /// Codice identificativo della moneta.
        /// </summary>
        /// <value>
        /// Codice hash della moneta (int)
        /// </value>
        #endregion
        public int IdMoneta { get; private set; }
        #endregion


        public Moneta(int idAttualeProprietario)
        {
            IdAttualeProprietario = idAttualeProprietario;
            IdMoneta = GetHashCode();
        }


        #region Documentazione

        /// <summary>
        /// Trasferisce la moneta dal proprietario al destinatario della transazione.
        /// </summary>
        /// <param name="idNuovoProprietario">Identificativo dell'utente a cui deve essere inviata.</param>
        /// <returns>Trasferimento effettuato con successo (bool)</returns>

        #endregion
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