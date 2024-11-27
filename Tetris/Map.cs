using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tetris
{
    internal class Map:IDraw
    {
        //面板
        public List<Panel> panels;
        //周围不变的墙
        public List<DrawObject> Walls = new List<DrawObject>();

        //底层墙
        public List<DrawObject> D_Walls = new List<DrawObject>();
        //顶部分界墙
        public List<DrawObject> EnterWalls = new List<DrawObject>();
        //顶部文字
        DrawObject Top = new DrawObject(E_FigureType.Font, 2, 6);
        DrawObject Buffer = new DrawObject(E_FigureType.Buffer, 2, 0);

        //下一个方块
        public List<DrawObject> showBlcoks = new List<DrawObject>();

          
        public Map()
        {
            //底部墙,包括后期加的方块,,,,,顶部分界墙
            for (int i = 0; i < Game.width-14; i+=2)
            {
                D_Walls.Add(new DrawObject(E_FigureType.Wall, i, Game.height - 2));
                if (i > 2 && i < Game.width - 16) 
                { 
                    EnterWalls.Add(new DrawObject(E_FigureType.EnterWall, i, 6));
                }
                
            }
            //左右墙
            for (int i = 0; i < Game.height -2; i++)
            {
                Walls.Add(new DrawObject(E_FigureType.Wall,0, i));
                Walls.Add(new DrawObject(E_FigureType.Wall,Game.width -16, i)); 
                
            }

            //初始化面板
            panels = new List<Panel>() { new Panel(48, 3, "下一个方块") ,new Panel(49,14, "左：A/←"),
                new Panel(49,16, "右：D/→"),new Panel(49,18, "下：S/↓") ,new Panel(49,20,"变形：Space")};


        }

        public void Draw()
        {
            
            foreach (DrawObject obj in Walls)
            {
                obj.Draw();
            }
            
            foreach(DrawObject obj in D_Walls)
            {
                obj.Draw();
            }
            foreach (DrawObject obj in EnterWalls)
            {
                obj.Draw();
            }
            Top.Draw();
            Buffer.Draw();
            foreach (Panel panel in panels)
            {
                panel.Draw();
            }
            
            foreach (DrawObject item in showBlcoks)
            {
                item.Draw();
            }
            
            
        }

        /// <summary>
        /// 把会移动的方块变成墙
        /// </summary>
        /// <param name="walls"></param>
        public void ChangeToD_Walls(Map map,DrawObject[] block)
        {
            
                foreach (DrawObject obj in block)
                {
                    obj.ChangeBlock(E_FigureType.Wall);
                    map.D_Walls.Add(obj);
                }
            
            
        }

        
        //检测行是否满，满则消除
        public void IfRowsFull()
        {
            //动态行计数
            int[] RowsCounts;
            //记录每一行是否满格，其中包括底部动态墙，注意检测时去掉
            RowsCounts = new int[Game.height - 1];
            

            //给每一行动态墙计数
            foreach(DrawObject obj in D_Walls)
            {
                for (int i = 0; i < RowsCounts.Length; i++)
                {
                    if(obj.position.y == Game.height - i-2)
                    {
                        RowsCounts[i] += 1;
                    }
                }
            }

            //检测有几行满
            int fullRowsNum = 0;
            //检测第几行满
            int RowsNum = 0;
            //如果行满，则从D_Walls中移除
            for(int i=0;i<RowsCounts.Length;i++ )
            {
                if (i == 0)
                {
                    continue;
                }
                else if (RowsCounts[i] == (Game.width-14)/2 - 2)
                {
                    RowsNum = i;
                    fullRowsNum++;
                    while(RowsCounts[i] > 0)
                    { 
                    for(int j = 0;j<D_Walls.Count;j++ )
                    {
                        if(D_Walls[j].position.y == Game.height - 2)
                        {
                            continue;
                        }
                        else if (D_Walls[j].position.y == Game.height -i - 2)
                        {
                            D_Walls[j].Clear();
                            D_Walls.RemoveAt(j);
                            RowsCounts[i]--;
                        }
                    }

                    }
                    
                }
            }
            Thread.Sleep(50);
            while (fullRowsNum > 0)
                { 
                //满行全部删除后，将剩下的动态墙位置下移一位
                    for (int k = 0; k < D_Walls.Count; k++)
                    {

                        if (D_Walls[k].position.y >Game.height - 2-RowsNum)
                        {
                            continue;
                        }
                        else 
                        {
                            D_Walls[k].Clear();
                            D_Walls[k].position.y += 1; 
                        }
                    }
                    fullRowsNum--;
                }
            

            
        }

        //检测列是否满，满则游戏结束
        public bool IfColumsFull()
        {
           
            bool ifGameOver = false;
            foreach (DrawObject item in D_Walls)
            {
                if (item.position.y <= 7)
                {
                    ifGameOver = true;
                    if (BlockWorker.ifGameOverToChangeScene) { 
                    Game.ChangeScene(E_SceneType.Over);
                    }

                    break;

                }
            }

            return ifGameOver;
        }
    }
}
