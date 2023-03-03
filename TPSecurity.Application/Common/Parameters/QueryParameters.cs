namespace TPSecurity.Application.Common.Parameters
{
    public abstract class QueryParameters
    {
        public const string defaultOrderOrientation = "asc";
        /// <summary>
        /// Maximum record to retrieve
        /// </summary>
        public int offSet { get; set; }
                                       
        /// <summary>
        /// Number of record to skip before taking the real record to retrieve
        /// </summary>
        public int limit { get; set; } = 0;

        /// <summary>
        /// The name of the property on which we will sort
        /// </summary>
        public string orderBy { get; set; } = null!;

        /// <summary>
        /// The orientation of the sorting
        /// </summary>
        public string orderOrientation { get; set; } = defaultOrderOrientation;

    }
}
