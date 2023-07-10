namespace projectXmixDrix
{
    public class GameLogic
    {
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private LogicBoard m_Board;
        private int m_BoardSize;
        private int m_TurnCount = 0;
        private bool m_IsGameOver = false;
        
        public GameLogic(int i_Size)
        {
            m_BoardSize = i_Size;
            m_Board = new LogicBoard(m_BoardSize);
            r_Player1 = new Player(CellState.Player1);
            r_Player2 = new Player(CellState.Player2);
        }

        public void ClearBoardForNewRound()
        {
            m_Board = new LogicBoard(m_BoardSize);
        }

        public bool IsGameOver
        {
            get
            {
                return m_IsGameOver;
            }

            set
            {
                m_IsGameOver = value;
            }
        }

        public int TurnCount
        {
            get
            {
                return m_TurnCount;
            }

            set
            {
                m_TurnCount = value;
            }
        }

        public void MakeMove(CellState i_PlayerNum, Point i_ChoosenPoint, ref bool io_IsWinner,ref bool io_IsTie)
        {
            string message = string.Empty;
            if (m_Board.isValidPoint(i_ChoosenPoint, ref message))
            {
                m_Board.UpdateLogicBoard(i_ChoosenPoint, i_PlayerNum);
                m_TurnCount++;
            }

            if(IsGameWon(i_PlayerNum, i_ChoosenPoint))
            {
                if(i_PlayerNum == CellState.Player1)
                {
                    r_Player2.Score++;
                }
                else
                {
                    r_Player1.Score++;
                }

                io_IsWinner = true;
                m_IsGameOver = true; 
            }
            else if(IsGameTie())
            {
                r_Player1.Score++;
                r_Player2.Score++;
                io_IsTie = true;
                m_IsGameOver = true;
            }
        }

        public string GetComputerMove() // AI SMART MOVE OF COMPUTER (IN THE BTTM OF THIS CLASS)
        {
            int[,] cellScores = new int[m_BoardSize, m_BoardSize];
            CalculateCellScores(cellScores);
            return GetBestMoveFromScores(cellScores);
        }
       
        public bool IsGameTie()
        {
            return m_TurnCount == m_BoardSize * m_BoardSize; 
        }

        public bool IsGameWon(CellState i_Player, Point i_CurrentPoint)
        {
            bool isGameWon = false;
            isGameWon = isGameWon || checkWinInCol(i_Player, i_CurrentPoint) || checkWinInRow(i_Player, i_CurrentPoint)
                         || checkWinInDiagonal(i_Player, i_CurrentPoint) || checkWinIn2ndDiagonal(i_Player, i_CurrentPoint);
            return isGameWon;
        }

        private bool checkWinInCol(CellState i_Player, Point i_Point)
        {
            bool isGameWon = false;
            int countStreak = 0;

            if (m_TurnCount >= m_BoardSize)
            {
                foreach (int i in System.Linq.Enumerable.Range(0, m_BoardSize))
                {
                    if (m_Board.Board[i, i_Point.X - 1] != i_Player)
                    {
                        break;
                    }
                    else
                    {
                        countStreak++;
                        if (countStreak == m_BoardSize)
                        {
                            isGameWon = true;
                        }
                    }
                }
            }

               return isGameWon;
        }

        private bool checkWinInRow(CellState i_Player, Point i_Point)
        {
            bool isGameWon = false;
            int countStreak = 0;
            if (m_TurnCount >= m_BoardSize)
            {
                foreach (int i in System.Linq.Enumerable.Range(0, m_BoardSize))
                {
                    if (m_Board.Board[i_Point.Y - 1, i] != i_Player)
                    {
                        break;
                    }
                    else
                    {
                        countStreak++;
                        if (countStreak == m_BoardSize)
                        {
                            isGameWon = true;
                        }
                    }
                }
            }

            return isGameWon;
        }

        private bool checkWinInDiagonal(CellState i_Player, Point i_Point)
        {
            bool isGameWon = false;
            int countStreak = 0;

            if (m_TurnCount >= m_BoardSize && i_Point.Y == i_Point.X)
            {
                foreach (int i in System.Linq.Enumerable.Range(0, m_BoardSize))
                {
                    if (m_Board.Board[i, i] != i_Player)
                    {
                        break;
                    }
                    else
                    {
                        countStreak++;
                        if (countStreak == m_BoardSize)
                        {
                            isGameWon = true;
                        }
                    }
                }
            }

            return isGameWon;
        }

        private bool checkWinIn2ndDiagonal(CellState i_Player, Point i_Point)
        {
            bool isGameWon = false;
            int countStreak = 0;

            if (m_TurnCount >= m_BoardSize && i_Point.Y + i_Point.X - 1 == m_BoardSize)
            {
                foreach (int i in System.Linq.Enumerable.Range(0, m_BoardSize))
                {
                    int j = m_BoardSize - 1 - i;
                    if (m_Board.Board[i, j] != i_Player)
                    {
                        break;
                    }
                    else
                    {
                        countStreak++;
                        if (countStreak == m_BoardSize)
                        {
                            isGameWon = true;
                        }
                    }
                }
            }

            return isGameWon;
        }

        private string GetBestMoveFromScores(int[,] io_CellScores)
        {
            int maxScore = -(m_BoardSize * m_BoardSize);
            int maxRow = 0, maxCol = 0, rowNum = 0, colNum = -1;

            foreach (var cell in io_CellScores)
            {
                colNum++;
                if (colNum == m_BoardSize)
                {
                    colNum = 0;
                    rowNum++;
                }

                if (cell > maxScore && m_Board.Board[rowNum, colNum] == CellState.Empty)
                {
                    maxScore = cell;
                    maxRow = rowNum;
                    maxCol = colNum;
                }
            }

            // Convert the cell coordinates to move string
            string bestMove = $"{maxRow + 1} {maxCol + 1}";

            return bestMove;
        }

        // The main AI to calculate better computer move
        private void CalculateCellScores(int[,] io_CellScoresMatrix)
        {
             /*here we calculate the scores of each cell int the matrix with the same size of the game board when each
             cell represent the same cell number in the game board. the cell that will get the maximum value it will
             be the computer move*/

            // Iterate over the board and update the scores
            for (int row = 0; row < m_BoardSize; row++)
            {
                for (int col = 0; col < m_BoardSize; col++)
                {
                    if (m_Board.Board[row, col] != CellState.Empty)
                    {  
                        /* with those bool param we check if in the same path that possible for sequence marks by both players
                        cus then it will be a good move for the computer (of course considering other scenarios that will
                        be count later int the function */
                        bool hasOtherPlayerMarkInRow = HasOtherPlayerMark(m_Board.Board[row, col], row, 0, 0, 1);
                        bool hasOtherPlayerMarkInColumn = HasOtherPlayerMark(m_Board.Board[row, col], 0, col, 1, 0);
                        bool hasOtherPlayerMarkInDiagonal = HasOtherPlayerMark(m_Board.Board[row, col], 0, 0, 1, 1);
                        bool hasOtherPlayerMarkInReverseDiagonal = HasOtherPlayerMark(m_Board.Board[row, col], 0, m_BoardSize - 1, 1, -1);
                        /* giving bad points for choose move in the same row or col or diagnoals of each player, 
                           if its the computer it will get -2 points if its the user it will get -1 point */
                        for (int i = 0; i < m_BoardSize; i++)
                        {
                            if (m_Board.Board[row, i] == CellState.Empty && !hasOtherPlayerMarkInRow)
                            {
                                if (m_Board.Board[row, col] == r_Player2.Mark)
                                {
                                    io_CellScoresMatrix[row, i] -= 2;
                                }
                                else
                                {
                                    io_CellScoresMatrix[row, i]--;
                                }
                            }
                            else if (m_Board.Board[row, i] == CellState.Empty)
                            {
                                io_CellScoresMatrix[row, i] = io_CellScoresMatrix[row, i];
                            }

                            if (m_Board.Board[i, col] == CellState.Empty && !hasOtherPlayerMarkInColumn)
                            {
                                if (m_Board.Board[row, col] == r_Player2.Mark)
                                {
                                    io_CellScoresMatrix[i, col] -= 2;
                                }
                                else
                                {
                                    io_CellScoresMatrix[i, col]--;
                                }
                            }
                            else if (m_Board.Board[i, col] == CellState.Empty)
                            {
                                continue;
                            }
                        }

                        if (row == col)
                        {
                            for (int i = 0; i < m_BoardSize; i++)
                            {
                                if (m_Board.Board[i, i] == CellState.Empty && !hasOtherPlayerMarkInDiagonal)
                                {
                                    if (m_Board.Board[row, col] == r_Player2.Mark)
                                    {
                                        io_CellScoresMatrix[i, i] -= 2;
                                    }
                                    else
                                    {
                                        io_CellScoresMatrix[i, i]--;
                                    }
                                }
                                else if (m_Board.Board[i, i] == CellState.Empty)
                                {
                                    continue;
                                }
                            }
                        }

                        if (row + col == m_BoardSize - 1)
                        {
                            for (int i = 0; i < m_BoardSize; i++)
                            {
                                if (m_Board.Board[i, m_BoardSize - 1 - i] == CellState.Empty && !hasOtherPlayerMarkInReverseDiagonal)
                                {
                                    if (m_Board.Board[row, col] == r_Player2.Mark)
                                    {
                                        io_CellScoresMatrix[i, m_BoardSize - 1 - i] -= 2;
                                    }
                                    else
                                    {
                                        io_CellScoresMatrix[i, m_BoardSize - 1 - i]--;
                                    }
                                }
                                else if (m_Board.Board[i, m_BoardSize - 1 - i] == CellState.Empty)
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool HasOtherPlayerMark(CellState i_CurrentPlayer, int i_StartRow, int i_StartCol, int i_RowIncrement, int i_ColIncrement)
        {
            for (int i = 0; i < m_BoardSize; i++)
            {
                int row = i_StartRow + (i * i_RowIncrement);
                int col = i_StartCol + (i * i_ColIncrement);

                if (m_Board.Board[row, col] != CellState.Empty && m_Board.Board[row, col] != i_CurrentPlayer)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
