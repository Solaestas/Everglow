namespace Everglow.Commons.IIID;

public static class ObjReader
{
	/// <summary>
	/// Find the index of the first occurrence of a character in a string.<br/>
	/// return the length of the string if the character is not found.
	/// </summary>
	/// <param name="str"></param>
	/// <param name="c"></param>
	/// <returns></returns>
	private static int OfIndex(this ReadOnlySpan<char> str, char c)
	{
		int i = 0;
		for (; i < str.Length; i++)
		{
			if (str[i] == c)
			{
				return i;
			}
		}
		return i;
	}

	private static T ParseNext<T>(ref ReadOnlySpan<char> text)
		where T : ISpanParsable<T>
	{
		int index = text.OfIndex(' ');
		T result = T.Parse(text[..index], null);
		text = index == text.Length ? default : text[(index + 1)..];
		return result;
	}

	public static Model Load(Stream stream)
	{
		Model mesh = new Model();
		using StreamReader objReader = new StreamReader(stream);
		mesh.Positions.Add(Vector3.Zero);
		mesh.TexCoords.Add(Vector2.Zero);
		mesh.Normals.Add(Vector3.Zero);
		while (objReader.ReadLine() is string text)
		{
			if (text.Length < 2)
			{
				continue;
			}
			var span = text.AsSpan();
			int index = span.OfIndex(' ');
			var leader = span[..index];
			if (leader[0] == 'v')
			{
				var content = span[(index + 1)..];
				if (leader.Length == 1)
				{
					// v -53.0413 158.84 -135.806 点
					mesh.Positions.Add(new Vector3(
						ParseNext<float>(ref content),
						ParseNext<float>(ref content),
						ParseNext<float>(ref content)));
				}
				else if (leader[1] == 't')
				{
					// vt 0.581151 0.979929 纹理
					mesh.TexCoords.Add(new Vector2(
						ParseNext<float>(ref content),
						ParseNext<float>(ref content)));
				}
				else if (leader[1] == 'n')
				{
					// vn 0.637005 -0.0421857 0.769705 法向量
					mesh.Normals.Add(new Vector3(
						ParseNext<float>(ref content),
						ParseNext<float>(ref content),
						ParseNext<float>(ref content)));
				}
				Debug.Assert(content == default);
			}
			else if (leader[0] == 'f')
			{
				// f 2443//2656 2442//2656 2444//2656 面
				var content = span[(index + 1)..];

				Face face = default;
				face[0] = ParseNext<Face.Array3<int>>(ref content);
				face[1] = ParseNext<Face.Array3<int>>(ref content);
				face[2] = ParseNext<Face.Array3<int>>(ref content);
				Debug.Assert(content == default);
				mesh.Faces.Add(face);
			}
		}
		return mesh;
	}

	public static Model LoadFile(string fileName)
	{
		return Load(ModIns.Mod.GetFileStream(fileName));
	}
}