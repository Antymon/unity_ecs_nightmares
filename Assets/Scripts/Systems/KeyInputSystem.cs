using Entitas;
using UnityEngine;

public class KeyInputSystem : ReactiveSystem<InputEntity>
{
    public KeyInputSystem():base(Contexts.sharedInstance.input)
    {

    }

    protected override void Execute(System.Collections.Generic.List<InputEntity> entities)
    {
        foreach(var entity in entities)
        {
            var padEntities = Contexts.sharedInstance.pad.GetEntities(PadMatcher.KeyInput);

            foreach(var padEntity in padEntities)
            {
                if(padEntity.keyInput.leftKeyCode.Equals(entity.keyPressed.keyCode))
                {
                    padEntity.ballChangedDirectionListener.listener.DirectionChanged(Vector2.left * .1f);
                }
                else if(padEntity.keyInput.rightKeyCode.Equals(entity.keyPressed.keyCode))
                {

                    padEntity.ballChangedDirectionListener.listener.DirectionChanged(Vector2.right * .1f);
                }
            }

            entity.Destroy();
        }
    }

    protected override bool Filter(InputEntity entity)
    {
        return true;
    }

    protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
    {
        return context.CreateCollector<InputEntity>(InputMatcher.KeyPressed.Added());
    }
}

