using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThamcoProfiles.Models;

namespace ThamcoProfiles.Data
{
    public class ProfileContext : DbContext
    {
        public ProfileContext (DbContextOptions<ProfileContext> options)
            : base(options)
        {
        }

        public DbSet<ThamcoProfiles.Models.Profile> Profile { get; set; } = default!;
    }
}