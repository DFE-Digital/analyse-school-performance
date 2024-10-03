namespace ASP.Core.Results
{
    public class Done
    {
        public static readonly Done Instance = new Done();

        private Done()
        {
        }

        public override bool Equals(object? obj)
            => obj is Done;

        public override int GetHashCode()
            => HashCode.Combine(Instance);

        public override string? ToString()
            => "Done";
    }
}
