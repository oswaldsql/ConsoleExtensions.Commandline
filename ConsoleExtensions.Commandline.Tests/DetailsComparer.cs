namespace ConsoleExtensions.Commandline.Tests
{
    using System.Collections.Generic;

    using ConsoleExtensions.Commandline.Help;

    public class DetailsComparer : IEqualityComparer<ArgumentDetails>
    {
        public bool Equals(ArgumentDetails x, ArgumentDetails y)
        {
            return x.Name == y.Name 
                   && x.DisplayName == y.DisplayName 
                   && x.Description == y.Description
                   && x.Optional == y.Optional 
                   && x.Type == y.Type 
                   && ((x.DefaultValue == null && y.DefaultValue == null) || x.DefaultValue.Equals(y.DefaultValue));
        }

        public int GetHashCode(ArgumentDetails obj)
        {
            throw new System.NotImplementedException();
        }
    }
}