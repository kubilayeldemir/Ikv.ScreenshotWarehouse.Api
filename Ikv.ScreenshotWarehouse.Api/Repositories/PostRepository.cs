using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Contexts;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ikv.ScreenshotWarehouse.Api.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly IkvContext _ikvContext;

        public PostRepository(IkvContext ikvContext)
        {
            _ikvContext = ikvContext;
        }

        public async Task<Post> GetPostById(string postId)
        {
            return await _ikvContext.Posts.Where(x => x.Id == postId).Include(x => x.User).FirstOrDefaultAsync();
        }

        public async Task<Post> SavePost(Post post)
        {
            await _ikvContext.Posts.AddAsync(post);
            await _ikvContext.SaveChangesAsync();
            return post;
        }

        public async Task<List<Post>> SavePostBulk(List<Post> posts)
        {
            await _ikvContext.Posts.AddRangeAsync(posts);
            return posts;
        }
    }
}