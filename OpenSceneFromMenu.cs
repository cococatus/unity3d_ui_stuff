using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class OpenSceneFromMenu : MonoBehaviour
 {
     [MenuItem("Oreon/Main")]
     static void Main()
     {
         OpenScene("Main");
     }
     
     [MenuItem("Oreon/Home")]
     static void Home()
     {
         OpenScene("Home");
     }

     static void OpenScene(string sceneName)
     {
         if(!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
         {
            return;
         }
         
         EditorSceneManager.OpenScene("Assets/Scenes/"+sceneName+".unity");
     }
 }