namespace E_StoreMVCTemplate.Web.DTOs.Admin
{
    public class AdminCreateDisscountDto
    {
        public string Name { get; set; } = null!;
        public decimal Rate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public IFormFile? File { get; set; }

        public List<int>? ProductIds { get; set; }



    }
}
