namespace projectXmixDrix
{
    public class UIplayer
    {
        private readonly string r_PlayerName;
        private readonly char r_PlayerSymbol;
        private int m_PlayerScore = 0;

        public UIplayer(int i_PlayerNumber, int i_GameMode, char i_OtherPlayerSymbol = ' ')
        {
            r_PlayerName = SetPlayerName(i_PlayerNumber, i_GameMode);
            if (i_OtherPlayerSymbol != ' ')
            {
                r_PlayerSymbol = i_OtherPlayerSymbol == 'X' ? 'O' : 'X';
            }
            else
            {
                r_PlayerSymbol = SetPlayerSymbol();
            }
        }

        public int PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public char PlayerSymbol
        {
            get
            {
                return r_PlayerSymbol;
            }
        }

        public string SetPlayerName(int i_PlayerNumber, int i_GameMode)
        {
            string playerName;
            if (i_PlayerNumber == 2 && i_GameMode == 1)
            {
                playerName = "PC"; 
            }
            else
            {
                System.Console.Write($"Enter name for player {i_PlayerNumber}: ");
                playerName = System.Console.ReadLine();
            }

            return playerName;
        }

        public char SetPlayerSymbol()
        {
            string playerSymbol = string.Empty;
            while (!isValidSymbol(playerSymbol))
            {
                System.Console.Write("Choose your symbol (X or O): ");
                playerSymbol = System.Console.ReadLine().ToUpper();
                if (!isValidSymbol(playerSymbol))
                {
                    System.Console.Write("Invalid input! please try again: ");
                }
            }

            return char.Parse(playerSymbol);
        }

        private bool isValidSymbol(string i_PlayerSymbol)
        {
            return i_PlayerSymbol == "X" || i_PlayerSymbol == "O";
        }

        public string GetPlayerMove(int i_BoardSize, char[,] i_BoardMatrix)
        {
            System.Console.Write($"{this.PlayerName}, enter your move (row column): ");
            string move = System.Console.ReadLine();
            while (!IsValidMove(move, i_BoardSize, i_BoardMatrix))
            {
                System.Console.Write($"Invalid move. {this.PlayerName}, enter your move (row column): ");
                move = System.Console.ReadLine();
            }

            return move;
        }

        private bool IsValidMove(string i_MoveStr, int i_BoardSize, char[,] i_BoardMatrix)
        {
            bool isValid = true;
            int row, col;
            if (i_MoveStr.ToUpper() == "Q")
            {
                isValid = true;
            }
            else if (i_MoveStr.Length != 3)
            {
                isValid = false;
            }
            else if (!int.TryParse(i_MoveStr[0].ToString(), out row) || !int.TryParse(i_MoveStr[2].ToString(), out col))
            {
                isValid = false;
            }
            else if (row < 1 || row > i_BoardSize || col < 1 || col > i_BoardSize)
            {
                isValid = false;
            }
            else if (i_MoveStr[1] != ' ')
            {
                isValid = false;
            }
            else if (i_BoardMatrix[row - 1, col - 1] != ' ')
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
