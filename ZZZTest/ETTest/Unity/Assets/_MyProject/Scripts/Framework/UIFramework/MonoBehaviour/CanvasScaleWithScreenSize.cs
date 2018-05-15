/***************菊花***************/

using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasScaleWithScreenSize: MonoBehaviour
    {
        private void Awake()
        {
            CanvasScaler canvasScaler = gameObject.GetComponent<CanvasScaler>();
            float screenWidthScale = Screen.width / canvasScaler.referenceResolution.x;
            float screenHeightScale = Screen.height / canvasScaler.referenceResolution.y;
            canvasScaler.matchWidthOrHeight = screenWidthScale > screenHeightScale? 1 : 0;
        }
    }
}