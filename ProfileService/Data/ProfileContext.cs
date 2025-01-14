
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProfileService.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;


namespace ProfileService.Data
{
    /// <summary>
    /// The database context for managing profiles.
    /// </summary>
    public class ProfileContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileContext"/> class.
        /// </summary>
        /// <param name="options">Options for configuring the context.</param>
        public ProfileContext(DbContextOptions<ProfileContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the profiles in the database.
        /// </summary>
        public DbSet<ProfileService.Models.Profile> Profile { get; set; } = null!;
    }
}