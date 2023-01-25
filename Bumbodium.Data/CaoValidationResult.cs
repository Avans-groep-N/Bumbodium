using Bumbodium.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bumbodium.Data
{
    public class CaoValidationResult : ICaoValidationResult
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CaoValidationResult(string description, string name)
        {
            Name = name;
            Description = description;
        }
    }
}
