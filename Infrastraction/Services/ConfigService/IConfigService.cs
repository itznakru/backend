namespace ItZnak.Infrastruction.Services{
    /* сервис конфигурации приложения */
    public interface IConfigService
    {
        int GetInt (string field);
        bool GetBool (string field);
        string GetString(string field);
        T GetObject<T>(string filed);
    }
}
