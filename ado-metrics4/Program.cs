using System;
using System.Threading.Tasks;

namespace adometrics
{
    class MainClass
    {
        public async static Task Main(string[] args)
        {
            Console.WriteLine("Launching app");

            QueryExecutor qe = new QueryExecutor();

            await qe.PrintOpenBugsAsync();
        }
    }
}
