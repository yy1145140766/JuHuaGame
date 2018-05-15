using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    /// <inheritdoc />
    /// <summary>
    /// 消息管理类（单例）
    /// </summary>
    public sealed class MsgManager: IMsgManager
    {
        private MsgManager()
        {
        }

        public static MsgManager Instance { get; } = new MsgManager();

        /// <summary>
        /// 储存所有消息与之对应的响应事件列表
        /// </summary>
        private Dictionary<string, List<Action<Message>>> messageDic;

        /// <summary>
        /// 初始化msgManager
        /// </summary>
        public void Init()
        {
            if (messageDic != null)
            {
                messageDic.Clear();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="msgName">名字</param>
        /// <param name="callback">注册事件</param>
        public void On(string msgName, Action<Message> callback)
        {
            if (messageDic == null)
            {
                messageDic = new Dictionary<string, List<Action<Message>>>();
            }

            List<Action<Message>> actions = null;
            if (messageDic.TryGetValue(msgName, out actions))
            {
                if (actions == null)
                {
                    actions = new List<Action<Message>> { callback };
                }
                else
                {
                    actions.Add(callback);
                }

                messageDic[msgName] = actions;
            }
            else
            {
                messageDic.Add(msgName, new List<Action<Message>> { callback });
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 注销消息
        /// </summary>
        /// <param name="msgName">名字</param>
        /// <param name="callback">注销事件</param>
        public void Off(string msgName, Action<Message> callback)
        {
            if (this.messageDic == null)
                return;
            List<Action<Message>> actions = null;
            if (this.messageDic.TryGetValue(msgName, out actions))
            {
                if (actions == null)
                    return;
                for (int i = 0; i < actions.Count; i++)
                {
                    if (actions[i] != callback)
                        continue;
                    actions.RemoveAt(i);
                    break;
                }
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">内容</param>
        public void Emit(Message message)
        {
            if (this.messageDic == null)
                return;
            List<Action<Message>> actions = null;
            if (messageDic.TryGetValue(message.Name, out actions))
            {
                if (actions == null)
                    return;
#if UNITY_EDITOR
                Debug.LogWarning(message.ToString());
#endif
                for (int i = 0; i < actions.Count; i++)
                {
#if UNITY_EDITOR
                    actions[i](message);
#else
                    try
                    {
                        actions[i](message);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"消息分发错误:{e}");
                    }
#endif
                }
            }
        }
    }
}