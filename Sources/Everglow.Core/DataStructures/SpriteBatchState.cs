using Everglow.Commons.Utilities;

namespace Everglow.Commons.DataStructures;

public record struct SpriteBatchState(
	SpriteSortMode SortMode,
	BlendState BlendState,
	SamplerState SamplerState,
	DepthStencilState DepthStencilState,
	RasterizerState RasterizerState,
	Matrix TransformMatrix,
	Effect Effect)
{
	private static Stack<SpriteBatchState?> states = new();

	public static SpriteBatchState Immediate => new SpriteBatchState(
		SpriteSortMode.Immediate,
		BlendState.AlphaBlend,
		SamplerState.PointClamp,
		DepthStencilState.None,
		RasterizerState.CullNone,
		Matrix.Identity,
		null);

	public static SpriteBatchState Defered => new SpriteBatchState(
		SpriteSortMode.Deferred,
		BlendState.AlphaBlend,
		SamplerState.PointClamp,
		DepthStencilState.None,
		RasterizerState.CullNone,
		Matrix.Identity,
		null);

	public readonly DrawState DrawState => new(BlendState, SamplerState, DepthStencilState, RasterizerState);

	public readonly IDisposable BeginScope(SpriteBatch spriteBatch)
	{
		SpriteBatchState? oldState = null;

		if (spriteBatch.beginCalled)
		{
			spriteBatch.End();
			oldState = spriteBatch.GetState();
		}

		spriteBatch.Begin(this);

		return new Scope(spriteBatch, oldState);
	}

	public static void Push()
	{
		var sb = Ins.SpriteBatch;
		states.Push(Ins.SpriteBatch.GetState());
		if (sb.beginCalled)
		{
			sb.End();
		}
	}

	public static void Pop()
	{
		var sb = Ins.SpriteBatch;
		Debug.Assert(states.Count != 0, "Pop too many times");
		var state = states.Pop();
		if (state != null)
		{
			sb.Begin(state.Value);
		}
	}

	private class Scope : IDisposable
	{
		private readonly SpriteBatchState? _state;
		private readonly SpriteBatch _spriteBatch;

		public Scope(SpriteBatch spriteBatch, SpriteBatchState? state)
		{
			_state = state;
			_spriteBatch = spriteBatch;
		}

		public void Dispose()
		{
			_spriteBatch.End();
			if (_state != null)
			{
				_spriteBatch.Begin(_state.Value);
			}
		}
	}
}