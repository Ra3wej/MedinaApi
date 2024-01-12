namespace MedinaApi.Helpers
{
    public static class StaticDirectories
    {
        public static readonly string ProductImages = "ProductImages";
        public static readonly string AppImages = "AppImages";
        public static readonly string CarouselImages = "CarouselImages";
    }

    public static class StaticAppLanguages
    {
        public static readonly byte English = 1;
        public static readonly byte Kurdish = 2;
        public static readonly byte Arabic = 3;
    }

    public static class StaticOrderStates
    {
        public static readonly short Pending = 0;
        public static readonly short Accepted = 1;
        public static readonly short Canceled = 2;
        public static readonly short WaitingForFibPayment = 3;
        public static readonly short FibPaymentDeclined = 4;
    }

    public static class StaticCarouselType
    {
        public static readonly short Image = 0;
        public static readonly short Video = 1;
    }

    //public static class StaticOrderCashType
    //{
    //    public static readonly byte CashOnDeliver = 0;
    //    public static readonly byte FibPayment = 1;
    //    public static readonly byte FastPayPayment = 2;
    //}

    //public static class StaticFIBPaymentState
    //{
    //    public static readonly byte ByteUnpaid = 0;
    //    public static readonly byte BytePaid = 1;
    //    public static readonly byte ByteDeclined = 2;
    //    //
    //    public static readonly string TextPaid = "PAID";
    //    public static readonly string TextUnpaid = "UNPAID";
    //    public static readonly string TextDeclined = "DECLINED";

    //    ///PAID | UNPAID | DECLINED
    //}
    //public static class StaticOfferTypes
    //{
    //    public static readonly int NoOffer = 0;
    //    public static readonly int BuyOneGetOne = 1;
    //    public static readonly int Percentage = 2;
    //    public static readonly int BuyOneGetSomePercentageOff = 3;
    //}
    public static class StaticTokenRole
    {
        public static readonly string Admin = "admin";
        public static readonly string User = "user";

    }

    public static class StaticGenders
    {
        public static readonly short Male = 0;
        public static readonly short Female = 1;
        public static readonly short PreferNotToSay = 2;
    }
    public static class StaticMessageTypes
    {
        public static readonly short Text = 0;
        public static readonly short Image = 1;
        public static readonly short Document = 2;
        public static readonly short Audio = 3;
    }

}
