using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EmailStorage : MonoBehaviour
{
    public string emailTest;

    private string formURL = "form link"; // Replace with form link

    //[Button]
    public void SubmitEmail()
    {
        StartCoroutine(Post(emailTest));
    }

    private IEnumerator Post(string emailTest)
    {
        WWWForm form = new WWWForm();
        form.AddField("new entry", emailTest);

        using (UnityWebRequest www = UnityWebRequest.Post(formURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Email sent successfully");
            }
            else
            {
                Debug.Log("Error in email submission: " + www.error);
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
