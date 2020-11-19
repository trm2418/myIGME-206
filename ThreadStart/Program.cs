using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadStart
{
	public class CounterThread
	{
		public static void run()
		{
			for (int i = 0; i < 10; i++)
            {
				Console.WriteLine("Count:  " + i);
				//Thread.Sleep(500);
			}
		}

		public static void Main(string[] args)
		{
			Thread ct = new Thread(run);
			Thread ct2 = new Thread(run);
			Thread ct3 = new Thread(run);
			Thread ct4 = new Thread(run);
			Thread ct5 = new Thread(run);
			ct.Start();
			ct2.Start();
			ct3.Start();
			ct4.Start();
			ct5.Start();
			Console.ReadLine();
		}
	}
}
