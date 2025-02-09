using System.Text;

namespace DoFramework.Mappers;

public class ToolingArgMapper : IMapper<object[], string>
{
    public string Map(object[] source)
    {
        var cmd = new StringBuilder($"doing");

        for (int i = 0; i < source.Length; i++)
        {
            cmd.Append($" {source[i]}");
        }

        return cmd.ToString();
    }
}
