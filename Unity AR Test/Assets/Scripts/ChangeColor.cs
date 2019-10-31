using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField]
    Material mat;
    List<Color> colors = new List<Color>();
    [SerializeField]
    float timedColorSwap;
    // Start is called before the first frame update
    void Start()
    {
        AddColors();
        mat.SetColor("_RimColor", colors[Random.Range(0, colors.Count)]);
        StartCoroutine(changeColor());
    }

    void AddColors()
    {
        colors.Add(Color.blue);
        colors.Add(Color.red);
        colors.Add(Color.yellow);
        colors.Add(Color.green);
        colors.Add(Color.magenta);
        colors.Add(Color.white);
    }

    IEnumerator changeColor()
    {
        yield return new WaitForSeconds(timedColorSwap);
        mat.SetColor("_RimColor", colors[Random.Range(0, colors.Count)]);
        StartCoroutine(changeColor());
    }
}
