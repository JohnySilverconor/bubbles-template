using System;
using System.Windows.Forms;
using System.Drawing;

namespace BobbleNet
{

	public class GraphicsHelper
	{
		public static Font FONTARIAL = new Font(FontFamily.GenericSansSerif ,14,FontStyle.Bold);
		public static SolidBrush BRUSHRED = new SolidBrush(Color.Red);
		public static SolidBrush BRUSHBLACK = new SolidBrush(Color.Black);
		public static Pen PENRED = new Pen(Color.Red);
	}

//	public class GameSurface : Panel 
//	{
//		public Bitmap bitGB;
//
//		public GameSurface()
//		{
//			//this.Location = new Point(0,0);
//		}
//
//		protected override void OnPaint(PaintEventArgs e)
//		{
//			e.Graphics.DrawImage(bitGB,0,0);
//			//base.OnPaint (e);
//		}
//
//
//		protected override void OnPaintBackground(PaintEventArgs e)
//		{
//			//base.OnPaintBackground (e);
//		}
//
//	}
}
