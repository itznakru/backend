namespace DbScanner.Process.Infrastruction
{
    public class SingletonProcessInfrastraction
    {
        private static IProcessInfrastraction s_instance;
        public static IProcessInfrastraction Itstance
        {
            get
            {
                return s_instance ??= new ProcessInfrastraction();
            }
        }
    }
}