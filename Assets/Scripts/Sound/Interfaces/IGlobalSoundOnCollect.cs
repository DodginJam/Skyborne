using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGlobalSoundOnCollect
{
    public SoundData_SO SoundCollect
    { get; set; }

    public SoundData_SO SoundMiss
    { get; set; }

    public void PlaySoundOnCollect()
    {
        if (SoundManager.Instance != null && SoundManager.Instance.SFXManagerScript != null)
        {
            if (SoundCollect == null)
            {
                Debug.LogError("The audio source has not been assigned.");
                return;
            }

            SoundManager.Instance.SFXManagerScript.PlayOneShotSFX(SoundCollect.AudioClip);
        }
    }

    public void PlaySoundOnMiss()
    {
        if (SoundManager.Instance != null && SoundManager.Instance.SFXManagerScript != null)
        {
            if (SoundMiss == null)
            {
                Debug.LogError("The audio source has not been assigned.");
                return;
            }

            SoundManager.Instance.SFXManagerScript.PlayOneShotSFX(SoundMiss.AudioClip);
        }
    }
}
