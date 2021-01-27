namespace BL.BLApi
{
    public static class BLFactory
    {
        public static IBL GetIBL()
        {
            return (IBL)new BLImp();
        }
    }
}
