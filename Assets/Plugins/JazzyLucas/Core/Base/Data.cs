namespace JazzyLucas.Core
{
    public abstract class Data<TBlueprint> where TBlueprint : Blueprint
    {
        public abstract void Init(TBlueprint blueprint);
    }
}