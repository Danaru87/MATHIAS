using mathiasModels.Xtend;

namespace mathiasModels.Xtend
{
    public interface IPlugin
    {
        void Install();
        void Init();
        PlugResponse DoAction(PlugCall Call);
    }
}