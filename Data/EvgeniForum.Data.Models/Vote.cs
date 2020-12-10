using EvgeniForum.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace EvgeniForum.Data.Models
{
    public class Vote : BaseModel<int>
    {
        public int PostId { get; set; }

        public Post Post { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public VoteType Type { get; set; }
    }
}
