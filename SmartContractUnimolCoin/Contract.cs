using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Services.Neo;

namespace SmartContractUnimolCoin
{
    public class Contract : SmartContract
    {
        public static void Main()
        {
            Storage.Put("Angelo", "Gino");
        }
    }
}
