using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Persistence.Contexts;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<List<Post>> SearchPosts(PostSearchRequestModel model)
        {
            var query = _ikvContext.Posts.Where(p => p.IsValidated == model.IsValidated);

            if (!model.Username.IsNullOrEmpty())
            {
                query = query.Where(p => p.Username == model.Username);
            }
            
            if (!model.Category.IsNullOrEmpty())
            {
                query = query.Where(p => p.Category == model.Category);
            }
            
            if (!model.Title.IsNullOrEmpty())
            {
                query = query.Where(p => p.Title == model.Title);
            }
            
            if (!model.GameMap.IsNullOrEmpty())
            {
                query = query.Where(p => p.GameMap == model.GameMap);
            }
            
            if (model.StarDate != DateTime.MinValue)
            {
                query = query.Where(p => p.ScreenshotDate >= model.StarDate);
            }
            
            if (model.EndDate != DateTime.MinValue)
            {
                query = query.Where(p => p.ScreenshotDate <= model.EndDate);
            }
            


            query = query.OrderBy(p => p.CreatedAt);

            return await query.ToListAsync();
        }
    }
}