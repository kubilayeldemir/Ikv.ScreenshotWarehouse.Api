﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ikv.ScreenshotWarehouse.Api.Persistent.Entities
{
    public class User : DateField
    {
        [Key]
        public long Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public string Role { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}