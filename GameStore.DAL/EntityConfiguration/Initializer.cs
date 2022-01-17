using GameStore.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameStore.DAL.EntityConfiguration
{
    public static class Initializer
    {
        public static void GenreSeed(EntityTypeBuilder<Genre> builder)
        {
            builder.HasData(
                        new Genre { Id = 1, Name = "Strategy" },
                        new Genre { Id = 2, Name = "RPG" },
                        new Genre { Id = 3, Name = "Sports" },
                        new Genre { Id = 4, Name = "Races" },
                        new Genre { Id = 5, Name = "Action" },
                        new Genre { Id = 6, Name = "Adventure" },
                        new Genre { Id = 7, Name = "Puzzle & Skill" },
                        new Genre { Id = 8, Name = "Misc." },
                        new Genre { Id = 9, Name = "RTS", ParentGenreId = 1 },
                        new Genre { Id = 10, Name = "TBS", ParentGenreId = 1 },
                        new Genre { Id = 11, Name = "Rally", ParentGenreId = 4 },
                        new Genre { Id = 12, Name = "Arcade", ParentGenreId = 4 },
                        new Genre { Id = 13, Name = "Formula", ParentGenreId = 4 },
                        new Genre { Id = 14, Name = "Off-road", ParentGenreId = 4 },
                        new Genre { Id = 15, Name = "FPS", ParentGenreId = 5 },
                        new Genre { Id = 16, Name = "TPS", ParentGenreId = 5 },
                        new Genre { Id = 17, Name = "Shooter" },
                        new Genre { Id = 18, Name = "Open World" }
                        );
        }

        public static void PlatformTypeSeed(EntityTypeBuilder<PlatformType> builder)
        {
            builder.HasData(
                new PlatformType { Id = 1, Type = "Mobile" },
                new PlatformType { Id = 2, Type = "Desktop" },
                new PlatformType { Id = 3, Type = "Browser" },
                new PlatformType { Id = 4, Type = "Console" }
                );
        }

        public static void GameSeed(EntityTypeBuilder<Game> builder)
        {
            builder.HasData(
                new Game()
                {
                    Id = 1,
                    Key = "GTA V",
                    Name = "Grand Theft Auto V and Criminal Enterprise Starter Pack Bundle",
                    Description = "Rockstar Games ESRB Rating M - Mature 17 + DRM Rockstar Games Social Club Purchase this bundle to get Grand Theft Auto V and the Criminal Enterprise Starter Pack for Grand Theft Auto Online. Please do not purchase this bundle if you already own the Criminal Enterprise Starter Pack",
                    Price = 45,
                    UnitsInStock = 5,
                    PublisherId = 1
                },
                new Game()
                {
                    Id = 2,
                    Key = "FF XIV",
                    Name = "Final Fantasy XIV: Shadowbringers - Standard Edition PC",
                    Description = "Please note that the FINAL FANTASY XIV: SHADOWBRINGERS expansion pack also includes FINAL FANTASY XIV: HEAVENSWARD & FINAL FANTASY XIV: STORMBLOOD. This package requires FINAL FANTASY XIV Online Starter Edition (A Realm Reborn) to play the game. Notice: This product is not compatible with the Steam DRM",
                    Price = 35,
                    UnitsInStock = 8,
                    PublisherId = 2
                },
                new Game()
                {
                    Id = 3,
                    Key = "COD G",
                    Name = "Call of Duty: Ghosts PC Game",
                    Description = "Activision M - Mature Genre First Person Shooter",
                    Price = 32,
                    UnitsInStock = 3,
                    PublisherId = 3
                },
                new Game()
                {
                    Id = 4,
                    Key = "W III WH",
                    Name = "The Witcher III: Wild Hunt",
                    Description = "In the past he has raised and overthrown monarchs, battled legendary monsters and saved the lives of many. Now Geralt embarks on his most personal quest to save his loved ones and protect the world from an ancient threat. The story is drawn based on player decisions.Each action will have consequences which change the story and the game world.NPCs, communities, monsters and locations all change, based on player choice.",
                    Price = 44,
                    UnitsInStock = 9,
                    PublisherId = 4
                },
                new Game()
                {
                    Id = 5,
                    Key = "Crysis",
                    Name = "Crysis",
                    Description = "What begins as a simple rescue mission becomes the battleground of a new war as alien invaders swarm over a North Korean island chain. Armed with a powerful Nanosuit, players can become invisible to stalk enemy patrols, or boost strength to lay waste to vehicles. The Nanosuit’s speed, strength, armor, and cloaking allow creative solutions for every kind of fight, while a huge arsenal of modular weaponry provides unprecedented control over play style. In the ever-changing environment, adapt tactics and gear to dominate your enemies, in an enormous sandbox world.",
                    Price = 50,
                    UnitsInStock = 3,
                    PublisherId = 6
                },
                new Game()
                {
                    Id = 6,
                    Key = "Crysis 2",
                    Name = "Crysis 2",
                    Description = "Following the events of the groundbreaking original Crysis, aliens have returned to a world ravaged by climate disasters. As the invaders lay waste to New York and begin an assault that threatens the total annihilation of humankind, only you have the technology to lead the fightback. Playing as a super-soldier equipped with the upgraded Nanosuit 2.0, you must adapt to the battlefield using its game-changing Stealth, Armor, and Power abilities. Customize your Nanosuit and weapons in real-time and unlock incredible new powers as you battle for humanity's survival.",
                    Price = 55,
                    UnitsInStock = 2,
                    PublisherId = 6
                },
                new Game()
                {
                    Id = 7,
                    Key = "Crysis 3",
                    Name = "Crysis 3",
                    Description = "In Crysis 3, the fate of the world is once again in your hands. Returning to the fight as super-soldier Prophet, wielding a powerful auto-loading Predator Bow that fires electric, explosive, and carbon arrows, take on new and old enemies that threaten the peace you worked so hard to achieve. The search for the alien Alpha Ceph continues, but now you must also expose the truth behind the C.E.L.L. corporation, which has turned New York City into a sprawling urban rainforest sheltered by a giant nanodome. Equipped with your legendary Nanosuit, you must assess, adapt, and attack as you choose your path and fight through seven distinct districts. Decimate your opponents in a blaze of brute force using the Nanosuit's superior technology, or use stealth to achieve your goals and become humanity's silent savior. There's no wrong way to save the world.",
                    Price = 60,
                    UnitsInStock = 2,
                    PublisherId = 6
                },
                new Game()
                {
                    Id = 8,
                    Key = "MechWarrior",
                    Name = "MechWarrior 5: Mercenaries",
                    Description = "Welcome to the year 3015! It’s a hell of a time to be alive. Humanity has colonized thousands of star systems, war is everywhere, and the battlefields of the future are dominated by hulking machines known as BattleMechs. It’s dangerous work for the elite pilots of these metal monstrosities, but that’s why a power hungry MechWarrior like you came here, right? If you’re looking to blast, wreck, stomp - and profit - step inside!",
                    Price = 40,
                    UnitsInStock = 20,
                    PublisherId = 7
                },
                new Game()
                {
                    Id = 9,
                    Key = "Valhalla",
                    Name = "Assassin's Creed Valhalla",
                    Description = "In Assassin's Creed® Valhalla, become Eivor, a legendary Viking warrior on a quest for glory. Explore England's Dark Ages as you raid your enemies, grow your settlement, and build your political power.",
                    Price = 35,
                    UnitsInStock = 25,
                    PublisherId = 2
                },
                new Game()
                {
                    Id = 10,
                    Key = "FAR CRY 6",
                    Name = "FAR CRY 6",
                    Description = "Welcome to Yara, a tropical paradise frozen in time. As the dictator of Yara, Antón Castillo is intent on restoring his nation back to its former glory by any means, with his son, Diego, following in his bloody footsteps. Their oppressive rule has ignited a revolution.",
                    Price = 42,
                    UnitsInStock = 5,
                    PublisherId = 2
                },
                new Game()
                {
                    Id = 11,
                    Key = "FAR CRY 5",
                    Name = "FAR CRY 5",
                    Description = "Welcome to Hope County, Montana, land of the free and the brave, but also home to a fanatical doomsday cult—known as The Project at Eden’s Gate—that is threatening the community's freedom. Stand up to the cult’s leaders, Joseph Seed and the Heralds, as you spark the fires of resistance that will liberate the besieged community.",
                    Price = 45,
                    UnitsInStock = 7,
                    PublisherId = 2
                });
        }

        public static void GameGenreSeed(EntityTypeBuilder<GameGenre> builder)
        {
            builder.HasData(
                new GameGenre { GameId = 1, GenreId = 11 },
                new GameGenre { GameId = 2, GenreId = 9 },
                new GameGenre { GameId = 3, GenreId = 14 },
                new GameGenre { GameId = 4, GenreId = 2 },
                new GameGenre { GameId = 5, GenreId = 5 },
                new GameGenre { GameId = 5, GenreId = 17 },
                new GameGenre { GameId = 5, GenreId = 18 },
                new GameGenre { GameId = 6, GenreId = 5 },
                new GameGenre { GameId = 6, GenreId = 17 },
                new GameGenre { GameId = 6, GenreId = 18 },
                new GameGenre { GameId = 7, GenreId = 5 },
                new GameGenre { GameId = 7, GenreId = 17 },
                new GameGenre { GameId = 7, GenreId = 18 },
                new GameGenre { GameId = 8, GenreId = 5 },
                new GameGenre { GameId = 8, GenreId = 17 },
                new GameGenre { GameId = 8, GenreId = 18 },
                new GameGenre { GameId = 9, GenreId = 5 },
                new GameGenre { GameId = 9, GenreId = 17 },
                new GameGenre { GameId = 9, GenreId = 18 },
                new GameGenre { GameId = 10, GenreId = 5 },
                new GameGenre { GameId = 10, GenreId = 17 },
                new GameGenre { GameId = 10, GenreId = 18 },
                new GameGenre { GameId = 11, GenreId = 5 },
                new GameGenre { GameId = 11, GenreId = 17 },
                new GameGenre { GameId = 11, GenreId = 18 }
                );
        }
        public static void GamePlatformTypeSeed(EntityTypeBuilder<GamePlatformType> builder)
        {
            builder.HasData(
                new GamePlatformType { GameId = 1, PlatformTypeId = 1 },
                new GamePlatformType { GameId = 2, PlatformTypeId = 2 },
                new GamePlatformType { GameId = 3, PlatformTypeId = 3 },
                new GamePlatformType { GameId = 4, PlatformTypeId = 4 },
                new GamePlatformType { GameId = 6, PlatformTypeId = 2 },
                new GamePlatformType { GameId = 7, PlatformTypeId = 2 },
                new GamePlatformType { GameId = 8, PlatformTypeId = 2 },
                new GamePlatformType { GameId = 9, PlatformTypeId = 2 },
                new GamePlatformType { GameId = 10, PlatformTypeId = 2 },
                new GamePlatformType { GameId = 11, PlatformTypeId = 2 }
                );
        }
        public static void CommentSeed(EntityTypeBuilder<Comment> builder)
        {
            builder.HasData(
                new Comment { Id = 1, GameId = 1, GameKey = "GTA V", Body = "Nice Game!", Name = "Ivan242" },
                new Comment { Id = 2, GameId = 1, GameKey = "GTA V", Body = "Completely agree", Name = "Mike", ParentCommentId = 1 },
                new Comment { Id = 4, GameId = 1, GameKey = "GTA V", Body = "You are right!", Name = "Alissa", ParentCommentId = 2 },
                new Comment { Id = 5, GameId = 1, GameKey = "GTA V", Body = "It could be better", Name = "Nikolay T" },
                new Comment
                {
                    Id = 6,
                    GameId = 1,
                    GameKey = "GTA V",
                    Body = "This is a wonderful game, with a great history and narrative, i really love the gameplay and all mechanics.",
                    Name = "Jacob"
                },
                new Comment
                {
                    Id = 7,
                    GameId = 1,
                    GameKey = "GTA V",
                    Body = "Deathloop is one of the best games of the year — at least from what I’ve played so far. And I say that as someone who has previously recognized the quality of Arkane games while struggling to enjoy them. But with this game, French team Arkane Lyon used some smart tricks to amplify its strengths. And the result is a special experience that has broader appeal than some other Arkane projects.",
                    Name = "GamesBeat"
                },
                new Comment
                {
                    Id = 8,
                    GameId = 1,
                    GameKey = "GTA V",
                    Body = "Mixing stealth, weighty gunplay, supernatural powers, and play-your-way weapons and gadgets, Deathloop is a greatest-hits amalgam of Arkane style with a splash of grindhouse grittiness and a satisfying murder puzzle narrative to put it apart from anything that has come before.",
                    Name = "Shacknews"
                },
                new Comment
                {
                    Id = 9,
                    GameId = 1,
                    GameKey = "GTA V",
                    Body = "Awesome game! Very unique if you like the Hitman/Dishonored games and detective stories with sci-fi elements you will love this game.",
                    Name = "ett526"
                },
                new Comment
                {
                    Id = 10,
                    GameId = 10,
                    GameKey = "FAR CRY 6",
                    Body = "Good game IMO not sure why people are reviewing bombing bet most are salty cause it’s a Xbox studio who made the game now",
                    Name = "Metawho"
                },
                new Comment
                {
                    Id = 11,
                    GameId = 10,
                    GameKey = "FAR CRY 6",
                    Body = "Deathloop is not only one of the most conceptually ambitious and well executed games ever made, but Arkane's PlayStation swansong also has a boundless energy and ingenuity that no other game can match. Quite simply, Deathloop is an unparalleled synergy of first-person shooter design, explorative bliss and narrative complexity that we likely won't see again for a very, very long time.",
                    Name = "PlayStation Universe"
                },
                new Comment
                {
                    Id = 12,
                    GameId = 10,
                    GameKey = "FAR CRY 6",
                    Body = "I'm not a big fan of FPS games and after first day of playing Deathloop I thought I will send the game onto shelf and that's it. But...I gave it a go and i have to admit - I like it :) I like it a lot. Its not a typical FPS where you just go forward and shoot everything you find on your way. It has interesting story and I'm curious to find out what's gonna happen next.",
                    Name = "FallenAsh"
                },
                new Comment
                {
                    Id = 13,
                    GameId = 2,
                    GameKey = "FF XIV",
                    Body = "Juego bastante frenético pero para el precio qué cuesta pudo haber sido mejor...",
                    Name = "BradleyJeremy"
                },
                new Comment
                {
                    Id = 14,
                    GameId = 2,
                    GameKey = "FF XIV",
                    Body = "Just couldn't get into this game. If you like Returnal on PS5 or Dishonored, you probably will like this. I didn't like either. You either like it or hate it.",
                    Name = "GAMERTONY"
                },
                new Comment
                {
                    Id = 15,
                    GameId = 2,
                    GameKey = "FF XIV",
                    Body = "Deathloop is in a weird spot. It isn't as good as Dishonored, but I also don't expect to see another game in the series. If you want to support the studio or need a Dishonored fix, this will help...but honestly, the game isn't that fun.",
                    Name = "Gamers Heroes"
                },
                new Comment
                {
                    Id = 16,
                    GameId = 2,
                    GameKey = "FF XIV",
                    Body = "Deathloop’s mind-bending time mechanic, captivating art style, open-ended gameplay, and colorful cast of characters create a captivating shooter experience that you shouldn’t miss.",
                    Name = "PCMag"
                },
                new Comment
                {
                    Id = 17,
                    GameId = 3,
                    GameKey = "COD G",
                    Body = "Graphics are okay, music is okay, ai is terrible. Game seems like it has an interesting concept, but at the end of the day what you do is to play at 4 different maps only and kill everyone in it. And theres not much of an exploration at these maps, they are very small. Like half size of a doom map. 6 is a fair score, and the only reason it has this score is haptic support and its interesting concept. I dont know how it managed to get these 10s but believe me they are more deceiving than zeros",
                    Name = "Sugo"
                },
                new Comment
                {
                    Id = 18,
                    GameId = 3,
                    GameKey = "COD G",
                    Body = "gameplay feels outdated. graphic is outdated. sound design is outdated. an outdated game",
                    Name = "smellyc15"
                },
                new Comment
                {
                    Id = 19,
                    GameId = 3,
                    GameKey = "COD G",
                    Body = "Dishonored with a new skin. outdated graphics outdated gemeplay.POS. Corrupt IGN with 10 point",
                    Name = "Pepe17"
                },
                new Comment
                {
                    Id = 20,
                    GameId = 3,
                    GameKey = "COD G",
                    Body = "Gameplay and Mechanics feels outdated as ***. gets repetitive so fast. wait for a sale if you wanna buy",
                    Name = "fardmicheal45"
                },
                new Comment
                {
                    Id = 21,
                    GameId = 3,
                    GameKey = "COD G",
                    Body = "Too broken up into fetch quests in small map areas. Go find this code and exit...go find this note but only at this time of day. Zzzzz",
                    Name = "chriswalker_1"
                });
        }

        public static void PublisherSeed(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasData(
                new Publisher { Id = 1,
                    CompanyName = "EA",
                    Description = "Electronic Arts",
                    HomePage = "www.ea.com"
                },
                new Publisher
                {
                    Id = 2,
                    CompanyName = "Ubisoft",
                    Description = "Ubisoft Entertainment S.A.",
                    HomePage = "www.ubisoft.com"
                },
                new Publisher
                {
                    Id = 3,
                    CompanyName = "4a Games",
                    Description = "4a Games Ukraine",
                    HomePage = "www.4a-games.com"
                },
                new Publisher
                {
                    Id = 4,
                    CompanyName = "GSC Game World",
                    Description = "GSC Game World",
                    HomePage = "www.gsc-game.com"
                },
                new Publisher
                {
                    Id = 5,
                    CompanyName = "Wargaming",
                    Description = "Wargaming.net",
                    HomePage = "wargaming.com"
                },
                new Publisher
                {
                    Id = 6,
                    CompanyName = "Crytek",
                    Description = "Crytek system",
                    HomePage = "crytek.com"
                },
                new Publisher
                {
                    Id = 7,
                    CompanyName = "Piranha Games",
                    Description = "Piranha Games",
                    HomePage = "piranha.games.com"
                }
            );
        }
    }
}
