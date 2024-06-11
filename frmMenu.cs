using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace BobbleNet
{
	/// <summary>
	/// Summary description for frmMenu.
	/// </summary>
	public class frmMenu : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox cboLevelGroup;
		private System.Windows.Forms.ComboBox cboDifficulty;
		private System.Windows.Forms.Label lblLevelGroup;
		private System.Windows.Forms.Label lblDifficulty;
		private System.Windows.Forms.Label lblTitle;
		private System.Windows.Forms.Button btnQuit;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Button btnNewGame;
	
		public frmMenu()
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

		static void Main() 
		{
			try
			{
				Application.Run(new frmMenu());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnNewGame = new System.Windows.Forms.Button();
			this.cboLevelGroup = new System.Windows.Forms.ComboBox();
			this.cboDifficulty = new System.Windows.Forms.ComboBox();
			this.lblLevelGroup = new System.Windows.Forms.Label();
			this.lblDifficulty = new System.Windows.Forms.Label();
			this.lblTitle = new System.Windows.Forms.Label();
			this.btnQuit = new System.Windows.Forms.Button();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblInfo = new System.Windows.Forms.Label();
			// 
			// btnNewGame
			// 
			this.btnNewGame.Location = new System.Drawing.Point(48, 160);
			this.btnNewGame.Size = new System.Drawing.Size(144, 20);
			this.btnNewGame.Text = "New Game";
			this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
			// 
			// cboLevelGroup
			// 
			this.cboLevelGroup.Location = new System.Drawing.Point(48, 88);
			this.cboLevelGroup.Size = new System.Drawing.Size(144, 21);
			this.cboLevelGroup.SelectedIndexChanged += new System.EventHandler(this.cboLevelGroup_SelectedIndexChanged);
			// 
			// cboDifficulty
			// 
			this.cboDifficulty.Items.Add("2");
			this.cboDifficulty.Items.Add("3");
			this.cboDifficulty.Items.Add("4");
			this.cboDifficulty.Items.Add("5");
			this.cboDifficulty.Items.Add("6");
			this.cboDifficulty.Items.Add("7");
			this.cboDifficulty.Items.Add("8");
			this.cboDifficulty.Location = new System.Drawing.Point(72, 136);
			this.cboDifficulty.Size = new System.Drawing.Size(100, 21);
			// 
			// lblLevelGroup
			// 
			this.lblLevelGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.lblLevelGroup.Location = new System.Drawing.Point(48, 72);
			this.lblLevelGroup.Size = new System.Drawing.Size(144, 16);
			this.lblLevelGroup.Text = "Game Type";
			// 
			// lblDifficulty
			// 
			this.lblDifficulty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.lblDifficulty.Location = new System.Drawing.Point(72, 120);
			this.lblDifficulty.Text = "Difficulty";
			this.lblDifficulty.Visible = false;
			// 
			// lblTitle
			// 
			this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
			this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(64)));
			this.lblTitle.Location = new System.Drawing.Point(8, 8);
			this.lblTitle.Size = new System.Drawing.Size(224, 32);
			this.lblTitle.Text = "Bobble.Net";
			this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// btnQuit
			// 
			this.btnQuit.Location = new System.Drawing.Point(48, 200);
			this.btnQuit.Size = new System.Drawing.Size(144, 20);
			this.btnQuit.Text = "Quit Game";
			this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
			// 
			// lblVersion
			// 
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.lblVersion.ForeColor = System.Drawing.Color.Navy;
			this.lblVersion.Location = new System.Drawing.Point(200, 248);
			this.lblVersion.Size = new System.Drawing.Size(32, 20);
			this.lblVersion.Text = "v1.0";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblInfo
			// 
			this.lblInfo.Location = new System.Drawing.Point(0, 248);
			this.lblInfo.Size = new System.Drawing.Size(184, 20);
			this.lblInfo.Text = "(c)2004 - Benoit MISCHLER";
			// 
			// frmMenu
			// 
			this.BackColor = System.Drawing.Color.Beige;
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.btnQuit);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.cboLevelGroup);
			this.Controls.Add(this.cboDifficulty);
			this.Controls.Add(this.lblDifficulty);
			this.Controls.Add(this.lblLevelGroup);
			this.Controls.Add(this.btnNewGame);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Bobble.Net";
			this.Load += new System.EventHandler(this.frmMenu_Load);

		}
		#endregion

		private void btnNewGame_Click(object sender, System.EventArgs e)
		{
			frmMain G = new frmMain();
			if (cboLevelGroup.SelectedIndex==0)
			{
				G.LevelName = "Random";
				G.LevelNum = int.Parse(cboDifficulty.Text);
			}
			else
			{
				G.LevelName = cboLevelGroup.Text +".lvl";
				G.LevelNum=1;
			}
			G.ShowDialog();
			G.Dispose();
		}

		private void frmMenu_Load(object sender, System.EventArgs e)
		{
			cboLevelGroup.Items.Add("(Random)");
			
			DirectoryInfo DI = new DirectoryInfo(BubbleGame.GamePath());

			foreach (FileInfo FI in DI.GetFiles("*.lvl"))
			{
				cboLevelGroup.Items.Add(FI.Name.Substring(0,FI.Name.Length-4));
			}
			if (cboLevelGroup.Items.Count >1)
			{
				cboLevelGroup.SelectedIndex=1;
			}
			else
			{	
				cboLevelGroup.SelectedIndex=0;
			}

			cboDifficulty.Visible=false;
			cboDifficulty.SelectedIndex=2;
		}

		private void cboLevelGroup_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cboLevelGroup.SelectedIndex==0)
			{
				cboDifficulty.Visible=true;
				lblDifficulty.Visible=true;
			}
			else
			{
				cboDifficulty.Visible=false;
				lblDifficulty.Visible=false;
			}
		}

		private void btnQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
