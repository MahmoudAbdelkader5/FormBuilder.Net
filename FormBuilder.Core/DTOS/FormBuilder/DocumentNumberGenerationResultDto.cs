namespace FormBuilder.Core.DTOS.FormBuilder
{
    public class DocumentNumberGenerationResultDto
    {
        public bool Success { get; set; }
        public string? DocumentNumber { get; set; }
        public int SequenceNumber { get; set; }
        public string? PeriodKey { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
