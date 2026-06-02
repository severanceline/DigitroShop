namespace EndPoint.Site.Areas.Admin.Models.ViewModels.Categories
{
    public class EditRequestViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
    }
}
