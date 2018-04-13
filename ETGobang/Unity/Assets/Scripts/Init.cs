using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
    public class Init : MonoBehaviour
    {
        private readonly OneThreadSynchronizationContext contex = new OneThreadSynchronizationContext();

        private async void Start()
        {
            try
            {
                Log.Warning($"当前版本:{Application.unityVersion}");

                SynchronizationContext.SetSynchronizationContext(this.contex);

                DontDestroyOnLoad(gameObject);

                Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

                Game.Scene.AddComponent<GlobalConfigComponent>();
                Game.Scene.AddComponent<NetOuterComponent>();
                Game.Scene.AddComponent<ResourcesComponent>();
                Game.Scene.AddComponent<BehaviorTreeComponent>();
                Game.Scene.AddComponent<PlayerComponent>();
                Game.Scene.AddComponent<UnitComponent>();
                Game.Scene.AddComponent<ClientFrameComponent>();
                Game.Scene.AddComponent<UIComponent>();

                await BundleHelper.DownloadBundle();

                Game.Hotfix.LoadHotfixAssembly();

                Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
                Game.Scene.AddComponent<ConfigComponent>();
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatherComponent>();

                Game.Hotfix.GotoHotfix();

                Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void Update()
        {
            this.contex.Update();
            Game.Hotfix.Update?.Invoke();
            Game.EventSystem.Update();
        }

        private void LateUpdate()
        {
            Game.Hotfix.LateUpdate?.Invoke();
            Game.EventSystem.LateUpdate();
        }

        private void OnApplicationQuit()
        {
            Game.Hotfix.OnApplicationQuit?.Invoke();
            Game.Close();
        }
    }
}