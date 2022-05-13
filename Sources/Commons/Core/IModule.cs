namespace Everglow.Sources.Commons
{
    public interface IModule
    {
        public string Name { get; }
        public string Description { get; }
        public void Load();
        public void Unload();
    }
}
