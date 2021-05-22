using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace DungeonNexus.ViewModel
{
    public class ViewModelBase : ReactiveObject
    {
        [Inject]
        [AllowNull]
        private IServiceScopeFactory ScopeFactory { get; set; }

        protected async Task InvokeTransaction(Func<Task> action)
        {
            using var scope = ScopeFactory.CreateScope();
            await action();
        }

        protected async Task<T> InvokeTransaction<T>(Func<Task<T>> action)
        {
            using var scope = ScopeFactory.CreateScope();
            return await action();
        }
    }
}
