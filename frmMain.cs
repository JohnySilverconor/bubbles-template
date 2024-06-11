using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;

namespace BobbleNet
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		public static bool FormAlive;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnMenu = new System.Windows.Forms.Button();
			this.mnuContext = new System.Windows.Forms.ContextMenu();
			this.Pause = new System.Windows.Forms.MenuItem();
			this.mnuResume = new System.Windows.Forms.MenuItem();
			this.mnuFrameRate = new System.Windows.Forms.MenuItem();
			this.mnuQuit = new System.Windows.Forms.MenuItem();
			this.lblScore = new System.Windows.Forms.Label();
			// 
			// btnMenu
			// 
			this.btnMenu.ContextMenu = this.mnuContext;
			this.btnMenu.Location = new System.Drawing.Point(0, 300);
			this.btnMenu.Size = new System.Drawing.Size(50, 20);
			this.btnMenu.Text = "Menu";
			this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
			// 
			// mnuContext
			// 
			this.mnuContext.MenuItems.Add(this.Pause);
			this.mnuContext.MenuItems.Add(this.mnuResume);
			this.mnuContext.MenuItems.Add(this.mnuFrameRate);
			this.mnuContext.MenuItems.Add(this.mnuQuit);
			this.mnuContext.Popup += new System.EventHandler(this.mnuContext_Popup);
			// 
			// Pause
			// 
			this.Pause.Text = "Pause";
			this.Pause.Click += new System.EventHandler(this.Pause_Click);
			// 
			// mnuResume
			// 
			this.mnuResume.Text = "Resume";
			this.mnuResume.Click += new System.EventHandler(this.mnuResume_Click);
			// 
			// mnuFrameRate
			// 
			this.mnuFrameRate.Text = "Const. Frame rate";
			this.mnuFrameRate.Click += new System.EventHandler(this.mnuFrameRate_Click);
			// 
			// mnuQuit
			// 
			this.mnuQuit.Text = "Quit";
			this.mnuQuit.Click += new System.EventHandler(this.mnuQuit_Click);
			// 
			// lblScore
			// 
			this.lblScore.Location = new System.Drawing.Point(50, 300);
			this.lblScore.Size = new System.Drawing.Size(190, 20);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// frmMain
			// 
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.Controls.Add(this.lblScore);
			this.Controls.Add(this.btnMenu);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "BobbleNet";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMain_Closing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Closed += new System.EventHandler(this.frmMain_Closed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmMain_MouseMove);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>

//		static void Main() 
//		{
//			Application.Run(new frmMain());
//		}

		public Bitmap bitGB;
		public System.Windows.Forms.Label lblScore;
		private System.Windows.Forms.Button btnMenu;
		private System.Windows.Forms.ContextMenu mnuContext;
		private System.Windows.Forms.MenuItem mnuQuit;
		private System.Windows.Forms.MenuItem Pause;
		private System.Windows.Forms.MenuItem mnuResume;
		public BubbleGame BG;
		private System.Windows.Forms.MenuItem mnuFrameRate;
		//public GameSurface GameSurf;

		public string LevelName;
		public int LevelNum;
		public int CurrentLevel;

		public GXInputLibrary.GXInput GxInput;

		private void frmMain_Load(object sender, System.EventArgs e)
		{

			BG = new BubbleGame(this);
			frmMain.FormAlive=true;
			this.WindowState=FormWindowState.Maximized ;

			GxInput = new GXInputLibrary.GXInput();
			GxInput.RegisterAllKeys();

			BG.ConstantFrameRate=true;

			CurrentLevel = LevelNum;
			lblScore.Text = "Lives: " + BG.Lives.ToString();

			while (frmMain.FormAlive)
			{
				switch (BG.Go(LevelName ,CurrentLevel))
				{
					case BubbleGame.LevelReturnValue.DeadLineReached:
						BG.RefreshBackground();
						this.Refresh();
						BG.Lives--;
						lblScore.Text = "Lives: " + BG.Lives.ToString();
						if (BG.Lives==0)
						{
							MessageBox.Show("Game Over !");
							this.Close();
						}
						else	
							MessageBox.Show ("Ho No ! Try again...");
						break;
					case BubbleGame.LevelReturnValue.LevelCompleted:
						BG.RefreshBackground();
						this.Refresh();
						MessageBox.Show("Level Completed !");
						if (LevelName!="Random")
							CurrentLevel++;
						break;
					case BubbleGame.LevelReturnValue.LevelDoesNotExists:
						MessageBox.Show("Game Complete !");
						this.Close();
						break;
					case BubbleGame.LevelReturnValue.ClosingForm:
						this.Close();
						break;
				}
			}

		}

		private void frmMain_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (bitGB!=null)
				e.Graphics.DrawImage(bitGB,0,0);

			#region Code part commented out - just for testing			
//			if (BS!=null)
//			{
////	
////				double DIST;
////
//				e.Graphics.DrawImage(BS.BubbleBitmap,BS.BubbleRectangle,0,0,BS.BubbleBitmap.Width ,BS.BubbleBitmap.Height ,GraphicsUnit.Pixel ,BubbleImages.GetTranspImageAttr());
////				e.Graphics.DrawRectangle(new Pen(Color.Yellow),BS.BubbleRectangle);
////				//Bubble BB = new Bubble(4,9,BubbleKind.Orange );
//				Bubble BB = new Bubble(BS.PositionOnGrid.X,BS.PositionOnGrid.Y,BS.Kind);
////				//DIST = (BS.BubbleRectangle.X-BB.BubbleRectangle.X)*(BS.BubbleRectangle.X-BB.BubbleRectangle.X)+(BS.BubbleRectangle.Y-BB.BubbleRectangle.Y)*(BS.BubbleRectangle.Y-BB.BubbleRectangle.Y);
//////				if (DIST<120)
//////					e.Graphics.DrawRectangle(new Pen(Color.Red),BB.BubbleRectangle );
//////				else
////					e.Graphics.DrawRectangle(new Pen(Color.Green),BB.BubbleRectangle );
////				//e.Graphics.DrawString(DIST.ToString(),new Font(FontFamily.GenericSansSerif,14,FontStyle.Bold ),new SolidBrush(Color.Red),10,10);
////
//				if (BG.GB.CheckSpriteCollision(BS)==true)
//					e.Graphics.DrawRectangle(new Pen(Color.Red),BB.BubbleRectangle );
//				else
//					e.Graphics.DrawRectangle(new Pen(Color.Green),BB.BubbleRectangle );
////
////
//				try
//				{
//					foreach (Bubble B in BG.GB.GetNeighbors(BS.PositionOnGrid))
//					{
//						if (B!=null)
//							e.Graphics.DrawRectangle(new Pen(Color.Blue),B.BubbleRectangle );	
//					}
//				}
//				finally{}
////		
////				try
////				{
////					foreach (Bubble B in  BG.GB.GetFallingBubbles())//BG.GB.GetSameKindNeighbors(BS))
////					{
////						//if (B!=null)
////							e.Graphics.DrawRectangle(new Pen(Color.Blue),B.BubbleRectangle );	
////					}
////				}
////				finally{}
////			
////
////			}
			#endregion
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//base.OnPaintBackground (e);
		}

		private void frmMain_Closed(object sender, System.EventArgs e)
		{
		}

		//BubbleSprite BS;
		private void frmMain_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.Focus();
//			try
//			{
//				BS = new BubbleSprite(e.X, e.Y,0,0,BubbleKind.Red );
//			}
//			finally
//			{}
//
//			if (e.Button == MouseButtons.Left)
//			{
//				BG.GB.Board[BS.PositionOnGrid.X][BS.PositionOnGrid.Y] = null;
//				BG.RefreshBackground();
//				this.Refresh();
//			}
		}

		private void btnMenu_Click(object sender, System.EventArgs e)
		{
			this.mnuContext.Show(this ,this.btnMenu.Location  );
		}

		private void mnuQuit_Click(object sender, System.EventArgs e)
		{
			frmMain.FormAlive=false;
		}

		private void mnuResume_Click(object sender, System.EventArgs e)
		{
			this.Focus();
			BG.Paused=false;
		}

		private void Pause_Click(object sender, System.EventArgs e)
		{
			//this.btnMenu.Focus();
			BG.Paused=true;
		}

		private void frmMain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
//			if (BS!=null)
//			{
//				BG.GB.Board[BS.PositionOnGrid.X][BS.PositionOnGrid.Y] = null;
//				BG.RefreshBackground();
//			}
		}

		private void mnuFrameRate_Click(object sender, System.EventArgs e)
		{
			BG.ConstantFrameRate= !BG.ConstantFrameRate;
			this.Focus();
		}

		private void mnuContext_Popup(object sender, System.EventArgs e)
		{
			mnuFrameRate.Checked = BG.ConstantFrameRate;
		}

		private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			frmMain.FormAlive=false;
			GxInput.UnegisterAllKeys ();
		}
	}
}
