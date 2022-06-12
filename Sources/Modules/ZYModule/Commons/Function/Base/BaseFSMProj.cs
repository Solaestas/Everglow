

using System.Diagnostics.CodeAnalysis;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function.Base;

internal class ProjState
{
    public string Name { get; private set; }
    public Func<int> Update { get; private set; }
    public Action Begin { get; private set; }
    public Action End { get; private set; }
    public IEnumerable<ICorrutine> Corrutines { get; private set; }
    public ProjState(string name,[NotNull] Func<int> update, Action begin, Action end, IEnumerable<ICorrutine> corrutines)
    {
        Name = name;
        Update = update;
        Begin = begin ?? EmptyAction;
        End = end ?? EmptyAction;
        Corrutines = corrutines;
    }
    private static void EmptyAction() { }
}
internal abstract class BaseFSMProj : BaseProjectile
{
    public virtual int MaxMemoryCount => 5;
    public List<ProjState> states;
    public List<int> stateMemory;
    public Dictionary<string, int> stateDir;
    public int StateID
    {
        get
        {
            return (int)Projectile.ai[0];
        }
        set
        {

        }
    }
    public ProjState State => states[StateID];
    public override bool PreAI()
    {
        return base.PreAI();
    }
    public override void AI()
    {
        base.AI();
    }
    public override void PostAI()
    {
        base.PostAI();
    }
    public override bool PreDraw(ref Color lightColor)
    {
        return base.PreDraw(ref lightColor);
    }
    public override void PostDraw(Color lightColor)
    {
        base.PostDraw(lightColor);
    }

}
