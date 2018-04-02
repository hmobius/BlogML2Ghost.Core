using System;
using System.IO;
using System.Linq;
using BlogML2Ghost.Core.BlogML;
using BlogML2Ghost.Core.BlogML.Xml;
using BlogML2Ghost.Core.GhostJson;
using BlogML2Ghost.Core.ExtensionMethods;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BlogML2Ghost.Core
{
    public class BlogML2GhostConverter
    {
        BlogMLBlog BlogToConvert { get; set; }

        GhostImportDocument ghostDoc { get; set; } = new GhostImportDocument();

        public bool AssignAllToOneUser { get; set; }
        public int OneUserId { get; set; }
        public int GreatestExistingUserId { get; set; }

        public void Run()
        {
            if (!Initialize()) { return; }

            WriteStats();

            ConvertCategoriesToTags();

            ConvertAuthorsToUsers();

            ConvertPosts();

            SaveGhostFileToDesktop();
        }

        private void SaveGhostFileToDesktop()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Include;
            settings.Formatting = Formatting.Indented;
            settings.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            settings.Converters.Add(new StringEnumConverter());
            settings.Converters.Add(new FormatNumbersAsTextConverter());

            using (StreamWriter ghostStream = new StreamWriter(GenerateOutputFileLocation()))
            {
                JsonSerializer ser = JsonSerializer.Create(settings);
                ser.Serialize(ghostStream, ghostDoc);
            }
        }

        private void ConvertPosts()
        {
            foreach (BlogMLPost post in BlogToConvert.Posts)
            {
                Console.WriteLine(post.Title);

                int postauthorId = AssignAllToOneUser ? OneUserId : GetNewUserIdForPostAuthor(post.Authors[0]);

                var newPost = new GhostPost
                {
                    id = ghostDoc.data.posts.Count + 1,
                    title = post.Title,
                    slug = post.Title.AsSlug(GhostConstants.maxSlugLength),
                    html = post.Content.Text,
                    page = (post.PostType == BlogPostTypes.Article),
                    status = post.IsPublished ? GhostPostStatus.published : GhostPostStatus.draft,
                    locale = new CultureInfo("en-GB"),
                    visibility = GhostVisibility.@public,
                    author_id = postauthorId,
                    created_at = post.DateCreated,
                    created_by = postauthorId,
                    updated_at = post.DateModified,
                    updated_by = postauthorId,
                    published_at = post.DateCreated,
                    published_by = postauthorId
                };

                ghostDoc.data.posts.Add(newPost);

                foreach (BlogMLCategoryReference catref in post.Categories)
                {
                    ghostDoc.data.posts_tags.Add(
                        new GhostPostHasTag
                        {
                            post_id = newPost.id,
                            tag_id = ghostDoc.data.tags
                                                .Where(tag => tag.BlogMLid == catref.Ref)
                                                .FirstOrDefault().id,
                            sort_order = 0
                        }
                    );
                }

                if (AssignAllToOneUser)
                {
                    ghostDoc.data.posts_authors.Add(
                        new GhostPostHasAuthor
                        {
                            post_id = newPost.id,
                            author_id = OneUserId,
                            sort_order = 0
                        }
                    );
                }
                else
                {
                    foreach (BlogMLAuthorReference author in post.Authors)
                    {
                        ghostDoc.data.posts_authors.Add(
                            new GhostPostHasAuthor
                            {
                                post_id = newPost.id,
                                author_id = GetNewUserIdForPostAuthor(author),
                                sort_order = 0
                            }
                        );
                    }
                }
            }
        }

        private int GetNewUserIdForPostAuthor(BlogMLAuthorReference authorRef)
        {
            if (AssignAllToOneUser)
            {
                return OneUserId;
            }

            var userMatches = ghostDoc.data.users.Where(user => user.name.ToLower() == authorRef.Ref.ToLower());

            if (!userMatches.Any())
            {
                var newUser = new GhostUser {
                        id = GreatestExistingUserId +  ghostDoc.data.users.Count + 1,
                        name = authorRef.Ref,
                        slug = authorRef.Ref.AsSlug(GhostConstants.maxSlugLength),                
                        status = GhostUserStatus.disabled,
                        locale = new CultureInfo("en-GB"),
                        visibility = GhostVisibility.@private,
                        last_seen = DateTime.UtcNow,
                        created_at = DateTime.UtcNow,
                        created_by = 1,
                        updated_at = DateTime.UtcNow,
                        updated_by = 1
                    };

                ghostDoc.data.users.Add(newUser);
                return newUser.id;
            }

            return userMatches.First().id;
        }

        private void ConvertAuthorsToUsers()
        {
            if (AssignAllToOneUser)
            {
                return;
            }

            foreach (BlogMLAuthor author in BlogToConvert.Authors)
            {
                ghostDoc.data.users.Add(
                    new GhostUser {
                        id = GreatestExistingUserId + ghostDoc.data.users.Count + 1,
                        name = author.ID,
                        slug = author.ID.AsSlug(GhostConstants.maxSlugLength),
                        email = author.email,
                        status = GhostUserStatus.active,
                        locale = new CultureInfo("en-GB"),
                        visibility = GhostVisibility.@public,
                        last_seen = DateTime.UtcNow,
                        created_at = author.DateCreated,
                        created_by = 1,
                        updated_at = author.DateModified,
                        updated_by = 1
                    }
                );
            }
        }

        private void ConvertCategoriesToTags()
        {
            foreach (BlogMLCategory category in BlogToConvert.Categories)
            {
                ghostDoc.data.tags.Add(
                    new GhostTag {
                        id =  ghostDoc.data.tags.Count+ 1,
                        name = category.Title,
                        slug = category.Title.AsSlug(GhostConstants.maxSlugLength),
                        description = category.Description,
                        visibility = GhostVisibility.@public,
                        BlogMLid = category.ID,
                        created_at = DateTime.UtcNow,
                        created_by = 1,
                        updated_at = DateTime.UtcNow,
                        updated_by = 1                        
                    }
                );
            }
        }

        private void WriteStats()
        {
            Console.WriteLine("Number of Authors :{0}", BlogToConvert.Authors.Count);
            Console.WriteLine("Number of Posts : {0}", BlogToConvert.Posts.Count);
            Console.WriteLine("Number of Post Categories  : {0}", BlogToConvert.Categories.Count);
        }

        private string GenerateOutputFileLocation()
		{
			return  Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
				String.Format("ghost-{0}.json", DateTime.Now.ToString("HH-m-s"))
				);
		}

        private bool Initialize()
        {
            string blogMLFileLocation;
            if (!CommandLineUtils.TryGetFileLocation("BlogML File To convert", out blogMLFileLocation))
            {
                return false;
            }

            using (StreamReader blogReader = File.OpenText(blogMLFileLocation))
            {
                BlogToConvert = BlogMLSerializer.Deserialize(blogReader);
            }

            AssignAllToOneUser = CommandLineUtils.GetBoolean("Assign all posts and tags to one existing user?");

            if (AssignAllToOneUser)
            {
                OneUserId = CommandLineUtils.GetInteger("What is the ID of the one existing user?");
            }

            GreatestExistingUserId = CommandLineUtils.GetInteger("What is the highest integer id of any existing user in your ghost blog?");

            return true;
        }
    }
}