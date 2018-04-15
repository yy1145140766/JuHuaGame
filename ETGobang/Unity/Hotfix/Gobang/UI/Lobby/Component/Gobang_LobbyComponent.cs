using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class Gobang_LobbyComponentAwakeSystem : AwakeSystem<Gobang_LobbyComponent>
    {
        public override void Awake(Gobang_LobbyComponent self)
        {
            self.Awake();
        }
    }
    public class Gobang_LobbyComponent : Component
    {
        private Text notice;
        internal void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            this.notice = rc.Get<GameObject>("Notice").GetComponent<Text>();

            notice.text = "请点击匹配！";

            Button button = rc.Get<GameObject>("Match").GetComponent<Button>();
            button.onClick.Add(delegate { Log.Debug("测试点击事件的移除"); });
            button.onClick.RemoveAllListeners();
            button.onClick.Add(delegate
            {
                notice.text = "匹配中...";

                Gobang_User user = new Gobang_User
                {
                    Account = "2133442342",
                    WinNumber = 0,
                    LoseNumber = 0,
                    TieNumber = 0
                };
                Gobang_DataComponent.Instance.GameInfo.Competitor = user;
                user = new Gobang_User
                {
                    Account = "1234567890",
                    WinNumber = 10,
                    LoseNumber = 0,
                    TieNumber = 0
                };
                Gobang_DataComponent.Instance.GameInfo.Self = user;

                Game.Scene.GetComponent<UIComponent>().Create(UIType.Gobang_UI_Game);
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.Gobang_UI_Lobby);
            });
        }
    }
}
