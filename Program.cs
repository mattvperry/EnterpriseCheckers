namespace EnterpriseCheckers
{
    using StructureMap;

    /// <summary>
    /// Console application class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point
        /// </summary>
        /// <param name="args">Command line args</param>
        static void Main(string[] args)
        {
            IContainer container = ConfigureDependencies();

            IGame game = container.GetInstance<IGame>();
            game.Run();
        }

        /// <summary>
        /// Configure inversion of control container
        /// </summary>
        /// <returns>IoC container</returns>
        private static IContainer ConfigureDependencies()
        {
            return new Container(c =>
            {
                c.For<IGame>().Use<Game>();
                c.For<IGameState>().Use<GameState>();
                c.For<IBoard>().Use<Board>();
            });
        }
    }
}
