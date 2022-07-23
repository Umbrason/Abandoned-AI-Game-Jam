using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FixedPositionLayout : LayoutGroup
{
    public override void CalculateLayoutInputVertical() => DoLayout();
    public override void SetLayoutHorizontal() => DoLayout();
    public override void SetLayoutVertical() => DoLayout();

    [SerializeField] private Matrix4x4[] trsMatrices = new Matrix4x4[0];
    public int Length { get { return trsMatrices.Length; } }


    public Matrix4x4 this[int index]
    {
        get
        {
            return trsMatrices[index].ValidTRS() ?
              trsMatrices[index]
            : Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }
        set { trsMatrices[index] = value; }
    }

    public void DoLayout()
    {
        if (Length == 0)
            return;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            rectChildren[i].transform.position = this[i % Length].MultiplyPoint(Vector3.zero) + transform.position;
            rectChildren[i].transform.rotation = this[i % Length].rotation * transform.rotation;
        }
    }
}
