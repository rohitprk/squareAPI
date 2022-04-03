using System.Globalization;
using CsvHelper.Configuration;
using SquareAPI.Data.Entities;

namespace SquareAPI.Business.Models
{
    /// <summary>
    /// CSVHelper map class to ignore UserId property.
    /// </summary>
    public sealed class UserPointMap : ClassMap<UserPoint>
    {

        /// <summary>
        /// Constructor to set property ignore.
        /// </summary>
        public UserPointMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.UserId).Ignore();
        }
    }
}