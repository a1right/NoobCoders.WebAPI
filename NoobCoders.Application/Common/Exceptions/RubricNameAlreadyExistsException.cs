using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoobCoders.Application.Common.Exceptions
{
    public class RubricNameAlreadyExistsException : Exception
    {
        public RubricNameAlreadyExistsException(string name) : base($"Rubric \"{name}\" already exists") { }
    }
}
