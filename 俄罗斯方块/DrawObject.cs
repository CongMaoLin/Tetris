using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块
{
    /// <summary>
    /// 图形类型
    /// </summary>
    enum E_FigureType
    {   /// <summary>
        /// 墙壁
        /// </summary>
        Wall,
        /// <summary>
        /// 正方块
        /// </summary>
        Cube =1,
        /// <summary>
        /// 长条块
        /// </summary>
        Line,
        /// <summary>
        /// 凸字形
        /// </summary>
        Tu,
        /// <summary>
        /// 左N形
        /// </summary>
        L_N,
        /// <summary>
        /// 右N形
        /// </summary>
        R_N,
        /// <summary>
        /// 左L形
        /// </summary>
        L_L,
        /// <summary>
        /// 右L形
        /// </summary>
        R_l,
        /// <summary>
        /// 顶部分界墙
        /// </summary>
        EnterWall


    }
    internal class DrawObject : IDraw,IClear
    {
        public Position positon;
        public E_FigureType figureType;

        public DrawObject(E_FigureType figureType)
        { 
            this.figureType = figureType;
        }
        public DrawObject(E_FigureType figureType,int x,int y) : this(figureType) 
        {
            positon.x = x;
            positon.y = y;
        }
        
        public void ChangeBlock(E_FigureType figuretype)
        {
            this.figureType = figuretype;
            
        }

        

        public void Draw()
        {
            Console.SetCursorPosition(positon.x, positon.y);
            switch (figureType)
            {
                case E_FigureType.EnterWall:
                case E_FigureType.Wall:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            if (figureType == E_FigureType.EnterWall)
            {
                Console.Write("_");
            }
            else { Console.Write("■"); }
        }

        public void Clear()
        {
            Console.SetCursorPosition(positon.x, positon.y);
            Console.Write("  ");
        }
    }
}
