namespace ChefizaApi.ApiModels {
    public class ApiResponse<T> {
        public required int StatusCode { get; set; }
        public bool Success { get; set; } = true;
        public T? Data { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}