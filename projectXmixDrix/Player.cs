namespace projectXmixDrix
{
    public class Player
    {
        private CellState m_Mark;
        private int m_Score = 0;

        public Player(CellState i_Mark)
        {
            m_Mark = i_Mark;
        }

        public CellState Mark
        {
            get
            {
                return m_Mark;
            }

            set
            {
                m_Mark = value;
            }
        }

        public int Score
        {
            get
            {
                return m_Score;
            }

            set
            {
                m_Score = value;
            }
        }
    }
}
