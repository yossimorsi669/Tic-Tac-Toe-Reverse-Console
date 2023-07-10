namespace projectXmixDrix
{
    public class UIgeneral
    {
        private GameLogic m_Game;
        private UIplayer m_Player1;
        private UIplayer m_Player2;
        private UIBoard m_PresentedBoard;
        private int m_BoardSize;
        private int m_GameMode;

        public void InitialNewGame()
        {
            m_BoardSize = GetBoardSizeFromUser();
            m_PresentedBoard = new UIBoard(m_BoardSize);
            m_GameMode = GetGameModeFromUser();
            m_Player1 = new UIplayer(1, m_GameMode);
            m_Player2 = new UIplayer(2, m_GameMode, m_Player1.PlayerSymbol);
            m_Game = new GameLogic(m_BoardSize);
            m_PresentedBoard.PrintBoard();
        }

        public void InitialNewRound()
        {
            m_Game.ClearBoardForNewRound();
            m_Game.TurnCount = 0;
            m_PresentedBoard = new UIBoard(m_BoardSize);
            m_PresentedBoard.PrintBoard();
        }

        public void StartGame()
        {
            bool isWinner = false;
            bool isTie = false;
            bool isQuit = false;
            m_Game.IsGameOver = false;
            while (!m_Game.IsGameOver && !isQuit)
            {
                string playerMove;
                playerMove = m_Player1.GetPlayerMove(m_BoardSize, m_PresentedBoard.Board);
                if (playerMove.ToUpper() == "Q")
                {
                    printQuitMessageAndNewScore(m_Player1.PlayerName);
                    isQuit = true;
                    continue;
                }

                Point currentPoint = convertStringNumToPoint(playerMove);
                m_Game.MakeMove(CellState.Player1, currentPoint, ref isWinner, ref isTie);
                m_PresentedBoard.UpdateBoard(currentPoint, m_Player1.PlayerSymbol);
                m_PresentedBoard.PrintBoard();
                printWinnerOrTieMessagesAndNewScore(isWinner, isTie, m_Player2.PlayerName);
                if (!m_Game.IsGameOver && m_GameMode == 2)
                {
                    playerMove = m_Player2.GetPlayerMove(m_BoardSize, m_PresentedBoard.Board);
                    if (playerMove.ToUpper() == "Q")
                    {
                        printQuitMessageAndNewScore(m_Player2.PlayerName);
                        isQuit = true;
                        continue;
                    }

                    currentPoint = convertStringNumToPoint(playerMove);
                    m_Game.MakeMove(CellState.Player2, currentPoint, ref isWinner, ref isTie);
                    m_PresentedBoard.UpdateBoard(currentPoint, m_Player2.PlayerSymbol);
                    m_PresentedBoard.PrintBoard();
                    printWinnerOrTieMessagesAndNewScore(isWinner, isTie, m_Player1.PlayerName);
                }
                else if (!m_Game.IsGameOver)
                {
                    playerMove = m_Game.GetComputerMove();
                    currentPoint = convertStringNumToPoint(playerMove);
                    m_Game.MakeMove(CellState.Player2, currentPoint, ref isWinner, ref isTie);
                    m_PresentedBoard.UpdateBoard(currentPoint, m_Player2.PlayerSymbol);
                    m_PresentedBoard.PrintBoard();
                    printWinnerOrTieMessagesAndNewScore(isWinner, isTie, m_Player1.PlayerName);
                }
            }
        }
        
        public bool UserWantNewRound()
        {
            int newRoundNum = 0;
            bool isNewRound = false;

            while (!isValidNum(newRoundNum))
            {
                System.Console.WriteLine("whould you like to play another round? For YES press 1," +
                        "for NO press 2");
                string input = System.Console.ReadLine();

                if (int.TryParse(input, out newRoundNum))
                {
                    if (!isValidNum(newRoundNum))
                    {
                        System.Console.WriteLine("Invalid Input. Please enter 1 for new round or 2 for end");
                    }
                }
            }

            if (newRoundNum == 1)
            {
                isNewRound = true;
            }

            return isNewRound;
        }

        private bool isValidNum(int i_NewRoundNum)
        {
            return i_NewRoundNum == 1 || i_NewRoundNum == 2;
        }
        
        private void printQuitMessageAndNewScore(string i_PlayerName)
        {
            System.Console.WriteLine("{0} quit the game", i_PlayerName);
            System.Console.WriteLine("SCORES:   {0}: {1}     ,    {2}: {3}", m_Player1.PlayerName, m_Player1.PlayerScore, m_Player2.PlayerName, m_Player2.PlayerScore);
        }

        private void printWinnerOrTieMessagesAndNewScore(bool i_IsWinner, bool i_IsTie, string i_PlayerName)
        {
            if (i_IsWinner)
            {
                if(i_PlayerName == m_Player1.PlayerName)
                {
                    m_Player1.PlayerScore++;
                }
                else
                {
                    m_Player2.PlayerScore++;
                }

                if(m_GameMode == 1 && i_PlayerName == "PC")
                {
                    System.Console.WriteLine("You lost!! Better luck next time (:");
                }
                else
                {
                    System.Console.WriteLine("Congratulation {0} you are the WINNER!!!", i_PlayerName);
                }  

                System.Console.WriteLine("SCORES:   {0}: {1}     ,    {2}: {3}", m_Player1.PlayerName, m_Player1.PlayerScore, m_Player2.PlayerName, m_Player2.PlayerScore);
            }

            if (i_IsTie)
            {
                m_Player1.PlayerScore++;
                m_Player2.PlayerScore++;
                System.Console.WriteLine("The game ended in a draw!");
                System.Console.WriteLine("SCORES:   {0}: {1}     ,    {2}: {3}", m_Player1.PlayerName, m_Player1.PlayerScore, m_Player2.PlayerName, m_Player2.PlayerScore);
            }
        }

        private Point convertStringNumToPoint(string i_StringNum)
        {
            int row, column;
            string[] moveParts = i_StringNum.Split(' ');
            row = int.Parse(moveParts[0]);
            column = int.Parse(moveParts[1]);
            Point newPoint = new Point(column, row);
            return newPoint;
        }

        public void printWelcome()
        {
            System.Console.WriteLine("=====================================================");
            System.Console.WriteLine("=====    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    =====");
            System.Console.WriteLine("======== Welcome to The Opposite X Mix Drix! ========");
            System.Console.WriteLine("=====    ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~    =====");
            System.Console.WriteLine("=====================================================");
        }

        public int GetBoardSizeFromUser()
        {
            int boardSize = 0;

            while (!IsValidBoardSize(boardSize))
            {
                System.Console.Write("Enter the size of the game board (3-9): ");
                string input = System.Console.ReadLine();

                if (int.TryParse(input, out boardSize))
                {
                    if (!IsValidBoardSize(boardSize))
                    {
                        System.Console.WriteLine("Invalid board size. Please enter a number between 3 and 9.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid input. Please enter a number between 3 and 9.");
                }
            }

            System.Console.WriteLine("You have chosen a {0}x{0} game board.", boardSize);
            return boardSize;
        }

        private bool IsValidBoardSize(int i_BoardSize)
        {
            return i_BoardSize >= 3 && i_BoardSize <= 9;
        }

        public int GetGameModeFromUser()
        {
            int gameMode = 0;

            while (!isValidGameMode(gameMode))
            {
                System.Console.Write("Enter 1 to play against the computer, or 2 to play against another player: ");
                string input = System.Console.ReadLine();

                if (int.TryParse(input, out gameMode))
                {
                    if (!isValidGameMode(gameMode))
                    {
                        System.Console.WriteLine("Invalid input. Please enter 1 or 2.");
                    }
                }
                else
                {
                    System.Console.WriteLine("Invalid input. Please enter 1 or 2.");
                }
            }

            return gameMode;
        }

        private bool isValidGameMode(int i_GameMode)
        {
            return i_GameMode == 1 || i_GameMode == 2;
        }
    }
}