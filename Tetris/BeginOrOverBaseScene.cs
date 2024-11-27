using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// 选项类型
    /// </summary>
    enum EChoice
    {
        /// <summary>
        /// 首选项
        /// </summary>
        Start,
        /// <summary>
        /// 结束游戏选项
        /// </summary>
        Over,

        /// <summary>
        /// 空选项
        /// </summary>
        None
    }
    internal abstract class BeginOrOverBaseScene : ISceneUpdate
    {
        protected EChoice choice = EChoice.None;//初始化选项类型为空
        protected string title;//主标题
        protected string choiceName;//首选项名称

        public abstract void EnterJ();//用于确定选择的选项
        public void Update()
        {
            //标题及选项字体设置
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Game.width / 2 - title.Length, 8);
            Console.Write(title);
            Console.ForegroundColor = choice == EChoice.Start ? ConsoleColor.Red : ConsoleColor.White;
            Console.SetCursorPosition(Game.width / 2 - choiceName.Length, 13);
            Console.Write(choiceName);
            Console.ForegroundColor = choice == EChoice.Over ? ConsoleColor.Red : ConsoleColor.White;
            Console.SetCursorPosition(Game.width / 2 - 4, 15);
            Console.Write("结束游戏");


            //选项选择，并改变字体颜色
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    choice = EChoice.Start;
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    choice = EChoice.Over;
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.J:
                    EnterJ();
                    break;
                default:
                    choice = EChoice.None;
                    break;
            }
        }
    }
}

