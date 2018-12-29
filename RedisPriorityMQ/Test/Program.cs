using RedisHelper;
using System;
using System.Threading;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //写点数据进去
            using (DoRedisList helper = new DoRedisList())
            {
                for (int i = 0; i < 100; i++)
                {
                    helper.Add("Priority:Low", i + "");//低优先级队列
                    helper.Add("Priority:High", i + 100 + "");//高优先级队列
                    helper.Add("Priority:Middle", i + 50 + "");//中优先级队列
                }
            }

            //消费者
            Thread t = new Thread(() =>
            {
                using (DoRedisList helper = new DoRedisList())
                {
                    while (true)
                    {
                        var item = helper.BlockingPopItemFromLists(new string[] { "Priority:High", "Priority:Middle", "Priority:Low" }, new TimeSpan(100000));
                        Console.WriteLine(item.Item);
                    }
                }
            })
            { IsBackground = true };
            t.Start();

            Console.ReadKey();
        }
    }
}
