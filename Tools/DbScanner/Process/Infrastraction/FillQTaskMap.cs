
using DbScanner.Process;
using Microsoft.Extensions.DependencyInjection;


namespace DbScanner.Process.Infrastruction
{
    public static class FillQTaskMap
    {
        static readonly Action<IServiceCollection> addFillTaskQProcess = (services) => _ = services.AddSingleton<IFillTaskQProcess, TransferFillTaskQProcess>();
        static readonly Action<IServiceCollection> addFillParseTaskQProcess = (services) => _ = services.AddSingleton<IFillTaskQProcess, ParseFillTaskQProcess>();
        public static Dictionary<ACTIONTYPE, Action<IServiceCollection>> Map => new()
        {
                {ACTIONTYPE.TRANSFER,addFillTaskQProcess},
                {ACTIONTYPE.PARSE,addFillParseTaskQProcess},
            };
    }
}