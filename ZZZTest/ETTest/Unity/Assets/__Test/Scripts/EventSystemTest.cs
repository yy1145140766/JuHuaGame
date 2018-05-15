/*
 * 菊花
 */

namespace ETModel
{
    [ObjectSystem]
    public class EventSystemTestAwakeSystem : AwakeSystem<EventSystemTest>
    {
        public override void Awake(EventSystemTest self)
        {
            self.Awake();
        }
    }
    [ObjectSystem]
    public class EventSystemTestStartSystem : StartSystem<EventSystemTest>
    {
        public override void Start(EventSystemTest self)
        {
            self.Start();
        }
    }

    [ObjectSystem]
    public class EventSystemTestLoadSystem : LoadSystem<EventSystemTest>
    {
        public override void Load(EventSystemTest self)
        {
            self.Load();
        }
    }

    [ObjectSystem]
    public class EventSystemTestDestroySystem : DestroySystem<EventSystemTest>
    {
        public override void Destroy(EventSystemTest self)
        {
            self.Destroy();
        }
    }

    //[ObjectSystem]
    //public class EventSystemTestChangeSystem : ChangeSystem<EventSystemTest>
    //{
    //    public override void Change(EventSystemTest self)
    //    {
    //        self.Change();
    //    }
    //}

    [ObjectSystem]
    public class EventSystemTestUpdateSystem : UpdateSystem<EventSystemTest>
    {
        public override void Update(EventSystemTest self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class EventSystemTestLateUpdateSystem : LateUpdateSystem<EventSystemTest>
    {
        public override void LateUpdate(EventSystemTest self)
        {
            self.LateUpdate();
        }
    }
    public class EventSystemTest : Component
    {
        internal void Awake()
        {
            Log.Debug("Awake");
        }

        internal void Change()
        {
            Log.Debug("Change");
        }

        internal void Destroy()
        {
            Log.Debug("Destroy");
        }

        internal void LateUpdate()
        {
            Log.Warning("LateUpdate");
        }

        internal void Load()
        {
            Log.Debug("Load");
        }

        internal void Start()
        {
            Log.Debug("Start");
        }

        internal void Update()
        {
            Log.Warning("Update");
        }
    }
}