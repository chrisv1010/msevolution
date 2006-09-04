using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

//build smart placemine
//change colors
//add flag and mine
//faster load on button add

namespace MSEvolution {
    public partial class MSEvolution : Form {
        private MineField field;
        
        public MSEvolution() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            panel1.Enabled = true;
            field = new MineField(this.panel1, 10, 10, 2, 6, pbmineadd);
            field.Tick += new EventHandler(GameTick);
            field.DismantledMinesChanged += new EventHandler(GameDismantledMinesChanged);            
            field.NewGame();            
        }

        private void GameTick(object sender, EventArgs e) {
            labelTime.Text = field.Time.ToString();
        }

        private void GameDismantledMinesChanged(object sender, EventArgs e) {
            labelBombs.Text = (field.GetMines - field.DismantledMines).ToString();
        }

        private void button2_Click(object sender, EventArgs e) {
            field.AddMine();            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            field = new MineField(this.panel1, 10, 10, 2, 6, pbmineadd);
        }
    }
}