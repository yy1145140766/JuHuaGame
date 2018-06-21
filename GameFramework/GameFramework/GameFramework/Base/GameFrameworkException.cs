﻿using System;
using System.Runtime.Serialization;

namespace GameFramework.Base
{
    /*
     * 异常类
     */
    [Serializable]
    public class GameFrameworkException : Exception
    {
        public GameFrameworkException()
        {
        }

        public GameFrameworkException(string message) : base(message)
        {
        }

        public GameFrameworkException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameFrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}