using ETModel;

namespace ETHotfix
{
    [Config(AppType.Client)]
    public class Gobang_ClientConfigCategory : ACategory<Gobang_ClientConfig>
    {

    }
    public class Gobang_ClientConfig : IConfig
    {
        public long Id { get; set; }

        public string AccountRecord;
        public string PasswordRecord;
    }
}
