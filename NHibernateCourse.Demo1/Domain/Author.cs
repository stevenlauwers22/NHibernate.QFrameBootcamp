using System;

namespace NHibernateCourse.Demo1.Domain
{
    public class Author
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}