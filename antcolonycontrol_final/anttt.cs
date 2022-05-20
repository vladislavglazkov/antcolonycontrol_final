#pragma once
using System;
using System.Collections.Generic;
using System.Runtime;
namespace AntAlgo {
	public class Pair<T, U>
	{
		public Pair()
		{
		}

		public Pair(T first, U second)
		{
			this.First = first;
			this.Second = second;
		}
		
		public T First { get; set; }
		public U Second { get; set; }
	};
	public class Ant : IComparable
	{
		public int id;

		double x;
		double y;
		double time = 0;
		double updtime = 0;
		double v;
		public double angle;
		double angledev;
		double radius;
		List<BlockInfo> infos;
		int xmax = GlobalConstraints.XSize, ymax = GlobalConstraints.YSize;
		DecisionMake decisionmaker = null;
		Map map;
		EventsCallback callback;
		public double TimePredict()
		{
			double xv = Math.Cos(angle) * this.v;
			double yv = Math.Sin(angle) * this.v;
			const double eps = 0.001;
			double timeR = 2 / this.v;
			double timeL = 0;
			while (timeR - timeL > eps)
			{
				double tmed = (timeR + timeL) / 2;
				double nx = this.x + xv * tmed; ;
				double ny = this.y + yv * tmed;
				int xx = (int)nx;
				int yy = (int)ny;
				if (xx != (int)this.x || yy != (int)this.y)
				{
					timeR = tmed;
				}
				else
				{
					timeL = tmed;
				}
			}
			return timeR;
		}
		private double TimeTo(double x,double y,double angle)
        {
			double R = 2;
			double L = 0;
			while (R - L > 0.001)
            {
				double med = (R + L) / 2;
				double xnw = x + med*v * Math.Cos(angle);
				double ynw = y + med*v * Math.Sin(angle);
				if ((int)x == (int)xnw && (int)y == (int)ynw)
                {
					L = med;
                }
                else
                {
					R = med;
                }
            }
			return R;
        }
		public void UpdateInfos()
		{
			//Console.Write("UpdateInfosSession\n");
			infos = new List<BlockInfo>();
			int id = 0;
			double yy = this.y - this.radius;
			if (yy < 0)
			{
				yy = 0;
			}
			int yyy = (int)(yy - 0.5);
			const double eps = 0.000001;

			int yval = Math.Max(0, (int)(this.y - this.radius - 1));
			
			for (; yval <= this.y + this.radius && yval < ymax; yval++)
			{
				int L = -1;
				int R = (int)this.x;
				
				int lblock = R;
				L = (int)this.x;
				R = xmax;
				int rblock = (int)x-1;
				lblock=(int)x;
				for (int i = (int)x; i < xmax; i++)
                {
					double xc = 0.5 + i;
					double yc = 0.5 + yval;
					double xd = x - xc;
					double yd = y - y;

					if (xd * xd + yd * yd <= radius * radius)
                    {
						rblock = i;
                    }
                    else
                    {
						break;
                    }
                }
				for (int i = (int)x; i >=0 ; i--)
				{
					double xc = 0.5 + i;
					double yc = 0.5 + yval;
					double xd = x - xc;
					double yd = y - y;

					if (xd * xd + yd * yd <= radius * radius)
					{
						lblock = i;
					}
                    else
                    {
						break;
                    }
				}


				for (int i = lblock; i <= rblock; i++)
				{
					double yc = yval + 0.5;
					double xc = i + 0.5;
					double xd = xc - this.x;
					double yd = yc - this.y;

					double angle = Math.Atan2(yd, xd);
					double diff = (angle - this.angle);
					if (diff >= 2 * Math.PI)
					{
						diff -= 2 * Math.PI;
					}
					if (diff < 0)
					{
						diff += 2 * Math.PI;
					}
					diff = Math.Min(diff, 2 * Math.PI - diff);
					if (Math.Abs(diff) <= this.angledev)
					{
						if (xd * xd + yd * yd <= radius * radius)
						{
							bool ok = true;
							double cx = x;
							double cy = y;
							while (true)
                            {
								if ((int)cx < 0)
                                {
									continue;
                                }
								if (map.Get(0, (int)cx, (int)cy) == 1)
                                {
									ok = false;
									break;
                                }
								if ((int)cx == i && (int)cy == yval)
                                {
									break;
                                }
								double timadd = TillNext(cx,cy,angle);
								cx += timadd * v * Math.Cos(angle);
								cy += timadd * v * Math.Sin(angle);

                            }


							if (ok)
							{
								//GlobalConstraints.Steps++;
								//Console.WriteLine($"Added for {id} at {yval} {i}");
								BlockInfo info = new BlockInfo();
								info.Init(Math.Sqrt(xd * xd + yd * yd), angle, map.Get(0, (int)i, (int)yval), map.GetAll((int)i, (int)yval)); ;
								infos.Add((info));
								id += 1;
							}
						}
					}
					
				}
			}
			//Console.Write("\n\n\n\n");
		}
		public void Init(double x, double y, double v, double viewAngle, double radius, Map _map, DecisionMake dm,EventsCallback callback,int id)
		{
			this.x = x;
			this.y = y;
			this.v = v;
			this.map = _map;
			this.decisionmaker = dm;
			this.angledev = viewAngle;
			this.radius = radius;
			this.callback = callback;
			this.id = id;
 
		}

		public void Move(double time)
		{
			while (this.updtime < time)
			{
				this.MoveBy();
			}
		}

		public void MoveBy()
		{
			double dt = updtime - time;
			double xv = Math.Cos(angle) * v;
			double yv = Math.Sin(angle) * v;
			int orx = (int)x;
			int ory = (int)y;
			BlockInfo cur = new BlockInfo();
			cur.Init(0, 0, map.Get(0, (int)x, (int)y), map.GetAll((int)x, (int)y));
			x += dt * xv;
			y += dt * yv;
			BlockInfo nxt = new BlockInfo();
			nxt.Init(0, 0, map.Get(0, (int)x, (int)y), map.GetAll((int)x, (int)y));
			int[] upds = decisionmaker.PropsUpdate(updtime, cur, nxt, infos);
			this.time = this.updtime;
			this.map.SetAll(orx, ory, upds);
			this.UpdateInfos();
			int nval;
			if ((nval = map.Get(0, (int)x, (int)y)) != 0)
			{
				// wtf handle does nothing
				//this->callback->Handle(this->updtime, this, nval);
				callback.Handle(this.updtime, this, nval);
			}
			this.angle = decisionmaker.GetAngle(this.angle,this.updtime, nxt, infos);
			this.updtime = this.time + this.TillNext();

		}
		public Pair<double,double> GetRealPos()
        {
			return new Pair<double,double>(x,y);
        }
		public double GetTime()
		{
			return this.updtime;
		}
		public double GetAngle()
        {
			return this.angle;
        }
		public Pair<int, int> GetPos()
		{
			Pair<int, int> a = new Pair<int, int>();
			a.First = (int)this.x;
			a.Second = (int)this.y;
			return a;
		}
		private double TillNext()
        {
			return TillNext(this.x, this.y, this.angle);
		}
		private double TillNext(double x,double y,double angle)
		{
			double xv = v * Math.Cos(angle);
			double yv = v * Math.Sin(angle);
			double xt = 0, yt = 0;
			const double eps = 0.00001;
			if (xv > 0)
			{
				double nx = (int)x + 1;
				xt = (nx - x) / xv + eps;
			}
			else
			{
				double nx = (int)x;
				xt = (nx - x) / xv + eps;
			}
			if (yv > 0)
			{
				double ny = (int)y + 1;
				yt = (ny - y) / yv + eps;
			}
			else
			{
				double ny = (int)y;
				yt = (ny - y) / yv + eps;
			}
			if (Math.Abs(yv) < eps)
            {
				return xt;
            }
            else if (Math.Abs(xv)<eps)
            {
				return yt;
            }
			return Math.Min(xt,yt);
		}


		public int CompareTo(object? obj)
        {
			Ant obj2 = (Ant)obj;
			if (updtime == obj2.updtime)
            {
				return this.id.CompareTo(obj2.id);
            }
			return this.updtime.CompareTo(obj2.updtime);
        }
    }

}