using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class EmailStorage : MonoBehaviour
{
    //public string emailTest;

    private string formURL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeJX9BMjpxOb9IZ-uKZpSWRUVsdKoNEwRDh6ekTYg2l3Rvi0Q/formResponse"; // Replace with form link

    [SerializeField] TMP_InputField userEmail;

    //[Button]
    public void SubmitEmail()
    {
        StartCoroutine(Post(userEmail.text));
    }

    private IEnumerator Post(string userEmail)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.2006081951", userEmail);

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
