using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MSEvolution {
    public partial class MSEvolution : Form {
        public MSEvolution() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            MSField.MineField Field = new MSField.MineField();
            Field.NewGame(10, 10, 10);

        }
    }
}