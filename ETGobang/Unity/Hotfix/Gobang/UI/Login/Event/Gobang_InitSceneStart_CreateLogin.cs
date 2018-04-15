using ETModel;

namespace ETHotfix
{
    [Event(EventIdType.Gobang_InitStart)]
    public class Gobang_InitSceneStart_CreateLogin : AEvent
    {
        public override void Run()
        {
            UI ui = Game.Scene.GetComponent<UIComponent>().Create(UIType.Gobang_UI_Login);
        }
    }
}
