using System;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
    [ObjectSystem]
    public class Gobang_GameComponentAwakeSystem : AwakeSystem<Gobang_GameComponent>
    {
        public override void Awake(Gobang_GameComponent self)
        {
            self.Awake();
        }
    }
    public class Gobang_GameComponent : Component
    {
        private Button[,] pieceBtns = new Button[13, 13];
        private Vector3[,] piecePoss = new Vector3[13, 13];
        private GameObject competitor;
        private GameObject self;

        internal void Awake()
        {
            ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            competitor = rc.Get<GameObject>("Competitor").gameObject;
            Gobang_User user = Gobang_DataComponent.Instance.GameInfo.Competitor;
            competitor.transform.Find("ID").GetComponent<Text>().text = user.Account;
            competitor.transform.Find("Record").GetComponent<Text>().text =
                $"{user.WinNumber}胜 {user.LoseNumber}败 {user.TieNumber}平局";

            self = rc.Get<GameObject>("Self").gameObject;
            user = Gobang_DataComponent.Instance.GameInfo.Self;
            self.transform.Find("ID").GetComponent<Text>().text = user.Account;
            self.transform.Find("Record").GetComponent<Text>().text =
                $"{user.WinNumber}胜 {user.LoseNumber}败 {user.TieNumber}平局";

            Button btn = rc.Get<GameObject>("Quit").GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.Add(OnQuit);

            GameObject go = rc.Get<GameObject>("PieceBtns").gameObject;
            for (int i = 0; i < 13; i++)
            {
                //前7行 蛇形取  后6行 从左往右取
                if (i < 7 && i % 2 == 1)
                {
                    //双
                    for (int j = 0; j < 13; j++)
                    {
                        pieceBtns[i, j] = go.transform.Find("1-1 (" + (((i + 1) * 13) - 1 - j) + ")").GetComponent<Button>();
                    }
                }
                else
                {
                    //单
                    for (int j = 0; j < 13; j++)
                    {
                        pieceBtns[i, j] = go.transform.Find("1-1 (" + (i * 13 + j) + ")").GetComponent<Button>();
                    }
                }
            }
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    OnPieceBtnClick(pieceBtns[i, j]);

                    piecePoss[i, j] = pieceBtns[i, j].gameObject.transform.position;
                }
            }
        }

        private void OnPieceBtnClick(Button button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.Add(delegate
            {
                for (int i = 0; i < 13; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        if (button == pieceBtns[i, j])
                        {
                            Log.Debug("发送位置 " + i + " - " + j);
                            return;
                        }
                    }
                }
            });
        }

        private void OnQuit()
        {
            Log.Debug("发送退出");
        }
    }
}
