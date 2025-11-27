using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Iaphub.Example.Shared.ViewModels;

public abstract partial class ViewModelBase : ObservableObject, IDisposable
{
    public virtual void Dispose()
    {
    }
}
