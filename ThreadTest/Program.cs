using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ThreadTest
{
	public class ThreadTest
	{
		private Thread test;
		private int number;

		public ThreadTest(int number)
		{
			test = new Thread(new ThreadStart(run));
			this.number = number;
			test.Start();
		}
		private void run()
		{
			for (int i = 0; i < 100; i++)
			{
				System.Console.Write("{0}", number);
			}
		}
		public static void Main(string[] args)
		{
			System.Console.WriteLine("Main Started.");
			ThreadTest myTest = new ThreadTest(1);
			ThreadTest myTest2 = new ThreadTest(2);
			System.Console.WriteLine("Main Ended.");
		}
	}

}
