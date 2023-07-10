using Ex02.ConsoleUtils;

namespace projectXmixDrix
{
    public class UIBoard
    {
        private readonly int r_BoardSize;
        private char[,] m_PresentedBoard;

        public UIBoard(int size)
        {
            r_BoardSize = size;
            m_PresentedBoard = new char[size, size];
            initBoardWithEmptyCells();
        }

        public char[,] Board
        {
            get
            {
                return m_PresentedBoard;
            }
        }

        private void initBoardWithEmptyCells()
        {
            for (int i = 0; i < r_BoardSize; i++)
            {
                for (int j = 0; j < r_BoardSize; j++)
                {
                    m_PresentedBoard[i, j] = ' ';
                }
            }
        }

        public void PrintBoard()
        {
            Screen.Clear();
            printColumnNumbers();
            for (int row = 0; row < r_BoardSize; row++)
            {
                printRowNumbersAndCells(row);
                printHorizontalLine(row);
            }
        }

        private void printColumnNumbers()
        {
            System.Console.Write("    ");
            for (int j = 0; j < r_BoardSize; j++)
            {
                System.Console.Write(" {0}  ", j + 1);
            }

            System.Console.WriteLine();
        }

        private void printRowNumbersAndCells(int i_Row)
        {
            for (int j = 0; j < r_BoardSize; j++)
            {
                if (j == 0)
                {
                    System.Console.Write(" {0} ", i_Row + 1);
                }

                System.Console.Write("| {0} ", m_PresentedBoard[i_Row, j]);
            }

            System.Console.Write("|");
            System.Console.WriteLine();
        }

        private void printHorizontalLine(int i_Row)
        {
            if (i_Row < r_BoardSize)
            {
                System.Console.Write("   ");
                for (int j = 0; j < r_BoardSize; j++)
                {
                    if (j == 0)
                    {
                        System.Console.Write("=====");
                    }
                    else
                    {
                        System.Console.Write("====");
                    }

                }

                System.Console.WriteLine();
            }
        }

        public void UpdateBoard(Point i_Point, char i_Symbol)
        {
            m_PresentedBoard[i_Point.Y - 1, i_Point.X - 1] = i_Symbol;
        }
    }
}
