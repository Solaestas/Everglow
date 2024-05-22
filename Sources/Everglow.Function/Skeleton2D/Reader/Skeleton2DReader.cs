using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Everglow.Commons.Skeleton2D.Reader;

public static class Skeleton2DReader
{
	public static Skeleton2D ReadSkeleton(byte[] buffer, string path)
	{
		using var ms = new MemoryStream(buffer);
		using var sr = new StreamReader(ms);
		string text = sr.ReadToEnd();
		var skeleton = JsonConvert.DeserializeObject<JSkeleton>(text);
		return ConvertTopublicSkeleton(skeleton, path);
	}

	private static Skeleton2D ConvertTopublicSkeleton(JSkeleton jSkeleton, string path)
	{
		var bonesDict = new Dictionary<string, Bone2D>();
		var bones = new List<Bone2D>();
		foreach (var jbone in jSkeleton.Bones)
		{
			var bone = new Bone2D();
			if (jbone.Parent != null && bonesDict.ContainsKey(jbone.Parent))
			{
				bonesDict[jbone.Parent].AddChild(bone);
			}
			else
			{
				bone.Parent = null;
			}
			bone.Name = jbone.Name;
			bone.Length = jbone.Length;

			// 由于软件坐标系是左下角为基准点，转化到TR坐标系需要翻转Y坐标以及旋转角
			bone.Position = new Vector2(jbone.PosX, -jbone.PosY);
			bone.Rotation = -jbone.Rotation / 180.0f * MathHelper.Pi;
			bone.Scale = new Vector2(jbone.ScaleX, jbone.ScaleY);
			bone.Color = HexToColor(jbone.Color);

			bonesDict.Add(bone.Name, bone);
			bones.Add(bone);
		}

		var skeleton = new Skeleton2D(bones);

		var attachmentDict = new Dictionary<string, Attachment>();

		// 加载Attachments
		foreach (var jSkin in jSkeleton.Skins)
		{
			foreach (var attachment in jSkin.Attachments.Attachments)
			{
				var attachmentCollection = new List<Attachment>();
				foreach (var inner in attachment.AttachmentInners)
				{
					var imagePath = Path.Combine(path, jSkeleton.SkeletonInfo.ImagesPath, inner.ImageName);
					var image = ModContent.Request<Texture2D>(imagePath, ReLogic.Content.AssetRequestMode.ImmediateLoad);

					if (inner.Type == "region")
					{
						var region = new RegionAttachment
						{
							Texture = image.Value,
							Position = new Vector2(inner.X, -inner.Y),
							Rotation = -inner.Rotation / 180f * MathHelper.Pi,
							Size = new Vector2(inner.Width, inner.Height),
						};
						attachmentCollection.Add(region);
						attachmentDict.Add(inner.ImageName, region);
					}
					else if (inner.Type == "mesh")
					{
						var mesh = new MeshAttachment
						{
							Texture = image.Value,
							TriangleIndices = inner.TriangleIndiciesList,
						};
						var vertices = new List<AnimationVertex>();
						for (int i = 0; i < inner.UVList.Count; i += 2)
						{
							float x = inner.UVList[i];
							float y = inner.UVList[i + 1];
							vertices.Add(new AnimationVertex() { UV = new Vector2(x, y) });
						}

						// 如果是加权网格数据，那么特殊处理
						if (inner.VerticesList.Count > inner.UVList.Count)
						{
							int index = 0;
							int vertexIndex = 0;
							while (index < inner.VerticesList.Count)
							{
								float count = inner.VerticesList[index];
								var bindings = new List<BoneBinding>();
								for (int i = 0; i < count; i++)
								{
									int boneIndex = (int)inner.VerticesList[++index];
									float x = inner.VerticesList[++index];
									float y = inner.VerticesList[++index];
									float weight = inner.VerticesList[++index];

									var binding = new BoneBinding()
									{
										BindPosition = new Vector2(x, -y),
										Bone = bones[boneIndex],
										Weight = weight,
									};
									bindings.Add(binding);
								}
								vertices[vertexIndex].BoneBindings = bindings;
								index++;
								vertexIndex++;
							}
						}
						mesh.AnimationVertices = vertices;
						attachmentCollection.Add(mesh);
						attachmentDict.Add(inner.ImageName, mesh);
					}
				}
			}
		}

		var slotsDict = new Dictionary<string, Slot>();

		// 加载Slots
		foreach (var jSlot in jSkeleton.Slots)
		{
			var slot = new Slot
			{
				Name = jSlot.Name,
				Bone = bonesDict[jSlot.Bone],
				Attachment = null,
			};
			if (jSlot.Attachment != null && attachmentDict.ContainsKey(jSlot.Attachment))
			{
				slot.Attachment = attachmentDict[jSlot.Attachment];
			}

			skeleton.Slots.Add(slot);
			slotsDict.Add(slot.Name, slot);
		}

		// 加载 Animations 动画元素
		ParseAnimations(jSkeleton.Animations, skeleton, attachmentDict, slotsDict, bonesDict);

		return skeleton;
	}

	/// <summary>
	/// 解析动画帧部分
	/// </summary>
	/// <param name="jAnimations"></param>
	/// <param name="skeleton"></param>
	/// <param name="attachmentsDict"></param>
	/// <param name="slotsDict"></param>
	/// <param name="bonesDict"></param>
	private static void ParseAnimations(
		Dictionary<string, JAnimation> jAnimations,
		Skeleton2D skeleton,
		Dictionary<string, Attachment> attachmentsDict,
		Dictionary<string, Slot> slotsDict,
		Dictionary<string, Bone2D> bonesDict)
	{
		skeleton.Animations = [];

		foreach (var animKV in jAnimations)
		{
			var animation = new Animation
			{
				Name = animKV.Key,
				BonesTimeline = ParseBoneTimelines(animKV.Value.Bones, bonesDict),
				SlotsTimeline = ParseSlotTimelines(animKV.Value.Slots, slotsDict, attachmentsDict),
			};
			skeleton.Animations.Add(animation.Name, animation);
		}
	}

	private static List<Timeline> ParseBoneTimelines(
		Dictionary<string, JObject> bones,
		Dictionary<string, Bone2D> bonesDict)
	{
		var timelines = new List<Timeline>();
		if (bones == null)
		{
			return timelines;
		}

		foreach (var kvPair in bones)
		{
			var bone = bonesDict[kvPair.Key];
			var jobject = kvPair.Value;

			var timeline = new Timeline();

			foreach (var componentKFPair in jobject)
			{
				var type = componentKFPair.Key;
				var values = componentKFPair.Value as JArray;
				var track = new Track();

				foreach (JObject keyFrame in values)
				{
					float time = 0;
					InterpolationMethod interpolation = InterpolationMethod.Lerp;

					// 非必要 time 属性
					if (keyFrame.ContainsKey("time"))
					{
						time = keyFrame.Value<float>("time");
					}

					// 非必要 curve 属性
					if (keyFrame.ContainsKey("curve"))
					{
						if (keyFrame["curve"].Type == JTokenType.String)
						{
							var curvetype = keyFrame.Value<string>("curve");
							switch (curvetype)
							{
								case "linear":
									{
										interpolation = InterpolationMethod.Lerp;
										break;
									}
								case "stepped":
									{
										interpolation = InterpolationMethod.Step;
										break;
									}
							}
						}
						else // 不是string那么就是曲线参数
						{
							float cx1 = keyFrame.Value<float>("curve");
							float cy1 = 0;
							float cx2 = 1;
							float cy2 = 1;
							if (keyFrame.ContainsKey("c2"))
							{
								cy1 = keyFrame.Value<float>("c2");
							}

							if (keyFrame.ContainsKey("c3"))
							{
								cx2 = keyFrame.Value<float>("c3");
							}

							if (keyFrame.ContainsKey("c4"))
							{
								cy2 = keyFrame.Value<float>("c4");
							}

							interpolation = new Curve(new Vector2(cx1, cy1), new Vector2(cx2, cy2));
						}
					}

					if (type == "translate")
					{
						// translate有x，y两个可选属性
						Vector2 translate = Vector2.Zero;
						if (keyFrame.ContainsKey("x"))
						{
							translate.X = keyFrame.Value<float>("x");
						}

						if (keyFrame.ContainsKey("y"))
						{
							translate.Y = keyFrame.Value<float>("y");
						}

						track.AddKeyFrame(new BoneTranslationKeyFrame(time, bone, interpolation, bone.Position + translate));
					}
					else if (type == "rotate")
					{
						// rotate 只有旋转角一个可选属性
						float angle = 0f;
						if (keyFrame.ContainsKey("angle"))
						{
							angle = keyFrame.Value<float>("angle");
						}

						track.AddKeyFrame(new BoneRotationKeyFrame(time, bone, interpolation, bone.Rotation - angle / 180.0f * MathHelper.Pi));
					}
				}
				timeline.AddTrack(track);
			}
			timelines.Add(timeline);
		}
		return timelines;
	}

	private static List<Timeline> ParseSlotTimelines(
		Dictionary<string, JObject> slots,
		Dictionary<string, Slot> slotsDict,
		Dictionary<string, Attachment> attachmentsDict)
	{
		var timelines = new List<Timeline>();
		if (slots == null)
		{
			return timelines;
		}

		foreach (var kvPair in slots)
		{
			var slot = slotsDict[kvPair.Key];
			var jobject = kvPair.Value;

			var timeline = new Timeline();

			foreach (var componentKFPair in jobject)
			{
				var type = componentKFPair.Key;
				var values = componentKFPair.Value as JArray;
				var track = new Track();

				foreach (JObject keyFrame in values.Cast<JObject>())
				{
					float time = 0;
					InterpolationMethod interpolation = InterpolationMethod.Lerp;

					// 非必要 time 属性
					if (keyFrame.ContainsKey("time"))
					{
						time = keyFrame.Value<float>("time");
					}

					// 非必要 curve 属性
					if (keyFrame.ContainsKey("curve"))
					{
						// TODO: 曲线控制参数暂时没做
						var curvetype = keyFrame.Value<string>("curve");
						switch (curvetype)
						{
							case "linear":
								{
									interpolation = InterpolationMethod.Lerp;
									break;
								}
							case "steppped":
								{
									interpolation = InterpolationMethod.Step;
									break;
								}
						}
					}

					if (type == "attachment")
					{
						Attachment attachment = null;
						if (keyFrame.GetValue("name").Type != JTokenType.Null)
						{
							var name = keyFrame.Value<string>("name");
							if (attachmentsDict.ContainsKey(name))
							{
								attachment = attachmentsDict[name];
							}
						}

						track.AddKeyFrame(new SlotAttachmentKeyFrame(time, slot, attachment));
					}
				}

				timeline.AddTrack(track);
			}
			timelines.Add(timeline);
		}
		return timelines;
	}

	private static int CharToHex(char c)
	{
		if (char.IsNumber(c))
		{
			return c - '0';
		}
		else if (char.IsLower(c))
		{
			return c - 'a';
		}
		else if (char.IsUpper(c))
		{
			return c - 'A';
		}
		return 0;
	}

	private static Color HexToColor(string hex)
	{
		int r = CharToHex(hex[0]) * 16 + CharToHex(hex[1]);
		int g = CharToHex(hex[2]) * 16 + CharToHex(hex[3]);
		int b = CharToHex(hex[4]) * 16 + CharToHex(hex[5]);
		int a = CharToHex(hex[6]) * 16 + CharToHex(hex[7]);
		return new Color(r, g, b, a);
	}
}