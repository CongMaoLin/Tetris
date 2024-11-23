using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块
{
    internal class BeginScene : BeginOrOverBaseScene
    {
        public BeginScene() { title = "俄罗斯方块";choiceName = "开始游戏"; }
        public override void EnterJ()
        {
            if(choice == EChoice.Start)
            {
                Game.ChangeScene(E_SceneType.Game);
            }
            else if(choice == EChoice.Over)
            {
                Environment.Exit(0);
            }
        }
    }
}
