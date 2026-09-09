// -------------------------------------------------------------------------------------------------------------------------------------------------------------
// Author: 3dapi (https://github.com/3dapi)
// -------------------------------------------------------------------------------------------------------------------------------------------------------------

using Vortice.Mathematics;

class GameMain : G2AppBase
{
	public override System.Drawing.Size ScreenSize => GameGlobal.ScreenSize;
	public override string GameName => GameGlobal.GameName;

	private G2Texture? _bgTexture = null;
	private G2Texture? _uiTitleTexture = null;
	private G2Texture? _uiButtonTexture = null;

	private G2Font? _fntMessage = null;


    protected override void Initialize()
	{
		//---------------------------------------
		// 게임 관련 객체를 생성합니다.
		//---------------------------------------
		var texUiDir = "resource/image/ui/";

		_bgTexture = new(texUiDir+"background.png");
        _uiTitleTexture = new(texUiDir + "menu/title.png");
		_uiButtonTexture = new(texUiDir + "menu/button.png");

		_fntMessage = new("resource/font/ChakraPetch-Bold", 42);
    }

	protected override void Update()
	{
		double elapsed = TotalTime;

		this.ClearColor = new Color4(
			red: (float)(Math.Sin(elapsed) * 0.5 + 0.5),
			green: (float)(Math.Sin(elapsed + Math.PI / 2.0) * 0.5 + 0.5),
			blue: (float)(Math.Sin(elapsed + Math.PI) * 0.5 + 0.5),
			alpha: 1.0f);

		//---------------------------------------
		// 게임 관련 객체를 갱신합니다.
		//---------------------------------------
	}

	protected override void Render()
	{
		//---------------------------------------
		// 게임 관련 객체를 렌더링 합니다.
		//---------------------------------------

		_bgTexture.Draw();
		_uiTitleTexture.Draw(257.2f, 84.3f);
		_uiButtonTexture.Draw();

		_fntMessage.DrawText("SCORE", new(20, 20, 500, 100), new(0.0f, 1.0f, 0.0f, 1.0f));
    }

	public override void Dispose()
	{
		base.Dispose();
		//---------------------------------------
		// 게임 관련 객체를 해제합니다.
		//---------------------------------------
		_bgTexture.Dispose();
		_uiTitleTexture.Dispose();
		_uiButtonTexture.Dispose();

		_fntMessage.Dispose();
    }
}
