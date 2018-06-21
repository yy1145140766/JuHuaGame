using System;
using System.Collections.Generic;

namespace GameFramework.Base
{
    public static class GameFrameworkEntry
    {
        private static readonly LinkedList<GameFrameworkModule> SGameFrameworkModules = new LinkedList<GameFrameworkModule>();

        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (GameFrameworkModule module in SGameFrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理所有游戏框架模块。
        /// </summary>
        public static void Shutdown()
        {
            for (LinkedListNode<GameFrameworkModule> current = SGameFrameworkModules.Last; current != null; current = current.Previous)
            {
                current.Value.Shutdown();
            }

            SGameFrameworkModules.Clear();
            //ReferencePool.ClearAll();
            //Log.SetLogHelper(null);
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型。</typeparam>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new GameFrameworkException($"You must get module by interface, but '{interfaceType.FullName}' is not.");
            }

            if (interfaceType.FullName == null)
            {
                throw new GameFrameworkException($"You must get a Game Framework module, but '{interfaceType.FullName}' is null.");
            }

            if (!interfaceType.FullName.StartsWith("GameFramework."))
            {
                throw new GameFrameworkException($"You must get a Game Framework module, but '{interfaceType.FullName}' is not.");
            }

            string moduleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new GameFrameworkException($"Can not find Game Framework module type '{moduleName}'.");
            }

            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型。</param>
        /// <returns>要获取的游戏框架模块。</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块。</remarks>
        private static GameFrameworkModule GetModule(Type moduleType)
        {
            foreach (GameFrameworkModule module in SGameFrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }

            return CreateModule(moduleType);
        }

        /// <summary>
        /// 创建游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要创建的游戏框架模块类型。</param>
        /// <returns>要创建的游戏框架模块。</returns>
        private static GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule) Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException($"Can not create module '{module.GetType().FullName}'.");
            }

            LinkedListNode<GameFrameworkModule> current = SGameFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                SGameFrameworkModules.AddBefore(current, module);
            }
            else
            {
                SGameFrameworkModules.AddLast(module);
            }

            return module;
        }
    }
}