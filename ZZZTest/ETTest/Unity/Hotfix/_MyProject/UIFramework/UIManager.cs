/*
 * 菊花
 */

using System;
using System.Collections.Generic;
using System.Linq;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [ObjectSystem]
    public class UIManagerAwakeSystem: AwakeSystem<UIManager>
    {
        public override void Awake(UIManager self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class UIManagerLoadSystem: LoadSystem<UIManager>
    {
        public override void Load(UIManager self)
        {
            self.Load();
        }
    }

    /// <summary>
    /// 控制所有的UI
    /// </summary>
    public class UIManager: Component
    {
        public static UIManager Instance;

        /// <summary>
        /// 用栈的形式管理所有实例面板对象
        /// </summary>
        private Stack<UIBaseComponent> stackUiBaseComponent;

        /// <summary>
        /// 当前显示在栈顶的界面
        /// </summary>
        private UIBaseComponent currentUiBaseComponent;

        /// <summary>
        /// 已经加载过的UI
        /// </summary>
        private readonly Dictionary<string, UIBaseComponent> loadedUiBaseComponent = new Dictionary<string, UIBaseComponent>();

        /// <summary>
        /// Canvas UI父节点
        /// </summary>
        private readonly Dictionary<UICanvasType, Transform> parentCanvas = new Dictionary<UICanvasType, Transform>();

        /// <summary>
        /// 加载UI
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userData"></param>
        public void PushStack<K>(string type, object userData) where K : UIBaseComponent
        {
            try
            {
                if (this.stackUiBaseComponent == null)
                {
                    this.stackUiBaseComponent = new Stack<UIBaseComponent>();
                }

                if (this.currentUiBaseComponent != null)
                {
                    this.currentUiBaseComponent.Pause();
                }

                UIBaseComponent uiBaseComponent;
                if (!loadedUiBaseComponent.TryGetValue(type, out uiBaseComponent))
                {
                    uiBaseComponent = Create<K>(type);
                }

                uiBaseComponent.Enter(userData);
                this.currentUiBaseComponent = uiBaseComponent;
            }
            catch (Exception e)
            {
                throw new Exception($"PushStack {type} UI 错误: {e}");
            }
        }

        /// <summary>
        /// 移除UI
        /// </summary>
        public void PopStack()
        {
            try
            {
                if (this.stackUiBaseComponent == null)
                {
                    this.stackUiBaseComponent = new Stack<UIBaseComponent>();
                    Log.Error("还没有PushStack 就使用PopStack");
                }

                if (this.stackUiBaseComponent.Count < 1)
                {
                    Log.Error("目前加载的UI个数小于1 请先PushStack 并检查PopStack的使用");
                    return;
                }

                this.currentUiBaseComponent.Exit();
                UIBaseComponent uiBaseComponent = this.stackUiBaseComponent.Pop();
                uiBaseComponent.Resume();
                this.currentUiBaseComponent = uiBaseComponent;
            }
            catch (Exception e)
            {
                throw new Exception($"PopStack 错误: {e}");
            }
        }

        /// <summary>
        /// 关闭所有的UI显示--一般用于场景切换
        /// </summary>
        public void CloseAll()
        {
        }

        #region UIComponent

        private GameObject Root;
        private readonly Dictionary<string, IUIFactory> UiTypes = new Dictionary<string, IUIFactory>();

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            foreach (string type in loadedUiBaseComponent.Keys.ToArray())
            {
                UIBaseComponent uiBaseComponent;
                if (!loadedUiBaseComponent.TryGetValue(type, out uiBaseComponent))
                {
                    continue;
                }

                loadedUiBaseComponent.Remove(type);
                uiBaseComponent.UI.Dispose();
            }

            this.UiTypes.Clear();
            this.loadedUiBaseComponent.Clear();
        }

        public void Awake()
        {
            Instance = this;

            this.Root = GameObject.Find("Global/UI/");
            parentCanvas.Add(UICanvasType.Bottom, this.Root.transform.Find("BottomCanvas"));
            parentCanvas.Add(UICanvasType.Middle, this.Root.transform.Find("MiddleCanvas"));
            parentCanvas.Add(UICanvasType.Top, this.Root.transform.Find("TopCanvas"));
            parentCanvas.Add(UICanvasType.TopMost, this.Root.transform.Find("TopMostCanvas"));

            this.Load();
        }

        public void Load()
        {
            UiTypes.Clear();

            Type[] types = ETModel.Game.Hotfix.GetHotfixTypes();

            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(UIFactoryAttribute), false);
                if (attrs.Length == 0)
                {
                    continue;
                }

                UIFactoryAttribute attribute = attrs[0] as UIFactoryAttribute;
                if (UiTypes.ContainsKey(attribute.Type))
                {
                    Log.Debug($"已经存在同类UI Factory: {attribute.Type}");
                    throw new Exception($"已经存在同类UI Factory: {attribute.Type}");
                }

                object o = Activator.CreateInstance(type);
                IUIFactory factory = o as IUIFactory;
                if (factory == null)
                {
                    Log.Error($"{o.GetType().FullName} 没有继承 IUIFactory");
                    continue;
                }

                this.UiTypes.Add(attribute.Type, factory);
            }
        }

        private UIBaseComponent Create<K>(string type) where K : UIBaseComponent
        {
            try
            {
                UI ui = UiTypes[type].Create(GetParent<Scene>(), type, Root);
                UICanvasType cavasType = ui.GameObject.GetComponent<UICanvasConfig>().CanvasType;
                ui.GameObject.transform.SetParent(parentCanvas[cavasType], false);
                UIBaseComponent uiBaseComponent = ui.GetComponent<K>();
                if (uiBaseComponent == null)
                {
                    Log.Error($"{type}界面的工厂类没有挂继承 UIBaseComponent 组件的脚本");
                }

                this.loadedUiBaseComponent.Add(type, uiBaseComponent);
                return uiBaseComponent;
            }
            catch (Exception e)
            {
                throw new Exception($"{type} UI 错误: {e}");
            }
        }

        private void Remove(string type)
        {
            UIBaseComponent uiBaseComponent;
            if (!this.loadedUiBaseComponent.TryGetValue(type, out uiBaseComponent))
            {
                return;
            }

            UiTypes[type].Remove(type);
            uiBaseComponent.UI.Dispose();
        }

        #endregion
    }
}