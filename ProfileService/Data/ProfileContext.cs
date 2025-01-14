using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProfileService.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ProfileService.Data
{
    public class ProfileContext : DbContext
    {
                public ProfileContext(DbContextOptions<ProfileContext> options)
                    : base(options)
                {
                
                }
                public DbSet<ProfileService.Models.Profile> Profile { get; set; } = default!;
            }
        }