using Entitas;

public class TickSystem : IInitializeSystem, IExecuteSystem
{
    private InputContext context;

    public TickSystem(InputContext context)
    {
        this.context = context;
    }

    public void Initialize()
    {
        context.SetTick(0);
    }

    public void Execute()
    {
        context.ReplaceTick(context.tick.currentTick + 1);
    }
}

