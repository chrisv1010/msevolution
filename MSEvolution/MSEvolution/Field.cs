using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace MSEvolution {
    class Field {
        private Button button;
        private bool flagged = false;
        private MineField field;
        private bool minded = false;
        private bool opened = false;
        private int x;
        private int y;
        public event EventHandler Dismantle;
        public event EventHandler Explode;
        

        public Field(MineField Field, int x, int y) {
            this.field = Field;
            this.x = x;
            this.y = y;
			button = new Button();
			button.Text = "";         
            
            int w = Field.GetPanel.Width / Field.GetWidth;
            int h = Field.GetPanel.Height / Field.GetHeight;

			button.Width = w + 1;
			button.Height = h + 1;
			button.Left = w * x;
			button.Top = h * y;
                        
			//button.Font = new System.Drawing.Font("Arial Black", 7, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			button.Click += new EventHandler(Click);
			button.MouseDown += new MouseEventHandler(DismantleClick);            
            Field.GetPanel.Controls.Add(button);
		}


        public Button GetButton {
            get { return (this.button); }
        }

        private void Click(object sender, System.EventArgs e) {
            if (!GetDismantled) {
                if (GetMinded) {
                    button.BackColor = Color.Red;
                    OnExplode();
                }
                else {
                    this.Open();
                }
            }
        }

        private void DismantleClick(object sender, MouseEventArgs e) {
            if (!Opened && e.Button == MouseButtons.Right) {
                if (GetDismantled) {
                    flagged = false;
                    button.BackColor = SystemColors.Control;
                    button.Text = "?";
                }
                else {
                    flagged = true;
                    button.Text = "";
                    button.BackColor = Color.Green;
                }
                OnDismantle();
            }
        }

        public bool GetDismantled {
            get { return (this.flagged); }
        }

        public bool GetMinded {
            get { return (this.minded); }
            set { this.minded = value; }
        }

        protected void OnDismantle() {
            if (Dismantle != null) {
                Dismantle(this, new EventArgs());
            }
        }

        protected void OnExplode() {
            if (Explode != null) {
                Explode(this, new EventArgs());
            }
        }

        public void Open() {
            if (!Opened && !GetDismantled) {
                opened = true;
                // Count Bombs
                int c = field.GetCount(x, y);

                if (c > 0) {
                    button.Text = c.ToString();
                    switch (c) {
                        case 1:
                            button.ForeColor = Color.Blue;
                            break;
                        case 2:
                            button.ForeColor = Color.Green;
                            break;
                        case 3:
                            button.ForeColor = Color.Red;
                            break;
                        case 4:
                            button.ForeColor = Color.DarkBlue;
                            break;
                        case 5:
                            button.ForeColor = Color.DarkRed;
                            break;
                        case 6:
                            button.ForeColor = Color.LightBlue;
                            break;
                        case 7:
                            button.ForeColor = Color.Orange;
                            break;
                        case 8:
                            button.ForeColor = Color.Ivory;
                            break;
                    }
                }
                else {
                    button.BackColor = SystemColors.ControlLight;
                    button.FlatStyle = FlatStyle.Flat;
                    button.Enabled = false;

                    field.OpenSpot(x - 1, y - 1);
                    field.OpenSpot(x - 0, y - 1);
                    field.OpenSpot(x + 1, y - 1);
                    field.OpenSpot(x - 1, y - 0);
                    field.OpenSpot(x - 0, y - 0);
                    field.OpenSpot(x + 1, y - 0);
                    field.OpenSpot(x - 1, y + 1);
                    field.OpenSpot(x - 0, y + 1);
                    field.OpenSpot(x + 1, y + 1);
                }
            }
        }

        public bool Opened {
            get { return (this.opened); }
        }

        public void RemoveEvents() {
            button.Click -= new EventHandler(Click);
            button.MouseDown -= new MouseEventHandler(DismantleClick);
        }

        public void MakeMine() {
            //button.BackColor = SystemColors.Control;
            button.FlatStyle = FlatStyle.System;
            button.Enabled = true;
            minded = true;
            opened = false;
            button.Text = "";
            flagged = false;
        }

        public void HideField() {
            //button.BackColor = SystemColors.Control;
            button.FlatStyle = FlatStyle.System;
            button.Enabled = true;
            opened = false;
            button.Text = "";
            flagged = false;
        }

    }
}
