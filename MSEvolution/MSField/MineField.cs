using System;
using System.Collections.Generic;
using System.Text;

namespace MSField {
    public class MineField {

        private int[,] DataField = null;
        private bool[,] ViewField = null;

        public MineField() {

        }

        public void NewGame(int Mines, int Height, int Width) {
            DataField = GenerateField(Mines, Height, Width);
            ViewField = ResetViewField(Height, Width);
        }

        private int[,] GenerateField(int mines, int Height, int Width) {
            int[,] Field = ResetField(Height, Width);
            while (mines > 0) {
                int[] Cords = GetRandom(Height, Width);
                if (!IsMine(Field, Cords[0], Cords[1])) {
                    Field = PlaceMine(Field, Cords[0], Cords[1], Height, Width);
                    mines--;
                }
            }
            return null;
        }

        private int[,] PlaceMine(int[,] Field, int x, int y, int Height, int Width) {
            Field[x, y] = -1;
            //2,2   1,1 1,2 1,3 2,1 2,3 3,1 3,2 3,3
            if ((x - 1 > -1) && (y - 1 > -1)) if (!IsMine(Field, x - 1, y - 1)) Field[x - 1, y - 1] += 1;
            if (x - 1 > -1) if (!IsMine(Field, x - 1, y)) Field[x - 1, y] += 1;
            if ((x - 1 > -1) && (y + 1 < Width - 1)) if (!IsMine(Field, x - 1, y + 1)) Field[x - 1, y + 1] += 1;
            if (y - 1 > -1) if (!IsMine(Field, x, y - 1)) Field[x, y - 1] += 1;
            if (y + 1 < Width - 1) if (!IsMine(Field, x, y + 1)) Field[x, y + 1] += 1;
            if ((x + 1 < Height - 1) && (y - 1 > -1)) if (!IsMine(Field, x + 1, y - 1)) Field[x + 1, y - 1] += 1;
            if (x + 1 < Height - 1) if (!IsMine(Field, x + 1, y)) Field[x + 1, y] += 1;
            if ((x + -1 < Height - 1) && (y + 1 < Width - 1)) if (!IsMine(Field, x + 1, y + 1)) Field[x + 1, y + 1] += 1;
            return Field;
        }

        private bool IsMine(int[,] Field, int x, int y) {
            if (Field[x, y] == -1) return true;
            return false;
        }

        private int[] GetRandom(int Height, int Width) {
            Random rand = new Random();
            int randWidth = rand.Next(0, Width-1);
            int randHeight = rand.Next(0, Height-1);
            return new int[] { randWidth, randHeight };
        }

        private int[,] ResetField(int Height, int Width) {
            int[,] Field = new int[Height, Width];
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    Field[i, j] = 0;
                }
            }
            return Field;
        }

        private bool[,] ResetViewField(int Height, int Width) {
            bool[,] Field = new bool[Height, Width];
            for (int i = 0; i < Height; i++) {
                for (int j = 0; j < Width; j++) {
                    Field[i, j] = false;
                }
            }
            return Field;
        }


        public void ChangeViewState(int x, int y) {
            ViewField[x, y] = !ViewField[x, y];
        }

    }
}
