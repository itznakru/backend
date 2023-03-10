
using DbScanner.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace DbScanner.Process.Infrastruction
{

    public static class ActionsMap
    {
        static readonly Action<IServiceCollection> addTransferAction = (services) => _ = services.AddSingleton<IScanerAction, TranserAction>();
        static readonly Action<IServiceCollection> addParseAction = (services) => _ = services.AddSingleton<IScanerAction, ParseAction>();
        static readonly Action<IServiceCollection> fillNodeBytemplatesAction = (services) => _ = services.AddSingleton<IScanerAction, FillNodeBytemplatesAction>();
        public static Dictionary<ACTIONTYPE, Action<IServiceCollection>> Map => new()
        {
                {ACTIONTYPE.TRANSFER,addTransferAction},
                {ACTIONTYPE.PARSE,addParseAction},
                {ACTIONTYPE.FILLNODEBYTEMPLATES,fillNodeBytemplatesAction},
            };
    }
}