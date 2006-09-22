using System;
using System.Drawing;
using System.Windows.Forms;

namespace MSEvolution {
    public class MineField {

        private int[,] DataField = null;
        public event EventHandler DismantledMinesChanged;
        public event EventHandler Tick;

        private int dismantledMines;
        private int height;
        private int width;
        private int incorrectdismantledMines;
        private int mines;
        private Panel panel;
        private Field[,] squares;
        private Timer timer;
        public int Time;
        private int interval;
        private ProgressBar pb;



        public MineField(Panel panel, int width, int height, int mines, int interval, ProgressBar pb) {
            this.panel = panel;
            this.width = width;
            this.height = height;
            this.mines = mines;
            this.interval = interval;
            this.pb = pb;
        }

        public void NewGame() {
            GenerateField(mines);
            Start();
        }

        private void GenerateField(int mines) {
            DataField = ResetField();
            while (mines > 0) {
                int[] Cords = GetRandom();
                if (!IsMine(Cords[0], Cords[1])) {
                    PlaceMine(Cords[0], Cords[1]);
                    mines--;
                }
            }
        }

        private void PlaceMine(int x, int y) {
            DataField[x, y] = -1;
            //2,2   1,1 1,2 1,3 2,1 2,3 3,1 3,2 3,3
            if (CheckField(x - 1, y - 1)) DataField[x - 1, y - 1] += 1;
            if (CheckField(x - 1, y)) DataField[x - 1, y] += 1;
            if (CheckField(x - 1, y + 1)) DataField[x - 1, y + 1] += 1;
            if (CheckField(x, y - 1)) DataField[x, y - 1] += 1;
            if (CheckField(x, y + 1)) DataField[x, y + 1] += 1;
            if (CheckField(x + 1, y - 1)) DataField[x + 1, y - 1] += 1;
            if (CheckField(x + 1, y)) DataField[x + 1, y] += 1;
            if (CheckField(x + 1, y + 1)) DataField[x + 1, y + 1] += 1;
        }

        private bool CheckField(int x, int y) {
            if (x >= 0 && x < width) {
                if (y >= 0 && y < height) {
                    if (!IsMine(x, y)) {
                        return true;
                    }
                }
            }
            return false;
        }


        private bool IsMine(int x, int y) {
            return DataField[x, y] == -1 ? true : false;
        }

        private int[] GetRandom() {
            Random rand = new Random();
            return new int[] { rand.Next(0, width - 1), rand.Next(0, height - 1) };
        }

        private int[,] ResetField() {
            int[,] Field = new int[height, width];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    Field[i, j] = 0;
                }
            }
            return Field;
        }

        public int GetCount(int x, int y) {
            return DataField[x, y];
        }

        private void Dismantle(object sender, EventArgs e) {
            Field s = (Field)sender;
            if (s.GetDismantled) {
                if (s.GetMinded) {
                    dismantledMines++;
                }
                else {
                    incorrectdismantledMines++;
                }
            }
            else {
                if (s.GetMinded) {
                    dismantledMines--;
                }
                else {
                    incorrectdismantledMines--;
                }
            }

            OnDismantledMinesChanged();

            if (dismantledMines == mines) {
                timer.Enabled = false;
                panel.Enabled = false;
            }
        }

        public int DismantledMines {
            get { return dismantledMines + incorrectdismantledMines; }
        }

        private void Explode(object sender, EventArgs e) {
            //Panel.Enabled = false;
            timer.Enabled = false;

            foreach (Field s in squares) {
                s.RemoveEvents();
                if (s.GetMinded) {
                    s.GetButton.Text = "*";
                    //s.GetButton.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
                    s.GetButton.ForeColor = Color.Black;
                }
                //				if (!s.Dismantled && s.Minded && s != sender) {
                //
                //					s.Button.BackColor = Color.Orange;
                //				}
            }
        }

        public int GetHeight {
            get { return (this.height); }
        }

        public int GetMines {
            get { return (this.mines); }
        }


        public int GetWidth {
            get { return (this.width); }
        }

        protected void OnDismantledMinesChanged() {
            if (DismantledMinesChanged != null) {
                DismantledMinesChanged(this, new EventArgs());
            }
        }

        protected void OnTick() {
            if (Tick != null) {
                Tick(this, new EventArgs());
                //int divvalue = 
                int modvalue = Time % interval;
                if (modvalue == 0 && Time > 0) {
                    pb.Value = 0;
                    AddMine();
                }
                else {
                    int divvalue = Time * 100 / interval;
                    if (divvalue > 100) pb.Value = divvalue % 100;
                    else pb.Value = divvalue;
                }
            }
        }

        public void OpenSpot(int x, int y) {
            if (x >= 0 && x < width) {
                if (y >= 0 && y < height) {
                    squares[x, y].Open();
                }
            }
        }

        public Panel GetPanel {
            get { return (this.panel); }
        }

        public void Start() {

            //Panel.SuspendLayout();

            Time = 0;
            dismantledMines = 0;
            incorrectdismantledMines = 0;
            OnTick();
            panel.Controls.Clear();

            // Create Spots
            squares = new Field[width, height];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Field s = new Field(this, x, y);
                    s.Explode += new EventHandler(Explode);
                    s.Dismantle += new EventHandler(Dismantle);
                    s.GetMinded = IsMine(x, y);
                    squares[x, y] = s;
                }
            }


            OnDismantledMinesChanged();

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerTick);
            timer.Enabled = true;
            panel.Show();
        }

        private void TimerTick(object sender, EventArgs e) {
            Time++;
            OnTick();
        }

        //public string Print() {
        //    string s = "";
        //    for (int i = 0; i < height; i++) {
        //        for (int j = 0; j < width; j++) {
        //            //s += "|" + DataField[i, j].ToString();
        //            s += "|" + squares[i, j].GetButton.FlatStyle.ToString();
        //        }
        //        s += "\n";
        //    }
        //    return s;
        //}

        public void AddMine() {
            int[] Cords;
            while (true) {
                Cords = GetRandom();
                if (!IsMine(Cords[0], Cords[1])) {
                    break;
                }
            }
            int x = Cords[0];
            int y = Cords[1];

            PlaceMine(x, y);
            
                       
            mines++;
            squares[x, y].MakeMine();
            HiddenFields(x - 1, y - 1);
            HiddenFields(x - 0, y - 1);
            HiddenFields(x + 1, y - 1);
            HiddenFields(x - 1, y - 0);
            HiddenFields(x + 1, y - 0);
            HiddenFields(x - 1, y + 1);
            HiddenFields(x - 0, y + 1);
            HiddenFields(x + 1, y + 1);
            OnDismantledMinesChanged();
        }

        public void HiddenFields(int x, int y) {
            if (x >= 0 && x < width) {
                if (y >= 0 && y < height) {
                    squares[x, y].HideField();
                }
            }
        }
    }
}
