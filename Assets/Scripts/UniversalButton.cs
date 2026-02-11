using UnityEngine;
using UnityEngine.Events; // Needed for the 'UnityEvent' magic

public class UniversalButton : MonoBehaviour
{
    // This creates a box in the Inspector just like a Button's OnClick!
    public UnityEvent actionAfterAnim; 

    // This is what your Animation Event marker should call
    public void ExecuteButtonLogic()
    {
        Debug.Log(gameObject.name + " finished anim, now doing its thing!");
        
        // This fires whatever you put in the box in the Inspector
        actionAfterAnim.Invoke(); 
    }
}