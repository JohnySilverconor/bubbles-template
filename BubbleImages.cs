using System;
using System.Drawing ;
using System.Reflection;

namespace BobbleNet
{
	public class BubbleImages
	{
		private static Bitmap[] bubbleImages;
		private static System.Drawing.Imaging.ImageAttributes transpAttr;

		static BubbleImages()
		{
			bubbleImages  = new Bitmap [(int)BubbleKind.LAST];

			bubbleImages[(int)BubbleKind.Black] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-black.png"));
			bubbleImages[(int)BubbleKind.Blue] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-blue.png"));
			bubbleImages[(int)BubbleKind.Green] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-green.png"));
			bubbleImages[(int)BubbleKind.Magenta] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-magenta.png"));
			bubbleImages[(int)BubbleKind.Orange] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-orange.png"));
			bubbleImages[(int)BubbleKind.Red] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-red.png"));
			bubbleImages[(int)BubbleKind.White] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-white.png"));
			bubbleImages[(int)BubbleKind.Yellow] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.bubble-yellow.png"));

			transpAttr = new System.Drawing.Imaging.ImageAttributes();
			transpAttr.SetColorKey (bubbleImages[(int)BubbleKind.Black].GetPixel(0,0),bubbleImages[(int)BubbleKind.Black].GetPixel(0,0));

		}

		public static Bitmap GetImage(BubbleKind Kind)
		{
			return bubbleImages[(int)Kind];
		}

		public static System.Drawing.Imaging.ImageAttributes GetTranspImageAttr()
		{
			return transpAttr;
		}
	}
}
