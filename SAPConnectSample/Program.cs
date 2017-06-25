using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPConnectSample
{
    class Program
    {
        static void Main(string[] args)
        {
            SAPConnectSample sample = new SAPConnectSample();
            sample.processe();
            Console.WriteLine("......................The End! .......................");
            Console.ReadKey();
        }
    }
}
