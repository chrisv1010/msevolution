using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MSEvolution {
    public partial class MSEvolution : Form {
        private MineField field;
        
        public MSEvolution() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            field = new MineField(this.panel1, 10, 10, 10);            
            field.NewGame();
            MessageBox.Show(field.Print());
            field.Tick += new EventHandler(GameTick);
            field.DismantledMinesChanged += new EventHandler(GameDismantledMinesChanged);
            field.Start();
        }

        private void GameTick(object sender, EventArgs e) {
            labelTime.Text = field.Time.ToString();
        }

        private void GameDismantledMinesChanged(object sender, EventArgs e) {
            labelBombs.Text = (field.Mines - field.DismantledMines).ToString();
        }
    }
}