using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Redis;

namespace CacheTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RedisClient("192.168.107.128", 6379);
            client.Set("location", "www.crsky.com");
            string location = Encoding.Default.GetString(client.Get("location"));
            Console.WriteLine(location);

            List<TestClass> testClassList = new List<TestClass>();
            testClassList.Add(new TestClass() { TestId = 1, TestName = "A" });
            testClassList.Add(new TestClass() { TestId = 2, TestName = "B" });
            testClassList.Add(new TestClass() { TestId = 3, TestName = "C" });
            client.Set<List<TestClass>>("testclassList", testClassList);
            var result = client.Get<List<TestClass>>("testclassList");
            Console.WriteLine(string.Format("{0}", "{1}", "{2}"), result.Count,
                string.Join(",", testClassList.Select(x => x.TestId).ToList()),
                string.Join(",", testClassList.Select(x => x.TestName).ToList()));
            if (client.ContainsKey("testclassList"))
            {
                Console.WriteLine("exist key testclassList");
            }
            client.Remove("testclassList");
            Console.WriteLine("remove key testclassList");
            if (!client.ContainsKey("testclassList"))
            {
                Console.WriteLine("not exist key testclassList");
            }


            Console.ReadLine();
        }

        public class TestClass
        {
            public string TestName { get; set; }

            public int TestId { get; set; }
        }
    }
}
