using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class UiLobbyComponentSystem : AwakeSystem<UILobbyComponent>
    {
        public override void Awake(UILobbyComponent self)
        {
            self.Awake();
        }
    }

    public class UILobbyComponent : Component
    {
        private Text text;

        public void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            this.text = rc.Get<GameObject>("Text").GetComponent<Text>();

            text.text = "这里是大厅，但被我代码改了";
            text.color = Color.red;
        }

        private void OnSend()
        {
            // 发送一个actor消息
            SessionComponent.Instance.Session.Send(new Actor_Test() { Info = "message client->gate->map->gate->client" });
        }

        private async void OnSendRpc()
        {
            try
            {
                // 向actor发起一次rpc调用
                Actor_TestResponse response = (Actor_TestResponse)await SessionComponent.Instance.Session.Call(new Actor_TestRequest() { request = "request actor test rpc" });
                Log.Info($"recv response: {JsonHelper.ToJson(response)}");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private async void OnTransfer1()
        {
            try
            {
                Actor_TransferResponse response = (Actor_TransferResponse)await SessionComponent.Instance.Session.Call(new Actor_TransferRequest() { MapIndex = 0 });
                Log.Info($"传送成功! {JsonHelper.ToJson(response)}");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private async void OnTransfer2()
        {
            Actor_TransferResponse response = (Actor_TransferResponse)await SessionComponent.Instance.Session.Call(new Actor_TransferRequest() { MapIndex = 1 });
            Log.Info($"传送成功! {JsonHelper.ToJson(response)}");
        }

        private async void EnterMap()
        {
            try
            {
                G2C_EnterMap g2CEnterMap = (G2C_EnterMap)await SessionComponent.Instance.Session.Call(new C2G_EnterMap());
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILobby);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
