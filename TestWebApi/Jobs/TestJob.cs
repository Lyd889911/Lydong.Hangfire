using Lydong.Hangfire.Symbols;

namespace TestWebApi.Jobs
{
    public class TestJob:IJob
    {
        [Job(Cron = "*/2 * * * * *")]
        public void T1()
        {
            Console.WriteLine($"t1:{DateTime.Now}");
        }

        [Job(Cron = "*/7 * * * * *")]
        public void T2()
        {
            Console.WriteLine($"t2:{DateTime.Now}");
        }

        [Job(Cron = "*/17 * * * * *")]
        public void T3()
        {
            Console.WriteLine($"t3:{DateTime.Now}");
        }
    }
}
