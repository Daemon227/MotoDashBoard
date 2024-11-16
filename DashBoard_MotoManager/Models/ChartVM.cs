namespace DashBoard_MotoManager.Models
{
    public class ChartVM
    {
        public List<BrandCountVM> brandChart = new List<BrandCountVM>();
        public List<TypeCountVM> typeChart = new List<TypeCountVM>();
        public int motoCount {  get; set; } = 0;
    }
}
