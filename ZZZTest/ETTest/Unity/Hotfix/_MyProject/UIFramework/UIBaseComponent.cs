/*
 * 菊花
 */

namespace ETHotfix
{
    public abstract class UIBaseComponent: Component
    {
        private UI ui;

        public UI UI
        {
            get
            {
                if (this.ui == null)
                {
                    ui = GetParent<UI>();
                }

                return ui;
            }
        }

        protected virtual void OnEnter(object userData, bool showTransitonAnimation = true)
        {
            Log.Debug(showTransitonAnimation? $"{this.UI.Name}需要显示Enter过渡动画" : $"{this.UI.Name}不需要显示Enter过渡动画");
        }

        protected virtual void OnExit(bool showTransitonAnimation = true)
        {
            Log.Debug(showTransitonAnimation? $"{this.UI.Name}需要显示Exit过渡动画" : $"{this.UI.Name}不需要显示Exit过渡动画");
        }

        protected virtual void OnPause()
        {
            Log.Debug($"{this.UI.Name}界面暂停显示");
        }

        protected virtual void OnResume()
        {
            Log.Debug($"{this.UI.Name}界面恢复显示");
        }

        public void Enter(object userData)
        {
            OnEnter(userData);
        }

        public void Exit()
        {
            OnExit();
        }

        public void Pause()
        {
            OnPause();
        }

        public void Resume()
        {
            OnResume();
        }
    }
}