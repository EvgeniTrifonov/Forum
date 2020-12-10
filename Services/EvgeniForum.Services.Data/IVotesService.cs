namespace EvgeniForum.Services.Data
{
    using System.Threading.Tasks;

    public interface IVotesService
    {
        Task VoteAsync(int postId, string userId, bool isUpVote);
    }
}
