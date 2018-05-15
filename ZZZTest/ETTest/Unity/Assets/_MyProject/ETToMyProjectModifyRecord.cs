/*
 * 菊花
 */

namespace ETModel
{
    /// <summary>
    /// 用于记录所有对ET框架进行的修改
    /// 便于更新ET框架
    /// </summary>
    public class ETToMyProjectModifyRecord
    {
        /*
         * Assets下新建自己工程的文件夹---自己工程“所有”的文件放这个里面
         * Bundles\UI下新建自己工程的文件夹---存放Prefab(类似Resources的作用)
         * * 需要动态加载的声音、图片等资源 先分类好 再做Prefab通过ReferenceCollector组件引用
         * Resources\KV 修改UILoading引用至自己工程里面的UILoading
         * ETModel----UIComponent修改成UIManager--Init.cs文件中不再使用UIComponent
         * ETHotfix---UIComponent修改成UIManager--Init.cs文件中不再使用UIComponent
         * 修改 LoadingFinishEvent_RemoveLoadingUI 脚本的Run方法
         */
    }
}