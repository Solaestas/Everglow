using Everglow.Commons.Enums;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Everglow.Commons.VFX.Pipelines;
using static Everglow.Commons.Mechanics.MusicHelper;
using static Everglow.Commons.Utilities.MathUtils;

namespace Everglow.Commons.DeveloperContent.VFXs;

[Pipeline(typeof(WCSPipeline))]
public class VisualizedMusicTrack : Visual
{
	public SoundTrackInfo MusicCue;

	public int OldMusicPos = 0;

	public int PauseTime = 0;

	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public override void Update()
	{
		if (MusicCue.Active == false)
		{
			Active = false;
		}
		if ((int)MusicCue.WavReader.BaseStream.Position == OldMusicPos)
		{
			PauseTime++;
		}
		else
		{
			PauseTime = 0;
		}
		if(PauseTime >= 60)
		{
			Active = false;
		}
		OldMusicPos = (int)MusicCue.WavReader.BaseStream.Position;
	}

	public override void Draw()
	{
		var centerPos = Main.screenPosition + new Vector2(Main.screenWidth * 0.5f, Main.screenHeight * 0.5f);
		if (MusicCue.FFTBands is not null)
		{
			for (int j = 0; j < MusicCue.FFTBands.Length; j++)
			{
				float value = MusicCue.FFTBands[j] * 100f;
				float x = j - MusicCue.FFTBands.Length / 2f;
				x *= 2f;
				DrawLine(centerPos + new Vector2(x, value + MusicCue.Index * 100), centerPos + new Vector2(x, -value + MusicCue.Index * 100));
			}
		}
	}

	public void DrawLine(Vector2 pos0, Vector2 pos1)
	{
		float dis = (pos0 - pos1).Length();
		float disValue = dis / 100f;
		var topColor = Color.Lerp(new Color(255, 160, 229, 200), new Color(175, 255, 249, 200), 0.5f - disValue);
		var bottomColor = Color.Lerp(new Color(255, 160, 229, 200), new Color(175, 255, 249, 200), 0.5f + disValue);
		Texture2D tex = ModAsset.White.Value;
		var bars = new List<Vertex2D>();
		Vector2 dir = (pos0 - pos1).RotatedBy(MathHelper.PiOver2).NormalizeSafe();
		bars.Add(pos0 + dir, bottomColor, new Vector3(0, 0, 0));
		bars.Add(pos0 - dir, bottomColor, new Vector3(0, 0, 0));
		bars.Add(pos1 + dir, topColor, new Vector3(0, 0, 0));
		bars.Add(pos1 - dir, topColor, new Vector3(0, 0, 0));
		Ins.Batch.Draw(tex, bars, PrimitiveType.TriangleStrip);
	}
}