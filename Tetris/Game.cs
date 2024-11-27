using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{   /// <summary>
    /// 场景类型
    /// </summary>
    enum E_SceneType
    {
        /// <summary>
        /// 开始场景
        /// </summary>
        Begin,
        /// <summary>
        /// 游戏场景
        /// </summary>
        Game,
        /// <summary>
        /// 结束场景
        /// </summary>
        Over
    }
   
    
    internal static class Game
    {
        public const int width = 60;
        public const int height = 36;
        public static ISceneUpdate scene;

        static Game()
        {
            ChangeScene(E_SceneType.Begin);
            Console.CursorVisible = false;
            Console.SetWindowSize(width,height);
            Console.SetBufferSize(width,height);    
        }
        public static void Start()
        {
            while (true)
            {
                if(scene != null)
                {
                     scene.Update();
                }
            }
        }

        public static  void ChangeScene(E_SceneType sceneType)
        {
            Console.Clear();
            switch (sceneType)
            {
                case E_SceneType.Begin:
                    scene = new BeginScene();
                    break;
                case E_SceneType.Game:
                    scene = new GameScene();
                    break;
                case E_SceneType.Over:
                    scene = new OverScene();
                    break;
                
            }



        }
    }
}
