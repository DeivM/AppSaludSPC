﻿
using App.Helpers;
using System.Collections.Generic;


namespace App.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Gender Gender { get; set; }

        public string Photo { get; set; }
    }
}
