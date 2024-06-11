using System;
using System.Drawing;

namespace BobbleNet
{
	
	public class BubbleSprite
	{

		// Maximum speed of the player Sprite.
		private const double MAX_SPEED=10d;

		double xpos, ypos;		// X & Y position on screen
		double xmove, ymove;	// relative X & Y move of the sprite for each step
		
		private BubbleKind kind;			// kind of bubble (red, green, blue, and so on ...)

		private Bitmap bubbleBitmap;		// Bitmap for the bubble

		// For an arbitrary positionned sprite
		public BubbleSprite(int XPos, int Ypos, double XMove, double YMove, BubbleKind Kind)
		{
			this.kind = Kind;
			this.bubbleBitmap = BubbleImages.GetImage(Kind);

			this.xpos = XPos;
			this.ypos = Ypos;
			this.xmove = XMove;
			this.ymove = YMove;
		}

		// For a player sprite : autoposition the sprite (uses the static 'CurrentPlayerBubble' of the GameBoard class)
		public BubbleSprite(double Angle, BubbleKind Kind)
		{
			this.kind = Kind;
			this.bubbleBitmap = BubbleImages.GetImage(Kind);

			this.xpos = GameBoard.CurrentPlayerBubble.BubbleRectangle.X;
			this.ypos = GameBoard.CurrentPlayerBubble.BubbleRectangle.Y;

			AngleHelper AH = AngleHelper.DeltaXYFromAngle(MAX_SPEED,Angle);

			this.xmove = AH.X;
			this.ymove = AH.Y;
			
		}

		public Point PositionOnGrid
		{
			get 
			{
				int gridx, gridy;

				gridy = (int)((this.ypos - GameBoard.Y_POS_OFFSET + (bubbleBitmap.Height/2)) / (bubbleBitmap.Height*GameBoard.ROW_DIST));
				gridx = (int)((this.xpos - GameBoard.X_POS_OFFSET + (bubbleBitmap.Width/2) - ((bubbleBitmap.Width/2)*(gridy%2)))/ bubbleBitmap.Width) ;

				if (gridy<0)
					gridy=0;

				if (gridy>GameBoard.GRID_Y_SIZE -1)
					gridy = GameBoard.GRID_Y_SIZE-1;

				if (gridx<0)
					gridx=0;

				if (gridx>GameBoard.GRID_X_SIZE-1)
					gridx = GameBoard.GRID_X_SIZE-1;

				if (gridy%2==1 && gridx==GameBoard.GRID_X_SIZE-1)
					gridx--;

				return new Point(gridx, gridy);
			}
		}

		public BubbleKind Kind
		{
			get { return this.kind; }
		}

		public Bitmap BubbleBitmap
		{
			get { return this.bubbleBitmap; }
		}

		public Rectangle BubbleRectangle
		{
			get { return new Rectangle((int)this.xpos , (int)this.ypos, this.bubbleBitmap.Width, this.bubbleBitmap.Height); }
		}

		public void Move()
		{
			this.Move(false);
		}

		public void Move(bool BackMove)
		{
			if (BackMove==false)
			{
				this.xpos+=this.xmove;
				this.ypos-=this.ymove;
			}
			else
			{
				this.xpos-=this.xmove;
				this.ypos+=this.ymove;
			}

			// Left rebound
			if (this.xpos < GameBoard.LeftMostBubble.BubbleRectangle.X)
			{
				this.xpos = GameBoard.LeftMostBubble.BubbleRectangle.X+(GameBoard.LeftMostBubble.BubbleRectangle.X-this.xpos);
				this.xmove = -this.xmove;
			}

			// Right rebound
			if (this.xpos > GameBoard.RightMostBubble.BubbleRectangle.X)
			{
				this.xpos = GameBoard.RightMostBubble.BubbleRectangle.X+(GameBoard.RightMostBubble.BubbleRectangle.X-this.xpos);
				this.xmove = -this.xmove;
			}
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
	}
}
