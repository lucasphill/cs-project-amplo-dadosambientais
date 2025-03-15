namespace cs_project_amplo_dadosambientais.Models
{
    public class ServiceResponseModel<T>
    {
        public T? Data { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public DateTime Timestamp { get; } = DateTime.Now;
    }
}
