using System.Media;
using System.Threading;
using System;


namespace Tetris
{
    internal class BlockWorker : IDraw
    {
        //播放游戏结束
        SoundPlayer player = new SoundPlayer();
        //播放游戏背景音乐
        SoundPlayer BGM = new SoundPlayer();
        

        //游戏结束转场时，为了防止主线程在副线程之前打印地图，造成游戏结束画面和地图同时出现
        public static bool mainGameThreadStop = true;
        //避免副线程结束之前提前转场，和上一个变量配合使用
        public static bool ifGameOverToChangeScene = true;
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


        //下一个方块
        DrawObject showFirstBlock;
        Block showBlocks;
        public DrawObject[] showBlock;

        //记录下一个方块的形状
        int recordDirection;
        public BlockWorker(Map map)
        {

            //随机初始化一种方块
            blockType = (E_FigureType)random.Next(1, 8);
            firstBlock = new DrawObject(blockType, 22, -3);
            blocks = new Block(blockType, firstBlock);
            //随机初始化下一个方块
            blockType = (E_FigureType)random.Next(1, 8);
            showFirstBlock = new DrawObject(blockType, 22, -3);
            showBlocks = new Block(blockType, showFirstBlock);
            //随机初始化一个方块方向
            direction = random.Next(0, 4);
            initBlock = blocks[direction];
            //随机初始化下一个方块
            showBlock = showBlocks[recordDirection];

            this.map = map;
            inputShowBlockIntoMap();

            player.SoundLocation = @"F:\Git\repositary\MyTetris\GameOverSound.wav";
            
            BGM.SoundLocation = @"F:\Git\repositary\MyTetris\GameBGM.wav";
            BGM.PlayLooping();

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
            foreach (DrawObject wall in map.Walls)
            {
                foreach (DrawObject block in blocks[direction])
                {
                    if (wall.position == block.position)
                    { ifCan = false; break; }

                }

            }
            foreach (DrawObject wall in map.D_Walls)
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
            //判断动态墙往上移动一格能否触及到方块
            bool ifStack = false;

            bool isReach = false;
            foreach (DrawObject wall in map.D_Walls)
            {
                foreach (var item in recordBlocks[direction])
                {
                    if (wall.position.y - 1 == item.position.y && wall.position.x == item.position.x)
                    {
                        ifStack = true;
                        break;
                    }
                }
                foreach (DrawObject block in initBlock)
                {

                    if (wall.position == block.position && ifStack)
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


            bool ifWallsCan = true;

            foreach (DrawObject wall in map.Walls)
            {
                foreach (DrawObject block in initBlock)
                {
                    if (wall.position == block.position)
                    { ifWallsCan = false; break; }

                }
            }


            return ifWallsCan;
        }
        //是否左右移动碰到动态墙
        bool IfCrashDWalls()
        {



            bool ifD_WallsCan = false;

            foreach (var wall in map.D_Walls)
            {
                foreach (var block in initBlock)
                {
                    if (wall.position == block.position)
                    { ifD_WallsCan = true; break; }
                }
            }

            return ifD_WallsCan;
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
                ifGameOverToChangeScene = false;
                initBlock = blocks[direction];
                if (IfCrashDWalls())
                {
                    if (IfReachBottom())
                    {
                        initBlock = recordBlocks[direction];
                        blocks = recordBlocks;
                        ChangeToD_Walls(map, initBlock);

                        InitialBlocks();

                        map.IfRowsFull();
                        map.Draw();

                        ifGameOver = map.IfColumsFull();
                        if (ifGameOver)
                        {

                            mainGameThreadStop = false;
                            map.Draw();
                            //BGMPlayer;
                            player.Play();
                            Thread.Sleep(2000);
                            Console.Clear();
                            GameScene.StopThread();
                            ifGameOverToChangeScene = true;
                            map.IfColumsFull();

                        }
                        return;
                    }
                    else
                    {
                        initBlock = recordBlocks[direction];
                        blocks = recordBlocks;
                    }
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

                    //并重新初始化格子
                    InitialBlocks();

                    map.IfRowsFull();

                    //先画一下地图，然后再画初始化的格子，不然地图会闪
                    map.Draw();
                    ifGameOverToChangeScene = true;
                    ifGameOver = map.IfColumsFull();
                    if (ifGameOver)
                    {
                        BGM.Stop();
                        //mainGameThreadStop = false;
                        map.Draw();
                        player.Play();
                        Thread.Sleep(2000);
                        Console.Clear();

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
        public void ChangeToD_Walls(Map map, DrawObject[] initBlock)
        {
            map.ChangeToD_Walls(map, initBlock);

        }
        //初始化方块
        void InitialBlocks()
        {
            //blockType = (E_FigureType)random.Next(1, 8);
            //firstBlock = new DrawObject(blockType, 22, -3);
            //blocks = new Block(blockType, firstBlock);
            //direction = random.Next(0, 4);
            //initBlock = blocks[direction];


            foreach (var block in showBlock)
            {
                block.Clear();
            }

            firstBlock = new DrawObject(blockType, 22, -3);
            blocks = new Block(firstBlock.figureType, firstBlock);
            direction = recordDirection;
            initBlock = blocks[direction];


            blockType = (E_FigureType)random.Next(1, 8);
            showFirstBlock = new DrawObject(blockType, 22, -3);
            showBlocks = new Block(blockType, showFirstBlock);
            recordDirection = random.Next(0, 4);
            showBlock = showBlocks[recordDirection];

            inputShowBlockIntoMap();
        }
        //将下一个方块放入map
        void inputShowBlockIntoMap()
        {

            map.showBlcoks.Clear();
            foreach (DrawObject item in showBlock)
            {

                item.position.x += 29;
                item.position.y += 10;
                map.showBlcoks.Add(item);
            }
        }
    }
}
