using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG
{
    public interface Interactable 
    {
        void OnHoverEnter();
        void OnHoverExit();
        void OnClick();
    }
}
