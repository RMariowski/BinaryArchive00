using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace BinaryArchive00.App.Helpers;

public class ObservableRangeCollection<T> : ObservableCollection<T>
{
    public void AddRange(IEnumerable<T> collection,
        NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
    {
        ArgumentNullException.ThrowIfNull(collection);

        if (notificationMode != NotifyCollectionChangedAction.Add &&
            notificationMode != NotifyCollectionChangedAction.Reset)
        {
            throw new ArgumentException("Mode must be either Add or Reset for AddRange.", nameof(notificationMode));
        }

        CheckReentrancy();

        var startIndex = Count;

        var itemsAdded = AddArrangeCore(collection);
        if (!itemsAdded)
            return;

        if (notificationMode == NotifyCollectionChangedAction.Reset)
        {
            RaiseChangeNotificationEvents(action: NotifyCollectionChangedAction.Reset);
            return;
        }

        var changedItems = collection as List<T> ?? [..collection];

        RaiseChangeNotificationEvents(
            action: NotifyCollectionChangedAction.Add,
            changedItems: changedItems,
            startingIndex: startIndex);
    }

    public void ReplaceRange(IEnumerable<T> collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        CheckReentrancy();

        var previouslyEmpty = Items.Count == 0;

        Items.Clear();

        AddArrangeCore(collection);

        var currentlyEmpty = Items.Count == 0;
        if (previouslyEmpty && currentlyEmpty)
            return;

        RaiseChangeNotificationEvents(action: NotifyCollectionChangedAction.Reset);
    }

    private bool AddArrangeCore(IEnumerable<T> collection)
    {
        foreach (var item in collection)
            Items.Add(item);
        return collection.Any();
    }

    private void RaiseChangeNotificationEvents(NotifyCollectionChangedAction action, List<T>? changedItems = null,
        int startingIndex = -1)
    {
        OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));

        OnCollectionChanged(changedItems is null
            ? new NotifyCollectionChangedEventArgs(action)
            : new NotifyCollectionChangedEventArgs(action, changedItems: changedItems, startingIndex: startingIndex));
    }
}
