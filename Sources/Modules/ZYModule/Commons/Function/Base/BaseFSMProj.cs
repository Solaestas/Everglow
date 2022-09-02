

using Everglow.Sources.Commons.Core.Coroutines;
using System.Diagnostics.CodeAnalysis;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base;

internal class ProjState
{
    public string Name { get; private set; }
    public Func<int> Update { get; private set; }
    public Action Begin { get; private set; }
    public Action End { get; private set; }
    public Func<IEnumerator<ICoroutineInstruction>> Corrutine { get; private set; }
    public ProjState(string name, [NotNull] Func<int> update, Action begin, Action end, Func<IEnumerator<ICoroutineInstruction>> corrutine)
    {
        Name = name;
        Update = update;
        Begin = begin ?? EmptyAction;
        End = end ?? EmptyAction;
        Corrutine = corrutine;
    }
    private static void EmptyAction() { }
}
public class Coroutine
{
    private Stack<IEnumerator<ICoroutineInstruction>> enumerators = new Stack<IEnumerator<ICoroutineInstruction>>();
    private bool finished = false;
    private ICoroutineInstruction coroutineInstruction;
    public void Replace(IEnumerator<ICoroutineInstruction> enumerator)
    {
        enumerators.Clear();
        enumerators.Push(enumerator);
        finished = false;
    }
    public void Update()
    {
        if (finished)
        {
            return;
        }
        if (coroutineInstruction != null)
        {
            if (coroutineInstruction is AwaitForTask task)
            {
                enumerators.Push(task.Task);
            }
            else
            {
                coroutineInstruction.Update();
                if (coroutineInstruction.ShouldWait())
                {
                    return;
                }
            }
        }
        if (enumerators.Count > 0)
        {
            var enumerator = enumerators.Peek();
            if (enumerator.MoveNext())
            {
                coroutineInstruction = enumerator.Current;
                return;
            }
            else
            {
                enumerators.Pop();
                coroutineInstruction = null;
                if (enumerators.Count == 0)
                {
                    finished = true;
                }
            }
        }
    }
}
internal abstract class BaseFSMProj : BaseProjectile
{
    private List<ProjState> states = new List<ProjState>();
    private Dictionary<string, int> stateDir = new Dictionary<string, int>();
    private Coroutine coroutine = new Coroutine();
    private int stateID;
    public int StateID
    {
        get
        {
            return stateID;
        }
        set
        {
            if (stateID != value && value >= 0)
            {
                State.End();
                OnStateChange(stateID, value);
                stateID = value;
                Reset();
                State.Begin();
                if (State.Corrutine != null)
                {
                    coroutine.Replace(State.Corrutine());
                }
                StateID = State.Update();
            }
        }
    }
    public int Timer { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
    public ProjState State => states[StateID];
    public override ModProjectile Clone(Projectile newEntity)
    {
        var clone = base.Clone(newEntity) as BaseFSMProj;
        clone.states = new List<ProjState>();
        clone.stateDir = new Dictionary<string, int>();
        clone.coroutine = new Coroutine();
        return clone;
    }
    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(stateID);
    }
    public override void ReceiveExtraAI(BinaryReader reader)
    {
        StateID = reader.ReadInt32();
    }
    public virtual void OnStateChange(int from, int to) { }
    public sealed override void SetDefaults()
    {
        Initialize();
        SetUp();
    }
    public virtual void Initialize() { }
    public virtual void SetUp()
    {
        int i = 0;
        foreach (var state in states)
        {
            stateDir.Add(state.Name, i++);
        }
    }
    public virtual void Reset()
    {
        Timer = 0;
    }
    public void RegisterState(ProjState projState) => states.Add(projState);
    public void RegisterState(string name, Func<int> update, Action begin = null, Action end = null, Func<IEnumerator<ICoroutineInstruction>> func = null)
    {
        states.Add(new ProjState(name, update, begin, end, func));
    }
    public int GetStateID(string name) => stateDir[name];
    public ProjState GetState(int id) => states[id];
    public sealed override void AI()
    {
        Timer++;
        coroutine.Update();
        StateID = State.Update();
    }
    public void ForceState(int id)
    {
        State.End();
        OnStateChange(stateID, id);
        stateID = id;
        Reset();
        State.Begin();
        if (State.Corrutine != null)
        {
            coroutine.Replace(State.Corrutine());
        }
    }
}
