using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 俄罗斯方块
{
    internal class BlockWorker : IDraw
    {
        
        Map map;
        Random random = new Random();
        Block blocks;
        //第一个方块
        DrawObject firstBlock;
        //初始化方向的方块
        public DrawObject[] initBlock;
        //方块类型
        E_FigureType blockType;
        //记录方块方向索引
        int direction;

        //记录移动前方块位置
        Block recordBlocks;

        //记录游戏是否结束，用于关闭线程
        bool ifGameOver;

        //新方块随机横坐标
        int X;
        public BlockWorker(Map map)
        {
            //新方块随机横坐标
            X = random.Next(8, Game.width - 20);
            //随机初始化一种方块
            blockType = (E_FigureType)random.Next(1, 8);
            firstBlock = new DrawObject(blockType,22, -3);
            blocks = new Block(blockType, firstBlock);
            //随机初始化一个方块方向
            direction = random.Next(0, 4);
            initBlock = blocks[direction];
            this.map = map;

           


        }

        public void Draw()
        {
            for (int i = 0; i < 4; i++)
            {
                initBlock[i].Draw();
            }
        }
        void ClearDraw()
        {
            for (int i = 0; i < 4; i++)
            {
                initBlock[i].Clear();
            }
        }
         
       //用来判断能不能转向
         bool IfCanChange(int direction)
        {
            
            bool ifCan = true;
            foreach(DrawObject wall in map.Walls)
            {
                foreach(DrawObject block in blocks[direction])
                {
                    if (wall.position == block.position)
                        { ifCan = false; break; }
                    
                }
                
            }
            foreach(DrawObject wall in map.D_Walls)
            {
                foreach (DrawObject block in blocks[direction])
                {
                    if (wall.position == block.position)
                    { ifCan = false; break; }
                }
            }
            return ifCan;
        }

        //用来方块转向
        public void ChangeDirection(ConsoleKey key)
        {

            switch (key)
            {
                case ConsoleKey.Spacebar:

                    direction++;
                    if (direction > 3)
                    {
                        direction = 0; 
                    }
                
                    break;

            }
            if (IfCanChange(direction))
            {
                ClearDraw();
                initBlock = blocks[direction];
                Draw();
            }
            else
            {
                direction--;
                if (direction < 0)
                {
                    direction = 3;
                }
            }
        }
        //判断方块是否触底
        bool IfReachBottom()
        {
            bool isReach = false;
            foreach (DrawObject wall in map.D_Walls)
            {
                foreach (DrawObject block in initBlock)
                {
                    if (wall.position == block.position)
                    {
                        isReach = true;
                        break;
                    }
                }

            }
            return isReach;
        }
        //判断能否左右移动
        bool IfCanMove()
        {
            
           
            bool ifCan = true;
            foreach (DrawObject wall in map.Walls)
            {
                foreach (DrawObject block in initBlock)
                {
                    if (wall.position == block.position)
                    { ifCan = false; break; }

                }

            }
            
            return ifCan;
        }

        //记录移动前方块的位置
        void CreateRecordBlocks()
        {
            DrawObject recordFirstBlock = new DrawObject(blocks[0][0].figureType, blocks[0][0].position.x, blocks[0][0].position.y); 
            recordBlocks = new Block(blocks[0][0].figureType, recordFirstBlock);
            

        }
        //移动方块
        public void Move(ConsoleKey key)
        {
            
            CreateRecordBlocks();
            ClearDraw();
            switch (key)
            {
                
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    Position pos = new Position(2, 0);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            blocks[i][j].position += pos;
                        }
                    }
                    
                    break;
                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    Position pos1 = new Position(-2, 0);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            blocks[i][j].position += pos1;
                        }
                    }
                    
                    break;
                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    Position pos2 = new Position(0, 1);
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            blocks[i][j].position += pos2;
                        }
                    }
                    
                    break;
            }
            if (IfCanMove())
            {
                
                initBlock = blocks[direction];
                if (IfReachBottom())
                {
                    initBlock = recordBlocks[direction];
                    blocks = recordBlocks;
                    ChangeToD_Walls(map, initBlock);
                    blockType = (E_FigureType)random.Next(1, 8);
                    X = random.Next(8, Game.width - 20);
                    firstBlock = new DrawObject(blockType, 22, -3);
                    blocks = new Block(blockType, firstBlock);
                    direction = random.Next(0, 4);
                    initBlock = blocks[direction];
                    map.IfRowsFull();
                    map.Draw();

                    ifGameOver = map.IfColumsFull();
                    if (ifGameOver)
                    {
                        GameScene.StopThread();
                    }
                    return;
                }
                Draw();
            }
            else
            {
                initBlock = recordBlocks[direction];
                blocks = recordBlocks;
                Draw();
            }
        }
            
        
        //方块自动向下移动
        public void AutoMove()
        {
            CreateRecordBlocks();
            ClearDraw();
            Position pos = new Position(0, 1);
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    blocks[i][j].position += pos;
                }
            }
            if (IfCanMove())
            {
                //可以动，就从新的blocks里选择原本方向的格子，然后绘制
                initBlock = blocks[direction];
                
                if (IfReachBottom())
                {
                    //到底了，就返回原来blocks，从里面选择一个方向的格子
                    initBlock = recordBlocks[direction];
                    blocks = recordBlocks;
                    //然后将blocks里面所有方向的格子变成动态墙
                    ChangeToD_Walls(map, initBlock);
                    //并重新初始化一个worker
                    blockType = (E_FigureType)random.Next(1, 8);
                    X = random.Next(8, Game.width - 20);
                    firstBlock = new DrawObject(blockType, 22, -3);
                    blocks = new Block(blockType, firstBlock);
                    direction = random.Next(0, 4);
                    initBlock = blocks[direction];

                    map.IfRowsFull();
                    
                    //先画一下地图，然后再画初始化的格子，不然地图会闪
                    map.Draw();

                    ifGameOver =   map.IfColumsFull();
                    if (ifGameOver)
                    {
                        GameScene.StopThread(); 
                    }
                    return;
                }
                Draw(); 
                
            }
            
            else
            {
                initBlock = recordBlocks[direction];
                blocks = recordBlocks;
                Draw();
            }
        }

        //方块到达底部后，将他变成墙
        public void ChangeToD_Walls(Map map, DrawObject[] initBlock  )
        {
            map.ChangeToD_Walls(map, initBlock);

        }
    }
}
