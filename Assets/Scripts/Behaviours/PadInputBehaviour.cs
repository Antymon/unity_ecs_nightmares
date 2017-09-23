using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadInputBehaviour : MonoBehaviour, IEntityDeserializer {

    public KeyCode leftKeyCode;
    public KeyCode rightKeyCode;

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(leftKeyCode))
        {
            Contexts.sharedInstance.input.CreateEntity().AddKeyPressed(leftKeyCode);
        }
        else if (Input.GetKey(rightKeyCode))
        {
            Contexts.sharedInstance.input.CreateEntity().AddKeyPressed(rightKeyCode);
        }
	}

    public void DeserializeEnitity(Entitas.Entity entity)
    {
        var keyInput = ((PadEntity)entity).keyInput;

        keyInput.leftKeyCode = leftKeyCode;
        keyInput.rightKeyCode = rightKeyCode;
    }
}
