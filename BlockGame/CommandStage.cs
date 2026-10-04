enum Direction
{
	Left,
	Down,
	Up,
	Right
}

enum CommandState
{
	Pending,
	Correct,
	Wrong
}

class CommandStage
{
	private Random random = new Random();
	public Direction[] Directions = new Direction[0];
	public CommandState[] States = new CommandState[0];
	public int CurrentIndex;
	public bool IsPerfect;

	public void Start(int stageNumber)
	{
		int count = 3 + (stageNumber - 1) / 2;
		Directions = new Direction[count];
		States = new CommandState[count];
		CurrentIndex = 0;
		IsPerfect = true;
		for (int i = 0; i < count; i++)
		{
			Directions[i] = (Direction)random.Next(4);
			States[i] = CommandState.Pending;
		}
	}

	public bool Input(Direction direction)
	{
		bool correct = Directions[CurrentIndex] == direction;
		if (correct)
		{
			States[CurrentIndex] = CommandState.Correct;
		}
		else
		{
			States[CurrentIndex] = CommandState.Wrong;
			IsPerfect = false;
		}
		CurrentIndex++;
		return correct;
	}

	public bool IsFinished()
	{
		return CurrentIndex == Directions.Length;
	}
}
