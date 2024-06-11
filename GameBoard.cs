using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace BobbleNet
{
	public class GameBoard
	{
		public const int GRID_X_SIZE = 8;  // Grid size X
		public const int GRID_Y_SIZE = 11; // Grid size Y
		public const double MINIMUM_DIST =530d;// 600d; // minimum distance for collision test
		public const double ROW_DIST = 0.85d; // row distance factor (bubble size relative)
		public static double X_POS_OFFSET = 24;
		public const double Y_POS_OFFSET_ADD=24d*ROW_DIST; // BubbleHeight*0.90  //20.4d;
		public const double Y_POS_OFFSET_INITIAL = Y_POS_OFFSET_ADD ;
		public static double Y_POS_OFFSET = Y_POS_OFFSET_INITIAL;

		public static Bubble LeftMostBubble = new Bubble(0,0,BubbleKind.Black);
		public static Bubble RightMostBubble = new Bubble(GRID_X_SIZE-1,0,BubbleKind.Black);
		public static Bubble DeadLineBubble = new Bubble(0,GRID_Y_SIZE-1,BubbleKind.Black);

		public static Bubble NextPlayerBubble = new Bubble(2,11,BubbleKind.Black);
		public static Bubble CurrentPlayerBubble = new Bubble(3,11,BubbleKind.Black);

		private Bubble[][] gameBoard = new Bubble[GRID_X_SIZE][];
		private Bitmap gameBoardBitmap;
		private Bitmap backGround;
		private Bitmap ceiling;
		private Graphics gameBoardGraphics;
		private Random RD =new Random(Environment.TickCount);

		public GameBoard(Rectangle ScreenSize)
		{
			for (int i=0;i<GRID_X_SIZE;i++)
			{
				gameBoard[i] = new Bubble[GRID_Y_SIZE];
			}

			backGround = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.Background.png"));
			ceiling = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BobbleNet.Images.Ceiling.png"));
			gameBoardBitmap = new Bitmap(ScreenSize.Width,ScreenSize.Height);
			gameBoardGraphics  = Graphics.FromImage(gameBoardBitmap);
			GameBoard.Y_POS_OFFSET=Y_POS_OFFSET_INITIAL;
		}

		public Bubble[][] Board
		{	
			get 
			{ return this.gameBoard; }
			set
			{ this.gameBoard = value; }
		}
	
		public bool LoadLevel(string LevelFile, int LevelNum)
		{	
			if (LevelFile=="Random")
			{
				// Create a random board
				for (int x=0;x<GRID_X_SIZE;x++)
					for (int y=0;y<GRID_Y_SIZE-3;y++) // 4
					{
						if (x!=GRID_X_SIZE-1 || y%2==0) 
						{
							this.gameBoard[x][y] = new Bubble(x,y,Bubble.GetRandomKind(LevelNum));
						}
					}
				return true;
			}
			else
			{
				int[,] BubbleData= this.GetLevelData(LevelFile,LevelNum);
				if (BubbleData==null)
					return false;

				for (int x=0;x<GRID_X_SIZE;x++)
					for (int y=0;y<GRID_Y_SIZE;y++)
					{
						if (x!=GRID_X_SIZE-1 || y%2==0) 
						{	
							if (BubbleData[x,y]!=0)
								this.gameBoard[x][y] = new Bubble(x,y,(BubbleKind)BubbleData[x,y]);
						}
					}
				return true;
			}
			
		}

		public int[,] GetLevelData(string LevelFile, int LevelNum)
		{
			int ReadingLevel=0;
			System.IO.StreamReader SR = new System.IO.StreamReader(BubbleGame.GamePath()+LevelFile);
			string OneLine;
			int[,] LevelData = new int[GRID_X_SIZE,GRID_Y_SIZE];
			int currentRow=0, currentColumn=0;

			OneLine=SR.ReadLine();
			
			// Read the entire file until the good level is found
			while (OneLine!=null)
			{
				if (OneLine=="[Level]")
					ReadingLevel++;

				if (ReadingLevel==LevelNum) // If we found the good level inside the file, break the loop and continue the process.
					break;
				
				OneLine=SR.ReadLine();
			}

			if (OneLine==null)	// If the loop exited because we reached the end of file, it means that the levelnumber does not exists in this file..
				return null;

			currentRow = 0;
			OneLine=SR.ReadLine();
			do
			{
				currentColumn=0;
				foreach(string X in OneLine.Split(new char[] {','}))
				{
					try
					{
						LevelData[currentColumn,currentRow]=int.Parse(X);
					}
					catch 
					{	}

					currentColumn++;
					if (currentColumn>GRID_X_SIZE)
						break;
				}
				currentRow++;
				OneLine=SR.ReadLine();
			} while (OneLine!=null && OneLine!="[Level]" && OneLine!="" && currentRow!=GRID_Y_SIZE);

			return LevelData;
		}

		public Bitmap CreateBoardBitmap()
		{
			gameBoardGraphics.DrawImage(backGround,0,0);
			Bubble oneBubble;

//			// Old code no longer used, since we now have some graphics ...
//			gameBoardGraphics.FillRectangle(GraphicsHelper.BRUSHRED ,0,0,24,320);
//			gameBoardGraphics.FillRectangle(GraphicsHelper.BRUSHRED ,240-24,0,240-24,320);
//			gameBoardGraphics.FillRectangle(GraphicsHelper.BRUSHRED ,0,0,240,(int)Y_POS_OFFSET);
//			gameBoardGraphics.FillRectangle(GraphicsHelper.BRUSHRED ,0,GameBoard.NextPlayerBubble.BubbleRectangle.Bottom ,240,GameBoard.NextPlayerBubble.BubbleRectangle.Bottom);

			Rectangle srcRect = new Rectangle(0,ceiling.Height-(int)Y_POS_OFFSET ,ceiling.Width,(int)Y_POS_OFFSET);
			gameBoardGraphics.DrawImage(ceiling,24,0,srcRect,GraphicsUnit.Pixel );

			gameBoardGraphics.DrawLine(GraphicsHelper.PENRED ,0,GameBoard.DeadLineBubble.BubbleCenter.Y,240,GameBoard.DeadLineBubble.BubbleCenter.Y);

			for (int x=0;x<GRID_X_SIZE;x++)
				for (int y=0;y<GRID_Y_SIZE;y++)
				{
					oneBubble= this.gameBoard[x][y];
					if (oneBubble!=null)
					{
						oneBubble.UpdateBubbleRectangle(); // update the bubble rectangle in case of board shift down
						gameBoardGraphics.DrawImage(oneBubble.BubbleBitmap, oneBubble.BubbleRectangle,0,0,oneBubble.BubbleBitmap.Width, oneBubble.BubbleBitmap.Height ,GraphicsUnit.Pixel, BubbleImages.GetTranspImageAttr());
						//gameBoardGraphics.DrawRectangle (new Pen(Color.Red ),oneBubble.BubbleRectangle);
					}
				}

			gameBoardGraphics.DrawImage(GameBoard.NextPlayerBubble.BubbleBitmap, GameBoard.NextPlayerBubble.BubbleRectangle,0,0,GameBoard.NextPlayerBubble.BubbleBitmap.Width, GameBoard.NextPlayerBubble.BubbleBitmap.Height ,GraphicsUnit.Pixel, BubbleImages.GetTranspImageAttr());

			//gameBoardGraphics.DrawString(GameBoard.CurrentPlayerBubble.BubbleRectangle.Bottom.ToString(),GraphicsHelper.FONTARIAL,GraphicsHelper.BRUSHBLACK ,0,0);
			//gameBoardGraphics.DrawString(GameBoard.RightMostBubble.BubbleRectangle.Right.ToString(),GraphicsHelper.FONTARIAL,GraphicsHelper.BRUSHRED ,0,0);

			return gameBoardBitmap ;
		}

		// This method automatically get the sprite's neighbors and detect a collision between the Sprite and the neighbors
		public bool CheckSpriteCollision(BubbleSprite Sprite)
		{
			return CheckSpriteCollision(Sprite, this.GetNeighbors(Sprite.PositionOnGrid));
		}

		// This method detects a collision between a sprite and a known list of neighbors
		public bool CheckSpriteCollision(BubbleSprite Sprite, ArrayList Neighbors)
		{
			double dist;

			foreach (Bubble OneBubble in Neighbors)
			{
				if (OneBubble!=null)
				{
					dist = (Sprite.BubbleRectangle.X-OneBubble.BubbleRectangle.X)*(Sprite.BubbleRectangle.X-OneBubble.BubbleRectangle.X)+(Sprite.BubbleRectangle.Y-OneBubble.BubbleRectangle.Y)*(Sprite.BubbleRectangle.Y-OneBubble.BubbleRectangle.Y);
					if (dist<=MINIMUM_DIST)
						return true;
				}
			}
			return false;
		}

		public ArrayList GetSameKindNeighbors(BubbleSprite ABubbleSprite)
		{
			Bubble oneBubble = this.Board[ABubbleSprite.PositionOnGrid.X][ABubbleSprite.PositionOnGrid.Y];

			ArrayList SameKindNeighbors = new ArrayList();
//			if (this.Board[ABubbleSprite.PositionOnGrid.X][ABubbleSprite.PositionOnGrid.Y]==null)
//				return new ArrayList(0);

			for (int x=0;x<GRID_X_SIZE;x++)
				for (int y=0;y<GRID_Y_SIZE;y++)
				{
					oneBubble= this.gameBoard[x][y];
					if (oneBubble!=null)
					{
						oneBubble.destroyChecked=false;
					}
				}

			if (oneBubble!=null)
			{
				oneBubble.destroyChecked=true;
				SameKindNeighbors.Add(oneBubble);
			}

			this.GetSameKindNeighbors(SameKindNeighbors, ABubbleSprite.PositionOnGrid,ABubbleSprite.Kind);
			
			return SameKindNeighbors;
		}

		public void GetSameKindNeighbors(ArrayList SameKindNeighbors, Point PosOnGrid, BubbleKind Kind)
		{
			foreach(Bubble oneBubble in GetNeighbors(PosOnGrid))
			{
				if (oneBubble!=null)
				{
					if (oneBubble.destroyChecked==false)
					{
						oneBubble.destroyChecked=true;
						if (oneBubble.Kind==Kind)
						{	
							SameKindNeighbors.Add(oneBubble);
							this.GetSameKindNeighbors(SameKindNeighbors,new Point(oneBubble.GridX,oneBubble.GridY),oneBubble.Kind);
						}
					}
				}
			}
		}

		public ArrayList GetFallingBubbles()
		{
			Bubble oneBubble;

			ArrayList FallingBubbles = new ArrayList();

			// Reinit the fallcheck flag to false
			for (int x=0;x<GRID_X_SIZE;x++)
				for (int y=0;y<GRID_Y_SIZE;y++)
				{
					oneBubble= this.gameBoard[x][y];
					if (oneBubble!=null)
					{
						oneBubble.fallChecked=false;
					}
				}		

			// Recursively checkfall each bubble of the top line
			for (int x=0;x<GRID_X_SIZE;x++)
			{
				if (this.Board[x][0]!=null)
				{
					this.Board[x][0].fallChecked=true;	// a bubble on line 0 is fallcheck (will not fall)
					this.FallCheck(new Point(x,0));
				}
			}	

			// Get an array of bubbles that are not fallchecked (not attached to a line 0 bubble -> they will fall)
			for (int x=0;x<GRID_X_SIZE;x++)
				for (int y=0;y<GRID_Y_SIZE;y++)
				{
					oneBubble= this.gameBoard[x][y];
					if (oneBubble!=null)
					{
						if (oneBubble.fallChecked==false)
						{
							FallingBubbles.Add(oneBubble);
						}
					}
				}		

			return FallingBubbles;
		}

		private void FallCheck(Point PosOnGrid)
		{	// Each Neighbor of a gridpoint is fallchecked (will not fall, because it's attached to a line 0 bubble)
			foreach(Bubble oneBubble in GetNeighbors(PosOnGrid))
			{
				if (oneBubble!=null)
				{
					if (oneBubble.fallChecked ==false)
					{	// if a bubble hasn't been fallchecked, fallcheck it and fallcheck its neightbors
						oneBubble.fallChecked=true;
						this.FallCheck(new Point(oneBubble.GridX,oneBubble.GridY));
					}
				}
			}		
		}

		public bool CheckDeadLineReached()
		{
			Bubble oneBubble;

			for (int x=0;x<GRID_X_SIZE;x++)
				for (int y=GRID_Y_SIZE-1;y>=0;y--)
				{
					oneBubble= this.gameBoard[x][y];
					if (oneBubble!=null)
					{
						//if (oneBubble.BubbleRectangle.Y==DeadLineBubble.BubbleRectangle.Y)
						if (oneBubble.BubbleRectangle.Bottom>=DeadLineBubble.BubbleCenter.Y)
							return true;
					}
				}

			return false;
		}

		public ArrayList CountBoardBubbles()
		{
			ArrayList HT = new ArrayList((int)BubbleKind.LAST);
			Bubble oneBubble;

			//LD.Add(BubbleKind.Black ,1);

			for (int x=0;x<GRID_X_SIZE;x++)
				for (int y=0;y<GRID_Y_SIZE;y++)
				{
					oneBubble= this.gameBoard[x][y];
					if (oneBubble!=null)
					{
//						if (HT.Contains(oneBubble.Kind)==true)
//						{
//							//((int)LD[oneBubble.Kind])++;
//							//HT[oneBubble.Kind]= ((int)HT[oneBubble.Kind])+1;
//						}
//						else
//						{
							HT.Add(oneBubble.Kind);
//						}
					}
				}

			return HT;
		}

		public BubbleKind GetBubbleKind()
		{
			ArrayList AL = this.CountBoardBubbles ();

			return (BubbleKind) AL[RD.Next(0,AL.Count)];
			
		}

		public ArrayList GetNeighbors(Point PosOnGrid)
		{
			ArrayList neighbors = new ArrayList();

			if ((PosOnGrid.Y % 2) == 0)
			{
				if (PosOnGrid.X > 0)
				{
					neighbors.Add(gameBoard[PosOnGrid.X-1][PosOnGrid.Y]);

					if (PosOnGrid.Y>0)
					{
						neighbors.Add(gameBoard[PosOnGrid.X-1][PosOnGrid.Y-1]);
					}

					if(PosOnGrid.Y<GRID_Y_SIZE-1)
					{
						neighbors.Add(gameBoard[PosOnGrid.X-1][PosOnGrid.Y+1]);
					}
				}
				
				if (PosOnGrid.X <GRID_X_SIZE-1)
				{
					neighbors.Add(gameBoard[PosOnGrid.X+1][PosOnGrid.Y]);

					if (PosOnGrid.Y>0)
					{
						neighbors.Add(gameBoard[PosOnGrid.X][PosOnGrid.Y-1]);
					}

					if (PosOnGrid.Y<GRID_Y_SIZE-1)
					{
						neighbors.Add(gameBoard[PosOnGrid.X][PosOnGrid.Y+1]);
					}
				}
			}
			else
			{
				if (PosOnGrid.X>0)
				{
					neighbors.Add(gameBoard[PosOnGrid.X-1][PosOnGrid.Y]);
				}

				if (PosOnGrid.X<GRID_X_SIZE-1)
				{
					neighbors.Add(gameBoard[PosOnGrid.X+1][PosOnGrid.Y]);

					if (PosOnGrid.Y>0)
					{
						neighbors.Add(gameBoard[PosOnGrid.X+1][PosOnGrid.Y-1]);
					}

					if (PosOnGrid.Y<GRID_Y_SIZE-1)
					{
						neighbors.Add(gameBoard[PosOnGrid.X+1][PosOnGrid.Y+1]);
					}
				}

				if (PosOnGrid.Y>0)
				{
					neighbors.Add(gameBoard[PosOnGrid.X][PosOnGrid.Y-1]);
					//neighbors.Add(gameBoard[PosOnGrid.X+1][PosOnGrid.Y-1]);
				}

				if (PosOnGrid.Y<GRID_Y_SIZE-1)
				{
					neighbors.Add(gameBoard[PosOnGrid.X][PosOnGrid.Y+1]);
					//neighbors.Add(gameBoard[PosOnGrid.X+1][PosOnGrid.Y+1]);
				}
			}

			return neighbors;
		}

	}
}
