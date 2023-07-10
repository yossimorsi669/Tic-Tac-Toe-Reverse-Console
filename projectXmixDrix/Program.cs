namespace projectXmixDrix
{
    public class Program
    {
        public static void Main()
        {
            UIgeneral manager = new UIgeneral();
            manager.printWelcome();
            bool isEndGame = false;
            bool isFirstRound = true;

            while (!isEndGame)
            {
                if (isFirstRound)
                {
                    manager.InitialNewGame();
                    manager.StartGame();
                    isFirstRound = false;
                }
                else
                {
                    if (manager.UserWantNewRound())
                    {
                        manager.InitialNewRound();
                        manager.StartGame();
                    }
                    else
                    {
                        isEndGame = true;
                        System.Console.WriteLine("Thank you for playing! See you soon (: ");
                    }
                }
            }
        }
    }
}
