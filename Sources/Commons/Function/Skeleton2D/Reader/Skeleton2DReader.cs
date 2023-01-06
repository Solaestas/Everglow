using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System.Net.Mail;

namespace Everglow.Sources.Commons.Function.Skeleton2D.Reader
{
    /// <summary>
    /// Json: Skin块每个皮肤都描述了可分配给每个槽位的附件
    /// </summary>
    internal class J_Skin
    {
        /// <summary>
        /// 插槽的名字，对于每个skeleton来说，slot的名字是唯一的
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// 在TPose的状态下该插槽里挂载的挂件的名字
        /// </summary>
        [JsonProperty("attachments")]
        [JsonConverter(typeof(AttachmentsJsonConverter))]
        public J_Attachments Attachments;
    }


    /// <summary>
    /// Json: Slots数据块描述的是渲染顺序以及2D图片的挂件挂载到哪些插孔清单
    /// </summary>
    internal class J_Slot
    {
        /// <summary>
        /// 插槽的名字，对于每个skeleton来说，slot的名字是唯一的
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// 插槽所在的骨头的名字
        /// </summary>
        [JsonProperty("bone")]
        public string Bone;

        /// <summary>
        /// 在TPose的状态下该插槽里挂载的挂件的名字
        /// </summary>
        [JsonProperty("attachment")]
        public string Attachment;
    }

    /// <summary>
    /// Json: Bone 段存储的是骨头的信息
    /// </summary>
    internal class J_Bone
    {
        /// <summary>
        /// 骨头的名字，在每一个骨架中，骨头的名字是唯一的
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        [DefaultValue("")]
        [JsonProperty(PropertyName = "parent", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Parent = "";

        /// <summary>
        /// 骨头的长度，这个东西在运行时没什么用，主要是为了Debug用的
        /// </summary>
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "length", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public float Length = 0f;

        /// <summary>
        /// TPose的时候该骨头相对于它的父亲节点的x的位置属性，如果这个属性没有出现在bone的属性列表里面的话，它的值是0
        /// </summary>
        [DefaultValue(0f)]
        [JsonProperty(PropertyName = "x", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public float PosX = 0f;

        /// <summary>
        ///  TPose的时候该骨头相对于它的父亲节点的y的位置属性，如果这个属性没有出现在bone的属性列表里面的话，它的值是0
        /// </summary>
        [DefaultValue(0f)]
        [JsonProperty(PropertyName = "y", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public float PosY = 0f;

        /// <summary>
        /// TPose的时候该骨头相对于它的父亲节点的旋转角度，如果这个属性没有出现在bone的属性列表里面的话，它的值是0
        /// </summary>
        [DefaultValue(0f)]
        [JsonProperty(PropertyName = "rotation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public float Rotation = 0f;

        /// <summary>
        /// TPose的时候该骨头相对于它的父亲节点的X方向缩放倍数（暂时不用）
        /// </summary>
        [DefaultValue(1f)]
        [JsonProperty(PropertyName = "scaleX", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public float ScaleX = 1f;

        /// <summary>
        /// TPose的时候该骨头相对于它的父亲节点的Y方向缩放倍数（暂时不用）
        /// </summary>
        [DefaultValue(1f)]
        [JsonProperty(PropertyName = "scaleY", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public float ScaleY = 1f;

        /// <summary>
        /// 骨头的颜色RGBA（Debug用）
        /// </summary>
        [DefaultValue("#ffffffff")]
        [JsonProperty(PropertyName = "color", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Color = "#ffffffff";

        //       {
        //	"name": "branch-1",
        //	"parent": "root",
        //	"length": 487.04,
        //	"rotation": 27.67,
        //	"x": -579.3,
        //	"y": -239.11,
        //	"color": "1a8600ff"
        //},
        public J_Bone()
        {
        }
    }

    /// <summary>
    /// Json: Skeleton 段存储的数据是关于骨架的一些元数据
    /// </summary>
    internal class J_SkeletonInfo
    {
        [JsonProperty("hash")]
        [JsonRequired]
        public string Hash
        {
            get;set;
        }

        [JsonProperty("spine")]
        [JsonRequired]
        public string SpineVersion
        {
            get; set;
        }

        [JsonProperty("x")]
        [JsonRequired]
        public float BoundingBoxX
        {
            get; set;
        }

        [JsonProperty("y")]
        [JsonRequired]
        public float BoundingBoxY
        {
            get; set;
        }

        [JsonProperty("width")]
        [JsonRequired]
        public float BoundingBoxWidth
        {
            get; set;
        }

        [JsonProperty("height")]
        [JsonRequired]
        public float BoundingBoxHeight
        {
            get; set;
        }

        [JsonProperty("images")]
        [JsonRequired]
        public string ImagesPath
        {
            get; set;
        }

        [DefaultValue(30)]
        [JsonProperty(PropertyName = "fps", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]

        public int FramesPerSecond
        {
            get; set;
        }

        [JsonProperty("audio")]
        public string AudioPath
        {
            get; set;
        }

        public J_SkeletonInfo()
        {
        }
    }
    /// <summary>
    /// Json 骨架数据格式
    /// </summary>
    internal class J_Skeleton
    {
        [JsonProperty("skeleton")]
        public J_SkeletonInfo SkeletonInfo;

        [JsonProperty("bones")]
        public List<J_Bone> Bones;

        [JsonProperty("slots")]
        public List<J_Slot> Slots;

        [JsonProperty("skins")]
        public List<J_Skin> Skins;

        [JsonProperty(PropertyName = "animations", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Dictionary<string, J_Animation> Animations;

        public J_Skeleton()
        {
        }


    }
    internal class Skeleton2DReader
    {
        public static Skeleton2D ReadSkeleton(byte[] buffer, string path)
        {
            using MemoryStream ms = new MemoryStream(buffer);
            using StreamReader sr = new StreamReader(ms);
            string text = sr.ReadToEnd();
            var skeleton = JsonConvert.DeserializeObject<J_Skeleton>(text);
            return ConvertToInternalSkeleton(skeleton, path);
        }

        private static Skeleton2D ConvertToInternalSkeleton(J_Skeleton jSkeleton, string path)
        {
            Dictionary<string, Bone2D> bonesDict = new Dictionary<string, Bone2D>();
            List<Bone2D> bones = new List<Bone2D>();
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

            Skeleton2D skeleton = new Skeleton2D(bones);

            Dictionary<string, Attachment> attachmentDict = new Dictionary<string, Attachment>();
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
                            RegionAttachment region = new RegionAttachment
                            {
                                Texture = image.Value,
                                Position = new Vector2(inner.X, -inner.Y),
                                Rotation = -inner.Rotation / 180f * MathHelper.Pi,
                                Size = new Vector2(inner.Width, inner.Height)
                            };
                            attachmentCollection.Add(region);
                            attachmentDict.Add(inner.ImageName, region);
                        }
                        else if (inner.Type == "mesh")
                        {
                            MeshAttachment mesh = new MeshAttachment
                            {
                                Texture = image.Value,
                                TriangleIndices = inner.TriangleIndiciesList,
                            };
                            List<AnimationVertex> vertices = new List<AnimationVertex>();
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
                                    List<BoneBinding> bindings = new List<BoneBinding>();
                                    for (int i = 0; i < count; i++)
                                    {
                                        int boneIndex = (int)inner.VerticesList[++index];
                                        float x = inner.VerticesList[++index];
                                        float y = inner.VerticesList[++index];
                                        float weight = inner.VerticesList[++index];

                                        BoneBinding binding = new BoneBinding()
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

            Dictionary<string, Slot> slotsDict = new Dictionary<string, Slot>();
            // 加载Slots
            foreach (var jSlot in jSkeleton.Slots)
            {
                var slot = new Slot
                {
                    Name = jSlot.Name,
                    Bone = bonesDict[jSlot.Bone],
                    Attachment = null
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
        private static void ParseAnimations(Dictionary<string, J_Animation> jAnimations, 
            Skeleton2D skeleton, 
            Dictionary<string, Attachment> attachmentsDict,
            Dictionary<string, Slot> slotsDict,
            Dictionary<string, Bone2D> bonesDict)
        {
            skeleton.Animations = new Dictionary<string, Animation>();

            foreach (var animKV in jAnimations)
            {
                Animation animation = new Animation();
                animation.Name = animKV.Key;
                animation.BonesTimeline = ParseBoneTimelines(animKV.Value.Bones, bonesDict);
                animation.SlotsTimeline = ParseSlotTimelines(animKV.Value.Slots, slotsDict, attachmentsDict);
                skeleton.Animations.Add(animation.Name, animation);
            }
        }


        private static List<Timeline> ParseBoneTimelines(Dictionary<string, JObject> bones, 
            Dictionary<string, Bone2D> bonesDict)
        {
            List<Timeline> timelines = new List<Timeline>();
            if (bones == null)
            {
                return timelines;
            }

            foreach (var kvPair in bones)
            {
                var bone = bonesDict[kvPair.Key];
                var jobject = kvPair.Value;

                Timeline timeline = new Timeline();

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


        private static List<Timeline> ParseSlotTimelines(Dictionary<string, JObject> slots,
            Dictionary<string, Slot> slotsDict, 
            Dictionary<string, Attachment> attachmentsDict
            )
        {
            List<Timeline> timelines = new List<Timeline>();
            if (slots == null)
            {
                return timelines;
            }
            foreach (var kvPair in slots)
            {
                var slot = slotsDict[kvPair.Key];
                var jobject = kvPair.Value;

                Timeline timeline = new Timeline();

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

        private static int charToHex(char c)
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
            int r = charToHex(hex[0]) * 16 + charToHex(hex[1]);
            int g = charToHex(hex[2]) * 16 + charToHex(hex[3]);
            int b = charToHex(hex[4]) * 16 + charToHex(hex[5]);
            int a = charToHex(hex[6]) * 16 + charToHex(hex[7]);
            return new Color(r, g, b, a);
        }
    }
}
