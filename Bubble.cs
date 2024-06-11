using System;
using System.Drawing;

namespace BobbleNet
{
	public enum BubbleKind
	{
		Black=1,
		Blue,
		Green,
		Magenta,
		Orange,
		Red,
		White,
		Yellow,
		LAST 
	}

	public class Bubble
	{
		private int gridX;					// X position of the bubble on the grid
		private int gridY;					// Y position of the bubble on the grid
		private BubbleKind kind;			// kind of bubble (red, green, blue, and so on ...)

		private Rectangle bubbleRectangle;	// Rectangle of the bubble sprite (position of the bubble on screen)

		private Bitmap bubbleBitmap;		// Bitmap for the bubble

		public bool destroyChecked;			// flag for destroy check 
		public bool fallChecked;			// flag for fall check

		public Bubble(int GridPosX, int GridPosY, BubbleKind Kind)
		{
			this.gridX = GridPosX;
			this.gridY = GridPosY;
			this.kind = Kind;
			this.bubbleBitmap = BubbleImages.GetImage(Kind);
			//this.bubbleRectangle = new Rectangle((GridPosX*this.bubbleBitmap.Width)+ GameBoard.X_POS_OFFSET +((this.bubbleBitmap.Width/2) * (GridPosY%2)),(int)(GridPosY*(this.bubbleBitmap.Height*0.85d)+GameBoard.Y_POS_OFFSET),this.bubbleBitmap.Width, this.bubbleBitmap.Height );
			this.UpdateBubbleRectangle();
		}

		public int GridX
		{	
			get { return this.gridX; }
		}

		public int GridY
		{
			get { return this.gridY; }
		}

		public BubbleKind Kind
		{
			get { return this.kind; }
			set
			{
				this.kind = value;
				this.bubbleBitmap = BubbleImages.GetImage(value);
			}
		}

		public Bitmap BubbleBitmap
		{
			get { return this.bubbleBitmap; }
		}

		public Rectangle BubbleRectangle
		{
			get { return this.bubbleRectangle; }
			//get { return new Rectangle((this.gridX*this.bubbleBitmap.Width)+ GameBoard.X_POS_OFFSET +((this.bubbleBitmap.Width/2) * (this.gridY%2)),(int)(this.gridY*(this.bubbleBitmap.Height*0.85d)+GameBoard.Y_POS_OFFSET),this.bubbleBitmap.Width, this.bubbleBitmap.Height ); }
		}

		public void UpdateBubbleRectangle()
		{
			this.bubbleRectangle = new Rectangle((int)((this.gridX*this.bubbleBitmap.Width)+ GameBoard.X_POS_OFFSET +((this.bubbleBitmap.Width/2) * (this.gridY%2))),(int)(this.gridY*(this.bubbleBitmap.Height*GameBoard.ROW_DIST)+GameBoard.Y_POS_OFFSET),this.bubbleBitmap.Width, this.bubbleBitmap.Height );
		}

		public Point BubbleCenter
		{
			get
			{
				int w,h;
				Rectangle BR = this.BubbleRectangle;
				w= (int)(BR.Left+BR.Width/2);
				h= (int)(BR.Top+BR.Height/2);
				return new Point(w,h);
			}
		}

		static Random RD;
		public static BubbleKind GetRandomKind(int Variety)
		{
			if (RD==null)
				 RD = new Random(Environment.TickCount);
//			return (BubbleKind)(RD.Next(0,((int)(BubbleKind.LAST))));			
			return (BubbleKind)(RD.Next(1,Variety+1));			
		}

	}
}
