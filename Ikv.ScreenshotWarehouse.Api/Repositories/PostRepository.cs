﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ikv.ScreenshotWarehouse.Api.Helpers;
using Ikv.ScreenshotWarehouse.Api.Persistence.Contexts;
using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels;
using Ikv.ScreenshotWarehouse.Api.V1.Models.ResponseModels;
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

        public async Task<List<Post>> GetPostsByIdsBulk(List<string> postIds)
        {
            return await _ikvContext.Posts.Where(p => postIds.Contains(p.Id)).ToListAsync();
        }
        
        public async Task<Post> UpdatePost(Post post)
        {
            await _ikvContext.SaveChangesAsync();
            return post;
        }
        
        public async Task<List<Post>> UpdatePosts(List<Post> post)
        {
            await _ikvContext.SaveChangesAsync();
            return post;
        }

        public async Task<Post> SavePost(Post post)
        {
            await _ikvContext.Posts.AddAsync(post);
            await _ikvContext.SaveChangesAsync();
            return post;
        }

        public async Task<List<Post>> SavePostBulk(List<Post> posts)
        {
            await _ikvContext.Posts.AddRangeAsync(posts, new CancellationToken());
            await _ikvContext.SaveChangesAsync();
            return posts;
        }

        public async Task<List<Post>> SearchPosts(PostSearchRequestModel model)
        {
            var query = PostSearchQuery(model);
            query = PostSortQuery(model, query);
            return await query.ToListAsync();
        }

        public async Task<List<Post>> GetRandomPosts()
        {
            var totalCount = _ikvContext.Posts.Count(p => p.IsValidated);

            var random = new Random();
            var offset = random.Next(Math.Max(0, totalCount - 10));
            var orderByRandomIndex = random.Next(4);

            IQueryable<Post> query = orderByRandomIndex switch
            {
                0 => _ikvContext.Posts.OrderBy(p => p.Id),
                1 => _ikvContext.Posts.OrderBy(p => p.Md5),
                2 => _ikvContext.Posts.OrderBy(p => p.ScreenshotDate),
                3 => _ikvContext.Posts.OrderBy(p => p.CreatedAt),
                _ => _ikvContext.Posts.OrderBy(p => p.Id)
            };

            return await query
                .Where(p => p.IsValidated)
                .Skip(offset)
                .Take(10)
                .ToListAsync();
        }


        public async Task<PagedResult<Post>> SearchPostsPaged(PostSearchRequestModel model, PagingRequestModel pagingModel)
        {
            var query = PostSearchQuery(model);
            query = PostSortQuery(model, query);
            return await query.GetPagedResultAsync(pagingModel.CurrentPage, pagingModel.PageSize);
        }
        
        public async Task<List<string>> CheckPostExistsByMd5(List<string> md5List)
        {
            var result = await _ikvContext.Posts.Where(p => md5List.Contains(p.Md5)).Select(p => p.Md5).ToListAsync();
            return result;
        }

        private IQueryable<Post> PostSearchQuery(PostSearchRequestModel model)
        {
            var query = _ikvContext.Posts.AsQueryable();
            if (model.OnlyNonValidatedPosts)
            {
                query = _ikvContext.Posts.Where(p => p.IsValidated == false);
            }
            
            else if (!model.IgnoreValidation)
            {
                query = _ikvContext.Posts.Where(p => p.IsValidated == true);
            }

            if (model.IncludeRawData)
            {
                query = _ikvContext.Posts.Include(post => post.PostRawData);
            }

            if (!model.Username.IsNullOrEmpty())
            {
                query = query.Where(p => p.Username == model.Username);
            }

            if (model.UserId != 0)
            {
                query = query.Where(p => p.UserId == model.UserId);
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
            
            if (!model.GameServer.IsNullOrEmpty())
            {
                query = query.Where(p => p.GameServer == model.GameServer);
            }
            
            if (!model.TitleMd5.IsNullOrEmpty())
            {
                query = query.Where(p => p.TitleMd5 == model.TitleMd5);
            }

            if (model.StarDate != DateTime.MinValue)
            {
                query = query.Where(p => p.ScreenshotDate >= model.StarDate);
            }

            if (model.EndDate != DateTime.MinValue)
            {
                query = query.Where(p => p.ScreenshotDate <= model.EndDate);
            }

            return query;
        }

        private static IQueryable<Post> PostSortQuery(PostSearchRequestModel model, IQueryable<Post> query)
        {
            if (model.SortField != null)
            {
                switch (model.SortField.ToLower())
                {
                    case "screenshotdate":
                        if (model.SortDirection == "ASC")
                        {
                            query = query.OrderBy(p => p.ScreenshotDate);
                        }
                        else
                        {
                            query = query.OrderByDescending(p => p.ScreenshotDate);
                        }

                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            return query;
        }
    }
}