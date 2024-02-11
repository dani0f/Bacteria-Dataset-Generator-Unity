using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Samplers;
using UnityEngine.Perception.Randomization.Randomizers.Tags;



[Serializable]
[AddRandomizerMenu("MyAnimationRandomizer")]
public class MyAnimationRandomizer : Randomizer
{

    const string k_ClipName = "PlayerIdle";
    const string k_StateName = "Base Layer.RandomState";

    [Tooltip("Duration of the animation in seconds")]
    public float animationDuration = 15f;

    [Tooltip("Sampler for generating random values")]
    public UniformSampler timeSampler = new UniformSampler(0f, 1f); // Specify the correct range for UniformSampler



    void RandomizeAnimation(AnimationRandomizerTag tag)
    {
        if (!tag.gameObject.activeInHierarchy)
            return;

        var animator = tag.gameObject.GetComponent<Animator>();
        //Debug.Log(animator);
        animator.applyRootMotion = tag.applyRootMotion;

        var overrider = tag.animatorOverrideController;
        if (overrider != null && tag.animationClips.Count > 0)
        {
            overrider[k_ClipName] = tag.animationClips.Sample();

            // Select a random time within the animation duration
            float randomTime = timeSampler.Sample() * animationDuration;
            //Debug.Log(randomTime);
            // Set the time and play the animation
            animator.Play(k_StateName, 0, randomTime / animationDuration);
        }
    }
     

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<AnimationRandomizerTag>();
        foreach (var tag in tags)
           RandomizeAnimation(tag);
      
    }
}
