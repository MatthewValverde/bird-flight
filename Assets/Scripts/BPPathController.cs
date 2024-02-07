using System.Collections.Generic;

public class BPPathController : BPTweeningBase
{
    public TweenBasePath[] sequence;
    Dictionary<string, TweenBasePath> sequenceDictionary;

    private void Start()
    {
        sequenceDictionary = new Dictionary<string, TweenBasePath>();
        foreach (TweenBasePath seq in sequence)
        {
            sequenceDictionary.Add(seq.trigger, seq);
        }
    }
    public void StartPath(string trigger)
    {
        if (sequenceDictionary.TryGetValue(trigger, out TweenBasePath seq))
        {
            SimplePath(seq);
        }
    }
}
