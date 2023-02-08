using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace TP_Blazor.Shared;

public partial class TableTemplate<TItem>
{
    [Parameter]
    public RenderFragment? TableHeader { get; set; }

    [Parameter]
    public RenderFragment<TItem>? RowTemplate { get; set; }

    [Parameter, AllowNull]
    public IReadOnlyList<TItem> Items { get; set; }
}