using mathiasModels.Xtend;

namespace EmailPlug
{
    public interface IPlugin
    {
        PlugResponse DoAction(PlugCall Call);
    }
}