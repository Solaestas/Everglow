using Everglow.Commons.Skeleton2D.Renderer.DrawCommands;
using Everglow.Commons.Vertex;
using Spine;

namespace Everglow.Commons.Skeleton2D.Renderer;

/// <summary>Draws region and mesh attachments.</summary>
public class SkeletonRenderer
{
	private const int TL = 0;
	private const int TR = 1;
	private const int BL = 2;
	private const int BR = 3;

	private SkeletonClipping clipper = new SkeletonClipping();
	private RasterizerState rasterizerState;
	private float[] vertices = new float[8];
	private int[] quadTriangles = { 0, 1, 2, 2, 3, 0 };
	private BlendState defaultBlendState;
	private bool premultipliedAlpha;

	public bool PremultipliedAlpha
	{
		get { return premultipliedAlpha; }
		set { premultipliedAlpha = value; }
	}

	public bool UseEnvironmentLight { get; set; }

	/// <summary>
	/// 绘制的时候将模型整体偏移的量
	/// </summary>
	public Vector2 DrawOffset { get; set; }

	/// <summary>
	/// 绘制的时候使用的绘制/变换中心，是TPose状态下的坐标，为空则为默认(root骨骼）中心
	/// </summary>
	public Vector2? DrawOrigin { get; set; }

	/// <summary>
	/// 绘制的时候使用的额外的旋转，DrawOrigin不为空的时候才会生效
	/// </summary>
	public float DrawRotation { get; set; }

	/// <summary>Attachments are rendered back to front in the x/y plane by the SkeletonRenderer.
	/// Each attachment is offset by a customizable z-spacing value on the z-axis to avoid z-fighting
	/// in shaders with ZWrite enabled. Typical values lie in the range [-0.1, 0].</summary>
	private float zSpacing = 0.0f;

	public float ZSpacing
	{
		get { return zSpacing; }
		set { zSpacing = value; }
	}

	/// <summary>A Z position offset added at each vertex.</summary>
	private float z = 0.0f;

	public float Z
	{
		get { return z; }
		set { z = value; }
	}

	public SkeletonRenderer()
	{
		UseEnvironmentLight = false;
		rasterizerState = new RasterizerState();
		rasterizerState.CullMode = CullMode.None;

		Bone.yDown = true;
	}

	public DrawCommandList Draw(Skeleton2D skeleton2d)
	{
		defaultBlendState = premultipliedAlpha ? BlendState.AlphaBlend : BlendState.NonPremultiplied;

		Skeleton skeleton = skeleton2d.Skeleton;
		var drawOrder = skeleton.DrawOrder;
		var drawOrderItems = skeleton.DrawOrder.Items;
		float skeletonR = skeleton.R, skeletonG = skeleton.G, skeletonB = skeleton.B, skeletonA = skeleton.A;
		Color color = default(Color);

		DrawCommandList commandList = new DrawCommandList();

		for (int i = 0, n = drawOrder.Count; i < n; i++)
		{
			Slot slot = drawOrderItems[i];
			Attachment attachment = slot.Attachment;
			float attachmentZOffset = z + zSpacing * i;

			float attachmentColorR, attachmentColorG, attachmentColorB, attachmentColorA;
			object textureObject = null;
			int verticesCount = 0;
			float[] vertices = this.vertices;
			int indicesCount = 0;
			int[] indices = null;
			float[] uvs = null;

			if (attachment is RegionAttachment)
			{
				RegionAttachment regionAttachment = (RegionAttachment)attachment;
				attachmentColorR = regionAttachment.R;
				attachmentColorG = regionAttachment.G;
				attachmentColorB = regionAttachment.B;
				attachmentColorA = regionAttachment.A;
				AtlasRegion region = (AtlasRegion)regionAttachment.RendererObject;
				textureObject = region.page.rendererObject;
				verticesCount = 4;
				regionAttachment.ComputeWorldVertices(slot.Bone, vertices, 0, 2);
				indicesCount = 6;
				indices = quadTriangles;
				uvs = regionAttachment.UVs;
			}
			else if (attachment is MeshAttachment)
			{
				MeshAttachment mesh = (MeshAttachment)attachment;
				attachmentColorR = mesh.R;
				attachmentColorG = mesh.G;
				attachmentColorB = mesh.B;
				attachmentColorA = mesh.A;
				AtlasRegion region = (AtlasRegion)mesh.RendererObject;
				textureObject = region.page.rendererObject;
				int vertexCount = mesh.WorldVerticesLength;
				if (vertices.Length < vertexCount)
				{
					vertices = new float[vertexCount];
				}

				verticesCount = vertexCount >> 1;
				mesh.ComputeWorldVertices(slot, vertices);
				indicesCount = mesh.Triangles.Length;
				indices = mesh.Triangles;
				uvs = mesh.UVs;
			}
			else if (attachment is ClippingAttachment)
			{
				ClippingAttachment clip = (ClippingAttachment)attachment;
				clipper.ClipStart(slot, clip);
				continue;
			}
			else
			{
				continue;
			}

			// set blend state
			BlendState blend = slot.Data.BlendMode == BlendMode.Additive ? BlendState.Additive : defaultBlendState;

			// calculate color
			float a = skeletonA * slot.A * attachmentColorA;
			if (premultipliedAlpha)
			{
				color = new Color(
						skeletonR * slot.R * attachmentColorR * a,
						skeletonG * slot.G * attachmentColorG * a,
						skeletonB * slot.B * attachmentColorB * a, a);
			}
			else
			{
				color = new Color(
						skeletonR * slot.R * attachmentColorR,
						skeletonG * slot.G * attachmentColorG,
						skeletonB * slot.B * attachmentColorB, a);
			}

			Color darkColor = default(Color);
			if (slot.HasSecondColor)
			{
				if (premultipliedAlpha)
				{
					darkColor = new Color(slot.R2 * a, slot.G2 * a, slot.B2 * a);
				}
				else
				{
					darkColor = new Color(slot.R2 * a, slot.G2 * a, slot.B2 * a);
				}
			}
			darkColor.A = premultipliedAlpha ? (byte)255 : (byte)0;

			// clip
			if (clipper.IsClipping)
			{
				clipper.ClipTriangles(vertices, verticesCount << 1, indices, indicesCount, uvs);
				vertices = clipper.ClippedVertices.Items;
				verticesCount = clipper.ClippedVertices.Count >> 1;
				indices = clipper.ClippedTriangles.Items;
				indicesCount = clipper.ClippedTriangles.Count;
				uvs = clipper.ClippedUVs.Items;
			}

			if (verticesCount == 0 || indicesCount == 0)
			{
				continue;
			}

			// submit to batch
			// MeshItem item = batcher.NextItem(verticesCount, indicesCount);
			PipelineStateObject pso = new PipelineStateObject()
			{
				RasterizerState = rasterizerState,
				BlendState = blend,
			};
			if (textureObject is Texture2D)
			{
				pso.Texture = (Texture2D)textureObject;

				// TextureAssets.MagicPixel.Value;
			}
			else
			{
				throw new NotImplementedException();
			}
			List<Vertex2D> renderVertices = new List<Vertex2D>();
			List<int> renderIndices = new List<int>();

			// itemVertices[ii].Color = color;
			// itemVertices[ii].Color2 = darkColor;
			// itemVertices[ii].Position.X = vertices[v];
			// itemVertices[ii].Position.Y = vertices[v+1];
			// itemVertices[ii].Position.Z = attachmentZOffset;
			// itemVertices[ii].TextureCoordinate.X = uvs[v];
			// itemVertices[ii].TextureCoordinate.Y = uvs[v + 1];
			// if (VertexEffect != null)
			// VertexEffect.Transform(ref itemVertices[ii]);
			for (int ii = 0, nn = indicesCount; ii < nn; ii++)
			{
				renderIndices.Add(indices[ii]);
			}
			for (int ii = 0, v = 0, nn = verticesCount << 1; v < nn; ii++, v += 2)
			{
				Color lightColor = color;
				Vector2 position = new Vector2(vertices[v], vertices[v + 1]);
				if (UseEnvironmentLight)
				{
					Vector2 worldPos = new Vector2(vertices[v], vertices[v + 1]);
					Point tileCoord = worldPos.ToTileCoordinates();
					lightColor = Lighting.GetColor(tileCoord.X, tileCoord.Y).MultiplyRGB(color);
					lightColor.A = color.A;
				}
				if (DrawOrigin.HasValue)
				{
					Vector2 preTransformPos = position - new Vector2(skeleton.RootBone.WorldX, skeleton.RootBone.WorldY);
					preTransformPos = preTransformPos.RotatedBy(-skeleton.RootBone.rotation * MathUtils.DegRad);
					preTransformPos = preTransformPos - DrawOrigin.Value;
					position = preTransformPos.RotatedBy(DrawRotation) + DrawOrigin.Value + new Vector2(skeleton.RootBone.WorldX, skeleton.RootBone.WorldY);
				}
				Vertex2D drawVertex = new Vertex2D(
					position + DrawOffset,
					lightColor,
					new Vector3(uvs[v], uvs[v + 1], 0));
				renderVertices.Add(drawVertex);
			}

			commandList.EmitDrawIndexedTriangleMesh(pso, renderVertices, renderIndices);

			clipper.ClipEnd(slot);
		}
		clipper.ClipEnd();
		return commandList;
	}

	/// <summary>
	/// Non-formal function, use cautiously.
	/// </summary>
	/// <param name="skeleton2d"></param>
	/// <param name="texture"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public DrawCommandList DrawWithOtherTexture(Skeleton2D skeleton2d, Texture2D texture)
	{
		defaultBlendState = premultipliedAlpha ? BlendState.AlphaBlend : BlendState.NonPremultiplied;

		Skeleton skeleton = skeleton2d.Skeleton;
		var drawOrder = skeleton.DrawOrder;
		var drawOrderItems = skeleton.DrawOrder.Items;
		float skeletonR = skeleton.R, skeletonG = skeleton.G, skeletonB = skeleton.B, skeletonA = skeleton.A;
		Color color = default(Color);

		DrawCommandList commandList = new DrawCommandList();

		for (int i = 0, n = drawOrder.Count; i < n; i++)
		{
			Slot slot = drawOrderItems[i];
			Attachment attachment = slot.Attachment;
			float attachmentZOffset = z + zSpacing * i;

			float attachmentColorR, attachmentColorG, attachmentColorB, attachmentColorA;
			object textureObject = texture;
			int verticesCount = 0;
			float[] vertices = this.vertices;
			int indicesCount = 0;
			int[] indices = null;
			float[] uvs = null;

			if (attachment is RegionAttachment)
			{
				RegionAttachment regionAttachment = (RegionAttachment)attachment;
				attachmentColorR = regionAttachment.R;
				attachmentColorG = regionAttachment.G;
				attachmentColorB = regionAttachment.B;
				attachmentColorA = regionAttachment.A;
				AtlasRegion region = (AtlasRegion)regionAttachment.RendererObject;
				textureObject = texture;
				verticesCount = 4;
				regionAttachment.ComputeWorldVertices(slot.Bone, vertices, 0, 2);
				indicesCount = 6;
				indices = quadTriangles;
				uvs = regionAttachment.UVs;
			}
			else if (attachment is MeshAttachment)
			{
				MeshAttachment mesh = (MeshAttachment)attachment;
				attachmentColorR = mesh.R;
				attachmentColorG = mesh.G;
				attachmentColorB = mesh.B;
				attachmentColorA = mesh.A;
				AtlasRegion region = (AtlasRegion)mesh.RendererObject;
				textureObject = texture;
				int vertexCount = mesh.WorldVerticesLength;
				if (vertices.Length < vertexCount)
				{
					vertices = new float[vertexCount];
				}

				verticesCount = vertexCount >> 1;
				mesh.ComputeWorldVertices(slot, vertices);
				indicesCount = mesh.Triangles.Length;
				indices = mesh.Triangles;
				uvs = mesh.UVs;
			}
			else if (attachment is ClippingAttachment)
			{
				ClippingAttachment clip = (ClippingAttachment)attachment;
				clipper.ClipStart(slot, clip);
				continue;
			}
			else
			{
				continue;
			}

			// set blend state
			BlendState blend = slot.Data.BlendMode == BlendMode.Additive ? BlendState.Additive : defaultBlendState;

			// calculate color
			float a = skeletonA * slot.A * attachmentColorA;
			if (premultipliedAlpha)
			{
				color = new Color(
						skeletonR * slot.R * attachmentColorR * a,
						skeletonG * slot.G * attachmentColorG * a,
						skeletonB * slot.B * attachmentColorB * a, a);
			}
			else
			{
				color = new Color(
						skeletonR * slot.R * attachmentColorR,
						skeletonG * slot.G * attachmentColorG,
						skeletonB * slot.B * attachmentColorB, a);
			}

			Color darkColor = default(Color);
			if (slot.HasSecondColor)
			{
				if (premultipliedAlpha)
				{
					darkColor = new Color(slot.R2 * a, slot.G2 * a, slot.B2 * a);
				}
				else
				{
					darkColor = new Color(slot.R2 * a, slot.G2 * a, slot.B2 * a);
				}
			}
			darkColor.A = premultipliedAlpha ? (byte)255 : (byte)0;

			// clip
			if (clipper.IsClipping)
			{
				clipper.ClipTriangles(vertices, verticesCount << 1, indices, indicesCount, uvs);
				vertices = clipper.ClippedVertices.Items;
				verticesCount = clipper.ClippedVertices.Count >> 1;
				indices = clipper.ClippedTriangles.Items;
				indicesCount = clipper.ClippedTriangles.Count;
				uvs = clipper.ClippedUVs.Items;
			}

			if (verticesCount == 0 || indicesCount == 0)
			{
				continue;
			}

			// submit to batch
			// MeshItem item = batcher.NextItem(verticesCount, indicesCount);
			PipelineStateObject pso = new PipelineStateObject()
			{
				RasterizerState = rasterizerState,
				BlendState = blend,
			};
			if (textureObject is Texture2D)
			{
				pso.Texture = (Texture2D)textureObject;

				// TextureAssets.MagicPixel.Value;
			}
			else
			{
				throw new NotImplementedException();
			}
			List<Vertex2D> renderVertices = new List<Vertex2D>();
			List<int> renderIndices = new List<int>();

			// itemVertices[ii].Color = color;
			// itemVertices[ii].Color2 = darkColor;
			// itemVertices[ii].Position.X = vertices[v];
			// itemVertices[ii].Position.Y = vertices[v+1];
			// itemVertices[ii].Position.Z = attachmentZOffset;
			// itemVertices[ii].TextureCoordinate.X = uvs[v];
			// itemVertices[ii].TextureCoordinate.Y = uvs[v + 1];
			// if (VertexEffect != null)
			// VertexEffect.Transform(ref itemVertices[ii]);
			for (int ii = 0, nn = indicesCount; ii < nn; ii++)
			{
				renderIndices.Add(indices[ii]);
			}
			for (int ii = 0, v = 0, nn = verticesCount << 1; v < nn; ii++, v += 2)
			{
				Color lightColor = color;
				Vector2 position = new Vector2(vertices[v], vertices[v + 1]);
				if (UseEnvironmentLight)
				{
					Vector2 worldPos = new Vector2(vertices[v], vertices[v + 1]);
					Point tileCoord = worldPos.ToTileCoordinates();
					lightColor = Lighting.GetColor(tileCoord.X, tileCoord.Y).MultiplyRGB(color);
					lightColor.A = color.A;
				}
				if (DrawOrigin.HasValue)
				{
					Vector2 preTransformPos = position - new Vector2(skeleton.RootBone.WorldX, skeleton.RootBone.WorldY);
					preTransformPos = preTransformPos.RotatedBy(-skeleton.RootBone.rotation * MathUtils.DegRad);
					preTransformPos = preTransformPos - DrawOrigin.Value;
					position = preTransformPos.RotatedBy(DrawRotation) + DrawOrigin.Value + new Vector2(skeleton.RootBone.WorldX, skeleton.RootBone.WorldY);
				}
				Vertex2D drawVertex = new Vertex2D(
					position + DrawOffset,
					lightColor,
					new Vector3(uvs[v], uvs[v + 1], 0));
				renderVertices.Add(drawVertex);
			}

			commandList.EmitDrawIndexedTriangleMesh(pso, renderVertices, renderIndices);

			clipper.ClipEnd(slot);
		}
		clipper.ClipEnd();
		return commandList;
	}
}