using System;

namespace UILogic
{
    public interface IMenuActionBehavior
    {
        Action<MenuActionArgs> InvokeMethod { get; }
    }
}
