using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Services.Runners;


public class RunInput
{
    public List<RunInputValue> Inputs { get; set; } = new();
    
    public T GetInputValue<T>(string name)
    {
        var input = Inputs.FirstOrDefault(x => x.Name == name);
        if (input == null)
        {
            throw new ArgumentException($"Input with name {name} not found.");
        }

        return (T)Convert.ChangeType(input.Value, typeof(T));
    }
}