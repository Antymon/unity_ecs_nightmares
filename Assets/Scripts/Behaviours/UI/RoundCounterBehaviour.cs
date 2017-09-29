using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class RoundCounterBehaviour : BindingEntitasBehaviour
{
    private const string ROUND_TEXT_FORMAT = "Round:{0}";

    public Text roundTextField;

    override public void DeserializeEnitity(GameEntity entity)
    {
        base.DeserializeEnitity(entity);

        roundTextField.text = string.Format(ROUND_TEXT_FORMAT,entity.roundCounter.round);
    }
}

