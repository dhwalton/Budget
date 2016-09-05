using System.Collections.Generic;

namespace Budgeter.Models
{
    public class HouseholdListViewModel
    {
        public IEnumerable<Invite> Invites;
        public IEnumerable<Household> Households;
    }
}