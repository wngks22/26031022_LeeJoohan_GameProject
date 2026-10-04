class GameTexture : IDisposable
{
	private readonly G2Texture texture;
	private readonly float width;
	private readonly float height;

	public GameTexture(string filePath)
	{
		texture = new G2Texture(filePath);

		string fullPath = G2Util.FindFilePath(filePath);
		using System.Drawing.Image image = System.Drawing.Image.FromFile(fullPath);
		width = image.Width;
		height = image.Height;
	}

	public void DrawCenter(float centerX, float centerY)
	{
		float x = centerX - width / 2.0f;
		float y = centerY - height / 2.0f;
		texture.Draw(x, y);
	}

	public void DrawCenter(float centerX, float centerY, float scale)
	{
		float x = centerX - width * scale / 2.0f;
		float y = centerY - height * scale / 2.0f;
		texture.Draw(new Vortice.Mathematics.Rect(x, y, width * scale, height * scale),
			new Vortice.Mathematics.Rect(0, 0, width, height));
	}

	public void Dispose()
	{
		texture.Dispose();
	}
}
