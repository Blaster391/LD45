using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFX : MonoBehaviour
{
    [SerializeField]
    private PowerPanel m_power;

    private List<GameObject> m_effectPoints = new List<GameObject>();
    private bool m_endGame = false;
    private bool m_finished =  false;
    private float m_finishedTime = 0.0f;

    private bool m_disable = false;
   

    Material blackMaterial;
    Material blackAndWhiteMaterial;
    Material funkyMaterial;

    Material pointMultiColourMaterial;
    Material pointMultiColourFunkyMaterial;
    Material passThroughMaterial;
    Material finishedMaterial;

    [SerializeField]
    List<Color> m_setOne;
    [SerializeField]
    List<Color> m_setTwo;
    [SerializeField]
    List<Color> m_setThree;

    // Start is called before the first frame update
    void Start()
    {
        blackMaterial = new Material(Shader.Find("Hidden/BlackShader"));
        blackAndWhiteMaterial = new Material(Shader.Find("Hidden/BlackAndWhiteShader"));
        funkyMaterial = new Material(Shader.Find("Hidden/MultiColourShader"));
        pointMultiColourMaterial = new Material(Shader.Find("Hidden/PointMultiColourShader"));
        pointMultiColourFunkyMaterial = new Material(Shader.Find("Hidden/PointMultiColourFunkyShader"));
        passThroughMaterial = new Material(Shader.Find("Hidden/PassThroughShader"));
        finishedMaterial = new Material(Shader.Find("Hidden/FinishedShader"));
    }

    public void Finished()
    {
        m_finished = true;
    }

    public void AddPointFX(GameObject effectPoint)
    {
        m_effectPoints.Add(effectPoint);
    }

    public void SetEndGame(bool end)
    {
        m_endGame = end;
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
        if (m_power.CorePower.PowerLevel == 0)
        {
            mainScreenMaterial = blackMaterial;
        }
        else if (m_power.CorePower.PowerLevel == 1)
        {
            mainScreenMaterial = blackAndWhiteMaterial;
        }
        else if(m_power.CorePower.PowerLevel == 3)
        {
            mainScreenMaterial = funkyMaterial;
            funkyMaterial.SetColor("_Colour_One", m_setOne[0]);
            funkyMaterial.SetColor("_Colour_Two", m_setTwo[0]);
            funkyMaterial.SetColor("_Colour_Three", m_setThree[0]);
        }
        Graphics.Blit(source, temp1, mainScreenMaterial);

        RenderTexture from = temp1;
        RenderTexture to = temp2;

        Vector2 endPos = new Vector2();

        foreach (var point in m_effectPoints)
        {
            float radius = 0.15f;
            
            EndGameScript endGame = point.GetComponent<EndGameScript>();
            Powerball ball = point.GetComponent<Powerball>();
            if (endGame)
            {
                radius = 0.3f;
                
                //if (!m_endGame)
                //{
                //    continue;
                //}
            }
            else if (m_power.CorePower.PowerLevel == 0)
            {
                
                if (m_endGame || !ball || ball.Type != PowerType.Core || ball.State == BallState.Free)
                {
                    continue;
                }
            }

            Vector2 pos = Camera.main.WorldToScreenPoint(point.gameObject.transform.position);
            pos.x = pos.x / source.width;
            pos.y = pos.y / source.height;

            if(endGame)
            {
                endPos = pos;
            }

            if((pos.x > -1 || pos.x < 2) && (pos.y > -1 || pos.y < 2))
            {
                Material mat = pointMultiColourMaterial;
                if (endGame || ball.Type == PowerType.Core)
                {
                    mat = pointMultiColourFunkyMaterial;
                    mat.SetColor("_Colour_One", m_setThree[0]);
                    mat.SetColor("_Colour_Two", m_setOne[0]);
                    mat.SetColor("_Colour_Three", m_setTwo[0]);
                }

                mat.SetTexture("_OriginalTex", source);
                mat.SetFloat("_radius", radius);
                mat.SetVector("_point", pos);
                Graphics.Blit(from, to, mat);
                RenderTexture t = to;
                to = from;
                from = t;
            }
        }



        if(m_finished)
        {

            m_finishedTime += Time.deltaTime;
            finishedMaterial.SetTexture("_OriginalTex", source);
            finishedMaterial.SetFloat("_time", m_finishedTime);
            finishedMaterial.SetVector("_point", endPos);
            Graphics.Blit(from, to, finishedMaterial);
            RenderTexture t = to;
            to = from;
            from = t;

            if(m_finishedTime > 25.0f)
            {
                SceneManager.LoadScene("End", LoadSceneMode.Single); 
            }
        }



        Graphics.Blit(from, destination, passThroughMaterial);

        RenderTexture.ReleaseTemporary(temp1);
        RenderTexture.ReleaseTemporary(temp2);

    }
}
