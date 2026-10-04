using System.Windows.Forms;
using Vortice.Mathematics;

class GameplayScene : IDisposable
{
	private readonly GameMain game;
	private CommandStage stage = new CommandStage();
	private int stageNumber;
	private double endTime;
	private double nextStageTime;
	private double judgmentEndTime;
	private bool showPerfect;
	private double remainingTime;

	private GameTexture? frame;
	private GameTexture? currentFrame;
	private GameTexture? perfect;
	private GameTexture? miss;
	private readonly GameTexture?[] grayArrows = new GameTexture?[4];
	private readonly GameTexture?[] blueArrows = new GameTexture?[4];
	private readonly GameTexture?[] redArrows = new GameTexture?[4];
	private readonly G2AudioSound?[] keySounds = new G2AudioSound?[4];
	private G2AudioSound? bgm;
	private G2AudioSound? wrongSound;
	private G2AudioSound? clearSound;
	private G2AudioSound? timeoverSound;
	private G2Font? titleFont;
	private G2Font? valueFont;
	private G2Font? guideFont;

	public GameplayScene(GameMain game)
	{
		this.game = game;
	}

	public void Initialize()
	{
		frame = new GameTexture("resource/image/frame/frame.png");
		currentFrame = new GameTexture("resource/image/ui/command/current.png");
		perfect = new GameTexture("resource/image/ui/judgment/perfect.png");
		miss = new GameTexture("resource/image/ui/judgment/miss.png");

		grayArrows[(int)Direction.Left] = new GameTexture("resource/image/note/gray/left.png");
		grayArrows[(int)Direction.Down] = new GameTexture("resource/image/note/gray/down.png");
		grayArrows[(int)Direction.Up] = new GameTexture("resource/image/note/gray/up.png");
		grayArrows[(int)Direction.Right] = new GameTexture("resource/image/note/gray/right.png");

		blueArrows[(int)Direction.Left] = new GameTexture("resource/image/note/blue/left.png");
		blueArrows[(int)Direction.Down] = new GameTexture("resource/image/note/blue/down.png");
		blueArrows[(int)Direction.Up] = new GameTexture("resource/image/note/blue/up.png");
		blueArrows[(int)Direction.Right] = new GameTexture("resource/image/note/blue/right.png");

		redArrows[(int)Direction.Left] = new GameTexture("resource/image/note/red/left.png");
		redArrows[(int)Direction.Down] = new GameTexture("resource/image/note/red/down.png");
		redArrows[(int)Direction.Up] = new GameTexture("resource/image/note/red/up.png");
		redArrows[(int)Direction.Right] = new GameTexture("resource/image/note/red/right.png");

		keySounds[(int)Direction.Left] = new G2AudioSound("resource/audio/effect/left.wav");
		keySounds[(int)Direction.Down] = new G2AudioSound("resource/audio/effect/down.wav");
		keySounds[(int)Direction.Up] = new G2AudioSound("resource/audio/effect/up.wav");
		keySounds[(int)Direction.Right] = new G2AudioSound("resource/audio/effect/right.wav");
		bgm = new G2AudioSound("resource/audio/bgm/game.wav");
		wrongSound = new G2AudioSound("resource/audio/effect/wrong.wav");
		clearSound = new G2AudioSound("resource/audio/effect/clear.wav");
		timeoverSound = new G2AudioSound("resource/audio/effect/timeover.wav");
		titleFont = new G2Font("Bahnschrift", 24);
		valueFont = new G2Font("Bahnschrift", 44);
		guideFont = new G2Font("Bahnschrift", 18);
	}

	public void Start()
	{
		game.Score.Reset();
		stageNumber = 1;
		stage.Start(stageNumber);
		remainingTime = 60;
		endTime = game.TotalTime + 60;
		nextStageTime = 0;
		judgmentEndTime = 0;
		showPerfect = false;
		clearSound?.Stop();
		timeoverSound?.Stop();
		bgm?.Play(true);
	}

	public void Stop()
	{
		bgm?.Stop();
	}

	public void Update()
	{
		remainingTime = Math.Max(0, endTime - game.TotalTime);
		// 마지막 입력까지 걸린 시간을 저장한다. 완료 후 판정 표시 시간은 제외한다.
		if (game.Score.ClearedStages < 10)
			game.Score.ElapsedTime = 60 - remainingTime;
		if (remainingTime <= 0)
		{
			if (game.Score.ClearedStages < 10)
				timeoverSound?.Play();
			game.ChangeScene(SceneType.Result);
			return;
		}

		// 마지막 화살표의 색과 판정을 잠깐 보여 준 뒤 다음 스테이지로 이동한다.
		if (stage.IsFinished())
		{
			if (game.TotalTime >= nextStageTime)
			{
				if (stageNumber == 10)
					game.ChangeScene(SceneType.Result);
				else
				{
					stageNumber++;
					stage.Start(stageNumber);
				}
			}
			return;
		}

		// 키를 누른 순간만 판정한다. 누르고 있어도 다음 화살표를 처리하지 않는다.
		if (game.Input.IsKeyDown(Keys.Left)) CheckInput(Direction.Left);
		else if (game.Input.IsKeyDown(Keys.Down)) CheckInput(Direction.Down);
		else if (game.Input.IsKeyDown(Keys.Up)) CheckInput(Direction.Up);
		else if (game.Input.IsKeyDown(Keys.Right)) CheckInput(Direction.Right);
	}

	private void CheckInput(Direction direction)
	{
		bool correct = stage.Input(direction);
		game.Score.AddInput(correct);
		if (correct)
			keySounds[(int)direction]?.Play();
		else
		{
			wrongSound?.Play();
			showPerfect = false;
			judgmentEndTime = game.TotalTime + 0.5;
		}

		if (stage.IsFinished())
		{
			game.Score.CompleteStage(stage.IsPerfect);
			nextStageTime = game.TotalTime + 0.6;
			if (stage.IsPerfect)
			{
				showPerfect = true;
				judgmentEndTime = nextStageTime;
			}
			if (stageNumber == 10) clearSound?.Play();
		}
	}

	public void Draw()
	{
		DrawFrame();
		titleFont?.DrawText("SCORE", new Rect(35, 27, 190, 45), new Color4(1, 1, 1, 1));
		valueFont?.DrawText(game.Score.Score.ToString(), new Rect(35, 60, 240, 65), new Color4(1, 1, 1, 1));
		titleFont?.DrawText("STAGE " + stageNumber.ToString("00"), new Rect(565, 69, 220, 50), new Color4(1, 1, 1, 1));
		titleFont?.DrawText("TIME", new Rect(1120, 27, 150, 45), new Color4(1, 1, 1, 1));
		valueFont?.DrawText(remainingTime.ToString("0.0"), new Rect(1120, 60, 160, 65), new Color4(1, 1, 1, 1));

		float startX = 640 - (stage.Directions.Length - 1) * 42;
		for (int i = 0; i < stage.Directions.Length; i++)
		{
			// 오답이어도 원래 제시한 방향을 유지하고 색만 바꾼다.
			int direction = (int)stage.Directions[i];
			GameTexture? texture = grayArrows[direction];
			if (stage.States[i] == CommandState.Correct) texture = blueArrows[direction];
			else if (stage.States[i] == CommandState.Wrong) texture = redArrows[direction];
			float centerX = startX + i * 84;
			texture?.DrawCenter(centerX, 350, 0.55f);
			if (i == stage.CurrentIndex) currentFrame?.DrawCenter(centerX, 350, 0.55f);
		}
		if (game.TotalTime < judgmentEndTime)
		{
			if (showPerfect) perfect?.DrawCenter(640, 482);
			else miss?.DrawCenter(640, 482);
		}
		guideFont?.DrawText("CLEARED  " + game.Score.ClearedStages + " / 10", new Rect(560, 620, 250, 45), new Color4(0.7f, 0.75f, 0.85f, 1));
	}

	private void DrawFrame()
	{
		frame?.DrawCenter(640, 360);
	}

	public void Dispose()
	{
		bgm?.Dispose();
		wrongSound?.Dispose();
		clearSound?.Dispose();
		timeoverSound?.Dispose();
		foreach (G2AudioSound? sound in keySounds)
		{
			sound?.Dispose();
		}

		guideFont?.Dispose();
		valueFont?.Dispose();
		titleFont?.Dispose();
		perfect?.Dispose();
		miss?.Dispose();
		currentFrame?.Dispose();

		foreach (GameTexture? texture in blueArrows)
		{
			texture?.Dispose();
		}
		foreach (GameTexture? texture in grayArrows)
		{
			texture?.Dispose();
		}

		foreach (GameTexture? texture in redArrows)
		{
			texture?.Dispose();
		}

		frame?.Dispose();
	}
}
