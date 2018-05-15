namespace ETModel
{
    /// <summary>
    /// 传递的消息（不可继承）
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// 内容
        /// </summary>
        public object Body { get; }

        /// <inheritdoc />
        /// <summary>
        /// 传递的消息
        /// </summary>
        /// <param name="name">名字</param>
        public Message(string name): this(name, null, null)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// 传递的消息
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="body">内容</param>
        public Message(string name, object body): this(name, body, null)
        {
        }

        /// <summary>
        /// 传递的消息
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="body">内容</param>
        /// <param name="type">类型</param>
        public Message(string name, object body, string type)
        {
            Name = name;
            Body = body;
            Type = type;
        }

        /// <summary>
        /// 待定
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string msg = "内部分发消息：Name: " + Name;
            msg += "\nBody:" + (Body?.ToString() ?? "null");
            msg += "\nType:" + (Type ?? "null");
            return msg;
        }
    }
}