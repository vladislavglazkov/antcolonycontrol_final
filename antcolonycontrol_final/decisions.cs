#pragma once
using System;
using System.Runtime;
using System.Collections.Generic;
namespace AntAlgo {
	public class DecisionMakeStandard:DecisionMake
	{
		public int state = 0;
		public double GetAngle(double currentAngle,double time, BlockInfo currentBlock, List<BlockInfo> blocks)
		{
			double sum = 0;
			int n = blocks.Count;
			for (int i = 0; i < n; i++)
			{
				
				BlockInfo h = blocks[i];
				if (h.type != 1)
				{
					double val = 1 / (time - h.props[2 * (state ^ 1) + 1]) / h.dist;
					sum += val;
				}
			}
			Random random = new Random();
			double rand = random.NextDouble()*sum;
			double cursum = 0;
			double ans=currentAngle-Math.PI; ;
			if (ans < 0)
            {
				ans += 2 * Math.PI;
            }

			for (int i = 0; i < n; i++)
			{
				BlockInfo h = blocks[i];
				double val = 1 / (time - h.props[2 * (state^1) + 1]) /*/ h.dist*/;
				if (h.type != 1)
				{
					if (cursum < rand)
					{
						ans = h.angle;
					}
				}
				cursum += val;
			}
			return ans;
		}
		public int[] PropsUpdate(double time, BlockInfo currentBlock, BlockInfo newBlock, List<BlockInfo> blocks)
		{
			int[] ans = new int[9];
			for (int i = 0; i < 9; i++)
            {
				ans[i] = currentBlock.props[i];
			}
			if (state == 0)
			{
				ans[0] = 1;
				ans[1] = (int)time - 1;
			}
			else
			{
				ans[2] = 1;
				ans[3] = (int)time - 1;
			}

			if (state == 0 && newBlock.type == 2)
			{
				state = 1;
			}
			if (state == 1 && newBlock.type == 3)
			{
				state = 0;
			}

			return ans;
		}
	}



	public class DecisionMakeStandardAlternative : DecisionMake
	{
		public int state = 0;
		public double GetAngle(double currentAngle, double time, BlockInfo currentBlock, List<BlockInfo> blocks)
		{
			double sum = 0;
			int n = blocks.Count;
			for (int i = 0; i < n; i++)
			{
				BlockInfo h = blocks[i];

				double val = 1 * h.props[2 * (state ^ 1) + 1] / h.dist;
				sum += val;
			}
			Random random = new Random();
			double rand = random.NextDouble() * sum;
			double cursum = 0;
			double ans = currentAngle - Math.PI; ;
			if (ans < 0)
			{
				ans += 2 * Math.PI;
			}

			for (int i = 0; i < n; i++)
			{
				BlockInfo h = blocks[i];
				double val = 1 *  h.props[2 * (state ^ 1) + 1] / h.dist;

				if (cursum < rand)
				{
					ans = h.angle;
				}
				cursum += val;
			}
			return ans;
		}
		public int[] PropsUpdate(double time, BlockInfo currentBlock, BlockInfo newBlock, List<BlockInfo> blocks)
		{
			int[] ans = new int[9];
			for (int i = 0; i < 9; i++)
			{
				ans[i] = newBlock.props[i];
			}
			if (state == 0)
			{
				ans[0] = 1;
				ans[1] = newBlock.props[1] + 1;
			}
			else
			{
				ans[2] = 1;
				ans[3] = newBlock.props[3] + 1;
			}

			if (state == 0 && newBlock.type == 2)
			{
				state = 1;
			}
			if (state == 1 && newBlock.type == 3)
			{
				state = 0;
			}

			return ans;
		}
	}

	
}