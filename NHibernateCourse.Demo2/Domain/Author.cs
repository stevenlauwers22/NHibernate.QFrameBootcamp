using System;

namespace NHibernateCourse.Demo2.Domain
{
    public class Author
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
    }
}