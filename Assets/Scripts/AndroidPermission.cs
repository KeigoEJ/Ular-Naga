using UnityEngine;
using UnityEngine.Android;

public class AndroidPermission : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID
        // Android 13+ permission
        if (!Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_IMAGES"))
            Permission.RequestUserPermission("android.permission.READ_MEDIA_IMAGES");

        // Older Android permissions (still needed for compatibility)
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
#endif
    }
}
