public interface IEffect
{
    void Apply(GameEntity entity);
    bool IsUsed();
    bool CanApply(GameEntity entity);
    void Update();
    bool IsCollectible();
}