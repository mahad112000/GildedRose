using System.Collections.Generic;
using GildedRoseKata;
using Xunit;

namespace GildedRoseTests;

public class GildedRoseTests
{
    [Fact]
    public void NormalItem_DegradesByOneBeforeSellDate()
    {
        var item = UpdateItem(new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 10 });

        Assert.Equal(4, item.SellIn);
        Assert.Equal(9, item.Quality);
    }

    [Fact]
    public void NormalItem_DegradesByTwoOnSellDateAndAfter()
    {
        var item = UpdateItem(new Item { Name = "Elixir of the Mongoose", SellIn = 0, Quality = 10 });

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(8, item.Quality);
    }

    [Fact]
    public void Quality_IsNeverNegative()
    {
        var item = UpdateItem(new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 0 });

        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void AgedBrie_IncreasesInQuality()
    {
        var item = UpdateItem(new Item { Name = "Aged Brie", SellIn = 5, Quality = 10 });

        Assert.Equal(4, item.SellIn);
        Assert.Equal(11, item.Quality);
    }

    [Fact]
    public void AgedBrie_IncreasesByTwoAfterSellDate()
    {
        var item = UpdateItem(new Item { Name = "Aged Brie", SellIn = 0, Quality = 10 });

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(12, item.Quality);
    }

    [Fact]
    public void AgedBrie_AlreadyExpired_StillIncreasesByTwo()
    {
        var item = UpdateItem(new Item { Name = "Aged Brie", SellIn = -1, Quality = 10 });

        Assert.Equal(-2, item.SellIn);
        Assert.Equal(12, item.Quality);
    }

    
    [Fact]
    public void Sulfuras_NeverChanges()
    {
        var item = UpdateItem(new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 40 });

        Assert.Equal(0, item.SellIn);
        Assert.Equal(40, item.Quality);
    }

    [Theory]
    [InlineData(8, 11)]
    [InlineData(7, 13)]
    [InlineData(2, 14)]
    public void BackstagePasses_IncreaseBasedOnSellIn(int sellIn, int expectedQuality)
    {
        var item = UpdateItem(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = sellIn,
            Quality = 10
        });

        Assert.Equal(expectedQuality, item.Quality);
    }

    [Fact]
    public void BackstagePasses_DropToZeroAfterConcert()
    {
        var item = UpdateItem(new Item
        {
            Name = "Backstage passes to a TAFKAL80ETC concert",
            SellIn = 0,
            Quality = 10
        });

        Assert.Equal(0, item.Quality);
    }

    [Fact]
    public void ConjuredItems_DegradeTwiceAsFastAsNormalItems()
    {
        var item = UpdateItem(new Item { Name = "Conjured Mana Cake", SellIn = 5, Quality = 10 });

        Assert.Equal(4, item.SellIn);
        Assert.Equal(8, item.Quality);
    }

    [Fact]
    public void ExpiredConjuredItems_DegradeTwiceAsFastAsExpiredNormalItems()
    {
        var item = UpdateItem(new Item { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 });

        Assert.Equal(-1, item.SellIn);
        Assert.Equal(6, item.Quality);
    }

    private static Item UpdateItem(Item item)
    {
        var items = new List<Item> { item };
        var app = new GildedRose(items);

        app.UpdateQuality();

        return item;
    }
}
