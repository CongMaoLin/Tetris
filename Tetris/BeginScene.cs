using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;


namespace Tetris
{
    internal class BeginScene : BeginOrOverBaseScene
    {
        SoundPlayer MenuBGM;
        public BeginScene() 
        { 
            title = "俄罗斯方块"; 
            choiceName = "开始游戏"; 
            BlockWorker.mainGameThreadStop = true; //静态变量不会随着实例化初始化
            MenuBGM = new SoundPlayer();
            MenuBGM.SoundLocation = @"F:\Git\repositary\MyTetris\Menu.WAV";
            MenuBGM.PlayLooping();
        }
        public override void EnterJ()
        {
            if (choice == EChoice.Start)
            {
                MenuBGM.Stop();
                Game.ChangeScene(E_SceneType.Game);
            }
            else if (choice == EChoice.Over)
            {
                Environment.Exit(0);
            }
        }
    }
}
