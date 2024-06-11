using System;
using System.Drawing;

namespace BobbleNet
{
	public class AngleHelper
	{

		public double X,Y;

		private AngleHelper()
		{
		}

		public static AngleHelper DeltaXYFromAngle(double Distance,double Angle)
		{
			double xx,yy;
			AngleHelper AngHel = new AngleHelper();

			xx = (int)(Distance* Math.Cos (Angle * Math.PI / 180d));
			yy = (int)(Distance* Math.Sin (Angle * Math.PI / 180d));

			AngHel.X = xx;
			AngHel.Y = yy;

			return AngHel;
		}
	}
}
