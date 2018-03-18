using System;
using System.IO;
using System.Linq;
using BlogML2Ghost.Core.BlogML;
using BlogML2Ghost.Core.BlogML.Xml;
using BlogML2Ghost.Core.GhostJson;
using BlogML2Ghost.Core.ExtensionMethods;
using System.Globalization;
using Newtonsoft.Json;

namespace BlogML2Ghost.Core
{
    public class BlogML2GhostConverter
    {
        BlogMLBlog BlogToConvert { get; set; }

        GhostImportDocument ghostDoc { get; set; } = new GhostImportDocument();

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
            JsonSerializer ser = new JsonSerializer();
            ser.NullValueHandling = NullValueHandling.Include;

            using (StreamWriter ghostStream = new StreamWriter(GenerateOutputFileLocation()))
            using (JsonWriter ghostWriter = new JsonTextWriter(ghostStream))
            {
                ser.Serialize(ghostWriter, ghostDoc);
            }
        }

        private void ConvertPosts()
        {
            foreach (BlogMLPost post in BlogToConvert.Posts)
            {
                Console.WriteLine(post.Title);

                var newPost = new GhostPost
                {
                    id = ghostDoc.data.posts.Count + 1,
                    title = post.Title,
                    slug = post.Title.AsSlug(GhostConstants.maxSlugLength),
                    html = post.Content.Text,
                    page = (post.PostType == BlogPostTypes.Article),
                    status = post.IsPublished ? GhostPostStatus.published : GhostPostStatus.draft,
                    language = new CultureInfo("en-GB"),
                    created_at = post.DateCreated.AsMillisecondsSinceEpoch(),
                    updated_at = post.DateModified.AsMillisecondsSinceEpoch(),
                    published_at = post.DateCreated.AsMillisecondsSinceEpoch(),
                    author_id = GetNewUserIdForPostAuthor(post.Authors[0])
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
                                                .FirstOrDefault().id
                        }
                    );
                }


            }
        }

        private int GetNewUserIdForPostAuthor(BlogMLAuthorReference authorRef)
        {
            var userMatches = ghostDoc.data.users.Where(user => user.name.ToLower() == authorRef.Ref.ToLower());

            if (!userMatches.Any())
            {
                var newUser = new GhostUser {
                        id = ghostDoc.data.users.Count + 1,
                        name = authorRef.Ref,
                        slug = authorRef.Ref.AsSlug(GhostConstants.maxSlugLength),
                        status = GhostUserStatus.disabled,
                        language = new CultureInfo("en-GB"),
                        created_at = DateTime.UtcNow.AsMillisecondsSinceEpoch(),
                        updated_at = DateTime.UtcNow.AsMillisecondsSinceEpoch()
                    };

                ghostDoc.data.users.Add(newUser);
                return newUser.id;
            }

            return userMatches.First().id;
        }

        private void ConvertAuthorsToUsers()
        {
            // TODO Ask about culture for each author as processed.
            foreach (BlogMLAuthor author in BlogToConvert.Authors)
            {
                ghostDoc.data.users.Add(
                    new GhostUser {
                        id = ghostDoc.data.users.Count + 1,
                        name = author.ID,
                        slug = author.ID.AsSlug(GhostConstants.maxSlugLength),
                        email = author.email,
                        status = GhostUserStatus.active,
                        language = new CultureInfo("en-GB"),
                        created_at = author.DateCreated.AsMillisecondsSinceEpoch(),
                        updated_at = author.DateModified.AsMillisecondsSinceEpoch()
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
                        id = ghostDoc.data.tags.Count+ 1,
                        name = category.Title,
                        slug = category.Title.AsSlug(GhostConstants.maxSlugLength),
                        description = category.Description,
                        BlogMLid = category.ID
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
            Console.Write("BlogML File To convert (full path)? ");
            var filelocation = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filelocation))
            {
                Console.WriteLine("{0} is not a valid file location", filelocation);
                return false;
            }

            if (!File.Exists(filelocation))
            {
                Console.WriteLine("{0} does not exist", filelocation);
                return false;
            }

            // if (BlogMLIsInvalid(filelocation))
            // {
            //     Console.WriteLine("{0} is not a valid BlogML file", filelocation);
            //     return false;
            // }

            using (StreamReader blogReader = File.OpenText(filelocation))
            {
                BlogToConvert = BlogMLSerializer.Deserialize(blogReader);
            }

            return true;
        }
    }
}