using System;

namespace ETModel
{
    /// <summary>
    /// 消息类管理接口
    /// </summary>
    public interface IMsgManager
    {
        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="msgName">名字</param>
        /// <param name="callback">注册事件</param>
        void On(string msgName, Action<Message> callback);

        /// <summary>
        /// 注销消息
        /// </summary>
        /// <param name="msgName">名字</param>
        /// <param name="callback">注销事件</param>
        void Off(string msgName, Action<Message> callback);

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgName">名字</param>
        /// <param name="message">内容</param>
        void Emit(Message message);
    }
}