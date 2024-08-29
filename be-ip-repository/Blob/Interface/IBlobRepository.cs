namespace be_ip_repository.Blob.Interface
{
    public interface IBlobRepository
    {
        string GetSasUrlForPicture(string pictureName);
    }
}
