namespace DalObject
{
    sealed class DalObject
    {
        #region Initial singleton
        /// <summary>
        /// crate single instance for singleton class
        /// </summary>
        static readonly DalObject instance = new DalObject();
        /// <summary>
        /// define static Ctor for singleton class
        /// </summary>
        static DalObject() { }
        DalObject() { }
        /// <summary>
        /// return instance
        /// </summary>
        public static DalObject Instance { get => instance; }
        #endregion
    }
}
