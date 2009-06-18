using System.Collections.Generic;

namespace Dante
{
    public interface ISharedAssembly
    {
        void DoString(string command);
        List<string> GetValues();
    }
}
