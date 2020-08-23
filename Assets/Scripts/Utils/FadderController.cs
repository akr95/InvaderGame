using System.Collections;
using UnityEngine;


namespace InVaderGame.Utils
{

    public class FadderController : Singleton<FadderController>
    {
        
        public IEnumerator FadeInOut(CanvasGroup fadderObject, float time, bool isFadeInRequest)
        {
            Debug.Log("fade in FadeInOut");

            yield return new WaitForEndOfFrame();

            float initialValue = isFadeInRequest ? 0 : 1;
            float finalValue = initialValue == 0 ? 1 : 0;
            float startTime = 0;

            fadderObject.interactable = isFadeInRequest;
            fadderObject.blocksRaycasts = isFadeInRequest;

            while (startTime <= 1)
            {
                startTime += Time.deltaTime / time;
                fadderObject.alpha = Mathf.Lerp(initialValue, finalValue, startTime);
                yield return null;

            }


        }



    }
}