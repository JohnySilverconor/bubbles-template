using System;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices ;
using System.Windows.Forms;
using System.Collections;

namespace BobbleNet
{
	public class BubbleGame
	{
		[DllImport("coredll.dll")]
		public static extern int GetAsyncKeyState(int vkey);

		public frmMain mainForm;
		private frmMain gameSurface;
		public GameBoard GB;
		public double angle;
		public bool Paused = false;
		public bool PausedAlready = false;
		public int Lives=3;
		public bool ConstantFrameRate=false;
		public const double MAX_WAIT_TIME=500;
		public static bool GameRunning=false;

		public	Bitmap GAME, BackGround;	// bitmaps: one for the real screen, another for the background (non moving bubbles)
		public 	Graphics GAMEGR;

		public enum LevelReturnValue
		{
			AbnormalTermination,
			LevelCompleted,
			DeadLineReached,
			LevelDoesNotExists,
			ClosingForm
		}

		public void RefreshBackground()
		{
			BackGround = GB.CreateBoardBitmap();
			GAMEGR.DrawImage(BackGround,0,0);
		}

		public BubbleGame(frmMain MainForm)
		{
			mainForm = MainForm;
			gameSurface = MainForm; // mainForm.GameSurf;
		}

		/// <summary>
		///		Start a bobble game level. This is the main game loop
		/// </summary>
		/// <param name="LevelFile">Name of the file containing the levels, or 'Random' to generate a random level </param>
		/// <param name="LevelNum">Level number if <see cref="LevelFile"/> is a file name, or the variety of bubbles for the random generator  </param>
		/// <returns></returns>
		public BubbleGame.LevelReturnValue Go(string LevelFile,int LevelNum)
		{
			BubbleKind currentKind, nextKind;
			BubbleSprite PlayerSprite;
			MovingSprites movingSprites = new MovingSprites(gameSurface.ClientRectangle);
			Random RD = new Random(Environment.TickCount);

			double WaitTime=0;		// game 'time frame' elapsed waiting for the player to launch the bubble
			double HurryTime;		// Used to display a hurry counter

			int FiredBubbles=0;		// After each 8 bubbles launched the ceiling gets down

			ArrayList SameKindNeighbors;

			bool ReadyToFire = true;

//			int xx,yy;
				
			int oldTick;

			int UsedTime=1;

			GB = new GameBoard (gameSurface.ClientRectangle);
			
			if (GB.LoadLevel(LevelFile, LevelNum)==false)
				return BubbleGame.LevelReturnValue.LevelDoesNotExists;

			angle = 90; // Arrow will point up

			currentKind  = GB.GetBubbleKind();
			nextKind = GB.GetBubbleKind();
			GameBoard.NextPlayerBubble.Kind = nextKind;

			PlayerSprite = new BubbleSprite(0,currentKind);

			BackGround = GB.CreateBoardBitmap();
			GAME = new Bitmap(BackGround);
			GAMEGR = Graphics.FromImage(GAME);
			
			gameSurface.bitGB  = GAME;

			oldTick = Environment.TickCount;
			BubbleGame.GameRunning=true;
			
			try
			{
				while (frmMain.FormAlive==true)
				{
					if (mainForm.Focused && Paused==false)
					{
						if ((GetAsyncKeyState((int)System.Windows.Forms.Keys.Left) & 0x8000) !=0)
							angle+=2;	//x--;
						if ((GetAsyncKeyState((int)System.Windows.Forms.Keys.Right) & 0x8000) !=0)
							angle-=2;	//x++;

						angle = angle<10 ? 10 : angle;
						angle = angle>170 ? 170 : angle;

						if (ReadyToFire)
						{
							WaitTime++;	// The bubble is readytofire, but the player isn't doing anything... we will auto-launch the bubble after some time...
						}

						// No sprite moving and Fire pressed:
						if (ReadyToFire && (WaitTime>=MAX_WAIT_TIME || (GetAsyncKeyState((int)System.Windows.Forms.Keys.Up) & 0x8000)!=0))
						{
							PlayerSprite = new BubbleSprite(angle,currentKind);
							currentKind = nextKind;
							nextKind = GB.GetBubbleKind(); //Bubble.GetRandomKind();
							GameBoard.NextPlayerBubble.Kind = nextKind;

							FiredBubbles++;
							ReadyToFire =false;
							WaitTime=0.0;
						}

						// A bubble sprite is moving:
						if (!ReadyToFire)
						{
							PlayerSprite.Move();
							if (GB.CheckSpriteCollision(PlayerSprite) || PlayerSprite.PositionOnGrid.Y==0 )
							{
								// test if bubbles must be destroyed
								// Backmove if sprite is over an existing bubble - sometimes, the player bubble is moving too fast, and 'jump' over another bubble, so we must go back if it happens...
								while (GB.Board[PlayerSprite.PositionOnGrid.X][PlayerSprite.PositionOnGrid.Y]!=null)
								{
									PlayerSprite.Move(true);
								}

								// 'Stick' the bubble on the board.
								GB.Board[PlayerSprite.PositionOnGrid.X][PlayerSprite.PositionOnGrid.Y] = new Bubble(PlayerSprite.PositionOnGrid.X, PlayerSprite.PositionOnGrid.Y,PlayerSprite.Kind);

								SameKindNeighbors = GB.GetSameKindNeighbors(PlayerSprite);
								if (SameKindNeighbors.Count>=3)
								{
									foreach (Bubble B in SameKindNeighbors)
									{
										GB.Board[B.GridX][B.GridY]=null;
										// Exploding bubbles
										movingSprites.AddMovingSprite(new BubbleSprite(B.BubbleRectangle.X,B.BubbleRectangle.Y,(double)RD.Next(0,10)-5d,(double)RD.Next(0,15)-25d,B.Kind));
									}

									// check to see if some bubbles must fall
									foreach (Bubble B in GB.GetFallingBubbles())
									{
										GB.Board[B.GridX][B.GridY]=null;
										movingSprites.AddMovingSprite(new BubbleSprite(B.BubbleRectangle.X,B.BubbleRectangle.Y,0d,-20d,B.Kind));
									}

									if (GB.CountBoardBubbles().Count==0)
										return BubbleGame.LevelReturnValue.LevelCompleted;

								}

								if (FiredBubbles==8)
								{
									FiredBubbles=0;
									GameBoard.Y_POS_OFFSET+=GameBoard.Y_POS_OFFSET_ADD;
									//BackGround = GB.CreateBoardBitmap();
								}

								// Refresh Background:
								BackGround = GB.CreateBoardBitmap();

								if (GB.CheckDeadLineReached()==true)
									return BubbleGame.LevelReturnValue.DeadLineReached;

								ReadyToFire = true;
								PlayerSprite = new BubbleSprite(angle,currentKind);
							}
						}

						GAMEGR.DrawImage(BackGround,0,0);
						#region Code part commented out - just for testing purpose...
						//GAMEGR.DrawString(angle.ToString(),new Font(FontFamily.GenericSansSerif ,14,FontStyle.Bold),new SolidBrush(Color.Red),10,10);

						//xx = (int)(20d* -Math.Cos (angle * Math.PI / 80d));
						//yy = (int)(20d* -Math.Sin (angle * Math.PI / 80d));

						//					// Line drawing
						//					AngleHelper AH = AngleHelper.DeltaXYFromAngle(30,angle);
						//					xx = (int)AH.X;
						//					yy = (int)AH.Y;
						//
						//					//GAMEGR.DrawLine(new Pen(Color.Red),120,gameSurface.ClientRectangle.Height-40 ,120+xx,gameSurface.ClientRectangle.Height- yy-40);
						//					GAMEGR.DrawLine(GraphicsHelper.PENRED,GameBoard.CurrentPlayerBubble.BubbleCenter.X,GameBoard.CurrentPlayerBubble.BubbleCenter.Y,GameBoard.CurrentPlayerBubble.BubbleCenter.X+xx,GameBoard.CurrentPlayerBubble.BubbleCenter.Y-yy);		
						#endregion

						// Draw player bubble
						GAMEGR.DrawImage(PlayerSprite.BubbleBitmap,PlayerSprite.BubbleRectangle,0,0,PlayerSprite.BubbleBitmap.Width,PlayerSprite.BubbleBitmap.Height,GraphicsUnit.Pixel,BubbleImages.GetTranspImageAttr());

						// Draw launcher
						// TODO: define constants for the width & height of the launcher images
						GAMEGR.DrawImage(LauncherImages.GetLauncher((170-(int)angle)/2),LauncherImages.GetLauncherRect () ,0,0,72,64,GraphicsUnit.Pixel,LauncherImages.GetTranspImageAttr());

						if (WaitTime > MAX_WAIT_TIME*0.7)
						{
							HurryTime = 4-40.0 * ((WaitTime - MAX_WAIT_TIME*0.7)/MAX_WAIT_TIME*0.3) ;
							GAMEGR.DrawString("Hurry Up ! " + HurryTime.ToString("0") ,GraphicsHelper.FONTARIAL ,GraphicsHelper.BRUSHRED,0,260);
						}

						movingSprites.MoveSprites();
						foreach (BubbleSprite BS in movingSprites.Sprites)
						{
							GAMEGR.DrawImage(BS.BubbleBitmap,BS.BubbleRectangle,0,0,BS.BubbleBitmap.Width,BS.BubbleBitmap.Height,GraphicsUnit.Pixel,BubbleImages.GetTranspImageAttr());
						}

						//GAMEGR.DrawString(UsedTime.ToString(),GraphicsHelper.FONTARIAL,GraphicsHelper.BRUSHRED ,0,0);
					
						gameSurface.Refresh();
						PausedAlready=false;
					}
					else
					{
						if (PausedAlready==false)
						{
							PausedAlready = true;
							GAMEGR.DrawString("PAUSE",new Font(FontFamily.GenericSansSerif,14,FontStyle.Bold),new SolidBrush(Color.Red),0,0);
							gameSurface.Refresh();
						}
					}

					System.Windows.Forms.Application.DoEvents();

					// Tried to use 'Thread.Sleep', but it was not very accurate ...
					if (ConstantFrameRate==true)
					{
						do
						{
							UsedTime = Environment.TickCount - oldTick;
						} while (UsedTime<25); // 50 frame/sec

						oldTick = Environment.TickCount;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			BubbleGame.GameRunning=false;
			return BubbleGame.LevelReturnValue.ClosingForm;
		}
	
		/// <summary>
		/// Get the game directory
		/// </summary>
		/// <returns>The game path</returns>
		public static string GamePath()
		{
			string FullName= System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase ;
			int LastSep = FullName.LastIndexOfAny(new char[] {'/','\\'});
			return FullName.Substring(0,LastSep+1);
		}
	
	}
}
