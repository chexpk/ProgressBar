using DefaultNamespace.System;

namespace DefaultNamespace.Progress
{
    public interface IDefaultDataCreator
    {
        public T CreateDefault<T>() where T : Data, new();
    }
}