public interface IEffect
{
    bool Apply(GameEntity entity);
    bool IsUsed();
    bool IsApplicable(GameEntity entity);
    void Update(ulong tick);
    bool IsCollectible();
}