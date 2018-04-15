using System;
using ETModel;

namespace ETHotfix
{
    [ObjectSystem]
    public class Gobang_DataComponentAwakeSystem : AwakeSystem<Gobang_DataComponent>
    {
        public override void Awake(Gobang_DataComponent self)
        {
            self.Awake();
        }
    }
    public class Gobang_DataComponent : Component
    {
        public static Gobang_DataComponent Instance;

        public Gobang_ClientInfo ClientInfo;
        public Gobang_GameInfo GameInfo;
        public Gobang_UserInfo UserInfo;
        public Gobang_ClientConfig ClientConfig;
        internal void Awake()
        {
            Instance = this;
            ClientInfo = new Gobang_ClientInfo();
            GameInfo = new Gobang_GameInfo();
            UserInfo = new Gobang_UserInfo();
            ClientConfig = (Gobang_ClientConfig)Game.Scene.GetComponent<ConfigComponent>().Get(typeof(Gobang_ClientConfig), 1);
            InitGameInfo();
        }
        private void InitGameInfo()
        {
            GameInfo.pieces_Competitor = new bool[13, 13];
            GameInfo.pieces_Self = new bool[13, 13];
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    GameInfo.pieces_Competitor[i, j] = false;
                    GameInfo.pieces_Self[i, j] = false;
                }
            }
        }
    }
}
