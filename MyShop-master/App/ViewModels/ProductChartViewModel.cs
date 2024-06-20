using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Chart;
using BusinessLogic.Interfaces;
using Models;

namespace App.ViewModels;
public class ProductChartViewModel
{

    private readonly IShopService _shopService;
    public ProductChartViewModel(IShopService shop)
    {
        _shopService = shop;

    }

    private async void SyncProductSoldMonthChart(int productId)
    {
        var data = await _shopService.OrderService.GetProductSoldCountByMonth(productId);
        ProductSoldMonthsChart.SyncProductSoldMonthsChart(data);
    }
    private async void SyncProductSoldYearChart(int productId)
    {
        var data = await _shopService.OrderService.GetProductSoldCountByYear(productId);
        ProductSoldYearsChart.SyncProductSoldYearsChart(data);
    }
    private async void SyncProductSoldWeekChart(int productId)
    {
        var data = await _shopService.OrderService.GetProductSoldCountByWeek(productId);
        ProductSoldWeeksChart.SyncProductSoldWeeksChart(data);
    }
    public async void SyncProductSoldDayChart(int productId)
    {
        var data = await _shopService.OrderService.GetProductSoldCountByDay(productId);
        ProductSoldDaysChart.SyncProductSoldDaysChart(data);
    }

    internal void SyncChart(int productId)
    {
        SyncProductSoldDayChart(productId);
        SyncProductSoldWeekChart(productId);
        SyncProductSoldMonthChart(productId);
        SyncProductSoldYearChart(productId);

    }

    public ProductSoldDaysChart ProductSoldDaysChart = new ("Product Sold By Day");
    public ProductSoldWeeksChart ProductSoldWeeksChart = new("Product Sold By Week");
    public ProductSoldMonthsChart ProductSoldMonthsChart = new("Product Sold By Month");
    public ProductSoldYearsChart ProductSoldYearsChart = new("Product Sold By Year");
}
