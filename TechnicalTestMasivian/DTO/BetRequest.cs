using System.ComponentModel.DataAnnotations;

namespace TechnicalTestMasivian.DTO
{
    public class BetRequest
    {
        [Range(-2, 36)]
        public int option { get; set; }

        [Range(1, maximum: 10000)]
        public double money { get; set; }
    }
}
