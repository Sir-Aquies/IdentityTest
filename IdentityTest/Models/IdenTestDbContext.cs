﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest.Models
{
    public class IdenTestDbContext : IdentityDbContext<IdenTestUser>
    {
        public IdenTestDbContext(DbContextOptions<IdenTestDbContext> options): base(options) { }
    }
}