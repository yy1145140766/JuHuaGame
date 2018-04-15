using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class Gobang_LoginComponentAwakeSystem : AwakeSystem<Gobang_LoginComponent>
    {
        public override void Awake(Gobang_LoginComponent self)
        {
            self.Awake();
        }
    }
    public class Gobang_LoginComponent : Component
    {
        private InputField input_Account;
        private InputField input_Password;
        private Button btn_Longin;
        private Button btn_Register;
        private Text text_Notice;
        internal void Awake()
        {
            InitGameObject();
            InitEvent();
        }

        private void InitEvent()
        {
            btn_Longin.onClick.RemoveAllListeners();
            btn_Longin.onClick.Add(OnLogin);
            btn_Register.onClick.RemoveAllListeners();
            btn_Register.onClick.Add(OnRegister);
        }
        private void InitGameObject()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            input_Account = rc.Get<GameObject>("Account").GetComponent<InputField>();
            input_Account.text = Gobang_DataComponent.Instance.ClientConfig.AccountRecord;
            input_Password = rc.Get<GameObject>("Password").GetComponent<InputField>();
            input_Password.text = Gobang_DataComponent.Instance.ClientConfig.PasswordRecord;
            btn_Longin = rc.Get<GameObject>("LoginBtn").GetComponent<Button>();
            btn_Register = rc.Get<GameObject>("RegisterBtn").GetComponent<Button>();
            text_Notice = rc.Get<GameObject>("Notice").GetComponent<Text>();
        }

        private void OnRegister()
        {
            if (!CheckString())
            {
                return;
            }
        }

        private async void OnLogin()
        {
            if (!CheckString())
            {
                return;
            }
            SessionWrap sessionWrap = null;
            try
            {
                IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);

                Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
                sessionWrap = new SessionWrap(session);
                R2C_Login r2CLogin = (R2C_Login)await sessionWrap.Call(new C2R_Login() { Account = input_Account.text, Password = input_Password.text });
                sessionWrap.Dispose();

                connetEndPoint = NetworkHelper.ToIPEndPoint(r2CLogin.Address);
                Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
                Game.Scene.AddComponent<SessionWrapComponent>().Session = new SessionWrap(gateSession);
                ETModel.Game.Scene.AddComponent<SessionComponent>().Session = gateSession;
                G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionWrapComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key });

                Log.Info("登陆gate成功!");

                Game.Scene.GetComponent<UIComponent>().Create(UIType.Gobang_UI_Lobby);
                Game.Scene.GetComponent<UIComponent>().Remove(UIType.Gobang_UI_Login);
            }
            catch (Exception e)
            {
                sessionWrap?.Dispose();
                Log.Error(e);
            }
        }

        private bool CheckString()
        {
            if (input_Account.text.Length < 6 || input_Account.text.Length > 16)
            {
                text_Notice.text = "账号为6~16位数字";
                return false;
            }
            if (input_Password.text.Length < 6 || input_Password.text.Length > 16)
            {
                text_Notice.text = "密码长度为6~16位";
                return false;
            }
            return true;
        }
    }
}
