
/*=======================================================================================================================================================  
    Проект:http://itznak.ru/ = плагин поиска и регистрации товарных знаков для юридических компаний.

    Утилита сканирования коллекции БД: 
    Цель: произвести обработку каждой строки коллекции, одним из достуных обработчиков IScanerAction
          ACTIONTYPE {NONE,
                TRANSFER = построчное копирование из из БД источника в БД приемника с проверкой на задвойки по DocId
                VECTORDOWNLOAD = выгрузка хэш-векторов изображений в файлы  
                VECTORBUILD = построение вектора изображения и сохранение его в БД
                WORDBUILDER = распознавание текста на картинке и сохранение в БД
                PARSING = парсинг внешних сайтов и формирование новых записей в БД
                }
    Реализация:
        Формируется и запускается поток IFillTaskQProcess считывающий постранично БД и заполняющий очередь ScannerQ
        Формируются и запускаются несколько потоков IScanerProcess. Каждый поток IScanerProcess считывает из ScannerQ строку, и обрабатывает ее одним 
        из ACTIONTYPE обработчиков.
        Формируется и запускается поток IPrintProcess логирующий статистику накапливаюмую во время работы IScanerProcess

        IFillTaskQProcess и IScanerProcess синхронизируются между собой и работают до полного прохода коллекции в БД 

    Особенности:   
       Приложение реализовано с использованием шаблона проектирования  Dependency Injection
          

=======================================================================================================================================================*/
using Microsoft.Extensions.DependencyInjection;
using ItZnak.Infrastruction.Services;
using DbScanner.Process.Infrastruction;
using ItZnak.PatentsDtoLibrary.Services;

namespace DbScanner
{
    internal static class Program
    {
        /* GLOBAL ACCES TO SERVICESS */
        static Func<Type, object> GetService;

        private static void Main(string[] args)
        {

            /* RUN DIALOG FOR SELECT OPERATION AND CALL INIT HOST */
            InitHost(RunInputActionTypeDialog());

            /* LAUNCH PRINT PROCESS */
            var taskPrintProcess = Task.Run(() =>
            {
                var srv = GetService(typeof(IPrintProcess)) as IPrintProcess;
                srv.Run(SingletonProcessInfrastraction.Itstance.CTS.Token);
            });

            /* LAUNCH READING DB AND FILLING Q */
            var taskDbReaderProcess = Task.Run(() =>
            {
                var srv = GetService(typeof(IFillTaskQProcess)) as IFillTaskQProcess;
                srv.Run(SingletonProcessInfrastraction.Itstance.CTS.Token);
            });

            List<Task> taskList = new() { taskPrintProcess, taskDbReaderProcess };

            /* LAUNCH JOB PROCESS */
            taskList.AddRange(BuildProcessTaskList());
            Task.WaitAll(taskList.ToArray());
        }

        /* CREATE LIST OF WORKERS FOR READ AND PROCESS QUEQ */
        static private List<Task> BuildProcessTaskList()
        {
            List<Task> rslt = new();
            var cnf = GetService(typeof(IConfigService)) as IConfigService;
            var prc = GetService(typeof(ITaskScanerProcess)) as ITaskScanerProcess;
            int lngt = cnf.GetInt("processWorkersCount");
            for (int i = 0; i < lngt; i++)
            {
                rslt.Add(Task.Run(() => prc.Run(SingletonProcessInfrastraction.Itstance.CTS.Token)));
            }
            return rslt;
        }

        /* INITIATE DI CONTAINER  */
        static void InitHost(ACTIONTYPE action)
        {
            IServiceCollection services = new ServiceCollection();
            IConfigService _configService = new ConfigService();
            var redisConfig=_configService.GetObject<RedisCacheConfig>(RedisCacheConfig.rootName);
            services.AddSingleton<ILogService, SerilogService>();
            services.AddSingleton(_configService);
            services.AddSingleton<IDistributeCache>(new RedisCache(services,redisConfig));
            services.AddSingleton<IPrintProcess, PrintProcess>();
            services.AddTransient<IMongoDbContextService, MongoDbContextService>();
            services.AddTransient<ITaskScanerProcess, TaskScanerProcess>();

            /*ADD  IScanerAction TO SERVICES  */
            ActionsMap.Map[action](services);

            /*ADD  IFillTaskQProcess TO SERVICES  */
            FillQTaskMap.Map[action](services);

            /*BUILD SERVICE PROVIDER   */
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            /*CALL THE INIT ACTION FOR IFillTaskQProcess. IT CAN BE CONSOLE INPUT OR NOTHING */
            IFillTaskQProcess scanerProcess = serviceProvider.GetService<IFillTaskQProcess>();
            scanerProcess.Init();

            /*ADD CNTL+C EVENT  */
            Console.CancelKeyPress += new ConsoleCancelEventHandler((object s, ConsoleCancelEventArgs a) => SingletonProcessInfrastraction.Itstance.CTS.Cancel());

            /*CLOUSERE FOR GLOBAL  ACCESS TO SERVICES */
            GetService = (t) => serviceProvider.GetService(t);
        }

        /* RUN DIALOG FOR SELECT OPERATION  */
        static ACTIONTYPE RunInputActionTypeDialog()
        {
            var srv = new ConfigService();
            ACTIONTYPE rslt = ACTIONTYPE.NONE;

            Action inputAction = () =>
            {
                int n = 0;
                var toActionName = (ACTIONTYPE at) => { n++; return $"{n} {at}"; };
                Console.WriteLine(DateTime.Now.ToString());
                Console.WriteLine("INPUT ACTION TYPE NUMBER");
                Console.WriteLine(string.Join("|", ActionsMap.Map.Select(d => toActionName(d.Key)).ToArray()));
                int idx = -1;
                bool isInt = Int32.TryParse(Console.ReadLine(), out idx);
                if (isInt && idx - 1 < ActionsMap.Map.ToArray().Length && idx > -1)
                {
                    rslt = ActionsMap.Map.ElementAt(idx - 1).Key;
                    return;
                }

                RunInputActionTypeDialog();
            };

            inputAction();
            return rslt;
        }
    }
}