using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    
    internal class Panel : DrawObject
    {
        string words;
       public Panel(int x,int y,string words)
        {
            position.x = x;
            position.y = y;
            this.words = words;
        }

        public override void Draw()
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(words);
        }
    }
}
