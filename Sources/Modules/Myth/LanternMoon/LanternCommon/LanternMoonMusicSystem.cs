using Microsoft.Xna.Framework.Audio;

namespace Everglow.Myth.LanternMoon.LanternCommon;

/// <summary>
/// A system for playing wav sources during the Lantern Moon event. This is necessary because the vanilla music system does not support multi-soundtrack.
/// </summary>
public class LanternMoonMusicSystem : ModSystem
{
	public struct SoundTrackInfo
	{
		public string Path;
		public DynamicSoundEffectInstance Track;
		public float FadeValue;
		public float CueVolumn;
		public bool Loop;
		public bool Fade;
		public bool Active;
		public BinaryReader WavReader;
		public int WavDataStart;
		public int WavDataSize;
		public int WavFrequency;
	}

	public List<SoundTrackInfo> LanternMoonMusicCues = new List<SoundTrackInfo>();
	private byte[] wavBuffer = new byte[4096 * 2];

	private AudioChannels wavChannels;

	public SoundTrackInfo GetWavAudioInstance(string path)
	{
		SoundTrackInfo soundTrackInfo = new SoundTrackInfo
		{
			Path = path,
			Track = null,
			Active = true,
			Fade = false,
			Loop = true,
			FadeValue = 0,
			CueVolumn = 1,
		};
		if (!MusicLoader.MusicExists(path))
		{
			soundTrackInfo.Active = false;
			return soundTrackInfo;
		}

		string extension = MusicLoader.musicExtensions[path];
		path = "tmod:" + path + extension;
		Stream stream = ModContent.OpenRead(path, true);
		using (Stream zipStream = stream)
		{
			MemoryStream ms = new MemoryStream();
			zipStream.CopyTo(ms);
			ms.Position = 0;
			if (ms == null)
			{
				soundTrackInfo.Active = false;
				return soundTrackInfo;
			}

			// Analysis WAV head
			using (BinaryReader br = new BinaryReader(ms))
			{
				// Jump rift
				br.ReadBytes(12);

				// Find "FMT"
				while (br.ReadInt32() != 0x20746D66) // "fmt "
				{
					br.ReadBytes(br.ReadInt32());
				}

				br.ReadInt32(); // fmtSize
				short audioFormat = br.ReadInt16();
				short channels = br.ReadInt16();
				soundTrackInfo.WavFrequency = br.ReadInt32();
				br.ReadInt32(); // byteRate
				br.ReadInt16(); // blockAlign
				br.ReadInt16(); // bitsPerSample

				// Find "data"
				while (br.ReadInt32() != 0x61746164)
				{
					br.ReadBytes(br.ReadInt32());
				}

				soundTrackInfo.WavDataSize = br.ReadInt32();
				soundTrackInfo.WavDataStart = (int)br.BaseStream.Position;
				wavChannels = channels == 1 ? AudioChannels.Mono : AudioChannels.Stereo;
			}
		}
		stream = ModContent.OpenRead(path, true);
		using (Stream zipStream = stream)
		{
			MemoryStream ms = new MemoryStream();
			zipStream.CopyTo(ms);
			ms.Position = 0;
			soundTrackInfo.WavReader = new BinaryReader(ms);
			soundTrackInfo.WavReader.BaseStream.Seek(soundTrackInfo.WavDataStart, SeekOrigin.Begin);
		}

		DynamicSoundEffectInstance dInst = new DynamicSoundEffectInstance(soundTrackInfo.WavFrequency, wavChannels);
		soundTrackInfo.Track = dInst;
		return soundTrackInfo;
	}

	public void UpdateMusic()
	{
		Main.NewText(LanternMoonMusicCues.Count);

		for (int i = LanternMoonMusicCues.Count - 1; i >= 0; i--)
		{
			var inst = LanternMoonMusicCues[i];

			if (inst.Track == null || inst.WavReader == null)
			{
				continue;
			}

			var track = inst.Track;

			if (inst.Fade)
			{
				inst.CueVolumn += inst.FadeValue;

				if (inst.CueVolumn >= 1f)
				{
					inst.CueVolumn = 1f;
					inst.Fade = false;
					inst.FadeValue = 0;
				}
				else if (inst.CueVolumn <= 0f)
				{
					inst.CueVolumn = 0f;
					inst.Fade = false;
					inst.FadeValue = 0;
					inst.Active = false;
				}
			}

			track.Volume = inst.CueVolumn * 0.25f;
			LanternMoonMusicCues[i] = inst;

			if (!inst.Active)
			{
				continue;
			}
			while (track.PendingBufferCount < 2)
			{
				//Main.NewText(new Vector2(track.PendingBufferCount, (float)Main.time));
				int bytesRead = inst.WavReader.Read(wavBuffer, 0, wavBuffer.Length);

				if (bytesRead > 0)
				{
					track.SubmitBuffer(wavBuffer, 0, bytesRead);
				}
				else
				{
					inst.WavReader.BaseStream.Position = inst.WavDataStart;
					if (!inst.Loop)
					{
						track.Volume = 0;
						inst.Active = false;
						LanternMoonMusicCues[i] = inst;
					}
				}
			}
		}
		LanternMoonMusicCues.RemoveAll(inst =>
			!inst.Active && inst.Track != null && inst.Track.PendingBufferCount == 0);
	}

	public override void PreUpdateTime()
	{
		UpdateMusic();
		base.PreUpdateTime();
	}

	public void PlayMusic(string path)
	{
		SoundTrackInfo sound = GetWavAudioInstance(path);
		var track = sound.Track;
		if (track != null)
		{
			track.Play();
		}
		LanternMoonMusicCues.Add(sound);
	}

	public void StopMusic(string path)
	{
		for (int i = 0; i < LanternMoonMusicCues.Count; i++)
		{
			var track = LanternMoonMusicCues[i].Track;
			if (track != null)
			{
				if (LanternMoonMusicCues[i].Path == path)
				{
					track.Stop();
				}
			}
		}
	}

	public void ContinueOrStopMusic(string path)
	{
		for (int i = 0; i < LanternMoonMusicCues.Count; i++)
		{
			var inst = LanternMoonMusicCues[i];
			var track = inst.Track;
			if (track != null)
			{
				if (inst.Path == path)
				{
					if (track.State == SoundState.Stopped)
					{
						track.Play();
					}
					else if (track.State == SoundState.Playing)
					{
						track.Stop();
					}
					inst.Track = track;
					LanternMoonMusicCues[i] = inst;
					return;
				}
			}
		}
	}

	public void FadeMusic(string path, int totalTime)
	{
		for (int i = 0; i < LanternMoonMusicCues.Count; i++)
		{
			var inst = LanternMoonMusicCues[i];
			var track = inst.Track;
			if (track != null)
			{
				if (inst.Path == path && inst.Active)
				{
					inst.Fade = true;
					inst.FadeValue = -1f / totalTime;
					inst.Track = track;
					LanternMoonMusicCues[i] = inst;
				}
			}
		}
	}

	public void StartMusic(string path, int totalTime)
	{
		SoundTrackInfo sound = GetWavAudioInstance(path);
		var track = sound.Track;
		if (track != null)
		{
			track.Play();
		}
		sound.CueVolumn = 0;
		sound.Fade = true;
		sound.FadeValue = 1f / totalTime;
		LanternMoonMusicCues.Add(sound);
	}
}