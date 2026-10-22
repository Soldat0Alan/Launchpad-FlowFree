internal class Program
{
    private static void Main(string[] args)
    {
       Level lvl = new Level(3, new List<EndpointPair> {
        new(GameColors.Red,    new Position(0, 0), new Position(0, 7)), 
        new(GameColors.Green,  new Position(1, 0), new Position(7, 0)), 
        new(GameColors.Blue,   new Position(7, 1), new Position(7, 7)), 
        new(GameColors.Purple, new Position(1, 7), new Position(6, 7)),
        new(GameColors.Yellow, new Position(1, 1), new Position(6, 6)),
        new(GameColors.Orange, new Position(2, 1), new Position(6, 5))
    });

    Game game = new Game(lvl);

    var launchpad = new LaunchpadManager(
        "MIDIOUT2 (LPMiniMK3 MIDI)",    // Input Port
        "MIDIIN2 (LPMiniMK3 MIDI)");    // Output port

    launchpad.PadPressed += pos =>
    {
        game.Press(pos); 
        launchpad.Show(game);

        if (game.IsGameWon)
        {
            Console.WriteLine("You have finished this game!");
        }
    };

    launchpad.Show(game); 
    launchpad.StartListening();

    Console.WriteLine("Premi Invio per uscire.");
    Console.ReadLine();
    }
}