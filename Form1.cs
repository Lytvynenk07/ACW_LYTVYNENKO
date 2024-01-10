using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;
using DFSAlgorithmMaze;


namespace WindowsApplication22
{
	public class Form1 : System.Windows.Forms.Form
	{

		[DllImport("winmm.dll")]
		public static extern long PlaySound(String lpszName, long hModule, long dwFlags);

		private ArrayList Diamonds = new ArrayList(30);
        private ArrayList Traps = new ArrayList(20);
        private ArrayList Hearts = new ArrayList(20);
        private Hero TheHero = new Hero(100, 100);
		private Random RandomGen = new Random();
		private Lives TheLives = new Lives(350, 290);
		private Heart TheHeart = new Heart();
        private const int NumberOfDiamonds = 25;
        private const int NumberOfTraps = 3;
        private const int NumberOfHearts = 1;
        private Score TheScore = new Score(250, 290);
		private const int GameSeconds = 180;
		private TimerDisplay TheTime = new TimerDisplay(20, 290);
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;
		private Thread oThread = null;
		private Maze TheMaze  = new Maze();
		private bool m_bGameDone = false;
		private GameMessage TheStatusMessage = new GameMessage(150, 10);
        private GameMessage TheLivesMessage = new GameMessage(290, 10);
        private MenuStrip Menu;
		private ToolStripMenuItem menuToolStripMenuItem;
		private ToolStripMenuItem quitToolStripMenuItem;
		private ToolStripMenuItem restartToolStripMenuItem;
		private ToolStripMenuItem newGameToolStripMenuItem;
		private GameMessage TheDiamondMessage = new GameMessage(150, 10);
		public enum Side  {top = 0, left = 1, bottom = 2, right = 3};
        private int TheSeconds = 0;
		public Form1()
		{
			InitializeComponent();
			Maze.kDimension = 15;
			Cell.kCellSize = 30;
            TheSeconds = GameSeconds;
			TheMaze.Initialize();
			TheMaze.Generate();

			InitializeDiamonds();
            InitializeTraps();
			InitializeHearts();
            InitializeHero();
			InitializeDiamondsMessage();
			InitializeLivesMessage();
            InitializeTimer();
			InitializeScore();
			InitializeLives();


			SetBounds(10, 10, (Maze.kDimension + 1) * Cell.kCellSize + Cell.kPadding, (Maze.kDimension + 3) * Cell.kCellSize + Cell.kPadding);
			// reduce flicker

			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true);

		}

		private string m_strCurrentSoundFile = "miss.wav";
		public void PlayASound()
		{
			if (m_strCurrentSoundFile.Length > 0)
			{
				PlaySound(Application.StartupPath + "\\" + m_strCurrentSoundFile, 0, 0);
			}
			m_strCurrentSoundFile = "";
			oThread.Abort();
		}

		public void PlaySoundInThread(string wavefile)
		{
			m_strCurrentSoundFile = wavefile;
			oThread = new Thread(new ThreadStart(PlayASound));
			oThread.Start();
		}


		public void InitializeTimer()
		{
			timer1.Start();
			TheTime.Direction = TimeDirection.Down;
			TheTime.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
		}

		public void InitializeDiamondsMessage()
		{
			TheStatusMessage.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
			TheDiamondMessage.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
			TheDiamondMessage.Message = "Diamonds";
		}
        public void InitializeLivesMessage()
        {
            TheStatusMessage.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
            TheLivesMessage.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
            TheLivesMessage.Message = "Lives";
        }



        public void InitializeScore()
		{
			TheScore.Reset();
			TheScore.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
		}
		public void InitializeLives()
		{
			TheLives.ResetLives();
			TheLives.Position.Y = (Maze.kDimension) * Cell.kCellSize + Cell.kPadding;
		}

        public void InitializeHero()
        {
            Point cellCenter = GetRandomCellPosition();
            if (IsOccupiedByTrap(cellCenter) || IsOccupiedByHeart(cellCenter) || IsOccupiedByDiamond(cellCenter))
            {
                cellCenter = GetRandomCellPosition();
            }
            TheHero.Position.X = cellCenter.X - 10;
            TheHero.Position.Y = cellCenter.Y - 10;
        }
        public void InitializeDiamonds()
        {
            for (int i = 0; i < NumberOfDiamonds; i++)
            {
                Point cellCenter = GetRandomCellPosition();
                while (IsOccupiedByTrap(cellCenter) || IsOccupiedByHeart(cellCenter) || IsOccupiedByDiamond(cellCenter))
                {
                    cellCenter = GetRandomCellPosition();
                }
                Diamonds.Add(new Diamond(cellCenter.X - 6, cellCenter.Y - 6));
            }
        }

        public void InitializeTraps()
        {
            for (int i = 0; i < NumberOfTraps; i++)
            {
                Point cellCenter = GetRandomCellPosition();
                while (IsOccupiedByTrap(cellCenter) || IsOccupiedByHeart(cellCenter) || IsOccupiedByDiamond(cellCenter))
                {
                    cellCenter = GetRandomCellPosition();
                }
                Traps.Add(new Trap(cellCenter.X - 10, cellCenter.Y - 10));
            }
        }
        public void InitializeHearts()
        {
            for (int i = 0; i < NumberOfHearts; i++)
            {
                Point cellCenter = GetRandomCellPosition();
                while (IsOccupiedByTrap(cellCenter) || IsOccupiedByHeart(cellCenter) || IsOccupiedByDiamond(cellCenter))
                {
                    cellCenter = GetRandomCellPosition();
                }
                Hearts.Add(new Heart(cellCenter.X - 6, cellCenter.Y - 6));
            }
        }

        private bool IsOccupiedByDiamond(Point cellCenter)
        {
            foreach (Diamond diamond in Diamonds)
            {
                if (diamond.GetFrame().Contains(cellCenter))
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsOccupiedByTrap(Point cellCenter)
        {
            foreach (Trap trap in Traps)
            {
                if (trap.GetFrame().Contains(cellCenter))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsOccupiedByHeart(Point cellCenter)
        {
            foreach (Heart heart in Hearts)
            {
                if (heart.GetFrame().Contains(cellCenter))
                {
                    return true;
                }
            }
            return false;
        }


        public Point GetRandomCellPosition()
		{
			int xCell = RandomGen.Next(0, Maze.kDimension);
			int yCell = RandomGen.Next(0, Maze.kDimension);
			Point cellCenter = TheMaze.GetCellCenter(xCell, yCell);
			return cellCenter;
		}

		



		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Menu
            // 
            this.Menu.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.newGameToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 549);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(592, 24);
            this.Menu.TabIndex = 0;
            this.Menu.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.QuitMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.RestartMenuItem_Click);
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.newGameToolStripMenuItem.Text = "New Game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGame_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(592, 573);
            this.Controls.Add(this.Menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hunter Maze Game";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}


		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.FillRectangle(Brushes.LightGray, 0, 0, this.ClientRectangle.Width, ClientRectangle.Height);

			TheMaze.Draw(g);

			// draw the score

			TheScore.Draw(g);		
			TheLives.Draw(g);
	        
			// draw the time
            
			TheTime.Draw(g, TheSeconds);
            
			// Draw a message indicating the status of the game
			TheStatusMessage.Draw(g);
            
			// Draw a message indicating a Diamond
			if (TheScore.Value == 1)
				TheDiamondMessage.Message = "Diamond";
			else
				TheDiamondMessage.Message = "Diamonds";
            
			TheDiamondMessage.Draw(g);
            
            TheLivesMessage.Message = "Lives";
            
            TheLivesMessage.Draw(g);
            
            
            // draw the diamonds
            
            for (int i = 0; i < Diamonds.Count; i++)
			{
				((Diamond)Diamonds[i]).Draw(g);
			}
            
            //draw the traps
            
            for (int i = 0; i < Traps.Count; i++)
            {
                ((Trap)Traps[i]).Draw(g);
            }
            for (int i = 0; i < Hearts.Count; i++)
            {
                ((Heart)Hearts[i]).Draw(g);
            }
            
            // also draw the hero
            TheHero.Draw(g);

		}
        private int CheckIntersectionTrap()
        {
            for (int i = 0; i < Traps.Count; i++)
            {
                Rectangle trapRect = ((Trap)Traps[i]).GetFrame();
                if (TheHero.GetFrame().IntersectsWith(trapRect))
                {
                    TheLives.DecreaseLife();
                    return i;
                }
            }

            return -1;
        }
        private int CheckIntersectionHeart()
        {
            for (int i = 0; i < Hearts.Count; i++)
            {
                Rectangle heartRect = ((Heart)Hearts[i]).GetFrame();
                if (TheHero.GetFrame().IntersectsWith(heartRect))
                {
                    TheLives.IncreaseLife();
                    return i;
                }
            }

            return -1;
        }
        private int CheckIntersection()
		{
			for (int i = 0; i < Diamonds.Count; i++)
			{
				Rectangle diamondRect = ((Diamond)Diamonds[i]).GetFrame();
				if (TheHero.GetFrame().IntersectsWith(diamondRect))
				{
					return i;
				}
			}

			return -1;
		}

		private bool CanHeroMove(Side aSide)
		{
		  int theSide = (int)aSide;
		  Cell HeroCell = TheMaze.GetCellFromPoint(TheHero.Position.X + 10, TheHero.Position.Y + 10);
			if (HeroCell.Walls[theSide] == 1)
			{
				if (HeroCell.GetWallRect((int)aSide).IntersectsWith(TheHero.GetFrame()))
				{				
					return false;  // blocked
				}


			}

			return true;  // not blocked
		}

		string LatestKey = "none";

		private void HandleLatestKey()
		{
			if (m_bGameDone)
				return;  // precondition

//			string result = e.KeyData.ToString();
			string result = LatestKey;
			Invalidate(TheHero.GetFrame());
			switch (result)
			{
				case "Left":
					if (CanHeroMove(Side.left))
					{
						TheHero.MoveLeft(ClientRectangle);
					}
					Invalidate(TheHero.GetFrame());
					break;
				case "Right":
					if (CanHeroMove(Side.right))
					{
						TheHero.MoveRight(ClientRectangle);
					}
					Invalidate(TheHero.GetFrame());
					break;
				case "Up":
					if (CanHeroMove(Side.top))
					{
						TheHero.MoveUp(ClientRectangle);
					}
					Invalidate(TheHero.GetFrame());
					break;
				case "Down":
					if (CanHeroMove(Side.bottom))
					{
						TheHero.MoveDown(ClientRectangle);
					}
					Invalidate(TheHero.GetFrame());
					break;
				default:
					break;

			}

			int hit = CheckIntersection();
			if (hit != -1)
			{
				TheScore.Increment();

				PlaySoundInThread("diamond.wav");
				Invalidate(TheScore.GetFrame());
				Invalidate(((Diamond)Diamonds[hit]).GetFrame()); 
				Diamonds.RemoveAt(hit);
				if (Diamonds.Count == 0)
				{
                    MessageBox.Show(
                    $"Your time is \" {TheTime.TheString} \" seconds.",
                    "You Win!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                    timer1.Stop();
                }
			}
            hit = CheckIntersectionTrap();
            if (hit != -1)
            {
                Invalidate(TheLives.GetFrame());

                PlaySoundInThread("hurt.wav");
                Invalidate(((Trap)Traps[hit]).GetFrame());
                Traps.RemoveAt(hit);
            }
            hit = CheckIntersectionHeart();
            if (hit != -1)
            {
                Invalidate(TheLives.GetFrame());

                PlaySoundInThread("heal.wav");
                Invalidate(((Heart)Hearts[hit]).GetFrame());
                Hearts.RemoveAt(hit);
            }

        }

		private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			string result = e.KeyData.ToString();
			LatestKey = result;
		}

		static long TimerTickCount = 0;
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			TimerTickCount++;

			if (TimerTickCount % 2 == 0) // do the key handling here
			{
				HandleLatestKey();
			}
			
			if (TimerTickCount % 50 == 0) // every 50 is one second
			{
				if (TheTime.Direction == TimeDirection.Up)
					TheSeconds++;
				else
					TheSeconds--;

				Invalidate(TheTime.GetFrame());

				if (TheSeconds == 0)
				{
                    MessageBox.Show(
                        "Time is Up",
                        "Game Over",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    m_bGameDone = true;
					timer1.Stop();
					Invalidate(TheStatusMessage.GetFrame());
				}
                if (TheLives.NumberOfLives <= 0)
                {
                    MessageBox.Show(
                        "Lives is null",
                        "Game Over",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information,
                        MessageBoxDefaultButton.Button1,
                        MessageBoxOptions.DefaultDesktopOnly);
                    timer1.Stop();
                }
            }

		}

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private int GetInitialSeconds()
        {
            return GameSeconds;
        }
        private void RestartGameField()
        {
            timer1.Stop();

            InitializeHero();

            Diamonds.Clear();
            InitializeDiamonds();

            Traps.Clear();
            InitializeTraps();

            Hearts.Clear();
            InitializeHearts();

            TheScore.Reset();
			TheLives.ResetLives();
            TheTime.Reset();
            TheSeconds = GetInitialSeconds();

            timer1.Start();

            TheStatusMessage.Message = "";
            m_bGameDone = false;

            Invalidate();
        }
        private void NewGameField()
        {
            timer1.Stop();

            InitializeHero();

            Diamonds.Clear();
            InitializeDiamonds();

            Traps.Clear();
            InitializeTraps();

            Hearts.Clear();
			InitializeHearts();

            TheScore.Reset();
            TheLives.ResetLives();
            TheTime.Reset();
            TheSeconds = GetInitialSeconds();

            TheMaze.Initialize();
            TheMaze.Generate();

            timer1.Start();

            TheStatusMessage.Message = "";
            m_bGameDone = false;

            Invalidate();
        }



        private void RestartMenuItem_Click(object sender, EventArgs e)
        {
			RestartGameField();
        }

		private void newGame_Click(object sender, EventArgs e)
		{
			NewGameField();
		}
	}
}
