class ScoreManager
{
	public int Score;
	public int Mistakes;
	public int ClearedStages;
	public double ElapsedTime;

	public void Reset()
	{
		Score = 0;
		Mistakes = 0;
		ClearedStages = 0;
		ElapsedTime = 0;
	}

	public void AddInput(bool correct)
	{
		if (correct)
			Score += 100;
		else
			Mistakes++;
	}

	public void CompleteStage(bool perfect)
	{
		ClearedStages++;
		if (perfect)
			Score += 500;
	}

	// 등급 이미지 배열의 순서: S, A, B, C, F
	public int GetGradeIndex()
	{
		if (Score >= 9000) return 0;
		if (Score >= 7500) return 1;
		if (Score >= 6000) return 2;
		if (Score >= 4000) return 3;
		return 4;
	}
}
