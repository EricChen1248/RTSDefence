using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;

public class ResourceHolderComponent : MonoBehaviour
{
    public RecipeItem CurrentItem { get; private set; }

    public void SetRecipeItem(RecipeItem item)
    {
        CurrentItem = item;
    }
}
