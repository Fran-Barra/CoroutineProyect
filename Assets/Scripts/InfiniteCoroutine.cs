using System.Collections;
using UnityEngine;

public class InfiniteCoroutine : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GarbageGenerator());
    }

    private IEnumerator GarbageGenerator()
    {
        while (true)
        {
            string garbage = "this is a string to generate garbage";
            yield return null;
        }
    }
}
