using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

public class LaunchpadManager
{
    private readonly OutputDevice _output;
    private readonly InputDevice _input;
    public event Action<Position>? PadPressed;

    //Linking the color to the respective (velocity) 
    private static readonly Dictionary<GameColors, byte> LedColors = new()
    {
        { GameColors.No_Color, 0 },
        { GameColors.Red, 5 },
        { GameColors.Orange, 9 },
        { GameColors.Yellow, 13 },
        { GameColors.Green, 21 },
        { GameColors.White, 3 },
        { GameColors.Blue, 45 },
        { GameColors.Purple, 53 },
        { GameColors.Pink, 57 },
    };

    public LaunchpadManager(string outputName, string inputName)
    {
        _output = OutputDevice.GetByName(outputName);
        _input = InputDevice.GetByName(inputName);

        _output.SendEvent(new NormalSysExEvent(
            new byte[] { 0x00, 0x20, 0x29, 0x02, 0x0D, 0x0E, 0x01, 0xF7 }));

        _input.EventReceived += OnMidiEvent;
    }

    // Check if there are some Launchpads connected
    public void StartListening()
    {
        _input.StartEventsListening();
    }

    /// <summary>
    /// Start listening all the midi notes of the Launchpad.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnMidiEvent(object? sender, MidiEventReceivedEventArgs e)
    {
        if (e.Event is NoteOnEvent noteOn && noteOn.Velocity > 0)
        {
            Position? pos = LaunchpadManager.NoteToPosition((int)noteOn.NoteNumber);
            if (pos != null)
            {
                PadPressed?.Invoke(pos);
            }
        }
    }

    /// <summary>
    /// Translate the launchpad Note to the respective Position
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    public static Position? NoteToPosition(int note)
    {
        int row = note / 10;      // 1..8, dal basso
        int col = note % 10;      // 1..8

        if (row < 1 || row > 8 || col < 1 || col > 8)
        {
            return null;
        }

        return new Position(8 - row, col - 1);
    }

    // Translate the position to the respective lauchapd note.
    public static int PositionToNote(Position pos)
    {
        return (8 - pos.Row) * 10 + (pos.Column + 1);
    }

    // Display the game's grid on the launchpad.
    public void Show(Game game)
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Position pos = new Position(r, c);
                GameColors color = game.Grid.ReturnColorFromPosition(pos);
                SetLed(pos, color);
            }
        }
    }

    private void SetLed(Position pos, GameColors color)
    {
        int note = LaunchpadManager.PositionToNote(pos);
        byte velocity = LedColors[color];

        _output.SendEvent(new NoteOnEvent((SevenBitNumber)note, (SevenBitNumber)velocity));
    }
}