using Edap.Models;

namespace Edap.Shared;

public class _PollOptionTableModel {
    public Poll Poll { get; init; }
    public bool IsReadOnly { get; init; }
    
    public static _PollOptionTableModel Create(Poll poll, bool isReadOnly = false)
        => new (poll, isReadOnly);
    
    private _PollOptionTableModel(Poll poll, bool isReadOnly) {
        Poll = poll;
        IsReadOnly = isReadOnly;
    }
}