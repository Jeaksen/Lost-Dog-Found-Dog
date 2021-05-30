namespace Backend.DTOs.Dogs
{
    public class GetPictureDto
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileType { get; set; }
        
        public byte[] Data { get; set; }
    }
}
