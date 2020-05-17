using Landfill.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using static Landfill.Common.Enums.EnumsContainer;

namespace Landfill.DAL.Implementation
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Content>().HasData
                 (
                new Content
                {
                    Id = 1,
                    State = State.Published,
                    ContentType = ContentType.FAQ,
                },
                 new Content
                 {
                     Id = 2,
                     State = State.Modified,
                     ContentType = ContentType.FAQ,
                 },
                  new Content
                  {
                      Id = 3,
                      State = State.Deleted,
                      ContentType = ContentType.FAQ,
                  },
                   new Content
                   {
                       Id = 4,
                       ContentType = ContentType.Announcement,
                       State = State.Deleted,
                   },
                   new Content
                   {
                       Id = 5,
                       ContentType = ContentType.Announcement,
                       State = State.Modified,
                   },
                     new Content
                     {
                         Id = 6,
                         ContentType = ContentType.Announcement,
                         State = State.Published,
                     });


            modelBuilder.Entity<FAQ>().HasData
                (
                new FAQ
                {
                    Id = 1,
                    ContentId = 1,
                    Tag = "First tag"

                },
             new FAQ
             {
                 Id = 2,
                 ContentId = 2,
                 Tag = "Second tag"

             },
               new FAQ
               {
                   Id = 3,
                   ContentId = 3,
                   Tag = "Third tag"

               }
               );

            modelBuilder.Entity<Announcement>().HasData
                (
                new Announcement { Id = 4, ContentId = 4, Header = "Short header", ValiUntil = DateTime.Now.AddDays(10) },
                new Announcement { Id = 5, ContentId = 5, Header = "Long header", ValiUntil = DateTime.Now.AddDays(20) },
                new Announcement { Id = 6, ContentId = 6, Header = "New header", ValiUntil = DateTime.Now.AddDays(30) }
                );

            modelBuilder.Entity<ContentTranslation>().HasData(
                new ContentTranslation { ContentId = 1, Id = 1, Language = Language.UA, Text = "Питання 1" },
                new ContentTranslation { ContentId = 2, Id = 2, Language = Language.UA, Text = "Питання 2" },
                new ContentTranslation { ContentId = 3, Id = 3, Language = Language.UA, Text = "Питання 3" },
                new ContentTranslation { ContentId = 1, Id = 4, Language = Language.EN, Text = "Qwestion 1" },
                new ContentTranslation { ContentId = 2, Id = 5, Language = Language.EN, Text = "Qwestion 2" },
                new ContentTranslation { ContentId = 3, Id = 6, Language = Language.EN, Text = "Qwestion 3" },
                new ContentTranslation { ContentId = 4, Id = 7, Language = Language.UA, Text = "Оголошення 1" },
                new ContentTranslation { ContentId = 5, Id = 8, Language = Language.UA, Text = "Оголошення 2" },
                new ContentTranslation { ContentId = 6, Id = 9, Language = Language.UA, Text = "Оголошення 3" },
                new ContentTranslation { ContentId = 4, Id = 10, Language = Language.EN, Text = "Annnouncement 1" },
                new ContentTranslation { ContentId = 5, Id = 11, Language = Language.EN, Text = "Annnouncement 2" },
                new ContentTranslation { ContentId = 6, Id = 12, Language = Language.EN, Text = "Annnouncement 3" }
            );

        }
    }
}
