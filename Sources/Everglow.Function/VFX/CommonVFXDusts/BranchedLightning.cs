using Everglow.Commons.DataStructures;
using Everglow.Commons.Enums;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX.Pipelines;
using ReLogic.Utilities;

namespace Everglow.Commons.VFX.CommonVFXDusts;

public class BranchedLightningPipeline : Pipeline
{
	public override void Load()
	{
		effect = ModAsset.BranchedLightning;
		effect.Value.Parameters["uDotLight"].SetValue(ModAsset.Point.Value);
		effect.Value.Parameters["uDisplacement"].SetValue(ModAsset.Noise_perlin.Value);
		effect.Value.Parameters["uDisplaceIntensity"].SetValue(BranchedLightning.DISPLACE_INTENSITY);
		effect.Value.Parameters["uNoiseSize"].SetValue(BranchedLightning.NOISE_SIZE);
		effect.Value.Parameters["uLineProportion"].SetValue(BranchedLightning.LINE_PROPORTION);
		effect.Value.Parameters["uEdgeColor"].SetValue((new Color(0, 200, 255)).ToVector4());
		effect.Value.Parameters["uBlurProportion"].SetValue(BranchedLightning.BLUR_PROPORTION);
	}
	public override void BeginRender()
	{
		var effect = this.effect.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		Texture2D lightningTrail = ModAsset.Trail.Value;
		Ins.Batch.BindTexture<Vertex2D>(lightningTrail);
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicClamp;
		Ins.Batch.Begin(BlendState.AlphaBlend, DepthStencilState.None, SamplerState.LinearClamp, RasterizerState.CullNone);
		effect.CurrentTechnique.Passes[0].Apply();
	}

	public override void EndRender()
	{
		Ins.Batch.End();
	}
}
[Pipeline(typeof(BranchedLightningPipeline), typeof(BloomPipeline))]
public class BranchedLightning : Visual
{
	public const int MAX_SEGMENTS = 9;
	public const int MAX_BRANCH_COUNT = 3;
	public const float DEFAULT_RENDER_STRIP_WIDTH = 125f;
	public const float DEVIATION_ANGLE = (float) Math.PI/3;
	public const float LENGTH_EXTENSION_SPEED = 175f; // pixel-length per frame
	public const float WIDTH_DECAY = 0.85f;
	public const float LENGTH_DECAY = 0.9f;
	public const float DEFAULT_SEGMENT_LENGTH = 125f;
	public const float WIDTH_EXPANSION_RATE = 1 / 5f;
	public const float WIDTH_SHRINK_RATE = 1 / 10f;

	public const float DISPLACE_INTENSITY = 0.15f;
	public const float DISPLACE_PERIOD = 45f;
	public const float NOISE_SIZE = 512;
	public const float RANDOM_SHIFT_RANGE = 1.85f;

	public const float LINE_PROPORTION = 0.1f;
	public const float EDGE_LINE_RATIO = 0.15f;
	public const float BLUR_PROPORTION = 0.02f;

	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;
	public Vector2 position;
	public float rotation;
	public float timer;
	public float maxTime;
	private LightningNode lightningRoot;
	public float segmentLength;
	public float renderStripWidth;

	private void SetUp()
	{
		Active = true;
		Visible = true;
		timer = 0;
		lightningRoot = new LightningNode(LINE_PROPORTION, Vector2.Zero, segmentLength, renderStripWidth);
	}

	public BranchedLightning()
	{
		segmentLength = DEFAULT_SEGMENT_LENGTH;
		renderStripWidth = DEFAULT_RENDER_STRIP_WIDTH;
		SetUp();
	}

	public BranchedLightning(float segmentLength, float width, Vector2 position, float rotation, float maxTime)
	{
		this.position = position;
		this.rotation = rotation;
		this.maxTime = maxTime;
		this.segmentLength = segmentLength;
		this.renderStripWidth = width / LINE_PROPORTION;
		SetUp();
	}

	public override void Update()
	{
		lightningRoot.Update(position, rotation);
		timer++;

		float remainingLife = maxTime - timer;
		if (remainingLife <= 0)
		{
			Active = false;
		} else if (remainingLife <= 1 / WIDTH_SHRINK_RATE) {
			lightningRoot.StartShrink();
		}
	}
	public override void Draw()
	{
		List<Vertex2D> barsList = new List<Vertex2D>();

		lightningRoot.AddVertexesToList(barsList, true);
		lightningRoot.AddVertexesToList(barsList, false);

		Ins.Batch.Draw(barsList, PrimitiveType.TriangleList);
	}

	// LightningNode: 单独闪电节点
	private class LightningNode 
	{
		// 长度(末尾位置)
		private Vector2 rawEndPos;
		private Vector2 currentEndPos;
		private float lengthProgress;
		private float lengthSpeed;
		private Vector2 shiftDisp;
		private Vector2 shiftVel;
		private float segmentLength;

		// 宽度
		private float widthProportion;
		private float widthProgress;
		private bool widthShrink;
		private float renderStripWidth;

		// 生成
		private int depth;
		private LightningNode parent;
		private List<LightningNode> children;

		public LightningNode(float nodeWidth, Vector2 rawEndPos, float segmentLength = DEFAULT_SEGMENT_LENGTH, float renderStripWidth = DEFAULT_RENDER_STRIP_WIDTH ,LightningNode parentNode = null, int nodeDepth = 0) {
			this.widthShrink = false;
			this.rawEndPos = rawEndPos;
			this.parent = parentNode;
			this.depth = nodeDepth;
			this.children = null;
			this.shiftDisp = Vector2.Zero;
			this.shiftVel = Vector2.Zero;
			this.segmentLength = segmentLength;
			this.renderStripWidth = renderStripWidth;

			if (this.depth < MAX_SEGMENTS)
			{
				this.widthProgress = 0;
				this.widthProportion = nodeWidth;
			} else {
				this.widthProgress = 1f;
				this.widthProportion = 0;
			}

			if (parent != null)
			{
				this.lengthProgress = 0f;
				this.lengthSpeed = LENGTH_EXTENSION_SPEED / (rawEndPos - parent.rawEndPos).Length();
				this.currentEndPos = parent.currentEndPos;
			} else {
				this.lengthProgress = 0f;
				this.lengthSpeed = 1;
			}
		}

		public void Update(Vector2 rootPosition, float lightningRotation) {
			// 更新位置
			currentEndPos = rootPosition + rawEndPos.RotatedBy(lightningRotation);
			if (parent != null)
			{
				float speedLimit = (rawEndPos - parent.rawEndPos).Length() * RANDOM_SHIFT_RANGE / 60;
				shiftDisp += Main.rand.NextVector2Circular(speedLimit, speedLimit);
				//shiftDisp += shiftVel;
				currentEndPos += shiftDisp;
			}

			if (lengthProgress < 1f)
			{
				// 成长未完成
				lengthProgress += lengthSpeed;

				if (lengthProgress >= 1f)
				{
					lengthProgress = 1;

					if (depth < MAX_SEGMENTS)
					{
						CreateChildren();
					}
				}
			} else {
				// 成长已完成
				if (widthShrink) {
					widthProgress -= WIDTH_SHRINK_RATE;
				} else if (widthProgress < 1f) {
					// 更新宽度
					widthProgress += WIDTH_EXPANSION_RATE;
				}

				if (children != null)
				{
					int childrenSize = children.Count;
					foreach (LightningNode child in children) 
					{
						child.Update(rootPosition, lightningRotation);
					}
					children.RemoveAll(child => (child.widthShrink && child.widthProgress <= 0));
				}
			}
		}

		private void CreateChildren()
		{
			int branchCount;
			Vector2 offset;
			if (parent != null)
			{
				branchCount = Main.rand.Next(0, MAX_BRANCH_COUNT) + (((depth - parent.depth) <= 1) ? 1 : 0);
				offset = rawEndPos - parent.rawEndPos;
			}
			else
			{
				branchCount = 1;
				offset = Vector2.UnitX * segmentLength;
			}

			for (int i = 0; i < branchCount; i++)
			{
				if (i == 0)
				{
					children = new List<LightningNode>();
				}

				int childDepth = depth + 1;
				float childWidth = widthProportion * WIDTH_DECAY;
				float childLengthDecay = LENGTH_DECAY;
				float childAngle = Main.rand.NextFloat(-0.5f * DEVIATION_ANGLE, 0.5f * DEVIATION_ANGLE);
				if (i != 0)
				{
					// 非主分支
					childWidth *= 0.3f;
					childLengthDecay *= 0.7f;
					childAngle *= 1.5f;
					childDepth += 2;
				}

				children.Add(new LightningNode(
					childWidth,
					rawEndPos + offset.RotatedBy(childAngle) * childLengthDecay,
					segmentLength,
					renderStripWidth,
					this,
					childDepth));
			}

			if (children == null)
			{
				widthProportion = 0;
			}
		}

		public void StartShrink() {
			widthShrink = true;
			if (children != null) {
				foreach (LightningNode child in children)
				{
					child.StartShrink();
				}
			}
		}

		public void AddVertexesToList(List<Vertex2D> barsList, bool drawEgde)
		{
			if (children != null)
			{
				foreach (LightningNode child in children)
				{
					child.AddVertexesToList(barsList, drawEgde);
				}
			}
			float multiplyFactor = (drawEgde ? 1 : (1 - 2 * EDGE_LINE_RATIO));
			float useEdgeColor = (drawEgde ? 1f : 0f);

			if (parent == null) {
				/*
				float currentWidth = LINE_PROPORTION * widthProgress * multiplyFactor;
				Vector3 widthLocationInfo = new Vector3(currentWidth, currentEndPos.X, currentEndPos.Y);
				Color c = new Color(0, 0, 0, useEdgeColor);

				Vertex2D upperLeft = new Vertex2D(currentEndPos + RENDER_STRIP_WIDTH * new Vector2(-1, -1), 
					new Color(0, 0, 0, useEdgeColor), widthLocationInfo);
				Vertex2D upperRight = new Vertex2D(currentEndPos + RENDER_STRIP_WIDTH * new Vector2(1, -1),
					new Color(1f, 0, 0, useEdgeColor), widthLocationInfo);
				Vertex2D lowerLeft = new Vertex2D(currentEndPos + RENDER_STRIP_WIDTH * new Vector2(-1, 1), 
					new Color(0, 1f, 0, useEdgeColor), widthLocationInfo);
				Vertex2D lowerRight = new Vertex2D(currentEndPos + RENDER_STRIP_WIDTH * new Vector2(1, 1), 
					new Color(1f, 1f, 0, useEdgeColor), widthLocationInfo);
				*/
				float currentWidth = 2 * renderStripWidth * LINE_PROPORTION * widthProgress * multiplyFactor;
				Color c = new Color(0, 0, 0, useEdgeColor);

				Vertex2D upperLeft = new Vertex2D(currentEndPos + currentWidth * new Vector2(-1, -1),
					c, new Vector3(0, 0, 0));
				Vertex2D upperRight = new Vertex2D(currentEndPos + currentWidth * new Vector2(1, -1),
					c, new Vector3(1, 0, 0));
				Vertex2D lowerLeft = new Vertex2D(currentEndPos + currentWidth * new Vector2(-1, 1),
					c, new Vector3(0, 1, 0));
				Vertex2D lowerRight = new Vertex2D(currentEndPos + currentWidth * new Vector2(1, 1),
					c, new Vector3(1, 1, 0));
				AddVertexesForOne(barsList, upperLeft, upperRight, lowerLeft, lowerRight);
			} else {
				Vector2 worldPos = Vector2.Lerp(parent.currentEndPos, currentEndPos, lengthProgress);
				Vector2 parentWorldPos = parent.currentEndPos;

				Vector2 normalDisplacement = GetRenderNormal();
				Vector2 parentNormalDisplacement = parent.GetRenderNormal();
				if (parentNormalDisplacement == Vector2.Zero)
				{
					parentNormalDisplacement = normalDisplacement;
				}

				float currentWidth = widthProportion * widthProgress * multiplyFactor;
				float parentCurrentWidth = parent.widthProportion * parent.widthProgress * multiplyFactor;

				Vertex2D upperLeft = new Vertex2D(parentWorldPos - parentNormalDisplacement, new Color(0f,0,1, useEdgeColor), new Vector3(parentCurrentWidth, parentWorldPos.X, parentWorldPos.Y));
				Vertex2D upperRight = new Vertex2D(worldPos - normalDisplacement, new Color(0f, 0, 1, useEdgeColor), new Vector3(currentWidth, worldPos.X, worldPos.Y));
				Vertex2D lowerLeft = new Vertex2D(parentWorldPos + parentNormalDisplacement, new Color(1f, 0, 1, useEdgeColor), new Vector3(parentCurrentWidth, parentWorldPos.X, parentWorldPos.Y));
				Vertex2D lowerRight = new Vertex2D(worldPos + normalDisplacement, new Color(1f, 0, 1, useEdgeColor), new Vector3(currentWidth, worldPos.X, worldPos.Y));
				AddVertexesForOne(barsList, upperLeft, upperRight, lowerLeft, lowerRight);
			}
		}

		private void AddVertexesForOne(List<Vertex2D> barsList, Vertex2D upperLeft, Vertex2D upperRight, Vertex2D lowerLeft, Vertex2D lowerRight)
		{
			barsList.Add(upperLeft);
			barsList.Add(upperRight);
			barsList.Add(lowerLeft);
			barsList.Add(lowerLeft);
			barsList.Add(upperRight);
			barsList.Add(lowerRight);
		}

		private Vector2 GetRenderNormal() {
			Vector2 renderNormal;
			if (parent == null) {
				renderNormal = Vector2.Zero;
			} else
				renderNormal = (currentEndPos - parent.currentEndPos).NormalizeSafe().RotatedBy(Math.PI / 2);
			return 0.5f * renderStripWidth * renderNormal;
		}
	}
}