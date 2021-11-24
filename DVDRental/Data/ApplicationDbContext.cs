using DVDRental.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DVDRental.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Adding 5 movies
            modelBuilder.Entity<Movie>().HasData(
                new Movie { id = 1, title = "Dune", year = 2021, price = 60, overview = "Paul Atreides, a brilliant and gifted young man born into a great destiny beyond his understanding, must travel to the most dangerous planet in the universe to ensure the future of his family and his people. As malevolent forces explode into conflict over the planet's exclusive supply of the most precious resource in existence-a commodity capable of unlocking humanity's greatest potential-only those who can conquer their fear will survive." },
                new Movie { id = 2, title = "Star Trek", year = 2009, price = 30, overview = "The fate of the galaxy rests in the hands of bitter rivals. One, James Kirk, is a delinquent, thrill-seeking Iowa farm boy. The other, Spock, a Vulcan, was raised in a logic-based society that rejects all emotion. As fiery instinct clashes with calm reason, their unlikely but powerful partnership is the only thing capable of leading their crew through unimaginable danger, boldly going where no one has gone before. The human adventure has begun again." },
                new Movie { id = 3, title = "The Godfather", year = 1972, price = 30, overview = "Spanning the years 1945 to 1955, a chronicle of the fictional Italian-American Corleone crime family. When organized crime family patriarch, Vito Corleone barely survives an attempt on his life, his youngest son, Michael steps in to take care of the would-be killers, launching a campaign of bloody revenge." },
                new Movie { id = 4, title = "Parasite", year = 2019, price = 45, overview = "All unemployed, Ki-taek's family takes peculiar interest in the wealthy and glamorous Parks for their livelihood until they get entangled in an unexpected incident." },
                new Movie { id = 5, title = "Forrest Gump", year = 1994, price = 45, overview = "A man with a low IQ has accomplished great things in his life and been present during significant historic events—in each case, far exceeding what anyone imagined he could do. But despite all he has achieved, his one true love eludes him." }
                );

            //Adding 10 copies of existing movies (2 for each)
            modelBuilder.Entity<Copy>().HasData(
                new Copy { id = 1, movieId = 1, available = true },
                new Copy { id = 2, movieId = 1, available = true },
                new Copy { id = 3, movieId = 2, available = true },
                new Copy { id = 4, movieId = 2, available = true },
                new Copy { id = 5, movieId = 3, available = true },
                new Copy { id = 6, movieId = 3, available = true },
                new Copy { id = 7, movieId = 4, available = true },
                new Copy { id = 8, movieId = 4, available = true },
                new Copy { id = 9, movieId = 5, available = true },
                new Copy { id = 10, movieId = 5, available = true }
                );
        }
    }
}
