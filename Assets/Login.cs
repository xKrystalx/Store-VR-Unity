using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class Login : MonoBehaviour
{
    public GameObject usernameObject;
    public GameObject passwordObject;
    public GameObject loginButtonObject;
    public GameObject productsController;

    public string loginPage = "http://localhost:3000/login";

    public void validateData() {
        Debug.Log("Validating data...");
        var usernameInput = usernameObject.GetComponent<TMP_InputField>();
        var passwordInput = passwordObject.GetComponent<TMP_InputField>();

        if (usernameInput.text.Length > 0 && passwordInput.text.Length > 0) {
            StartCoroutine(sendRequest(usernameInput.text, passwordInput.text));
        }
        else {
            return;
        }
    }

    IEnumerator sendRequest(string username, string password) {
        //clearError();
        loginButtonObject.transform.parent.gameObject.GetComponent<Button>().interactable = false;

        var values = new Dictionary<string, string>
        {
            { "username", username },
            { "password", password }
        };

        var textComponent = loginButtonObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = "Logging In...";

        var request = UnityWebRequest.Post(loginPage, values);

        yield return request.SendWebRequest();

        if(request.result != UnityWebRequest.Result.Success) {
            Debug.Log(request.error);
            StartCoroutine(LoginErrorHandler(2.0f, textComponent));
            yield break;
        }

        var responseString = request.downloadHandler.text;
        Response myObject = new Response();
        myObject = JsonUtility.FromJson<Response>(responseString);


        if (myObject.jwt != null) {
            User.Instance.token = myObject.jwt;
            User.Instance.userName = username;
            Debug.Log("[Login] Token: " + myObject.jwt + "\nUsername: " + username);
            textComponent.text = "Downloading...";
            productsController.GetComponent<ProductsController>().GetProducts(result => {
                if (result) {
                    textComponent.text = "Success";
                }
                else {
                    textComponent.text = "Error downloading!";
                    StartCoroutine(LoginErrorHandler(2.0f, textComponent));
                }
            });
        }
    }
    IEnumerator LoginErrorHandler(float time, TMPro.TextMeshProUGUI field) {
        float currentTime = 0.0f;
        while(currentTime < time) {
            currentTime += Time.deltaTime;
        }
        field.text = "Log In";
        loginButtonObject.transform.parent.gameObject.GetComponent<Button>().interactable = true;
        yield break;
    }
}
