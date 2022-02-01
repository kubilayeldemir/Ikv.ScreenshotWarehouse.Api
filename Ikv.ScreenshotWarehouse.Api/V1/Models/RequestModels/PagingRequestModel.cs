namespace Ikv.ScreenshotWarehouse.Api.V1.Models.RequestModels
{
    public class PagingRequestModel
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public PagingRequestModel()
        {
            ValidatePagingModel();
        }

        private void ValidatePagingModel()
        {
            switch (PageSize)
            {
                case <1:
                    PageSize = 10;
                    break;
                case > 25:
                    PageSize = 25;
                    break;
            }

            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
        }
    }
}