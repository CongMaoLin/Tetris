using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
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
        EnterWall,
        ///<summary>
        ///文字
        ///</summary>
        Font,
        Buffer,
        showWall,


    }
    internal class DrawObject : IDraw,IClear
    {
        public Position position;
        public E_FigureType figureType;

        public DrawObject()
        {

        }
        public DrawObject(E_FigureType figureType)
        { 
            this.figureType = figureType;
        }
        public DrawObject(E_FigureType figureType,int x,int y) : this(figureType) 
        {
            position.x = x;
            position.y = y;
        }
        
        public void ChangeBlock(E_FigureType figuretype)
        {
            this.figureType = figuretype;
            
        }

        

        public virtual void Draw()
        {
            if (position.y < 0)
            {
                return;
            }
            
            Console.SetCursorPosition(position.x, position.y);
            switch (figureType)
            {
                case E_FigureType.EnterWall:
                case E_FigureType.Wall:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case E_FigureType.showWall:
                case E_FigureType.Buffer:
                case E_FigureType.Font:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            if (figureType == E_FigureType.EnterWall)
            {
                Console.Write("_");
            }
            else if(figureType == E_FigureType.Font)
            {
                Console.Write("Top");
            }
            else if (figureType == E_FigureType.Buffer)
            {
                Console.Write("缓冲区");
            }
            else { Console.Write("■"); }
        }

        public void Clear()
        {
            if (position.y < 0)
            {
                return;
            }
            Console.SetCursorPosition(position.x, position.y);
            Console.Write("  ");
        }
    }
}
