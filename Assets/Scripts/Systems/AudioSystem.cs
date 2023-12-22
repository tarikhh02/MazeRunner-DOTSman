using Unity.Entities;

public partial struct AudioSystem : ISystem
{
    public void OnUpdate(ref SystemState systemState)
    {
        foreach (DynamicBuffer<AudioBuffer> audioPlays in SystemAPI.Query<DynamicBuffer<AudioBuffer>>())
        {
            for (int i = 0; i < audioPlays.Length; i++)
            {
                if (audioPlays[i].isMusic)
                    AudioManager.instance.PlayMusic(audioPlays[i].name.ToString(), true);
                else
                    AudioManager.instance.PlaySFX(audioPlays[i].name.ToString());
            }
            audioPlays.Clear();
        }
    }
    public void OnCreate(ref SystemState systemState)
    {
    }
    public void OnDestroy(ref SystemState systemState)
    {
    }
}