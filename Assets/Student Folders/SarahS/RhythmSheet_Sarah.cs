using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RhythmSheet", menuName = "Scriptable Objects/RhythmSheet_Sarah")]
public class RhythmSheet_Sarah : ThingOption
{
    public bool Loop = true;
    public List<RhythmSection_Sarah> Sections = new List<RhythmSection_Sarah>();

    public float GetInterval(int sectionIndex)
    {
        if (Sections == null || Sections.Count == 0) return 0.75f;
        sectionIndex = Mathf.Clamp(sectionIndex, 0, Sections.Count - 1);
        float bpm = Mathf.Max(1f, Sections[sectionIndex].BPM);
        return 60f / bpm;
    }

    public float GetDuration(int sectionIndex)
    {
        if (Sections == null || Sections.Count == 0) return 999f;
        sectionIndex = Mathf.Clamp(sectionIndex, 0, Sections.Count - 1);
        return Mathf.Max(0.1f, Sections[sectionIndex].Duration);
    }
    
    public int SectionCount => Sections == null ? 0 : Sections.Count;
}

[System.Serializable]
public class RhythmSection_Sarah
{
    public float BPM;
    public float Duration;
}
