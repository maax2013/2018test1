using UnityEngine;

public interface IFall
{
    Vector2Int FallFrom { get; set; }

    Vector2Int FallTo { get; set; }
}