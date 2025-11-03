using System.ComponentModel;

namespace Cartify.Shared;

public enum OrderByEnum : byte
{
    [Description("To Order By Title Aces/Desc")]
    Title,

    [Description("To Order By Price Aces/Desc")]
    Price,

    [Description("To Order By Sold Aces/Desc")]
    Sold,

    [Description("To Order By Ratings Average Aces/Desc")]
    RatingsAverage,

    [Description("To Order By Ratings Quantity Aces/Desc")]
    RatingsQuantity,

    [Description("To Order By Quantity Aces/Desc")]
    Quantity,

    [Description("To Order By Creation Time Aces/Desc")]
    CreationTime,

    [Description("To Order By Update Time Aces/Desc")]
    UpdateTime
}