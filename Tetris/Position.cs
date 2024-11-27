using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    //用来存储位置，重写操作符用于判断位置是否相等
    internal struct Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static bool operator ==(Position left, Position right)
        { if(left.x == right.x && left.y == right.y) 
            {  
                return true; 
            }
            else
            {
                return false;
            }
        }
                                        
        
        public static bool operator !=(Position left, Position right)
        {
            if (left.x == right.x && left.y == right.y)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }


        //用来移动方块
        public static Position operator +(Position left, Position right)
        {
            Position position = new Position(left.x+right.x,left.y+right.y);
            return position;
        }
    }
}
