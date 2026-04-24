# Gilded Rose Solution Summary

## Approach Taken

The solution keeps the requirement shape by leaving `Item` unchanged and by keeping `GildedRose` dependent on `IList<Item>`. This was done to stay close to the original public interface while still improving the maintainability of the update logic.

The original `UpdateQuality` method was refactored into smaller updater classes. Each updater class is responsible for one category of item:

- `NormalItemUpdater`
- `AgedBrieUpdater`
- `BackstagePassUpdater`
- `ConjuredItemUpdater`
- `SulfurasUpdater`

`GildedRose` now selects the correct updater using a simple `if`-based selector on the item name. This approach was chosen because it is easy to read, stays close to the requirement, and avoids pushing item-type knowledge into callers.

## Why This Approach Was Taken

This approach was chosen for the following reasons:

- It improves readability by separating business rules by item type instead of keeping all logic inside one large nested method.
- It keeps the external contract simple because callers still create plain `Item` objects.
- It makes it easier to add new item types such as `Conjured` without increasing complexity in `GildedRose.UpdateQuality`.
- It avoids changing the `Item` class, which is explicitly restricted by the requirements.

## Business Rules Implemented

The solution implements the following rules:

- Normal items decrease in quality by `1`, or by `2` after the sell date.
- `Aged Brie` increases in quality as it gets older, and increases faster after the sell date.
- `Backstage passes` increase in quality as the concert approaches, with larger increases at the stated thresholds, and drop to `0` after the concert.
- `Sulfuras` never changes.
- `Conjured` items degrade twice as fast as normal items.
- Quality is never below `0`.
- Quality is never above `40`.

## Testing Approach

The unit tests were updated to cover the main item categories and edge cases:

- normal item degradation
- expired item degradation
- `Aged Brie` growth before and after expiration
- backstage pass threshold behavior
- backstage pass drop to zero after the concert
- `Sulfuras` stability
- `Conjured` accelerated degradation
- quality lower and upper bounds

This test coverage was added to protect the refactor and confirm that the new structure still satisfies the required behavior.
