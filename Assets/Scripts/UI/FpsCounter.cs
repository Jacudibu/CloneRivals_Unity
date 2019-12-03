using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FpsCounter : MonoBehaviour
{
    private Text _text;

    private int _frameCount = 0;
    private float _elapsedTime = 0;
    private const float UpdateRate = 0.25f;

    public static float FPS { get; private set; }

    private void Start() 
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        _frameCount++;
        _elapsedTime += Time.unscaledDeltaTime;

        if (!(_elapsedTime > UpdateRate))
        {
            return;
        }
        
        FPS = _frameCount / _elapsedTime;
        _frameCount = 0;
        _elapsedTime -= UpdateRate;
            
        _text.text = Mathf.Round(FPS) + " FPS";
    }
}
