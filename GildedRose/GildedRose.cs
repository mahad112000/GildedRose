using System;
using System.Collections.Generic;

namespace GildedRoseKata;

public class GildedRose
{
    private readonly IList<Item> items;

    public GildedRose(IList<Item> items)
    {
        this.items = items;
    }

    public void UpdateQuality()
    {
        foreach (var item in items)
        {
            GetUpdater(item).Update(item);
        }
    }

    private static ItemUpdater GetUpdater(Item item)
    {
        if (item.Name == "Aged Brie")
        {
            return new AgedBrieUpdater();
        }

        if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
        {
            return new BackstagePassUpdater();
        }

        if (item.Name == "Sulfuras, Hand of Ragnaros")
        {
            return new SulfurasUpdater();
        }

        if (item.Name.StartsWith("Conjured", StringComparison.Ordinal))
        {
            return new ConjuredItemUpdater();
        }

        return new NormalItemUpdater();
    }
}

internal abstract class ItemUpdater
{
    protected const int MaxQuality = 40;

    public abstract void Update(Item item);

    protected static void DecreaseSellIn(Item item)
    {
        item.SellIn--;
    }

    protected static void ChangeQuality(Item item, int amount)
    {
        item.Quality = Math.Clamp(item.Quality + amount, 0, MaxQuality);
    }
}

internal sealed class NormalItemUpdater : ItemUpdater
{
    public override void Update(Item item)
    {
        var decay = item.SellIn <= 0 ? 2 : 1;
        ChangeQuality(item, -decay);
        DecreaseSellIn(item);
    }
}

internal sealed class ConjuredItemUpdater : ItemUpdater
{
    public override void Update(Item item)
    {
        var decay = item.SellIn <= 0 ? 4 : 2;
        ChangeQuality(item, -decay);
        DecreaseSellIn(item);
    }
}

internal sealed class AgedBrieUpdater : ItemUpdater
{
    public override void Update(Item item)
    {
        ChangeQuality(item, 1);
        DecreaseSellIn(item);

        if (item.SellIn < 0)
        {
            ChangeQuality(item, 1);
        }
    }
}

internal sealed class BackstagePassUpdater : ItemUpdater
{
    public override void Update(Item item)
    {
        if (item.SellIn <= 0)
        {
            item.Quality = 0;
            DecreaseSellIn(item);
            return;
        }

        var increase = 1;

        if (item.SellIn <= 7)
        {
            increase = 3;
        }

        if (item.SellIn <= 2)
        {
            increase = 4;
        }

        ChangeQuality(item, increase);
        DecreaseSellIn(item);
    }
}

internal sealed class SulfurasUpdater : ItemUpdater
{
    public override void Update(Item item)
    {
    }
}
