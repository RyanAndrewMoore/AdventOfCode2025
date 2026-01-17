var machines = File.ReadAllLines(args[0]);
var totalNumButtonsPressed = 0L;

foreach (var machine in machines)
{
    var (target, buttons) = ParseMachine(machine);
    var numButtonsPressed = 1;

    while (!buttons.Contains(target))
    {
        PressButtons(buttons);
        numButtonsPressed++;
    }

    totalNumButtonsPressed += numButtonsPressed;
}

Console.WriteLine($"Part 1: {totalNumButtonsPressed}");

return;

static (int, List<int>) ParseMachine(string machine)
{
    var startTarget = machine.IndexOf('[') + 1;
    var endTarget = machine.IndexOf(']') - 1;

    var targetString = machine[startTarget..endTarget];
    var target = 0;

    foreach (var c in targetString)
    {
        target *= 2;
        target += c == '#' ? 1 : 0;
    }

    var buttonsStart = machine.IndexOf('(');
    var buttonsEnd = machine.LastIndexOf(')');
    var buttonsString = machine[buttonsStart..buttonsEnd];

    var buttons = buttonsString.Split(' ')
        .Select(s =>
            s[1..^1].Split(',')
                .Select(int.Parse)
                .Sum(i => (int)Math.Pow(2, i))
        )
        .ToList();

    return (target, buttons);
}

static void PressButtons(List<int> buttons)
{
    var buttonsCopy = new int[buttons.Count];
    buttons.CopyTo(buttonsCopy);

    buttons.AddRange(
        buttonsCopy.SelectMany(
            (_, i) => buttons.Skip(i + 1),
            (outerButton, innerButton) => outerButton ^ innerButton
        )
    );

    buttons = buttons.Distinct().ToList();
}