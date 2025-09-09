using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public interface IUI_Sound
{
    public SoundData_SO InteractSound
    { get; set; }

    public void ImplementSoundListeners(List<Selectable> selectables)
    {
        foreach (Selectable selectableItem in selectables)
        {
            EventTrigger eventTrigger;

            if (!selectableItem.TryGetComponent(out eventTrigger))
            {
                eventTrigger = selectableItem.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };

            entry.callback.AddListener((eventData) => PlaySound());

            eventTrigger.triggers.Add(entry);
        }
    }

    public void PlaySound()
    {
        if (SoundManager.Instance != null && SoundManager.Instance.SFXManagerScript != null)
        {
            if (InteractSound != null)
            {
                SoundManager.Instance.SFXManagerScript.PlayOneShotSFX(InteractSound.AudioClip);
            }
            else
            {
                Debug.LogError("Audio has not been assigned into the UI interact sound SO.");
            }
        }
        else
        {
            Debug.LogError("The singleton or the SFXManager has not be instantiated.");
        }
    }
}
