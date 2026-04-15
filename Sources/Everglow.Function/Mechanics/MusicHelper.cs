using System.IO;
using Everglow.Commons.DeveloperContent.VFXs;
using Microsoft.Xna.Framework.Audio;

namespace Everglow.Commons.Mechanics;

/// <summary>
/// A system for playing wav sources during the Lantern Moon event. This is necessary because the vanilla music system does not support multi-soundtrack.
/// </summary>
public abstract class MusicHelper : ModSystem
{
	public static bool EnableSoundTrackVisualization = false;

	public struct SoundTrackInfo
	{
		public string Path;
		public DynamicSoundEffectInstance Track;
		public VisualizedMusicTrack VFX;
		public float FadeValue;
		public float CueVolume;
		public bool Loop;
		public bool Fade;
		public bool Active;
		public BinaryReader WavReader;
		public int WavDataStart;
		public int WavDataSize;
		public int WavFrequency;
		public int Index;
		public float[] FFTBands;
	}

	public List<SoundTrackInfo> CustomMusicCues = new List<SoundTrackInfo>();
	private byte[] wavBuffer = new byte[4096 * 2];

	private AudioChannels wavChannels;

	public class Complex
	{
		public float Re;
		public float Im;

		public Complex(float re, float im)
		{
			Re = re;
			Im = im;
		}

		public static Complex[] FromPCM(byte[] pcm, int sampleCount, int bytesPerSample)
		{
			var data = new Complex[sampleCount];
			for (int i = 0; i < sampleCount; i++)
			{
				short sample = BitConverter.ToInt16(pcm, i * bytesPerSample);
				float f = sample / 32768f; // 转成 -1~1
				data[i] = new Complex(f, 0);
			}
			return data;
		}
	}

	public static void FFT(Complex[] data, bool invert)
	{
		int n = data.Length;
		for (int i = 1, j = 0; i < n; i++)
		{
			int bit = n >> 1;
			for (; j >= bit; bit >>= 1)
			{
				j -= bit;
			}

			j += bit;
			if (i < j)
			{
				(data[i], data[j]) = (data[j], data[i]);
			}
		}

		for (int len = 2; len <= n; len <<= 1)
		{
			float ang = 2 * MathF.PI / len * (invert ? -1 : 1);
			for (int i = 0; i < n; i += len)
			{
				for (int j = 0; j < len / 2; j++)
				{
					float c = MathF.Cos(j * ang);
					float s = MathF.Sin(j * ang);
					Complex u = data[i + j];
					var v = new Complex(
						data[i + j + len / 2].Re * c - data[i + j + len / 2].Im * s,
						data[i + j + len / 2].Re * s + data[i + j + len / 2].Im * c);
					data[i + j] = new Complex(u.Re + v.Re, u.Im + v.Im);
					data[i + j + len / 2] = new Complex(u.Re - v.Re, u.Im - v.Im);
				}
			}
		}
	}

	public SoundTrackInfo GetWavAudioInstance(string path, bool loop)
	{
		var soundTrackInfo = new SoundTrackInfo
		{
			Path = path,
			Track = null,
			Active = true,
			Fade = false,
			Loop = loop,
			FadeValue = 0,
			CueVolume = 1,
			Index = 0,
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
			var ms = new MemoryStream();
			zipStream.CopyTo(ms);
			ms.Position = 0;
			if (ms == null)
			{
				soundTrackInfo.Active = false;
				return soundTrackInfo;
			}

			// Analysis WAV head
			using (var br = new BinaryReader(ms))
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
			var ms = new MemoryStream();
			zipStream.CopyTo(ms);
			ms.Position = 0;
			soundTrackInfo.WavReader = new BinaryReader(ms);
			soundTrackInfo.WavReader.BaseStream.Seek(soundTrackInfo.WavDataStart, SeekOrigin.Begin);
		}

		var dInst = new DynamicSoundEffectInstance(soundTrackInfo.WavFrequency, wavChannels);
		soundTrackInfo.Track = dInst;
		StartMusicEffect(path);
		return soundTrackInfo;
	}

	public virtual void UpdateMusic()
	{
		// Main.NewText(CustomMusicCues.Count);
		int index = 0;
		for (int i = CustomMusicCues.Count - 1; i >= 0; i--)
		{
			var inst = CustomMusicCues[i];

			if (inst.Track == null || inst.WavReader == null)
			{
				continue;
			}
			if (!inst.Active)
			{
				continue;
			}
			//Main.NewText(inst.Index,new Color(1f, 0.7f, 0));
			inst.Index = index;
			index++;
			var track = inst.Track;
			if (inst.Fade)
			{
				inst.CueVolume += inst.FadeValue;

				if (inst.CueVolume >= 1f)
				{
					inst.CueVolume = 1f;
					inst.Fade = false;
					inst.FadeValue = 0;
				}
				else if (inst.CueVolume <= 0f)
				{
					inst.CueVolume = 0f;
					inst.Fade = false;
					inst.FadeValue = 0;
					inst.Active = false;
				}
			}
			track.Volume = inst.CueVolume * 0.25f;
			while (track.PendingBufferCount < 2)
			{
				Array.Clear(wavBuffer, 0, wavBuffer.Length);  // Keep this
				long remaining = inst.WavReader.BaseStream.Length - inst.WavReader.BaseStream.Position;
				int readLength = Math.Min(wavBuffer.Length, (int)remaining);
				int bytesRead = inst.WavReader.Read(wavBuffer, 0, readLength);
				if (remaining <= readLength)
				{
					Array.Clear(wavBuffer, bytesRead, wavBuffer.Length - bytesRead);
					inst.WavReader.BaseStream.Position = inst.WavDataStart;
					if (!inst.Loop)
					{
						inst.Active = false;
						EndMusicEffect(inst.Path);
						break;
					}
				}

				if (bytesRead > 0)
				{
					if (remaining > 16384)
					{
						track.SubmitBuffer(wavBuffer, 0, bytesRead);
					}
					if (EnableSoundTrackVisualization)
					{
						// Visualize audio.
						int channels = 2;
						int bitsPerSample = 16;
						int bytesPerSample = bitsPerSample / 8;
						int sampleCount = bytesRead / (channels * bytesPerSample);
						Complex[] fftData = Complex.FromPCM(wavBuffer, sampleCount, bytesPerSample);
						int fftSize = 1024;
						Array.Resize(ref fftData, fftSize);
						FFT(fftData, invert: false);
						float[] bands = new float[fftSize / 2];
						for (int k = 0; k < fftSize / 2; k++)
						{
							float mag = MathF.Sqrt(fftData[k].Re * fftData[k].Re + fftData[k].Im * fftData[k].Im);
							bands[k] = mag;
							bands[k] = bands[k] / 100f;
						}
						inst.FFTBands = bands;
						if(inst.VFX is not null)
						{
							inst.VFX.Visible = true;
						}
					}
					else
					{
						if (inst.VFX is not null)
						{
							inst.VFX.Visible = false;
						}
					}
				}
			}
			if(inst.VFX is not null)
			{
				inst.VFX.MusicCue = inst;
			}
			else
			{
				var vfx = new VisualizedMusicTrack()
				{
					MusicCue = inst,
				};
				inst.VFX = vfx;
				Ins.VFXManager.Add(vfx);
			}
			CustomMusicCues[i] = inst;
		}
		CustomMusicCues.RemoveAll(inst =>
			!inst.Active && inst.Track != null && inst.Track.PendingBufferCount == 0);
	}

	public override void PreUpdateTime()
	{
		UpdateMusic();
		base.PreUpdateTime();
	}

	public virtual void StartMusicEffect(string path)
	{
	}

	public virtual void EndMusicEffect(string path)
	{
	}

	public bool HasMusic(string path)
	{
		for (int i = 0; i < CustomMusicCues.Count; i++)
		{
			if (CustomMusicCues[i].Path == path)
			{
				return true;
			}
		}
		return false;
	}

	public void PlayMusic(string path, bool loop)
	{
		SoundTrackInfo sound = GetWavAudioInstance(path, loop);
		var track = sound.Track;
		if (track != null)
		{
			track.Play();
		}
		CustomMusicCues.Add(sound);
	}

	public void StopMusic(string path)
	{
		for (int i = 0; i < CustomMusicCues.Count; i++)
		{
			var track = CustomMusicCues[i].Track;
			if (track != null)
			{
				if (CustomMusicCues[i].Path == path)
				{
					track.Stop();
				}
			}
		}
	}

	public void ContinueOrStopMusic(string path)
	{
		for (int i = 0; i < CustomMusicCues.Count; i++)
		{
			var inst = CustomMusicCues[i];
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
					CustomMusicCues[i] = inst;
					return;
				}
			}
		}
	}

	public void FadeMusic(string path, int fadeOutTime)
	{
		for (int i = 0; i < CustomMusicCues.Count; i++)
		{
			var inst = CustomMusicCues[i];
			var track = inst.Track;
			if (track != null)
			{
				if (inst.Path == path && inst.Active)
				{
					inst.Fade = true;
					inst.FadeValue = -1f / fadeOutTime;
					inst.Track = track;
					CustomMusicCues[i] = inst;
				}
			}
		}
	}

	public void FadeAllTheCurrentMusic(int fadeOutTime)
	{
		for (int i = 0; i < CustomMusicCues.Count; i++)
		{
			var inst = CustomMusicCues[i];
			var track = inst.Track;
			if (track != null)
			{
				inst.Fade = true;
				inst.FadeValue = -1f / fadeOutTime;
				inst.Track = track;
				CustomMusicCues[i] = inst;
			}
		}
	}

	public void FadeMusic(SoundTrackInfo inst, int fadeOutTime)
	{
		int index = -1;
		for (int i = 0; i < CustomMusicCues.Count; i++)
		{
			if (CustomMusicCues[i].Equals(inst))
			{
				index = i;
				break;
			}
		}
		var track = inst.Track;
		if (track != null)
		{
			if (inst.Active)
			{
				inst.Fade = true;
				inst.FadeValue = -1f / fadeOutTime;
				inst.Track = track;
				CustomMusicCues[index] = inst;
			}
		}
	}

	public void StartMusic(string path, int fadeInTime, bool loop)
	{
		SoundTrackInfo sound = GetWavAudioInstance(path, loop);
		var track = sound.Track;
		if (track != null)
		{
			track.Play();
		}
		sound.CueVolume = 0;
		sound.Fade = true;
		sound.FadeValue = 1f / fadeInTime;
		CustomMusicCues.Add(sound);
	}
}