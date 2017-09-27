using Entitas;

public interface IEffectsFactory
{
    IEffect Create<T>() where T : IEffect, new();
}

public class EffectsFactory : IEffectsFactory
{
    public IEffect Create<T>() where T:IEffect,new()
    {
        return new T();
    }
}

