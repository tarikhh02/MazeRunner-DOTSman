using Unity.Collections;
using Unity.Entities;

public partial struct AudioBuffer : IBufferElementData
{
    public FixedString64Bytes name;
    public bool isMusic;
}