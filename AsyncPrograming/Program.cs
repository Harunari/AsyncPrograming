using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncPrograming
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //スレッドの処理の戻り値は、スレッドに投入したメソッドの戻り値のことですから、intです。
            //しかしintがもらえるのはスレッドの処理が完了した後なので、
            //すぐに int がもらえるとおかしいのです。
            var task = Task.Run(() =>
            {
                int total = 0;
                for (int i = 0; i < 100; ++i)
                {
                    total += 1;
                }
                Thread.Sleep(4560);
                return total;
            });
            // int result = task.Result; // スレッドの終了まで「待つ」

            // awaitがあるとメインスレッドでは一度returnし、子スレッドでTask開始。
            int result = await task; // スレッドの処理の結果を待ち受ける 
        }

        static int Calculate()
        {
            int total = 0;
            for (int i = 1; i < 100; i++)
            {
                total += i;
            }
            Thread.Sleep(4560); // 何か重い処理をしている
            return total;
        }

        static async Task A()
        {
            int m = 999;
            var task = Task.Run(() =>
            {
                Thread.Sleep(3000);
                return m * 2;
            });
            m = 100;

            int result = await task; // 通常、この行の直前でTaskが子スレッドで開始され、このメソッドはいったん一旦リターンされる
            Console.WriteLine($"{result}"); // 値は200
        }

        static async Task B()
        {
            int m = 999;
            var task = new Task<int>(x =>
            {
                Thread.Sleep(3000);
                return (int)x * 2;
            }, m);
            task.Start(); // このStartメソッドで任意の位置からTaskを子スレッドで実行できる。

            m = 100;

            int result = await task;
            Console.WriteLine($"{result}"); // 値は1998

        }
    }
}
