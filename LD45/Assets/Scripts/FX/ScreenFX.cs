using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFX : MonoBehaviour
{
    [SerializeField]
    private PowerPanel m_power;

    private List<GameObject> m_effectPoints = new List<GameObject>();
    private bool m_endGame = false;

    private bool m_disable = false;

    Material blackMaterial;
    Material blackAndWhiteMaterial;

    Material pointMultiColourMaterial;
    Material passThroughMaterial;
    // Start is called before the first frame update
    void Start()
    {
        blackMaterial = new Material(Shader.Find("Hidden/BlackShader"));
        blackAndWhiteMaterial = new Material(Shader.Find("Hidden/BlackAndWhiteShader"));
        pointMultiColourMaterial = new Material(Shader.Find("Hidden/PointMultiColourShader"));
        passThroughMaterial = new Material(Shader.Find("Hidden/PassThroughShader"));
    }

    public void AddPointFX(GameObject effectPoint)
    {
        m_effectPoints.Add(effectPoint);
    }

    public void SetEndGame(bool end)
    {
        m_endGame = true;
    }

    public void SetDisabled(bool disabled)
    {
        m_disable = disabled;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(m_disable)
        {
            return;
        }

        RenderTexture temp1 = RenderTexture.GetTemporary(source.width, source.height);
        RenderTexture temp2 = RenderTexture.GetTemporary(source.width, source.height);

        Material mainScreenMaterial = passThroughMaterial;
        //Power level check
        if (m_power.CorePower.PowerLevel == 0 || m_endGame)
        {
            mainScreenMaterial = blackMaterial;
        }
        else if (m_power.CorePower.PowerLevel == 1)
        {
            mainScreenMaterial = blackAndWhiteMaterial;
        }
        Graphics.Blit(source, temp1, mainScreenMaterial);

        RenderTexture from = temp1;
        RenderTexture to = temp2;

        float radius = 0.2f;
        foreach (var point in m_effectPoints)
        {
            if(m_endGame)
            {
                radius = 0.6f;
                EndGameScript endGame = point.GetComponent<EndGameScript>();
                if(!endGame)
                {
                    continue;
                }
            }
            if (m_power.CorePower.PowerLevel == 0)
            {
                EndGameScript endGame = point.GetComponent<EndGameScript>();
                if (endGame)
                {
                    continue;
                }
                    Powerball ball = point.GetComponent<Powerball>();
                if(!ball || ball.Type != PowerType.Core || ball.State == BallState.Free)
                {
                    continue;
                }
            }

            Vector2 pos = Camera.main.WorldToScreenPoint(point.gameObject.transform.position);
            pos.x = pos.x / source.width;
            pos.y = pos.y / source.height;

            if((pos.x > -1 || pos.x < 2) && (pos.y > -1 || pos.y < 2))
            {
                
                pointMultiColourMaterial.SetTexture("_OriginalTex", source);
                pointMultiColourMaterial.SetFloat("_radius", radius);
                pointMultiColourMaterial.SetVector("_point", pos);
                Graphics.Blit(from, to, pointMultiColourMaterial);
                RenderTexture t = to;
                to = from;
                from = t;
            }
        }

        Graphics.Blit(from, destination, passThroughMaterial);

        RenderTexture.ReleaseTemporary(temp1);
        RenderTexture.ReleaseTemporary(temp2);

    }
}
