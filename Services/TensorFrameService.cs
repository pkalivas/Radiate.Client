using Radiate.Tensors;

namespace Radiate.Client.Services;

public interface ITensorFrameService
{
    void SetTensorFrame(Guid runId, TensorFrame tensorFrame);
    TensorFrame? GetTensorFrame(Guid runId);
}

public class TensorFrameService : ITensorFrameService
{
    private readonly Dictionary<Guid, TensorFrame> _tensorFrames = new();

    public void SetTensorFrame(Guid runId, TensorFrame tensorFrame)
    {
        _tensorFrames[runId] = tensorFrame;
    }

    public TensorFrame? GetTensorFrame(Guid runId)
    {
        if (_tensorFrames.TryGetValue(runId, out var tensorFrame))
        {
            return tensorFrame;
        }

        return null;
    }
}