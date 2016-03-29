using mathiasModels.Xtend;

namespace EmailPlug
{
    public interface IPlugin
    {
        void Install();
        void Init();
        PlugResponse DoAction(PlugCall Call);
    }
}