using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class OverScene : BeginOrOverBaseScene
    {
        public OverScene()
        {
            
            title = "游戏结束"; 
            choiceName = "返回主页";
            
        }
        public override void EnterJ()
        {
            if (choice == EChoice.Start)
            {
                Game.ChangeScene(E_SceneType.Begin);
            }
            else if (choice == EChoice.Over)
            {
                Environment.Exit(0);
            }
        }
    }
}
