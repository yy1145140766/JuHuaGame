/*
 * 菊花
 */

using UnityEngine;

namespace ETModel
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public class UICanvasConfig: MonoBehaviour
    {
        /// <summary>
        /// 该UI从属于哪个Canvas下面
        /// </summary>
        public UICanvasType CanvasType = UICanvasType.Middle;
    }

    /// <summary>
    /// 分为四个层面的Canvas
    /// </summary>
    public enum UICanvasType
    {
        /// <summary>
        /// 最底层的UI  比如大厅背景  游戏底层背景------------Order in Layer 10
        /// </summary>
        Bottom,

        /// <summary>
        /// 中间层  大部分UI集中于此--------------------------Order in Layer 20
        /// </summary>
        Middle,

        /// <summary>
        /// 上层  弹窗之类的UI--------------------------------Order in Layer 50
        /// </summary>
        Top,

        /// <summary>
        /// 最上层  过渡界面   提示框   遮罩   屏蔽输入等UI---Order in Layer 100
        /// </summary>
        TopMost
    }
}