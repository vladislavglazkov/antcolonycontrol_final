using System;
using System.Collections.Generic;
namespace AntAlgo
{
    public class BlockInfo
    {
        // public static void Main(string[] args) { }

        public double dist;
        public int type;
        public double angle;
        public int[] props = new int[1000];
        public void Init(double _dist, double _angle, int _type, int[] a)
        {
            this.dist = _dist;
            this.angle = _angle;
            this.type = _type;
            this.props = a;
        }
    }
    
    public class Map
    {
        const int pnum = 10;

        static void Main(string[] args) { }
        public int xsz, ysz;
        public int[,,] vals = new int[pnum, 650, 370];

        public void Init(int xsz1, int ysz1, int types)
        {
            this.xsz = xsz1;
            this.ysz = ysz1;
        }
        public void Set(int type, int x, int y, int val)
        {
            this.vals[type, x, y] = val;
        }
        public int Get(int type, int x, int y)
        {
            return this.vals[type, x, y];
        }
        public void SetAll(int x, int y, int[] a)
        {
            int m = a.Length;
            for (int i = 0; i < m; i++)
            {
                this.vals[i + 1, x, y] = a[i];
            }
        }
        public int[] GetAll(int x, int y)
        {
            int[] a = new int[pnum - 1];
            for (int i = 1; i < pnum; i++)
            {
                a[i - 1] = this.Get(i, x, y);
            }
            return a;
        }
    }
    public interface DecisionMake
    {
        double GetAngle(double currentAngle, double time, BlockInfo currentBlock, List<BlockInfo> blocks);
        int[] PropsUpdate(double time, BlockInfo currentBlock, BlockInfo newBlock, List<BlockInfo> blocks);

    }
}
