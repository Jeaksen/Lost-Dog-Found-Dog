namespace Backend.DTOs.Dogs
{
    public class GetPictureDto
    {
        public string FileName { get; set; }

        public int Id { get; set; }

        public string FileType { get; set; }
        
        public byte[] Data { get; set; }
    }
}
