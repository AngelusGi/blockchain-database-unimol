namespace BlockChainMenu
{
    public class Utente
    {
        public string Nome { get; private set; }
        public int Saldo { get; set; }
        public string IdUnivoco { get; private set; }

        public Utente(string nome)
        {
            Nome = nome;
            Saldo = 10;
            IdUnivoco = SmartContract.AutenticaUtente(this);
        }
    }
}