using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class Block
    {
        //存储每个类型的方块整体四个方向的信息
        public List<DrawObject[]> blocks = new List<DrawObject[]>();


        List<Position[]> blockInfo;
        //每个方块的相对于中心方块的位置信息，并存在blockInfo中
        void BlockInfo(E_FigureType figureType)
        {
            blockInfo = new List<Position[]>();
            //方块相对于中心点的位置
            switch (figureType)
            {

                case E_FigureType.Cube:
                    blockInfo.Add(new Position[]{ new Position(2, 0), new Position(0, 1), new Position(2, 1)});
                    blockInfo.Add(new Position[] { new Position(2, 0), new Position(0, 1), new Position(2, 1) });
                    blockInfo.Add(new Position[] { new Position(2, 0), new Position(0, 1), new Position(2, 1) });
                    blockInfo.Add(new Position[] { new Position(2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                case E_FigureType.Line:
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(0, 1), new Position(0, 2) });
                    blockInfo.Add(new Position[] { new Position(-4, 0), new Position(-2, 0), new Position(2, 0) });
                    blockInfo.Add(new Position[] { new Position(0, -2), new Position(0, -1), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(4, 0) });
                    break;
                case E_FigureType.Tu:
                    blockInfo.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(-2, 0), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(-2, 0), new Position(2, 0) });
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(2, 0), new Position(0, 1) });
                    break;
                case E_FigureType.L_N:
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(-2, 0), new Position(-2, 1) });
                    blockInfo.Add(new Position[] { new Position(-2, -1), new Position(0, -1), new Position(2, 0) });
                    blockInfo.Add(new Position[] { new Position(2, -1), new Position(2, 0), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(-2, 0), new Position(0, 1), new Position(2, 1) });
                    break;
                case E_FigureType.R_N:
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(2, 0), new Position(2, 1) });
                    blockInfo.Add(new Position[] { new Position(2, 0), new Position(-2, 1), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(-2, -1), new Position(-2, 0), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(2, -1), new Position(-2, 0) });
                    break;
                case E_FigureType.L_L:
                    blockInfo.Add(new Position[] { new Position(-2, 0), new Position(0, 1), new Position(0, 2) });
                    blockInfo.Add(new Position[] { new Position(2, -1), new Position(-2, 0), new Position(2, 0) });
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(0, 1), new Position(2, 1) });
                    blockInfo.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(-2, 1) });
                    break;
                case E_FigureType.R_l:
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(2, -1), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(-2, 0), new Position(2, 0), new Position(2, 1) });
                    blockInfo.Add(new Position[] { new Position(0, -1), new Position(-2, 1), new Position(0, 1) });
                    blockInfo.Add(new Position[] { new Position(-2, -1), new Position(-2, 0), new Position(2, 0) });
                    break;

            }
        }


        //从外部访问四种方向的方块信息
        public DrawObject[] this[int index]
        {
            get
            {
                if (index < 0)
                    return blocks[0];
                else if (index > blocks.Count)
                    return blocks[blocks.Count - 1];
                else
                    return blocks[index];
            }

        }

        //初始化每个类型的方块信息并存储在block列表中
        public Block(E_FigureType figureType, DrawObject firstBlock)
        {

            BlockInfo(figureType);

            foreach (Position[] item in blockInfo)
            {
                DrawObject[] blocksUpdated = new DrawObject[4];
                //防止内部中心方块信息改变，导致外部firstBlock中心方块信息改变
                DrawObject newFirstBlock = new DrawObject(figureType, firstBlock.position.x, firstBlock.position.y);
                blocksUpdated[0] = newFirstBlock;
                int index = 1;
                foreach (Position item2 in item)
                {
                    DrawObject blockUpdated = new DrawObject(figureType);
                    blockUpdated.position = item2 + blocksUpdated[0].position;
                    blocksUpdated[index] = blockUpdated;
                    index++;
                }
                blocks.Add(blocksUpdated);

            }



        }
    }
}
