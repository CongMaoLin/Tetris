using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace Tetris
{
    internal class GameScene : ISceneUpdate
    {

        
        Map map;
        BlockWorker worker;
        ConsoleKey key;
        static Thread input;
        
        //记录线程是否结束
        static bool ifThreadRun;
        public GameScene()
        {
            map = new Map();
            worker = new BlockWorker(map);
            ifThreadRun = true;
            input = new Thread(Input);
            //设置成后台模式，主线程结束，副线程也结束
            input.IsBackground = true;
            input.Start();
        }

        void Input()
        {
            while (ifThreadRun)
            {
                lock (worker)
                {
                    if (Console.KeyAvailable)
                    {
                        
                        key = Console.ReadKey(true).Key;
                        worker.ChangeDirection(key);
                        worker.Move(key);
                    }
                    
                }

            }
        }
        public void Update()
        {
            lock (worker)
            {
                if (BlockWorker.mainGameThreadStop) { 
                map.Draw();
                worker.Draw();
                worker.AutoMove();
                }
            }
            Thread.Sleep(300);
  
        } 

        public static void StopThread()
        {
            ifThreadRun = false;
            input = null;
        }
    }
}
