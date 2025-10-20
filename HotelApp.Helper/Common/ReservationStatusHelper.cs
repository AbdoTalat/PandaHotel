using HotelApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Helper.Common
{
    public static class ReservationStatusHelper
    {
        public static readonly List<ReservationStatus> OrderedStatuses =
            new List<ReservationStatus>
            {
            ReservationStatus.Pending,
            ReservationStatus.Confirmed,
            ReservationStatus.CheckedIn,
            ReservationStatus.CheckedOut,
            ReservationStatus.Cancelled,
            ReservationStatus.NoShow
            };
        public static string GetDisplayName(ReservationStatus status) => status switch
        {
            ReservationStatus.Pending => "Pending",
            ReservationStatus.Confirmed => "Confirmed",
            ReservationStatus.CheckedIn => "Checked In",
            ReservationStatus.CheckedOut => "Checked Out",
            ReservationStatus.Cancelled => "Cancelled",
            ReservationStatus.NoShow => "No Show",
            _ => status.ToString()
        };
        public static string GetColorClass(ReservationStatus status) => status switch
        {
            ReservationStatus.Pending => "alpha-info",
            ReservationStatus.Confirmed => "alpha-blue",
            ReservationStatus.CheckedIn => "alpha-teal",
            ReservationStatus.CheckedOut => "alpha-orange",
            ReservationStatus.Cancelled => "alpha-slate",
            ReservationStatus.NoShow => "alpha-danger",
            _ => "bg-dark"
        };
        public static IEnumerable<(int Value, string Text)> GetSelectList()
        {
            return OrderedStatuses.Select(s => ((int)s, GetDisplayName(s)));
        }

        /// Returns true if transition from one status to another is logically valid.
        public static bool IsValidTransition(ReservationStatus current, ReservationStatus next)
        {
            var validNextStatuses = current switch
            {
                ReservationStatus.Pending => new[] { ReservationStatus.Confirmed, ReservationStatus.Cancelled, ReservationStatus.NoShow },
                ReservationStatus.Confirmed => new[] { ReservationStatus.CheckedIn, ReservationStatus.Cancelled, ReservationStatus.NoShow },
                ReservationStatus.CheckedIn => new[] { ReservationStatus.CheckedOut },
                _ => Array.Empty<ReservationStatus>()
            };

            return validNextStatuses.Contains(next);
        }
        public static IEnumerable<ReservationStatus> GetNextStatuses(ReservationStatus current)
        {
            var index = OrderedStatuses.IndexOf(current);
            return index >= 0 && index < OrderedStatuses.Count - 1
                ? OrderedStatuses.Skip(index + 1) : Enumerable.Empty<ReservationStatus>();
        }
        public static IEnumerable<(string Text, string ColorClass)> GetNextStatusesWithColor(ReservationStatus current)
        {
            return GetNextStatuses(current)
                .Select(s => (GetDisplayName(s), GetColorClass(s)));
        }
        public static IEnumerable<ReservationStatus> SortStatuses(IEnumerable<ReservationStatus> statuses)
        {
            var ordered = OrderedStatuses.ToList();
            return statuses.OrderBy(s => ordered.IndexOf(s));
        }
    }
}
