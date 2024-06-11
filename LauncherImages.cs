using System;
using System.Drawing ;
using System.Reflection;

namespace BobbleNet
{
	public class LauncherImages
	{
		private const int POSX=84;
		private const int POSY=219;

		private static Bitmap[] launcherImages;
		private static System.Drawing.Imaging.ImageAttributes transpAttr;
		private static Rectangle launcherRect;

		static LauncherImages()
		{
			launcherImages = new Bitmap[81];
			string ResName;

			for (int i=0;i<=80;i++)
			{
				ResName = String.Format("BobbleNet.Images.Launcher.Launcher{0:0000}.png",i);
				launcherImages[i] = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(ResName));
			}

			transpAttr = new System.Drawing.Imaging.ImageAttributes();
			transpAttr.SetColorKey (launcherImages[0].GetPixel(0,0),launcherImages[0].GetPixel(0,0));
			
			launcherRect = new Rectangle(POSX,POSY,launcherImages[0].Width ,launcherImages[0].Height);
		}

		public static Bitmap GetLauncher(int Index)
		{
			return launcherImages[Index];
		}

		public static System.Drawing.Imaging.ImageAttributes GetTranspImageAttr()
		{
			return transpAttr;
		}

		public static Rectangle GetLauncherRect()
		{
			return launcherRect;
		}
	
	}
}
